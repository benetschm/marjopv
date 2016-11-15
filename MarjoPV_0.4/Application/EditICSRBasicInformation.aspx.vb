Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditICSRBasicInformation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            'Check if user is logged in and redirect to login page if not
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Edit Basic Information"
            If Login_Status = True Then
                'Check if user has edit rights and lock out if not
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Edit) = True Then
                    'Format Controls depending on user edit rights for each control
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Assignee_ID) = True Then
                        DropDownListEnabled(Assignees_DropDownList)
                        Assignees_DropDownList.ToolTip = "Please select an assignee"
                    Else
                        DropDownListDisabled(Assignees_DropDownList)
                        Assignees_DropDownList.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Patient_Initials) = True Then
                        TextBoxReadWrite(PatientInitials_TextBox)
                        PatientInitials_TextBox.ToolTip = "Please enter patient initials"
                    Else
                        TextBoxReadOnly(PatientInitials_TextBox)
                        PatientInitials_TextBox.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Patient_YearOfBirth_ID) = True Then
                        DropDownListEnabled(PatientYearOfBirth_DropDownList)
                        PatientYearOfBirth_DropDownList.ToolTip = "Please select patient year of birth"
                    Else
                        DropDownListDisabled(PatientYearOfBirth_DropDownList)
                        PatientYearOfBirth_DropDownList.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Patient_Gender_ID) = True Then
                        DropDownListEnabled(PatientGender_DropDownList)
                        PatientGender_DropDownList.ToolTip = "Please select patient gender"
                    Else
                        DropDownListDisabled(PatientGender_DropDownList)
                        PatientGender_DropDownList.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.IsSerious) = True Then
                        DropDownListEnabled(IsSerious_DropDownList)
                        IsSerious_DropDownList.ToolTip = "Please select whether the ICSR is serious"
                    Else
                        DropDownListDisabled(IsSerious_DropDownList)
                        IsSerious_DropDownList.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.SeriousnessCriterion_ID) = True Then
                        DropDownListEnabled(SeriousnessCriteria_DropDownList)
                        SeriousnessCriteria_DropDownList.ToolTip = "Please select a seriousness criterion if the ICSR is serious"
                    Else
                        DropDownListDisabled(SeriousnessCriteria_DropDownList)
                        SeriousnessCriteria_DropDownList.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.Narrative) = True Then
                        TextBoxReadWrite(Narrative_Textbox)
                        Narrative_Textbox.ToolTip = "Please enter a narrative text"
                    Else
                        TextBoxReadOnly(Narrative_Textbox)
                        Narrative_Textbox.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRs, fields.CompanyComment) = True Then
                        TextBoxReadWrite(CompanyComment_Textbox)
                        CompanyComment_Textbox.ToolTip = "Please enter a company comment text"
                    Else
                        TextBoxReadOnly(CompanyComment_Textbox)
                        CompanyComment_Textbox.ToolTip = CannotEditControlText
                    End If
                    'Populate ICSRStatuses_DropDownList with statuses the current user can update the current ICSR to
                    ICSRStatuses_DropDownList.DataSource = CreateUpdateToStatusesDatatable(tables.ICSRStatuses, CurrentICSR_ID)
                    ICSRStatuses_DropDownList.DataValueField = "Status_ID"
                    ICSRStatuses_DropDownList.DataTextField = "Status_Name"
                    ICSRStatuses_DropDownList.DataBind()
                    'Populate Assignees_DropDownList with users who have any role for the company assoiated with the current ICSR
                    Assignees_DropDownList.DataSource = CreateAssigneesDropDownListDataTable(CurrentICSR_ID)
                    Assignees_DropDownList.DataValueField = "ID"
                    Assignees_DropDownList.DataTextField = "Name"
                    Assignees_DropDownList.DataBind()
                    'Populate PatientYearOfBirth_DropDownList
                    PatientYearOfBirth_DropDownList.DataSource = CreateDropDownListDatatable(tables.YearsOfBirth)
                    PatientYearOfBirth_DropDownList.DataValueField = "ID"
                    PatientYearOfBirth_DropDownList.DataTextField = "Name"
                    PatientYearOfBirth_DropDownList.DataBind()
                    'Populate PatientGender_DropDownList
                    PatientGender_DropDownList.DataSource = CreateDropDownListDatatable(tables.Genders)
                    PatientGender_DropDownList.DataValueField = "ID"
                    PatientGender_DropDownList.DataTextField = "Name"
                    PatientGender_DropDownList.DataBind()
                    'Populate IsSerious_DropDownList
                    IsSerious_DropDownList.DataSource = CreateDropDownListDatatable(tables.IsSerious)
                    IsSerious_DropDownList.DataValueField = "ID"
                    IsSerious_DropDownList.DataTextField = "Name"
                    IsSerious_DropDownList.DataBind()
                    'Populate Seriousness Criterion DropDownList
                    SeriousnessCriteria_DropDownList.DataSource = CreateDropDownListDatatable(tables.SeriousnessCriteria)
                    SeriousnessCriteria_DropDownList.DataValueField = "ID"
                    SeriousnessCriteria_DropDownList.DataTextField = "Name"
                    SeriousnessCriteria_DropDownList.DataBind()
                    'Populate controls and store values as present in the database at edit page load to use when checking for database update conflicts (see save button click event)
                    Dim AtEditPageLoadReadCommand As New SqlCommand("SELECT Companies.Name AS Company_Name, CASE WHEN ICSRStatus_ID IS NULL THEN 0 ELSE ICSRStatus_ID END AS ICSRStatus_ID, CASE WHEN Assignee_ID IS NULL THEN 0 ELSE Assignee_ID END AS Assignee_ID, CASE WHEN Patient_Initials IS NULL THEN '' ELSE Patient_Initials END AS Patient_Initials, CASE WHEN Patient_YearOfBirth_ID IS NULL THEN 0 ELSE Patient_YearOfBirth_ID END AS Patient_YearOfBirth_ID, CASE WHEN Patient_Gender_ID IS NULL THEN 0 ELSE Patient_Gender_ID end AS Patient_Gender_ID, CASE WHEN IsSerious_ID IS NULL THEN 0 ELSE IsSerious_ID END AS IsSerious_ID, CASE WHEN SeriousnessCriterion_ID IS NULL THEN 0 ELSE SeriousnessCriterion_ID END AS SeriousnessCriterion_ID, CASE WHEN Narrative IS NULL THEN '' ELSE Narrative END AS Narrative, CASE WHEN CompanyComment IS NULL THEN '' ELSE CompanyComment END AS CompanyComment FROM ICSRs INNER JOIN Companies ON ICSRs.Company_ID = Companies.ID WHERE ICSRs.ID = @CurrentICSR_ID", Connection)
                    AtEditPageLoadReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
                    Try
                        Connection.Open()
                        Dim AtEditPageLoadReader As SqlDataReader = AtEditPageLoadReadCommand.ExecuteReader()
                        While AtEditPageLoadReader.Read()
                            CompanyName_Textbox.Text = AtEditPageLoadReader.GetString(0)
                            ICSRStatuses_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(1)
                            AtEditPageLoad_ICSRStatusID_HiddenField.Value = AtEditPageLoadReader.GetInt32(1)
                            Assignees_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(2)
                            AtEditPageLoad_AssigneeID_HiddenField.Value = AtEditPageLoadReader.GetInt32(2)
                            PatientInitials_TextBox.Text = AtEditPageLoadReader.GetString(3)
                            AtEditPageLoad_PatientInitials_HiddenField.Value = AtEditPageLoadReader.GetString(3)
                            PatientYearOfBirth_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(4)
                            AtEditPageLoad_PatientYearOfBirthID_HiddenField.Value = AtEditPageLoadReader.GetInt32(4)
                            PatientGender_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(5)
                            AtEditPageLoad_PatientGenderID_HiddenField.Value = AtEditPageLoadReader.GetInt32(5)
                            IsSerious_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(6)
                            AtEditPageLoad_IsSerious_HiddenField.Value = AtEditPageLoadReader.GetInt32(6)
                            SeriousnessCriteria_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(7)
                            AtEditPageLoad_SeriousnessCriterionID_HiddenField.Value = AtEditPageLoadReader.GetInt32(7)
                            Narrative_Textbox.Text = AtEditPageLoadReader.GetString(8)
                            AtEditPageLoad_Narrative_HiddenField.Value = AtEditPageLoadReader.GetString(8)
                            CompanyComment_Textbox.Text = AtEditPageLoadReader.GetString(9)
                            AtEditPageLoad_CompanyComment_HiddenField.Value = AtEditPageLoadReader.GetString(9)
                        End While
                    Catch ex As Exception
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                        Exit Sub
                    Finally
                        Connection.Close()
                    End Try
                Else 'lock out user if he/she does not have edit rights 
                    Title_Label.Text = Lockout_Text
                    ButtonGroup_Div.Visible = False
                    Main_Table.Visible = False
                End If
            Else 'Redirect user to login if he/she is not logged in
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditBasicInformation.aspx?ICSRID=" & CurrentICSR_ID)
            End If
        End If
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
        If Page.IsValid = True Then
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            'Retrieve ICSR values as present in database at edit page load from hidden fields and store in variables to use when checking for database update conflicts (see page load event)
            Dim AtEditPageLoad_ICSRStatusID As Integer = AtEditPageLoad_ICSRStatusID_HiddenField.Value
            Dim AtEditPageLoad_Assignee_ID As Integer = AtEditPageLoad_AssigneeID_HiddenField.Value
            Dim AtEditPageLoad_PatientInitials As String = AtEditPageLoad_PatientInitials_HiddenField.Value
            Dim AtEditPageLoad_PatientYearOfBirth_ID As Integer = AtEditPageLoad_PatientYearOfBirthID_HiddenField.Value
            Dim AtEditPageLoad_PatientGender_ID As Integer = AtEditPageLoad_PatientGenderID_HiddenField.Value
            Dim AtEditPageLoad_IsSerious_ID As Integer = AtEditPageLoad_IsSerious_HiddenField.Value
            Dim AtEditPageLoad_SeriousnessCriterion_ID As Integer = AtEditPageLoad_SeriousnessCriterionID_HiddenField.Value
            Dim AtEditPageLoad_Narrative As String = AtEditPageLoad_Narrative_HiddenField.Value
            Dim AtEditPageLoad_CompanyComment As String = AtEditPageLoad_CompanyComment_HiddenField.Value
            'Store ICSR values as present in database when save button is clicked in variables
            Dim AtSaveButtonClickReadCommand As New SqlCommand("SELECT CASE WHEN ICSRStatus_ID IS NULL THEN 0 ELSE ICSRStatus_ID END AS ICSRStatus_ID, CASE WHEN Assignee_ID IS NULL THEN 0 ELSE Assignee_ID END AS Assignee_ID, CASE WHEN Patient_Initials IS NULL THEN '' ELSE Patient_Initials END AS Patient_Initials, CASE WHEN Patient_YearOfBirth_ID IS NULL THEN 0 ELSE Patient_YearOfBirth_ID END AS Patient_YearOfBirth_ID, CASE WHEN Patient_Gender_ID IS NULL THEN 0 ELSE Patient_Gender_ID END AS Patient_Gender_ID, CASE WHEN IsSerious_ID IS NULL THEN 0 ELSE IsSerious_ID END AS IsSerious_ID, CASE WHEN SeriousnessCriterion_ID IS NULL THEN 0 ELSE SeriousnessCriterion_ID END AS SeriousnessCriterion_ID, CASE WHEN Narrative IS NULL THEN '' ELSE Narrative END AS Narrative, CASE WHEN CompanyComment IS NULL THEN '' ELSE CompanyComment END AS CompanyComment FROM ICSRs WHERE ICSRs.ID = @CurrentICSR_ID", Connection)
            AtSaveButtonClickReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            Dim AtSaveButtonClick_ICSRStatus_ID As Integer = Nothing
            Dim AtSaveButtonClick_Assignee_ID As Integer = Nothing
            Dim AtSaveButtonClick_PatientInitials As String = String.Empty
            Dim AtSaveButtonClick_PatientYearOfBirth_ID As Integer = Nothing
            Dim AtSaveButtonClick_PatientGender_ID As Integer = Nothing
            Dim AtSaveButtonClick_IsSerious_ID As Integer = Nothing
            Dim AtSaveButtonClick_SeriousnessCriterion_ID As Integer = Nothing
            Dim AtSaveButtonClick_Narrative As String = String.Empty
            Dim AtSaveButtonClick_CompanyComment As String = String.Empty
            Try
                Connection.Open()
                Dim AtSaveButtonClickReader As SqlDataReader = AtSaveButtonClickReadCommand.ExecuteReader()
                While AtSaveButtonClickReader.Read()
                    AtSaveButtonClick_ICSRStatus_ID = AtSaveButtonClickReader.GetInt32(0)
                    AtSaveButtonClick_Assignee_ID = AtSaveButtonClickReader.GetInt32(1)
                    AtSaveButtonClick_PatientInitials = AtSaveButtonClickReader.GetString(2)
                    AtSaveButtonClick_PatientYearOfBirth_ID = AtSaveButtonClickReader.GetInt32(3)
                    AtSaveButtonClick_PatientGender_ID = AtSaveButtonClickReader.GetInt32(4)
                    AtSaveButtonClick_IsSerious_ID = AtSaveButtonClickReader.GetInt32(5)
                    AtSaveButtonClick_SeriousnessCriterion_ID = AtSaveButtonClickReader.GetInt32(6)
                    AtSaveButtonClick_Narrative = AtSaveButtonClickReader.GetString(7)
                    AtSaveButtonClick_CompanyComment = AtSaveButtonClickReader.GetString(8)
                End While
                Connection.Close()
            Catch ex As Exception
                Connection.Close()
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            End Try
            'Check for discrepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked
            Dim DiscrepancyString As String = String.Empty
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ICSRStatusID, AtSaveButtonClick_ICSRStatus_ID, "ICSR Status")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Assignee_ID, AtSaveButtonClick_Assignee_ID, "Assignee")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_PatientInitials, AtSaveButtonClick_PatientInitials, "Patient Initials")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_PatientYearOfBirth_ID, AtSaveButtonClick_PatientYearOfBirth_ID, "Patient Year Of Birth")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_PatientGender_ID, AtSaveButtonClick_PatientGender_ID, "Patient Gender")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_IsSerious_ID, AtSaveButtonClick_IsSerious_ID, "Is Serious Status")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_SeriousnessCriterion_ID, AtSaveButtonClick_SeriousnessCriterion_ID, "Seriousness Criterion")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Narrative, AtSaveButtonClick_Narrative, "Narrative")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_CompanyComment, AtSaveButtonClick_CompanyComment, "Company Comment")
            'If Discprepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked are found, show warning and abort update
            If DiscrepancyString <> String.Empty Then
                AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
                Status_Label.Style.Add("text-align", "left")
                Status_Label.Style.Add("height", "auto")
                Status_Label.Text = DiscrepancyStringIntro & DiscrepancyString & DiscrepancyStringOutro
                Status_Label.CssClass = "form-control alert-danger"
                If LoggedIn_User_CanViewCompanies = True Then
                    Company_Row.Visible = True
                End If
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
                Exit Sub
            End If
            'If no discrepancies were found between database contents as present when edit page was loaded and database contents as present when save button is clicked, write ICSR Updates to database
            Dim UpdateCommand As New SqlCommand("UPDATE ICSRs SET Assignee_ID = (CASE WHEN @Assignee_ID = 0 THEN NULL ELSE @Assignee_ID END), ICSRStatus_ID = (CASE WHEN @ICSRStatus_ID = 0 THEN NULL ELSE @ICSRStatus_ID END), Patient_Initials = (CASE WHEN @Patient_Initials = '' THEN NULL ELSE @Patient_Initials END), Patient_YearOfBirth_ID = (CASE WHEN @Patient_YearOfBirth_ID = 0 THEN NULL ELSE @Patient_YearOfBirth_ID END), Patient_Gender_ID = (CASE WHEN @Patient_Gender_ID = 0 THEN NULL ELSE @Patient_Gender_ID END), IsSerious_ID = (CASE WHEN @IsSerious_ID = 0 THEN NULL ELSE @IsSerious_ID END), SeriousnessCriterion_ID = (CASE WHEN @SeriousnessCriterion_ID = 0 THEN NULL ELSE @SeriousnessCriterion_ID END), Narrative = (CASE WHEN @Narrative = '' THEN NULL ELSE @Narrative END), CompanyComment = (CASE WHEN @CompanyComment = '' THEN NULL ELSE @CompanyComment end) WHERE ID = @CurrentICSR_ID", Connection)
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
            Try
                Connection.Open()
                UpdateCommand.ExecuteNonQuery()
                Connection.Close()
            Catch ex As Exception
                Connection.Close()
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            End Try
            'Read updated values from database and store in variables
            Dim UpdatedReadCommand As New SqlCommand("SELECT CASE WHEN ICSRStatus_ID IS NULL THEN 0 ELSE ICSRStatus_ID END AS ICSRStatus_ID, CASE WHEN Assignee_ID IS NULL THEN 0 ELSE Assignee_ID END AS Assignee_ID, CASE WHEN Patient_Initials IS NULL THEN '' ELSE Patient_Initials END AS Patient_Initials, CASE WHEN Patient_YearOfBirth_ID IS NULL THEN 0 ELSE Patient_YearOfBirth_ID END AS Patient_YearOfBirth_ID, CASE WHEN Patient_Gender_ID IS NULL THEN 0 ELSE Patient_Gender_ID END AS Patient_Gender_ID, CASE WHEN IsSerious_ID IS NULL THEN 0 ELSE IsSerious_ID END AS IsSerious_ID, CASE WHEN SeriousnessCriterion_ID IS NULL THEN 0 ELSE SeriousnessCriterion_ID END AS SeriousnessCriterion_ID, CASE WHEN Narrative IS NULL THEN '' ELSE Narrative END AS Narrative, CASE WHEN CompanyComment IS NULL THEN '' ELSE CompanyComment END AS CompanyComment FROM ICSRs WHERE ICSRs.ID = @CurrentICSR_ID", Connection)
            UpdatedReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            Dim Updated_ICSRStatus_ID As Integer = Nothing
            Dim Updated_Assignee_ID As Integer = Nothing
            Dim Updated_PatientInitials As String = String.Empty
            Dim Updated_PatientYearOfBirth_ID As Integer = Nothing
            Dim Updated_PatientGender_ID As Integer = Nothing
            Dim Updated_IsSerious_ID As Integer = Nothing
            Dim Updated_SeriousnessCriterion_ID As Integer = Nothing
            Dim Updated_Narrative As String = String.Empty
            Dim Updated_CompanyComment As String = String.Empty
            Try
                Connection.Open()
                Dim UpdatedReader As SqlDataReader = UpdatedReadCommand.ExecuteReader()
                While UpdatedReader.Read()
                    Updated_ICSRStatus_ID = UpdatedReader.GetInt32(0)
                    Updated_Assignee_ID = UpdatedReader.GetInt32(1)
                    Updated_PatientInitials = UpdatedReader.GetString(2)
                    Updated_PatientYearOfBirth_ID = UpdatedReader.GetInt32(3)
                    Updated_PatientGender_ID = UpdatedReader.GetInt32(4)
                    Updated_IsSerious_ID = UpdatedReader.GetInt32(5)
                    Updated_SeriousnessCriterion_ID = UpdatedReader.GetInt32(6)
                    Updated_Narrative = UpdatedReader.GetString(7)
                    Updated_CompanyComment = UpdatedReader.GetString(8)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Compare old and new variables to generate EntryString for Case History Entry
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            If Updated_ICSRStatus_ID <> AtSaveButtonClick_ICSRStatus_ID Then
                EntryString += HistoryEntryReferencedValue("ICSR", CurrentICSR_ID, "Status", tables.ICSRStatuses, fields.Name, AtSaveButtonClick_ICSRStatus_ID, Updated_ICSRStatus_ID)
            End If
            If Updated_Assignee_ID <> AtSaveButtonClick_Assignee_ID Then
                EntryString += HistoryEntryReferencedValue("ICSR", CurrentICSR_ID, "Assignee", tables.Users, fields.Name, AtSaveButtonClick_Assignee_ID, Updated_Assignee_ID)
            End If
            If Updated_PatientInitials <> AtSaveButtonClick_PatientInitials Then
                EntryString += HistoryEnrtyPlainValue("ICSR", CurrentICSR_ID, "Patient Initials", AtSaveButtonClick_PatientInitials, Updated_PatientInitials)
            End If
            If Updated_PatientYearOfBirth_ID <> AtSaveButtonClick_PatientYearOfBirth_ID Then
                EntryString += HistoryEntryReferencedValue("ICSR", CurrentICSR_ID, "Patient Year of Birth", tables.YearsOfBirth, fields.Name, AtSaveButtonClick_PatientYearOfBirth_ID, Updated_PatientYearOfBirth_ID)
            End If
            If Updated_PatientGender_ID <> AtSaveButtonClick_PatientGender_ID Then
                EntryString += HistoryEntryReferencedValue("ICSR", CurrentICSR_ID, "Patient Gender", tables.Genders, fields.Name, AtSaveButtonClick_PatientGender_ID, Updated_PatientGender_ID)
            End If
            If Updated_IsSerious_ID <> AtSaveButtonClick_IsSerious_ID Then
                EntryString += HistoryEntryReferencedValue("ICSR", CurrentICSR_ID, "Is Serious", tables.IsSerious, fields.Name, AtSaveButtonClick_IsSerious_ID, Updated_IsSerious_ID)
            End If
            If Updated_SeriousnessCriterion_ID <> AtSaveButtonClick_SeriousnessCriterion_ID Then
                EntryString += HistoryEntryReferencedValue("ICSR", CurrentICSR_ID, "Seriousness Criterion", tables.SeriousnessCriteria, fields.Name, AtSaveButtonClick_SeriousnessCriterion_ID, Updated_SeriousnessCriterion_ID)
            End If
            If Updated_Narrative <> AtSaveButtonClick_Narrative Then
                EntryString += HistoryEnrtyPlainValue("ICSR", CurrentICSR_ID, "Narrative", AtSaveButtonClick_Narrative, Updated_Narrative)
            End If
            If Updated_CompanyComment <> AtSaveButtonClick_CompanyComment Then
                EntryString += HistoryEnrtyPlainValue("ICSR", CurrentICSR_ID, "Company Comment", AtSaveButtonClick_CompanyComment, Updated_CompanyComment)
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
            If LoggedIn_User_CanViewCompanies = True Then
                Company_Row.Visible = True
            End If
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
        End If
    End Sub

End Class
