Imports Microsoft.VisualBasic
Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports System.Data.SqlTypes


Public Class GlobalCode

    Public Enum [tables]
        Companies
        ICSRs
        Medications
        Reports
        AEs
        Users
        ICSRStatuses
        ReportStatuses
        YearsOfBirth
        Genders
        IsSerious
        SeriousnessCriteria
        ReportTypes
        ReportComplexities
        ReportSources
        ExpeditedReportingRequired
        ExpeditedReportingDone
        MedicationTypes
        AdministrationRoutes
        ActiveIngredients
        MedicationIngredients
        MedicationsInCountries
        MedicationsPerICSR
        MedicationPerICSRRoles
        Units
        Countries
        DoseTypes
        DrugActions
        DechallengeResults
        RechallengeResults
        Outcomes
        Relations
        RelatednessCriteriaReporter
        RelatednessCriteriaManufacturer
        ExpectednessCriteria
        ICSRHistories
        MedicationHistories
        ICSRsAttachedFiles
        MedicationsAttachedFiles
        MedicalHistories
        Roles
    End Enum

    Public Enum [fields]
        Create
        Edit
        Delete
        Company_ID
        Assignee_ID
        Name
        ICSRStatus_ID
        Patient_Initials
        Patient_YearOfBirth_ID
        Patient_Gender_ID
        IsSerious
        SeriousnessCriterion_ID
        Narrative
        CompanyComment
        ReportComplexity_ID
        ReportType_ID
        ReportSource_ID
        ReportStatus_ID
        ReporterName
        ReporterAddress
        ReporterPhone
        ReporterFax
        ReporterEmail
        Received
        Due
        ExpeditedReportingRequired_ID
        ExpeditedReportingDone_ID
        ExpeditedReportingDate
        MedicationType_ID
        GenericName
        AdministrationRoute_ID
        ActiveIngredient_ID
        Quantity
        Unit_ID
        Country_ID
        DoseType_ID
        Medication_ID
        TotalDailyDose
        Allocations
        Start
        [Stop]
        DrugAction_ID
        MedicationPerICSRRole_ID
        MedDRATerm
        Outcome_ID
        DechallengeResult_ID
        RechallengeResult_ID
        Relation_ID
        AE_ID
        MedicationPerICSR_ID
        RelatednessCriterionReporter_ID
        RelatednessCriterionManufacturer_ID
        ExpectednessCriterion_ID
        Entry
        ICSR_ID
    End Enum

    Public Enum [InputTypes]
        [Date]
        Number
        Text
        Phone
        Email
        Selector
    End Enum

    Public Shared Sub AtEditPageLoadButtonsFormat(Status_Label As Label, SaveUpdates_Button As Button, Delete_Button As Button, ConfirmDeletion_Button As Button, Cancel_Button As Button, ReturnToICSROverview_Button As Button)
        If Status_Label IsNot Nothing Then
            Status_Label.Text = "Changes not saved"
            Status_Label.CssClass = "form-control alert-warning"
        End If
        If SaveUpdates_Button IsNot Nothing Then
            SaveUpdates_Button.Visible = True
        End If
        If Delete_Button IsNot Nothing Then
            Delete_Button.Visible = True
        End If
        If ConfirmDeletion_Button IsNot Nothing Then
            ConfirmDeletion_Button.Visible = False
        End If
        If Cancel_Button IsNot Nothing Then
            Cancel_Button.Visible = True
        End If
        If ReturnToICSROverview_Button IsNot Nothing Then
            ReturnToICSROverview_Button.Visible = False
        End If
    End Sub

    Public Shared Sub AtSaveButtonClickButtonsFormat(Status_Label As Label, SaveUpdates_Button As Button, Delete_Button As Button, ConfirmDeletion_Button As Button, Cancel_Button As Button, ReturnToOverview_Button As Button)
        If Status_Label IsNot Nothing Then
            Status_Label.Text = "Changes saved"
            Status_Label.CssClass = "form-control alert-success"
        End If
        If SaveUpdates_Button IsNot Nothing Then
            SaveUpdates_Button.Visible = False
        End If
        If Delete_Button IsNot Nothing Then
            Delete_Button.Visible = False
        End If
        If ConfirmDeletion_Button IsNot Nothing Then
            ConfirmDeletion_Button.Visible = False
        End If
        If Cancel_Button IsNot Nothing Then
            Cancel_Button.Visible = False
        End If
        If ReturnToOverview_Button IsNot Nothing Then
            ReturnToOverview_Button.Visible = True
        End If
    End Sub

    Public Shared Sub AtDeleteButtonClickButtonsFormat(Status_Label As Label, SaveUpdates_Button As Button, Delete_Button As Button, ConfirmDeletion_Button As Button, Cancel_Button As Button, ReturnToOverview_Button As Button)
        If Status_Label IsNot Nothing Then
            Status_Label.Text = "WARNING: Deletions cannot be reversed. Please confirm or cancel."
            Status_Label.CssClass = "form-control alert-danger"
        End If
        If SaveUpdates_Button IsNot Nothing Then
            SaveUpdates_Button.Visible = False
        End If
        If Delete_Button IsNot Nothing Then
            Delete_Button.Visible = False
        End If
        If ConfirmDeletion_Button IsNot Nothing Then
            ConfirmDeletion_Button.Visible = True
        End If
        If Cancel_Button IsNot Nothing Then
            Cancel_Button.Visible = True
        End If
        If ReturnToOverview_Button IsNot Nothing Then
            ReturnToOverview_Button.Visible = False
        End If
    End Sub

    Public Shared Sub AtConfirmDeletionButtonClickButtonsFormat(Status_Label As Label, SaveUpdates_Button As Button, Delete_Button As Button, ConfirmDeletion_Button As Button, Cancel_Button As Button, ReturnToOverview_Button As Button)
        If Status_Label IsNot Nothing Then
            Status_Label.Text = "Dataset deleted"
            Status_Label.CssClass = "form-control alert-danger"
        End If
        If SaveUpdates_Button IsNot Nothing Then
            SaveUpdates_Button.Visible = False
        End If
        If ConfirmDeletion_Button IsNot Nothing Then
            ConfirmDeletion_Button.Visible = False
        End If
        If Cancel_Button IsNot Nothing Then
            Cancel_Button.Visible = False
        End If
        If ReturnToOverview_Button IsNot Nothing Then
            ReturnToOverview_Button.Visible = True
        End If
    End Sub

    Public Shared Function CanEdit(group As tables, CurrentGroup_ID As Integer, table As tables, field As fields) As Boolean
        'Populate groupName
        Dim groupName As String = [Enum].GetName(GetType(tables), group)
        'Populate tableName
        Dim tableName As String = [Enum].GetName(GetType(tables), table)
        'Populate EditRightType
        Dim EditRightType As String = String.Empty
        If field = fields.Create Then 'If the global placeholder-field 'Create' was passed to the function
            EditRightType = "Create"
        ElseIf field = fields.Delete Then 'If the global placeholder-field 'Delete' was passed to the function
            EditRightType = "Delete"
        Else 'If any other field than the global placeholder-field 'Create' or 'Delete' was passed to the function
            EditRightType = "Edit"
        End If
        'Populate CurrentGroup_ID_Component
        Dim CurrentGroup_ID_Component As String = String.Empty
        If CurrentGroup_ID = Nothing Then
            CurrentGroup_ID_Component = String.Empty
        Else 'If CurrentGroup_ID <> Nothing was passed to the function
            CurrentGroup_ID_Component = " AND " & groupName & ".ID = " & CurrentGroup_ID
        End If
        'Populate fieldName
        Dim fieldName As String = String.Empty
        If field = fields.Create Or field = fields.Edit Or field = fields.Delete Then 'If the global placeholder-field 'Create', 'Edit' or 'Delete' was passed to the function
            fieldName = String.Empty
        Else 'If any other field thnat the global placeholder-field 'Create' or the global placeholder-field 'Edit' was passed to the function
            fieldName = [Enum].GetName(GetType(fields), field)
        End If
        Dim CanEditReadCommandString As String = "SELECT DISTINCT Can" & EditRightType & tableName & fieldName & " FROM Roles INNER JOIN RoleAllocations ON Roles.ID = RoleAllocations.Role_ID INNER JOIN Companies ON RoleAllocations.Company_ID = Companies.ID INNER JOIN " & groupName & " ON Companies.ID = " & groupName & ".Company_ID WHERE Companies.Active = @Active AND RoleAllocations.User_ID = @CurrentUser_ID" & CurrentGroup_ID_Component
        Dim CanEditReadCommand As New SqlCommand(CanEditReadCommandString, Connection)
        CanEditReadCommand.Parameters.AddWithValue("@Active", 1)
        CanEditReadCommand.Parameters.AddWithValue("@CurrentUser_ID", LoggedIn_User_ID)
        CanEditReadCommand.Parameters.AddWithValue("@CurrentGroup_ID", CurrentGroup_ID)
        Dim CanEditResult As Boolean = False
        Try
            Connection.Open()
            Dim CanEditReader As SqlDataReader = CanEditReadCommand.ExecuteReader()
            While CanEditReader.Read()
                If CanEditReader.GetBoolean(0) = True Then
                    CanEditResult = True
                End If
            End While
        Catch ex As Exception
            CanEditResult = False
        Finally
            Connection.Close()
        End Try
        Return CanEditResult
    End Function

    Public Shared Function AttachedFiles(Association_Table As tables, Association_ID As Integer) As DataTable
        Dim ReadCommandString As String = "SELECT AttachedFiles.ID AS AttachedFile_ID, " & [Enum].GetName(GetType(tables), Association_Table) & ".Company_ID, AttachedFiles.GUID, AttachedFiles.Name, AttachedFiles.Extension, AttachedFiles.Added FROM AttachedFiles JOIN " & [Enum].GetName(GetType(tables), Association_Table) & " ON " & [Enum].GetName(GetType(tables), Association_Table) & ".ID = AttachedFiles.Association_ID WHERE Association_Table = '" & [Enum].GetName(GetType(tables), Association_Table) & "' AND Association_ID = @Association_ID AND Removed IS NULL ORDER BY AttachedFiles.ID"
        Dim ReadCommand As New SqlCommand(ReadCommandString, Connection)
        ReadCommand.Parameters.AddWithValue("@Association_ID", Association_ID)
        Dim Datatable As New DataTable()
        Datatable.Columns.AddRange(New DataColumn(6) {
                                                                     New DataColumn("AttachedFile_ID", Type.GetType("System.Int32")),
                                                                     New DataColumn("Company_ID", Type.GetType("System.Int32")),
                                                                     New DataColumn("Association_ID", Type.GetType("System.Int32")),
                                                                     New DataColumn("GUID", Type.GetType("System.String")),
                                                                     New DataColumn("Name", Type.GetType("System.String")),
                                                                     New DataColumn("Extension", Type.GetType("System.String")),
                                                                     New DataColumn("Added", Type.GetType("System.DateTime"))
                                                                     })
        Dim AttachedFile_ID As Integer = Nothing
        Dim Company_ID As Integer = Nothing
        Dim GUID As String = String.Empty
        Dim Name As String = String.Empty
        Dim Extension As String = String.Empty
        Dim Added As DateTime = Date.MinValue
        Try
            Connection.Open()
            Dim Reader As SqlDataReader = ReadCommand.ExecuteReader()
            While Reader.Read()
                AttachedFile_ID = Reader.GetInt32(0)
                Company_ID = Reader.GetInt32(1)
                GUID = Reader.GetString(2)
                Name = Reader.GetString(3)
                Extension = Reader.GetString(4)
                Added = Reader.GetDateTime(5)
                Datatable.Rows.Add(AttachedFile_ID, Company_ID, Association_ID, GUID, Name, Extension, Added)
            End While
        Catch ex As Exception
            Datatable.Rows.Add(0, 0, 0, DatabaseConnectionErrorString, String.Empty, String.Empty, Date.MinValue)
        Finally
            Connection.Close()
        End Try
        Datatable.DefaultView.Sort = "AttachedFile_ID"
        Return Datatable
    End Function

    Public Shared Function History(Association_Table As tables, Association_Field As fields, Association_ID As Integer) As DataTable
        Dim ReadCommandString As String = "SELECT Users.Name AS User_Name, " & [Enum].GetName(GetType(tables), Association_Table) & ".Timepoint, " & [Enum].GetName(GetType(tables), Association_Table) & ".Entry FROM " & [Enum].GetName(GetType(tables), Association_Table) & " INNER JOIN Users ON " & [Enum].GetName(GetType(tables), Association_Table) & ".User_ID = Users.ID WHERE " & [Enum].GetName(GetType(tables), Association_Table) & "." & [Enum].GetName(GetType(fields), Association_Field) & " = @Association_ID ORDER BY " & [Enum].GetName(GetType(tables), Association_Table) & ".ID DESC"
        Dim ReadCommand As New SqlCommand(ReadCommandString, Connection)
        ReadCommand.Parameters.AddWithValue("@Association_ID", Association_ID)
        Dim Datatable As New DataTable()
        Datatable.Columns.AddRange(New DataColumn(2) {
                                                                     New DataColumn("User_Name", Type.GetType("System.String")),
                                                                     New DataColumn("Timepoint", Type.GetType("System.DateTime")),
                                                                     New DataColumn("Entry", Type.GetType("System.String"))
                                                                     })
        Dim User_Name As String = String.Empty
        Dim Timepoint As DateTime = Date.MinValue
        Dim Entry As String = String.Empty
        Try
            Connection.Open()
            Dim Reader As SqlDataReader = ReadCommand.ExecuteReader()
            While Reader.Read()
                User_Name = Reader.GetString(0)
                Timepoint = Reader.GetDateTime(1)
                Entry = Reader.GetString(2)
                Datatable.Rows.Add(User_Name, Timepoint, Entry)
            End While
        Catch ex As Exception
            Datatable.Rows.Add(DatabaseConnectionErrorString, Date.MinValue, String.Empty)
        Finally
            Connection.Close()
        End Try
        Return Datatable
    End Function

    Public Shared Function HistoryEntryReferencedValue(tableName As String, ID As Integer, fieldName As String, referencetable As tables, referencefield As fields, AtSaveButtonClick_ID As Integer, Updated_ID As Integer) As String
        Dim ReturnString As String = String.Empty
        If AtSaveButtonClick_ID <> Updated_ID Then
            Dim referencetableName As String = [Enum].GetName(GetType(tables), referencetable)
            Dim referencefieldName As String = [Enum].GetName(GetType(fields), referencefield)
            'Get name of value as present in the database before update
            Dim AtSaveButtonClick_Name As String = String.Empty
            Dim AtSaveButtonClickReadCommandString As String = "Select " & referencefieldName & " FROM " & referencetableName & " WHERE ID = @AtSaveButtonClick_ID"
            Dim AtSaveButtonClickReadCommand As New SqlCommand(AtSaveButtonClickReadCommandString, Connection)
            AtSaveButtonClickReadCommand.Parameters.AddWithValue("@AtSaveButtonClick_ID", AtSaveButtonClick_ID)
            Try
                Connection.Open()
                Dim AtSaveButtonClickReader As SqlDataReader = AtSaveButtonClickReadCommand.ExecuteReader()
                While AtSaveButtonClickReader.Read()
                    Try
                        AtSaveButtonClick_Name = AtSaveButtonClickReader.GetString(0)
                    Catch ex As Exception
                        AtSaveButtonClick_Name = String.Empty
                    End Try
                End While
            Catch ex As Exception
                AtSaveButtonClick_Name = DatabaseConnectionErrorString
            Finally
                Connection.Close()
            End Try
            'Get name of value as present in the database after update
            Dim Updated_Name As String = String.Empty
            Dim UpdatedReadCommandString As String = "Select " & referencefieldName & " FROM " & referencetableName & " WHERE ID = @Updated_ID"
            Dim UpdatedReadCommand As New SqlCommand(UpdatedReadCommandString, Connection)
            UpdatedReadCommand.Parameters.AddWithValue("@Updated_ID", Updated_ID)
            Try
                Connection.Open()
                Dim UpdatedReader As SqlDataReader = UpdatedReadCommand.ExecuteReader()
                While UpdatedReader.Read()
                    Try
                        Updated_Name = UpdatedReader.GetString(0)
                    Catch ex As Exception
                        Updated_Name = String.Empty
                    End Try
                End While
            Catch ex As Exception
                Updated_Name = DatabaseConnectionErrorString
            Finally
                Connection.Close()
            End Try
            ReturnString = HistoryEnrtyPlainValue(tableName, ID, fieldName, AtSaveButtonClick_Name, Updated_Name)
        Else
            ReturnString = String.Empty
        End If
        Return ReturnString
    End Function

    Public Shared Function DateOrDateMinValue(Value As Object) As DateTime
        Dim Result As DateTime = Date.MinValue
        DateTime.TryParse(Value, Result)
        If Result = "#1/1/2000 12:00:00 AM#" Then
            Result = Date.MinValue
        ElseIf Result = "#1/1/1900 12:00:00 AM#" Then
            Result = Date.MinValue
        End If
        Return Result
    End Function

    Public Shared Function DateStringOrEmpty(Value As Object) As String
        Dim DateResult As DateTime = DateOrDateMinValue(Value)
        Dim Result As String = String.Empty
        If DateResult = Date.MinValue Then
            Result = String.Empty
        Else
            Result = DateResult.ToString
        End If
        Return Result
    End Function

    Public Shared Function HistoryEnrtyPlainValue(tableName As String, ID As Integer, fieldName As String, AtSaveButtonClick_Value As Object, Updated_Value As Object) As String
        Dim ReturnString As String = String.Empty
        If AtSaveButtonClick_Value <> Updated_Value Then
            Dim AtSaveButtonClick_Name As String = String.Empty
            Dim Updated_Name As String = String.Empty
            If TypeOf Updated_Value Is String Then
                If AtSaveButtonClick_Value = String.Empty Then 'If no value is specified in the database before the update
                    AtSaveButtonClick_Name = "Nothing"
                Else
                    AtSaveButtonClick_Name = AtSaveButtonClick_Value
                End If
                If Updated_Value = String.Empty Then 'If no value is specified in the database after the update
                    Updated_Name = "Nothing"
                Else
                    Updated_Name = Updated_Value
                End If
            ElseIf TypeOf Updated_Value Is Integer Then
                If AtSaveButtonClick_Value = 0 Then 'If no value is specified in the database before the update
                    AtSaveButtonClick_Name = "Nothing"
                Else
                    AtSaveButtonClick_Name = CType(AtSaveButtonClick_Value, String)
                End If
                If Updated_Value = 0 Then 'If no value is specified in the database after the update
                    Updated_Name = "Nothing"
                Else
                    Updated_Name = CType(Updated_Value, String)
                End If
            ElseIf TypeOf Updated_Value Is DateTime Then
                If AtSaveButtonClick_Value = Date.MinValue Then 'If no value is specified in the database before the update
                    AtSaveButtonClick_Name = "Nothing"
                Else
                    AtSaveButtonClick_Name = DateTime.Parse(AtSaveButtonClick_Value).ToString("dd-MMM-yyyy")
                End If
                If Updated_Value = Date.MinValue Then 'If no value is specified in the database after the update
                    Updated_Name = "Nothing"
                Else
                    Updated_Name = DateTime.Parse(Updated_Value).ToString("dd-MMM-yyyy")
                End If
            Else
                If CType(AtSaveButtonClick_Value, String) = String.Empty Then 'If no value is specified in the database before the update
                    AtSaveButtonClick_Name = "Nothing"
                Else
                    AtSaveButtonClick_Name = CType(AtSaveButtonClick_Value, String)
                End If
                If CType(Updated_Value, String) = String.Empty Then 'If no value is specified in the database after the update
                    Updated_Name = "Nothing"
                Else
                    Updated_Name = CType(Updated_Value, String)
                End If
            End If
            If AtSaveButtonClick_Name <> Updated_Name Then 'If a change was made
                ReturnString = "<b>" & tableName & " " & ID & " " & fieldName & "</b> changed from '" & AtSaveButtonClick_Name & "' to '" & Updated_Name & "'</br>"
            Else
                ReturnString = String.Empty
            End If
        Else
            ReturnString = String.Empty
        End If
        Return ReturnString
    End Function

    Public Shared Function NewReportIntro(ReportName As String, NewReport_ID As Integer) As String
        Dim ReturnString As String = String.Empty
        ReturnString += "New " & ReportName & " created.</br>"
        ReturnString += "<b>" & ReportName & " ID</b> set to '" & NewReport_ID & "'</br>"
        Return ReturnString
    End Function

    Public Shared Function DeleteReportIntro(ReportName As String, Report_ID As Integer) As String
        Dim ReturnString As String = String.Empty
        ReturnString += ReportName & " " & Report_ID & " deleted.</br>"
        Return ReturnString
    End Function

    Public Shared Function CreateUpdateToStatusesDatatable(statusestable As tables, Report_ID As Integer) As DataTable
        'Store current ICSR_ID as CurrentICSR_ID
        Dim CurrentICSR_ID As Integer = Nothing
        If statusestable = tables.ReportStatuses Then 'Retrieve ICSR_ID from database if the function is called for a report
            Dim CurrentICSRIDReadCommand As New SqlCommand("SELECT ICSR_ID FROM Reports WHERE ID = @Report_ID", Connection)
            CurrentICSRIDReadCommand.Parameters.AddWithValue("@Report_ID", Report_ID)
            Try
                Connection.Open()
                Dim CurrentICSRIDReader As SqlDataReader = CurrentICSRIDReadCommand.ExecuteReader()
                While CurrentICSRIDReader.Read()
                    CurrentICSR_ID = CurrentICSRIDReader.GetInt32(0)
                End While
            Catch ex As Exception
                CurrentICSR_ID = Nothing
            Finally
                Connection.Close()
            End Try
        ElseIf statusestable = tables.ICSRStatuses Then 'Use argument 'ID' to populate CurrentICSR_ID if the function is called for an ICSR
            CurrentICSR_ID = Report_ID
        Else
            CurrentICSR_ID = Nothing
        End If
        'Store the current ICSR's Company_ID as variable
        Dim CurrentICSRCompany_ID As Integer = Nothing
        Dim CurrentICSRCompanyReadCommand As New SqlCommand("SELECT Company_ID FROM ICSRs WHERE ID = @CurrentICSR_ID", Connection)
        CurrentICSRCompanyReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
        Try
            Connection.Open()
            Dim CurrentICSRCompanyReader As SqlDataReader = CurrentICSRCompanyReadCommand.ExecuteReader()
            While CurrentICSRCompanyReader.Read()
                CurrentICSRCompany_ID = CurrentICSRCompanyReader.GetInt32(0)
            End While
        Catch ex As Exception
            CurrentICSRCompany_ID = Nothing
        Finally
            Connection.Close()
        End Try
        'Store the current status_ID, Status_Name and Status_SortOrder as variables
        Dim CurrentStatusReadCommandString As String = String.Empty
        If statusestable = tables.ICSRStatuses Then
            CurrentStatusReadCommandString = "SELECT ICSRStatuses.ID, ICSRStatuses.Name, ICSRStatuses.SortOrder FROM ICSRStatuses INNER JOIN ICSRs ON ICSRStatuses.ID = ICSRs.ICSRStatus_ID WHERE ICSRs.ID = @Report_ID"
        ElseIf statusestable = tables.ReportStatuses Then
            CurrentStatusReadCommandString = "SELECT ReportStatuses.ID, ReportStatuses.Name, ReportStatuses.SortOrder FROM ReportStatuses INNER JOIN Reports ON ReportStatuses.ID = Reports.ReportStatus_ID WHERE Reports.ID = @Report_ID"
        Else
            CurrentStatusReadCommandString = String.Empty
        End If
        Dim CurrentStatusReadCommand As New SqlCommand(CurrentStatusReadCommandString, Connection)
        CurrentStatusReadCommand.Parameters.AddWithValue("@Report_ID", Report_ID)
        Dim CurrentStatus_ID As Integer = Nothing
        Dim CurrentStatus_Name As String = String.Empty
        Dim CurrentStatus_SortOrder As Integer = Nothing
        Try
            Connection.Open()
            Dim CurrentStatusReader As SqlDataReader = CurrentStatusReadCommand.ExecuteReader()
            While CurrentStatusReader.Read()
                CurrentStatus_ID = CurrentStatusReader.GetInt32(0)
                CurrentStatus_Name = CurrentStatusReader.GetString(1)
                CurrentStatus_SortOrder = CurrentStatusReader.GetInt32(2)
            End While
        Catch ex As Exception
            CurrentStatus_ID = Nothing
            CurrentStatus_Name = String.Empty
            CurrentStatus_SortOrder = Nothing
        Finally
            Connection.Close()
        End Try
        'Store the current status and all statuses the current user can update the current ICSR or report to in datatable
        Dim UpdateToStatusesReadCommandString As String = String.Empty
        If statusestable = tables.ICSRStatuses Then
            UpdateToStatusesReadCommandString = "SELECT DISTINCT CanUpdateICSRStatus.CanUpdateToICSRStatus_ID AS CanUpdateToStatus_ID, ICSRStatuses.Name AS CanUpdateToStatus_Name, ICSRStatuses.SortOrder AS CanUpdateToStatus_SortOrder FROM ICSRStatuses INNER JOIN CanUpdateICSRStatus ON ICSRStatuses.ID = CanUpdateICSRStatus.CanUpdateToICSRStatus_ID INNER JOIN RoleAllocations ON CanUpdateICSRStatus.Role_ID = RoleAllocations.Role_ID INNER JOIN ICSRs ON RoleAllocations.Company_ID = ICSRs.Company_ID INNER JOIN Companies ON ICSRs.Company_ID = Companies.ID WHERE ICSRStatuses.Active = @Active AND Companies.Active = @Active AND CanUpdateICSRStatus.CanUpdateFromICSRStatus_ID = @CurrentStatus_ID AND RoleAllocations.User_ID = @CurrentUser_ID AND RoleAllocations.Company_ID = @CurrentICSR_CompanyID AND ICSRs.ID = @CurrentICSR_ID"
        ElseIf statusestable = tables.ReportStatuses Then
            UpdateToStatusesReadCommandString = "SELECT DISTINCT CanUpdateReportStatus.CanUpdateToReportStatus_ID AS CanUpdateToStatus_ID, ReportStatuses.Name AS CanUpdateToStatus_Name, ReportStatuses.SortOrder AS CanUpdateToStatus_SortOrder FROM ReportStatuses INNER JOIN CanUpdateReportStatus ON ReportStatuses.ID = CanUpdateReportStatus.CanUpdateToReportStatus_ID INNER JOIN RoleAllocations ON CanUpdateReportStatus.Role_ID = RoleAllocations.Role_ID INNER JOIN ICSRs ON RoleAllocations.Company_ID = ICSRs.Company_ID INNER JOIN Companies ON ICSRs.Company_ID = Companies.ID WHERE ReportStatuses.Active = @Active AND Companies.Active = @Active AND CanUpdateReportStatus.CanUpdateFromReportStatus_ID = @CurrentStatus_ID AND RoleAllocations.User_ID = @CurrentUser_ID AND RoleAllocations.Company_ID = @CurrentICSR_CompanyID AND ICSRs.ID = @CurrentICSR_ID"
        Else
            UpdateToStatusesReadCommandString = String.Empty
        End If
        Dim UpdateToStatusesReadCommand As New SqlCommand(UpdateToStatusesReadCommandString, Connection)
        UpdateToStatusesReadCommand.Parameters.AddWithValue("@Active", 1)
        UpdateToStatusesReadCommand.Parameters.AddWithValue("@CurrentStatus_ID", CurrentStatus_ID)
        UpdateToStatusesReadCommand.Parameters.AddWithValue("@CurrentUser_ID", LoggedIn_User_ID)
        UpdateToStatusesReadCommand.Parameters.AddWithValue("@CurrentICSR_CompanyID", CurrentICSRCompany_ID)
        UpdateToStatusesReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
        Dim UpdateToStatusesDatatable As New DataTable()
        UpdateToStatusesDatatable.Columns.AddRange(New DataColumn(2) {
                                                                     New DataColumn("ID", Type.GetType("System.Int32")),
                                                                     New DataColumn("Name", Type.GetType("System.String")),
                                                                     New DataColumn("SortOrder", Type.GetType("System.Int32"))
                                                                     })
        UpdateToStatusesDatatable.Rows.Add(CurrentStatus_ID, CurrentStatus_Name, CurrentStatus_SortOrder) 'Add row with current Status so that users can specify that the status should remain unchanged
        Dim UpdateToStatus_ID As Integer = Nothing
        Dim UpdateToStatus_Name As String = String.Empty
        Dim UpdateToStatus_SortOrder As Integer = Nothing
        Try
            Connection.Open()
            Dim UpdateToStatusesReader As SqlDataReader = UpdateToStatusesReadCommand.ExecuteReader()
            While UpdateToStatusesReader.Read()
                UpdateToStatus_ID = UpdateToStatusesReader.GetInt32(0)
                UpdateToStatus_Name = UpdateToStatusesReader.GetString(1)
                UpdateToStatus_SortOrder = UpdateToStatusesReader.GetInt32(2)
                UpdateToStatusesDatatable.Rows.Add(UpdateToStatus_ID, UpdateToStatus_Name, UpdateToStatus_SortOrder)
            End While
        Catch ex As Exception
            UpdateToStatus_ID = Nothing
            UpdateToStatus_Name = String.Empty
            UpdateToStatus_SortOrder = Nothing
        Finally
            Connection.Close()
        End Try
        UpdateToStatusesDatatable.DefaultView.Sort = "SortOrder"
        Return UpdateToStatusesDatatable
    End Function

    Public Shared Function CreateAssigneesDropDownListDataTable(CurrentICSR_ID As Integer) As DataTable
        Dim AssigneesDropDownListReadCommand As New SqlCommand("SELECT DISTINCT Users.ID AS ID, Name, SortOrder FROM Users INNER JOIN RoleAllocations ON Users.ID = RoleAllocations.User_ID INNER JOIN ICSRs ON RoleAllocations.Company_ID = ICSRs.Company_ID WHERE ICSRs.ID = @CurrentICSR_ID AND Users.Active = @Active", Connection)
        AssigneesDropDownListReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
        AssigneesDropDownListReadCommand.Parameters.AddWithValue("@Active", 1)
        Dim AssigneesDropDownList_DataTable As New DataTable()
        AssigneesDropDownList_DataTable.Columns.AddRange(New DataColumn(2) {
                                                                     New DataColumn("ID", Type.GetType("System.Int32")),
                                                                     New DataColumn("Name", Type.GetType("System.String")),
                                                                     New DataColumn("SortOrder", Type.GetType("System.Int32"))
                                                                     })
        AssigneesDropDownList_DataTable.Rows.Add(0, "Select", 0) 'Add row 'Select' so that users can specify that there should be no assignee
        Dim ID As Integer = Nothing
        Dim Name As String = String.Empty
        Dim SortOrder As Integer = Nothing
        Try
            Connection.Open()
            Dim AssigneesDropDownListReader As SqlDataReader = AssigneesDropDownListReadCommand.ExecuteReader()
            While AssigneesDropDownListReader.Read()
                ID = AssigneesDropDownListReader.GetInt32(0)
                Name = AssigneesDropDownListReader.GetString(1)
                SortOrder = AssigneesDropDownListReader.GetInt32(2)
                AssigneesDropDownList_DataTable.Rows.Add(ID, Name, SortOrder)
            End While
            Connection.Close()
        Catch ex As Exception
            ID = Nothing
            Name = String.Empty
            SortOrder = Nothing
        Finally
            Connection.Close()
        End Try
        AssigneesDropDownList_DataTable.DefaultView.Sort = "SortOrder"
        Return AssigneesDropDownList_DataTable
    End Function

    Public Shared Function CreateAccessibleCompaniesDropDownListDatatable(table As tables) As DataTable
        Dim tableName As String = [Enum].GetName(GetType(tables), table)
        Dim DropDownListReadCommandString As String = String.Empty
        DropDownListReadCommandString = "SELECT DISTINCT " & tableName & ".ID, Name, SortOrder FROM " & tableName & " INNER JOIN RoleAllocations ON " & tableName & ".ID = RoleAllocations.Company_ID WHERE RoleAllocations.User_ID = @LoggedIn_User_ID AND " & tableName & ".Active = @Active"
        Dim DropDownListReadCommand As New SqlCommand(DropDownListReadCommandString, Connection)
        DropDownListReadCommand.Parameters.AddWithValue("@LoggedIn_User_ID", LoggedIn_User_ID)
        DropDownListReadCommand.Parameters.AddWithValue("@Active", 1)
        Dim DropDownListDataTable As New DataTable()
        DropDownListDataTable.Columns.AddRange(New DataColumn(2) {
                                                         New DataColumn("ID", Type.GetType("System.Int32")),
                                                         New DataColumn("Name", Type.GetType("System.String")),
                                                         New DataColumn("SortOrder", Type.GetType("System.Int32"))
                                                         })
        DropDownListDataTable.Rows.Add(0, "Select", 0)
        Dim ID As Integer = Nothing
        Dim Name As String = String.Empty
        Dim SortOrder As Integer = Nothing
        Try
            Connection.Open()
            Dim DropDownListReader As SqlDataReader = DropDownListReadCommand.ExecuteReader()
            While DropDownListReader.Read()
                ID = DropDownListReader.GetInt32(0)
                Name = DropDownListReader.GetString(1)
                SortOrder = DropDownListReader.GetInt32(2)
                DropDownListDataTable.Rows.Add(ID, Name, SortOrder)
            End While
        Catch ex As Exception
            DropDownListDataTable.Rows.Add(-1, DatabaseConnectionErrorString, 0)
        Finally
            Connection.Close()
        End Try
        DropDownListDataTable.DefaultView.Sort = "SortOrder"
        Return DropDownListDataTable
    End Function

    Public Shared Function CreateDropDownListDatatable(table As tables) As DataTable
        Dim tableName As String = [Enum].GetName(GetType(tables), table)
        Dim DropDownListReadCommandString As String = String.Empty
        DropDownListReadCommandString = "SELECT DISTINCT ID, Name, SortOrder FROM " & tableName & " WHERE Active = @Active"
        Dim DropDownListReadCommand As New SqlCommand(DropDownListReadCommandString, Connection)
        DropDownListReadCommand.Parameters.AddWithValue("@LoggedIn_User_ID", LoggedIn_User_ID)
        DropDownListReadCommand.Parameters.AddWithValue("@Active", 1)
        Dim DropDownListDataTable As New DataTable()
        DropDownListDataTable.Columns.AddRange(New DataColumn(2) {
                                                         New DataColumn("ID", Type.GetType("System.Int32")),
                                                         New DataColumn("Name", Type.GetType("System.String")),
                                                         New DataColumn("SortOrder", Type.GetType("System.Int32"))
                                                         })
        DropDownListDataTable.Rows.Add(0, "Select", 0)
        Dim ID As Integer = Nothing
        Dim Name As String = String.Empty
        Dim SortOrder As Integer = Nothing
        Try
            Connection.Open()
            Dim DropDownListReader As SqlDataReader = DropDownListReadCommand.ExecuteReader()
            While DropDownListReader.Read()
                ID = DropDownListReader.GetInt32(0)
                Name = DropDownListReader.GetString(1)
                SortOrder = DropDownListReader.GetInt32(2)
                DropDownListDataTable.Rows.Add(ID, Name, SortOrder)
            End While
        Catch ex As Exception
            DropDownListDataTable.Rows.Add(-1, DatabaseConnectionErrorString, 0)
        Finally
            Connection.Close()
        End Try
        DropDownListDataTable.DefaultView.Sort = "SortOrder"
        Return DropDownListDataTable
    End Function

    Public Shared Function CreateAEsOfCurrentICSRDropDownListDatatable(CurrentICSR_ID As Integer) As DataTable
        Dim DropDownListReadCommand As New SqlCommand("SELECT CASE WHEN AEs.ID IS NULL THEN 0 ELSE AEs.ID END AS ID, CASE WHEN MedDRATerm IS NULL THEN '' ELSE MedDRATerm END AS Name, CASE WHEN AEs.ID IS NULL THEN 0 ELSE AEs.ID END AS SortOrder FROM AEs INNER JOIN ICSRs On ICSRs.ID = AEs.ICSR_ID WHERE ICSRs.ID = @CurrentICSR_ID", Connection)
        DropDownListReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
        Dim DropDownListDataTable As New DataTable()
        DropDownListDataTable.Columns.AddRange(New DataColumn(2) {
                                                         New DataColumn("ID", Type.GetType("System.Int32")),
                                                         New DataColumn("Name", Type.GetType("System.String")),
                                                         New DataColumn("SortOrder", Type.GetType("System.Int32"))
                                                         })
        DropDownListDataTable.Rows.Add(0, "Select", 0)
        Dim ID As Integer = Nothing
        Dim Name As String = String.Empty
        Dim SortOrder As Integer = Nothing
        Try
            Connection.Open()
            Dim DropDownListReader As SqlDataReader = DropDownListReadCommand.ExecuteReader()
            While DropDownListReader.Read()
                ID = DropDownListReader.GetInt32(0)
                Name = DropDownListReader.GetString(1) & " (Event ID " & DropDownListReader.GetInt32(0) & ")"
                SortOrder = DropDownListReader.GetInt32(2)
                DropDownListDataTable.Rows.Add(ID, Name, SortOrder)
            End While
        Catch ex As Exception
            DropDownListDataTable.Rows.Add(-1, DatabaseConnectionErrorString, 0)
        Finally
            Connection.Close()
        End Try
        DropDownListDataTable.DefaultView.Sort = "SortOrder"
        Return DropDownListDataTable
    End Function

    Public Shared Function RelationDependency(CurrentICSRMedication_ID As Integer) As Boolean
        Dim DependencyFound As Boolean = False
        Dim RelationDependencyReadCommand As New SqlCommand("SELECT ID FROM Relations WHERE MedicationPerICSR_ID = @CurrentICSRMedication_ID", Connection)
        RelationDependencyReadCommand.Parameters.AddWithValue("@CurrentICSRMedication_ID", CurrentICSRMedication_ID)
        Try
            Connection.Open()
            Dim RelationDependencyReader As SqlDataReader = RelationDependencyReadCommand.ExecuteReader()
            While RelationDependencyReader.Read()
                If RelationDependencyReader.GetInt32(0) <> Nothing Then
                    DependencyFound = True
                End If
            End While
        Catch ex As Exception
            DependencyFound = True
        Finally
            Connection.Close()
        End Try
        Return DependencyFound
    End Function

    Public Shared Function CreateSuspectedDrugsOfCurrentICSRDropDownListDatatable(CurrentICSR_ID As Integer) As DataTable
        Dim DropDownListReadCommand As New SqlCommand("SELECT DISTINCT CASE WHEN MedicationsPerICSR.ID IS NULL THEN 0 ELSE MedicationsPerICSR.ID END AS ID, CASE WHEN Medications.Name IS NULL THEN '' ELSE Medications.Name END AS Name, CASE WHEN MedicationsPerICSR.ID IS NULL THEN 0 ELSE MedicationsPerICSR.ID END AS SortOrder FROM MedicationsPerICSR LEFT JOIN Medications ON Medications.ID = MedicationsPerICSR.Medication_ID WHERE MedicationsPerICSR.ICSR_ID = @CurrentICSR_ID AND MedicationsPerICSR.MedicationPerICSRRole_ID = @MedicationPerICSRRole_SuspectedDrug", Connection)
        DropDownListReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
        DropDownListReadCommand.Parameters.AddWithValue("@MedicationPerICSRRole_SuspectedDrug", 1)
        Dim DropDownListDataTable As New DataTable()
        DropDownListDataTable.Columns.AddRange(New DataColumn(2) {
                                                         New DataColumn("ID", Type.GetType("System.Int32")),
                                                         New DataColumn("Name", Type.GetType("System.String")),
                                                         New DataColumn("SortOrder", Type.GetType("System.Int32"))
                                                         })
        DropDownListDataTable.Rows.Add(0, "Select", 0)
        Dim ID As Integer = Nothing
        Dim Name As String = String.Empty
        Dim SortOrder As Integer = Nothing
        Try
            Connection.Open()
            Dim DropDownListReader As SqlDataReader = DropDownListReadCommand.ExecuteReader()
            While DropDownListReader.Read()
                ID = DropDownListReader.GetInt32(0)
                Name = DropDownListReader.GetString(1) & " (ICSR Medication ID " & DropDownListReader.GetInt32(0) & ")"
                SortOrder = DropDownListReader.GetInt32(2)
                DropDownListDataTable.Rows.Add(ID, Name, SortOrder)
            End While
        Catch ex As Exception
            DropDownListDataTable.Rows.Add(-1, DatabaseConnectionErrorString, 0)
        Finally
            Connection.Close()
        End Try
        DropDownListDataTable.DefaultView.Sort = "SortOrder"
        Return DropDownListDataTable
    End Function

    Public Shared Function TryCType(value As Object, InputType As InputTypes) As Object
        Dim Result As Object
        If InputType = InputTypes.Number Then

            Try
                Result = CType(value, Integer)
                Return Result
            Catch ex As Exception
                Result = Nothing
                Return Result
            End Try
        End If
        If InputType = InputTypes.Selector Then
            Try
                Result = CType(value, Integer)
                Return Result
            Catch ex As Exception
                Result = Nothing
                Return Result
            End Try
        End If
        If InputType = InputTypes.Date Then
            Result = DateOrDateMinValue(value)
            Return Result
        End If
        If InputType = InputTypes.Text Then
            Try
                Result = CType(value, String)
                Return Result
            Catch ex As Exception
                Result = String.Empty
                Return Result
            End Try
        End If
        If InputType = InputTypes.Email Then
            Try
                Result = CType(value, String)
                Return Result
            Catch ex As Exception
                Result = String.Empty
                Return Result
            End Try
        End If
        If InputType = InputTypes.Phone Then
            Try
                Result = CType(value, String)
                Return Result
            Catch ex As Exception
                Result = String.Empty
                Return Result
            End Try
        End If
        Return value
    End Function

    'Public Shared Function TryInteger(value As Object) As Integer
    '    Dim ReturnInteger As Integer = Nothing
    '    Try
    '        ReturnInteger = CType(value, Integer)
    '    Catch ex As Exception
    '        ReturnInteger = Nothing
    '    End Try
    '    Return ReturnInteger
    'End Function

    'Public Shared Function TryString(value As Object) As String
    '    Dim ReturnString As String = String.Empty
    '    Try
    '        ReturnString = value
    '    Catch ex As Exception
    '        ReturnString = String.Empty
    '    End Try
    '    Return ReturnString
    'End Function

    'Public Shared Function TryDate(value As Object) As DateTime
    '    Dim ReturnString As DateTime = Date.MinValue
    '    Try
    '        ReturnString = CType(value, String)
    '    Catch ex As Exception
    '        ReturnString = Date.MinValue
    '    End Try
    '    Return ReturnString
    'End Function

    Public Shared Sub TextBoxReadOnly(TextBox As TextBox)
        TextBox.ReadOnly = True
        TextBox.ToolTip = String.Empty
        TextBox.CssClass = "form-control"
    End Sub

    Public Shared Sub DropDownListDisabled(DropDownList As DropDownList)
        DropDownList.Enabled = False
        DropDownList.ToolTip = String.Empty
        DropDownList.CssClass = "form-control"
    End Sub

    Public Shared Sub TextBoxReadWrite(TextBox As TextBox)
        TextBox.ReadOnly = False
        TextBox.ToolTip = "Please enter text"
        TextBox.CssClass = "form-control"
    End Sub

    Public Shared Sub DropDownListEnabled(DropDownList As DropDownList)
        DropDownList.Enabled = True
        DropDownList.ToolTip = SelectorInputToolTip
        DropDownList.CssClass = "form-control"
    End Sub

    Public Shared Function DiscrepancyCheck(AtEditPageLoadValue As Object, AtSaveButtonClickValue As Object, FieldName As String) As String
        Dim DiscrepancyString As String = String.Empty
        If AtEditPageLoadValue <> AtSaveButtonClickValue Then
            DiscrepancyString = "<li>" & FieldName & " was changed</li>"
        End If
        Return DiscrepancyString
    End Function

    Public Shared Function SqlDateDisplay(Value As DateTime) As String
        Dim Result As String = DateOrDateMinValue(Value).ToString("dd-MMM-yyyy")
        If Result = Date.MinValue.ToString("dd-MMM-yyyy") Then
            Result = String.Empty
        End If
        Return Result
    End Function

    Public Shared Function SqlIntDisplay(Value As Integer) As String
        Dim Result As String = String.Empty
        If Value <> 0 Then
            Result = Value.ToString
        Else
            Result = String.Empty
        End If
        Return Result
    End Function

    Public Shared Function ParentID(ParentTable As tables, ChildTable As tables, ForeignKey As fields, Child_ID As Integer) As Integer
        Dim Result As Integer = Nothing
        Dim ParentTableName As String = [Enum].GetName(GetType(tables), ParentTable)
        Dim ChildTableName As String = [Enum].GetName(GetType(tables), ChildTable)
        Dim ForeignKeyName As String = [Enum].GetName(GetType(fields), ForeignKey)
        Dim ParentIDReadCommandString As String = "SELECT " & ParentTableName & ".ID From " & ParentTableName & " INNER JOIN " & ChildTableName & " ON " & ParentTableName & ".ID = " & ChildTableName & "." & ForeignKeyName & " WHERE " & ChildTableName & ".ID = @Child_ID"
        Dim ParentIDReadCommand As New SqlCommand(ParentIDReadCommandString, Connection)
        ParentIDReadCommand.Parameters.AddWithValue("@Child_ID", Child_ID)
        Try
            Connection.Open()
            Dim ParentIDReader As SqlDataReader = ParentIDReadCommand.ExecuteReader()
            While ParentIDReader.Read()
                Result = ParentIDReader.GetInt32(0)
            End While
        Catch ex As Exception
            Result = Nothing
        Finally
            Connection.Close()
        End Try
        Return Result
    End Function

    Public Shared Function ICSRIDFOfRelation(Relation_ID As String) As Integer
        Dim Result As Integer = Nothing
        Dim ReadCommand As New SqlCommand("SELECT ICSRs.ID From ICSRs INNER JOIN AEs ON AEs.ICSR_ID = ICSRs.ID INNER JOIN Relations ON Relations.AE_ID = AEs.ID WHERE Relations.ID = @Relation_ID", Connection)
        ReadCommand.Parameters.AddWithValue("@Relation_ID", Relation_ID)
        Try
            Connection.Open()
            Dim Reader As SqlDataReader = ReadCommand.ExecuteReader()
            While Reader.Read()
                Result = Reader.GetInt32(0)
            End While
        Catch ex As Exception
            Result = Nothing
        Finally
            Connection.Close()
        End Try
        Return Result
    End Function

    Public Shared Function GetCompanyID(Current_ID As Integer, Group As tables) As Integer
        Dim Result As Integer = Nothing
        Dim GroupName As String = [Enum].GetName(GetType(tables), Group)
        Dim ReadCommandString As String = "SELECT " & GroupName & ".Company_ID FROM " & GroupName & " WHERE ID = @Current_ID"
        Dim ReadCommand As New SqlCommand(ReadCommandString, Connection)
        ReadCommand.Parameters.AddWithValue("@Current_ID", Current_ID)
        Try
            Connection.Open()
            Dim Reader As SqlDataReader = ReadCommand.ExecuteReader()
            While Reader.Read()
                Result = Reader.GetInt32(0)
            End While
        Catch ex As Exception
            Result = Nothing
        Finally
            Connection.Close()
        End Try
        Return Result
    End Function

    Public Shared Function Validation(Input As String, Type As InputTypes) As Boolean
        Dim ValidationSuccess As Boolean = False
        If Type = InputTypes.Number Then
            Dim ConversionResult As Integer = Nothing
            Try
                ConversionResult = CType(Input.Trim, Integer)
                ValidationSuccess = True
            Catch ex As Exception
                ValidationSuccess = False
            End Try
        ElseIf Type = InputTypes.Date Then
            Dim ConversionResult As DateTime = Date.MinValue
            If DateTime.TryParse(Input.Trim, ConversionResult) = True Then
                ValidationSuccess = True
            Else
                ValidationSuccess = False
            End If
        ElseIf Type = InputTypes.Selector Then
            If CInt(Input) <> 0 Then
                ValidationSuccess = True
            Else
                ValidationSuccess = False
            End If
        End If
        Return ValidationSuccess
    End Function

    Public Shared Function NoValidator(Source As Object) As Boolean
        Dim Result As Boolean = True
        Return Result
    End Function

    Public Shared Function TextValidator(Source As TextBox) As Boolean
        Dim Result As Boolean = False
        If Source.Text.Trim <> String.Empty Then
            Result = True
        End If
        Return Result
    End Function

    Public Shared Function DateOrEmptyValidator(Source As TextBox) As Boolean
        Dim Result As Boolean = False
        If Source.Text <> String.Empty Then
            If Validation(Source.Text, InputTypes.Date) = True Then 'If a valid date was entered
                Result = True
            Else 'If no valid date was entered
                Result = False
            End If
        Else 'If no date was entered
            Result = True
        End If
        Return Result
    End Function

    Public Shared Function DateValidator(Source As TextBox) As Boolean
        Dim Result As Boolean = False
        If Validation(Source.Text, InputTypes.Date) = True Then 'If a valid date was entered
            Result = True
        Else 'If no valid date was entered
            Result = False
        End If
        Return Result
    End Function

    Public Shared Function PhoneValidator(Source As TextBox) As Boolean
        Dim Result As Boolean = False
        If Source.Text = String.Empty Then
            Result = True
        ElseIf Regex.IsMatch(Source.Text, "^[0-9-+]+$") Then
            Result = True
        Else
            Result = False
        End If
        Return Result
    End Function

    Public Shared Function EmailValidator(Source As TextBox) As Boolean
        Dim Result As Boolean = False
        If Source.Text = String.Empty Then
            Result = True
        ElseIf Source.Text Like "*[@]*?.[a-z]*" Then
            Result = True
        Else
            Result = False
        End If
        Return Result
    End Function

    Public Shared Function FileAttachValidator(Source As FileUpload) As Boolean
        Dim Result As Boolean = False
        If Source.HasFile = True Then
            Result = True
        Else
            Result = False
        End If
        Return Result
    End Function

    Public Shared Function PasswordValidator(Source As TextBox) As Boolean
        Dim Result As Boolean = False
        If Source.Text.Trim <> String.Empty Then
            Result = True
        Else
            Result = False
        End If
        Return Result
    End Function

    Public Shared Function SelectionValidator(Source As DropDownList) As Boolean
        Dim Result As Boolean = False
        If Source.SelectedValue <> 0 Then
            Result = True
        Else
            Result = False
        End If
        Return Result
    End Function

    Public Shared Function IntegerValidator(Source As TextBox) As Boolean
        Dim Result As Boolean = False
        If Validation(Source.Text.Trim, InputTypes.Number) = True Then
            Result = True
        End If
        Return Result
    End Function

    Public Shared Function IntegerOrEmptyValidator(Source As TextBox) As Boolean
        Dim Result As Boolean = False
        If Validation(Source.Text.Trim, InputTypes.Number) = True Then
            Result = True
        ElseIf Source.Text.Trim = String.Empty Then
            Result = True
        End If
        Return Result
    End Function

End Class
