Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_Medications
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                Title_Label.Text = "Medications"
                'Display CreateMedixation_Button if LoggedIn_User can create Medications
                If CanEdit(tables.Medications, Nothing, tables.Medications, fields.Create) = True Then
                    CreateMedication_Button.Visible = True
                Else
                    CreateMedication_Button.Visible = False
                End If
                'Show and populate ICSR company header fields as per the user's right to view companies
                If LoggedIn_User_CanViewCompanies = True Then
                    MedicationCompany_Label.Visible = True
                    Companies_Filter_DropDownList_Medications.Visible = True
                    Companies_Filter_DropDownList_Medications.DataSource = CreateDropDownListDatatable(tables.Companies)
                    Companies_Filter_DropDownList_Medications.DataValueField = "ID"
                    Companies_Filter_DropDownList_Medications.DataTextField = "Name"
                    Companies_Filter_DropDownList_Medications.DataBind()
                    'Note: Company data fields are unhid after they have been populated from the database (see below)
                End If
                'Populate Medication_Types_Filter_DropDownList
                Medication_Types_Filter_DropDownList.DataSource = CreateDropDownListDatatable(tables.MedicationTypes)
                Medication_Types_Filter_DropDownList.DataValueField = "ID"
                Medication_Types_Filter_DropDownList.DataTextField = "Name"
                Medication_Types_Filter_DropDownList.DataBind()
                'Populate AdministrationRoute_Filter_DropDownList
                AdministrationRoute_Filter_DropDownList.DataSource = CreateDropDownListDatatable(tables.AdministrationRoutes)
                AdministrationRoute_Filter_DropDownList.DataValueField = "ID"
                AdministrationRoute_Filter_DropDownList.DataTextField = "Name"
                AdministrationRoute_Filter_DropDownList.DataBind()
                'Populate Medications List according to user rights to view companies
                Dim MedicationsListCommand As New SqlCommand("SELECT DISTINCT Medications.ID, Companies.Name AS Company_Name, Medications.Name AS Name, MedicationTypes.Name AS MedicationType_Name, AdministrationRoutes.Name AS AdministrationRoute_Name FROM Medications INNER JOIN Companies ON Medications.Company_ID = Companies.ID INNER JOIN RoleAllocations ON Companies.ID = RoleAllocations.Company_ID LEFT JOIN MedicationTypes ON Medications.MedicationType_ID = MedicationTypes.ID LEFT JOIN AdministrationRoutes ON Medications.AdministrationRoute_ID = AdministrationRoutes.ID WHERE Companies.Active = @Active AND RoleAllocations.User_ID = @CurrentUser_ID", Connection)
                MedicationsListCommand.Parameters.AddWithValue("@Active", 1)
                MedicationsListCommand.Parameters.AddWithValue("@CurrentUser_ID", LoggedIn_User_ID)
                Try
                    Connection.Open()
                    MedicationsList_Repeater.DataSource = MedicationsListCommand.ExecuteReader()
                    MedicationsList_Repeater.DataBind()
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
                'Unhide Company Data in ICSRsList_Repeater if LoggedIn_User_CanViewCompanies = True
                If LoggedIn_User_CanViewCompanies = True Then
                    For Each item In MedicationsList_Repeater.Items
                        item.findcontrol("Company_Hyperlink").visible = True
                    Next
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/Medications.aspx")
            End If
        End If
    End Sub

    Protected Sub Filter_Medications_List(sender As Object, e As EventArgs)
        'Store User Input in Variables
        Dim Input_ID As Integer = Nothing
        If Medication_ID_Filter_TextBox.Text <> String.Empty Then
            Try
                Input_ID = CType(Medication_ID_Filter_TextBox.Text, Integer)
            Catch ex As Exception
                Input_ID = Nothing
            End Try
        Else
            Input_ID = Nothing
        End If
        Dim Input_Company_ID As Integer = TryCType(Companies_Filter_DropDownList_Medications.SelectedValue, InputTypes.Number)
        Dim Input_Name As String = TryCType(Name_Filter_TextBox.Text, InputTypes.Text)
        Dim Input_MedicationType_ID As Integer = TryCType(Medication_Types_Filter_DropDownList.SelectedValue, InputTypes.Number)
        Dim Input_AdministrationRoute_ID As Integer = TryCType(AdministrationRoute_Filter_DropDownList.SelectedValue, InputTypes.Number)
        Dim Input_ID_Component As String = String.Empty
        If Input_ID <> Nothing Then
            Input_ID_Component = " AND Medications.ID = " & Input_ID
        Else
            Input_ID_Component = " AND 1 = 1"
        End If
        Dim Input_Company_ID_Component As String = String.Empty
        If Input_Company_ID <> Nothing Then
            Input_Company_ID_Component = " AND Medications.Company_ID = " & Input_Company_ID
        Else
            Input_Company_ID_Component = " AND 1 = 1"
        End If
        Dim Input_Name_Component As String = String.Empty
        If Input_Name <> String.Empty Then
            Input_Name_Component = " AND Medications.Name = @Name"
        Else
            Input_Name_Component = " AND 1 = 1"
        End If
        Dim Input_MedicationType_ID_Component As String = String.Empty
        If Input_MedicationType_ID <> Nothing Then
            Input_MedicationType_ID_Component = " AND Medications.MedicationType_ID = " & Input_MedicationType_ID
        Else
            Input_MedicationType_ID_Component = " AND 1 = 1 "
        End If
        Dim Input_AdministrationRoute_Component As String = String.Empty
        If Input_AdministrationRoute_ID <> Nothing Then
            Input_AdministrationRoute_Component = " AND Medications.AdministrationRoute_ID = " & Input_AdministrationRoute_ID
        Else
            Input_AdministrationRoute_Component = " AND 1 = 1 "
        End If
        Dim SqlCommandString As String = "SELECT DISTINCT Medications.ID, Companies.Name AS Company_Name, Medications.Name AS Name, MedicationTypes.Name AS MedicationType_Name, AdministrationRoutes.Name AS AdministrationRoute_Name FROM Medications INNER JOIN Companies ON Medications.Company_ID = Companies.ID INNER JOIN RoleAllocations ON Companies.ID = RoleAllocations.Company_ID LEFT JOIN MedicationTypes ON Medications.MedicationType_ID = MedicationTypes.ID LEFT JOIN AdministrationRoutes ON Medications.AdministrationRoute_ID = AdministrationRoutes.ID WHERE Companies.Active = @Active AND RoleAllocations.User_ID = @CurrentUser_ID" & Input_ID_Component & Input_Company_ID_Component & Input_Name_Component & Input_MedicationType_ID_Component & Input_AdministrationRoute_Component
        Dim MedicationsListCommand As New SqlCommand(SqlCommandString, Connection)
        MedicationsListCommand.Parameters.AddWithValue("@Active", 1)
        MedicationsListCommand.Parameters.AddWithValue("@CurrentUser_ID", LoggedIn_User_ID)
        If Input_Name <> String.Empty Then
            MedicationsListCommand.Parameters.AddWithValue("@Name", Input_Name)
        End If
        Try
            Connection.Open()
            MedicationsList_Repeater.DataSource = MedicationsListCommand.ExecuteReader()
            MedicationsList_Repeater.DataBind()
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
        'Unhide Company Data in MedicationsList_Repeater if LoggedIn_User_CanViewCompanies = True
        If LoggedIn_User_CanViewCompanies = True Then
            For Each item In MedicationsList_Repeater.Items
                item.findcontrol("Company_Hyperlink").visible = True
            Next
        End If
    End Sub

    Protected Sub CreateMedication_Button_Click(sender As Object, e As EventArgs) Handles CreateMedication_Button.Click
        Response.Redirect("~/Application/CreateMedication.aspx")
    End Sub

End Class
