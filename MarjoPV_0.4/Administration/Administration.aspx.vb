Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables


Partial Class Admininstration_Administration
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Dim UserListCommand As New SqlCommand("SELECT ID, Username, Password, Name, Phone, Active, CanViewCompanies, Admin, LastLogin FROM Users", Connection)
                    Connection.Open()
                    UserList_Repeater.DataSource = UserListCommand.ExecuteReader()
                    UserList_Repeater.DataBind()
                    Connection.Close()
                    Dim CompaniesCommand As New SqlCommand("SELECT ID, Name, Street, PostalCode, City, Country, SortOrder, Active FROM Companies ORDER BY SortOrder", Connection)
                    Connection.Open()
                    CompaniesList_Repeater.DataSource = CompaniesCommand.ExecuteReader()
                    CompaniesList_Repeater.DataBind()
                    Connection.Close()
                    Dim UsersOrganizationsRolesCommand As New SqlCommand("SELECT RoleAllocations.ID AS RoleAllocation_ID, Companies.Name AS Company_Name, Users.Name, Roles.Name AS Role_Name FROM RoleAllocations INNER JOIN Companies ON RoleAllocations.Company_ID = Companies.ID INNER JOIN Users ON RoleAllocations.User_ID = Users.ID INNER JOIN Roles ON RoleAllocations.Role_ID = Roles.ID", Connection)
                    Connection.Open()
                    RoleAllocationsList_Repeater.DataSource = UsersOrganizationsRolesCommand.ExecuteReader()
                    RoleAllocationsList_Repeater.DataBind()
                    Connection.Close()
                    Dim ICSRStatusUpdateRightsCommand As New SqlCommand("SELECT CanUpdateICSRStatus.ID, Roles.Name AS Role_Name, ICSRStatuses_From.Name AS CanUpdateFromICSRStatus_Name, ICSRStatuses_To.Name AS CanUpdateToICSRStatus_Name FROM CanUpdateICSRStatus INNER JOIN Roles On CanUpdateICSRStatus.Role_ID = Roles.ID INNER JOIN ICSRStatuses AS ICSRStatuses_From ON CanUpdateICSRStatus.CanUpdateFromICSRStatus_ID = ICSRStatuses_From.ID INNER JOIN ICSRStatuses AS ICSRStatuses_To ON CanUpdateICSRStatus.CanUpdateToICSRStatus_ID = ICSRStatuses_To.ID ORDER BY Role_Name, ICSRStatuses_From.ID", Connection)
                    Connection.Open()
                    ICSRStatusUpdateRightsList_Repeater.DataSource = ICSRStatusUpdateRightsCommand.ExecuteReader()
                    ICSRStatusUpdateRightsList_Repeater.DataBind()
                    Connection.Close()
                    Dim ReportStatusUpdateRightsCommand As New SqlCommand("SELECT CanUpdateReportStatus.ID, Roles.Name AS Role_Name, ReportStatuses_From.Name AS CanUpdateFromReportStatus_Name, ReportStatuses_To.Name AS CanUpdateToReportStatus_Name FROM CanUpdateReportStatus INNER JOIN Roles On CanUpdateReportStatus.Role_ID = Roles.ID INNER JOIN ReportStatuses AS ReportStatuses_From ON CanUpdateReportStatus.CanUpdateFromReportStatus_ID = ReportStatuses_From.ID INNER JOIN ReportStatuses AS ReportStatuses_To ON CanUpdateReportStatus.CanUpdateToReportStatus_ID = ReportStatuses_To.ID ORDER BY Roles.ID", Connection)
                    Connection.Open()
                    ReportStatusUpdateRightsList_Repeater.DataSource = ReportStatusUpdateRightsCommand.ExecuteReader()
                    ReportStatusUpdateRightsList_Repeater.DataBind()
                    Connection.Close()
                    Dim ICSRStatusesCommand As New SqlCommand("SELECT ID, Name, SortOrder, IsStatusNew, IsStatusClosed, Active FROM ICSRStatuses ORDER BY SortOrder", Connection)
                    Connection.Open()
                    ICSRStatusesList_Repeater.DataSource = ICSRStatusesCommand.ExecuteReader()
                    ICSRStatusesList_Repeater.DataBind()
                    Connection.Close()
                    Dim ReportStatusesCommand As New SqlCommand("SELECT ID, Name, SortOrder, IsStatusNew, IsStatusClosed, Active FROM ReportStatuses ORDER BY SortOrder", Connection)
                    Connection.Open()
                    ReportStatusesList_Repeater.DataSource = ReportStatusesCommand.ExecuteReader()
                    ReportStatusesList_Repeater.DataBind()
                    Connection.Close()
                    Dim ReportComplexitiesCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM ReportComplexities ORDER BY SortOrder", Connection)
                    Connection.Open()
                    ReportComplexitiesList_Repeater.DataSource = ReportComplexitiesCommand.ExecuteReader()
                    ReportComplexitiesList_Repeater.DataBind()
                    Connection.Close()
                    Dim ReportSourcesCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM ReportSources ORDER BY SortOrder", Connection)
                    Connection.Open()
                    ReportSourcesList_Repeater.DataSource = ReportSourcesCommand.ExecuteReader()
                    ReportSourcesList_Repeater.DataBind()
                    Connection.Close()
                    Dim ReportTypesCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM ReportTypes ORDER BY SortOrder", Connection)
                    Connection.Open()
                    ReportTypesList_Repeater.DataSource = ReportTypesCommand.ExecuteReader()
                    ReportTypesList_Repeater.DataBind()
                    Connection.Close()
                    Dim MedicationTypesCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM MedicationTypes ORDER BY SortOrder", Connection)
                    Connection.Open()
                    MedicationTypesList_Repeater.DataSource = MedicationTypesCommand.ExecuteReader()
                    MedicationTypesList_Repeater.DataBind()
                    Connection.Close()
                    Dim UnitsCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM Units ORDER BY SortOrder", Connection)
                    Connection.Open()
                    UnitsList_Repeater.DataSource = UnitsCommand.ExecuteReader()
                    UnitsList_Repeater.DataBind()
                    Connection.Close()
                    Dim AdministrationRoutesCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM AdministrationRoutes ORDER BY SortOrder", Connection)
                    Connection.Open()
                    AdministrationRoutesList_Repeater.DataSource = AdministrationRoutesCommand.ExecuteReader()
                    AdministrationRoutesList_Repeater.DataBind()
                    Connection.Close()
                    Dim DrugActionsCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM DrugActions ORDER BY SortOrder", Connection)
                    Connection.Open()
                    DrugActionsList_Repeater.DataSource = DrugActionsCommand.ExecuteReader()
                    DrugActionsList_Repeater.DataBind()
                    Connection.Close()
                    Dim SeriousnessCriteriaListCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM SeriousnessCriteria ORDER BY SortOrder", Connection)
                    Connection.Open()
                    SeriousnessCriteriaList_Repeater.DataSource = SeriousnessCriteriaListCommand.ExecuteReader()
                    SeriousnessCriteriaList_Repeater.DataBind()
                    Connection.Close()
                    Dim RelatednessCriteriaManufacturerListCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM RelatednessCriteriaManufacturer ORDER BY SortOrder", Connection)
                    Connection.Open()
                    RelatednessCriteriaManufacturerList_Repeater.DataSource = RelatednessCriteriaManufacturerListCommand.ExecuteReader()
                    RelatednessCriteriaManufacturerList_Repeater.DataBind()
                    Connection.Close()
                    Dim RelatednessCriteriaReporterListCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM RelatednessCriteriaReporter ORDER BY SortOrder", Connection)
                    Connection.Open()
                    RelatednessCriteriaReporterList_Repeater.DataSource = RelatednessCriteriaReporterListCommand.ExecuteReader()
                    RelatednessCriteriaReporterList_Repeater.DataBind()
                    Connection.Close()
                    Dim ExpectednessCriteriaListCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM ExpectednessCriteria ORDER BY SortOrder", Connection)
                    Connection.Open()
                    ExpectednessCriteriaList_Repeater.DataSource = ExpectednessCriteriaListCommand.ExecuteReader()
                    ExpectednessCriteriaList_Repeater.DataBind()
                    Connection.Close()
                    Dim OutcomesListCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM Outcomes ORDER BY SortOrder", Connection)
                    Connection.Open()
                    OutcomesList_Repeater.DataSource = OutcomesListCommand.ExecuteReader()
                    OutcomesList_Repeater.DataBind()
                    Connection.Close()
                    Dim DechallengeResultsListCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM DechallengeResults ORDER BY SortOrder", Connection)
                    Connection.Open()
                    DechallengeResultsList_Repeater.DataSource = DechallengeResultsListCommand.ExecuteReader()
                    DechallengeResultsList_Repeater.DataBind()
                    Connection.Close()
                    Dim RechallengeResultsListCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM RechallengeResults ORDER BY SortOrder", Connection)
                    Connection.Open()
                    RechallengeResultsList_Repeater.DataSource = RechallengeResultsListCommand.ExecuteReader()
                    RechallengeResultsList_Repeater.DataBind()
                    Connection.Close()
                    Dim GendersCommand As New SqlCommand("SELECT ID, Name, SortOrder, Active FROM Genders ORDER BY SortOrder", Connection)
                    Connection.Open()
                    GendersList_Repeater.DataSource = GendersCommand.ExecuteReader()
                    GendersList_Repeater.DataBind()
                    Connection.Close()
                Else
                    Title_Label.Text = Lockout_Text
                    Content_Tabs.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/Administration.aspx")
            End If
        End If
    End Sub

    Protected Sub UserList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles UserList_Repeater.ItemCommand
        If e.CommandName = "CreateAccount" Then
            Response.Redirect("~/Administration/CreateAccount.aspx")
        End If
    End Sub

    Protected Sub ComapniesList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles CompaniesList_Repeater.ItemCommand
        If e.CommandName = "CreateCompany" Then
            Response.Redirect("~/Administration/CreateCompany.aspx")
        End If
    End Sub

    Protected Sub RoleAllocationsList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RoleAllocationsList_Repeater.ItemCommand
        If e.CommandName = "CreateRoleAllocation" Then
            Response.Redirect("~/Administration/CreateRoleAllocation.aspx")
        End If
    End Sub

    Protected Sub CreateICSRStatusUpdateRightsList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles ICSRStatusUpdateRightsList_Repeater.ItemCommand
        If e.CommandName = "CreateICSRStatusUpdateRight" Then
            Response.Redirect("~/Administration/CreateICSRStatusUpdateRight.aspx")
        End If
    End Sub

    Protected Sub CreateReportStatusUpdateRightsList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles ReportStatusUpdateRightsList_Repeater.ItemCommand
        If e.CommandName = "CreateReportStatusUpdateRight" Then
            Response.Redirect("~/Administration/CreateReportStatusUpdateRight.aspx")
        End If
    End Sub

    Protected Sub ICSRStatusesList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles ICSRStatusesList_Repeater.ItemCommand
        If e.CommandName = "CreateICSRStatus" Then
            Response.Redirect("~/Administration/CreateICSRStatus.aspx")
        End If
    End Sub

    Protected Sub ReportStatusesList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles ReportStatusesList_Repeater.ItemCommand
        If e.CommandName = "CreateReportStatus" Then
            Response.Redirect("~/Administration/CreateReportStatus.aspx")
        End If
    End Sub

    Protected Sub ReportComplexitiesList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles ReportComplexitiesList_Repeater.ItemCommand
        If e.CommandName = "CreateReportComplexity" Then
            Response.Redirect("~/Administration/CreateReportComplexity.aspx")
        End If
    End Sub

    Protected Sub ReportSourcesList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles ReportSourcesList_Repeater.ItemCommand
        If e.CommandName = "CreateReportSource" Then
            Response.Redirect("~/Administration/CreateReportSource.aspx")
        End If
    End Sub

    Protected Sub ReportTypesList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles ReportTypesList_Repeater.ItemCommand
        If e.CommandName = "CreateReportType" Then
            Response.Redirect("~/Administration/CreateReportType.aspx")
        End If
    End Sub

    Protected Sub MedicationTypesList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles MedicationTypesList_Repeater.ItemCommand
        If e.CommandName = "CreateMedicationType" Then
            Response.Redirect("~/Administration/CreateMedicationType.aspx")
        End If
    End Sub

    Protected Sub UnitsList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles UnitsList_Repeater.ItemCommand
        If e.CommandName = "CreateUnit" Then
            Response.Redirect("~/Administration/CreateUnit.aspx")
        End If
    End Sub

    Protected Sub AdministrationRoutesList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles AdministrationRoutesList_Repeater.ItemCommand
        If e.CommandName = "CreateAdministrationRoute" Then
            Response.Redirect("~/Administration/CreateAdministrationRoute.aspx")
        End If
    End Sub

    Protected Sub DrugActionsList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles DrugActionsList_Repeater.ItemCommand
        If e.CommandName = "CreateDrugAction" Then
            Response.Redirect("~/Administration/CreateDrugAction.aspx")
        End If
    End Sub

    Protected Sub SeriousnessCriteriaList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles SeriousnessCriteriaList_Repeater.ItemCommand
        If e.CommandName = "CreateSeriousnessCriterion" Then
            Response.Redirect("~/Administration/CreateSeriousnessCriterion.aspx")
        End If
    End Sub

    Protected Sub RelatednessCriteriaManufacturerList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RelatednessCriteriaManufacturerList_Repeater.ItemCommand
        If e.CommandName = "CreateRelatednessCriterionManufacturer" Then
            Response.Redirect("~/Administration/CreateRelatednessCriterionManufacturer.aspx")
        End If
    End Sub

    Protected Sub RelatednessCriteriaReporterList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RelatednessCriteriaReporterList_Repeater.ItemCommand
        If e.CommandName = "CreateRelatednessCriterionReporter" Then
            Response.Redirect("~/Administration/CreateRelatednessCriterionReporter.aspx")
        End If
    End Sub

    Protected Sub ExpectednessCriteriaList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles ExpectednessCriteriaList_Repeater.ItemCommand
        If e.CommandName = "CreateExpectednessCriterion" Then
            Response.Redirect("~/Administration/CreateExpectednessCriterion.aspx")
        End If
    End Sub

    Protected Sub OutcomesList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles OutcomesList_Repeater.ItemCommand
        If e.CommandName = "CreateOutcome" Then
            Response.Redirect("~/Administration/CreateOutcome.aspx")
        End If
    End Sub

    Protected Sub DechallengeResultsList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles DechallengeResultsList_Repeater.ItemCommand
        If e.CommandName = "CreateDechallengeResult" Then
            Response.Redirect("~/Administration/CreateDechallengeResult.aspx")
        End If
    End Sub

    Protected Sub RechallengeResultsList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles RechallengeResultsList_Repeater.ItemCommand
        If e.CommandName = "CreateRechallengeResult" Then
            Response.Redirect("~/Administration/CreateRechallengeResult.aspx")
        End If
    End Sub

    Protected Sub GendersList_Repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles GendersList_Repeater.ItemCommand
        If e.CommandName = "CreateGender" Then
            Response.Redirect("~/Administration/CreateGender.aspx")
        End If
    End Sub

End Class
