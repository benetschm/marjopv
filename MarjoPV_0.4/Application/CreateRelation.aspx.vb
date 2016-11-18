Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_CreateRelation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Add Relation"
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                'Check if user has create rights and lock out if not
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Relations, fields.Create) = True Then
                    'Format Controls depending on user edit rights for each control
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Relations, fields.AE_ID) Then
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
                    'Populate AEs_DropDownlist
                    AEs_DropDownList.DataSource = CreateAEsOfCurrentICSRDropDownListDatatable(CurrentICSR_ID)
                    AEs_DropDownList.DataValueField = "ID"
                    AEs_DropDownList.DataTextField = "Name"
                    AEs_DropDownList.DataBind()
                    'Populate MedicationsPerICSR_DropDownList
                    MedicationsPerICSR_DropDownList.DataSource = CreateSuspectedDrugsOfCurrentICSRDropDownListDatatable(CurrentICSR_ID)
                    MedicationsPerICSR_DropDownList.DataValueField = "ID"
                    MedicationsPerICSR_DropDownList.DataTextField = "Name"
                    MedicationsPerICSR_DropDownList.DataBind()
                    'Populate RelatednessCriteriaReporter_DropDownList
                    RelatednessCriteriaReporter_DropDownList.DataSource = CreateDropDownListDatatable(tables.RelatednessCriteriaReporter)
                    RelatednessCriteriaReporter_DropDownList.DataValueField = "ID"
                    RelatednessCriteriaReporter_DropDownList.DataTextField = "Name"
                    RelatednessCriteriaReporter_DropDownList.DataBind()
                    'Populate RelatednessCriteriaManufacturer_DropDownList
                    RelatednessCriteriaManufacturer_DropDownList.DataSource = CreateDropDownListDatatable(tables.RelatednessCriteriaManufacturer)
                    RelatednessCriteriaManufacturer_DropDownList.DataValueField = "ID"
                    RelatednessCriteriaManufacturer_DropDownList.DataTextField = "Name"
                    RelatednessCriteriaManufacturer_DropDownList.DataBind()
                    'Populate ExpectendessCriteria_DropDownList
                    ExpectendessCriteria_DropDownList.DataSource = CreateDropDownListDatatable(tables.ExpectednessCriteria)
                    ExpectendessCriteria_DropDownList.DataValueField = "ID"
                    ExpectendessCriteria_DropDownList.DataTextField = "Name"
                    ExpectendessCriteria_DropDownList.DataBind()
                Else 'Lock out user if he/she does not have create rights
                    Title_Label.Text = Lockout_Text
                    ButtonGroup_Div.Visible = False
                    Main_Table.Visible = False
                End If
            Else 'Redirect user to login if he/she is not logged in
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/CreateRelation.aspx?ICSRID=" & CurrentICSR_ID)
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click() Handles Cancel_Button.Click, ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub RelationCriteria_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim CurrentICSR_ID = ICSRID_HiddenField.Value
        If SelectionValidator(AEs_DropDownList) = True And SelectionValidator(MedicationsPerICSR_DropDownList) = True Then 'If AE and Medication were selected
            'Check whether relation is unique in current ICSR and fail validation if not
            Dim RelationDuplicationFound As Boolean = False
            Dim RelationDuplicationReadCommand As New SqlCommand("SELECT CASE WHEN Relations.AE_ID IS NULL THEN 0 ELSE Relations.AE_ID END AS AE_ID, CASE WHEN Relations.MedicationperICSR_ID IS NULL THEN 0 ELSE Relations.MedicationperICSR_ID END AS MedicationperICSR_ID FROM Relations INNER JOIN AEs ON AEs.ID = Relations.AE_ID WHERE AEs.ICSR_ID = @CurrentICSR_ID", Connection)
            RelationDuplicationReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
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
        Else 'If AE or Medication was not selected
            AEs_DropDownList.CssClass = CssClassFailure
            AEs_DropDownList.ToolTip = RelationCriteriaNotFullySpecified
            MedicationsPerICSR_DropDownList.CssClass = CssClassFailure
            MedicationsPerICSR_DropDownList.ToolTip = RelationCriteriaNotFullySpecified
            args.IsValid = False
        End If
    End Sub

    Protected Sub RelatednessCriteriaReporter_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(RelatednessCriteriaReporter_DropDownList) = True Then
            RelatednessCriteriaReporter_DropDownList.CssClass = CssClassSuccess
            RelatednessCriteriaReporter_DropDownList.ToolTip = String.Empty
        Else
            RelatednessCriteriaReporter_DropDownList.CssClass = CssClassFailure
            RelatednessCriteriaReporter_DropDownList.ToolTip = SelectorValidationFailToolTip
        End If
    End Sub

    Protected Sub RelatednessCriteriaManufacturer_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(RelatednessCriteriaManufacturer_DropDownList) = True Then
            RelatednessCriteriaManufacturer_DropDownList.CssClass = CssClassSuccess
            RelatednessCriteriaManufacturer_DropDownList.ToolTip = String.Empty
        Else
            RelatednessCriteriaManufacturer_DropDownList.CssClass = CssClassFailure
            RelatednessCriteriaManufacturer_DropDownList.ToolTip = SelectorValidationFailToolTip
        End If
    End Sub

    Protected Sub ExpectendessCriteria_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(ExpectendessCriteria_DropDownList) = True Then
            ExpectendessCriteria_DropDownList.CssClass = CssClassSuccess
            ExpectendessCriteria_DropDownList.ToolTip = String.Empty
        Else
            ExpectendessCriteria_DropDownList.CssClass = CssClassFailure
            ExpectendessCriteria_DropDownList.ToolTip = SelectorValidationFailToolTip
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            'Write new dataset to database
            Dim InsertCommand As New SqlCommand("INSERT INTO Relations (MedicationperICSR_ID, AE_ID, RelatednessCriterionReporter_ID, RelatednessCriterionManufacturer_ID, ExpectednessCriterion_ID) VALUES(@MedicationperICSR_ID, @AE_ID, CASE WHEN @RelatednessCriterionReporter_ID = 0 THEN NULL ELSE @RelatednessCriterionReporter_ID END, CASE WHEN @RelatednessCriterionManufacturer_ID = 0 THEN NULL ELSE @RelatednessCriterionManufacturer_ID END, CASE WHEN @ExpectednessCriterion_ID = 0 THEN NULL ELSE @ExpectednessCriterion_ID END)", Connection)
            InsertCommand.Parameters.AddWithValue("@MedicationperICSR_ID", MedicationsPerICSR_DropDownList.SelectedValue)
            InsertCommand.Parameters.AddWithValue("@AE_ID", AEs_DropDownList.SelectedValue)
            InsertCommand.Parameters.AddWithValue("@RelatednessCriterionReporter_ID", RelatednessCriteriaReporter_DropDownList.SelectedValue)
            InsertCommand.Parameters.AddWithValue("@RelatednessCriterionManufacturer_ID", RelatednessCriteriaManufacturer_DropDownList.SelectedValue)
            InsertCommand.Parameters.AddWithValue("@ExpectednessCriterion_ID", ExpectendessCriteria_DropDownList.SelectedValue)
            Try
                Connection.Open()
                InsertCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Add change history entry with database updates
            Dim NewReadCommand As New SqlCommand("SELECT TOP 1 Relations.ID, Relations.MedicationPerICSR_ID, Relations.AE_ID, CASE WHEN Relations.RelatednessCriterionReporter_ID IS NULL THEN 0 ELSE Relations.RelatednessCriterionReporter_ID END AS RelatednessCriterionReporter_ID, CASE WHEN Relations.RelatednessCriterionManufacturer_ID IS NULL THEN 0 ELSE Relations.RelatednessCriterionManufacturer_ID END AS RelatednessCriterionManufacturer_ID, CASE WHEN Relations.ExpectednessCriterion_ID IS NULL THEN 0 ELSE Relations.ExpectednessCriterion_ID END AS ExpectednessCriterion_ID FROM Relations INNER JOIN AEs ON Relations.AE_ID = AEs.ID WHERE AEs.ICSR_ID = @CurrentICSR_ID ORDER BY ID DESC", Connection)
            NewReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            Dim NewRelation_ID As Integer = Nothing
            Dim NewMedicationPerICSR_ID As Integer = Nothing
            Dim NewAE_ID As Integer = Nothing
            Dim NewRelatednessCriterionReporter_ID As Integer = Nothing
            Dim NewRelatednessCriterionManufacturer_ID As Integer = Nothing
            Dim NewExpectednessCriterion_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim NewReader As SqlDataReader = NewReadCommand.ExecuteReader()
                While NewReader.Read()
                    NewRelation_ID = NewReader.GetInt32(0)
                    NewMedicationPerICSR_ID = NewReader.GetInt32(1)
                    NewAE_ID = NewReader.GetInt32(2)
                    NewRelatednessCriterionReporter_ID = NewReader.GetInt32(3)
                    NewRelatednessCriterionManufacturer_ID = NewReader.GetInt32(4)
                    NewExpectednessCriterion_ID = NewReader.GetInt32(5)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Dim EntryString As String = String.Empty
            'EntryString = HistoryDatabasebUpdateIntro
            'EntryString += NewReportIntro("Relation", NewRelation_ID)
            'EntryString += HistoryEntryReferencedValue("Relation", NewRelation_ID, "Medication", tables.Medications, fields.Name, Nothing, ParentID(tables.Medications, tables.MedicationsPerICSR, fields.Medication_ID, NewMedicationPerICSR_ID))
            'EntryString += HistoryEntryReferencedValue("Relation", NewRelation_ID, "Event", tables.AEs, fields.MedDRATerm, Nothing, NewAE_ID)
            'EntryString += HistoryEntryReferencedValue("Relation", NewRelation_ID, "Relatedness as per Reporter", tables.RelatednessCriteriaReporter, fields.Name, Nothing, NewRelatednessCriterionReporter_ID)
            'EntryString += HistoryEntryReferencedValue("Relation", NewRelation_ID, "Relatedness as per Manufacturer", tables.RelatednessCriteriaManufacturer, fields.Name, Nothing, NewRelatednessCriterionManufacturer_ID)
            'EntryString += HistoryEntryReferencedValue("Relation", NewRelation_ID, "Expectedness", tables.ExpectednessCriteria, fields.Name, Nothing, NewExpectednessCriterion_ID)
            'EntryString += HistoryDatabasebUpdateOutro
            'Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO ICSRHistories(ICSR_ID, User_ID, Timepoint, Entry) VALUES (@ICSR_ID, @User_ID, @Timepoint, @Entry)", Connection)
            'InsertHistoryEntryCommand.Parameters.AddWithValue("@ICSR_ID", CurrentICSR_ID)
            'InsertHistoryEntryCommand.Parameters.AddWithValue("@User_ID", LoggedIn_User_ID)
            'InsertHistoryEntryCommand.Parameters.AddWithValue("@Timepoint", Now())
            'InsertHistoryEntryCommand.Parameters.AddWithValue("@Entry", EntryString)
            'Try
            '    Connection.Open()
            '    InsertHistoryEntryCommand.ExecuteNonQuery()
            'Catch ex As Exception
            '    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            '    Exit Sub
            'Finally
            '    Connection.Close()
            'End Try
            'Format Page Controls
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
            DropDownListDisabled(AEs_DropDownList)
            DropDownListDisabled(MedicationsPerICSR_DropDownList)
            DropDownListDisabled(RelatednessCriteriaReporter_DropDownList)
            DropDownListDisabled(RelatednessCriteriaManufacturer_DropDownList)
            DropDownListDisabled(ExpectendessCriteria_DropDownList)
        End If
    End Sub

End Class
