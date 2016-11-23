Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditICSRBasicInformation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Carry out Sub only on initial page load
        If Page.IsPostBack = True Then Exit Sub

        'Redirect to login if user is not logged in
        If Login_Status = False Then
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditICSRBasicInformation.aspx?" & Convert.ToString(Request.QueryString))
        End If

        'Determine call reason, store reason in hidden field and redirect to parent page if query string is invalid
        Dim CallReason As CallReasons
        If Not (sender.Request.QueryString("ICSRID") Is Nothing) Then
            CallReason = CallReasons.Update
        Else
            sender.Response.Redirect("~/Application/ICSRs.aspx")
        End If

        'Store query string values in hidden fields and variables
        ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value

        'Populate Title_Label
        Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Edit Basic Information"

        'Lock out if user does not have adequate edit rights
        LockoutCheck(CallReason, CurrentICSR_ID, tables.ICSRs, Title_Label, ButtonGroup_Div, Main_Table)

        'Format controls based on edit rights
        AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Assignee_ID) = True Then
            DropDownListEnabled(Assignees_DropDownList)
            Assignees_DropDownList.ToolTip = SelectorInputToolTip
        Else
            DropDownListDisabled(Assignees_DropDownList)
            Assignees_DropDownList.ToolTip = CannotEditControlText
        End If
        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Patient_Initials) = True Then
            TextBoxReadWrite(PatientInitials_TextBox)
            PatientInitials_TextBox.ToolTip = PatientInitialsInputToolTip
        Else
            TextBoxReadOnly(PatientInitials_TextBox)
            PatientInitials_TextBox.ToolTip = CannotEditControlText
        End If
        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Patient_YearOfBirth_ID) = True Then
            DropDownListEnabled(PatientYearOfBirth_DropDownList)
            PatientYearOfBirth_DropDownList.ToolTip = PatientYearOfBirthInputToolTip
        Else
            DropDownListDisabled(PatientYearOfBirth_DropDownList)
            PatientYearOfBirth_DropDownList.ToolTip = CannotEditControlText
        End If
        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Patient_Gender_ID) = True Then
            DropDownListEnabled(PatientGender_DropDownList)
            PatientGender_DropDownList.ToolTip = PatientGenderInputToolTip
        Else
            DropDownListDisabled(PatientGender_DropDownList)
            PatientGender_DropDownList.ToolTip = CannotEditControlText
        End If
        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.IsSerious_ID) = True Then
            DropDownListEnabled(IsSerious_DropDownList)
            IsSerious_DropDownList.ToolTip = ICSRSeriousnessSelectorInputToolTip
        Else
            DropDownListDisabled(IsSerious_DropDownList)
            IsSerious_DropDownList.ToolTip = CannotEditControlText
        End If
        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.SeriousnessCriterion_ID) = True Then
            DropDownListEnabled(SeriousnessCriteria_DropDownList)
            SeriousnessCriteria_DropDownList.ToolTip = ICSRSeriousnessCriterionSelectorInputToolTip
        Else
            DropDownListDisabled(SeriousnessCriteria_DropDownList)
            SeriousnessCriteria_DropDownList.ToolTip = CannotEditControlText
        End If
        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Narrative) = True Then
            TextBoxReadWrite(Narrative_Textbox)
            Narrative_Textbox.ToolTip = NarrativeInputToolTip
        Else
            TextBoxReadOnly(Narrative_Textbox)
            Narrative_Textbox.ToolTip = CannotEditControlText
        End If
        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.CompanyComment) = True Then
            TextBoxReadWrite(CompanyComment_Textbox)
            CompanyComment_Textbox.ToolTip = CompanyCommentInputToolTip
        Else
            TextBoxReadOnly(CompanyComment_Textbox)
            CompanyComment_Textbox.ToolTip = CannotEditControlText
        End If

        'Populate DropDownLists
        ICSRStatuses_DropDownList.DataSource = CreateUpdateToStatusesDatatable(tables.ICSRStatuses, CurrentICSR_ID)
        ICSRStatuses_DropDownList.DataValueField = "ID"
        ICSRStatuses_DropDownList.DataTextField = "Name"
        ICSRStatuses_DropDownList.DataBind()
        Assignees_DropDownList.DataSource = CreateAssigneesDropDownListDataTable(CurrentICSR_ID)
        Assignees_DropDownList.DataValueField = "ID"
        Assignees_DropDownList.DataTextField = "Name"
        Assignees_DropDownList.DataBind()
        PopulateDropDownList(PatientYearOfBirth_DropDownList, tables.YearsOfBirth)
        PopulateDropDownList(PatientGender_DropDownList, tables.Genders)
        PopulateDropDownList(IsSerious_DropDownList, tables.IsSerious)
        PopulateDropDownList(SeriousnessCriteria_DropDownList, tables.SeriousnessCriteria)

        'Populate data fields
        Dim CurrentValuesDataTable As DataTable = SqlRead(Me, "SELECT Companies.ID AS Company_ID, Companies.Name AS Company_Name, CASE WHEN ICSRStatus_ID IS NULL THEN 0 ELSE ICSRStatus_ID END AS ICSRStatus_ID, CASE WHEN Assignee_ID IS NULL THEN 0 ELSE Assignee_ID END AS Assignee_ID, CASE WHEN Patient_Initials IS NULL THEN '' ELSE Patient_Initials END AS Patient_Initials, CASE WHEN Patient_YearOfBirth_ID IS NULL THEN 0 ELSE Patient_YearOfBirth_ID END AS Patient_YearOfBirth_ID, CASE WHEN Patient_Gender_ID IS NULL THEN 0 ELSE Patient_Gender_ID end AS Patient_Gender_ID, CASE WHEN IsSerious_ID IS NULL THEN 0 ELSE IsSerious_ID END AS IsSerious_ID, CASE WHEN SeriousnessCriterion_ID IS NULL THEN 0 ELSE SeriousnessCriterion_ID END AS SeriousnessCriterion_ID, CASE WHEN Narrative IS NULL THEN '' ELSE Narrative END AS Narrative, CASE WHEN CompanyComment IS NULL THEN '' ELSE CompanyComment END AS CompanyComment FROM ICSRs INNER JOIN Companies ON ICSRs.Company_ID = Companies.ID WHERE ICSRs.ID = " & CurrentICSR_ID)
        'CompanyID_HiddenField.Value = CurrentValuesDataTable.Rows(0)(0)
        CompanyName_Textbox.Text = CurrentValuesDataTable.Rows(0)(1)
        ICSRStatuses_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(2)
        AtEditPageLoad_ICSRStatusID_HiddenField.Value = CurrentValuesDataTable.Rows(0)(2)
        Assignees_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(3)
        AtEditPageLoad_AssigneeID_HiddenField.Value = CurrentValuesDataTable.Rows(0)(3)
        PatientInitials_TextBox.Text = CurrentValuesDataTable.Rows(0)(4)
        AtEditPageLoad_PatientInitials_HiddenField.Value = CurrentValuesDataTable.Rows(0)(4)
        PatientYearOfBirth_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(5)
        AtEditPageLoad_PatientYearOfBirthID_HiddenField.Value = CurrentValuesDataTable.Rows(0)(5)
        PatientGender_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(6)
        AtEditPageLoad_PatientGenderID_HiddenField.Value = CurrentValuesDataTable.Rows(0)(6)
        IsSerious_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(7)
        AtEditPageLoad_IsSerious_HiddenField.Value = CurrentValuesDataTable.Rows(0)(7)
        SeriousnessCriteria_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(8)
        AtEditPageLoad_SeriousnessCriterionID_HiddenField.Value = CurrentValuesDataTable.Rows(0)(8)
        Narrative_Textbox.Text = CurrentValuesDataTable.Rows(0)(9)
        AtEditPageLoad_Narrative_HiddenField.Value = CurrentValuesDataTable.Rows(0)(9)
        CompanyComment_Textbox.Text = CurrentValuesDataTable.Rows(0)(10)
        AtEditPageLoad_CompanyComment_HiddenField.Value = CurrentValuesDataTable.Rows(0)(10)

    End Sub

    Protected Sub Cancel_Button_Click() Handles Cancel_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub ReturnToICSROverview_Button_Click() Handles ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub ICSRStatuses_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(ICSRStatuses_DropDownList) = True Then
            ICSRStatuses_DropDownList.CssClass = CssClassSuccess
            ICSRStatuses_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            ICSRStatuses_DropDownList.CssClass = CssClassFailure
            ICSRStatuses_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Assignees_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(Assignees_DropDownList) = True Then
            Assignees_DropDownList.CssClass = CssClassSuccess
            Assignees_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            Assignees_DropDownList.CssClass = CssClassFailure
            Assignees_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub PatientInitials_TextBox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(PatientInitials_TextBox) = True Then
            PatientInitials_TextBox.CssClass = CssClassSuccess
            PatientInitials_TextBox.ToolTip = String.Empty
            args.IsValid = True
        Else
            PatientInitials_TextBox.CssClass = CssClassFailure
            PatientInitials_TextBox.ToolTip = TextValidationFailToolTip
            args.IsValid = True
        End If
    End Sub

    Protected Sub PatientYearOfBirth_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(PatientYearOfBirth_DropDownList) = True Then
            PatientYearOfBirth_DropDownList.CssClass = CssClassSuccess
            PatientYearOfBirth_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            PatientYearOfBirth_DropDownList.CssClass = CssClassFailure
            PatientYearOfBirth_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = True
        End If
    End Sub

    Protected Sub PatientGender_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(PatientGender_DropDownList) = True Then
            PatientGender_DropDownList.CssClass = CssClassSuccess
            PatientGender_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            PatientGender_DropDownList.CssClass = CssClassFailure
            PatientGender_DropDownList.ToolTip = String.Empty
            args.IsValid = False
        End If
    End Sub

    Protected Sub IsSerious_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(IsSerious_DropDownList) = True Then
            IsSerious_DropDownList.CssClass = CssClassSuccess
            IsSerious_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            IsSerious_DropDownList.CssClass = CssClassFailure
            IsSerious_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If

    End Sub

    Protected Sub SeriousnessCriteria_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If IsSerious_DropDownList.SelectedValue = 0 And SeriousnessCriteria_DropDownList.SelectedValue = 0 Then 'If it was not specified whether the ICSR is serious and no seriousness criterion was selected
            SeriousnessCriteria_DropDownList.CssClass = CssClassSuccess
            SeriousnessCriteria_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        ElseIf IsSerious_DropDownList.SelectedValue = 1 And SeriousnessCriteria_DropDownList.SelectedValue <> 0 Then 'If the ICSR is serious and a seriousness criterion was selected
            SeriousnessCriteria_DropDownList.CssClass = CssClassSuccess
            SeriousnessCriteria_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        ElseIf IsSerious_DropDownList.SelectedValue = 2 And SeriousnessCriteria_DropDownList.SelectedValue = 0 Then 'If the ICSR is not serious and a seriousness criterion was not selected
            SeriousnessCriteria_DropDownList.CssClass = CssClassSuccess
            SeriousnessCriteria_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else 'If the ICSR is not serious and a seriousness criterion was selected or if the ICSR is serious and no seriousness criterion was selected 
            IsSerious_DropDownList.CssClass = CssClassFailure
            IsSerious_DropDownList.ToolTip = SeriousnessConsistencyValidationFailToolTip
            SeriousnessCriteria_DropDownList.CssClass = CssClassFailure
            SeriousnessCriteria_DropDownList.ToolTip = SeriousnessConsistencyValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Narrative_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(Narrative_Textbox) = True Then
            Narrative_Textbox.CssClass = CssClassSuccess
            Narrative_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Narrative_Textbox.CssClass = CssClassSuccess
            Narrative_Textbox.ToolTip = TextValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub CompanyComment_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(CompanyComment_Textbox) = True Then
            CompanyComment_Textbox.CssClass = CssClassSuccess
            CompanyComment_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            CompanyComment_Textbox.CssClass = CssClassFailure
            CompanyComment_Textbox.ToolTip = TextValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click

        'Carry out Sub only if input is valid
        If Page.IsValid = False Then Exit Sub

        'Redirect to login if user is not logged in
        If Login_Status = False Then
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditICSRBasicInformation.aspx?" & Convert.ToString(Request.QueryString))
        End If

        'Store values from hidden fields in variables
        Dim CallReason As CallReasons = CallReasons.Update
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        'Dim CurrentICSR_Company_ID As Integer = CompanyID_HiddenField.Value

        'Lock out if user does not have adequate edit rights
        LockoutCheck(CallReason, CurrentICSR_ID, tables.ICSRs, Title_Label, ButtonGroup_Div, Main_Table)

        'Format Controls
        AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
        TextBoxReadOnly(CompanyName_Textbox)
        DropDownListDisabled(ICSRStatuses_DropDownList)
        DropDownListDisabled(Assignees_DropDownList)
        TextBoxReadOnly(PatientInitials_TextBox)
        DropDownListDisabled(PatientYearOfBirth_DropDownList)
        DropDownListDisabled(PatientYearOfBirth_DropDownList)
        DropDownListDisabled(PatientGender_DropDownList)
        DropDownListDisabled(IsSerious_DropDownList)
        DropDownListDisabled(SeriousnessCriteria_DropDownList)
        TextBoxReadOnly(Narrative_Textbox)
        TextBoxReadOnly(CompanyComment_Textbox)

        'Warn & abort if there are discrepancies between the data as shown at edit page load and as stored at save button click
        Dim DiscrepancyString As String = String.Empty
        DiscrepancyString += DiscrepancyCheck(tables.ICSRs, fields.ICSRStatus_ID, InputTypes.Integer, CurrentICSR_ID, AtEditPageLoad_ICSRStatusID_HiddenField)
        DiscrepancyString += DiscrepancyCheck(tables.ICSRs, fields.Assignee_ID, InputTypes.Integer, CurrentICSR_ID, AtEditPageLoad_AssigneeID_HiddenField)
        DiscrepancyString += DiscrepancyCheck(tables.ICSRs, fields.Patient_Initials, InputTypes.String, CurrentICSR_ID, AtEditPageLoad_PatientInitials_HiddenField)
        DiscrepancyString += DiscrepancyCheck(tables.ICSRs, fields.Patient_YearOfBirth_ID, InputTypes.Integer, CurrentICSR_ID, AtEditPageLoad_PatientYearOfBirthID_HiddenField)
        DiscrepancyString += DiscrepancyCheck(tables.ICSRs, fields.Patient_Gender_ID, InputTypes.Integer, CurrentICSR_ID, AtEditPageLoad_PatientGenderID_HiddenField)
        DiscrepancyString += DiscrepancyCheck(tables.ICSRs, fields.IsSerious_ID, InputTypes.Integer, CurrentICSR_ID, AtEditPageLoad_IsSerious_HiddenField)
        DiscrepancyString += DiscrepancyCheck(tables.ICSRs, fields.SeriousnessCriterion_ID, InputTypes.Integer, CurrentICSR_ID, AtEditPageLoad_SeriousnessCriterionID_HiddenField)
        DiscrepancyString += DiscrepancyCheck(tables.ICSRs, fields.Narrative, InputTypes.String, CurrentICSR_ID, AtEditPageLoad_Narrative_HiddenField)
        DiscrepancyString += DiscrepancyCheck(tables.ICSRs, fields.CompanyComment, InputTypes.String, CurrentICSR_ID, AtEditPageLoad_CompanyComment_HiddenField)
        If DiscrepancyString <> String.Empty Then
            ShowDiscrepancyWarning(Status_Label, DiscrepancyString)
            Exit Sub
        End If

        'Store updates in database
        Dim UpdateCommand As New SqlCommand
        UpdateCommand.Connection = Connection
        UpdateCommand.CommandText = "UPDATE ICSRs SET Assignee_ID = (CASE WHEN @Assignee_ID = 0 THEN NULL ELSE @Assignee_ID END), ICSRStatus_ID = (CASE WHEN @ICSRStatus_ID = 0 THEN NULL ELSE @ICSRStatus_ID END), Patient_Initials = (CASE WHEN @Patient_Initials = '' THEN NULL ELSE @Patient_Initials END), Patient_YearOfBirth_ID = (CASE WHEN @Patient_YearOfBirth_ID = 0 THEN NULL ELSE @Patient_YearOfBirth_ID END), Patient_Gender_ID = (CASE WHEN @Patient_Gender_ID = 0 THEN NULL ELSE @Patient_Gender_ID END), IsSerious_ID = (CASE WHEN @IsSerious_ID = 0 THEN NULL ELSE @IsSerious_ID END), SeriousnessCriterion_ID = (CASE WHEN @SeriousnessCriterion_ID = 0 THEN NULL ELSE @SeriousnessCriterion_ID END), Narrative = (CASE WHEN @Narrative = '' THEN NULL ELSE @Narrative END), CompanyComment = (CASE WHEN @CompanyComment = '' THEN NULL ELSE @CompanyComment end) WHERE ID = @CurrentICSR_ID"
        UpdateCommand.Parameters.AddWithValue("@ICSRStatus_ID", ICSRStatuses_DropDownList.SelectedValue)
        UpdateCommand.Parameters.AddWithValue("@Assignee_ID", Assignees_DropDownList.SelectedValue)
        UpdateCommand.Parameters.AddWithValue("@Patient_Initials", PatientInitials_TextBox.Text)
        UpdateCommand.Parameters.AddWithValue("@Patient_YearOfBirth_ID", PatientYearOfBirth_DropDownList.SelectedValue)
        UpdateCommand.Parameters.AddWithValue("@Patient_Gender_ID", PatientGender_DropDownList.SelectedValue)
        UpdateCommand.Parameters.AddWithValue("@IsSerious_ID", IsSerious_DropDownList.SelectedValue)
        UpdateCommand.Parameters.AddWithValue("@SeriousnessCriterion_ID", SeriousnessCriteria_DropDownList.SelectedValue)
        UpdateCommand.Parameters.AddWithValue("@Narrative", Narrative_Textbox.Text)
        UpdateCommand.Parameters.AddWithValue("@CompanyComment", CompanyComment_Textbox.Text)
        UpdateCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
        SqlUpdate(Me, UpdateCommand)

        'Add audit trail entry
        Dim EntryString As String = HistoryDatabasebUpdateIntro
        EntryString += HistoryEntryReferencedValue(CallReason, "ICSR", CurrentICSR_ID, "Status", tables.ICSRStatuses, fields.Name, AtEditPageLoad_ICSRStatusID_HiddenField.Value, ICSRStatuses_DropDownList.SelectedValue)
        EntryString += HistoryEntryReferencedValue(CallReason, "ICSR", CurrentICSR_ID, "Assignee", tables.Users, fields.Name, AtEditPageLoad_AssigneeID_HiddenField.Value, Assignees_DropDownList.SelectedValue)
        EntryString += HistoryEntryPlainValue(CallReason, "ICSR", CurrentICSR_ID, "Patient Initials", AtEditPageLoad_PatientInitials_HiddenField.Value, PatientInitials_TextBox.Text.Trim)
        EntryString += HistoryEntryReferencedValue(CallReason, "ICSR", CurrentICSR_ID, "Patient Year of Birth", tables.YearsOfBirth, fields.Name, AtEditPageLoad_PatientYearOfBirthID_HiddenField.Value, PatientYearOfBirth_DropDownList.SelectedValue)
        EntryString += HistoryEntryReferencedValue(CallReason, "ICSR", CurrentICSR_ID, "Patient Gender", tables.Genders, fields.Name, AtEditPageLoad_PatientGenderID_HiddenField.Value, PatientGender_DropDownList.SelectedValue)
        EntryString += HistoryEntryReferencedValue(CallReason, "ICSR", CurrentICSR_ID, "Is Serious", tables.IsSerious, fields.Name, AtEditPageLoad_IsSerious_HiddenField.Value, IsSerious_DropDownList.SelectedValue)
        EntryString += HistoryEntryReferencedValue(CallReason, "ICSR", CurrentICSR_ID, "Seriousness Criterion", tables.SeriousnessCriteria, fields.Name, AtEditPageLoad_SeriousnessCriterionID_HiddenField.Value, SeriousnessCriteria_DropDownList.SelectedValue)
        EntryString += HistoryEntryPlainValue(CallReason, "ICSR", CurrentICSR_ID, "Narrative", AtEditPageLoad_Narrative_HiddenField.Value, Narrative_Textbox.Text.Trim)
        EntryString += HistoryEntryPlainValue(CallReason, "ICSR", CurrentICSR_ID, "Company Comment", AtEditPageLoad_CompanyComment_HiddenField.Value, CompanyComment_Textbox.Text.Trim)
        SaveAuditTrailEntry(Me, CurrentICSR_ID, LoggedIn_User_ID, EntryString)

    End Sub

End Class
