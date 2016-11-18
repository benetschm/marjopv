Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditICSRMedication
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Carry out Sub only on initial page load
        If Page.IsPostBack = True Then
            Exit Sub
        End If

        'Redirect to login if user is not logged in
        If Login_Status = False Then
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditICSRMedication.aspx?" & Convert.ToString(Request.QueryString))
        End If

        'Determine why page was called, store reason in variables and redirect to parent page if query string is invalid
        Dim PageCallReason As PageCallReasons
        If Not (Request.QueryString("ICSRID") Is Nothing) Then
            PageCallReason = PageCallReasons.Create
        ElseIf Not (Request.Item("ICSRMedicationID") Is Nothing) And (Request.Item("Delete") Is Nothing) Then
            PageCallReason = PageCallReasons.Edit
        ElseIf Not (Request.Item("ICSRMedicationID") Is Nothing) And Not (Request.QueryString("Delete") Is Nothing) Then
            PageCallReason = PageCallReasons.Delete
        Else
            Response.Redirect("~/Application/ICSRs.aspx")
            Exit Sub
        End If

        'Store query string values in hidden fields and variables
        Dim CurrentICSR_ID As Integer = Nothing
        Dim CurrentICSRMedication_ID As Integer = Nothing
        Dim Delete As Boolean = False
        If PageCallReason = PageCallReasons.Create Then
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If PageCallReason = PageCallReasons.Edit Or PageCallReason = PageCallReasons.Delete Then
            ICSRMedicationID_HiddenField.Value = Request.QueryString("ICSRMedicationID")
            CurrentICSRMedication_ID = ICSRMedicationID_HiddenField.Value
            ICSRID_HiddenField.Value = ParentID(tables.ICSRs, tables.MedicationsPerICSR, fields.ICSR_ID, CurrentICSRMedication_ID)
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If PageCallReason = PageCallReasons.Delete Then
            Delete_HiddenField.Value = Request.QueryString("Delete")
            Delete = Delete_HiddenField.Value
        End If

        'Populate Title_Label
        If PageCallReason = PageCallReasons.Create Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Add ICSR Medication"
        ElseIf PageCallReason = PageCallReasons.Edit Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Edit ICSR Medication " & CurrentICSRMedication_ID
        ElseIf PageCallReason = PageCallReasons.Delete Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Delete ICSR Medication " & CurrentICSRMedication_ID
        End If

        'Lock out if user does not have adequate edit rights
        If PageCallReason = PageCallReasons.Create Then
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Create) = False Then
                Title_Label.Text = Lockout_Text
                ButtonGroup_Div.Visible = False
                Main_Table.Visible = False
                Exit Sub
            End If
        ElseIf PageCallReason = PageCallReasons.Edit Then
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Edit) = False Then
                Title_Label.Text = Lockout_Text
                ButtonGroup_Div.Visible = False
                Main_Table.Visible = False
                Exit Sub
            End If
        ElseIf PageCallReason = PageCallReasons.Delete Then
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Delete) = False Then
                Title_Label.Text = Lockout_Text
                ButtonGroup_Div.Visible = False
                Main_Table.Visible = False
                Exit Sub
            End If
        End If

        'Format controls based on edit rights
        If PageCallReason = PageCallReasons.Create Or PageCallReason = PageCallReasons.Edit Then
            AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.MedicationPerICSRRole_ID) = True Then
                DropDownListEnabled(MedicationPerICSRRoles_DropDownList)
            Else
                DropDownListDisabled(MedicationPerICSRRoles_DropDownList)
                MedicationPerICSRRoles_DropDownList.ToolTip = CannotEditControlText
            End If
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Medication_ID) = True Then
                DropDownListEnabled(Medications_DropDownList)
            Else
                DropDownListDisabled(Medications_DropDownList)
                Medications_DropDownList.ToolTip = CannotEditControlText
            End If
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.TotalDailyDose) = True Then
                TextBoxReadWrite(TotalDailyDose_Textbox)
                TotalDailyDose_Textbox.ToolTip = NumberInputToolTip
            Else
                TextBoxReadOnly(TotalDailyDose_Textbox)
                TotalDailyDose_Textbox.ToolTip = CannotEditControlText
            End If
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Allocations) = True Then
                TextBoxReadWrite(Allocations_Textbox)
                Allocations_Textbox.ToolTip = NumberInputToolTip
            Else
                TextBoxReadOnly(Allocations_Textbox)
                Allocations_Textbox.ToolTip = CannotEditControlText
            End If
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Start) = True Then
                TextBoxReadWrite(Start_Textbox)
                Start_Textbox.ToolTip = DateInputToolTip
            Else
                TextBoxReadOnly(Start_Textbox)
                Start_Textbox.ToolTip = CannotEditControlText
            End If
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Stop) = True Then
                TextBoxReadWrite(Stop_Textbox)
                Stop_Textbox.ToolTip = DateInputToolTip
            Else
                TextBoxReadOnly(Stop_Textbox)
                Stop_Textbox.ToolTip = CannotEditControlText
            End If
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.DrugAction_ID) = True Then
                DropDownListEnabled(DrugActions_DropDownList)
            Else
                DropDownListDisabled(DrugActions_DropDownList)
            End If
        ElseIf PageCallReason = PageCallReasons.Delete Then
            AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
            DropDownListDisabled(MedicationPerICSRRoles_DropDownList)
            DropDownListDisabled(Medications_DropDownList)
            TextBoxReadOnly(TotalDailyDose_Textbox)
            TextBoxReadOnly(Allocations_Textbox)
            TextBoxReadOnly(Start_Textbox)
            TextBoxReadOnly(Stop_Textbox)
            DropDownListDisabled(DrugActions_DropDownList)
        End If

        'Check if page is called to delete and there is a dataset in 'Relations' which is dependent on the current dataset
        If PageCallReason = PageCallReasons.Delete Then
            If RelationDependency(CurrentICSRMedication_ID) = True Then
                AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                Status_Label.CssClass = CssClassFailure
                Status_Label.Text = DependencyFoundMessage
            End If
        End If

        'Populate DropDownLists
        MedicationPerICSRRoles_DropDownList.DataSource = CreateDropDownListDatatable(tables.MedicationPerICSRRoles)
        MedicationPerICSRRoles_DropDownList.DataValueField = "ID"
        MedicationPerICSRRoles_DropDownList.DataTextField = "Name"
        MedicationPerICSRRoles_DropDownList.DataBind()
        Medications_DropDownList.DataSource = CreateMedicationsPerCompanyDropDownListDatatable(tables.Medications, ParentID(tables.Companies, tables.ICSRs, fields.Company_ID, CurrentICSR_ID))
        Medications_DropDownList.DataValueField = "ID"
        Medications_DropDownList.DataTextField = "Name"
        Medications_DropDownList.DataBind()
        DrugActions_DropDownList.DataSource = CreateDropDownListDatatable(tables.DrugActions)
        DrugActions_DropDownList.DataValueField = "ID"
        DrugActions_DropDownList.DataTextField = "Name"
        DrugActions_DropDownList.DataBind()

        'Populate data fields
        If PageCallReason = PageCallReasons.Edit Or PageCallReason = PageCallReasons.Delete Then
            Dim AtEditPageLoadReadCommand As New SqlCommand("SELECT CASE WHEN MedicationPerICSRRole_ID IS NULL THEN 0 ELSE MedicationPerICSRRole_ID END AS MedicationPerICSRRole_ID, CASE WHEN Medication_ID IS NULL THEN 0 ELSE Medication_ID END AS Medication_ID, CASE WHEN TotalDailyDose IS NULL THEN 0 ELSE TotalDailyDose END AS TotalDailyDose, CASE WHEN Allocations IS NULL THEN 0 ELSE Allocations END AS Allocations, CASE WHEN Start IS NULL THEN 0 ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN 0 ELSE Stop END AS Stop, CASE WHEN DrugAction_ID IS NULL THEN 0 ELSE DrugAction_ID END AS DrugAction_ID FROM MedicationsPerICSR WHERE ID = @CurrentICSRMedication_ID", Connection)
            AtEditPageLoadReadCommand.Parameters.AddWithValue("@CurrentICSRMedication_ID", CurrentICSRMedication_ID)
            Try
                Connection.Open()
                Dim AtEditPageLoadReader As SqlDataReader = AtEditPageLoadReadCommand.ExecuteReader()
                While AtEditPageLoadReader.Read()
                    MedicationPerICSRRoles_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(0)
                    AtEditPageLoad_MedicationPerICSRRole_ID_HiddenField.Value = AtEditPageLoadReader.GetInt32(0)
                    Medications_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(1)
                    AtEditPageLoad_Medication_ID_HiddenField.Value = AtEditPageLoadReader.GetInt32(1)
                    TotalDailyDose_Textbox.Text = AtEditPageLoadReader.GetInt32(2)
                    AtEditPageLoad_TotalDailyDose_HiddenField.Value = AtEditPageLoadReader.GetInt32(2)
                    Allocations_Textbox.Text = AtEditPageLoadReader.GetInt32(3)
                    AtEditPageLoad_Allocations_HiddenField.Value = AtEditPageLoadReader.GetInt32(3)
                    Start_Textbox.Text = SqlDateDisplay(AtEditPageLoadReader.GetDateTime(4))
                    AtEditPageLoad_Start_HiddenField.Value = AtEditPageLoadReader.GetDateTime(4)
                    Stop_Textbox.Text = SqlDateDisplay(AtEditPageLoadReader.GetDateTime(5))
                    AtEditPageLoad_Stop_HiddenField.Value = AtEditPageLoadReader.GetDateTime(5)
                    DrugActions_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(6)
                    AtEditPageLoad_DrugAction_ID_HiddenField.Value = AtEditPageLoadReader.GetInt32(6)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
        End If

        'Specify dose type in TotalDailyDose_Textbox label
        If PageCallReason = PageCallReasons.Edit Or PageCallReason = PageCallReasons.Delete Then
            Dim Medications_DropDownList_SelectedValue_DoseType_Name As String = String.Empty
            Dim DoseTypeReadCommand As New SqlCommand("SELECT DoseTypes.Name FROM Medications INNER JOIN DoseTypes ON DoseTypes.ID = Medications.DoseType_ID WHERE Medications.ID = @SelectedMedication_ID", Connection)
            DoseTypeReadCommand.Parameters.AddWithValue("@SelectedMedication_ID", Medications_DropDownList.SelectedValue)
            Try
                Connection.Open()
                Dim DoseTypeReader As SqlDataReader = DoseTypeReadCommand.ExecuteReader()
                While DoseTypeReader.Read()
                    Medications_DropDownList_SelectedValue_DoseType_Name = DoseTypeReader.GetString(0)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            If Medications_DropDownList.SelectedValue = 0 Then
                TotalDailyDose_Label.Text = "Total Daily Dose:"
            Else
                TotalDailyDose_Label.Text = "Total Daily Dose (in " & Medications_DropDownList_SelectedValue_DoseType_Name & "):"
            End If
        End If
    End Sub

    Protected Sub Medications_DropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim Medications_DropDownList_SelectedValue_DoseType_Name As String = String.Empty
        Dim DoseTypeReadCommand As New SqlCommand("SELECT DoseTypes.Name FROM Medications INNER JOIN DoseTypes ON DoseTypes.ID = Medications.DoseType_ID WHERE Medications.ID = @SelectedMedication_ID", Connection)
        DoseTypeReadCommand.Parameters.AddWithValue("@SelectedMedication_ID", Medications_DropDownList.SelectedValue)
        Try
            Connection.Open()
            Dim DoseTypeReader As SqlDataReader = DoseTypeReadCommand.ExecuteReader()
            While DoseTypeReader.Read()
                Medications_DropDownList_SelectedValue_DoseType_Name = DoseTypeReader.GetString(0)
            End While
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
        If Medications_DropDownList.SelectedValue = 0 Then
            TotalDailyDose_Label.Text = "Total Daily Dose:"
        Else
            TotalDailyDose_Label.Text = "Total Daily Dose (in " & Medications_DropDownList_SelectedValue_DoseType_Name & "):"
        End If
    End Sub

    Protected Sub Cancel_Button_Click() Handles Cancel_Button.Click, ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub MedicationPerICSRRoles_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Not (Request.QueryString("ICSRMedicationID") Is Nothing) Then
            Dim CurrentICSRMedication_ID As Integer = ICSRMedicationID_HiddenField.Value
            If MedicationPerICSRRoles_DropDownList.SelectedValue = 2 And RelationDependency(CurrentICSRMedication_ID) = True Then 'If the selected ICSR medication role is 'Concomitant Medication' and there is at least one dataset in 'Relations' which is dependent on the current ICSR Medication
                MedicationPerICSRRoles_DropDownList.CssClass = CssClassFailure
                MedicationPerICSRRoles_DropDownList.ToolTip = DependencyFoundMessage
                args.IsValid = False
            ElseIf SelectionValidator(MedicationPerICSRRoles_DropDownList) = False Then 'If no ICSR Medication Role was selected
                MedicationPerICSRRoles_DropDownList.CssClass = CssClassFailure
                MedicationPerICSRRoles_DropDownList.ToolTip = SelectorValidationFailToolTip
                args.IsValid = False
            Else
                MedicationPerICSRRoles_DropDownList.CssClass = CssClassSuccess
                MedicationPerICSRRoles_DropDownList.ToolTip = String.Empty
                args.IsValid = True
            End If
        ElseIf Not (Request.QueryString("ICSRID") Is Nothing) Then
            If MedicationPerICSRRoles_DropDownList.SelectedValue > 0 Then
                MedicationPerICSRRoles_DropDownList.CssClass = CssClassSuccess
                MedicationPerICSRRoles_DropDownList.ToolTip = String.Empty
                args.IsValid = True
            Else
                MedicationPerICSRRoles_DropDownList.CssClass = CssClassFailure
                MedicationPerICSRRoles_DropDownList.ToolTip = SelectorValidationFailToolTip
                args.IsValid = False
            End If
        End If
    End Sub

    Protected Sub Medications_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Medications_DropDownList.SelectedValue > 0 Then
            Medications_DropDownList.CssClass = CssClassSuccess
            Medications_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            Medications_DropDownList.CssClass = CssClassFailure
            Medications_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub TotalDailyDose_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Validation(TotalDailyDose_Textbox.Text, InputTypes.Integer) = True Then
            TotalDailyDose_Textbox.CssClass = CssClassSuccess
            TotalDailyDose_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf TotalDailyDose_Textbox.Text = String.Empty Then
            TotalDailyDose_Textbox.CssClass = CssClassSuccess
            TotalDailyDose_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            TotalDailyDose_Textbox.CssClass = CssClassFailure
            TotalDailyDose_Textbox.ToolTip = NumberValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Allocations_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Validation(Allocations_Textbox.Text, InputTypes.Integer) = True Then
            Allocations_Textbox.CssClass = CssClassSuccess
            Allocations_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf Allocations_Textbox.Text = String.Empty Then
            Allocations_Textbox.CssClass = CssClassSuccess
            Allocations_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Allocations_Textbox.CssClass = "form-control alert-danger"
            Allocations_Textbox.ToolTip = NumberValidationFailToolTip
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

    Protected Sub StartStop_Textbox_Consistency_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Validation(Stop_Textbox.Text, InputTypes.Date) = True And Validation(Stop_Textbox.Text, InputTypes.Date) = True And DateOrDateMinValue(Stop_Textbox.Text) < DateOrDateMinValue(Start_Textbox.Text) Then
            Start_Textbox.CssClass = CssClassFailure
            Start_Textbox.ToolTip = DateInconsistencyValidationFailToolTip
            Stop_Textbox.CssClass = CssClassFailure
            Stop_Textbox.ToolTip = DateInconsistencyValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub DrugActions_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        DrugActions_DropDownList.CssClass = CssClassSuccess
        DrugActions_DropDownList.ToolTip = String.Empty
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click, ConfirmDeletion_Button.Click

        'Carry out Sub only if input is valid
        If Page.IsValid = False Then
            Exit Sub
        End If

        'Redirect to login if user is not logged in
        If Login_Status = False Then
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditICSRMedication.aspx?" & Convert.ToString(Request.QueryString))
        End If

        'Determine why page was called and store reason in variables
        Dim PageCallReason As PageCallReasons
        If ICSRMedicationID_HiddenField.Value = String.Empty And Delete_HiddenField.Value = String.Empty Then
            PageCallReason = PageCallReasons.Create
        ElseIf ICSRMedicationID_HiddenField.Value <> String.Empty And Delete_HiddenField.Value = String.Empty Then
            PageCallReason = PageCallReasons.Edit
        ElseIf Delete_HiddenField.Value <> String.Empty Then
            PageCallReason = PageCallReasons.Delete
        End If

        'Store values from hidden fields in variables
        Dim CurrentICSR_ID As Integer = Nothing
        Dim CurrentICSRMedication_ID As Integer = Nothing
        Dim Delete As Boolean = False
        If PageCallReason = PageCallReasons.Create Then
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If PageCallReason = PageCallReasons.Edit Or PageCallReason = PageCallReasons.Delete Then
            CurrentICSRMedication_ID = ICSRMedicationID_HiddenField.Value
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If PageCallReason = PageCallReasons.Delete Then
            Delete = Delete_HiddenField.Value
        End If

        'Lock out if user does not have adequate edit rights
        If PageCallReason = PageCallReasons.Create Then
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Create) = False Then
                Title_Label.Text = Lockout_Text
                ButtonGroup_Div.Visible = False
                Main_Table.Visible = False
                Exit Sub
            End If
        ElseIf PageCallReason = PageCallReasons.Edit Then
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Edit) = False Then
                Title_Label.Text = Lockout_Text
                ButtonGroup_Div.Visible = False
                Main_Table.Visible = False
                Exit Sub
            End If
        ElseIf PageCallReason = PageCallReasons.Delete Then
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.MedicationsPerICSR, fields.Delete) = False Then
                Title_Label.Text = Lockout_Text
                ButtonGroup_Div.Visible = False
                Main_Table.Visible = False
                Exit Sub
            End If
        End If

        'Format Controls
        AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
        If PageCallReason = PageCallReasons.Create Or PageCallReason = PageCallReasons.Edit Then
            DropDownListDisabled(MedicationPerICSRRoles_DropDownList)
            DropDownListDisabled(Medications_DropDownList)
            TextBoxReadOnly(TotalDailyDose_Textbox)
            TextBoxReadOnly(Allocations_Textbox)
            TextBoxReadOnly(Start_Textbox)
            TextBoxReadOnly(Stop_Textbox)
            DropDownListDisabled(DrugActions_DropDownList)
        ElseIf PageCallReason = PageCallReasons.Delete Then
            MedicationRole_Row.Visible = False
            MedicationName_Row.Visible = False
            TotalDailyDose_Row.Visible = False
            Allocations_Row.Visible = False
            Start_Row.Visible = False
            Stop_Row.Visible = False
            DrugAction_Row.Visible = False
        End If

        'Warn & abort if there are discrepancies between the data as shown at edit page load and as stored at save button click
        If PageCallReason = PageCallReasons.Edit Or PageCallReason = PageCallReasons.Delete Then
            Dim DiscrepancyString As String = String.Empty
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.MedicationPerICSRRole_ID, InputTypes.Integer, CurrentICSRMedication_ID, AtEditPageLoad_MedicationPerICSRRole_ID_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.Medication_ID, InputTypes.Integer, CurrentICSRMedication_ID, AtEditPageLoad_Medication_ID_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.TotalDailyDose, InputTypes.Integer, CurrentICSRMedication_ID, AtEditPageLoad_TotalDailyDose_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.Allocations, InputTypes.Integer, CurrentICSRMedication_ID, AtEditPageLoad_Allocations_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.Start, InputTypes.Date, CurrentICSRMedication_ID, AtEditPageLoad_Start_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.Stop, InputTypes.Date, CurrentICSRMedication_ID, AtEditPageLoad_Stop_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.DrugAction_ID, InputTypes.Integer, CurrentICSRMedication_ID, AtEditPageLoad_DrugAction_ID_HiddenField)
            If DiscrepancyString <> String.Empty Then
                AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                ShowDiscrepancyWarning(Status_Label, DiscrepancyString)
                DropDownListDisabled(MedicationPerICSRRoles_DropDownList)
                DropDownListDisabled(Medications_DropDownList)
                TextBoxReadOnly(TotalDailyDose_Textbox)
                TextBoxReadOnly(Allocations_Textbox)
                TextBoxReadOnly(Start_Textbox)
                TextBoxReadOnly(Stop_Textbox)
                DropDownListDisabled(DrugActions_DropDownList)
                Exit Sub
            End If
        End If

        'Store updates in database
        Dim UpdateCommand As New SqlCommand
        UpdateCommand.Connection = Connection
        If PageCallReason = PageCallReasons.Create Then
            UpdateCommand.CommandText = "INSERT INTO MedicationsPerICSR (ICSR_ID, Medication_ID, TotalDailyDose, Allocations, Start, Stop, DrugAction_ID, MedicationPerICSRRole_ID) VALUES(@CurrentICSR_ID, @Medication_ID, CASE WHEN @TotalDailyDose = 0 THEN NULL ELSE @TotalDailyDose END, CASE WHEN @Allocations = 0 THEN NULL ELSE @Allocations END, CASE WHEN @Start = '' THEN NULL ELSE @Start END, CASE WHEN @Stop = '' THEN NULL ELSE @Stop END, CASE WHEN @DrugAction_ID = 0 THEN NULL ELSE @DrugAction_ID END, CASE WHEN @MedicationPerICSRRole_ID = 0 THEN NULL ELSE @MedicationPerICSRRole_ID END)"
            UpdateCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            UpdateCommand.Parameters.AddWithValue("@Medication_ID", Medications_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@TotalDailyDose", TryCType(TotalDailyDose_Textbox.Text.Trim, InputTypes.Integer))
            UpdateCommand.Parameters.AddWithValue("@Allocations", TryCType(Allocations_Textbox.Text.Trim, InputTypes.Integer))
            UpdateCommand.Parameters.AddWithValue("@Start", DateStringOrEmpty(Start_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@Stop", DateStringOrEmpty(Stop_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@DrugAction_ID", DrugActions_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@MedicationPerICSRRole_ID", MedicationPerICSRRoles_DropDownList.SelectedValue)
        ElseIf PageCallReason = PageCallReasons.Edit Then
            UpdateCommand.CommandText = "UPDATE MedicationsPerICSR SET MedicationPerICSRRole_ID = (CASE WHEN @MedicationPerICSRRole_ID = 0 THEN NULL ELSE @MedicationPerICSRRole_ID END), Medication_ID = (CASE WHEN @Medication_ID = 0 THEN NULL ELSE @Medication_ID END), TotalDailyDose = (CASE WHEN @TotalDailyDose = 0 THEN NULL ELSE @TotalDailyDose END), Allocations = (CASE WHEN @Allocations = 0 THEN NULL ELSE @Allocations END), Start = (CASE WHEN @Start = '' THEN NULL ELSE @Start END), Stop = (CASE WHEN @Stop = '' THEN NULL ELSE @Stop END), DrugAction_ID = (CASE WHEN @DrugAction_ID = 0 THEN NULL ELSE @DrugAction_ID END) WHERE ID = @CurrentICSRMedication_ID"
            UpdateCommand.Parameters.AddWithValue("@MedicationPerICSRRole_ID", MedicationPerICSRRoles_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@Medication_ID", Medications_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@TotalDailyDose", TotalDailyDose_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@Allocations", Allocations_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@Start", DateStringOrEmpty(Start_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@Stop", DateStringOrEmpty(Stop_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@DrugAction_ID", DrugActions_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@CurrentICSRMedication_ID", CurrentICSRMedication_ID)
        ElseIf PageCallReason = PageCallReasons.Delete Then
            UpdateCommand.CommandText = "DELETE FROM MedicationsPerICSR WHERE ID = @CurrentICSRMedication_ID"
            UpdateCommand.Parameters.AddWithValue("@CurrentICSRMedication_ID", CurrentICSRMedication_ID)
        End If
        'Try
        Connection.Open()
        UpdateCommand.ExecuteNonQuery()
        'Catch ex As Exception
        '    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
        '    Exit Sub
        'Finally
        Connection.Close()
        'End Try

        'Add audit trail entry
        Dim EntryString As String = HistoryDatabasebUpdateIntro
        If PageCallReason = PageCallReasons.Create Then
            Dim NewICSRMedicationReadCommand As New SqlCommand("SELECT TOP 1 ID FROM MedicationsPerICSR WHERE ICSR_ID = @CurrentICSR_ID ORDER BY ID DESC", Connection)
            NewICSRMedicationReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            Try
                Connection.Open()
                Dim NewICSRMedicationReader As SqlDataReader = NewICSRMedicationReadCommand.ExecuteReader()
                While NewICSRMedicationReader.Read()
                    CurrentICSRMedication_ID = NewICSRMedicationReader.GetInt32(0)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            EntryString += NewReportIntro("ICSR Medication", CurrentICSRMedication_ID)
        ElseIf PageCallReason = PageCallReasons.Delete Then
            EntryString += DeleteReportIntro("ICSR Medication", CurrentICSRMedication_ID)
        End If
        EntryString += HistoryEntryReferencedValue(PageCallReason, "ICSR Medication", CurrentICSRMedication_ID, "Role", tables.MedicationPerICSRRoles, fields.Name, TryCType(AtEditPageLoad_MedicationPerICSRRole_ID_HiddenField.Value, InputTypes.Integer), TryCType(MedicationPerICSRRoles_DropDownList.SelectedValue, InputTypes.Integer))
        EntryString += HistoryEntryReferencedValue(PageCallReason, "ICSR Medication", CurrentICSRMedication_ID, "Name", tables.Medications, fields.Name, TryCType(AtEditPageLoad_Medication_ID_HiddenField.Value, InputTypes.Integer), TryCType(Medications_DropDownList.SelectedValue, InputTypes.Integer))
        EntryString += HistoryEntryPlainValue(PageCallReason, "ICSR Medication", CurrentICSRMedication_ID, "Total Daily Dose", TryCType(AtEditPageLoad_TotalDailyDose_HiddenField.Value, InputTypes.Integer), TryCType(TotalDailyDose_Textbox.Text, InputTypes.Integer))
        EntryString += HistoryEntryPlainValue(PageCallReason, "ICSR Medication", CurrentICSRMedication_ID, "Allocations per Day", TryCType(AtEditPageLoad_Allocations_HiddenField.Value, InputTypes.Integer), TryCType(Allocations_Textbox.Text, InputTypes.Integer))
        EntryString += HistoryEntryPlainValue(PageCallReason, "ICSR Medication", CurrentICSRMedication_ID, "Start Date", TryCType(AtEditPageLoad_Start_HiddenField.Value, InputTypes.Date), TryCType(Start_Textbox.Text, InputTypes.Date))
        EntryString += HistoryEntryPlainValue(PageCallReason, "ICSR Medication", CurrentICSRMedication_ID, "Stop Date", TryCType(AtEditPageLoad_Stop_HiddenField.Value, InputTypes.Date), TryCType(Stop_Textbox.Text, InputTypes.Date))
        EntryString += HistoryEntryReferencedValue(PageCallReason, "ICSR Medication", CurrentICSRMedication_ID, "Action Taken With Drug", tables.DrugActions, fields.Name, TryCType(AtEditPageLoad_DrugAction_ID_HiddenField.Value, InputTypes.Integer), TryCType(DrugActions_DropDownList.SelectedValue, InputTypes.Integer))
        SaveAuditTrailEntry(CurrentICSR_ID, LoggedIn_User_ID, EntryString)
    End Sub

End Class
