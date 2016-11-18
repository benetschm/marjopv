Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditAE
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            AEID_HiddenField.Value = Request.QueryString("AEID")
            Delete_HiddenField.Value = Request.QueryString("Delete")
            Dim CurrentAE_ID As Integer = AEID_HiddenField.Value
            Dim Delete As Boolean = False
            If Delete_HiddenField.Value = "True" Then
                Delete = True
            End If
            Dim CurrentICSR_ID As Integer = ParentID(tables.ICSRs, tables.AEs, fields.ICSR_ID, CurrentAE_ID)
            ICSRID_HiddenField.Value = CurrentICSR_ID
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                If Delete = False Then
                    Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Edit Event " & CurrentAE_ID
                ElseIf Delete = True Then
                    Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Delete Event " & CurrentAE_ID
                End If
                'Format Controls according to user edit rights
                If Delete = False Then
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.Edit) = True Then
                        AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.MedDRATerm) = True Then
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
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.Outcome_ID) = True Then
                            DropDownListEnabled(Outcomes_DropDownList)
                        Else
                            DropDownListDisabled(Outcomes_DropDownList)
                            Outcomes_DropDownList.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.DechallengeResult_ID) = True Then
                            DropDownListEnabled(DechallengeResults_DropDownList)
                        Else
                            DropDownListDisabled(DechallengeResults_DropDownList)
                            DechallengeResults_DropDownList.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.RechallengeResult_ID) = True Then
                            DropDownListEnabled(RechallengeResults_DropDownList)
                        Else
                            DropDownListDisabled(RechallengeResults_DropDownList)
                            RechallengeResults_DropDownList.ToolTip = CannotEditControlText
                        End If
                    Else 'Lock out user if he/she does not have edit rights
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
                ElseIf Delete = True Then
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.Delete) = True Then
                        AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                        TextBoxReadOnly(MedDRATerm_Textbox)
                        TextBoxReadOnly(Start_Textbox)
                        TextBoxReadOnly(Stop_Textbox)
                        DropDownListDisabled(Outcomes_DropDownList)
                        DropDownListDisabled(DechallengeResults_DropDownList)
                        DropDownListDisabled(RechallengeResults_DropDownList)
                        'Check if there is a dataset which is dependent on the current dataset
                        Dim DependencyCheckReadCommand As New SqlCommand("SELECT ID FROM Relations WHERE AE_ID = @CurrentAE_ID", Connection)
                        DependencyCheckReadCommand.Parameters.AddWithValue("@CurrentAE_ID", CurrentAE_ID)
                        Dim DependencyFound As Boolean = False
                        Try
                            Connection.Open()
                            Dim DependencyCheckReader As SqlDataReader = DependencyCheckReadCommand.ExecuteReader()
                            While DependencyCheckReader.Read()
                                If DependencyCheckReader.GetInt32(0) <> Nothing Then
                                    DependencyFound = True
                                End If
                            End While
                        Catch ex As Exception
                            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                            Exit Sub
                        Finally
                            Connection.Close()
                        End Try
                        If DependencyFound = True Then
                            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                            Status_Label.Style.Add("text-align", "left")
                            Status_Label.Style.Add("height", "auto")
                            Status_Label.Text = DependencyFoundMessage
                            Status_Label.CssClass = CssClassFailure
                        End If
                    Else 'Lock out user if he/she does not have create rights
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
                End If
                'Populate Outcomes_DropDownList
                Outcomes_DropDownList.DataSource = CreateDropDownListDatatable(tables.Outcomes)
                Outcomes_DropDownList.DataValueField = "ID"
                Outcomes_DropDownList.DataTextField = "Name"
                Outcomes_DropDownList.DataBind()
                'Populate DechallengeResults_DropDownList
                DechallengeResults_DropDownList.DataSource = CreateDropDownListDatatable(tables.DechallengeResults)
                DechallengeResults_DropDownList.DataValueField = "ID"
                DechallengeResults_DropDownList.DataTextField = "Name"
                DechallengeResults_DropDownList.DataBind()
                'Populate RechallengeResults_DropDownList
                RechallengeResults_DropDownList.DataSource = CreateDropDownListDatatable(tables.RechallengeResults)
                RechallengeResults_DropDownList.DataValueField = "ID"
                RechallengeResults_DropDownList.DataTextField = "Name"
                RechallengeResults_DropDownList.DataBind()
                'Populate controls from database
                Dim AtEditPageLoadReadCommand As New SqlCommand("SELECT ID, CASE WHEN MedDRATerm IS NULL THEN '' ELSE MedDRATerm END AS MedDRATerm, CASE WHEN Start IS NULL THEN '' ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN '' ELSE Stop END AS Stop, CASE WHEN Outcome_ID IS NULL THEN 0 ELSE Outcome_ID END AS Outcome_ID, CASE WHEN DechallengeResult_ID IS NULL THEN 0 ELSE DechallengeResult_ID END AS DechallengeResult_ID, CASE WHEN RechallengeResult_ID IS NULL THEN 0 ELSE RechallengeResult_ID END AS RechallengeResult_ID FROM AEs WHERE ID = @CurrentAE_ID", Connection)
                AtEditPageLoadReadCommand.Parameters.AddWithValue("@CurrentAE_ID", CurrentAE_ID)
                Try
                    Connection.Open()
                    Dim AtEditPageLoadReader As SqlDataReader = AtEditPageLoadReadCommand.ExecuteReader()
                    While AtEditPageLoadReader.Read()
                        MedDRATerm_Textbox.Text = AtEditPageLoadReader.GetString(1)
                        AtEditPageLoad_MedDRATerm_HiddenField.Value = AtEditPageLoadReader.GetString(1)
                        Start_Textbox.Text = SqlDateDisplay(AtEditPageLoadReader.GetDateTime(2))
                        AtEditPageLoad_Start_HiddenField.Value = SqlDateDisplay(AtEditPageLoadReader.GetDateTime(2))
                        Stop_Textbox.Text = SqlDateDisplay(AtEditPageLoadReader.GetDateTime(3))
                        AtEditPageLoad_Stop_HiddenField.Value = SqlDateDisplay(AtEditPageLoadReader.GetDateTime(3))
                        Outcomes_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(4)
                        AtEditPageLoad_Outcome_HiddenField.Value = AtEditPageLoadReader.GetInt32(4)
                        DechallengeResults_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(5)
                        AtEditPageLoad_DechallengeResult_HiddenField.Value = AtEditPageLoadReader.GetInt32(5)
                        RechallengeResults_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(6)
                        AtEditPageLoad_RechallengeResult_HiddenField.Value = AtEditPageLoadReader.GetInt32(6)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            Else 'Redirect user to login if he/she is not logged in
                If Delete = False Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditAE.aspx?AEID=" & CurrentAE_ID)
                ElseIf Delete = True Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditAE.aspx?AEID=" & CurrentAE_ID & "&Delete=True")
                End If
            End If
        End If
    End Sub

    Protected Sub ReturnToICSROverview() Handles Cancel_Button.Click, ReturnToICSROverview_Button.Click
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

    Protected Sub Outcomes_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(Outcomes_DropDownList) = True Then
            Outcomes_DropDownList.CssClass = CssClassSuccess
            Outcomes_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            Outcomes_DropDownList.CssClass = CssClassFailure
            Outcomes_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub DechallengeResults_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(DechallengeResults_DropDownList) = True Then
            DechallengeResults_DropDownList.CssClass = CssClassSuccess
            DechallengeResults_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            DechallengeResults_DropDownList.CssClass = CssClassFailure
            DechallengeResults_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub RechallengeResults_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(RechallengeResults_DropDownList) = True Then
            RechallengeResults_DropDownList.CssClass = CssClassSuccess
            RechallengeResults_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            RechallengeResults_DropDownList.CssClass = CssClassFailure
            RechallengeResults_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = True
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click, ConfirmDeletion_Button.Click
        If Page.IsValid = True Then
            Dim CurrentAE_ID As Integer = AEID_HiddenField.Value
            Dim CurrentICSR_ID As Integer = ParentID(tables.ICSRs, tables.AEs, fields.ICSR_ID, CurrentAE_ID)
            'Retrieve values as present in database at edit page load and store in variables to use when checking for database update conflicts (see page load event)
            Dim AtEditPageLoad_MedDRATerm As String = AtEditPageLoad_MedDRATerm_HiddenField.Value
            Dim AtEditPageLoad_Start As DateTime = TryCType(AtEditPageLoad_Start_HiddenField.Value, InputTypes.Date)
            Dim AtEditPageLoad_Stop As DateTime = TryCType(AtEditPageLoad_Stop_HiddenField.Value, InputTypes.Date)
            Dim AtEditPageLoad_Outcome_ID As Integer = AtEditPageLoad_Outcome_HiddenField.Value
            Dim AtEditPageLoad_DechallengeResult_ID As Integer = AtEditPageLoad_DechallengeResult_HiddenField.Value
            Dim AtEditPageLoad_RechallengeResult_ID As Integer = AtEditPageLoad_RechallengeResult_HiddenField.Value
            'Store values as present in database when save button is clicked in variables
            Dim AtSaveButtonClickReadCommand As New SqlCommand("SELECT ID, CASE WHEN MedDRATerm IS NULL THEN '' ELSE MedDRATerm END AS MedDRATerm, CASE WHEN Start IS NULL THEN '' ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN '' ELSE Stop END AS Stop, CASE WHEN Outcome_ID IS NULL THEN 0 ELSE Outcome_ID END AS Outcome_ID, CASE WHEN DechallengeResult_ID IS NULL THEN 0 ELSE DechallengeResult_ID END AS DechallengeResult_ID, CASE WHEN RechallengeResult_ID IS NULL THEN 0 ELSE RechallengeResult_ID END AS RechallengeResult_ID FROM AEs WHERE ID = @CurrentAE_ID", Connection)
            AtSaveButtonClickReadCommand.Parameters.AddWithValue("@CurrentAE_ID", CurrentAE_ID)
            Dim AtSaveButtonClick_MedDRATerm As String = String.Empty
            Dim AtSaveButtonClick_Start As DateTime = Date.MinValue
            Dim AtSaveButtonClick_Stop As DateTime = Date.MinValue
            Dim AtSaveButtonClick_Outcome_ID As Integer = Nothing
            Dim AtSaveButtonClick_DechallengeResult_ID As Integer = Nothing
            Dim AtSaveButtonClick_RechallengeResult_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim AtSaveButtonClickReader As SqlDataReader = AtSaveButtonClickReadCommand.ExecuteReader()
                While AtSaveButtonClickReader.Read()
                    AtSaveButtonClick_MedDRATerm = AtSaveButtonClickReader.GetString(1)
                    AtSaveButtonClick_Start = DateOrDateMinValue(AtSaveButtonClickReader.GetDateTime(2))
                    AtSaveButtonClick_Stop = DateOrDateMinValue(AtSaveButtonClickReader.GetDateTime(3))
                    AtSaveButtonClick_Outcome_ID = AtSaveButtonClickReader.GetInt32(4)
                    AtSaveButtonClick_DechallengeResult_ID = AtSaveButtonClickReader.GetInt32(5)
                    AtSaveButtonClick_RechallengeResult_ID = AtSaveButtonClickReader.GetInt32(6)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Check for discrepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked
            'Dim DiscrepancyString As String = String.Empty
            'DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_MedDRATerm, AtSaveButtonClick_MedDRATerm, "MedDRA LLT")
            'DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Start, AtSaveButtonClick_Start, "Start Date")
            'DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Stop, AtSaveButtonClick_Stop, "Stop Date")
            'DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_DechallengeResult_ID, AtSaveButtonClick_DechallengeResult_ID, "Dechallenge Result")
            'DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_RechallengeResult_ID, AtSaveButtonClick_RechallengeResult_ID, "Rechallenge Result")
            'If Discprepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked are found, show warning and abort update
            'If DiscrepancyString <> String.Empty Then
            '    AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
            '    Status_Label.Style.Add("text-align", "left")
            '    Status_Label.Style.Add("height", "auto")
            '    Status_Label.Text = DiscrepancyStringIntro & DiscrepancyString & DiscrepancyStringOutro
            '    Status_Label.CssClass = CssClassFailure
            '    TextBoxReadOnly(MedDRATerm_Textbox)
            '    TextBoxReadOnly(Start_Textbox)
            '    TextBoxReadOnly(Stop_Textbox)
            '    DropDownListDisabled(Outcomes_DropDownList)
            '    DropDownListDisabled(DechallengeResults_DropDownList)
            '    DropDownListDisabled(RechallengeResults_DropDownList)
            '    Exit Sub
            'End If
            'If no discrepancies were found between database contents as present when edit page was loaded and database contents as present when save button is clicked, write updates to database
            Dim UpdateCommand As New SqlCommand
            UpdateCommand.Connection = Connection
            If sender Is SaveUpdates_Button Then
                UpdateCommand.CommandText = "UPDATE AEs SET MedDRATerm = (CASE WHEN @MedDRATerm = '' THEN NULL ELSE @MedDRATerm END), Start = (CASE WHEN @Start = '' THEN NULL ELSE @Start END), Stop = (CASE WHEN @Stop = '' THEN NULL ELSE @Stop END), Outcome_ID = (CASE WHEN @Outcome_ID = 0 THEN NULL ELSE @Outcome_ID END), DechallengeResult_ID = (CASE WHEN @DechallengeResult_ID = 0 THEN NULL ELSE @DechallengeResult_ID END), RechallengeResult_ID = (CASE WHEN @RechallengeResult_ID = 0 THEN NULL ELSE @RechallengeResult_ID END) WHERE ID = @CurrentAE_ID"
                UpdateCommand.Parameters.AddWithValue("@MedDRATerm", MedDRATerm_Textbox.Text.Trim)
                UpdateCommand.Parameters.AddWithValue("@Start", DateStringOrEmpty(Start_Textbox.Text.Trim))
                UpdateCommand.Parameters.AddWithValue("@Stop", DateStringOrEmpty(Stop_Textbox.Text.Trim))
                UpdateCommand.Parameters.AddWithValue("@Outcome_ID", Outcomes_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@DechallengeResult_ID", DechallengeResults_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@RechallengeResult_ID", RechallengeResults_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@CurrentAE_ID", CurrentAE_ID)
            ElseIf sender Is ConfirmDeletion_Button Then
                UpdateCommand.CommandText = "DELETE FROM AEs WHERE ID = @CurrentAE_ID"
                UpdateCommand.Parameters.AddWithValue("@CurrentAE_ID", CurrentAE_ID)
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
            Dim Updated_Outcome_ID As Integer = Nothing
            Dim Updated_DechallengeResult_ID As Integer = Nothing
            Dim Updated_RechallengeResult_ID As Integer = Nothing
            If sender Is SaveUpdates_Button Then
                Dim UpdatedReadCommand As New SqlCommand("SELECT ID, CASE WHEN MedDRATerm IS NULL THEN '' ELSE MedDRATerm END AS MedDRATerm, CASE WHEN Start IS NULL THEN '' ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN '' ELSE Stop END AS Stop, CASE WHEN Outcome_ID IS NULL THEN 0 ELSE Outcome_ID END AS Outcome_ID, CASE WHEN DechallengeResult_ID IS NULL THEN 0 ELSE DechallengeResult_ID END AS DechallengeResult_ID, CASE WHEN RechallengeResult_ID IS NULL THEN 0 ELSE RechallengeResult_ID END AS RechallengeResult_ID FROM AEs WHERE ID = @CurrentAE_ID", Connection)
                UpdatedReadCommand.Parameters.AddWithValue("@CurrentAE_ID", CurrentAE_ID)
                Try
                    Connection.Open()
                    Dim UpdatedReader As SqlDataReader = UpdatedReadCommand.ExecuteReader()
                    While UpdatedReader.Read()
                        Updated_MedDRATerm = UpdatedReader.GetString(1)
                        Updated_Start = DateOrDateMinValue(UpdatedReader.GetDateTime(2))
                        Updated_Stop = DateOrDateMinValue(UpdatedReader.GetDateTime(3))
                        Updated_Outcome_ID = UpdatedReader.GetInt32(4)
                        Updated_DechallengeResult_ID = UpdatedReader.GetInt32(5)
                        Updated_RechallengeResult_ID = UpdatedReader.GetInt32(6)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            End If
            'Compare old and new variables to generate EntryString for Change History Entry
            'Dim EntryString As String = String.Empty
            'EntryString = HistoryDatabasebUpdateIntro
            'If sender Is ConfirmDeletion_Button Then
            '    EntryString += DeleteReportIntro("Event", CurrentAE_ID)
            'End If
            'EntryString += HistoryEntryPlainValue("Event", CurrentAE_ID, "MedDRA LLT", AtSaveButtonClick_MedDRATerm, Updated_MedDRATerm)
            'EntryString += HistoryEntryPlainValue("Event", CurrentAE_ID, "Start Date", AtSaveButtonClick_Start, Updated_Start)
            'EntryString += HistoryEntryPlainValue("Event", CurrentAE_ID, "Stop Date", AtSaveButtonClick_Stop, Updated_Stop)
            'EntryString += HistoryEntryReferencedValue("Event", CurrentAE_ID, "Outcome", tables.Outcomes, fields.Name, AtSaveButtonClick_Outcome_ID, Updated_Outcome_ID)
            'EntryString += HistoryEntryReferencedValue("Event", CurrentAE_ID, "Dechallenge Result", tables.DechallengeResults, fields.Name, AtSaveButtonClick_DechallengeResult_ID, Updated_DechallengeResult_ID)
            'EntryString += HistoryEntryReferencedValue("Event", CurrentAE_ID, "Rechallenge Result", tables.RechallengeResults, fields.Name, AtSaveButtonClick_RechallengeResult_ID, Updated_RechallengeResult_ID)
            'EntryString += HistoryDatabasebUpdateOutro
            ''Generate History Entry if any data was changed in the database
            'If EntryString <> HistoryDatabasebUpdateIntro & HistoryDatabasebUpdateOutro Then
            '    Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO ICSRHistories (ICSR_ID, User_ID, Timepoint, Entry) VALUES (@ICSR_ID, @User_ID, @Timepoint, CASE WHEN @Entry = '' THEN NULL ELSE @Entry END)", Connection)
            '    InsertHistoryEntryCommand.Parameters.AddWithValue("@ICSR_ID", CurrentICSR_ID)
            '    InsertHistoryEntryCommand.Parameters.AddWithValue("@User_ID", LoggedIn_User_ID)
            '    InsertHistoryEntryCommand.Parameters.AddWithValue("@Timepoint", Now())
            '    InsertHistoryEntryCommand.Parameters.AddWithValue("@Entry", EntryString)
            '    Try
            '        Connection.Open()
            '        InsertHistoryEntryCommand.ExecuteNonQuery()
            '    Catch ex As Exception
            '        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            '        Exit Sub
            '    Finally
            '        Connection.Close()
            '    End Try
            'End If
            'Format Controls
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
            If sender Is SaveUpdates_Button Then
                TextBoxReadOnly(MedDRATerm_Textbox)
                TextBoxReadOnly(Start_Textbox)
                TextBoxReadOnly(Stop_Textbox)
                DropDownListDisabled(Outcomes_DropDownList)
                DropDownListDisabled(DechallengeResults_DropDownList)
                DropDownListDisabled(RechallengeResults_DropDownList)
            ElseIf sender Is ConfirmDeletion_Button Then
                MedDRATerm_Row.Visible = False
                Start_Row.Visible = False
                Stop_Row.Visible = False
                Outcome_Row.Visible = False
                DechallengeResult_Row.Visible = False
                RechallengeResult_Row.Visible = False
            End If
        End If
    End Sub

End Class
