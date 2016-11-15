Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditRelation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            RelationID_HiddenField.Value = Request.QueryString("RelationID")
            Dim CurrentRelation_ID As Integer = RelationID_HiddenField.Value
            Delete_HiddenField.Value = Request.QueryString("Delete")
            Dim Delete As Boolean = False
            If Delete_HiddenField.Value = "True" Then
                Delete = True
            End If
            Dim CurrentICSR_ID = ICSRIDFOfRelation(CurrentRelation_ID)
            ICSRID_HiddenField.Value = CurrentICSR_ID
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                If Delete = False Then
                    Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Edit Relation " & CurrentRelation_ID & " Details"
                ElseIf Delete = True Then
                    Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Delete Relation " & CurrentRelation_ID & " Details"
                End If
                'Check if user has edit rights and lock out if not
                If Delete = False Then
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Relations, fields.Edit) = True Then
                        AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
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
                    Else 'Lock out user if he/she does not have create rights
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
                ElseIf Delete = True Then
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.Relations, fields.Delete) = True Then
                        AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                        DropDownListDisabled(AEs_DropDownList)
                        DropDownListDisabled(MedicationsPerICSR_DropDownList)
                        DropDownListDisabled(RelatednessCriteriaReporter_DropDownList)
                        DropDownListDisabled(RelatednessCriteriaManufacturer_DropDownList)
                        DropDownListDisabled(ExpectendessCriteria_DropDownList)
                    Else 'Lock out user if he/she does not have create rights
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
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
                'Populate controls from database
                Dim AtEditPageLoadReadCommand As New SqlCommand("SELECT Relations.AE_ID, Relations.MedicationPerICSR_ID, CASE WHEN Relations.RelatednessCriterionReporter_ID IS NULL THEN 0 ELSE Relations.RelatednessCriterionReporter_ID END AS RelatednessCriterionReporter_ID, CASE WHEN Relations.RelatednessCriterionManufacturer_ID IS NULL THEN 0 ELSE Relations.RelatednessCriterionManufacturer_ID END AS RelatednessCriterionManufacturer_ID, CASE WHEN Relations.ExpectednessCriterion_ID IS NULL THEN 0 ELSE Relations.ExpectednessCriterion_ID END AS ExpectednessCriterion_ID FROM Relations INNER JOIN AEs ON Relations.AE_ID = AEs.ID WHERE Relations.ID = @CurrentRelation_ID", Connection)
                AtEditPageLoadReadCommand.Parameters.AddWithValue("@CurrentRelation_ID", CurrentRelation_ID)
                Try
                    Connection.Open()
                    Dim AtEditPageLoadReader As SqlDataReader = AtEditPageLoadReadCommand.ExecuteReader()
                    While AtEditPageLoadReader.Read()
                        AEs_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(0)
                        AtEditPageLoad_AE_HiddenField.Value = AtEditPageLoadReader.GetInt32(0)
                        MedicationsPerICSR_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(1)
                        AtEditPageLoad_MedicationPerICSR_HiddenField.Value = AtEditPageLoadReader.GetInt32(1)
                        RelatednessCriteriaReporter_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(2)
                        AtEditPageLoad_RelatednessCriterionReporter_HiddenField.Value = AtEditPageLoadReader.GetInt32(2)
                        RelatednessCriteriaManufacturer_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(3)
                        AtEditPageLoad_RelatednessCriteriaManufacturer_HiddenField.Value = AtEditPageLoadReader.GetInt32(3)
                        ExpectendessCriteria_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(4)
                        AtEditPageLoad_ExpectendessCriterion_HiddenField.Value = AtEditPageLoadReader.GetInt32(4)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            Else 'Redirect user to login if he/she is not logged in
                If Delete = False Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditRelation.aspx?RelationID=" & CurrentRelation_ID)
                ElseIf Delete = True Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditRelation.aspx?RelationID=" & CurrentRelation_ID & "&Delete=True")
                End If
            End If
        End If
    End Sub

    Protected Sub ReturnToICSROverview() Handles Cancel_Button.Click, ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub RelationCriteria_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim CurrentRelation_ID = RelationID_HiddenField.Value
        Dim CurrentICSR_ID = ICSRIDFOfRelation(CurrentRelation_ID)
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
        If Page.IsValid = True Then
            Dim CurrentRelation_ID As Integer = RelationID_HiddenField.Value
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            'Retrieve values as present in database at edit page load and store in variables to use when checking for database update conflicts (see page load event)
            Dim AtEditPageLoad_AE_ID As Integer = TryCType(AtEditPageLoad_AE_HiddenField.Value, InputTypes.Number)
            Dim AtEditPageLoad_MedicationPerICSR_ID As Integer = AtEditPageLoad_MedicationPerICSR_HiddenField.Value
            Dim AtEditPageLoad_RelatednessCriterionReporter_ID As Integer = AtEditPageLoad_RelatednessCriterionReporter_HiddenField.Value
            Dim AtEditPageLoad_RelatednessCriteriaManufacturer_ID As Integer = AtEditPageLoad_RelatednessCriteriaManufacturer_HiddenField.Value
            Dim AtEditPageLoad_ExpectendessCriterion_ID As Integer = AtEditPageLoad_ExpectendessCriterion_HiddenField.Value
            'Store values as present in database when save button is clicked in variables
            Dim AtSaveButtonClickReadCommand As New SqlCommand("SELECT Relations.AE_ID, Relations.MedicationPerICSR_ID, CASE WHEN Relations.RelatednessCriterionReporter_ID IS NULL THEN 0 ELSE Relations.RelatednessCriterionReporter_ID END AS RelatednessCriterionReporter_ID, CASE WHEN Relations.RelatednessCriterionManufacturer_ID IS NULL THEN 0 ELSE Relations.RelatednessCriterionManufacturer_ID END AS RelatednessCriterionManufacturer_ID, CASE WHEN Relations.ExpectednessCriterion_ID IS NULL THEN 0 ELSE Relations.ExpectednessCriterion_ID END AS ExpectednessCriterion_ID FROM Relations INNER JOIN AEs ON Relations.AE_ID = AEs.ID WHERE Relations.ID = @CurrentRelation_ID", Connection)
            AtSaveButtonClickReadCommand.Parameters.AddWithValue("@CurrentRelation_ID", CurrentRelation_ID)
            Dim AtSaveButtonClick_AE_ID As Integer = Nothing
            Dim AtSaveButtonClick_MedicationPerICSR_ID As Integer = Nothing
            Dim AtSaveButtonClick_RelatednessCriterionReporter_ID As Integer = Nothing
            Dim AtSaveButtonClick_RelatednessCriterionManufacturer_ID As Integer = Nothing
            Dim AtSaveButtonClick_ExpectednessCriterion_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim AtSaveButtonClickReader As SqlDataReader = AtSaveButtonClickReadCommand.ExecuteReader()
                While AtSaveButtonClickReader.Read()
                    AtSaveButtonClick_AE_ID = AtSaveButtonClickReader.GetInt32(0)
                    AtSaveButtonClick_MedicationPerICSR_ID = AtSaveButtonClickReader.GetInt32(1)
                    AtSaveButtonClick_RelatednessCriterionReporter_ID = AtSaveButtonClickReader.GetInt32(2)
                    AtSaveButtonClick_RelatednessCriterionManufacturer_ID = AtSaveButtonClickReader.GetInt32(3)
                    AtSaveButtonClick_ExpectednessCriterion_ID = AtSaveButtonClickReader.GetInt32(4)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Check for discrepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked
            Dim DiscrepancyString As String = String.Empty
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_AE_ID, AtSaveButtonClick_AE_ID, "Event")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_MedicationPerICSR_ID, AtSaveButtonClick_MedicationPerICSR_ID, "ICSR Medication")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_RelatednessCriterionReporter_ID, AtSaveButtonClick_RelatednessCriterionReporter_ID, "Relatedness as per Reporter")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_RelatednessCriteriaManufacturer_ID, AtSaveButtonClick_RelatednessCriterionManufacturer_ID, "Relatedness as per Manufacturer")
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ExpectendessCriterion_ID, AtSaveButtonClick_ExpectednessCriterion_ID, "Expectedness")
            'If Discprepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked are found, show warning and abort update
            If DiscrepancyString <> String.Empty Then
                AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
                Status_Label.Style.Add("text-align", "left")
                Status_Label.Style.Add("height", "auto")
                Status_Label.Text = DiscrepancyStringIntro & DiscrepancyString & DiscrepancyStringOutro
                Status_Label.CssClass = "form-control alert-danger"
                DropDownListDisabled(AEs_DropDownList)
                DropDownListDisabled(MedicationsPerICSR_DropDownList)
                DropDownListDisabled(RelatednessCriteriaReporter_DropDownList)
                DropDownListDisabled(RelatednessCriteriaManufacturer_DropDownList)
                DropDownListDisabled(ExpectendessCriteria_DropDownList)
                Exit Sub
            End If
            'If no discrepancies were found between database contents as present when edit page was loaded and database contents as present when save button is clicked, write updates to database
            Dim UpdateCommand As New SqlCommand
            UpdateCommand.Connection = Connection
            If sender Is SaveUpdates_Button Then
                UpdateCommand.CommandText = "UPDATE Relations SET AE_ID = @AE_ID, MedicationPerICSR_ID = @MedicationPerICSR_ID, RelatednessCriterionReporter_ID = (CASE WHEN @RelatednessCriterionReporter_ID = 0 THEN NULL ELSE @RelatednessCriterionReporter_ID END), RelatednessCriterionManufacturer_ID = (CASE WHEN @RelatednessCriterionManufacturer_ID = 0 THEN NULL ELSE @RelatednessCriterionManufacturer_ID END), ExpectednessCriterion_ID = (CASE WHEN @ExpectednessCriterion_ID = 0 THEN NULL ELSE @ExpectednessCriterion_ID END) WHERE ID = @CurrentRelation_ID"
                UpdateCommand.Parameters.AddWithValue("@AE_ID", AEs_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@MedicationPerICSR_ID", MedicationsPerICSR_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@RelatednessCriterionReporter_ID", RelatednessCriteriaReporter_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@RelatednessCriterionManufacturer_ID", RelatednessCriteriaManufacturer_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@ExpectednessCriterion_ID", ExpectendessCriteria_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@CurrentRelation_ID", CurrentRelation_ID)
            ElseIf sender Is ConfirmDeletion_Button Then
                UpdateCommand.CommandText = "DELETE FROM Relations WHERE ID = @CurrentRelation_ID"
                UpdateCommand.Parameters.AddWithValue("@CurrentRelation_ID", CurrentRelation_ID)
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
            Dim Updated_AE_ID As Integer = Nothing
            Dim Updated_MedicationPerICSR_ID As Integer = Nothing
            Dim Updated_RelatednessCriterionReporter_ID As Integer = Nothing
            Dim Updated_RelatednessCriterionManufacturer_ID As Integer = Nothing
            Dim Updated_ExpectednessCriterion_ID As Integer = Nothing
            If sender Is SaveUpdates_Button Then
                Dim UpdatedReadCommand As New SqlCommand("SELECT Relations.AE_ID, Relations.MedicationPerICSR_ID, CASE WHEN Relations.RelatednessCriterionReporter_ID IS NULL THEN 0 ELSE Relations.RelatednessCriterionReporter_ID END AS RelatednessCriterionReporter_ID, CASE WHEN Relations.RelatednessCriterionManufacturer_ID IS NULL THEN 0 ELSE Relations.RelatednessCriterionManufacturer_ID END AS RelatednessCriterionManufacturer_ID, CASE WHEN Relations.ExpectednessCriterion_ID IS NULL THEN 0 ELSE Relations.ExpectednessCriterion_ID END AS ExpectednessCriterion_ID FROM Relations INNER JOIN AEs ON Relations.AE_ID = AEs.ID WHERE Relations.ID = @CurrentRelation_ID", Connection)
                UpdatedReadCommand.Parameters.AddWithValue("@CurrentRelation_ID", CurrentRelation_ID)
                Try
                    Connection.Open()
                    Dim UpdatedReader As SqlDataReader = UpdatedReadCommand.ExecuteReader()
                    While UpdatedReader.Read()
                        Updated_AE_ID = UpdatedReader.GetInt32(0)
                        Updated_MedicationPerICSR_ID = UpdatedReader.GetInt32(1)
                        Updated_RelatednessCriterionReporter_ID = UpdatedReader.GetInt32(2)
                        Updated_RelatednessCriterionManufacturer_ID = UpdatedReader.GetInt32(3)
                        Updated_ExpectednessCriterion_ID = UpdatedReader.GetInt32(4)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            End If
            'Compare old and new variables to generate EntryString for Change History Entry
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            If sender Is ConfirmDeletion_Button Then
                EntryString += DeleteReportIntro("Relation", CurrentRelation_ID)
            End If
            If Updated_AE_ID <> AtSaveButtonClick_AE_ID Then
                EntryString += HistoryEntryReferencedValue("Relation", CurrentRelation_ID, "Event", tables.AEs, fields.MedDRATerm, AtSaveButtonClick_AE_ID, Updated_AE_ID)
            End If
            If Updated_MedicationPerICSR_ID <> AtSaveButtonClick_MedicationPerICSR_ID Then
                EntryString += HistoryEntryReferencedValue("Relation", CurrentRelation_ID, "Medication", tables.Medications, fields.Name, ParentID(tables.Medications, tables.MedicationsPerICSR, fields.Medication_ID, AtSaveButtonClick_MedicationPerICSR_ID), ParentID(tables.Medications, tables.MedicationsPerICSR, fields.Medication_ID, Updated_MedicationPerICSR_ID))
            End If
            If Updated_RelatednessCriterionReporter_ID <> AtSaveButtonClick_RelatednessCriterionReporter_ID Then
                EntryString += HistoryEntryReferencedValue("Relation", CurrentRelation_ID, "Relatedness as per Reporter", tables.RelatednessCriteriaReporter, fields.Name, AtSaveButtonClick_RelatednessCriterionReporter_ID, Updated_RelatednessCriterionReporter_ID)
            End If
            If Updated_RelatednessCriterionManufacturer_ID <> AtSaveButtonClick_RelatednessCriterionManufacturer_ID Then
                EntryString += HistoryEntryReferencedValue("Relation", CurrentRelation_ID, "Relatedness as per Manufacturer", tables.RelatednessCriteriaManufacturer, fields.Name, AtSaveButtonClick_RelatednessCriterionManufacturer_ID, Updated_RelatednessCriterionManufacturer_ID)
            End If
            If Updated_ExpectednessCriterion_ID <> AtSaveButtonClick_ExpectednessCriterion_ID Then
                EntryString += HistoryEntryReferencedValue("Relation", CurrentRelation_ID, "Expectedness", tables.ExpectednessCriteria, fields.Name, AtSaveButtonClick_ExpectednessCriterion_ID, Updated_ExpectednessCriterion_ID)
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
                DropDownListDisabled(AEs_DropDownList)
                DropDownListDisabled(MedicationsPerICSR_DropDownList)
                DropDownListDisabled(RelatednessCriteriaReporter_DropDownList)
                DropDownListDisabled(RelatednessCriteriaManufacturer_DropDownList)
                DropDownListDisabled(ExpectendessCriteria_DropDownList)
            ElseIf sender Is ConfirmDeletion_Button Then
                AE_Row.Visible = False
                ICSRMedication_Row.Visible = False
                RelatednessReporter_Row.Visible = False
                RelatednessManufacturer_Row.Visible = False
                Expectendess_Row.Visible = False
            End If
        End If
    End Sub

End Class
