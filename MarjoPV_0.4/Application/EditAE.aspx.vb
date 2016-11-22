Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditAE
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Carry out Sub only on initial page load
        If Page.IsPostBack = True Then Exit Sub

        'Redirect to login if user is not logged in
        If Login_Status = False Then
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditAE.aspx?" & Convert.ToString(Request.QueryString))
        End If

        'Determine call reason, store reason in hidden field and redirect to parent page if query string is invalid
        Dim CallReason As CallReasons = GetCallReason(Me, "AEID", CallReason_HiddenField)

        'Store query string values in hidden fields and variables
        Dim CurrentICSR_ID As Integer = Nothing
        Dim CurrentAE_ID As Integer = Nothing
        Dim Delete As Boolean = False
        If CallReason = CallReasons.Create Then
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            AEID_HiddenField.Value = Request.QueryString("AEID")
            CurrentAE_ID = AEID_HiddenField.Value
            ICSRID_HiddenField.Value = ParentID(tables.ICSRs, tables.AEs, fields.ICSR_ID, CurrentAE_ID)
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Delete Then
            Delete_HiddenField.Value = Request.QueryString("Delete")
        End If

        'Populate Title_Label
        If CallReason = CallReasons.Create Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Add Event"
        ElseIf CallReason = CallReasons.Update Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Edit Event " & CurrentAE_ID
        ElseIf CallReason = CallReasons.Delete Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Delete Event " & CurrentAE_ID
        End If

        'Lock out if user does not have adequate edit rights
        LockoutCheck(CallReason, CurrentICSR_ID, tables.AEs, Title_Label, ButtonGroup_Div, Main_Table)

        'Format controls based on edit rights
        If CallReason = CallReasons.Create Or CallReason = CallReasons.Update Then
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
        ElseIf CallReason = CallReasons.Delete Then
            AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
            TextBoxReadOnly(MedDRATerm_Textbox)
            TextBoxReadOnly(Start_Textbox)
            TextBoxReadOnly(Stop_Textbox)
            DropDownListDisabled(Outcomes_DropDownList)
            DropDownListDisabled(DechallengeResults_DropDownList)
            DropDownListDisabled(RechallengeResults_DropDownList)
        End If

        'Populate DropDownLists
        PopulateDropDownList(Outcomes_DropDownList, tables.Outcomes)
        PopulateDropDownList(DechallengeResults_DropDownList, tables.DechallengeResults)
        PopulateDropDownList(RechallengeResults_DropDownList, tables.RechallengeResults)

        'Check if page is called to delete and there is a dataset in 'Relations' which is dependent on the current dataset
        If CallReason = CallReasons.Delete Then
            RelationDependencyCheck(fields.AE_ID, CurrentAE_ID, Status_Label, SaveUpdates_Button, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
        End If

        'Populate data fields
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            Dim CurrentValuesDataTable As DataTable = SqlRead(Me, "SELECT ID, CASE WHEN MedDRATerm IS NULL THEN '' ELSE MedDRATerm END AS MedDRATerm, CASE WHEN Start IS NULL THEN '' ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN '' ELSE Stop END AS Stop, CASE WHEN Outcome_ID IS NULL THEN 0 ELSE Outcome_ID END AS Outcome_ID, CASE WHEN DechallengeResult_ID IS NULL THEN 0 ELSE DechallengeResult_ID END AS DechallengeResult_ID, CASE WHEN RechallengeResult_ID IS NULL THEN 0 ELSE RechallengeResult_ID END AS RechallengeResult_ID FROM AEs WHERE ID = " & CurrentAE_ID)
            MedDRATerm_Textbox.Text = CurrentValuesDataTable.Rows(0)(1)
            AtEditPageLoad_MedDRATerm_HiddenField.Value = CurrentValuesDataTable.Rows(0)(1)
            Start_Textbox.Text = SqlDateDisplay(CurrentValuesDataTable.Rows(0)(2))
            AtEditPageLoad_Start_HiddenField.Value = CurrentValuesDataTable.Rows(0)(2)
            Stop_Textbox.Text = SqlDateDisplay(CurrentValuesDataTable.Rows(0)(3))
            AtEditPageLoad_Stop_HiddenField.Value = CurrentValuesDataTable.Rows(0)(3)
            Outcomes_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(4)
            AtEditPageLoad_Outcome_HiddenField.Value = CurrentValuesDataTable.Rows(0)(4)
            DechallengeResults_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(5)
            AtEditPageLoad_DechallengeResult_HiddenField.Value = CurrentValuesDataTable.Rows(0)(5)
            RechallengeResults_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(6)
            AtEditPageLoad_RechallengeResult_HiddenField.Value = CurrentValuesDataTable.Rows(0)(6)
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

        'Carry out Sub only if input Is valid
        If Page.IsValid = False Then Exit Sub

        'Redirect to login if user is not logged in
        If Login_Status = False Then
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditAE.aspx?" & Convert.ToString(Request.QueryString))
        End If

        'Store values from hidden fields in variables
        Dim CallReason As CallReasons
        Dim CurrentICSR_ID As Integer = Nothing
        Dim CurrentAE_ID As Integer = Nothing
        Dim Delete As Boolean = False
        CallReason = CallReason_HiddenField.Value
        If CallReason = CallReasons.Create Then
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            CurrentAE_ID = AEID_HiddenField.Value
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Delete Then
            Delete = Delete_HiddenField.Value
        End If

        'Lock out if user does not have adequate edit rights
        LockoutCheck(CallReason, CurrentICSR_ID, tables.AEs, Title_Label, ButtonGroup_Div, Main_Table)

        'Format Controls
        AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
        If CallReason = CallReasons.Create Or CallReason = CallReasons.Update Then
            TextBoxReadOnly(MedDRATerm_Textbox)
            TextBoxReadOnly(Start_Textbox)
            TextBoxReadOnly(Stop_Textbox)
            DropDownListDisabled(Outcomes_DropDownList)
            DropDownListDisabled(DechallengeResults_DropDownList)
            DropDownListDisabled(RechallengeResults_DropDownList)
        ElseIf CallReason = CallReasons.Delete Then
            MedDRATerm_Row.Visible = False
            Start_Row.Visible = False
            Stop_Row.Visible = False
            Outcome_Row.Visible = False
            DechallengeResult_Row.Visible = False
            RechallengeResult_Row.Visible = False
        End If

        'Warn & abort if there are discrepancies between the data as shown at edit page load and as stored at save button click
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            Dim DiscrepancyString As String = String.Empty
            DiscrepancyString += DiscrepancyCheck(tables.AEs, fields.MedDRATerm, InputTypes.String, CurrentAE_ID, AtEditPageLoad_MedDRATerm_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.AEs, fields.Start, InputTypes.Date, CurrentAE_ID, AtEditPageLoad_Start_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.AEs, fields.Stop, InputTypes.Date, CurrentAE_ID, AtEditPageLoad_Stop_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.AEs, fields.Outcome_ID, InputTypes.Integer, CurrentAE_ID, AtEditPageLoad_Outcome_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.AEs, fields.DechallengeResult_ID, InputTypes.Integer, CurrentAE_ID, AtEditPageLoad_DechallengeResult_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.AEs, fields.RechallengeResult_ID, InputTypes.Integer, CurrentAE_ID, AtEditPageLoad_RechallengeResult_HiddenField)
            If DiscrepancyString <> String.Empty Then
                ShowDiscrepancyWarning(Status_Label, DiscrepancyString)
                Exit Sub
            End If
        End If

        'Store updates in database
        Dim UpdateCommand As New SqlCommand
        UpdateCommand.Connection = Connection
        If CallReason = CallReasons.Create Then
            UpdateCommand.CommandText = "INSERT INTO AEs (ICSR_ID, MedDRATerm, Start, Stop, Outcome_ID, DechallengeResult_ID, RechallengeResult_ID) VALUES(@CurrentICSR_ID, @MedDRATerm, CASE WHEN @Start = '' THEN NULL ELSE @Start END, CASE WHEN @Stop = '' THEN NULL ELSE @Stop END, CASE WHEN @Outcome_ID = 0 THEN NULL ELSE @Outcome_ID END, CASE WHEN @DechallengeResult_ID = 0 THEN NULL ELSE @DechallengeResult_ID END, CASE WHEN @RechallengeResult_ID = 0 THEN NULL ELSE @RechallengeResult_ID END)"
            UpdateCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            UpdateCommand.Parameters.AddWithValue("@MedDRATerm", MedDRATerm_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@Start", DateStringOrEmpty(Start_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@Stop", DateStringOrEmpty(Stop_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@Outcome_ID", Outcomes_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@DechallengeResult_ID", DechallengeResults_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@RechallengeResult_ID", RechallengeResults_DropDownList.SelectedValue)
        ElseIf CallReason = CallReasons.Update Then
            UpdateCommand.CommandText = "UPDATE AEs SET MedDRATerm = (CASE WHEN @MedDRATerm = '' THEN NULL ELSE @MedDRATerm END), Start = (CASE WHEN @Start = '' THEN NULL ELSE @Start END), Stop = (CASE WHEN @Stop = '' THEN NULL ELSE @Stop END), Outcome_ID = (CASE WHEN @Outcome_ID = 0 THEN NULL ELSE @Outcome_ID END), DechallengeResult_ID = (CASE WHEN @DechallengeResult_ID = 0 THEN NULL ELSE @DechallengeResult_ID END), RechallengeResult_ID = (CASE WHEN @RechallengeResult_ID = 0 THEN NULL ELSE @RechallengeResult_ID END) WHERE ID = @CurrentAE_ID"
            UpdateCommand.Parameters.AddWithValue("@MedDRATerm", MedDRATerm_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@Start", (Start_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@Stop", DateStringOrEmpty(Stop_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@Outcome_ID", Outcomes_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@DechallengeResult_ID", DechallengeResults_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@RechallengeResult_ID", RechallengeResults_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@CurrentAE_ID", CurrentAE_ID)
        ElseIf CallReason = CallReasons.Delete Then
            UpdateCommand.CommandText = "DELETE FROM AEs WHERE ID = @CurrentAE_ID"
            UpdateCommand.Parameters.AddWithValue("@CurrentAE_ID", CurrentAE_ID)
        End If
        SqlUpdate(Me, UpdateCommand)

        'Add audit trail entry
        Dim EntryString As String = HistoryDatabasebUpdateIntro
        If CallReason = CallReasons.Create Then
            Dim NewAE_ID As DataTable = SqlRead(Me, "SELECT TOP 1 ID FROM AEs WHERE ICSR_ID = " & TryCType(CurrentICSR_ID, InputTypes.Integer) & " ORDER BY ID DESC")
            CurrentAE_ID = NewAE_ID.Rows(0)(0)
            EntryString += NewReportIntro("Event", CurrentAE_ID)
        ElseIf CallReason = CallReasons.Delete Then
            EntryString += DeleteReportIntro("Event", CurrentAE_ID)
        End If
        EntryString += HistoryEntryPlainValue(CallReason, "AE", CurrentAE_ID, "MedDRA LLT", AtEditPageLoad_MedDRATerm_HiddenField.Value, MedDRATerm_Textbox.Text.Trim)
        EntryString += HistoryEntryPlainValue(CallReason, "Event", CurrentAE_ID, "Start Date", TryCType(AtEditPageLoad_Start_HiddenField.Value, InputTypes.Date), TryCType(Start_Textbox.Text, InputTypes.Date))
        EntryString += HistoryEntryPlainValue(CallReason, "Event", CurrentAE_ID, "Stop Date", TryCType(AtEditPageLoad_Stop_HiddenField.Value, InputTypes.Date), TryCType(Stop_Textbox.Text, InputTypes.Date))
        EntryString += HistoryEntryReferencedValue(CallReason, "Event", CurrentAE_ID, "Outcome", tables.Outcomes, fields.Name, TryCType(AtEditPageLoad_Outcome_HiddenField.Value, InputTypes.Integer), TryCType(Outcomes_DropDownList.SelectedValue, InputTypes.Integer))
        EntryString += HistoryEntryReferencedValue(CallReason, "Event", CurrentAE_ID, "Dechallenge Result", tables.DechallengeResults, fields.Name, TryCType(AtEditPageLoad_DechallengeResult_HiddenField.Value, InputTypes.Integer), TryCType(DechallengeResults_DropDownList.SelectedValue, InputTypes.Integer))
        EntryString += HistoryEntryReferencedValue(CallReason, "Event", CurrentAE_ID, "Rechallenge Result", tables.RechallengeResults, fields.Name, TryCType(AtEditPageLoad_RechallengeResult_HiddenField.Value, InputTypes.Integer), TryCType(RechallengeResults_DropDownList.SelectedValue, InputTypes.Integer))
        SaveAuditTrailEntry(Me, CurrentICSR_ID, LoggedIn_User_ID, EntryString)

    End Sub

End Class
