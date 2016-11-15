Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables

Partial Class Administration_CreateRoleAllocation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "Create New Role Allocation"
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
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
                Else
                    Title_Label.Text = Lockout_Text
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/CreateRoleAllocation.aspx")
            End If
        End If
    End Sub

    Protected Sub Name_Dropdownlist_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if UserName was selected and fail validation if not
        If Name_Dropdownlist.SelectedValue = 0 Then
            Name_Dropdownlist.CssClass = CssClassFailure
            Name_Dropdownlist.ToolTip = SelectUserValidationFailToolTip
            args.IsValid = False
        Else
            Name_Dropdownlist.CssClass = CssClassSuccess
            Name_Dropdownlist.ToolTip = String.Empty
            args.IsValid = True
        End If
    End Sub

    Protected Sub Company_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if Company was selected and fail validation if not
        If Company_DropDownList.SelectedValue = 0 Then
            Company_DropDownList.CssClass = CssClassFailure
            Company_DropDownList.ToolTip = SelectCompanyValidationFailToolTip
            args.IsValid = False
        Else
            Company_DropDownList.CssClass = CssClassSuccess
            Company_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        End If
    End Sub

    Protected Sub Role_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if Role was selected and fail validation if not
        If Role_DropDownList.SelectedValue = 0 Then
            Role_DropDownList.CssClass = CssClassFailure
            Role_DropDownList.ToolTip = SelectRoleValidationFailToolTip
            args.IsValid = False
        Else
            Role_DropDownList.CssClass = CssClassSuccess
            Role_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        End If
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
            DropDownListDisabled(Name_Dropdownlist)
            DropDownListDisabled(Company_DropDownList)
            DropDownListDisabled(Role_DropDownList)
            Name_Row.Visible = True
            Name_Dropdownlist.Enabled = False
            Name_Dropdownlist.ToolTip = String.Empty
            Name_Dropdownlist.CssClass = "form-control alert-success"
            Company_Row.Visible = True
            Company_DropDownList.Enabled = False
            Company_DropDownList.ToolTip = String.Empty
            Company_DropDownList.CssClass = "form-control alert-success"
            Role_Row.Visible = True
            Role_DropDownList.Enabled = False
            Role_DropDownList.ToolTip = String.Empty
            Role_DropDownList.CssClass = "form-control alert-success"
            Dim Command As New SqlCommand("INSERT INTO RoleAllocations(Company_ID, User_ID, Role_ID) VALUES(@Company_ID, @User_ID, @Role_ID)", Connection)
            Command.Parameters.Add("@Company_ID", SqlDbType.Int, 32)
            Command.Parameters("@Company_ID").Value = Company_DropDownList.SelectedValue
            Command.Parameters.Add("@User_ID", SqlDbType.Int, 32)
            Command.Parameters("@User_ID").Value = Name_Dropdownlist.SelectedValue
            Command.Parameters.Add("@Role_ID", SqlDbType.Int, 32)
            Command.Parameters("@Role_ID").Value = Role_DropDownList.SelectedValue
            Connection.Open()
            Command.ExecuteNonQuery()
            Connection.Close()
        End If
    End Sub
End Class
