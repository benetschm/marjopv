Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditRelation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Carry out Sub only on initial page load
        If Page.IsPostBack = True Then Exit Sub

        'Redirect to login if user is not logged in
        If Login_Status = False Then
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditRelation.aspx?" & Convert.ToString(Request.QueryString))
        End If

        'Determine call reason, store reason in hidden field and redirect to parent page if query string is invalid
        Dim CallReason As CallReasons = GetCallReason(Me, "RelationID", CallReason_HiddenField)

        'Store query string values in hidden fields and variables
        Dim CurrentICSR_ID As Integer = Nothing
        Dim CurrentRelation_ID As Integer = Nothing
        If CallReason = CallReasons.Create Then
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            RelationID_HiddenField.Value = Request.QueryString("RelationID")
            CurrentRelation_ID = RelationID_HiddenField.Value
            ICSRID_HiddenField.Value = ICSRIDFOfRelation(CurrentRelation_ID)
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Delete Then
            Delete_HiddenField.Value = Request.QueryString("Delete")
        End If

        'Populate Title_Label
        If CallReason = CallReasons.Create Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Add Relation"
        ElseIf CallReason = CallReasons.Update Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Edit Relation " & CurrentRelation_ID
        ElseIf CallReason = CallReasons.Delete Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Delete Relation " & CurrentRelation_ID
        End If

        'Lock out if user does not have adequate edit rights
        LockoutCheck(CallReason, CurrentICSR_ID, tables.Relations, Title_Label, ButtonGroup_Div, Main_Table)

        'Format controls based on edit rights
        If CallReason = CallReasons.Create Or CallReason = CallReasons.Update Then
            AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Relations, fields.AE_ID) = True Then
                DropDownListEnabled(AEs_DropDownList)
            Else
                DropDownListDisabled(AEs_DropDownList)
                AEs_DropDownList.ToolTip = CannotEditControlText
            End If
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Relations, fields.MedicationPerICSR_ID) = True Then
                DropDownListEnabled(MedicationsPerICSR_DropDownList)
            Else
                DropDownListDisabled(MedicationsPerICSR_DropDownList)
                MedicationsPerICSR_DropDownList.ToolTip = CannotEditControlText
            End If
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Relations, fields.RelatednessCriterionReporter_ID) = True Then
                DropDownListEnabled(RelatednessCriteriaReporter_DropDownList)
            Else
                DropDownListDisabled(RelatednessCriteriaReporter_DropDownList)
                RelatednessCriteriaReporter_DropDownList.ToolTip = CannotEditControlText
            End If
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Relations, fields.RelatednessCriterionManufacturer_ID) = True Then
                DropDownListEnabled(RelatednessCriteriaManufacturer_DropDownList)
            Else
                DropDownListDisabled(RelatednessCriteriaManufacturer_DropDownList)
                RelatednessCriteriaManufacturer_DropDownList.ToolTip = CannotEditControlText
            End If
            If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Relations, fields.ExpectednessCriterion_ID) = True Then
                DropDownListEnabled(ExpectendessCriteria_DropDownList)
            Else
                DropDownListDisabled(ExpectendessCriteria_DropDownList)
                ExpectendessCriteria_DropDownList.ToolTip = CannotEditControlText
            End If
        ElseIf CallReason = CallReasons.Delete Then
            AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
            DropDownListDisabled(AEs_DropDownList)
            DropDownListDisabled(MedicationsPerICSR_DropDownList)
            DropDownListDisabled(RelatednessCriteriaReporter_DropDownList)
            DropDownListDisabled(RelatednessCriteriaManufacturer_DropDownList)
            DropDownListDisabled(ExpectendessCriteria_DropDownList)
        End If

        'Populate DropDownLists
        AEs_DropDownList.DataSource = CreateAEsOfCurrentICSRDropDownListDatatable(CurrentICSR_ID)
        AEs_DropDownList.DataValueField = "ID"
        AEs_DropDownList.DataTextField = "Name"
        AEs_DropDownList.DataBind()
        MedicationsPerICSR_DropDownList.DataSource = CreateSuspectedDrugsOfCurrentICSRDropDownListDatatable(CurrentICSR_ID)
        MedicationsPerICSR_DropDownList.DataValueField = "ID"
        MedicationsPerICSR_DropDownList.DataTextField = "Name"
        MedicationsPerICSR_DropDownList.DataBind()
        PopulateDropDownList(RelatednessCriteriaReporter_DropDownList, tables.RelatednessCriteriaReporter)
        PopulateDropDownList(RelatednessCriteriaManufacturer_DropDownList, tables.RelatednessCriteriaManufacturer)
        PopulateDropDownList(ExpectendessCriteria_DropDownList, tables.ExpectednessCriteria)

        'Populate data fields
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            Dim CurrentValuesDataTable As DataTable = SqlRead(Me, "SELECT Relations.AE_ID, Relations.MedicationPerICSR_ID, CASE WHEN Relations.RelatednessCriterionReporter_ID IS NULL THEN 0 ELSE Relations.RelatednessCriterionReporter_ID END AS RelatednessCriterionReporter_ID, CASE WHEN Relations.RelatednessCriterionManufacturer_ID IS NULL THEN 0 ELSE Relations.RelatednessCriterionManufacturer_ID END AS RelatednessCriterionManufacturer_ID, CASE WHEN Relations.ExpectednessCriterion_ID IS NULL THEN 0 ELSE Relations.ExpectednessCriterion_ID END AS ExpectednessCriterion_ID FROM Relations INNER JOIN AEs ON Relations.AE_ID = AEs.ID WHERE Relations.ID = " & CurrentRelation_ID)
            AEs_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(0)
            AtEditPageLoad_AE_HiddenField.Value = CurrentValuesDataTable.Rows(0)(0)
            MedicationsPerICSR_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(1)
            AtEditPageLoad_MedicationPerICSR_HiddenField.Value = CurrentValuesDataTable.Rows(0)(1)
            RelatednessCriteriaReporter_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(2)
            AtEditPageLoad_RelatednessCriterionReporter_HiddenField.Value = CurrentValuesDataTable.Rows(0)(2)
            RelatednessCriteriaManufacturer_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(3)
            AtEditPageLoad_RelatednessCriterionManufacturer_HiddenField.Value = CurrentValuesDataTable.Rows(0)(3)
            ExpectendessCriteria_DropDownList.SelectedValue = CurrentValuesDataTable.Rows(0)(4)
            AtEditPageLoad_ExpectendessCriterion_HiddenField.Value = CurrentValuesDataTable.Rows(0)(4)
        End If

    End Sub

    Protected Sub ReturnToICSROverview() Handles Cancel_Button.Click, ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub RelationCriteria_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim CurrentRelation_ID = RelationID_HiddenField.Value
        Dim CurrentICSR_ID = ICSRID_HiddenField.Value
        If AEs_DropDownList.SelectedValue <> 0 And MedicationsPerICSR_DropDownList.SelectedValue <> 0 Then
            Dim RelationDuplicationFound As Boolean = False
            Dim RelationDuplicationReadCommand As New SqlCommand("SELECT CASE WHEN Relations.AE_ID IS NULL THEN 0 ELSE Relations.AE_ID END AS AE_ID, CASE WHEN Relations.MedicationperICSR_ID IS NULL THEN 0 ELSE Relations.MedicationperICSR_ID END AS MedicationperICSR_ID FROM Relations INNER JOIN AEs ON AEs.ID = Relations.AE_ID WHERE AEs.ICSR_ID = @CurrentICSR_ID AND Relations.ID <> @CurrentRelation_ID", Connection)
            RelationDuplicationReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            RelationDuplicationReadCommand.Parameters.AddWithValue("@CurrentRelation_ID", CurrentRelation_ID)
            Try
                Connection.Open()
                Dim RelationDuplicationReader As SqlDataReader = RelationDuplicationReadCommand.ExecuteReader()
                While RelationDuplicationReader.Read()
                    If AEs_DropDownList.SelectedValue = RelationDuplicationReader.GetInt32(0) Then
                        If MedicationsPerICSR_DropDownList.SelectedValue = RelationDuplicationReader.GetInt32(1) Then
                            RelationDuplicationFound = True
                        End If
                    End If
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            If RelationDuplicationFound = False Then
                AEs_DropDownList.CssClass = CssClassSuccess
                AEs_DropDownList.ToolTip = String.Empty
                MedicationsPerICSR_DropDownList.CssClass = CssClassSuccess
                MedicationsPerICSR_DropDownList.ToolTip = String.Empty
                args.IsValid = True
            Else
                AEs_DropDownList.CssClass = CssClassFailure
                AEs_DropDownList.ToolTip = RelationDuplicationFoundMessage
                MedicationsPerICSR_DropDownList.CssClass = CssClassFailure
                MedicationsPerICSR_DropDownList.ToolTip = RelationDuplicationFoundMessage
                args.IsValid = False
            End If
        Else
            AEs_DropDownList.CssClass = CssClassFailure
            AEs_DropDownList.ToolTip = RelationCriteriaNotFullySpecified
            MedicationsPerICSR_DropDownList.CssClass = CssClassFailure
            MedicationsPerICSR_DropDownList.ToolTip = RelationCriteriaNotFullySpecified
            args.IsValid = False
        End If
    End Sub

    Protected Sub RelatednessCriteriaReporter_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        RelatednessCriteriaReporter_DropDownList.CssClass = CssClassSuccess
        RelatednessCriteriaReporter_DropDownList.ToolTip = String.Empty
    End Sub

    Protected Sub RelatednessCriteriaManufacturer_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        RelatednessCriteriaManufacturer_DropDownList.CssClass = CssClassSuccess
        RelatednessCriteriaManufacturer_DropDownList.ToolTip = String.Empty
    End Sub

    Protected Sub ExpectendessCriteria_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        ExpectendessCriteria_DropDownList.CssClass = CssClassSuccess
        ExpectendessCriteria_DropDownList.ToolTip = String.Empty
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click, ConfirmDeletion_Button.Click

        'Carry out Sub only if input is valid
        If Page.IsValid = False Then Exit Sub

        'Redirect to login if user is not logged in
        If Login_Status = False Then
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditRelation.aspx?" & Convert.ToString(Request.QueryString))
        End If

        'Store values from hidden fields in variables
        Dim CallReason As CallReasons
        Dim CurrentICSR_ID As Integer = Nothing
        Dim CurrentRelation_ID As Integer = Nothing
        Dim Delete As Boolean = False
        CallReason = CallReason_HiddenField.Value
        If CallReason = CallReasons.Create Then
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            CurrentRelation_ID = RelationID_HiddenField.Value
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Delete Then
            Delete = Delete_HiddenField.Value
        End If

        'Lock out if user does not have adequate edit rights
        LockoutCheck(CallReason, CurrentICSR_ID, tables.Relations, Title_Label, ButtonGroup_Div, Main_Table)

        'Format Controls
        AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
        If CallReason = CallReasons.Create Or CallReason = CallReasons.Update Then
            DropDownListDisabled(AEs_DropDownList)
            DropDownListDisabled(MedicationsPerICSR_DropDownList)
            DropDownListDisabled(RelatednessCriteriaReporter_DropDownList)
            DropDownListDisabled(RelatednessCriteriaManufacturer_DropDownList)
            DropDownListDisabled(ExpectendessCriteria_DropDownList)
        ElseIf CallReason = CallReasons.Delete Then
            AE_Row.Visible = False
            ICSRMedication_Row.Visible = False
            RelatednessReporter_Row.Visible = False
            RelatednessManufacturer_Row.Visible = False
            Expectendess_Row.Visible = False
        End If

        'Warn & abort if there are discrepancies between the data as shown at edit page load and as stored at save button click
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            Dim DiscrepancyString As String = String.Empty
            DiscrepancyString += DiscrepancyCheck(tables.Relations, fields.AE_ID, InputTypes.Integer, CurrentRelation_ID, AtEditPageLoad_AE_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.Relations, fields.MedicationPerICSR_ID, InputTypes.Integer, CurrentRelation_ID, AtEditPageLoad_MedicationPerICSR_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.Relations, fields.RelatednessCriterionReporter_ID, InputTypes.Integer, CurrentRelation_ID, AtEditPageLoad_RelatednessCriterionReporter_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.Relations, fields.RelatednessCriterionManufacturer_ID, InputTypes.Integer, CurrentRelation_ID, AtEditPageLoad_RelatednessCriterionManufacturer_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.Relations, fields.ExpectednessCriterion_ID, InputTypes.Integer, CurrentRelation_ID, AtEditPageLoad_ExpectendessCriterion_HiddenField)
            If DiscrepancyString <> String.Empty Then
                ShowDiscrepancyWarning(Status_Label, DiscrepancyString)
                Exit Sub
            End If
        End If

        'Store updates in database
        Dim UpdateCommand As New SqlCommand
        UpdateCommand.Connection = Connection
        If CallReason = CallReasons.Create Then
            UpdateCommand.CommandText = "INSERT INTO Relations (MedicationperICSR_ID, AE_ID, RelatednessCriterionReporter_ID, RelatednessCriterionManufacturer_ID, ExpectednessCriterion_ID) VALUES(@MedicationperICSR_ID, @AE_ID, CASE WHEN @RelatednessCriterionReporter_ID = 0 THEN NULL ELSE @RelatednessCriterionReporter_ID END, CASE WHEN @RelatednessCriterionManufacturer_ID = 0 THEN NULL ELSE @RelatednessCriterionManufacturer_ID END, CASE WHEN @ExpectednessCriterion_ID = 0 THEN NULL ELSE @ExpectednessCriterion_ID END)"
            UpdateCommand.Parameters.AddWithValue("@MedicationperICSR_ID", MedicationsPerICSR_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@AE_ID", AEs_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@RelatednessCriterionReporter_ID", RelatednessCriteriaReporter_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@RelatednessCriterionManufacturer_ID", RelatednessCriteriaManufacturer_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@ExpectednessCriterion_ID", ExpectendessCriteria_DropDownList.SelectedValue)
        ElseIf CallReason = CallReasons.Update Then
            UpdateCommand.CommandText = "UPDATE Relations SET AE_ID = @AE_ID, MedicationPerICSR_ID = @MedicationPerICSR_ID, RelatednessCriterionReporter_ID = (CASE WHEN @RelatednessCriterionReporter_ID = 0 THEN NULL ELSE @RelatednessCriterionReporter_ID END), RelatednessCriterionManufacturer_ID = (CASE WHEN @RelatednessCriterionManufacturer_ID = 0 THEN NULL ELSE @RelatednessCriterionManufacturer_ID END), ExpectednessCriterion_ID = (CASE WHEN @ExpectednessCriterion_ID = 0 THEN NULL ELSE @ExpectednessCriterion_ID END) WHERE ID = @CurrentRelation_ID"
            UpdateCommand.Parameters.AddWithValue("@AE_ID", AEs_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@MedicationPerICSR_ID", MedicationsPerICSR_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@RelatednessCriterionReporter_ID", RelatednessCriteriaReporter_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@RelatednessCriterionManufacturer_ID", RelatednessCriteriaManufacturer_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@ExpectednessCriterion_ID", ExpectendessCriteria_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@CurrentRelation_ID", CurrentRelation_ID)
        ElseIf CallReason = CallReasons.Delete Then
            UpdateCommand.CommandText = "DELETE FROM Relations WHERE ID = @CurrentRelation_ID"
            UpdateCommand.Parameters.AddWithValue("@CurrentRelation_ID", CurrentRelation_ID)
        End If
        SqlUpdate(Me, UpdateCommand)

        'Add audit trail entry
        Dim EntryString As String = HistoryDatabasebUpdateIntro
        If CallReason = CallReasons.Create Then
            Dim NewRelation_ID As DataTable = SqlRead(Me, "SELECT TOP 1 ID FROM Relations WHERE MedicationperICSR_ID = " & TryCType(MedicationsPerICSR_DropDownList.SelectedValue, InputTypes.Integer) & " ORDER BY ID DESC")
            CurrentRelation_ID = NewRelation_ID.Rows(0)(0)
            EntryString += NewReportIntro("Relation", CurrentRelation_ID)
        ElseIf CallReason = CallReasons.Delete Then
            EntryString += DeleteReportIntro("Relation", CurrentRelation_ID)
        End If
        EntryString += HistoryEntryReferencedValue(CallReason, "Relation", CurrentRelation_ID, "Event", tables.AEs, fields.MedDRATerm, TryCType(AtEditPageLoad_AE_HiddenField.Value, InputTypes.Integer), TryCType(AEs_DropDownList.SelectedValue, InputTypes.Integer))
        EntryString += HistoryEntryReferencedValue(CallReason, "Relation", CurrentRelation_ID, "Medication", tables.Medications, fields.Name, TryCType(AtEditPageLoad_MedicationPerICSR_HiddenField.Value, InputTypes.Integer), ParentID(tables.Medications, tables.MedicationsPerICSR, fields.Medication_ID, TryCType(MedicationsPerICSR_DropDownList.SelectedValue, InputTypes.Integer)))
        EntryString += HistoryEntryReferencedValue(CallReason, "Relation", CurrentRelation_ID, "Relatedness as per Reporter", tables.RelatednessCriteriaReporter, fields.Name, TryCType(AtEditPageLoad_RelatednessCriterionReporter_HiddenField.Value, InputTypes.Integer), TryCType(RelatednessCriteriaReporter_DropDownList.SelectedValue, InputTypes.Integer))
        EntryString += HistoryEntryReferencedValue(CallReason, "Relation", CurrentRelation_ID, "Relatedness as per Manufacturer", tables.RelatednessCriteriaManufacturer, fields.Name, TryCType(AtEditPageLoad_RelatednessCriterionManufacturer_HiddenField.Value, InputTypes.Integer), TryCType(RelatednessCriteriaManufacturer_DropDownList.SelectedValue, InputTypes.Integer))
        EntryString += HistoryEntryReferencedValue(CallReason, "Relation", CurrentRelation_ID, "Expectedness", tables.ExpectednessCriteria, fields.Name, TryCType(AtEditPageLoad_ExpectendessCriterion_HiddenField.Value, InputTypes.Integer), TryCType(ExpectendessCriteria_DropDownList.SelectedValue, InputTypes.Integer))
        SaveAuditTrailEntry(Me, CurrentICSR_ID, LoggedIn_User_ID, EntryString)

    End Sub

End Class
