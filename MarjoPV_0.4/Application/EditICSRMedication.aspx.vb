Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditICSRMedication
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Carry out Sub only on initial page load
        If Page.IsPostBack = True Then Exit Sub

        'Redirect to login if user is not logged in
        If Login_Status = False Then
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditICSRMedication.aspx?" & Convert.ToString(Request.QueryString))
        End If

        'Determine call reason, store reason in hidden field and redirect to parent page if query string is invalid
        Dim CallReason As CallReasons = GetCallReason(Me, "ICSRMedicationID", CallReason_HiddenField)

        'Store query string values in hidden fields and variables
        Dim CurrentICSR_ID As Integer = Nothing
        Dim CurrentICSRMedication_ID As Integer = Nothing
        If CallReason = CallReasons.Create Then
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            ICSRMedicationID_HiddenField.Value = Request.QueryString("ICSRMedicationID")
            CurrentICSRMedication_ID = ICSRMedicationID_HiddenField.Value
            ICSRID_HiddenField.Value = ParentID(tables.ICSRs, tables.MedicationsPerICSR, fields.ICSR_ID, CurrentICSRMedication_ID)
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Delete Then
            Delete_HiddenField.Value = Request.QueryString("Delete")
        End If

        'Populate Title_Label
        If CallReason = CallReasons.Create Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Add ICSR Medication"
        ElseIf CallReason = CallReasons.Update Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Edit ICSR Medication " & CurrentICSRMedication_ID
        ElseIf CallReason = CallReasons.Delete Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Delete ICSR Medication " & CurrentICSRMedication_ID
        End If

        'Lock out if user does not have adequate edit rights
        LockoutCheck(CallReason, CurrentICSR_ID, tables.MedicationsPerICSR, Title_Label, ButtonGroup_Div, Main_Table)

        'Format controls based on edit rights
        If CallReason = CallReasons.Create Or CallReason = CallReasons.Update Then
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
        ElseIf CallReason = CallReasons.Delete Then
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
        If CallReason = CallReasons.Delete Then
            RelationDependencyCheck(fields.MedicationPerICSR_ID, CurrentICSRMedication_ID, Status_Label, SaveUpdates_Button, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
        End If

        'Populate DropDownLists
        PopulateDropDownList(MedicationPerICSRRoles_DropDownList, tables.MedicationPerICSRRoles)
        Medications_DropDownList.DataSource = CreateMedicationsPerCompanyDropDownListDatatable(tables.Medications, ParentID(tables.Companies, tables.ICSRs, fields.Company_ID, CurrentICSR_ID))
        Medications_DropDownList.DataValueField = "ID"
        Medications_DropDownList.DataTextField = "Name"
        Medications_DropDownList.DataBind()
        PopulateDropDownList(DrugActions_DropDownList, tables.DrugActions)

        'Populate data fields
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            Dim CurrentValuesDataTable As DataTable = SqlRead(Me, "SELECT CASE WHEN MedicationPerICSRRole_ID IS NULL THEN 0 ELSE MedicationPerICSRRole_ID END AS MedicationPerICSRRole_ID, CASE WHEN Medication_ID IS NULL THEN 0 ELSE Medication_ID END AS Medication_ID, CASE WHEN TotalDailyDose IS NULL THEN 0 ELSE TotalDailyDose END AS TotalDailyDose, CASE WHEN Allocations IS NULL THEN 0 ELSE Allocations END AS Allocations, CASE WHEN Start IS NULL THEN 0 ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN 0 ELSE Stop END AS Stop, CASE WHEN DrugAction_ID IS NULL THEN 0 ELSE DrugAction_ID END AS DrugAction_ID FROM MedicationsPerICSR WHERE ID = " & CurrentICSRMedication_ID)
            MedicationPerICSRRoles_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(0)
            AtEditPageLoad_MedicationPerICSRRole_ID_HiddenField.Value = CurrentValuesDataTable.Rows(0)(0)
            Medications_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(1)
            AtEditPageLoad_Medication_ID_HiddenField.Value = CurrentValuesDataTable.Rows(0)(1)
            TotalDailyDose_Textbox.Text = SqlIntDisplay(CurrentValuesDataTable.Rows(0)(2))
            AtEditPageLoad_TotalDailyDose_HiddenField.Value = CurrentValuesDataTable.Rows(0)(2)
            Allocations_Textbox.Text = SqlIntDisplay(CurrentValuesDataTable.Rows(0)(3))
            AtEditPageLoad_Allocations_HiddenField.Value = CurrentValuesDataTable.Rows(0)(3)
            Start_Textbox.Text = SqlDateDisplay(CurrentValuesDataTable.Rows(0)(4))
            AtEditPageLoad_Start_HiddenField.Value = CurrentValuesDataTable.Rows(0)(4)
            Stop_Textbox.Text = SqlDateDisplay(CurrentValuesDataTable.Rows(0)(5))
            AtEditPageLoad_Stop_HiddenField.Value = CurrentValuesDataTable.Rows(0)(5)
            DrugActions_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(6)
            AtEditPageLoad_DrugAction_ID_HiddenField.Value = CurrentValuesDataTable.Rows(0)(6)
        End If

        'Specify dose type in TotalDailyDose_Textbox label
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            If Medications_DropDownList.SelectedValue = 0 Then
                TotalDailyDose_Label.Text = "Total Daily Dose:"
            Else
                Dim DoseTypeDataTable As DataTable = SqlRead(Me, "SELECT DoseTypes.Name FROM Medications INNER JOIN DoseTypes ON DoseTypes.ID = Medications.DoseType_ID WHERE Medications.ID = " & Medications_DropDownList.SelectedValue)
                Dim Medications_DropDownList_SelectedValue_DoseType_Name As String = DoseTypeDataTable.Rows(0)(0)
                TotalDailyDose_Label.Text = "Total Daily Dose (in " & Medications_DropDownList_SelectedValue_DoseType_Name & "):"
            End If
        End If
    End Sub

    Protected Sub Medications_DropDownList_SelectedIndexChanged(sender As Object, e As EventArgs)
        If Medications_DropDownList.SelectedValue = 0 Then
            TotalDailyDose_Label.Text = "Total Daily Dose:"
        Else
            Dim DoseTypeDataTable As DataTable = SqlRead(Me, "SELECT DoseTypes.Name FROM Medications INNER JOIN DoseTypes ON DoseTypes.ID = Medications.DoseType_ID WHERE Medications.ID = " & Medications_DropDownList.SelectedValue)
            Dim Medications_DropDownList_SelectedValue_DoseType_Name As String = DoseTypeDataTable.Rows(0)(0)
            TotalDailyDose_Label.Text = "Total Daily Dose (in " & Medications_DropDownList_SelectedValue_DoseType_Name & "):"
        End If
    End Sub

    Protected Sub Cancel_Button_Click() Handles Cancel_Button.Click, ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub MedicationPerICSRRoles_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim CallReason As CallReasons = CallReason_HiddenField.Value
        If CallReason = CallReasons.Update Then
            'Fail validation if no medication role was chosen of if there is a relation dependency
            Dim CurrentICSRMedication_ID As Integer = ICSRMedicationID_HiddenField.Value
            If MedicationPerICSRRoles_DropDownList.SelectedValue = 2 And RelationDependency(fields.MedicationPerICSR_ID, CurrentICSRMedication_ID) = True Then
                MedicationPerICSRRoles_DropDownList.CssClass = CssClassFailure
                MedicationPerICSRRoles_DropDownList.ToolTip = DependencyFoundMessage
                args.IsValid = False
            ElseIf SelectionValidator(MedicationPerICSRRoles_DropDownList) = False Then
                MedicationPerICSRRoles_DropDownList.CssClass = CssClassFailure
                MedicationPerICSRRoles_DropDownList.ToolTip = SelectorValidationFailToolTip
                args.IsValid = False
            Else
                MedicationPerICSRRoles_DropDownList.CssClass = CssClassSuccess
                MedicationPerICSRRoles_DropDownList.ToolTip = String.Empty
                args.IsValid = True
            End If
        ElseIf CallReason = CallReasons.Create Then
            'Fail validation if no medication role was chosen 
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
            Allocations_Textbox.CssClass = CssClassFailure
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
        If Page.IsValid = False Then Exit Sub

        'Redirect to login if user is not logged in
        If Login_Status = False Then
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditICSRMedication.aspx?" & Convert.ToString(Request.QueryString))
        End If

        'Store values from hidden fields in variables
        Dim CallReason As CallReasons
        Dim CurrentICSR_ID As Integer = Nothing
        Dim CurrentICSRMedication_ID As Integer = Nothing
        Dim Delete As Boolean = False
        CallReason = CallReason_HiddenField.Value
        If CallReason = CallReasons.Create Then
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            CurrentICSRMedication_ID = ICSRMedicationID_HiddenField.Value
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Delete Then
            Delete = Delete_HiddenField.Value
        End If

        'Lock out if user does not have adequate edit rights
        LockoutCheck(CallReason, CurrentICSR_ID, tables.MedicationsPerICSR, Title_Label, ButtonGroup_Div, Main_Table)

        'Format Controls
        AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
        If CallReason = CallReasons.Create Or CallReason = CallReasons.Update Then
            DropDownListDisabled(MedicationPerICSRRoles_DropDownList)
            DropDownListDisabled(Medications_DropDownList)
            TextBoxReadOnly(TotalDailyDose_Textbox)
            TextBoxReadOnly(Allocations_Textbox)
            TextBoxReadOnly(Start_Textbox)
            TextBoxReadOnly(Stop_Textbox)
            DropDownListDisabled(DrugActions_DropDownList)
        ElseIf CallReason = CallReasons.Delete Then
            MedicationRole_Row.Visible = False
            MedicationName_Row.Visible = False
            TotalDailyDose_Row.Visible = False
            Allocations_Row.Visible = False
            Start_Row.Visible = False
            Stop_Row.Visible = False
            DrugAction_Row.Visible = False
        End If

        'Warn & abort if there are discrepancies between the data as shown at edit page load and as stored at save button click
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            Dim DiscrepancyString As String = String.Empty
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.MedicationPerICSRRole_ID, InputTypes.Integer, CurrentICSRMedication_ID, AtEditPageLoad_MedicationPerICSRRole_ID_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.Medication_ID, InputTypes.Integer, CurrentICSRMedication_ID, AtEditPageLoad_Medication_ID_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.TotalDailyDose, InputTypes.Integer, CurrentICSRMedication_ID, AtEditPageLoad_TotalDailyDose_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.Allocations, InputTypes.Integer, CurrentICSRMedication_ID, AtEditPageLoad_Allocations_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.Start, InputTypes.Date, CurrentICSRMedication_ID, AtEditPageLoad_Start_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.Stop, InputTypes.Date, CurrentICSRMedication_ID, AtEditPageLoad_Stop_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicationsPerICSR, fields.DrugAction_ID, InputTypes.Integer, CurrentICSRMedication_ID, AtEditPageLoad_DrugAction_ID_HiddenField)
            If DiscrepancyString <> String.Empty Then
                ShowDiscrepancyWarning(Status_Label, DiscrepancyString)
                Exit Sub
            End If
        End If

        'Store updates in database
        Dim UpdateCommand As New SqlCommand
        UpdateCommand.Connection = Connection
        If CallReason = CallReasons.Create Then
            UpdateCommand.CommandText = "INSERT INTO MedicationsPerICSR (ICSR_ID, Medication_ID, TotalDailyDose, Allocations, Start, Stop, DrugAction_ID, MedicationPerICSRRole_ID) VALUES(@CurrentICSR_ID, @Medication_ID, CASE WHEN @TotalDailyDose = 0 THEN NULL ELSE @TotalDailyDose END, CASE WHEN @Allocations = 0 THEN NULL ELSE @Allocations END, CASE WHEN @Start = '' THEN NULL ELSE @Start END, CASE WHEN @Stop = '' THEN NULL ELSE @Stop END, CASE WHEN @DrugAction_ID = 0 THEN NULL ELSE @DrugAction_ID END, CASE WHEN @MedicationPerICSRRole_ID = 0 THEN NULL ELSE @MedicationPerICSRRole_ID END)"
            UpdateCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            UpdateCommand.Parameters.AddWithValue("@Medication_ID", Medications_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@TotalDailyDose", TryCType(TotalDailyDose_Textbox.Text.Trim, InputTypes.Integer))
            UpdateCommand.Parameters.AddWithValue("@Allocations", TryCType(Allocations_Textbox.Text.Trim, InputTypes.Integer))
            UpdateCommand.Parameters.AddWithValue("@Start", DateStringOrEmpty(Start_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@Stop", DateStringOrEmpty(Stop_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@DrugAction_ID", DrugActions_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@MedicationPerICSRRole_ID", MedicationPerICSRRoles_DropDownList.SelectedValue)
        ElseIf CallReason = CallReasons.Update Then
            UpdateCommand.CommandText = "UPDATE MedicationsPerICSR SET MedicationPerICSRRole_ID = (CASE WHEN @MedicationPerICSRRole_ID = 0 THEN NULL ELSE @MedicationPerICSRRole_ID END), Medication_ID = (CASE WHEN @Medication_ID = 0 THEN NULL ELSE @Medication_ID END), TotalDailyDose = (CASE WHEN @TotalDailyDose = 0 THEN NULL ELSE @TotalDailyDose END), Allocations = (CASE WHEN @Allocations = 0 THEN NULL ELSE @Allocations END), Start = (CASE WHEN @Start = '' THEN NULL ELSE @Start END), Stop = (CASE WHEN @Stop = '' THEN NULL ELSE @Stop END), DrugAction_ID = (CASE WHEN @DrugAction_ID = 0 THEN NULL ELSE @DrugAction_ID END) WHERE ID = @CurrentICSRMedication_ID"
            UpdateCommand.Parameters.AddWithValue("@MedicationPerICSRRole_ID", MedicationPerICSRRoles_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@Medication_ID", Medications_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@TotalDailyDose", TotalDailyDose_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@Allocations", Allocations_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@Start", DateStringOrEmpty(Start_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@Stop", DateStringOrEmpty(Stop_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@DrugAction_ID", DrugActions_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@CurrentICSRMedication_ID", CurrentICSRMedication_ID)
        ElseIf CallReason = CallReasons.Delete Then
            UpdateCommand.CommandText = "DELETE FROM MedicationsPerICSR WHERE ID = @CurrentICSRMedication_ID"
            UpdateCommand.Parameters.AddWithValue("@CurrentICSRMedication_ID", CurrentICSRMedication_ID)
        End If
        SqlUpdate(Me, UpdateCommand)

        'Add audit trail entry
        Dim EntryString As String = HistoryDatabasebUpdateIntro
        If CallReason = CallReasons.Create Then
            Dim NewICSRMedication_ID As DataTable = SqlRead(Me, "SELECT TOP 1 ID FROM MedicationsPerICSR WHERE ICSR_ID = " & CurrentICSR_ID & " ORDER BY ID DESC")
            CurrentICSRMedication_ID = NewICSRMedication_ID.Rows(0)(0)
            EntryString += NewReportIntro("ICSR Medication", CurrentICSRMedication_ID)
        ElseIf CallReason = CallReasons.Delete Then
            EntryString += DeleteReportIntro("ICSR Medication", CurrentICSRMedication_ID)
        End If
        EntryString += HistoryEntryReferencedValue(CallReason, "ICSR Medication", CurrentICSRMedication_ID, "Role", tables.MedicationPerICSRRoles, fields.Name, TryCType(AtEditPageLoad_MedicationPerICSRRole_ID_HiddenField.Value, InputTypes.Integer), TryCType(MedicationPerICSRRoles_DropDownList.SelectedValue, InputTypes.Integer))
        EntryString += HistoryEntryReferencedValue(CallReason, "ICSR Medication", CurrentICSRMedication_ID, "Name", tables.Medications, fields.Name, TryCType(AtEditPageLoad_Medication_ID_HiddenField.Value, InputTypes.Integer), TryCType(Medications_DropDownList.SelectedValue, InputTypes.Integer))
        EntryString += HistoryEntryPlainValue(CallReason, "ICSR Medication", CurrentICSRMedication_ID, "Total Daily Dose", TryCType(AtEditPageLoad_TotalDailyDose_HiddenField.Value, InputTypes.Integer), TryCType(TotalDailyDose_Textbox.Text, InputTypes.Integer))
        EntryString += HistoryEntryPlainValue(CallReason, "ICSR Medication", CurrentICSRMedication_ID, "Allocations per Day", TryCType(AtEditPageLoad_Allocations_HiddenField.Value, InputTypes.Integer), TryCType(Allocations_Textbox.Text, InputTypes.Integer))
        EntryString += HistoryEntryPlainValue(CallReason, "ICSR Medication", CurrentICSRMedication_ID, "Start Date", TryCType(AtEditPageLoad_Start_HiddenField.Value, InputTypes.Date), TryCType(Start_Textbox.Text, InputTypes.Date))
        EntryString += HistoryEntryPlainValue(CallReason, "ICSR Medication", CurrentICSRMedication_ID, "Stop Date", TryCType(AtEditPageLoad_Stop_HiddenField.Value, InputTypes.Date), TryCType(Stop_Textbox.Text, InputTypes.Date))
        EntryString += HistoryEntryReferencedValue(CallReason, "ICSR Medication", CurrentICSRMedication_ID, "Action Taken With Drug", tables.DrugActions, fields.Name, TryCType(AtEditPageLoad_DrugAction_ID_HiddenField.Value, InputTypes.Integer), TryCType(DrugActions_DropDownList.SelectedValue, InputTypes.Integer))
        SaveAuditTrailEntry(Me, CurrentICSR_ID, LoggedIn_User_ID, EntryString)
    End Sub

End Class
