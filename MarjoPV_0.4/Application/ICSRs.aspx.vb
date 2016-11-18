Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_ICSRs
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                Title_Label.Text = "ICSRs"
                'Display CreateICSR_Button if LoggedIn_User_CanCreateICSRs = True
                If LoggedIn_User_CanCreateICSRs = True Then
                    CreateICSR_Button.Visible = True
                Else
                    CreateICSR_Button.Visible = False
                End If
                'Show and populate ICSR company header fields as per the user's right to view companies
                If LoggedIn_User_CanViewCompanies = True Then
                    ICSRCompany_Label.Visible = True
                    Companies_Filter_DropDownList_ICSRs.Visible = True
                    Companies_Filter_DropDownList_ICSRs.DataSource = CreateAccessibleCompaniesDropDownListDatatable(tables.Companies)
                    Companies_Filter_DropDownList_ICSRs.DataValueField = "ID"
                    Companies_Filter_DropDownList_ICSRs.DataTextField = "Name"
                    Companies_Filter_DropDownList_ICSRs.DataBind()
                    'Note: Company data fields are unhid after they have been populated from the database (see below)
                End If
                'Populate ICSRStatuses_Filter_DropDownList
                ICSRStatuses_Filter_DropDownList.DataSource = CreateDropDownListDatatable(tables.ICSRStatuses)
                ICSRStatuses_Filter_DropDownList.DataValueField = "ID"
                ICSRStatuses_Filter_DropDownList.DataTextField = "Name"
                ICSRStatuses_Filter_DropDownList.DataBind()
                'Populate Assignees_Filter_DropDownList
                Assignees_Filter_DropDownList.DataSource = CreateDropDownListDatatable(tables.Users)
                Assignees_Filter_DropDownList.DataValueField = "ID"
                Assignees_Filter_DropDownList.DataTextField = "Name"
                Assignees_Filter_DropDownList.DataBind()
                'Populate IsSerious_Filter_DropDownList
                IsSerious_Filter_DropDownList.DataSource = CreateDropDownListDatatable(tables.IsSerious)
                IsSerious_Filter_DropDownList.DataValueField = "ID"
                IsSerious_Filter_DropDownList.DataTextField = "Name"
                IsSerious_Filter_DropDownList.DataBind()
                'Populate ICSRs List according to user rights to view companies
                Dim ICSRsListCommand As New SqlCommand("SELECT DISTINCT ICSRs.ID, Companies.Name AS Company_Name, ICSRs.Patient_Initials AS Patient_Initials, ICSRStatuses.Name AS ICSRStatus_Name, Users.Name AS Assignee_Name, IsSerious.Name AS IsSerious_Name FROM ICSRs INNER JOIN Companies ON ICSRs.Company_ID = Companies.ID INNER JOIN ICSRStatuses ON ICSRs.ICSRStatus_ID = ICSRStatuses.ID LEFT JOIN Users ON ICSRs.Assignee_ID = Users.ID LEFT JOIN IsSerious ON ICSRs.IsSerious_ID = IsSerious.ID INNER JOIN RoleAllocations ON Companies.ID = RoleAllocations.Company_ID WHERE Companies.Active = @Active AND RoleAllocations.User_ID = @CurrentUser_ID", Connection)
                ICSRsListCommand.Parameters.AddWithValue("@Active", 1)
                ICSRsListCommand.Parameters.AddWithValue("@CurrentUser_ID", LoggedIn_User_ID)
                Try
                    Connection.Open()
                    ICSRsList_Repeater.DataSource = ICSRsListCommand.ExecuteReader()
                    ICSRsList_Repeater.DataBind()
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
                'Unhide Company Data in ICSRsList_Repeater if LoggedIn_User_CanViewCompanies = True
                If LoggedIn_User_CanViewCompanies = True Then
                    For Each item In ICSRsList_Repeater.Items
                        item.findcontrol("Company").visible = True
                    Next
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/ICSRs.aspx")
            End If
        End If
    End Sub

    Protected Sub Filter_ICSRs_List(sender As Object, e As EventArgs)
        'Store User Input in Variables
        Dim Input_ID As Integer = Nothing
        If ICSR_ID_Filter_Textbox.Text <> String.Empty Then
            Try
                Input_ID = CType(ICSR_ID_Filter_Textbox.Text, Integer)
            Catch ex As Exception
                Input_ID = Nothing
            End Try
        Else
            Input_ID = Nothing
        End If
        Dim Input_Company_ID As Integer = TryCType(Companies_Filter_DropDownList_ICSRs.SelectedValue, InputTypes.Integer)
        Dim Input_PatientInitials As String = TryCType(PatientInitials_Filter_Textbox.Text, InputTypes.String)
        Dim Input_ICSRStatus_ID As Integer = TryCType(ICSRStatuses_Filter_DropDownList.SelectedValue, InputTypes.Integer)
        Dim Input_Assignee_ID As Integer = TryCType(Assignees_Filter_DropDownList.SelectedValue, InputTypes.Integer)
        Dim Input_IsSerious_ID As Integer = TryCType(IsSerious_Filter_DropDownList.SelectedValue, InputTypes.Integer)
        Dim Input_ID_Component As String = String.Empty
        If Input_ID <> Nothing Then
            Input_ID_Component = " AND ICSRs.ID = " & Input_ID
        Else
            Input_ID_Component = " AND 1 = 1"
        End If
        Dim Input_Company_ID_Component As String = String.Empty
        If Input_Company_ID <> Nothing Then
            Input_Company_ID_Component = " AND ICSRs.Company_ID = " & Companies_Filter_DropDownList_ICSRs.SelectedValue
        Else
            Input_Company_ID_Component = " AND 1 = 1"
        End If
        Dim Input_PatientInitials_Component As String = String.Empty
        If Input_PatientInitials <> String.Empty Then
            Input_PatientInitials_Component = " AND ICSRs.Patient_Initials = @PatientInitials"
        Else
            Input_PatientInitials_Component = " AND 1 = 1"
        End If
        Dim Input_ICSRStatus_ID_Component As String = String.Empty
        If Input_ICSRStatus_ID <> Nothing Then
            Input_ICSRStatus_ID_Component = " AND ICSRs.ICSRStatus_ID = " & Input_ICSRStatus_ID
        Else
            Input_ICSRStatus_ID_Component = " AND 1 = 1 "
        End If
        Dim Input_Assignee_ID_Component As String = String.Empty
        If Input_Assignee_ID <> Nothing Then
            Input_Assignee_ID_Component = " AND ICSRs.Assignee_ID = " & Input_Assignee_ID
        Else
            Input_Assignee_ID_Component = " AND 1 = 1"
        End If
        Dim Input_IsSerious_ID_Component As String = String.Empty
        If Input_IsSerious_ID <> Nothing Then
            Input_IsSerious_ID_Component = " AND ICSRs.IsSerious_ID = " & Input_IsSerious_ID
        Else
            Input_IsSerious_ID_Component = " AND 1 = 1"
        End If
        Dim SqlCommandString As String = "SELECT DISTINCT ICSRs.ID, Companies.Name AS Company_Name, ICSRs.Patient_Initials AS Patient_Initials, ICSRStatuses.Name AS ICSRStatus_Name, Users.Name AS Assignee_Name, IsSerious.Name As IsSerious_Name FROM ICSRs INNER JOIN Companies ON ICSRs.Company_ID = Companies.ID INNER JOIN ICSRStatuses ON ICSRs.ICSRStatus_ID = ICSRStatuses.ID LEFT JOIN Users ON ICSRs.Assignee_ID = Users.ID LEFT JOIN IsSerious ON ICSRs.IsSerious_ID = IsSerious.ID INNER JOIN RoleAllocations ON Companies.ID = RoleAllocations.Company_ID WHERE Companies.Active = @Active AND RoleAllocations.User_ID = @LoggedIn_User_ID" & Input_ID_Component & Input_Company_ID_Component & Input_ICSRStatus_ID_Component & Input_Assignee_ID_Component & Input_IsSerious_ID_Component & Input_PatientInitials_Component
        Dim ICSRsListCommand As New SqlCommand(SqlCommandString, Connection)
        ICSRsListCommand.Parameters.AddWithValue("@Active", 1)
        ICSRsListCommand.Parameters.AddWithValue("@LoggedIn_User_ID", LoggedIn_User_ID)
        If Input_PatientInitials <> String.Empty Then
            ICSRsListCommand.Parameters.AddWithValue("@PatientInitials", Input_PatientInitials)
        End If
        Try
            Connection.Open()
            ICSRsList_Repeater.DataSource = ICSRsListCommand.ExecuteReader()
            ICSRsList_Repeater.DataBind()
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
        'Unhide Company Data in ICSRsList_Repeater if LoggedIn_User_CanViewCompanies = True
        If LoggedIn_User_CanViewCompanies = True Then
            For Each item In ICSRsList_Repeater.Items
                item.findcontrol("Company").visible = True
            Next
        End If
    End Sub

    Protected Sub CreateICSR_Button_Click(sender As Object, e As EventArgs) Handles CreateICSR_Button.Click
        Response.Redirect("~/Application/CreateICSR.aspx")
    End Sub

End Class
