Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystrings in hiddenfields to prevent issues through URL tampering
            ReportID_HiddenField.Value = Request.QueryString("ReportID")
            Dim CurrentReport_ID As Integer = ReportID_HiddenField.Value
            Delete_HiddenField.Value = Request.QueryString("Delete")
            Dim Delete As Boolean = False
            If Delete_HiddenField.Value = "True" Then
                Delete = True
            End If
            Dim CurrentICSR_ID As Integer = ParentID(tables.ICSRs, tables.Reports, fields.ICSR_ID, CurrentReport_ID)
            ICSRID_HiddenField.Value = CurrentICSR_ID
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                'Populate Title_Label
                If Delete = False Then
                    Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Edit Report " & CurrentReport_ID
                ElseIf Delete = True Then
                    Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Delete Report " & CurrentReport_ID
                End If
                If Delete = False Then
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.Edit) = True Then
                        'Format Controls depending on user edit rights for each control
                        AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ReportType_ID) = True Then
                            DropDownListEnabled(ReportType_DropDownList)
                            ReportType_DropDownList.ToolTip = SelectorInputToolTip
                        Else
                            DropDownListDisabled(ReportType_DropDownList)
                            ReportType_DropDownList.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.Received) = True Then
                            TextBoxReadWrite(ReportReceived_TextBox)
                            ReportReceived_TextBox.ToolTip = DateInputToolTip
                        Else
                            TextBoxReadOnly(ReportReceived_TextBox)
                            ReportReceived_TextBox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.Due) = True Then
                            TextBoxReadWrite(ReportDue_TextBox)
                            ReportDue_TextBox.ToolTip = DateInputToolTip
                        Else
                            TextBoxReadOnly(ReportDue_TextBox)
                            ReportDue_TextBox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ReportComplexity_ID) = True Then
                            DropDownListEnabled(ReportComplexity_DropDownList)
                            ReportComplexity_DropDownList.ToolTip = SelectorInputToolTip
                        Else
                            DropDownListDisabled(ReportComplexity_DropDownList)
                            ReportComplexity_DropDownList.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ReportSource_ID) = True Then
                            DropDownListEnabled(ReportSource_DropDownList)
                            ReportSource_DropDownList.ToolTip = SelectorInputToolTip
                        Else
                            DropDownListDisabled(ReportSource_DropDownList)
                            ReportSource_DropDownList.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ReporterName) = True Then
                            TextBoxReadWrite(ReporterName_Textbox)
                            ReporterName_Textbox.ToolTip = "Please enter a reporter name"
                        Else
                            TextBoxReadOnly(ReporterName_Textbox)
                            ReporterName_Textbox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ReporterAddress) = True Then
                            TextBoxReadWrite(ReporterAddress_Textbox)
                            ReporterAddress_Textbox.ToolTip = "Please enter a reporter address"
                        Else
                            TextBoxReadOnly(ReporterAddress_Textbox)
                            ReporterAddress_Textbox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ReporterPhone) = True Then
                            TextBoxReadWrite(ReporterPhone_Textbox)
                            ReporterPhone_Textbox.ToolTip = "Please enter a reporter phone"
                        Else
                            TextBoxReadOnly(ReporterPhone_Textbox)
                            ReporterPhone_Textbox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ReporterFax) = True Then
                            TextBoxReadWrite(ReporterFax_Textbox)
                            ReporterFax_Textbox.ToolTip = "Please enter a reporter fax"
                        Else
                            TextBoxReadOnly(ReporterFax_Textbox)
                            ReporterFax_Textbox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ReporterEmail) = True Then
                            TextBoxReadWrite(ReporterEmail_Textbox)
                            ReporterEmail_Textbox.ToolTip = "Please enter a reporter email"
                        Else
                            TextBoxReadOnly(ReporterEmail_Textbox)
                            ReporterEmail_Textbox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ExpeditedReportingRequired_ID) = True Then
                            DropDownListEnabled(ExpeditedReportingRequired_DropDownList)
                            ExpeditedReportingRequired_DropDownList.ToolTip = "Please specify whether expedited reporting is required for this report"
                        Else
                            DropDownListDisabled(ExpeditedReportingRequired_DropDownList)
                            ExpeditedReportingRequired_DropDownList.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ExpeditedReportingDone_ID) = True Then
                            DropDownListEnabled(ExpeditedReportingDone_DropDownList)
                            ExpeditedReportingDone_DropDownList.ToolTip = "Please specify whether expedited reporting was done for this report"
                        Else
                            DropDownListDisabled(ExpeditedReportingDone_DropDownList)
                            ExpeditedReportingDone_DropDownList.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ExpeditedReportingDate) = True Then
                            TextBoxReadWrite(ExpeditedReportingDate_Textbox)
                            ExpeditedReportingDate_Textbox.ToolTip = "Please enter the date expedited reporting was carried out for this report"
                        Else
                            TextBoxReadOnly(ExpeditedReportingDate_Textbox)
                            ExpeditedReportingDate_Textbox.ToolTip = CannotEditControlText
                        End If
                    Else
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
                ElseIf Delete = True Then
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, Delete) = True Then
                        AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                        DropDownListDisabled(ReportType_DropDownList)
                        DropDownListDisabled(ReportStatus_DropDownList)
                        TextBoxReadOnly(ReportReceived_TextBox)
                        TextBoxReadOnly(ReportDue_TextBox)
                        DropDownListDisabled(ReportComplexity_DropDownList)
                        DropDownListDisabled(ReportSource_DropDownList)
                        TextBoxReadOnly(ReporterName_Textbox)
                        TextBoxReadOnly(ReporterAddress_Textbox)
                        TextBoxReadOnly(ReporterPhone_Textbox)
                        TextBoxReadOnly(ReporterFax_Textbox)
                        TextBoxReadOnly(ReporterEmail_Textbox)
                        DropDownListDisabled(ExpeditedReportingRequired_DropDownList)
                        DropDownListDisabled(ExpeditedReportingDone_DropDownList)
                        TextBoxReadOnly(ExpeditedReportingDate_Textbox)
                    Else
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
                End If
                'Populate ReportType_DropDownList
                ReportType_DropDownList.DataSource = CreateDropDownListDatatable(tables.ReportTypes)
                ReportType_DropDownList.DataValueField = "ID"
                ReportType_DropDownList.DataTextField = "Name"
                ReportType_DropDownList.DataBind()
                'Populate ReportStatus_DropDownList with statuses the current user can update the current report to
                ReportStatus_DropDownList.DataSource = CreateUpdateToStatusesDatatable(tables.ReportStatuses, CurrentReport_ID)
                ReportStatus_DropDownList.DataValueField = "Status_ID"
                ReportStatus_DropDownList.DataTextField = "Status_Name"
                ReportStatus_DropDownList.DataBind()
                'Populate ReportComplexity_DropDownList
                ReportComplexity_DropDownList.DataSource = CreateDropDownListDatatable(tables.ReportComplexities)
                ReportComplexity_DropDownList.DataValueField = "ID"
                ReportComplexity_DropDownList.DataTextField = "Name"
                ReportComplexity_DropDownList.DataBind()
                'Populate ReportSource_DropDownList
                ReportSource_DropDownList.DataSource = CreateDropDownListDatatable(tables.ReportSources)
                ReportSource_DropDownList.DataValueField = "ID"
                ReportSource_DropDownList.DataTextField = "Name"
                ReportSource_DropDownList.DataBind()
                'Populate ExpeditedReportingRequired_DropDownList
                ExpeditedReportingRequired_DropDownList.DataSource = CreateDropDownListDatatable(tables.ExpeditedReportingRequired)
                ExpeditedReportingRequired_DropDownList.DataValueField = "ID"
                ExpeditedReportingRequired_DropDownList.DataTextField = "Name"
                ExpeditedReportingRequired_DropDownList.DataBind()
                'Populate ExpeditedReportingDone_DropDownList
                ExpeditedReportingDone_DropDownList.DataSource = CreateDropDownListDatatable(tables.ExpeditedReportingDone)
                ExpeditedReportingDone_DropDownList.DataValueField = "ID"
                ExpeditedReportingDone_DropDownList.DataTextField = "Name"
                ExpeditedReportingDone_DropDownList.DataBind()
                'Populate controls and store values as present in the database at edit page load to use when checking for database update conflicts (see save button click event)
                Dim AtEditPageLoadReadCommand As New SqlCommand("SELECT Reports.ID AS Report_ID, ReportType_ID, CASE WHEN ReportStatus_ID IS NULL THEN 0 ELSE ReportStatus_ID END AS ReportStatus_ID, Received AS Report_Received, CASE WHEN Due IS NULL THEN '' ELSE Due END AS Report_Due, CASE WHEN ReportComplexity_ID IS NULL THEN 0 ELSE ReportComplexity_ID END AS ReportComplexity_ID, CASE WHEN ReportSource_ID IS NULL THEN 0 ELSE ReportSource_ID END AS ReportSource_ID, CASE WHEN ReporterName IS NULL THEN '' ELSE ReporterName END AS ReporterName, CASE WHEN ReporterAddress IS NULL THEN '' ELSE ReporterAddress END AS ReporterAddress, CASE WHEN ReporterPhone IS NULL THEN '' ELSE ReporterPhone END AS ReporterPhone, CASE WHEN ReporterFax IS NULL THEN '' ELSE ReporterFax END AS ReporterFax, CASE WHEN ReporterEmail IS NULL THEN '' ELSE ReporterEmail END AS ReporterEmail, CASE WHEN ExpeditedReportingRequired_ID IS NULL THEN 0 ELSE ExpeditedReportingRequired_ID END AS ExpeditedReportingRequired_ID, CASE WHEN ExpeditedReportingDone_ID IS NULL THEN 0 ELSE ExpeditedReportingDone_ID END AS ExpeditedReportingDone_ID, CASE WHEN ExpeditedReportingDate IS NULL THEN '' ELSE ExpeditedReportingDate END AS ExpeditedReportingDate FROM Reports LEFT JOIN ReportTypes ON Reports.ReportType_ID = ReportTypes.ID LEFT JOIN ReportStatuses ON Reports.ReportStatus_ID = ReportStatuses.ID LEFT JOIN ReportComplexities ON Reports.ReportComplexity_ID = ReportComplexities.ID LEFT JOIN ReportSources ON Reports.ReportSource_ID = ReportSources.ID LEFT JOIN ExpeditedReportingRequired ON Reports.ExpeditedReportingRequired_ID = ExpeditedReportingRequired.ID LEFT JOIN ExpeditedReportingDone ON Reports.ExpeditedReportingDone_ID = ExpeditedReportingDone.ID WHERE Reports.ID = @CurrentReport_ID", Connection)
                AtEditPageLoadReadCommand.Parameters.AddWithValue("@CurrentReport_Id", CurrentReport_ID)
                Try
                    Connection.Open()
                    Dim AtEditPageLoadReader As SqlDataReader = AtEditPageLoadReadCommand.ExecuteReader()
                    While AtEditPageLoadReader.Read()
                        ReportType_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(1)
                        AtEditPageLoad_ReportType_HiddenField.Value = AtEditPageLoadReader.GetInt32(1)
                        ReportStatus_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(2)
                        AtEditPageLoad_ReportStatus_HiddenField.Value = AtEditPageLoadReader.GetInt32(2)
                        ReportReceived_TextBox.Text = SqlDateDisplay(AtEditPageLoadReader.GetDateTime(3))
                        AtEditPageLoad_ReportReceived_HiddenField.Value = AtEditPageLoadReader.GetDateTime(3)
                        ReportDue_TextBox.Text = SqlDateDisplay(AtEditPageLoadReader.GetDateTime(4))
                        AtEditPageLoad_ReportDue_HiddenField.Value = AtEditPageLoadReader.GetDateTime(4)
                        ReportComplexity_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(5)
                        AtEditPageLoad_ReportComplexity_HiddenField.Value = AtEditPageLoadReader.GetInt32(5)
                        ReportSource_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(6)
                        AtEditPageLoad_ReportSource_HiddenField.Value = AtEditPageLoadReader.GetInt32(6)
                        ReporterName_Textbox.Text = AtEditPageLoadReader.GetString(7)
                        AtEditPageLoad_ReporterName_HiddenField.Value = AtEditPageLoadReader.GetString(7)
                        ReporterAddress_Textbox.Text = AtEditPageLoadReader.GetString(8)
                        AtEditPageLoad_ReporterAddress_HiddenField.Value = AtEditPageLoadReader.GetString(8)
                        ReporterPhone_Textbox.Text = AtEditPageLoadReader.GetString(9)
                        AtEditPageLoad_ReporterPhone_HiddenField.Value = AtEditPageLoadReader.GetString(9)
                        ReporterFax_Textbox.Text = AtEditPageLoadReader.GetString(10)
                        AtEditPageLoad_ReporterFax_HiddenField.Value = AtEditPageLoadReader.GetString(10)
                        ReporterEmail_Textbox.Text = AtEditPageLoadReader.GetString(11)
                        AtEditPageLoad_ReporterEmail_HiddenField.Value = AtEditPageLoadReader.GetString(11)
                        ExpeditedReportingRequired_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(12)
                        AtEditPageLoad_ExpeditedReportingRequired_HiddenField.Value = AtEditPageLoadReader.GetInt32(12)
                        ExpeditedReportingDone_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(13)
                        AtEditPageLoad_ExpeditedReportingDone_HiddenField.Value = AtEditPageLoadReader.GetInt32(13)
                        ExpeditedReportingDate_Textbox.Text = SqlDateDisplay(AtEditPageLoadReader.GetDateTime(14))
                        AtEditPageLoad_ExpeditedReportingDate_HiddenField.Value = AtEditPageLoadReader.GetDateTime(14)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            Else 'Redirect user to login if he/she is not logged in
                If Delete = False Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditReport.aspx?ReportID=" & CurrentReport_ID)
                ElseIf Delete = True Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditReport.aspx?ReportID=" & CurrentReport_ID & "&Delete=True")
                End If
            End If
        End If
    End Sub

    Protected Sub ReturnToICSROverview() Handles Cancel_Button.Click, ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub ReportType_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If ReportType_DropDownList.SelectedValue <> 0 Then 'If a report type was selected
            Dim CurrentReport_ID As Integer = ReportID_HiddenField.Value
            Dim CurrentICSR_ID As Integer = ParentID(tables.ICSRs, tables.Reports, fields.ICSR_ID, CurrentReport_ID)
            Dim ReporTypeMatchesOtherReportType As Boolean = False
            Dim OtherReportTypesReadCommand As New SqlCommand("SELECT ReportType_ID FROM Reports WHERE ID <> @CurrentReport_ID AND ICSR_ID = @CurrentICSR_ID", Connection)
            OtherReportTypesReadCommand.Parameters.AddWithValue("@CurrentReport_ID", ReportID_HiddenField.Value)
            OtherReportTypesReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            Try
                Connection.Open()
                Dim OtherReportTypesReader As SqlDataReader = OtherReportTypesReadCommand.ExecuteReader()
                While OtherReportTypesReader.Read()
                    If ReportType_DropDownList.SelectedValue = OtherReportTypesReader.GetInt32(0) Then
                        ReporTypeMatchesOtherReportType = True
                    End If
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            If ReporTypeMatchesOtherReportType = False Then 'If selected report type is not already taken
                ReportType_DropDownList.CssClass = "form-control alert-success"
                ReportType_DropDownList.ToolTip = String.Empty
                args.IsValid = True
            Else 'If selected report type is already taken by another report
                ReportType_DropDownList.CssClass = "form-control alert-danger"
                ReportType_DropDownList.ToolTip = "The report type you have selected has already been selected for this ICSR. Please select another report type"
                args.IsValid = False
            End If
        Else 'if no report type was selected
            ReportType_DropDownList.CssClass = "form-control alert-danger"
            ReportType_DropDownList.ToolTip = "Please ensure you are selecting a valid report type"
            args.IsValid = False
        End If
    End Sub

    Protected Sub ReportStatus_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        ReportStatus_DropDownList.CssClass = "form-control alert-success"
        ReportStatus_DropDownList.ToolTip = String.Empty
    End Sub

    Protected Sub ReportReceived_TextBox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If ReportReceived_TextBox.Text <> String.Empty Then 'If a date was entered
            Try 'If a valid date was entered
                DateTime.Parse(ReportReceived_TextBox.Text)
                ReportReceived_TextBox.CssClass = "form-control alert-success"
                ReportReceived_TextBox.ToolTip = String.Empty
                args.IsValid = True
            Catch ex As FormatException 'If an invalid date was entered
                ReportReceived_TextBox.CssClass = "form-control alert-danger"
                ReportReceived_TextBox.ToolTip = "Please ensure you are entering a valid date"
                args.IsValid = False
            End Try
        Else 'If no date was entered
            ReportReceived_TextBox.CssClass = "form-control alert-danger"
            ReportReceived_TextBox.ToolTip = "Please ensure you are entering a date"
            args.IsValid = False
        End If
    End Sub

    Protected Sub ReportDue_TextBox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If ReportDue_TextBox.Text <> String.Empty Then 'If a date was entered
            Try 'If a valid date was entered
                DateTime.Parse(ReportDue_TextBox.Text)
                ReportDue_TextBox.CssClass = "form-control alert-success"
                ReportDue_TextBox.ToolTip = String.Empty
                args.IsValid = True
            Catch ex As FormatException 'If an invalid date was entered
                ReportDue_TextBox.CssClass = "form-control alert-danger"
                ReportDue_TextBox.ToolTip = "Please ensure you are entering a valid date"
                args.IsValid = False
            End Try
        Else 'If no date was entered
            ReportDue_TextBox.CssClass = "form-control alert-success"
            ReportDue_TextBox.ToolTip = String.Empty
            args.IsValid = True
        End If
    End Sub

    Protected Sub ReportComplexity_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        ReportComplexity_DropDownList.CssClass = "form-control alert-success"
        ReportComplexity_DropDownList.ToolTip = String.Empty
    End Sub

    Protected Sub ReportSource_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        ReportSource_DropDownList.CssClass = "form-control alert-success"
        ReportSource_DropDownList.ToolTip = String.Empty
    End Sub

    Protected Sub ReporterName_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        ReporterName_Textbox.CssClass = "form-control alert-success"
        ReporterName_Textbox.ToolTip = String.Empty
    End Sub

    Protected Sub ReporterAddress_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        ReporterAddress_Textbox.CssClass = "form-control alert-success"
        ReporterAddress_Textbox.ToolTip = String.Empty
    End Sub

    Protected Sub ReporterPhone_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If ReporterPhone_Textbox.Text = String.Empty Then
            ReporterPhone_Textbox.CssClass = "form-control alert-success"
            ReporterPhone_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf Regex.IsMatch(ReporterPhone_Textbox.Text, "^[0-9-+]+$") Then
            ReporterPhone_Textbox.CssClass = "form-control alert-success"
            ReporterPhone_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            ReporterPhone_Textbox.CssClass = "form-control alert-danger"
            ReporterPhone_Textbox.ToolTip = "Please make sure you are entering a valid phone number (format xxxx-xxxx-xxxxxxxx)"
            args.IsValid = False
        End If
    End Sub

    Protected Sub ReporterFax_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If ReporterFax_Textbox.Text = String.Empty Then
            ReporterFax_Textbox.CssClass = "form-control alert-success"
            ReporterFax_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf Regex.IsMatch(ReporterFax_Textbox.Text, "^[0-9-+]+$") Then
            ReporterFax_Textbox.CssClass = "form-control alert-success"
            ReporterFax_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            ReporterFax_Textbox.CssClass = "form-control alert-danger"
            ReporterFax_Textbox.ToolTip = "Please make sure you are entering a valid fax number (format xxxx-xxxx-xxxxxxxx)"
            args.IsValid = False
        End If
    End Sub

    Protected Sub ReporterEmail_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If ReporterEmail_Textbox.Text <> String.Empty Then 'If an email address was entered
            If ReporterEmail_Textbox.Text Like "*[@]*?.[a-z]*" Then 'If a valid email address was entered
                ReporterEmail_Textbox.CssClass = "form-control alert-success"
                ReporterEmail_Textbox.ToolTip = String.Empty
                args.IsValid = True
            Else 'If an invalid email address was entered
                ReporterEmail_Textbox.CssClass = "form-control alert-danger"
                ReporterEmail_Textbox.ToolTip = "Please ensure you are entering a valid enail address"
                args.IsValid = True
            End If
        Else 'If no email address was entered
            ReporterEmail_Textbox.CssClass = "form-control alert-success"
            ReporterEmail_Textbox.ToolTip = String.Empty
            args.IsValid = True
        End If
    End Sub

    Protected Sub ExpeditedReportingConsistency_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim ValidationResult As String = ExpediteReportingConsistencyValidator(ExpeditedReportingRequired_DropDownList, ExpeditedReportingDone_DropDownList, ExpeditedReportingDate_Textbox)
        If ValidationResult = "Expedited reporting required, not done, no date entered" Then
            ExpeditedReportingRequired_DropDownList.CssClass = CssClassSuccess
            ExpeditedReportingRequired_DropDownList.ToolTip = String.Empty
            ExpeditedReportingDone_DropDownList.CssClass = CssClassSuccess
            ExpeditedReportingDone_DropDownList.ToolTip = String.Empty
            ExpeditedReportingDate_Textbox.CssClass = CssClassSuccess
            ExpeditedReportingDate_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf ValidationResult = "Expedited reporting required, done, valid date entered" Then 'If expedited reporting is required and was done and a valid expedited reporting date was entered
            ExpeditedReportingRequired_DropDownList.CssClass = CssClassSuccess
            ExpeditedReportingRequired_DropDownList.ToolTip = String.Empty
            ExpeditedReportingDone_DropDownList.CssClass = CssClassSuccess
            ExpeditedReportingDone_DropDownList.ToolTip = String.Empty
            ExpeditedReportingDate_Textbox.CssClass = CssClassSuccess
            ExpeditedReportingDate_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf ValidationResult = "Expedited reporting required, done, invalid date entered" Then 'If expedited reporting is required and was done and an in valid expedited reporting date was entered
            ExpeditedReportingRequired_DropDownList.CssClass = CssClassSuccess
            ExpeditedReportingRequired_DropDownList.ToolTip = String.Empty
            ExpeditedReportingDone_DropDownList.CssClass = CssClassSuccess
            ExpeditedReportingDone_DropDownList.ToolTip = String.Empty
            ExpeditedReportingDate_Textbox.CssClass = CssClassFailure
            ExpeditedReportingDate_Textbox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        ElseIf ValidationResult = "Expedited reporting not required, not done, no date entered" Then 'If expedited reporting is not required, was not done and no expedited reporting date was entered
            ExpeditedReportingRequired_DropDownList.CssClass = CssClassSuccess
            ExpeditedReportingRequired_DropDownList.ToolTip = String.Empty
            ExpeditedReportingDone_DropDownList.CssClass = CssClassSuccess
            ExpeditedReportingDone_DropDownList.ToolTip = String.Empty
            ExpeditedReportingDate_Textbox.CssClass = CssClassSuccess
            ExpeditedReportingDate_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf ValidationResult = "Inconsistency with valid date" Then 'If expedited reporting is not required, but was done and/or there is an inconsistency between expedited reporting done and expedited reporting date entries
            ExpeditedReportingRequired_DropDownList.CssClass = CssClassFailure
            ExpeditedReportingRequired_DropDownList.ToolTip = ExpeditedReportingConsistencyValidationFailToolTip
            ExpeditedReportingDone_DropDownList.CssClass = CssClassFailure
            ExpeditedReportingDone_DropDownList.ToolTip = ExpeditedReportingConsistencyValidationFailToolTip
            ExpeditedReportingDate_Textbox.CssClass = CssClassFailure
            ExpeditedReportingDate_Textbox.ToolTip = ExpeditedReportingConsistencyValidationFailToolTip
            args.IsValid = False
        ElseIf ValidationResult = "Inconsistency with invalid date" Then
            ExpeditedReportingRequired_DropDownList.CssClass = CssClassFailure
            ExpeditedReportingRequired_DropDownList.ToolTip = ExpeditedReportingConsistencyValidationFailToolTip
            ExpeditedReportingDone_DropDownList.CssClass = CssClassFailure
            ExpeditedReportingDone_DropDownList.ToolTip = ExpeditedReportingConsistencyValidationFailToolTip
            ExpeditedReportingDate_Textbox.CssClass = CssClassFailure
            ExpeditedReportingDate_Textbox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click, ConfirmDeletion_Button.Click
        If Page.IsValid = True Then
            Dim CurrentReport_ID As Integer = ReportID_HiddenField.Value
            'Retrieve values as present in database at edit page load and store in variables to use when checking for database update conflicts (see page load event)
            Dim CurrentICSR_ID = ParentID(tables.ICSRs, tables.Reports, fields.ICSR_ID, CurrentReport_ID)
            Dim AtEditPageLoad_ReportType_ID As Integer = AtEditPageLoad_ReportType_HiddenField.Value
            Dim AtEditPageLoad_ReportStatus_ID As Integer = AtEditPageLoad_ReportStatus_HiddenField.Value
            Dim AtEditPageLoad_ReportReceived As DateTime = TryCType(AtEditPageLoad_ReportReceived_HiddenField.Value, InputTypes.Date)
            Dim AtEditPageLoad_ReportDue As DateTime = TryCType(AtEditPageLoad_ReportDue_HiddenField.Value, InputTypes.Date)
            Dim AtEditPageLoad_ReportComplexity_ID As Integer = AtEditPageLoad_ReportComplexity_HiddenField.Value
            Dim AtEditPageLoad_ReportSource_ID As Integer = AtEditPageLoad_ReportSource_HiddenField.Value
            Dim AtEditPageLoad_ReporterName As String = AtEditPageLoad_ReporterName_HiddenField.Value
            Dim AtEditPageLoad_ReporterAddress As String = AtEditPageLoad_ReporterAddress_HiddenField.Value
            Dim AtEditPageLoad_ReporterPhone As String = AtEditPageLoad_ReporterPhone_HiddenField.Value
            Dim AtEditPageLoad_ReporterFax As String = AtEditPageLoad_ReporterFax_HiddenField.Value
            Dim AtEditPageLoad_ReporterEmail As String = AtEditPageLoad_ReporterEmail_HiddenField.Value
            Dim AtEditPageLoad_ExpeditedReportingRequired_ID As Integer = AtEditPageLoad_ExpeditedReportingRequired_HiddenField.Value
            Dim AtEditPageLoad_ExpeditedReportingDone_ID As Integer = AtEditPageLoad_ExpeditedReportingDone_HiddenField.Value
            Dim AtEditPageLoad_ExpeditedReportingDate As Date = TryCType(AtEditPageLoad_ExpeditedReportingDate_HiddenField.Value, InputTypes.Date)
            'Store report values as present in database when save button is clicked in variables
            Dim AtSaveButtonClickReportReadCommand As New SqlCommand("SELECT CASE WHEN ReportType_ID IS NULL THEN 0 ELSE ReportType_ID END AS ReportType_ID, CASE WHEN ReportStatus_ID IS NULL THEN 0 ELSE ReportStatus_ID END AS ReportStatus_ID, CASE WHEN Received IS NULL THEN '' ELSE Received END AS Received, CASE WHEN Due IS NULL THEN '' ELSE Due END AS Due, CASE WHEN ReportComplexity_ID IS NULL THEN 0 ELSE ReportComplexity_ID END AS ReportComplexity_ID, CASE WHEN ReportSource_ID IS NULL THEN 0 ELSE ReportSource_ID END AS ReportSource_ID, CASE WHEN ReporterName IS NULL THEN '' ELSE ReporterName END AS ReporterName, CASE WHEN ReporterAddress IS NULL THEN '' ELSE ReporterAddress END AS ReporterAddress, CASE WHEN ReporterPhone IS NULL THEN '' ELSE ReporterPhone END AS ReporterPhone, CASE WHEN ReporterFax IS NULL THEN '' ELSE ReporterFax END AS ReporterFax, CASE WHEN ReporterEmail IS NULL THEN '' ELSE ReporterEmail END AS ReporterEmail, CASE WHEN ExpeditedReportingRequired_ID IS NULL THEN 0 ELSE ExpeditedReportingRequired_ID END AS ExpeditedReportingRequired_ID, CASE WHEN ExpeditedReportingDone_ID IS NULL THEN 0 ELSE ExpeditedReportingDone_ID END AS ExpeditedReportingDone_ID, CASE WHEN ExpeditedReportingDate IS NULL THEN '' ELSE ExpeditedReportingDate END AS ExpeditedReportingDate FROM Reports WHERE Reports.ID = @CurrentReport_ID", Connection)
            AtSaveButtonClickReportReadCommand.Parameters.AddWithValue("@CurrentReport_ID", CurrentReport_ID)
            Dim AtSaveButtonClick_ReportType_ID As Integer = Nothing
            Dim AtSaveButtonClick_ReportStatus_ID As Integer = Nothing
            Dim AtSaveButtonClick_ReportReceived As DateTime = DateTime.MinValue
            Dim AtSaveButtonClick_ReportDue As DateTime = DateTime.MinValue
            Dim AtSaveButtonClick_ReportComplexity_ID As Integer = Nothing
            Dim AtSaveButtonClick_ReportSource_ID As Integer = Nothing
            Dim AtSaveButtonClick_ReporterName As String = String.Empty
            Dim AtSaveButtonClick_ReporterAddress As String = String.Empty
            Dim AtSaveButtonClick_ReporterPhone As String = String.Empty
            Dim AtSaveButtonClick_ReporterFax As String = String.Empty
            Dim AtSaveButtonClick_ReporterEmail As String = String.Empty
            Dim AtSaveButtonClick_ExpeditedReportingRequired_ID As Integer = Nothing
            Dim AtSaveButtonClick_ExpeditedReportingDone_ID As Integer = Nothing
            Dim AtSaveButtonClick_ExpeditedReportingDate As DateTime = DateTime.MinValue
            Try
                Connection.Open()
                Dim AtSaveButtonClickReportReader As SqlDataReader = AtSaveButtonClickReportReadCommand.ExecuteReader()
                While AtSaveButtonClickReportReader.Read()
                    AtSaveButtonClick_ReportType_ID = AtSaveButtonClickReportReader.GetInt32(0)
                    AtSaveButtonClick_ReportStatus_ID = AtSaveButtonClickReportReader.GetInt32(1)
                    AtSaveButtonClick_ReportReceived = DateOrDateMinValue(AtSaveButtonClickReportReader.GetDateTime(2))
                    AtSaveButtonClick_ReportDue = DateOrDateMinValue(AtSaveButtonClickReportReader.GetDateTime(3))
                    AtSaveButtonClick_ReportComplexity_ID = AtSaveButtonClickReportReader.GetInt32(4)
                    AtSaveButtonClick_ReportSource_ID = AtSaveButtonClickReportReader.GetInt32(5)
                    AtSaveButtonClick_ReporterName = AtSaveButtonClickReportReader.GetString(6)
                    AtSaveButtonClick_ReporterAddress = AtSaveButtonClickReportReader.GetString(7)
                    AtSaveButtonClick_ReporterPhone = AtSaveButtonClickReportReader.GetString(8)
                    AtSaveButtonClick_ReporterFax = AtSaveButtonClickReportReader.GetString(9)
                    AtSaveButtonClick_ReporterEmail = AtSaveButtonClickReportReader.GetString(10)
                    AtSaveButtonClick_ExpeditedReportingRequired_ID = AtSaveButtonClickReportReader.GetInt32(11)
                    AtSaveButtonClick_ExpeditedReportingDone_ID = AtSaveButtonClickReportReader.GetInt32(12)
                    AtSaveButtonClick_ExpeditedReportingDate = DateOrDateMinValue(AtSaveButtonClickReportReader.GetDateTime(13))
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Check for discrepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked
            Dim DiscrepancyString As String = String.Empty
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ReportType_ID, AtSaveButtonClick_ReportType_ID, "Report Type")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ReportStatus_ID, AtSaveButtonClick_ReportStatus_ID, "Report Status")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ReportReceived, AtSaveButtonClick_ReportReceived, "Report Received")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ReportDue, AtSaveButtonClick_ReportDue, "Report Due")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ReportComplexity_ID, AtSaveButtonClick_ReportComplexity_ID, "Report Complexity")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ReportSource_ID, AtSaveButtonClick_ReportSource_ID, "Report Source")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ReporterName, AtSaveButtonClick_ReporterName, "Reporter Name")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ReporterAddress, AtSaveButtonClick_ReporterAddress, "Reporter Address")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ReporterPhone, AtSaveButtonClick_ReporterPhone, "Reporter Phone")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ReporterFax, AtSaveButtonClick_ReporterFax, "Reporter Fax")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ReporterEmail, AtSaveButtonClick_ReporterEmail, "Reporter Email")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ExpeditedReportingRequired_ID, AtSaveButtonClick_ExpeditedReportingRequired_ID, "Expedited Reporting Required")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ExpeditedReportingDone_ID, AtSaveButtonClick_ExpeditedReportingDone_ID, "Expedited Reporting Done")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ExpeditedReportingDate, AtSaveButtonClick_ExpeditedReportingDate, "Expedite Reporting Date")
            'If Discprepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked are found, show warning and abort update
            If DiscrepancyString <> String.Empty Then
                AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                Status_Label.Style.Add("text-align", "left")
                Status_Label.Style.Add("height", "auto")
                Status_Label.Text = DiscrepancyStringIntro & DiscrepancyString & DiscrepancyStringOutro
                Status_Label.CssClass = "form-control alert-danger"
                DropDownListDisabled(ReportType_DropDownList)
                DropDownListDisabled(ReportStatus_DropDownList)
                TextBoxReadOnly(ReportReceived_TextBox)
                TextBoxReadOnly(ReportDue_TextBox)
                DropDownListDisabled(ReportComplexity_DropDownList)
                DropDownListDisabled(ReportSource_DropDownList)
                TextBoxReadOnly(ReporterName_Textbox)
                TextBoxReadOnly(ReporterAddress_Textbox)
                TextBoxReadOnly(ReporterPhone_Textbox)
                TextBoxReadOnly(ReporterFax_Textbox)
                TextBoxReadOnly(ReporterEmail_Textbox)
                DropDownListDisabled(ExpeditedReportingRequired_DropDownList)
                DropDownListDisabled(ExpeditedReportingDone_DropDownList)
                TextBoxReadOnly(ExpeditedReportingDate_Textbox)
                Exit Sub
            End If
            'If no discrepancies were found between database contents as present when edit page was loaded and database contents as present when save button is clicked, write updates to database
            Dim UpdateCommand As New SqlCommand
            UpdateCommand.Connection = Connection
            If sender Is SaveUpdates_Button Then
                UpdateCommand.CommandText = "Update Reports SET ReportType_ID = (CASE WHEN @ReportType_ID = 0 THEN NULL ELSE @ReportType_ID END), ReportStatus_ID = (CASE WHEN @ReportStatus_ID = 0 THEN NULL ELSE @ReportStatus_ID END), Received = (CASE WHEN @Received = '' THEN NULL ELSE @Received END), Due = (CASE WHEN @Due = '' THEN NULL ELSE @Due END), ReportComplexity_ID = (CASE WHEN @ReportComplexity_ID = 0 THEN NULL ELSE @ReportComplexity_ID END), ReportSource_ID = (CASE WHEN @ReportSource_ID = 0 THEN NULL ELSE @ReportSource_ID END), ReporterName = (CASE WHEN @ReporterName = '' THEN NULL ELSE @ReporterName END), ReporterAddress = (CASE WHEN @ReporterAddress = '' THEN NULL ELSE @ReporterAddress END), ReporterPhone = (CASE WHEN @ReporterPhone = '' THEN NULL ELSE @ReporterPhone END), ReporterFax = (CASE WHEN @ReporterFax = '' THEN NULL ELSE @ReporterFax END), ReporterEmail = (CASE WHEN @ReporterEmail = '' THEN NULL ELSE @ReporterEmail END), ExpeditedReportingRequired_ID = (CASE WHEN @ExpeditedReportingRequired_ID = 0 THEN NULL ELSE @ExpeditedReportingRequired_ID END), ExpeditedReportingDone_ID = (CASE WHEN @ExpeditedReportingDone_ID = 0 THEN NULL ELSE @ExpeditedReportingDone_ID END), ExpeditedReportingDate = (CASE WHEN @ExpeditedReportingDate = '' THEN NULL ELSE @ExpeditedReportingDate END) WHERE ID = @CurrentReport_ID"
                UpdateCommand.Parameters.AddWithValue("@ReportType_ID", ReportType_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@ReportStatus_ID", ReportStatus_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@Received", DateStringOrEmpty(ReportReceived_TextBox.Text.Trim))
                UpdateCommand.Parameters.AddWithValue("@Due", DateStringOrEmpty(ReportDue_TextBox.Text.Trim))
                UpdateCommand.Parameters.AddWithValue("@ReportComplexity_ID", ReportComplexity_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@ReportSource_ID", ReportSource_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@ReporterName", ReporterName_Textbox.Text.Trim)
                UpdateCommand.Parameters.AddWithValue("@ReporterAddress", ReporterAddress_Textbox.Text.Trim)
                UpdateCommand.Parameters.AddWithValue("@ReporterPhone", ReporterPhone_Textbox.Text.Trim)
                UpdateCommand.Parameters.AddWithValue("@ReporterFax", ReporterFax_Textbox.Text.Trim)
                UpdateCommand.Parameters.AddWithValue("@ReporterEmail", ReporterEmail_Textbox.Text.Trim)
                UpdateCommand.Parameters.AddWithValue("@ExpeditedReportingRequired_ID", ExpeditedReportingRequired_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@ExpeditedReportingDone_ID", ExpeditedReportingDone_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@ExpeditedReportingDate", DateStringOrEmpty(ExpeditedReportingDate_Textbox.Text.Trim))
                UpdateCommand.Parameters.AddWithValue("@CurrentReport_ID", CurrentReport_ID)
            ElseIf sender Is ConfirmDeletion_Button Then
                UpdateCommand.CommandText = "DELETE FROM Reports WHERE ID = @CurrentReport_ID"
                UpdateCommand.Parameters.AddWithValue("@CurrentReport_ID", CurrentReport_ID)
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
            Dim Updated_ReportType_ID As Integer = Nothing
            Dim Updated_ReportStatus_ID As Integer = Nothing
            Dim Updated_ReportReceived As DateTime = DateTime.MinValue
            Dim Updated_ReportDue As DateTime = DateTime.MinValue
            Dim Updated_ReportComplexity_ID As Integer = Nothing
            Dim Updated_ReportSource_ID As Integer = Nothing
            Dim Updated_ReporterName As String = String.Empty
            Dim Updated_ReporterAddress As String = String.Empty
            Dim Updated_ReporterPhone As String = String.Empty
            Dim Updated_ReporterFax As String = String.Empty
            Dim Updated_ReporterEmail As String = String.Empty
            Dim Updated_ExpeditedReportingRequired_ID As Integer = Nothing
            Dim Updated_ExpeditedReportingDone_ID As Integer = Nothing
            Dim Updated_ExpeditedReportingDate As DateTime = DateTime.MinValue
            If sender Is SaveUpdates_Button Then
                Dim UpdatedReadCommand As New SqlCommand("SELECT CASE WHEN ReportType_ID IS NULL THEN 0 ELSE ReportType_ID END AS ReportType_ID, CASE WHEN ReportStatus_ID IS NULL THEN 0 ELSE ReportStatus_ID END AS ReportStatus_ID, CASE WHEN Received IS NULL THEN '' ELSE Received END AS Received, CASE WHEN Due IS NULL THEN '' ELSE Due END AS Due, CASE WHEN ReportComplexity_ID IS NULL THEN 0 ELSE ReportComplexity_ID END AS ReportComplexity_ID, CASE WHEN ReportSource_ID IS NULL THEN 0 ELSE ReportSource_ID END AS ReportSource_ID, CASE WHEN ReporterName IS NULL THEN '' ELSE ReporterName END AS ReporterName, CASE WHEN ReporterAddress IS NULL THEN '' ELSE ReporterAddress END AS ReporterAddress, CASE WHEN ReporterPhone IS NULL THEN '' ELSE ReporterPhone END AS ReporterPhone, CASE WHEN ReporterFax IS NULL THEN '' ELSE ReporterFax END AS ReporterFax, CASE WHEN ReporterEmail IS NULL THEN '' ELSE ReporterEmail END AS ReporterEmail, CASE WHEN ExpeditedReportingRequired_ID IS NULL THEN 0 ELSE ExpeditedReportingRequired_ID END AS ExpeditedReportingRequired_ID, CASE WHEN ExpeditedReportingDone_ID IS NULL THEN 0 ELSE ExpeditedReportingDone_ID END AS ExpeditedReportingDone_ID, CASE WHEN ExpeditedReportingDate IS NULL THEN '' ELSE ExpeditedReportingDate END AS ExpeditedReportingDate FROM Reports WHERE Reports.ID = @CurrentReport_ID", Connection)
                UpdatedReadCommand.Parameters.AddWithValue("@CurrentReport_ID", CurrentReport_ID)
                Try
                    Connection.Open()
                    Dim UpdatedReader As SqlDataReader = UpdatedReadCommand.ExecuteReader()
                    While UpdatedReader.Read()
                        Updated_ReportType_ID = UpdatedReader.GetInt32(0)
                        Updated_ReportStatus_ID = UpdatedReader.GetInt32(1)
                        Updated_ReportReceived = DateOrDateMinValue(UpdatedReader.GetDateTime(2))
                        Updated_ReportDue = DateOrDateMinValue(UpdatedReader.GetDateTime(3))
                        Updated_ReportComplexity_ID = UpdatedReader.GetInt32(4)
                        Updated_ReportSource_ID = UpdatedReader.GetInt32(5)
                        Updated_ReporterName = UpdatedReader.GetString(6)
                        Updated_ReporterAddress = UpdatedReader.GetString(7)
                        Updated_ReporterPhone = UpdatedReader.GetString(8)
                        Updated_ReporterFax = UpdatedReader.GetString(9)
                        Updated_ReporterEmail = UpdatedReader.GetString(10)
                        Updated_ExpeditedReportingRequired_ID = UpdatedReader.GetInt32(11)
                        Updated_ExpeditedReportingDone_ID = UpdatedReader.GetInt32(12)
                        Updated_ExpeditedReportingDate = DateOrDateMinValue(UpdatedReader.GetDateTime(13))
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            End If
            'Compare old and new variables to generate EntryString for Case History Entry
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            If sender Is ConfirmDeletion_Button Then
                EntryString += DeleteReportIntro("Report", CurrentReport_ID)
            End If
            If Updated_ReportType_ID <> AtSaveButtonClick_ReportType_ID Then
                EntryString += HistoryEntryReferencedValue("Report", CurrentReport_ID, "Type", tables.ReportTypes, fields.Name, AtSaveButtonClick_ReportType_ID, Updated_ReportType_ID)
            End If
            If Updated_ReportStatus_ID <> AtSaveButtonClick_ReportStatus_ID Then
                EntryString += HistoryEntryReferencedValue("Report", CurrentReport_ID, "Status", tables.ReportStatuses, fields.Name, AtSaveButtonClick_ReportStatus_ID, Updated_ReportStatus_ID)
            End If
            If Updated_ReportReceived <> AtSaveButtonClick_ReportReceived Then
                EntryString += HistoryEnrtyPlainValue("Report", CurrentReport_ID, "Received Date", AtSaveButtonClick_ReportReceived, Updated_ReportReceived)
            End If
            If Updated_ReportDue <> AtSaveButtonClick_ReportDue Then
                EntryString += HistoryEnrtyPlainValue("Report", CurrentReport_ID, "Due Date", AtSaveButtonClick_ReportDue, Updated_ReportDue)
            End If
            If Updated_ReportComplexity_ID <> AtSaveButtonClick_ReportComplexity_ID Then
                EntryString += HistoryEntryReferencedValue("Report", CurrentReport_ID, "Complexity", tables.ReportComplexities, fields.Name, AtSaveButtonClick_ReportComplexity_ID, Updated_ReportComplexity_ID)
            End If
            If Updated_ReportSource_ID <> AtSaveButtonClick_ReportSource_ID Then
                EntryString += HistoryEntryReferencedValue("Report", CurrentReport_ID, "Source", tables.ReportSources, fields.Name, AtSaveButtonClick_ReportSource_ID, Updated_ReportSource_ID)
            End If
            If Updated_ReporterName <> AtSaveButtonClick_ReporterName Then
                EntryString += HistoryEnrtyPlainValue("Report", CurrentReport_ID, "Reporter Name", AtSaveButtonClick_ReporterName, Updated_ReporterName)
            End If
            If Updated_ReporterAddress <> AtSaveButtonClick_ReporterAddress Then
                EntryString += HistoryEnrtyPlainValue("Report", CurrentReport_ID, "Reporter Address", AtSaveButtonClick_ReporterAddress, Updated_ReporterAddress)
            End If
            If Updated_ReporterPhone <> AtSaveButtonClick_ReporterPhone Then
                EntryString += HistoryEnrtyPlainValue("Report", CurrentReport_ID, "Reporter Phone", AtSaveButtonClick_ReporterPhone, Updated_ReporterPhone)
            End If
            If Updated_ReporterFax <> AtSaveButtonClick_ReporterFax Then
                EntryString += HistoryEnrtyPlainValue("Report", CurrentReport_ID, "Reporter Fax", AtSaveButtonClick_ReporterFax, Updated_ReporterFax)
            End If
            If Updated_ReporterEmail <> AtSaveButtonClick_ReporterEmail Then
                EntryString += HistoryEnrtyPlainValue("Report", CurrentReport_ID, "Reporter Email", AtSaveButtonClick_ReporterEmail, Updated_ReporterEmail)
            End If
            If Updated_ExpeditedReportingRequired_ID <> AtSaveButtonClick_ExpeditedReportingRequired_ID Then
                EntryString += HistoryEntryReferencedValue("Report", CurrentReport_ID, "Expedited Reporting Required", tables.ExpeditedReportingRequired, fields.Name, AtSaveButtonClick_ExpeditedReportingRequired_ID, Updated_ExpeditedReportingRequired_ID)
            End If
            If Updated_ExpeditedReportingDone_ID <> AtSaveButtonClick_ExpeditedReportingDone_ID Then
                EntryString += HistoryEntryReferencedValue("Report", CurrentReport_ID, "Expedited Reporting Done", tables.ExpeditedReportingDone, fields.Name, AtSaveButtonClick_ExpeditedReportingDone_ID, Updated_ExpeditedReportingDone_ID)
            End If
            If Updated_ExpeditedReportingDate <> AtSaveButtonClick_ExpeditedReportingDate Then
                EntryString += HistoryEnrtyPlainValue("Report", CurrentReport_ID, "Expedited Reporting Date", AtSaveButtonClick_ExpeditedReportingDate, Updated_ExpeditedReportingDate)
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
                DropDownListDisabled(ReportType_DropDownList)
                DropDownListDisabled(ReportStatus_DropDownList)
                TextBoxReadOnly(ReportReceived_TextBox)
                TextBoxReadOnly(ReportDue_TextBox)
                DropDownListDisabled(ReportComplexity_DropDownList)
                DropDownListDisabled(ReportSource_DropDownList)
                TextBoxReadOnly(ReporterName_Textbox)
                TextBoxReadOnly(ReporterAddress_Textbox)
                TextBoxReadOnly(ReporterPhone_Textbox)
                TextBoxReadOnly(ReporterFax_Textbox)
                TextBoxReadOnly(ReporterEmail_Textbox)
                DropDownListDisabled(ExpeditedReportingRequired_DropDownList)
                DropDownListDisabled(ExpeditedReportingDone_DropDownList)
                TextBoxReadOnly(ExpeditedReportingDate_Textbox)
            ElseIf sender Is ConfirmDeletion_Button Then
                ReportType_Row.Visible = False
                ReportStatus_Row.Visible = False
                ReportReceived_Row.Visible = False
                ReportDue_Row.Visible = False
                ReportComplexity_Row.Visible = False
                ReportSource_Row.Visible = False
                ReporterName_Row.Visible = False
                ReporterAddress_Row.Visible = False
                ReporterPhone_Row.Visible = False
                ReporterFax_Row.Visible = False
                ReporterEmail_Row.Visible = False
                ExpeditedReportingRequired_Row.Visible = False
                ExpeditedReportingDone_Row.Visible = False
                ExpeditedReportingDate_Row.Visible = False
            End If
        End If
    End Sub
End Class
