Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables

Partial Class Administration_CreateRole
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "Create New Role"
                    SaveNewRole_Button.Visible = True
                    Cancel_Button.Visible = True
                    Status_Label.Visible = True
                    Status_Label.CssClass = "form-control alert-warning"
                    Status_Label.Text = "Changes not saved"
                    Name_Row.Visible = True
                    Name_Textbox.ReadOnly = False
                    Name_Textbox.ToolTip = "Please enter a role name"
                    Name_Textbox.CssClass = "form-control"
                    Description_Row.Visible = True
                    Description_Textbox.ReadOnly = False
                    Description_Textbox.ToolTip = "Please enter a role description (not mandatory)"
                    Description_Textbox.CssClass = "form-control"
                    CanViewICSRs_Row.Visible = True
                    CanViewICSRs_DropDownList.Enabled = True
                    CanViewICSRs_DropDownList.CssClass = "form-control"
                    CanCreateICSRs_Row.Visible = True
                    CanCreateICSRs_Dropdownlist.Enabled = True
                    CanCreateICSRs_Dropdownlist.CssClass = "form-control"
                    CanEditICSRs_Row.Visible = True
                    CanEditICSRs_DropDownList.Enabled = True
                    CanEditICSRs_DropDownList.CssClass = "form-control"
                    CanEditICSRAssignee_Row.Visible = True
                    CanEditICSRAssignee_DropDownList.Enabled = True
                    CanEditICSRAssignee_DropDownList.CssClass = "form-control"
                    CanEditICSRSeriousness_Row.Visible = True
                    CanEditICSRSeriousness_DropDownList.Enabled = True
                    CanEditICSRSeriousness_DropDownList.CssClass = "form-control"
                    CanEditICSRCompanyComment_Row.Visible = True
                    CanEditICSRCompanyComment_DropDownList.Enabled = True
                    CanEditICSRCompanyComment_DropDownList.CssClass = "form-control"
                    CanEditReportComplexity_Row.Visible = True
                    CanEditReportComplexity_DropDownList.Enabled = True
                    CanEditReportComplexity_DropDownList.CssClass = "form-control"
                    CanEditReportType_Row.Visible = True
                    CanEditReportType_DropDownList.Enabled = True
                    CanEditReportType_DropDownList.CssClass = "form-control"
                    CanEditReportStatus_Row.Visible = True
                    CanEditReportStatus_DropDownList.Enabled = True
                    CanEditReportStatus_DropDownList.CssClass = "form-control"
                    CanEditReportDue_Row.Visible = True
                    CanEditReportDue_DropDownList.Enabled = True
                    CanEditReportDue_DropDownList.CssClass = "form-control"
                    CanEditReportExpeditedReportingRequired_Row.Visible = True
                    CanEditReportExpeditedReportingRequired_DropDownList.Enabled = True
                    CanEditReportExpeditedReportingRequired_DropDownList.CssClass = "form-control"
                    CanEditReportExpeditedReportingDone_Row.Visible = True
                    CanEditReportExpeditedReportingDone_DropDownList.Enabled = True
                    CanEditReportExpeditedReportingDone_DropDownList.CssClass = "form-control"
                    CanEditReportExpeditedReportingDate_Row.Visible = True
                    CanEditReportExpeditedReportingDate_DropDownList.Enabled = True
                    CanEditReportExpeditedReportingDate_DropDownList.CssClass = "form-control"
                    CanEditRelations_Row.Visible = True
                    CanEditRelations_DropDownList.Enabled = True
                    CanEditRelations_DropDownList.CssClass = "form-control"
                Else
                    Title_Label.Text = Lockout_Text
                    SaveNewRole_Button.Visible = False
                    Cancel_Button.Visible = False
                    Status_Label.Visible = False
                    Name_Row.Visible = False
                    Description_Row.Visible = False
                    CanViewICSRs_Row.Visible = False
                    CanCreateICSRs_Row.Visible = False
                    CanEditICSRs_Row.Visible = False
                    CanEditICSRAssignee_Row.Visible = False
                    CanEditICSRSeriousness_Row.Visible = False
                    CanEditICSRCompanyComment_Row.Visible = False
                    CanEditReportComplexity_Row.Visible = False
                    CanEditReportType_Row.Visible = False
                    CanEditReportStatus_Row.Visible = False
                    CanEditReportDue_Row.Visible = False
                    CanEditReportExpeditedReportingRequired_Row.Visible = False
                    CanEditReportExpeditedReportingDone_Row.Visible = False
                    CanEditReportExpeditedReportingDate_Row.Visible = False
                    CanEditRelations_Row.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/CreateRole.aspx")
            End If
        End If
    End Sub

    Protected Sub Name_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim Command As New SqlCommand("Select [Name] from [Roles]", Connection)
        Dim Reader As SqlDataReader
        Connection.Open()
        Reader = Command.ExecuteReader()
        Dim Db_Name As String = String.Empty
        While Reader.Read()
            Db_Name = Reader.GetString(0)
            If Db_Name = Name_Textbox.Text Then
                Name_Textbox.CssClass = "form-control alert-danger"
                Name_Textbox.ToolTip = "The role name you have entered Is already taken. Please choose a different user name"
                args.IsValid = False
                Connection.Close()
                Exit Sub
            End If
        End While
        Connection.Close()
        'Check if username was entered as valid eMail address and fail validation if not
        If Name_Textbox.Text = String.Empty Then
            Name_Textbox.CssClass = "form-control alert-danger"
            Name_Textbox.ToolTip = "Please make sure you are entering a valid role name"
            args.IsValid = False
        Else
            Name_Textbox.CssClass = "form-control alert-success"
            Name_Textbox.ToolTip = String.Empty
            args.IsValid = True
        End If
    End Sub

    Protected Sub Description_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if role name was entered and fail validation if not
        If Description_Textbox.Text = String.Empty Then
            Description_Textbox.CssClass = "form-control alert-success"
            Description_Textbox.ToolTip = ""
            args.IsValid = True
        Else
            Name_Textbox.CssClass = "form-control alert-success"
            Name_Textbox.ToolTip = ""
            args.IsValid = True
        End If
    End Sub


    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub SaveNewRole_Button_Click(sender As Object, e As EventArgs) Handles SaveNewRole_Button.Click
        Title_Label.Text = "Create New Role"
        SaveNewRole_Button.Visible = False
        Cancel_Button.Visible = False
        Status_Label.Visible = True
        Status_Label.CssClass = "form-control alert-success"
        Status_Label.Text = "Changes saved"
        Name_Row.Visible = True
        Name_Textbox.ReadOnly = True
        Name_Textbox.ToolTip = String.Empty
        Name_Textbox.CssClass = "form-control alert-success"
        Description_Row.Visible = True
        Description_Textbox.ReadOnly = True
        Description_Textbox.ToolTip = String.Empty
        Description_Textbox.CssClass = "form-control alert-success"
        CanViewICSRs_DropDownList.Enabled = False
        CanViewICSRs_DropDownList.CssClass = "form-control alert-success"
        CanCreateICSRs_Dropdownlist.Enabled = False
        CanCreateICSRs_Dropdownlist.CssClass = "form-control alert-success"
        CanEditICSRs_DropDownList.Enabled = False
        CanEditICSRs_DropDownList.CssClass = "form-control alert-success"
        CanEditICSRAssignee_DropDownList.Enabled = False
        CanEditICSRAssignee_DropDownList.CssClass = "form-control alert-success"
        CanEditICSRSeriousness_DropDownList.Enabled = False
        CanEditICSRSeriousness_DropDownList.CssClass = "form-control alert-success"
        CanEditICSRCompanyComment_DropDownList.Enabled = False
        CanEditICSRCompanyComment_DropDownList.CssClass = "form-control alert-success"
        CanEditReportComplexity_DropDownList.Enabled = False
        CanEditReportComplexity_DropDownList.CssClass = "form-control alert-success"
        CanEditReportType_DropDownList.Enabled = False
        CanEditReportType_DropDownList.CssClass = "form-control alert-success"
        CanEditReportStatus_DropDownList.Enabled = False
        CanEditReportStatus_DropDownList.CssClass = "form-control alert-success"
        CanEditReportDue_DropDownList.Enabled = False
        CanEditReportDue_DropDownList.CssClass = "form-control alert-success"
        CanEditReportExpeditedReportingRequired_DropDownList.Enabled = False
        CanEditReportExpeditedReportingRequired_DropDownList.CssClass = "form-control alert-success"
        CanEditReportExpeditedReportingDone_DropDownList.Enabled = False
        CanEditReportExpeditedReportingDone_DropDownList.CssClass = "form-control alert-success"
        CanEditReportExpeditedReportingDate_DropDownList.Enabled = False
        CanEditReportExpeditedReportingDate_DropDownList.CssClass = "form-control alert-success"
        CanEditRelations_DropDownList.Enabled = False
        CanEditRelations_DropDownList.CssClass = "form-control alert-success"
        If Page.IsValid = True Then
            Dim Command As New SqlCommand("INSERT INTO [Roles]([Name], [Description], [CanViewICSRs], [CanCreateICSRs], [CanEditICSRs], [CanEditICSRAssignee], [CanEditICSRSeriousness], [CanEditICSRCompanyComment],[CanEditReportComplexity], [CanEditReportType], [CanEditReportStatus], [CanEditReportDue], [CanEditReportExpeditedReportingRequired], [CanEditReportExpeditedReportingDone], [CanEditReportExpeditedReportingDate], [CanEditRelations]) VALUES(@Name, @Description, @CanViewICSRs, @CanCreateICSRs, @CanEditICSRs, @CanEditICSRAssignee, @CanEditICSRSeriousness, @CanEditICSRCompanyComment,@CanEditReportComplexity, @CanEditReportType, @CanEditReportStatus, @CanEditReportDue, @CanEditReportExpeditedReportingRequired, @CanEditReportExpeditedReportingDone, @CanEditReportExpeditedReportingDate, @CanEditRelations)", Connection)
            Command.Parameters.Add("@Name", SqlDbType.NVarChar, 200)
            Command.Parameters("@Name").Value = Name_Textbox.Text
            Command.Parameters.Add("@Description", SqlDbType.NVarChar, 200)
            Command.Parameters("@Description").Value = Description_Textbox.Text
            Command.Parameters.Add("@CanViewICSRs", SqlDbType.Bit)
            Command.Parameters("@CanViewICSRs").Value = CanViewICSRs_Dropdownlist.SelectedValue
            Command.Parameters.Add("@CanCreateICSRs", SqlDbType.Bit)
            Command.Parameters("@CanCreateICSRs").Value = CanCreateICSRs_Dropdownlist.SelectedValue
            Command.Parameters.Add("@CanEditICSRs", SqlDbType.Bit)
            Command.Parameters("@CanEditICSRs").Value = CanEditICSRs_DropDownList.SelectedValue
            Command.Parameters.Add("@CanEditICSRAssignee", SqlDbType.Bit)
            Command.Parameters("@CanEditICSRAssignee").Value = CanEditICSRAssignee_DropDownList.SelectedValue
            Command.Parameters.Add("@CanEditICSRSeriousness", SqlDbType.Bit)
            Command.Parameters("@CanEditICSRSeriousness").Value = CanEditICSRSeriousness_DropDownList.SelectedValue
            Command.Parameters.Add("@CanEditICSRCompanyComment", SqlDbType.Bit)
            Command.Parameters("@CanEditICSRCompanyComment").Value = CanEditICSRCompanyComment_DropDownList.SelectedValue
            Command.Parameters.Add("@CanEditReportComplexity", SqlDbType.Bit)
            Command.Parameters("@CanEditReportComplexity").Value = CanEditReportComplexity_DropDownList.SelectedValue
            Command.Parameters.Add("@CanEditReportType", SqlDbType.Bit)
            Command.Parameters("@CanEditReportType").Value = CanEditReportType_DropDownList.SelectedValue
            Command.Parameters.Add("@CanEditReportStatus", SqlDbType.Bit)
            Command.Parameters("@CanEditReportStatus").Value = CanEditReportStatus_DropDownList.SelectedValue
            Command.Parameters.Add("@CanEditReportDue", SqlDbType.Bit)
            Command.Parameters("@CanEditReportDue").Value = CanEditReportDue_DropDownList.SelectedValue
            Command.Parameters.Add("@CanEditReportExpeditedReportingRequired", SqlDbType.Bit)
            Command.Parameters("@CanEditReportExpeditedReportingRequired").Value = CanEditReportExpeditedReportingRequired_DropDownList.SelectedValue
            Command.Parameters.Add("@CanEditReportExpeditedReportingDone", SqlDbType.Bit)
            Command.Parameters("@CanEditReportExpeditedReportingDone").Value = CanEditReportExpeditedReportingDone_DropDownList.SelectedValue
            Command.Parameters.Add("@CanEditReportExpeditedReportingDate", SqlDbType.Bit)
            Command.Parameters("@CanEditReportExpeditedReportingDate").Value = CanEditReportExpeditedReportingDate_DropDownList.SelectedValue
            Command.Parameters.Add("@CanEditRelations", SqlDbType.Bit)
            Command.Parameters("@CanEditRelations").Value = CanEditRelations_DropDownList.SelectedValue
            Connection.Open()
            Command.ExecuteNonQuery()
            Connection.Close()
        End If
    End Sub
End Class
