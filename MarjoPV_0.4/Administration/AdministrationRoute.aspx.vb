Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables

Partial Class Administration_AdministrationRoute
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "View Administration Route"
                    EditAdministrationRoute_Button.Visible = True
                    DeleteAdministrationRoute_Button.Visible = True
                    SaveAdministrationRouteUpdate_Button.Visible = False
                    ConfirmAdministrationRouteDeletion_Button.Visible = False
                    Cancel_Button.Visible = False
                    Status_Label.Visible = True
                    Status_Label.CssClass = "form-control alert-success"
                    Status_Label.Text = "No changes pending"
                    Name_Row.Visible = True
                    Name_Textbox.ReadOnly = True
                    Name_Textbox.ToolTip = String.Empty
                    Name_Textbox.CssClass = "form-control"
                    SortOrder_Row.Visible = True
                    SortOrder_Textbox.ReadOnly = True
                    SortOrder_Textbox.ToolTip = String.Empty
                    SortOrder_Textbox.CssClass = "form-control"
                    Active_Row.Visible = True
                    Active_DropDownList.Enabled = False
                    Active_DropDownList.ToolTip = String.Empty
                    Active_DropDownList.CssClass = "form-control"
                    Dim ReadCommand As New SqlCommand("SELECT Name, SortOrder, Active FROM AdministrationRoutes WHERE ID = @ID", Connection)
                    ReadCommand.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
                    Dim Reader As SqlDataReader
                    Dim Db_Name As String = String.Empty
                    Dim Db_SortOrder As Integer = Nothing
                    Dim Db_Active As Boolean = False
                    Connection.Open()
                    Reader = ReadCommand.ExecuteReader()
                    Reader.Read()
                    Try
                        Db_Name = Reader.GetString(0)
                    Catch ex As Exception
                        Db_Name = String.Empty
                    End Try
                    Try
                        Db_SortOrder = Reader.GetInt32(1)
                    Catch ex As Exception
                        Db_SortOrder = Nothing
                    End Try
                    Try
                        Db_Active = Reader.GetBoolean(2)
                    Catch ex As Exception
                        Db_Active = False
                    End Try
                    Connection.Close()
                    Name_Textbox.Text = Db_Name
                    SortOrder_Textbox.Text = Db_SortOrder
                    Active_DropDownList.SelectedValue = Db_Active
                Else
                    Title_Label.Text = Lockout_Text
                    EditAdministrationRoute_Button.Visible = False
                    DeleteAdministrationRoute_Button.Visible = False
                    SaveAdministrationRouteUpdate_Button.Visible = False
                    ConfirmAdministrationRouteDeletion_Button.Visible = False
                    Cancel_Button.Visible = False
                    Status_Label.Visible = False
                    Name_Row.Visible = False
                    SortOrder_Textbox.Visible = False
                    Active_Row.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/AdministrationRoute.aspx?ID=" & Request.QueryString("ID"))
            End If
        End If
    End Sub

    Protected Sub EditAdministrationRoute_Button_Click(sender As Object, e As EventArgs) Handles EditAdministrationRoute_Button.Click
        Title_Label.Text = "Edit Administration Route"
        EditAdministrationRoute_Button.Visible = False
        DeleteAdministrationRoute_Button.Visible = False
        SaveAdministrationRouteUpdate_Button.Visible = True
        ConfirmAdministrationRouteDeletion_Button.Visible = False
        Cancel_Button.Visible = True
        Status_Label.Visible = True
        Status_Label.CssClass = "form-control alert-warning"
        Status_Label.Text = "Changes not saved"
        Name_Row.Visible = True
        Name_Textbox.ReadOnly = False
        Name_Textbox.ToolTip = "Please enter a administration route name"
        Name_Textbox.CssClass = "form-control"
        SortOrder_Row.Visible = True
        SortOrder_Textbox.ReadOnly = False
        SortOrder_Textbox.ToolTip = "Please enter a sort order"
        SortOrder_Textbox.CssClass = "form-control"
        Active_Row.Visible = True
        Active_DropDownList.Enabled = True
        Active_DropDownList.ToolTip = "Please select 'True' or 'False'"
        Active_DropDownList.CssClass = "form-control"
    End Sub

    Protected Sub DeleteAdministrationRoute_Button_Click(sender As Object, e As EventArgs) Handles DeleteAdministrationRoute_Button.Click
        Title_Label.Text = "Permanently Delete Administration Route"
        EditAdministrationRoute_Button.Visible = False
        DeleteAdministrationRoute_Button.Visible = False
        SaveAdministrationRouteUpdate_Button.Visible = False
        ConfirmAdministrationRouteDeletion_Button.Visible = True
        Cancel_Button.Visible = True
        Status_Label.Visible = True
        Status_Label.CssClass = "form-control alert-danger"
        Status_Label.Text = "Confirming will permanently delete this administration route"
        Name_Row.Visible = True
        Name_Textbox.ReadOnly = True
        Name_Textbox.ToolTip = String.Empty
        Name_Textbox.CssClass = "form-control"
        SortOrder_Row.Visible = True
        SortOrder_Textbox.ReadOnly = True
        SortOrder_Textbox.ToolTip = String.Empty
        SortOrder_Textbox.CssClass = "form-control"
        Active_Row.Visible = True
        Active_DropDownList.Enabled = False
        Active_DropDownList.ToolTip = String.Empty
        Active_DropDownList.CssClass = "form-control"
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Title_Label.Text = "View Administration Route"
        EditAdministrationRoute_Button.Visible = True
        DeleteAdministrationRoute_Button.Visible = True
        SaveAdministrationRouteUpdate_Button.Visible = False
        ConfirmAdministrationRouteDeletion_Button.Visible = False
        Cancel_Button.Visible = False
        Status_Label.Visible = True
        Status_Label.CssClass = "form-control alert-success"
        Status_Label.Text = "No changes pending"
        Name_Row.Visible = True
        Name_Textbox.ReadOnly = True
        Name_Textbox.ToolTip = String.Empty
        Name_Textbox.CssClass = "form-control"
        SortOrder_Row.Visible = True
        SortOrder_Textbox.ReadOnly = True
        SortOrder_Textbox.ToolTip = String.Empty
        SortOrder_Textbox.CssClass = "form-control"
        Active_Row.Visible = True
        Active_DropDownList.Enabled = False
        Active_DropDownList.ToolTip = String.Empty
        Active_DropDownList.CssClass = "form-control"
        Dim ReadCommand As New SqlCommand("SELECT Name, SortOrder, Active FROM AdministrationRoutes WHERE ID = @ID", Connection)
        ReadCommand.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
        Dim Reader As SqlDataReader
        Dim Db_Name As String = String.Empty
        Dim Db_SortOrder As Integer = Nothing
        Dim Db_Active As Boolean = False
        Connection.Open()
        Reader = ReadCommand.ExecuteReader()
        Reader.Read()
        Try
            Db_Name = Reader.GetString(0)
        Catch ex As Exception
            Db_Name = String.Empty
        End Try
        Try
            Db_SortOrder = Reader.GetInt32(1)
        Catch ex As Exception
            Db_SortOrder = Nothing
        End Try
        Try
            Db_Active = Reader.GetBoolean(2)
        Catch ex As Exception
            Db_Active = False
        End Try
        Connection.Close()
        Name_Textbox.Text = Db_Name
        SortOrder_Textbox.Text = Db_SortOrder
        Active_DropDownList.SelectedValue = Db_Active
    End Sub

    Protected Sub Name_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if entered report Type name is already taken and fail validation if not
        Dim Command As New SqlCommand("SELECT Name FROM AdministrationRoutes WHERE ID <> @ID", Connection)
        Command.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
        Dim Reader As SqlDataReader
        Connection.Open()
        Reader = Command.ExecuteReader()
        Dim Db_Name As String = String.Empty
        While Reader.Read()
            Db_Name = Reader.GetString(0)
            If Db_Name = Name_Textbox.Text Then
                Name_Textbox.CssClass = "form-control alert-danger"
                Name_Textbox.ToolTip = "The administration route name you have entered is already taken. Please choose a different administration route name"
                Connection.Close()
                args.IsValid = False
                Exit Sub
            End If
        End While
        Connection.Close()
        'Check if report Type name was entered and fail validation if not
        If Name_Textbox.Text.Trim <> String.Empty Then
            Name_Textbox.CssClass = "form-control alert-success"
            Name_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Name_Textbox.CssClass = "form-control alert-danger"
            Name_Textbox.ToolTip = "Please make sure you are entering a valid administration route name"
            args.IsValid = False
        End If
    End Sub

    Protected Sub SortOrder_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if entered sort order is already taken and fail validation if not
        Dim Command As New SqlCommand("SELECT SortOrder FROM AdministrationRoutes WHERE ID <> @ID", Connection)
        Command.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
        Dim Reader As SqlDataReader
        Connection.Open()
        Reader = Command.ExecuteReader()
        Dim Db_SortOrder As String = String.Empty
        While Reader.Read()
            Db_SortOrder = Reader.GetInt32(0).ToString
            If Db_SortOrder = SortOrder_Textbox.Text Then
                SortOrder_Textbox.CssClass = "form-control alert-danger"
                SortOrder_Textbox.ToolTip = "The sort order entry you have specified is already taken. Please choose a different entry"
                Connection.Close()
                args.IsValid = False
                Exit Sub
            End If
        End While
        Connection.Close()
        'Check if sort order was entered as numeric characters only and fail validation if not
        If SortOrder_Textbox.Text.Trim = String.Empty Then
            SortOrder_Textbox.CssClass = "form-control alert-danger"
            SortOrder_Textbox.ToolTip = "Please make sure you are specifying a valid sort order"
            args.IsValid = False
        ElseIf Regex.IsMatch(SortOrder_Textbox.Text, "^\d{0,16}$") Then
            SortOrder_Textbox.CssClass = "form-control alert-success"
            SortOrder_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            SortOrder_Textbox.CssClass = "form-control alert-danger"
            SortOrder_Textbox.ToolTip = "Please make sure you are specifying a valid sort order"
            args.IsValid = False
        End If
    End Sub

    Protected Sub Active_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Active_DropDownList.CssClass = "form-control alert-success"
        Active_DropDownList.ToolTip = String.Empty
        args.IsValid = True
    End Sub

    Protected Sub SaveAdministrationRouteUpdate_Button_Click(sender As Object, e As EventArgs) Handles SaveAdministrationRouteUpdate_Button.Click
        If Page.IsValid = True Then
            Title_Label.Text = "Edit Administration Route"
            EditAdministrationRoute_Button.Visible = True
            SaveAdministrationRouteUpdate_Button.Visible = True
            ConfirmAdministrationRouteDeletion_Button.Visible = False
            Cancel_Button.Visible = False
            Status_Label.Visible = True
            Status_Label.CssClass = "form-control alert-success"
            Status_Label.Text = "Changes saved"
            Name_Row.Visible = True
            Name_Textbox.ReadOnly = True
            Name_Textbox.ToolTip = String.Empty
            Name_Textbox.CssClass = "form-control alert-success"
            SortOrder_Row.Visible = True
            SortOrder_Textbox.ReadOnly = True
            SortOrder_Textbox.ToolTip = String.Empty
            SortOrder_Textbox.CssClass = "form-control alert-success"
            Active_Row.Visible = True
            Active_DropDownList.Enabled = False
            Active_DropDownList.ToolTip = String.Empty
            Active_DropDownList.CssClass = "form-control alert-success"
            Dim UpdateCommand As New SqlCommand("UPDATE AdministrationRoutes SET Name = @Name, SortOrder = @SortOrder, Active = @Active WHERE ID = @ID", Connection)
            UpdateCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 200)
            UpdateCommand.Parameters("@Name").Value = Name_Textbox.Text
            UpdateCommand.Parameters.Add("@SortOrder", SqlDbType.Int, 32)
            UpdateCommand.Parameters("@SortOrder").Value = CType(SortOrder_Textbox.Text, Integer)
            UpdateCommand.Parameters.Add("@Active", SqlDbType.Bit, 1)
            UpdateCommand.Parameters("@Active").Value = Active_DropDownList.SelectedValue
            UpdateCommand.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
            Connection.Open()
            UpdateCommand.ExecuteNonQuery()
            Connection.Close()
        End If
    End Sub

    Protected Sub ConfirmAdministrationRouteDeletion_Button_Click(sender As Object, e As EventArgs) Handles ConfirmAdministrationRouteDeletion_Button.Click
        Title_Label.Text = "Permanently Delete Administration Route"
        EditAdministrationRoute_Button.Visible = True
        DeleteAdministrationRoute_Button.Visible = True
        SaveAdministrationRouteUpdate_Button.Visible = False
        ConfirmAdministrationRouteDeletion_Button.Visible = False
        Cancel_Button.Visible = False
        Status_Label.Visible = True
        Status_Label.CssClass = "form-control alert-success"
        Status_Label.Text = "Changes saved"
        Name_Row.Visible = False
        SortOrder_Row.Visible = False
        Active_Row.Visible = False
        Dim DeleteCommand As New SqlCommand("DELETE FROM AdministrationRoutes WHERE ID = @ID", Connection)
        DeleteCommand.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
        Connection.Open()
        DeleteCommand.ExecuteNonQuery()
        Connection.Close()
    End Sub
End Class
