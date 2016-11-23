Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_CreateMedicalHistory
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Add Medical History Entry"
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                'Check if user has create rights and lock out if not
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicalHistories, fields.Create) = True Then
                    'Format Controls depending on user edit rights for each control
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicalHistories, fields.MedDRATerm) Then
                        TextBoxReadWrite(MedDRATerm_Textbox)
                    Else
                        TextBoxReadOnly(MedDRATerm_Textbox)
                        MedDRATerm_Textbox.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicalHistories, fields.Start) = True Then
                        TextBoxReadWrite(Start_Textbox)
                    Else
                        TextBoxReadOnly(Start_Textbox)
                        Start_Textbox.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicalHistories, fields.Stop) = True Then
                        TextBoxReadWrite(Stop_Textbox)
                    Else
                        TextBoxReadOnly(Stop_Textbox)
                        Stop_Textbox.ToolTip = CannotEditControlText
                    End If
                Else 'Lock out user if he/she does not have create rights
                    Title_Label.Text = Lockout_Text
                    ButtonGroup_Div.Visible = False
                    Main_Table.Visible = False
                End If
            Else 'Redirect user to login if he/she is not logged in
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/CreateMedicalHistory.aspx?ICSRID=" & CurrentICSR_ID)
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click() Handles Cancel_Button.Click, ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub MedDRATerm_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If TextValidator(MedDRATerm_Textbox) = True Then
            MedDRATerm_Textbox.CssClass = CssClassSuccess
            MedDRATerm_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            MedDRATerm_Textbox.CssClass = CssClassFailure
            MedDRATerm_Textbox.ToolTip = MedDRATermValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Start_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If DateOrEmptyValidator(Start_Textbox) = True Then
            Start_Textbox.CssClass = CssClassSuccess
            Start_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Start_Textbox.CssClass = CssClassFailure
            Start_Textbox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Stop_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If DateOrEmptyValidator(Stop_Textbox) = True Then
            Stop_Textbox.CssClass = CssClassSuccess
            Stop_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Stop_Textbox.CssClass = CssClassFailure
            Stop_Textbox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            'Write new dataset to database
            Dim InsertCommand As New SqlCommand("INSERT INTO MedicalHistories (ICSR_ID, MedDRATerm, Start, Stop) VALUES(@ICSR_ID, @MedDRATerm, CASE WHEN @Start = '' THEN NULL ELSE @Start END, CASE WHEN @Stop = '' THEN NULL ELSE @Stop END)", Connection)
            InsertCommand.Parameters.AddWithValue("@ICSR_ID", CurrentICSR_ID)
            InsertCommand.Parameters.AddWithValue("@MedDRATerm", MedDRATerm_Textbox.Text.Trim)
            InsertCommand.Parameters.AddWithValue("@Start", DateStringOrEmpty(Start_Textbox.Text.Trim))
            InsertCommand.Parameters.AddWithValue("@Stop", DateStringOrEmpty(Stop_Textbox.Text.Trim))
            Try
                Connection.Open()
                InsertCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Add change history entry with database updates
            Dim NewReadCommand As New SqlCommand("SELECT TOP 1 ID, MedDRATerm, CASE WHEN Start IS NULL THEN 0 ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN 0 ELSE Stop END AS Stop FROM MedicalHistories WHERE ICSR_ID = @CurrentICSR_ID ORDER BY ID DESC", Connection)
            NewReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            Dim NewMedicalHistory_ID As Integer = Nothing
            Dim NewMedicalHistory_MedDRATerm As String = String.Empty
            Dim NewMedicalHistory_Start As DateTime = Date.MinValue
            Dim NewMedicalHistory_Stop As DateTime = Date.MinValue
            Try
                Connection.Open()
                Dim NewReader As SqlDataReader = NewReadCommand.ExecuteReader()
                While NewReader.Read()
                    NewMedicalHistory_ID = NewReader.GetInt32(0)
                    NewMedicalHistory_MedDRATerm = NewReader.GetString(1)
                    NewMedicalHistory_Start = DateOrDateMinValue(NewReader.GetDateTime(2))
                    NewMedicalHistory_Stop = DateOrDateMinValue(NewReader.GetDateTime(3))
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Dim EntryString As String = String.Empty
            'EntryString = HistoryDatabasebUpdateIntro
            'EntryString += NewReportIntro("Medical History Entry", NewMedicalHistory_ID)
            'EntryString += HistoryEntryPlainValue("Medical History Entry", NewMedicalHistory_ID, "MedDRA LLT", String.Empty, NewMedicalHistory_MedDRATerm)
            'EntryString += HistoryEntryPlainValue("Medical History Entry", NewMedicalHistory_ID, "Start Date", Date.MinValue, NewMedicalHistory_Start)
            'EntryString += HistoryEntryPlainValue("Medical History Entry", NewMedicalHistory_ID, "Stop Date", Date.MinValue, NewMedicalHistory_Stop)
            'EntryString += HistoryDatabasebUpdateOutro
            'Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO ICSRHistories(ICSR_ID, User_ID, Timepoint, Entry) VALUES (@ICSR_ID, @User_ID, @Timepoint, @Entry)", Connection)
            'InsertHistoryEntryCommand.Parameters.AddWithValue("@ICSR_ID", CurrentICSR_ID)
            'InsertHistoryEntryCommand.Parameters.AddWithValue("@User_ID", LoggedIn_User_ID)
            'InsertHistoryEntryCommand.Parameters.AddWithValue("@Timepoint", Now())
            'InsertHistoryEntryCommand.Parameters.AddWithValue("@Entry", EntryString)
            'Try
            '    Connection.Open()
            '    InsertHistoryEntryCommand.ExecuteNonQuery()
            'Catch ex As Exception
            '    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            '    Exit Sub
            'Finally
            '    Connection.Close()
            'End Try
            'Format Page Controls
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
            TextBoxReadOnly(MedDRATerm_Textbox)
            TextBoxReadOnly(Start_Textbox)
            TextBoxReadOnly(Stop_Textbox)
        End If
    End Sub

End Class
