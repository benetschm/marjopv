Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditMedicalHistory
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            MedicalHistoryID_HiddenField.Value = Request.QueryString("MedicalHistoryID")
            Dim CurrentMedicalHistory_ID As Integer = MedicalHistoryID_HiddenField.Value
            Dim Delete As Boolean = False
            Delete_HiddenField.Value = Request.QueryString("Delete")
            If Delete_HiddenField.Value = "True" Then
                Delete = True
            End If
            Dim CurrentICSR_ID As Integer = ParentID(tables.ICSRs, tables.MedicalHistories, fields.ICSR_ID, CurrentMedicalHistory_ID)
            ICSRID_HiddenField.Value = CurrentICSR_ID
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                If Delete = False Then
                    Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Edit Medical History Entry " & CurrentMedicalHistory_ID
                ElseIf Delete = True Then
                    Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Delete Medical History Entry " & CurrentMedicalHistory_ID
                End If
                If Delete = False Then
                    'Check if user has edit rights and lock out if not
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicalHistories, fields.Edit) = True Then
                        AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicalHistories, fields.MedDRATerm) = True Then
                            TextBoxReadWrite(MedDRATerm_Textbox)
                            MedDRATerm_Textbox.ToolTip = "Please enter a valid MedDRA Low Level Term"
                        Else
                            TextBoxReadOnly(MedDRATerm_Textbox)
                            MedDRATerm_Textbox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.Start) = True Then
                            TextBoxReadWrite(Start_Textbox)
                            Start_Textbox.ToolTip = DateInputToolTip
                        Else
                            TextBoxReadOnly(Start_Textbox)
                            Start_Textbox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.Stop) = True Then
                            TextBoxReadWrite(Stop_Textbox)
                            Stop_Textbox.ToolTip = DateInputToolTip
                        Else
                            TextBoxReadOnly(Stop_Textbox)
                            Stop_Textbox.ToolTip = CannotEditControlText
                        End If
                    Else 'Lock out user if he/she does not have create rights
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
                ElseIf Delete = True Then
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicalHistories, fields.Delete) = True Then
                        AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                        TextBoxReadOnly(MedDRATerm_Textbox)
                        TextBoxReadOnly(Start_Textbox)
                        TextBoxReadOnly(Stop_Textbox)
                    Else 'Lock out user if he/she does not have create rights
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
                End If
                'Populate controls from database
                Dim AtEditPageLoadReadCommand As New SqlCommand("SELECT ID, MedDRATerm, CASE WHEN Start IS NULL THEN '' ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN '' ELSE Stop END AS Stop FROM MedicalHistories WHERE ID = @CurrentMedicalHistory_ID", Connection)
                AtEditPageLoadReadCommand.Parameters.AddWithValue("@CurrentMedicalHistory_ID", CurrentMedicalHistory_ID)
                Try
                    Connection.Open()
                    Dim AtEditPageLoadReader As SqlDataReader = AtEditPageLoadReadCommand.ExecuteReader()
                    While AtEditPageLoadReader.Read()
                        MedDRATerm_Textbox.Text = AtEditPageLoadReader.GetString(1)
                        AtEditPageLoad_MedDRATerm_HiddenField.Value = AtEditPageLoadReader.GetString(1)
                        Start_Textbox.Text = SqlDateDisplay(AtEditPageLoadReader.GetDateTime(2))
                        AtEditPageLoad_Start_HiddenField.Value = AtEditPageLoadReader.GetDateTime(2)
                        Stop_Textbox.Text = SqlDateDisplay(AtEditPageLoadReader.GetDateTime(3))
                        AtEditPageLoad_Stop_HiddenField.Value = AtEditPageLoadReader.GetDateTime(3)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            Else 'Redirect user to login if he/she is not logged in
                If Delete = False Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditMedicalHistory.aspx?MedicalHistoryID=" & CurrentMedicalHistory_ID)
                ElseIf Delete = True Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditMedicalHistory.aspx?MedicalHistoryID=" & CurrentMedicalHistory_ID & "&Delete=True")
                End If
            End If
        End If
    End Sub

    Protected Sub ReturnToICSROverview() Handles Cancel_Button.Click, ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub MedDRATerm_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If MedDRATerm_Textbox.Text.Trim <> String.Empty Then
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
        If Validation(Start_Textbox.Text, InputTypes.Date) = True Then
            Start_Textbox.CssClass = CssClassSuccess
            Start_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf Start_Textbox.Text = String.Empty Then
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
        If Validation(Stop_Textbox.Text, InputTypes.Date) = True Then
            Stop_Textbox.CssClass = CssClassSuccess
            Stop_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf Stop_Textbox.Text = String.Empty Then
            Stop_Textbox.CssClass = CssClassSuccess
            Stop_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Stop_Textbox.CssClass = CssClassFailure
            Stop_Textbox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click, ConfirmDeletion_Button.Click
        If Page.IsValid = True Then
            Dim CurrentMedicalHistory_ID As Integer = MedicalHistoryID_HiddenField.Value
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            'Retrieve values as present in database at edit page load and store in variables to use when checking for database update conflicts (see page load event)
            Dim AtEditPageLoad_MedDRATerm As String = AtEditPageLoad_MedDRATerm_HiddenField.Value
            Dim AtEditPageLoad_Start As DateTime = TryCType(AtEditPageLoad_Start_HiddenField.Value, InputTypes.Date)
            Dim AtEditPageLoad_Stop As DateTime = TryCType(AtEditPageLoad_Stop_HiddenField.Value, InputTypes.Date)
            'Store values as present in database when save button is clicked in variables
            Dim AtSaveButtonClickReadCommand As New SqlCommand("SELECT ID, MedDRATerm, CASE WHEN Start IS NULL THEN '' ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN '' ELSE Stop END AS Stop FROM MedicalHistories WHERE ID = @CurrentMedicalHistory_ID", Connection)
            AtSaveButtonClickReadCommand.Parameters.AddWithValue("@CurrentMedicalHistory_ID", CurrentMedicalHistory_ID)
            Dim AtSaveButtonClick_MedDRATerm As String = String.Empty
            Dim AtSaveButtonClick_Start As DateTime = Date.MinValue
            Dim AtSaveButtonClick_Stop As DateTime = Date.MinValue
            Try
                Connection.Open()
                Dim AtSaveButtonClickReader As SqlDataReader = AtSaveButtonClickReadCommand.ExecuteReader()
                While AtSaveButtonClickReader.Read()
                    AtSaveButtonClick_MedDRATerm = AtSaveButtonClickReader.GetString(1)
                    AtSaveButtonClick_Start = DateOrDateMinValue(AtSaveButtonClickReader.GetDateTime(2))
                    AtSaveButtonClick_Stop = DateOrDateMinValue(AtSaveButtonClickReader.GetDateTime(3))
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Check for discrepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked
            Dim DiscrepancyString As String = String.Empty
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_MedDRATerm, AtSaveButtonClick_MedDRATerm, "MedDRA LLT")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Start, AtSaveButtonClick_Start, "Start Date")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Stop, AtSaveButtonClick_Stop, "Stop Date")
            'If Discprepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked are found, show warning and abort update
            If DiscrepancyString <> String.Empty Then
                AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                Status_Label.Style.Add("text-align", "left")
                Status_Label.Style.Add("height", "auto")
                Status_Label.Text = DiscrepancyStringIntro & DiscrepancyString & DiscrepancyStringOutro
                Status_Label.CssClass = "form-control alert-danger"
                TextBoxReadOnly(MedDRATerm_Textbox)
                TextBoxReadOnly(Start_Textbox)
                TextBoxReadOnly(Stop_Textbox)
                Exit Sub
            End If
            'If no discrepancies were found between database contents as present when edit page was loaded and database contents as present when save button is clicked, write updates to database
            Dim UpdateCommand As New SqlCommand
            UpdateCommand.Connection = Connection
            If sender Is SaveUpdates_Button Then
                UpdateCommand.CommandText = "UPDATE MedicalHistories SET MedDRATerm = @MedDRATerm, Start = (CASE WHEN @Start = '' THEN NULL ELSE @Start END), Stop = (CASE WHEN @Stop = '' THEN NULL ELSE @Stop END) WHERE ID = @CurrentMedicalHistory_ID"
                UpdateCommand.Parameters.AddWithValue("@MedDRATerm", MedDRATerm_Textbox.Text.Trim)
                UpdateCommand.Parameters.AddWithValue("@Start", Start_Textbox.Text.Trim)
                UpdateCommand.Parameters.AddWithValue("@Stop", Stop_Textbox.Text.Trim)
                UpdateCommand.Parameters.AddWithValue("@CurrentMedicalHistory_ID", CurrentMedicalHistory_ID)
            ElseIf sender Is ConfirmDeletion_Button Then
                UpdateCommand.CommandText = "DELETE FROM MedicalHistories WHERE ID = @CurrentMedicalHistory_ID"
                UpdateCommand.Parameters.AddWithValue("@CurrentMedicalHistory_ID", CurrentMedicalHistory_ID)
            End If
            Try
                Connection.Open()
                UpdateCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Read updated values from database and store in variables
            Dim Updated_MedDRATerm As String = String.Empty
            Dim Updated_Start As DateTime = Date.MinValue
            Dim Updated_Stop As DateTime = Date.MinValue
            If sender Is SaveUpdates_Button Then
                Dim UpdatedReadCommand As New SqlCommand("SELECT ID, MedDRATerm, CASE WHEN Start IS NULL THEN 0 ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN 0 ELSE Stop END AS Stop FROM MedicalHistories WHERE ID = @CurrentMedicalHistory_ID", Connection)
                UpdatedReadCommand.Parameters.AddWithValue("@CurrentMedicalHistory_ID", CurrentMedicalHistory_ID)
                Try
                    Connection.Open()
                    Dim UpdatedReader As SqlDataReader = UpdatedReadCommand.ExecuteReader()
                    While UpdatedReader.Read()
                        Updated_MedDRATerm = UpdatedReader.GetString(1)
                        Updated_Start = DateOrDateMinValue(UpdatedReader.GetDateTime(2))
                        Updated_Stop = DateOrDateMinValue(UpdatedReader.GetDateTime(3))
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            End If
            'Compare old and new variables to generate EntryString for Change History Entry
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            If sender Is ConfirmDeletion_Button Then
                EntryString += DeleteReportIntro("Medical History Entry", CurrentMedicalHistory_ID)
            End If
            If Updated_MedDRATerm <> AtSaveButtonClick_MedDRATerm Then
                EntryString += HistoryEnrtyPlainValue("Medical Hsitory Entry", CurrentMedicalHistory_ID, "MedDRA LLT", AtSaveButtonClick_MedDRATerm, Updated_MedDRATerm)
            End If
            If Updated_Start <> AtSaveButtonClick_Start Then
                EntryString += HistoryEnrtyPlainValue("Medical History Entry", CurrentMedicalHistory_ID, "Start Date", AtSaveButtonClick_Start, Updated_Start)
            End If
            If Updated_Stop <> AtSaveButtonClick_Stop Then
                EntryString += HistoryEnrtyPlainValue("Medical History Entry", CurrentMedicalHistory_ID, "Stop Date", AtSaveButtonClick_Stop, Updated_Stop)
            End If
            EntryString += HistoryDatabasebUpdateOutro
            'Generate History Entry if any data was changed in the database
            If EntryString <> HistoryDatabasebUpdateIntro & HistoryDatabasebUpdateOutro Then
                Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO ICSRHistories (ICSR_ID, User_ID, Timepoint, Entry) VALUES (@ICSR_ID, @User_ID, @Timepoint, CASE WHEN @Entry = '' THEN NULL ELSE @Entry END)", Connection)
                InsertHistoryEntryCommand.Parameters.AddWithValue("@ICSR_ID", CurrentICSR_ID)
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
            End If
            'Format Controls
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
            If sender Is SaveUpdates_Button Then
                TextBoxReadOnly(MedDRATerm_Textbox)
                TextBoxReadOnly(Start_Textbox)
                TextBoxReadOnly(Stop_Textbox)
            ElseIf sender Is ConfirmDeletion_Button Then
                MedDRATerm_Row.Visible = False
                Start_Row.Visible = False
                Stop_Row.Visible = False
            End If
        End If
    End Sub

End Class
