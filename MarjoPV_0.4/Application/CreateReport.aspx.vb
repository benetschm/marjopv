Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_CreateReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            Dim CurrentICSR_ID As Integer = CType(ICSRID_HiddenField.Value, Integer)
            If Login_Status = True Then
                Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Add Report"
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.Create) = True Then
                    'Format Controls depending on user edit rights for each control
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ReportType_ID) = True Then
                        DropDownListEnabled(ReportType_DropDownList)
                        ReportType_DropDownList.ToolTip = "Please select a report type"
                    Else
                        DropDownListDisabled(ReportType_DropDownList)
                        ReportType_DropDownList.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.Received) = True Then
                        TextBoxReadWrite(ReportReceived_TextBox)
                        ReportReceived_TextBox.ToolTip = "Please enter the date when the report was received (format dd-MMM-yyyy)"
                    Else
                        TextBoxReadOnly(ReportReceived_TextBox)
                        ReportReceived_TextBox.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.Due) = True Then
                        TextBoxReadWrite(ReportDue_TextBox)
                        ReportDue_TextBox.ToolTip = "Please enter the date when the report is due (format dd-MMM-yyyy)"
                    Else
                        TextBoxReadOnly(ReportDue_TextBox)
                        ReportDue_TextBox.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ReportComplexity_ID) = True Then
                        DropDownListEnabled(ReportComplexity_DropDownList)
                        ReportComplexity_DropDownList.ToolTip = "Please select a report complexity"
                    Else
                        DropDownListDisabled(ReportComplexity_DropDownList)
                        ReportComplexity_DropDownList.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Reports, fields.ReportSource_ID) = True Then
                        DropDownListEnabled(ReportSource_DropDownList)
                        ReportSource_DropDownList.ToolTip = "Please select a report source"
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
                    'Hide or Unhide PopulateReporterFromLastReport_Button depending on whether there are previous reports for thge current ICSR
                    Dim CurrentICSRLastReportID As Integer = Nothing
                    Dim CurrentICSRLastReportIDReadCommand As New SqlCommand("SELECT TOP 1 ID FROM Reports WHERE ICSR_ID = @CurrentICSR_ID ORDER BY ID DESC", Connection)
                    CurrentICSRLastReportIDReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
                    Try
                        Connection.Open()
                        Dim CurrentICSRLastReportIDReader As SqlDataReader = CurrentICSRLastReportIDReadCommand.ExecuteReader()
                        While CurrentICSRLastReportIDReader.Read()
                            Try
                                CurrentICSRLastReportID = CurrentICSRLastReportIDReader.GetInt32(0)
                            Catch ex As Exception
                                CurrentICSRLastReportID = Nothing
                            End Try
                        End While
                    Catch ex As Exception
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                        Exit Sub
                    Finally
                        Connection.Close()
                    End Try
                    If CurrentICSRLastReportID <> Nothing Then
                        PopulateReporterFromLastReport_Button.Visible = True
                    Else
                        PopulateReporterFromLastReport_Button.Visible = False
                    End If
                    'Populate ReportType_DropDownList
                    ReportType_DropDownList.DataSource = CreateDropDownListDatatable(tables.ReportTypes)
                    ReportType_DropDownList.DataValueField = "ID"
                    ReportType_DropDownList.DataTextField = "Name"
                    ReportType_DropDownList.DataBind()
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
                Else
                    Title_Label.Text = Lockout_Text
                    ButtonGroup_Div.Visible = False
                    Main_Table.Visible = False
                End If
            Else 'Redirect user to login if he/she is not logged in
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/CreateReport.aspx?ICSRID=" & CurrentICSR_ID)
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

    Protected Sub PopulateReporterFromLastReport_Button_Click(sender As Object, e As EventArgs) Handles PopulateReporterFromLastReport_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Dim LastReportReporterDetailsReadCommand As New SqlCommand("SELECT TOP 1 CASE WHEN ReporterName IS NULL THEN '' ELSE ReporterName END AS ReporterName, CASE WHEN ReporterAddress IS NULL THEN '' ELSE ReporterAddress END AS ReporterAddress, CASE WHEN ReporterPhone IS NULL THEN '' ELSE ReporterPhone END AS ReporterPhone, CASE WHEN ReporterFax IS NULL THEN '' ELSE ReporterFax END AS ReporterFax, CASE WHEN ReporterEmail IS NULL THEN '' ELSE ReporterEmail END AS ReporterEmail FROM Reports WHERE ICSR_ID = @CurrentICSR_ID ORDER BY ID DESC", Connection)
        LastReportReporterDetailsReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
        Try
            Connection.Open()
            Dim LastReportReporterDetailsReader As SqlDataReader = LastReportReporterDetailsReadCommand.ExecuteReader()
            While LastReportReporterDetailsReader.Read()
                ReporterName_Textbox.Text = LastReportReporterDetailsReader.GetString(0)
                ReporterAddress_Textbox.Text = LastReportReporterDetailsReader.GetString(1)
                ReporterPhone_Textbox.Text = LastReportReporterDetailsReader.GetString(2)
                ReporterFax_Textbox.Text = LastReportReporterDetailsReader.GetString(3)
                ReporterEmail_Textbox.Text = LastReportReporterDetailsReader.GetString(4)
            End While
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
        ReportType_DropDownList.CssClass = "form-control"
        ReportReceived_TextBox.CssClass = "form-control"
        ReportDue_TextBox.CssClass = "form-control"
        ReportComplexity_DropDownList.CssClass = "form-control"
        ReportSource_DropDownList.CssClass = "form-control"
        ReporterName_Textbox.CssClass = "form-control"
        ReporterAddress_Textbox.CssClass = "form-control"
        ReporterPhone_Textbox.CssClass = "form-control"
        ReporterFax_Textbox.CssClass = "form-control"
        ReporterEmail_Textbox.CssClass = "form-control"
        ExpeditedReportingRequired_DropDownList.CssClass = "form-control"
        ExpeditedReportingDone_DropDownList.CssClass = "form-control"
        ExpeditedReportingDate_Textbox.CssClass = "form-control"
    End Sub

    Protected Sub ReportType_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        If ReportType_DropDownList.SelectedValue <> -1 Then 'If a report type was selected
            Dim NewReporTypeMatchesOldReportType As Boolean = False
            Dim ExistingReportTypesReadCommand As New SqlCommand("SELECT ReportTypes.ID FROM Reports INNER JOIN ReportTypes ON Reports.ReportType_ID = ReportTypes.ID WHERE Reports.ICSR_ID = @CurrentICSR_ID", Connection)
            ExistingReportTypesReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            Try
                Connection.Open()
                Dim ExistingReportTypesReader As SqlDataReader = ExistingReportTypesReadCommand.ExecuteReader()
                While ExistingReportTypesReader.Read()
                    If ReportType_DropDownList.SelectedValue = ExistingReportTypesReader.GetInt32(0) Then
                        NewReporTypeMatchesOldReportType = True
                    End If
                End While
                Connection.Close()
            Catch ex As Exception
                Connection.Close()
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            End Try
            If NewReporTypeMatchesOldReportType = False Then 'If selected report type is not already taken
                ReportType_DropDownList.CssClass = CssClassSuccess
                args.IsValid = True
            Else 'If selected report type is already taken
                ReportType_DropDownList.CssClass = CssClassFailure
                ReportType_DropDownList.ToolTip = ReportTypeUniquenessValidationFailToolTip
                args.IsValid = False
            End If
        Else 'if no report type was selected
            ReportType_DropDownList.CssClass = CssClassFailure
            ReportType_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub ReportReceived_TextBox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If DateOrEmptyValidator(ReportReceived_TextBox) = True Then
            ReportReceived_TextBox.CssClass = CssClassSuccess
            ReportReceived_TextBox.ToolTip = String.Empty
            args.IsValid = True
        Else
            ReportReceived_TextBox.CssClass = CssClassFailure
            ReportReceived_TextBox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub ReportDue_TextBox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If DateValidator(ReportDue_TextBox) = True Then
            ReportDue_TextBox.CssClass = CssClassSuccess
            ReportDue_TextBox.ToolTip = String.Empty
            args.IsValid = True
        Else
            ReportDue_TextBox.CssClass = CssClassFailure
            ReportDue_TextBox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub ReportComplexity_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(ReportComplexity_DropDownList) = True Then
            ReportComplexity_DropDownList.CssClass = CssClassSuccess
            ReportComplexity_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            ReportComplexity_DropDownList.CssClass = CssClassFailure
            ReportComplexity_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub ReportSource_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(ReportSource_DropDownList) = True Then
            ReportSource_DropDownList.CssClass = CssClassSuccess
            ReportSource_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            ReportSource_DropDownList.CssClass = CssClassFailure
            ReportSource_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub ReporterName_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(ReporterName_Textbox) = True Then
            ReporterName_Textbox.CssClass = CssClassSuccess
            ReporterName_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            ReporterName_Textbox.CssClass = CssClassFailure
            ReporterName_Textbox.ToolTip = TextValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub ReporterAddress_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(ReporterAddress_Textbox) = True Then
            ReporterAddress_Textbox.CssClass = CssClassSuccess
            ReporterAddress_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            ReporterAddress_Textbox.CssClass = CssClassFailure
            ReporterAddress_Textbox.ToolTip = TextValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub ReporterPhone_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If PhoneValidator(ReporterPhone_Textbox) = True Then
            ReporterPhone_Textbox.CssClass = CssClassSuccess
            ReporterPhone_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            ReporterPhone_Textbox.CssClass = CssClassFailure
            ReporterPhone_Textbox.ToolTip = PhoneValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub ReporterFax_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If PhoneValidator(ReporterFax_Textbox) = True Then
            ReporterFax_Textbox.CssClass = CssClassSuccess
            ReporterFax_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            ReporterFax_Textbox.CssClass = CssClassFailure
            ReporterFax_Textbox.ToolTip = PhoneValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub ReporterEmail_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If EmailValidator(ReporterEmail_Textbox) = True Then
            ReporterEmail_Textbox.CssClass = CssClassSuccess
            ReporterEmail_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            ReporterEmail_Textbox.CssClass = CssClassFailure
            ReporterEmail_Textbox.ToolTip = EmailValidationFailToolTip
            args.IsValid = False
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

    Protected Sub ExpeditedReportingDate_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If DateValidator(ExpeditedReportingDate_Textbox) = True Then
            ExpeditedReportingDate_Textbox.CssClass = CssClassSuccess
            ExpeditedReportingDate_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            ExpeditedReportingDate_Textbox.CssClass = CssClassFailure
            ExpeditedReportingDate_Textbox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveNewReport_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            'Determine Reporttatus_ID of ReportStatus where IsStatusNew = True
            Dim ReportStatusNew_ID As Integer = Nothing
            Dim NewReportStatusReadCommand As New SqlCommand("SELECT ID From ReportStatuses WHERE IsStatusNew = @IsStatusNew", Connection)
            NewReportStatusReadCommand.Parameters.AddWithValue("@IsStatusNew", 1)
            Try
                Connection.Open()
                Dim NewReportStatusReader As SqlDataReader = NewReportStatusReadCommand.ExecuteReader()
                While NewReportStatusReader.Read()
                    ReportStatusNew_ID = NewReportStatusReader.GetSqlInt32(0)
                End While
                Connection.Close()
            Catch ex As Exception
                Connection.Close()
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            End Try
            'Write New Report to database
            Dim InsertReportCommand As New SqlCommand("INSERT INTO Reports (ICSR_ID, ReportType_ID, ReportStatus_ID, Received, Due, ReportComplexity_ID, ReportSource_ID, ReporterName, ReporterAddress, ReporterPhone, ReporterFax, ReporterEmail, ExpeditedReportingRequired_ID, ExpeditedReportingDone_ID, ExpeditedReportingDate) VALUES(@CurrentICSR_ID, @ReportType_ID, @ReportStatus_ID, @Received, CASE WHEN @Due = '' THEN NULL ELSE @Due END, CASE WHEN @ReportComplexity_ID = 0 THEN NULL ELSE @ReportComplexity_ID END, CASE WHEN @ReportSource_ID = 0 THEN NULL ELSE @ReportSource_ID END, CASE WHEN @ReporterName = '' THEN NULL ELSE @ReporterName END, CASE WHEN @ReporterAddress = '' THEN NULL ELSE @ReporterAddress END, CASE WHEN @ReporterPhone = '' THEN NULL ELSE @ReporterPhone END, CASE WHEN @ReporterFax = '' THEN NULL ELSE @ReporterFax END, CASE WHEN @ReporterEmail = '' THEN NULL ELSE @ReporterEmail END, CASE WHEN @ExpeditedReportingRequired_ID = 0 THEN NULL ELSE @ExpeditedReportingRequired_ID END, CASE WHEN @ExpeditedReportingDone_ID = 0 THEN NULL ELSE @ExpeditedReportingDone_ID END, CASE WHEN @ExpeditedReportingDate = 0 THEN NULL ELSE @ExpeditedReportingDate END)", Connection)
            InsertReportCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            InsertReportCommand.Parameters.AddWithValue("@ReportType_ID", ReportType_DropDownList.SelectedValue)
            InsertReportCommand.Parameters.AddWithValue("@ReportStatus_ID", ReportStatusNew_ID) 'uses ReportStatus where IsStatusNew = True
            InsertReportCommand.Parameters.AddWithValue("@Received", DateStringOrEmpty(ReportReceived_TextBox.Text.Trim))
            InsertReportCommand.Parameters.AddWithValue("@Due", DateStringOrEmpty(ReportDue_TextBox.Text.Trim))
            InsertReportCommand.Parameters.AddWithValue("@ReportComplexity_ID", ReportComplexity_DropDownList.SelectedValue)
            InsertReportCommand.Parameters.AddWithValue("@ReportSource_ID", ReportSource_DropDownList.SelectedValue)
            InsertReportCommand.Parameters.AddWithValue("@ReporterName", ReporterName_Textbox.Text.Trim)
            InsertReportCommand.Parameters.AddWithValue("@ReporterAddress", ReporterAddress_Textbox.Text.Trim)
            InsertReportCommand.Parameters.AddWithValue("@ReporterPhone", ReporterPhone_Textbox.Text.Trim)
            InsertReportCommand.Parameters.AddWithValue("@ReporterFax", ReporterFax_Textbox.Text.Trim)
            InsertReportCommand.Parameters.AddWithValue("@ReporterEmail", ReporterEmail_Textbox.Text.Trim)
            InsertReportCommand.Parameters.AddWithValue("@ExpeditedReportingRequired_ID", ExpeditedReportingRequired_DropDownList.SelectedValue)
            InsertReportCommand.Parameters.AddWithValue("@ExpeditedReportingDone_ID", ExpeditedReportingDone_DropDownList.SelectedValue)
            InsertReportCommand.Parameters.AddWithValue("@ExpeditedReportingDate", DateStringOrEmpty(ExpeditedReportingDate_Textbox.Text.Trim))
            Try
                Connection.Open()
                InsertReportCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Add case history entry with database updates
            Dim NewReportReadCommand As New SqlCommand("SELECT TOP 1 ID, ReportType_ID, ReportStatus_ID, Received, CASE WHEN Due IS NULL THEN 0 ELSE Due END AS Due, CASE WHEN ReportComplexity_ID IS NULL THEN 0 ELSE ReportComplexity_ID END AS ReportComplexity_ID, CASE WHEN ReportSource_ID IS NULL THEN 0 ELSE ReportSource_ID END AS ReportSource_ID, CASE WHEN ReporterName IS NULL THEN '' ELSE ReporterName END AS ReporterName, CASE WHEN ReporterAddress IS NULL THEN '' ELSE ReporterAddress END AS ReporterAddress, CASE WHEN ReporterPhone IS NULL THEN '' ELSE ReporterPhone END AS ReporterPhone, CASE WHEN ReporterFax IS NULL THEN '' ELSE ReporterFax END AS ReporterFax, CASE WHEN ReporterEmail IS NULL THEN '' ELSE ReporterEmail END AS ReporterEmail, CASE WHEN ExpeditedReportingRequired_ID IS NULL THEN 0 ELSE ExpeditedReportingRequired_ID END AS ExpeditedReportingRequired_ID, CASE WHEN ExpeditedReportingDone_ID IS NULL THEN 0 ELSE ExpeditedReportingDone_ID END AS ExpeditedReportingDone_ID, CASE WHEN ExpeditedReportingDate IS NULL THEN 0 ELSE ExpeditedReportingDate END AS ExpeditedReportingDate FROM Reports WHERE ICSR_ID = @CurrentICSR_ID ORDER BY ID DESC", Connection)
            NewReportReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            Dim NewReport_ID As Integer = Nothing
            Dim NewReportType_ID As Integer = Nothing
            Dim NewReportStatus_ID As Integer = Nothing
            Dim NewReport_Received As Date = Date.MinValue
            Dim NewReport_Due As Date = Date.MinValue
            Dim NewReportComplexity_ID As Integer = Nothing
            Dim NewReportSource_ID As Integer = Nothing
            Dim NewReporterName As String = String.Empty
            Dim NewReporterAddress As String = String.Empty
            Dim NewReporterPhone As String = String.Empty
            Dim NewReporterFax As String = String.Empty
            Dim NewReporterEmail As String = String.Empty
            Dim NewReportExpeditedReportingRequired_ID As Integer = Nothing
            Dim NewReportExpeditedReportingDone_ID As Integer = Nothing
            Dim NewReportExpeditedReportingDate As Date = Date.MinValue
            Try
                Connection.Open()
                Dim NewReportReader As SqlDataReader = NewReportReadCommand.ExecuteReader()
                While NewReportReader.Read()
                    NewReport_ID = NewReportReader.GetInt32(0)
                    NewReportType_ID = NewReportReader.GetInt32(1)
                    NewReportStatus_ID = NewReportReader.GetInt32(2)
                    NewReport_Received = DateOrDateMinValue(NewReportReader.GetDateTime(3))
                    NewReport_Due = DateOrDateMinValue(NewReportReader.GetDateTime(4))
                    NewReportComplexity_ID = NewReportReader.GetInt32(5)
                    NewReportSource_ID = NewReportReader.GetInt32(6)
                    NewReporterName = NewReportReader.GetString(7)
                    NewReporterAddress = NewReportReader.GetString(8)
                    NewReporterPhone = NewReportReader.GetString(9)
                    NewReporterFax = NewReportReader.GetString(10)
                    NewReporterEmail = NewReportReader.GetString(11)
                    NewReportExpeditedReportingRequired_ID = NewReportReader.GetInt32(12)
                    NewReportExpeditedReportingDone_ID = NewReportReader.GetInt32(13)
                    NewReportExpeditedReportingDate = DateOrDateMinValue(NewReportReader.GetDateTime(14))
                End While
                Connection.Close()
            Catch ex As Exception
                Connection.Close()
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            End Try
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            EntryString += NewReportIntro("Report", NewReport_ID)
            EntryString += HistoryEntryReferencedValue("Report", NewReport_ID, "Type", tables.ReportTypes, fields.Name, Nothing, NewReportType_ID)
            EntryString += HistoryEntryReferencedValue("Report", NewReport_ID, "Status", tables.ReportStatuses, fields.Name, Nothing, NewReportStatus_ID)
            EntryString += HistoryEnrtyPlainValue("Report", NewReport_ID, "Received Date", Date.MinValue, NewReport_Received)
            EntryString += HistoryEnrtyPlainValue("Report", NewReport_ID, "Due Date", Date.MinValue, NewReport_Due)
            EntryString += HistoryEntryReferencedValue("Report", NewReport_ID, "Complexity", tables.ReportComplexities, fields.Name, Nothing, NewReportComplexity_ID)
            EntryString += HistoryEntryReferencedValue("Report", NewReport_ID, "Source", tables.ReportSources, fields.Name, Nothing, NewReportSource_ID)
            EntryString += HistoryEnrtyPlainValue("Report", NewReport_ID, "Reporter Name", String.Empty, NewReporterName)
            EntryString += HistoryEnrtyPlainValue("Report", NewReport_ID, "Reporter Address", String.Empty, NewReporterAddress)
            EntryString += HistoryEnrtyPlainValue("Report", NewReport_ID, "Reporter Phone", String.Empty, NewReporterPhone)
            EntryString += HistoryEnrtyPlainValue("Report", NewReport_ID, "Reporter Fax", String.Empty, NewReporterFax)
            EntryString += HistoryEnrtyPlainValue("Report", NewReport_ID, "Reporter Email", String.Empty, NewReporterEmail)
            EntryString += HistoryEntryReferencedValue("Report", NewReport_ID, "Expedited Reporting Required Status", tables.ExpeditedReportingRequired, fields.Name, Nothing, NewReportExpeditedReportingRequired_ID)
            EntryString += HistoryEntryReferencedValue("Report", NewReport_ID, "Expedited Reporting Done Status", tables.ExpeditedReportingDone, fields.Name, Nothing, NewReportExpeditedReportingDone_ID)
            EntryString += HistoryEnrtyPlainValue("Report", NewReport_ID, "Expedited Reporting Date", Date.MinValue, NewReportExpeditedReportingDate)
            EntryString += HistoryDatabasebUpdateOutro
            Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO ICSRHistories(ICSR_ID, User_ID, Timepoint, Entry) VALUES (@ICSR_ID, @User_ID, @Timepoint, @Entry)", Connection)
            InsertHistoryEntryCommand.Parameters.AddWithValue("@ICSR_ID", CurrentICSR_ID)
            InsertHistoryEntryCommand.Parameters.AddWithValue("@User_ID", LoggedIn_User_ID)
            InsertHistoryEntryCommand.Parameters.AddWithValue("@Timepoint", Now())
            InsertHistoryEntryCommand.Parameters.Add("@Entry", SqlDbType.NVarChar, -1)
            InsertHistoryEntryCommand.Parameters("@Entry").Value = EntryString
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
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
            PopulateReporterFromLastReport_Button.Visible = False
            DropDownListDisabled(ReportType_DropDownList)
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
        End If
    End Sub
End Class
