Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_CreateMedicationHistoryEntry
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            MedicationID_HiddenField.Value = Request.QueryString("MedicationID")
            Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
            Title_Label.Text = "Medication " & CurrentMedication_ID & ": Add History Entry"
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                'Check if user has create rights and lock out if not
                If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationHistories, fields.Create) = True Then
                    'Format Controls depending on user edit rights for each control
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToMedicationOverview_Button)
                    If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationHistories, fields.Entry) = True Then
                        TextBoxReadWrite(MedicationHistoryEntry_Textbox)
                    Else
                        TextBoxReadOnly(MedicationHistoryEntry_Textbox)
                        MedicationHistoryEntry_Textbox.ToolTip = CannotEditControlText
                    End If
                Else 'Lock out user if he/she does not have create rights
                    Title_Label.Text = Lockout_Text
                    ButtonGroup_Div.Visible = False
                    Main_Table.Visible = False
                End If
            Else 'Redirect user to login if he/she is not logged in
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/CreateMedicationHistoryEntry.aspx?MedicationID=" & CurrentMedication_ID)
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click() Handles Cancel_Button.Click, ReturnToMedicationOverview_Button.Click
        Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
        Response.Redirect("~/Application/MedicationOverview.aspx?MedicationID=" & CurrentMedication_ID)
    End Sub

    Protected Sub MedicationHistoryEntry_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If TextValidator(MedicationHistoryEntry_Textbox) = True Then
            MedicationHistoryEntry_Textbox.CssClass = CssClassSuccess
            MedicationHistoryEntry_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            MedicationHistoryEntry_Textbox.CssClass = CssClassFailure
            MedicationHistoryEntry_Textbox.ToolTip = TextValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
            'Add change history entry
            Dim EntryString As String = HistoryManualEntryIntro
            EntryString += MedicationHistoryEntry_Textbox.Text.Trim
            EntryString += HistoryManualEntryOutro
            Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO MedicationHistories(Medication_ID, User_ID, Timepoint, Entry) VALUES (@Medication_ID, @User_ID, @Timepoint, @Entry)", Connection)
            InsertHistoryEntryCommand.Parameters.AddWithValue("@Medication_ID", CurrentMedication_ID)
            InsertHistoryEntryCommand.Parameters.AddWithValue("@User_ID", LoggedIn_User_ID)
            InsertHistoryEntryCommand.Parameters.AddWithValue("@Timepoint", Now())
            InsertHistoryEntryCommand.Parameters.AddWithValue("@Entry", EntryString)
            Try
                Connection.Open()
                InsertHistoryEntryCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Format Page Controls
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToMedicationOverview_Button)
            TextBoxReadOnly(MedicationHistoryEntry_Textbox)
        End If
    End Sub

End Class
