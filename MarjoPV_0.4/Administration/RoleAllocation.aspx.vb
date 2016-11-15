Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables

Partial Class Administration_RoleAllocation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "Edit Role Allocation"
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Delete_Button, ConfirmDeletion_Button, Cancel_Button, Nothing)
                    DropDownListEnabled(Name_Dropdownlist)
                    DropDownListEnabled(Company_DropDownList)
                    DropDownListEnabled(Role_DropDownList)
                    Name_Dropdownlist.DataSource = CreateDropDownListDatatable(tables.Users)
                    Name_Dropdownlist.DataValueField = "ID"
                    Name_Dropdownlist.DataTextField = "Name"
                    Name_Dropdownlist.DataBind()
                    Company_DropDownList.DataSource = CreateDropDownListDatatable(tables.Companies)
                    Company_DropDownList.DataValueField = "ID"
                        Company_DropDownList.DataTextField = "Name"
                    Company_DropDownList.DataBind()
                    Role_DropDownList.DataSource = CreateDropDownListDatatable(tables.Roles)
                    Role_DropDownList.DataValueField = "ID"
                    Role_DropDownList.DataTextField = "Name"
                    Role_DropDownList.DataBind()
                    Dim RoleAllocationReadCommand As New SqlCommand("SELECT User_ID, Company_ID, Role_ID FROM RoleAllocations WHERE ID = @CurrentRole_ID", Connection)
                    RoleAllocationReadCommand.Parameters.AddWithValue("@CurrentRole_ID", Request.QueryString("ID"))
                    Try
                        Connection.Open()
                        Dim RoleaAllocationReader As SqlDataReader = RoleAllocationReadCommand.ExecuteReader()
                        While RoleaAllocationReader.Read()
                            Name_Dropdownlist.SelectedValue = RoleaAllocationReader.GetInt32(0)
                            Company_DropDownList.SelectedValue = RoleaAllocationReader.GetInt32(1)
                            Role_DropDownList.SelectedValue = RoleaAllocationReader.GetInt32(2)
                        End While
                    Catch ex As Exception
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                        Exit Sub
                    Finally
                        Connection.Close()
                    End Try
                Else
                    Title_Label.Text = Lockout_Text
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/RoleAllocation.aspx?ID=" & Request.QueryString("ID"))
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        Title_Label.Text = "Edit Role Allocation"
        AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Delete_Button, ConfirmDeletion_Button, Cancel_Button, Nothing)
        DropDownListDisabled(Name_Dropdownlist)
        DropDownListDisabled(Company_DropDownList)
        DropDownListDisabled(Role_DropDownList)
        Dim UpdateCommand As New SqlCommand("UPDATE RoleAllocations SET Company_ID = @Company_ID, User_ID = @User_ID, Role_ID = @Role_ID WHERE ID = @ID", Connection)
        UpdateCommand.Parameters.AddWithValue("@Company_ID", Company_DropDownList.SelectedValue)
        UpdateCommand.Parameters.AddWithValue("@User_ID", Name_Dropdownlist.SelectedValue)
        UpdateCommand.Parameters.AddWithValue("@Role_ID", Role_DropDownList.SelectedValue)
        UpdateCommand.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
        Try
            Connection.Open()
            UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
    End Sub

    Protected Sub Delete_Button_Click(sender As Object, e As EventArgs) Handles Delete_Button.Click
        Title_Label.Text = "Permanently Delete Role Allocation"
        AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Delete_Button, ConfirmDeletion_Button, Cancel_Button, Nothing)
    End Sub

    Protected Sub ConfirmDeletion_Button_Click(sender As Object, e As EventArgs) Handles ConfirmDeletion_Button.Click
        AtConfirmDeletionButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Delete_Button, ConfirmDeletion_Button, Cancel_Button, Nothing)
        Name_Row.Visible = False
        Company_Row.Visible = False
        Role_Row.Visible = False
        Dim DeleteCommand As New SqlCommand("DELETE FROM RoleAllocations WHERE ID = @ID", Connection)
        DeleteCommand.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
        Try
            Connection.Open()
            DeleteCommand.ExecuteNonQuery()
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
    End Sub
End Class
