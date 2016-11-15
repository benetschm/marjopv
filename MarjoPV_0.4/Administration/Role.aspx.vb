Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables

Partial Class Administration_Role
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    EditRoleDetails_Button.Visible = True
                    DeleteRole_Button.Visible = True
                    SaveRoleUpdates_Button.Visible = False
                    ConfirmRoleDeletion_Button.Visible = False
                    Cancel_Button.Visible = False
                    Status_Label.Visible = True
                    Status_Label.CssClass = "form-control alert-success"
                    Status_Label.Text = "No changes pending"
                    Name_Row.Visible = True
                    Name_Textbox.ReadOnly = True
                    Name_Textbox.ToolTip = String.Empty
                    Name_Textbox.CssClass = "form-control"
                    Description_Row.Visible = True
                    Description_Textbox.ReadOnly = True
                    Description_Textbox.ToolTip = String.Empty
                    Description_Textbox.CssClass = "form-control"
                    CanViewICSRs_Row.Visible = True
                    CanViewICSRs_DropDownList.Enabled = False
                    CanViewICSRs_DropDownList.CssClass = "form-control"
                    CanCreateICSRs_Row.Visible = True
                    CanCreateICSRs_Dropdownlist.Enabled = False
                    CanCreateICSRs_Dropdownlist.CssClass = "form-control"
                    CanEditICSRs_Row.Visible = True
                    CanEditICSRs_DropDownList.Enabled = False
                    CanEditICSRs_DropDownList.CssClass = "form-control"
                    CanEditICSRAssignee_Row.Visible = True
                    CanEditICSRAssignee_DropDownList.Enabled = False
                    CanEditICSRAssignee_DropDownList.CssClass = "form-control"
                    CanEditICSRSeriousness_Row.Visible = True
                    CanEditICSRSeriousness_DropDownList.Enabled = False
                    CanEditICSRSeriousness_DropDownList.CssClass = "form-control"
                    CanEditICSRCompanyComment_Row.Visible = True
                    CanEditICSRCompanyComment_DropDownList.Enabled = False
                    CanEditICSRCompanyComment_DropDownList.CssClass = "form-control"
                    CanEditReportComplexity_Row.Visible = True
                    CanEditReportComplexity_DropDownList.Enabled = False
                    CanEditReportComplexity_DropDownList.CssClass = "form-control"
                    CanEditReportType_Row.Visible = True
                    CanEditReportType_DropDownList.Enabled = False
                    CanEditReportType_DropDownList.CssClass = "form-control"
                    CanEditReportStatus_Row.Visible = True
                    CanEditReportStatus_DropDownList.Enabled = False
                    CanEditReportStatus_DropDownList.CssClass = "form-control"
                    CanEditReportDue_Row.Visible = True
                    CanEditReportDue_DropDownList.Enabled = False
                    CanEditReportDue_DropDownList.CssClass = "form-control"
                    CanEditReportExpeditedReportingRequired_Row.Visible = True
                    CanEditReportExpeditedReportingRequired_DropDownList.Enabled = False
                    CanEditReportExpeditedReportingRequired_DropDownList.CssClass = "form-control"
                    CanEditReportExpeditedReportingDone_Row.Visible = True
                    CanEditReportExpeditedReportingDone_DropDownList.Enabled = False
                    CanEditReportExpeditedReportingDone_DropDownList.CssClass = "form-control"
                    CanEditReportExpeditedReportingDate_Row.Visible = True
                    CanEditReportExpeditedReportingDate_DropDownList.Enabled = False
                    CanEditReportExpeditedReportingDate_DropDownList.CssClass = "form-control"
                    CanEditRelations_Row.Visible = True
                    CanEditRelations_DropDownList.Enabled = False
                    CanEditRelations_DropDownList.CssClass = "form-control"
                    Dim Command As New SqlCommand("SELECT [Name], [Description], [CanViewICSRs], [CanCreateICSRs], [CanEditICSRs], [CanEditICSRAssignee], [CanEditICSRSeriousness], [CanEditICSRCompanyComment],[CanEditReportComplexity], [CanEditReportType], [CanEditReportStatus], [CanEditReportDue], [CanEditReportExpeditedReportingRequired], [CanEditReportExpeditedReportingDone], [CanEditReportExpeditedReportingDate], [CanEditRelations] FROM [Roles] WHERE [ID] = @ID", Connection)
                    Command.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
                    Dim Reader As SqlDataReader
                    Dim Db_Name As String = String.Empty
                    Dim Db_Description As String = String.Empty
                    Dim Db_CanViewICSRs As Boolean = False
                    Dim Db_CanCreateICSRs As Boolean = False
                    Dim Db_CanEditICSRs As Boolean = False
                    Dim Db_CanEditICSRAssignee As Boolean = False
                    Dim Db_CanEditICSRSeriousness As Boolean = False
                    Dim Db_CanEditICSRCompanyComment As Boolean = False
                    Dim Db_CanEditReportComplexity As Boolean = False
                    Dim Db_CanEditReportType As Boolean = False
                    Dim Db_CanEditReportStatus As Boolean = False
                    Dim Db_CanEditReportDue As Boolean = False
                    Dim Db_CanEditReportExpeditedReportingRequired As Boolean = False
                    Dim Db_CanEditReportExpeditedReportingDone As Boolean = False
                    Dim Db_CanEditReportExpeditedReportingDate As Boolean = False
                    Dim Db_CanEditRelations As Boolean = False
                    Connection.Open()
                    Reader = Command.ExecuteReader()
                    Reader.Read()
                    Try
                        Db_Name = Reader.GetString(0)
                    Catch ex As Exception
                        Db_Name = String.Empty
                    End Try
                    Try
                        Db_Description = Reader.GetString(1)
                    Catch ex As Exception
                        Db_Description = String.Empty
                    End Try
                    Try
                        Db_CanViewICSRs = Reader.GetBoolean(2)
                    Catch ex As Exception
                        Db_CanViewICSRs = False
                    End Try
                    Try
                        Db_CanCreateICSRs = Reader.GetBoolean(3)
                    Catch ex As Exception
                        Db_CanCreateICSRs = False
                    End Try
                    Try
                        Db_CanEditICSRs = Reader.GetBoolean(4)
                    Catch ex As Exception
                        Db_CanEditICSRs = False
                    End Try
                    Try
                        Db_CanEditICSRAssignee = Reader.GetBoolean(5)
                    Catch ex As Exception
                        Db_CanEditICSRAssignee = False
                    End Try
                    Try
                        Db_CanEditICSRSeriousness = Reader.GetBoolean(6)
                    Catch ex As Exception
                        Db_CanEditICSRSeriousness = False
                    End Try
                    Try
                        Db_CanEditICSRCompanyComment = Reader.GetBoolean(7)
                    Catch ex As Exception
                        Db_CanEditICSRCompanyComment = False
                    End Try
                    Try
                        Db_CanEditReportComplexity = Reader.GetBoolean(8)
                    Catch ex As Exception
                        Db_CanEditReportComplexity = False
                    End Try
                    Try
                        Db_CanEditReportType = Reader.GetBoolean(9)
                    Catch ex As Exception
                        Db_CanEditReportType = False
                    End Try
                    Try
                        Db_CanEditReportStatus = Reader.GetBoolean(10)
                    Catch ex As Exception
                        Db_CanEditReportStatus = False
                    End Try
                    Try
                        Db_CanEditReportDue = Reader.GetBoolean(11)
                    Catch ex As Exception
                        Db_CanEditReportDue = False
                    End Try
                    Try
                        Db_CanEditReportExpeditedReportingRequired = Reader.GetBoolean(12)
                    Catch ex As Exception
                        Db_CanEditReportExpeditedReportingRequired = False
                    End Try
                    Try
                        Db_CanEditReportExpeditedReportingDone = Reader.GetBoolean(13)
                    Catch ex As Exception
                        Db_CanEditReportExpeditedReportingDone = False
                    End Try
                    Try
                        Db_CanEditReportExpeditedReportingDate = Reader.GetBoolean(14)
                    Catch ex As Exception
                        Db_CanEditReportExpeditedReportingDate = False
                    End Try
                    Try
                        Db_CanEditRelations = Reader.GetBoolean(15)
                    Catch ex As Exception
                        Db_CanEditRelations = False
                    End Try
                    Connection.Close()
                    Name_Textbox.Text = Db_Name
                    Description_Textbox.Text = Db_Description
                    CanViewICSRs_DropDownList.SelectedValue = Db_CanViewICSRs
                    CanCreateICSRs_Dropdownlist.SelectedValue = Db_CanCreateICSRs
                    CanEditICSRs_DropDownList.SelectedValue = Db_CanEditICSRs
                    CanEditICSRAssignee_DropDownList.SelectedValue = Db_CanEditICSRAssignee
                    CanEditICSRSeriousness_DropDownList.SelectedValue = Db_CanEditICSRSeriousness
                    CanEditICSRCompanyComment_DropDownList.SelectedValue = Db_CanEditICSRCompanyComment
                    CanEditReportComplexity_DropDownList.SelectedValue = Db_CanEditReportComplexity
                    CanEditReportType_DropDownList.SelectedValue = Db_CanEditReportType
                    CanEditReportStatus_DropDownList.SelectedValue = Db_CanEditReportStatus
                    CanEditReportDue_DropDownList.SelectedValue = Db_CanEditReportDue
                    CanEditReportExpeditedReportingRequired_DropDownList.SelectedValue = Db_CanEditReportExpeditedReportingRequired
                    CanEditReportExpeditedReportingDone_DropDownList.SelectedValue = Db_CanEditReportExpeditedReportingDone
                    CanEditReportExpeditedReportingDate_DropDownList.SelectedValue = Db_CanEditReportExpeditedReportingDate
                    CanEditRelations_DropDownList.SelectedValue = Db_CanEditRelations
                Else
                    Title_Label.Text = Lockout_Text
                    EditRoleDetails_Button.Visible = False
                    DeleteRole_Button.Visible = False
                    SaveRoleUpdates_Button.Visible = False
                    ConfirmRoleDeletion_Button.Visible = False
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
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/Role.aspx?ID=" & Request.QueryString("ID"))
            End If
        End If
    End Sub

    Protected Sub EditRoleDetails_Button_Click(sender As Object, e As EventArgs) Handles EditRoleDetails_Button.Click
        EditRoleDetails_Button.Visible = False
        DeleteRole_Button.Visible = False
        SaveRoleUpdates_Button.Visible = True
        ConfirmRoleDeletion_Button.Visible = False
        Cancel_Button.Visible = True
        Status_Label.Visible = True
        Status_Label.CssClass = "form-control alert-warning"
        Status_Label.Text = "Changes not saved"
        Name_Row.Visible = True
        Name_Textbox.ReadOnly = False
        Name_Textbox.ToolTip = "Please enter a valid role name"
        Name_Textbox.CssClass = "form-control"
        Description_Row.Visible = True
        Description_Textbox.ReadOnly = False
        Description_Textbox.ToolTip = "Please enter a role description (not mandatory)"
        Description_Textbox.CssClass = "form-control"
        CanViewICSRs_Row.Visible = True
        CanViewICSRs_DropDownList.Enabled = True
        CanViewICSRs_DropDownList.ToolTip = "Please choose 'True' or 'False'"
        CanViewICSRs_DropDownList.CssClass = "form-control"
        CanCreateICSRs_Row.Visible = True
        CanCreateICSRs_Dropdownlist.Enabled = True
        CanCreateICSRs_Dropdownlist.ToolTip = "Please choose 'True' or 'False'"
        CanCreateICSRs_Dropdownlist.CssClass = "form-control"
        CanEditICSRs_Row.Visible = True
        CanEditICSRs_DropDownList.Enabled = True
        CanEditICSRs_DropDownList.ToolTip = "Please choose 'True' or 'False'"
        CanEditICSRs_DropDownList.CssClass = "form-control"
        CanEditICSRAssignee_Row.Visible = True
        CanEditICSRAssignee_DropDownList.Enabled = True
        CanEditICSRAssignee_DropDownList.ToolTip = "Please choose 'True' or 'False'"
        CanEditICSRAssignee_DropDownList.CssClass = "form-control"
        CanEditICSRSeriousness_Row.Visible = True
        CanEditICSRSeriousness_DropDownList.Enabled = True
        CanEditICSRSeriousness_DropDownList.ToolTip = "Please choose 'True' or 'False'"
        CanEditICSRSeriousness_DropDownList.CssClass = "form-control"
        CanEditICSRCompanyComment_Row.Visible = True
        CanEditICSRCompanyComment_DropDownList.Enabled = True
        CanEditICSRCompanyComment_DropDownList.ToolTip = "Please choose 'True' or 'False'"
        CanEditICSRCompanyComment_DropDownList.CssClass = "form-control"
        CanEditReportComplexity_Row.Visible = True
        CanEditReportComplexity_DropDownList.Enabled = True
        CanEditReportComplexity_DropDownList.CssClass = "form-control"
        CanEditReportComplexity_DropDownList.ToolTip = "Please choose 'True' or 'False'"
        CanEditReportType_Row.Visible = True
        CanEditReportType_DropDownList.Enabled = True
        CanEditReportType_DropDownList.ToolTip = "Please choose 'True' or 'False'"
        CanEditReportType_DropDownList.CssClass = "form-control"
        CanEditReportStatus_Row.Visible = True
        CanEditReportStatus_DropDownList.Enabled = True
        CanEditReportStatus_DropDownList.ToolTip = "Please choose 'True' or 'False'"
        CanEditReportStatus_DropDownList.CssClass = "form-control"
        CanEditReportDue_Row.Visible = True
        CanEditReportDue_DropDownList.Enabled = True
        CanEditReportDue_DropDownList.ToolTip = "Please choose 'True' or 'False'"
        CanEditReportDue_DropDownList.CssClass = "form-control"
        CanEditReportExpeditedReportingRequired_Row.Visible = True
        CanEditReportExpeditedReportingRequired_DropDownList.Enabled = True
        CanEditReportExpeditedReportingRequired_DropDownList.ToolTip = "Please choose 'True' or 'False'"
        CanEditReportExpeditedReportingRequired_DropDownList.CssClass = "form-control"
        CanEditReportExpeditedReportingDone_Row.Visible = True
        CanEditReportExpeditedReportingDone_DropDownList.Enabled = True
        CanEditReportExpeditedReportingDone_DropDownList.ToolTip = "Please choose 'True' or 'False'"
        CanEditReportExpeditedReportingDone_DropDownList.CssClass = "form-control"
        CanEditReportExpeditedReportingDate_Row.Visible = True
        CanEditReportExpeditedReportingDate_DropDownList.Enabled = True
        CanEditReportExpeditedReportingDate_DropDownList.ToolTip = "Please choose 'True' or 'False'"
        CanEditReportExpeditedReportingDate_DropDownList.CssClass = "form-control"
        CanEditRelations_Row.Visible = True
        CanEditRelations_DropDownList.Enabled = True
        CanEditRelations_DropDownList.CssClass = "form-control"
        CanEditRelations_DropDownList.ToolTip = "Please choose 'True' or 'False'"
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        EditRoleDetails_Button.Visible = True
        DeleteRole_Button.Visible = True
        SaveRoleUpdates_Button.Visible = False
        ConfirmRoleDeletion_Button.Visible = False
        Cancel_Button.Visible = False
        Status_Label.Visible = True
        Status_Label.CssClass = "form-control alert-success"
        Status_Label.Text = "No changes pending"
        Name_Row.Visible = True
        Name_Textbox.ReadOnly = True
        Name_Textbox.ToolTip = String.Empty
        Name_Textbox.CssClass = "form-control"
        Description_Row.Visible = True
        Description_Textbox.ReadOnly = True
        Description_Textbox.ToolTip = String.Empty
        Description_Textbox.CssClass = "form-control"
        CanViewICSRs_Row.Visible = True
        CanViewICSRs_DropDownList.Enabled = False
        CanViewICSRs_DropDownList.CssClass = "form-control"
        CanCreateICSRs_Row.Visible = True
        CanCreateICSRs_Dropdownlist.Enabled = False
        CanCreateICSRs_Dropdownlist.CssClass = "form-control"
        CanEditICSRs_Row.Visible = True
        CanEditICSRs_DropDownList.Enabled = False
        CanEditICSRs_DropDownList.CssClass = "form-control"
        CanEditICSRAssignee_Row.Visible = True
        CanEditICSRAssignee_DropDownList.Enabled = False
        CanEditICSRAssignee_DropDownList.CssClass = "form-control"
        CanEditICSRSeriousness_Row.Visible = True
        CanEditICSRSeriousness_DropDownList.Enabled = False
        CanEditICSRSeriousness_DropDownList.CssClass = "form-control"
        CanEditICSRCompanyComment_Row.Visible = True
        CanEditICSRCompanyComment_DropDownList.Enabled = False
        CanEditICSRCompanyComment_DropDownList.CssClass = "form-control"
        CanEditReportComplexity_Row.Visible = True
        CanEditReportComplexity_DropDownList.Enabled = False
        CanEditReportComplexity_DropDownList.CssClass = "form-control"
        CanEditReportType_Row.Visible = True
        CanEditReportType_DropDownList.Enabled = False
        CanEditReportType_DropDownList.CssClass = "form-control"
        CanEditReportStatus_Row.Visible = True
        CanEditReportStatus_DropDownList.Enabled = False
        CanEditReportStatus_DropDownList.CssClass = "form-control"
        CanEditReportDue_Row.Visible = True
        CanEditReportDue_DropDownList.Enabled = False
        CanEditReportDue_DropDownList.CssClass = "form-control"
        CanEditReportExpeditedReportingRequired_Row.Visible = True
        CanEditReportExpeditedReportingRequired_DropDownList.Enabled = False
        CanEditReportExpeditedReportingRequired_DropDownList.CssClass = "form-control"
        CanEditReportExpeditedReportingDone_Row.Visible = True
        CanEditReportExpeditedReportingDone_DropDownList.Enabled = False
        CanEditReportExpeditedReportingDone_DropDownList.CssClass = "form-control"
        CanEditReportExpeditedReportingDate_Row.Visible = True
        CanEditReportExpeditedReportingDate_DropDownList.Enabled = False
        CanEditReportExpeditedReportingDate_DropDownList.CssClass = "form-control"
        CanEditRelations_Row.Visible = True
        CanEditRelations_DropDownList.Enabled = False
        CanEditRelations_DropDownList.CssClass = "form-control"
        Dim Command As New SqlCommand("SELECT [Name], [Description], [CanViewICSRs], [CanCreateICSRs], [CanEditICSRs], [CanEditICSRAssignee], [CanEditICSRSeriousness], [CanEditICSRCompanyComment],[CanEditReportComplexity], [CanEditReportType], [CanEditReportStatus], [CanEditReportDue], [CanEditReportExpeditedReportingRequired], [CanEditReportExpeditedReportingDone], [CanEditReportExpeditedReportingDate], [CanEditRelations] FROM [Roles] WHERE [ID] = @ID", Connection)
        Command.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
        Dim Reader As SqlDataReader
        Dim Db_Name As String = String.Empty
        Dim Db_Description As String = String.Empty
        Dim Db_CanViewICSRs As Boolean = False
        Dim Db_CanCreateICSRs As Boolean = False
        Dim Db_CanEditICSRs As Boolean = False
        Dim Db_CanEditICSRAssignee As Boolean = False
        Dim Db_CanEditICSRSeriousness As Boolean = False
        Dim Db_CanEditICSRCompanyComment As Boolean = False
        Dim Db_CanEditReportComplexity As Boolean = False
        Dim Db_CanEditReportType As Boolean = False
        Dim Db_CanEditReportStatus As Boolean = False
        Dim Db_CanEditReportDue As Boolean = False
        Dim Db_CanEditReportExpeditedReportingRequired As Boolean = False
        Dim Db_CanEditReportExpeditedReportingDone As Boolean = False
        Dim Db_CanEditReportExpeditedReportingDate As Boolean = False
        Dim Db_CanEditRelations As Boolean = False
        Connection.Open()
        Reader = Command.ExecuteReader()
        Reader.Read()
        Try
            Db_Name = Reader.GetString(0)
        Catch ex As Exception
            Db_Name = String.Empty
        End Try
        Try
            Db_Description = Reader.GetString(1)
        Catch ex As Exception
            Db_Description = String.Empty
        End Try
        Try
            Db_CanViewICSRs = Reader.GetBoolean(2)
        Catch ex As Exception
            Db_CanViewICSRs = False
        End Try
        Try
            Db_CanCreateICSRs = Reader.GetBoolean(3)
        Catch ex As Exception
            Db_CanCreateICSRs = False
        End Try
        Try
            Db_CanEditICSRs = Reader.GetBoolean(4)
        Catch ex As Exception
            Db_CanEditICSRs = False
        End Try
        Try
            Db_CanEditICSRAssignee = Reader.GetBoolean(5)
        Catch ex As Exception
            Db_CanEditICSRAssignee = False
        End Try
        Try
            Db_CanEditICSRSeriousness = Reader.GetBoolean(6)
        Catch ex As Exception
            Db_CanEditICSRSeriousness = False
        End Try
        Try
            Db_CanEditICSRCompanyComment = Reader.GetBoolean(7)
        Catch ex As Exception
            Db_CanEditICSRCompanyComment = False
        End Try
        Try
            Db_CanEditReportComplexity = Reader.GetBoolean(8)
        Catch ex As Exception
            Db_CanEditReportComplexity = False
        End Try
        Try
            Db_CanEditReportType = Reader.GetBoolean(9)
        Catch ex As Exception
            Db_CanEditReportType = False
        End Try
        Try
            Db_CanEditReportStatus = Reader.GetBoolean(10)
        Catch ex As Exception
            Db_CanEditReportStatus = False
        End Try
        Try
            Db_CanEditReportDue = Reader.GetBoolean(11)
        Catch ex As Exception
            Db_CanEditReportDue = False
        End Try
        Try
            Db_CanEditReportExpeditedReportingRequired = Reader.GetBoolean(12)
        Catch ex As Exception
            Db_CanEditReportExpeditedReportingRequired = False
        End Try
        Try
            Db_CanEditReportExpeditedReportingDone = Reader.GetBoolean(13)
        Catch ex As Exception
            Db_CanEditReportExpeditedReportingDone = False
        End Try
        Try
            Db_CanEditReportExpeditedReportingDate = Reader.GetBoolean(14)
        Catch ex As Exception
            Db_CanEditReportExpeditedReportingDate = False
        End Try
        Try
            Db_CanEditRelations = Reader.GetBoolean(15)
        Catch ex As Exception
            Db_CanEditRelations = False
        End Try
        Connection.Close()
        Name_Textbox.Text = Db_Name
        Description_Textbox.Text = Db_Description
        CanViewICSRs_DropDownList.SelectedValue = Db_CanViewICSRs
        CanCreateICSRs_Dropdownlist.SelectedValue = Db_CanCreateICSRs
        CanEditICSRs_DropDownList.SelectedValue = Db_CanEditICSRs
        CanEditICSRAssignee_DropDownList.SelectedValue = Db_CanEditICSRAssignee
        CanEditICSRSeriousness_DropDownList.SelectedValue = Db_CanEditICSRSeriousness
        CanEditICSRCompanyComment_DropDownList.SelectedValue = Db_CanEditICSRCompanyComment
        CanEditReportComplexity_DropDownList.SelectedValue = Db_CanEditReportComplexity
        CanEditReportType_DropDownList.SelectedValue = Db_CanEditReportType
        CanEditReportStatus_DropDownList.SelectedValue = Db_CanEditReportStatus
        CanEditReportDue_DropDownList.SelectedValue = Db_CanEditReportDue
        CanEditReportExpeditedReportingRequired_DropDownList.SelectedValue = Db_CanEditReportExpeditedReportingRequired
        CanEditReportExpeditedReportingDone_DropDownList.SelectedValue = Db_CanEditReportExpeditedReportingDone
        CanEditReportExpeditedReportingDate_DropDownList.SelectedValue = Db_CanEditReportExpeditedReportingDate
        CanEditRelations_DropDownList.SelectedValue = Db_CanEditRelations
    End Sub

    Protected Sub Name_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim Command As New SqlCommand("Select [Name] from [Roles] WHERE [ID] <> @ID", Connection)
        Command.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
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

    Protected Sub SaveRoleUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveRoleUpdates_Button.Click
        EditRoleDetails_Button.Visible = True
        DeleteRole_Button.Visible = True
        SaveRoleUpdates_Button.Visible = False
        ConfirmRoleDeletion_Button.Visible = False
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
        CanViewICSRs_Row.Visible = True
        CanViewICSRs_DropDownList.Enabled = False
        CanViewICSRs_DropDownList.CssClass = "form-control alert-success"
        CanCreateICSRs_Row.Visible = True
        CanCreateICSRs_Dropdownlist.Enabled = False
        CanCreateICSRs_Dropdownlist.CssClass = "form-control alert-success"
        CanEditICSRs_Row.Visible = True
        CanEditICSRs_DropDownList.Enabled = False
        CanEditICSRs_DropDownList.CssClass = "form-control alert-success"
        CanEditICSRAssignee_Row.Visible = True
        CanEditICSRAssignee_DropDownList.Enabled = False
        CanEditICSRAssignee_DropDownList.CssClass = "form-control alert-success"
        CanEditICSRSeriousness_Row.Visible = True
        CanEditICSRSeriousness_DropDownList.Enabled = False
        CanEditICSRSeriousness_DropDownList.CssClass = "form-control alert-success"
        CanEditICSRCompanyComment_Row.Visible = True
        CanEditICSRCompanyComment_DropDownList.Enabled = False
        CanEditICSRCompanyComment_DropDownList.CssClass = "form-control alert-success"
        CanEditReportComplexity_Row.Visible = True
        CanEditReportComplexity_DropDownList.Enabled = False
        CanEditReportComplexity_DropDownList.CssClass = "form-control alert-success"
        CanEditReportType_Row.Visible = True
        CanEditReportType_DropDownList.Enabled = False
        CanEditReportType_DropDownList.CssClass = "form-control alert-success"
        CanEditReportStatus_Row.Visible = True
        CanEditReportStatus_DropDownList.Enabled = False
        CanEditReportStatus_DropDownList.CssClass = "form-control alert-success"
        CanEditReportDue_Row.Visible = True
        CanEditReportDue_DropDownList.Enabled = False
        CanEditReportDue_DropDownList.CssClass = "form-control alert-success"
        CanEditReportExpeditedReportingRequired_Row.Visible = True
        CanEditReportExpeditedReportingRequired_DropDownList.Enabled = False
        CanEditReportExpeditedReportingRequired_DropDownList.CssClass = "form-control alert-success"
        CanEditReportExpeditedReportingDone_Row.Visible = True
        CanEditReportExpeditedReportingDone_DropDownList.Enabled = False
        CanEditReportExpeditedReportingDone_DropDownList.CssClass = "form-control alert-success"
        CanEditReportExpeditedReportingDate_Row.Visible = True
        CanEditReportExpeditedReportingDate_DropDownList.Enabled = False
        CanEditReportExpeditedReportingDate_DropDownList.CssClass = "form-control alert-success"
        CanEditRelations_Row.Visible = True
        CanEditRelations_DropDownList.Enabled = False
        If Page.IsValid = True Then
            Dim Command As New SqlCommand("UPDATE Roles SET Name = @Name, Description = @Description, CanViewICSRs = @CanViewICSRs, CanCreateICSRs = @CanCreateICSRs, CanEditICSRs = @CanEditICSRs, CanEditICSRAssignee = @CanEditICSRAssignee, CanEditICSRSeriousness = @CanEditICSRSeriousness, CanEditICSRCompanyComment = @CanEditICSRCompanyComment, CanEditReportComplexity = @CanEditReportComplexity, CanEditReportType = @CanEditReportType, CanEditReportStatus = @CanEditReportStatus, CanEditReportDue = @CanEditReportDue, CanEditReportExpeditedReportingRequired = @CanEditReportExpeditedReportingRequired, CanEditReportExpeditedReportingDone = @CanEditReportExpeditedReportingDone, CanEditReportExpeditedReportingDate = @CanEditReportExpeditedReportingDate, CanEditRelations = @CanEditRelations WHERE ID = @ID", Connection)
            Command.Parameters.Add("@Name", SqlDbType.NVarChar, 200)
            Command.Parameters("@Name").Value = Name_Textbox.Text
            Command.Parameters.Add("@Description", SqlDbType.NVarChar, 200)
            Command.Parameters("@Description").Value = Description_Textbox.Text
            Command.Parameters.Add("@CanViewICSRs", SqlDbType.Bit)
            Command.Parameters("@CanViewICSRs").Value = CanViewICSRs_DropDownList.SelectedValue
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
            Command.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
            Connection.Open()
            Command.ExecuteNonQuery()
            Connection.Close()
        End If
    End Sub

    Protected Sub DeleteRole_Button_Click(sender As Object, e As EventArgs) Handles DeleteRole_Button.Click
        'Check if role is assigned to user and abort action if yes
        Dim UserIDAssignedToRole As Integer = Nothing
        Dim UserIDAssignedToRoleReadCommand As New SqlCommand("SELECT User_ID FROM RoleAllocations WHERE Role_ID = @CurrentRole_ID", Connection)
        UserIDAssignedToRoleReadCommand.Parameters.AddWithValue("@CurrentRole_ID", Request.QueryString("ID"))
        Try
            Connection.Open()
            Dim UserIDAssignedToRoleReader As SqlDataReader = UserIDAssignedToRoleReadCommand.ExecuteReader()
            While UserIDAssignedToRoleReader.Read()
                Try
                    UserIDAssignedToRole = UserIDAssignedToRoleReader.GetInt32(0)
                Catch ex As Exception
                    UserIDAssignedToRole = Nothing
                End Try
            End While
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
        If UserIDAssignedToRole = Nothing Then
            EditRoleDetails_Button.Visible = False
            DeleteRole_Button.Visible = False
            SaveRoleUpdates_Button.Visible = False
            ConfirmRoleDeletion_Button.Visible = True
            Cancel_Button.Visible = True
            Status_Label.Visible = True
            Status_Label.CssClass = "form-control alert-danger"
            Status_Label.Text = "Confirming will permanently delete this role"
            Name_Row.Visible = True
            Name_Textbox.ReadOnly = True
            Name_Textbox.ToolTip = String.Empty
            Name_Textbox.CssClass = "form-control"
            Description_Row.Visible = True
            Description_Textbox.ReadOnly = True
            Description_Textbox.ToolTip = String.Empty
            Description_Textbox.CssClass = "form-control"
            CanViewICSRs_Row.Visible = True
            CanViewICSRs_DropDownList.Enabled = False
            CanViewICSRs_DropDownList.CssClass = "form-control"
            CanCreateICSRs_Row.Visible = True
            CanCreateICSRs_Dropdownlist.Enabled = False
            CanCreateICSRs_Dropdownlist.CssClass = "form-control"
            CanEditICSRs_Row.Visible = True
            CanEditICSRs_DropDownList.Enabled = False
            CanEditICSRs_DropDownList.CssClass = "form-control"
            CanEditICSRAssignee_Row.Visible = True
            CanEditICSRAssignee_DropDownList.Enabled = False
            CanEditICSRAssignee_DropDownList.CssClass = "form-control"
            CanEditICSRSeriousness_Row.Visible = True
            CanEditICSRSeriousness_DropDownList.Enabled = False
            CanEditICSRSeriousness_DropDownList.CssClass = "form-control"
            CanEditICSRCompanyComment_Row.Visible = True
            CanEditICSRCompanyComment_DropDownList.Enabled = False
            CanEditICSRCompanyComment_DropDownList.CssClass = "form-control"
            CanEditReportComplexity_Row.Visible = True
            CanEditReportComplexity_DropDownList.Enabled = False
            CanEditReportComplexity_DropDownList.CssClass = "form-control"
            CanEditReportType_Row.Visible = True
            CanEditReportType_DropDownList.Enabled = False
            CanEditReportType_DropDownList.CssClass = "form-control"
            CanEditReportStatus_Row.Visible = True
            CanEditReportStatus_DropDownList.Enabled = False
            CanEditReportStatus_DropDownList.CssClass = "form-control"
            CanEditReportDue_Row.Visible = True
            CanEditReportDue_DropDownList.Enabled = False
            CanEditReportDue_DropDownList.CssClass = "form-control"
            CanEditReportExpeditedReportingRequired_Row.Visible = True
            CanEditReportExpeditedReportingRequired_DropDownList.Enabled = False
            CanEditReportExpeditedReportingRequired_DropDownList.CssClass = "form-control"
            CanEditReportExpeditedReportingDone_Row.Visible = True
            CanEditReportExpeditedReportingDone_DropDownList.Enabled = False
            CanEditReportExpeditedReportingDone_DropDownList.CssClass = "form-control"
            CanEditReportExpeditedReportingDate_Row.Visible = True
            CanEditReportExpeditedReportingDate_DropDownList.Enabled = False
            CanEditReportExpeditedReportingDate_DropDownList.CssClass = "form-control"
            CanEditRelations_Row.Visible = True
            CanEditRelations_DropDownList.Enabled = False
            CanEditRelations_DropDownList.CssClass = "form-control"
        Else
            EditRoleDetails_Button.Visible = False
            DeleteRole_Button.Visible = False
            SaveRoleUpdates_Button.Visible = False
            ConfirmRoleDeletion_Button.Visible = False
            Cancel_Button.Visible = True
            Status_Label.Visible = True
            Status_Label.CssClass = "form-control alert-danger"
            Status_Label.Text = "This role is currently used in one or more role allocation(s). To delete this role, please first delete all role allocations which use it."
            Name_Row.Visible = True
            Name_Textbox.ReadOnly = True
            Name_Textbox.ToolTip = String.Empty
            Name_Textbox.CssClass = "form-control"
            Description_Row.Visible = True
            Description_Textbox.ReadOnly = True
            Description_Textbox.ToolTip = String.Empty
            Description_Textbox.CssClass = "form-control"
            CanViewICSRs_Row.Visible = True
            CanViewICSRs_DropDownList.Enabled = False
            CanViewICSRs_DropDownList.CssClass = "form-control"
            CanCreateICSRs_Row.Visible = True
            CanCreateICSRs_Dropdownlist.Enabled = False
            CanCreateICSRs_Dropdownlist.CssClass = "form-control"
            CanEditICSRs_Row.Visible = True
            CanEditICSRs_DropDownList.Enabled = False
            CanEditICSRs_DropDownList.CssClass = "form-control"
            CanEditICSRAssignee_Row.Visible = True
            CanEditICSRAssignee_DropDownList.Enabled = False
            CanEditICSRAssignee_DropDownList.CssClass = "form-control"
            CanEditICSRSeriousness_Row.Visible = True
            CanEditICSRSeriousness_DropDownList.Enabled = False
            CanEditICSRSeriousness_DropDownList.CssClass = "form-control"
            CanEditICSRCompanyComment_Row.Visible = True
            CanEditICSRCompanyComment_DropDownList.Enabled = False
            CanEditICSRCompanyComment_DropDownList.CssClass = "form-control"
            CanEditReportComplexity_Row.Visible = True
            CanEditReportComplexity_DropDownList.Enabled = False
            CanEditReportComplexity_DropDownList.CssClass = "form-control"
            CanEditReportType_Row.Visible = True
            CanEditReportType_DropDownList.Enabled = False
            CanEditReportType_DropDownList.CssClass = "form-control"
            CanEditReportStatus_Row.Visible = True
            CanEditReportStatus_DropDownList.Enabled = False
            CanEditReportStatus_DropDownList.CssClass = "form-control"
            CanEditReportDue_Row.Visible = True
            CanEditReportDue_DropDownList.Enabled = False
            CanEditReportDue_DropDownList.CssClass = "form-control"
            CanEditReportExpeditedReportingRequired_Row.Visible = True
            CanEditReportExpeditedReportingRequired_DropDownList.Enabled = False
            CanEditReportExpeditedReportingRequired_DropDownList.CssClass = "form-control"
            CanEditReportExpeditedReportingDone_Row.Visible = True
            CanEditReportExpeditedReportingDone_DropDownList.Enabled = False
            CanEditReportExpeditedReportingDone_DropDownList.CssClass = "form-control"
            CanEditReportExpeditedReportingDate_Row.Visible = True
            CanEditReportExpeditedReportingDate_DropDownList.Enabled = False
            CanEditReportExpeditedReportingDate_DropDownList.CssClass = "form-control"
            CanEditRelations_Row.Visible = True
            CanEditRelations_DropDownList.Enabled = False
            CanEditRelations_DropDownList.CssClass = "form-control"
        End If
    End Sub

    Protected Sub ConfirmRoleDeletion_Button_Click(sender As Object, e As EventArgs) Handles ConfirmRoleDeletion_Button.Click
        EditRoleDetails_Button.Visible = False
        DeleteRole_Button.Visible = False
        SaveRoleUpdates_Button.Visible = False
        ConfirmRoleDeletion_Button.Visible = False
        Cancel_Button.Visible = False
        Status_Label.Visible = True
        Status_Label.CssClass = "form-control alert-success"
        Status_Label.Text = "Changes saved"
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
        Dim DeleteCommand As New SqlCommand("DELETE FROM Roles WHERE ID = @ID", Connection)
        DeleteCommand.Parameters.Add("@ID", SqlDbType.Int, 32, "ID")
        DeleteCommand.Parameters("@ID").Value = Request.QueryString("ID")
        Connection.Open()
        DeleteCommand.ExecuteNonQuery()
        Connection.Close()
    End Sub
End Class
