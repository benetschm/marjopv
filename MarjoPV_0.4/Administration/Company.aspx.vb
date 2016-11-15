Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables

Partial Class Administration_Company
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "Edit Selected Company Details"
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
                    TextBoxReadWrite(Name_Textbox)
                    TextBoxReadWrite(Street_Textbox)
                    TextBoxReadWrite(PostalCode_Textbox)
                    TextBoxReadWrite(City_Textbox)
                    TextBoxReadWrite(Country_Textbox)
                    TextBoxReadWrite(SortOrder_Textbox)
                    DropDownListEnabled(Active_DropDownList)
                    Dim Command As New SqlCommand("SELECT Name, CASE WHEN Street IS NULL THEN '' ELSE Street END AS Street, CASE WHEN PostalCode IS NULL THEN '' ELSE PostalCode END AS PostalCode, CASE WHEN City IS NULL THEN '' ELSE City END AS City, CASE WHEN Country IS NULL THEN '' ELSE Country END AS Country, SortOrder, Active FROM Companies WHERE ID = @ID", Connection)
                    Command.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
                    Try
                        Connection.Open()
                        Dim Reader As SqlDataReader = Command.ExecuteReader()
                        While Reader.Read()
                            Name_Textbox.Text = Reader.GetString(0)
                            Street_Textbox.Text = Reader.GetString(1)
                            PostalCode_Textbox.Text = Reader.GetString(2)
                            City_Textbox.Text = Reader.GetString(3)
                            Country_Textbox.Text = Reader.GetString(4)
                            SortOrder_Textbox.Text = Reader.GetInt32(5)
                            Active_DropDownList.SelectedValue = Reader.GetBoolean(6)
                        End While
                    Catch ex As Exception
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                        Exit Sub
                    Finally
                        Connection.Close()
                    End Try
                Else
                    Title_Label.Text = Lockout_Text
                    SaveUpdates_Button.Visible = False
                    Cancel_Button.Visible = False
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/Company.aspx?ID=" & Request.QueryString("ID"))
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Command(sender As Object, e As CommandEventArgs) Handles Cancel_Button.Command
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub Name_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if entered company name is already taken and fail validation if so
        Dim Command As New SqlCommand("SELECT Name FROM Companies WHERE ID <> @ID", Connection)
        Command.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
        Dim Name As String = String.Empty
        Try
            Connection.Open()
            Dim Reader As SqlDataReader = Command.ExecuteReader()
            While Reader.Read()
                If Reader.GetString(0) = Name_Textbox.Text.Trim Then
                    Name_Textbox.CssClass = CssClassFailure
                    Name_Textbox.ToolTip = CompanyNameTakenValidationFailToolTip
                    Connection.Close()
                    args.IsValid = False
                    Exit Sub
                End If
            End While
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
        'Check if company name was entered and fail validation if not
        If Name_Textbox.Text = String.Empty Then
            Name_Textbox.CssClass = CssClassFailure
            Name_Textbox.ToolTip = NameValidationFailToolTip
            args.IsValid = False
        Else
            Name_Textbox.CssClass = CssClassSuccess
            Name_Textbox.ToolTip = String.Empty
            args.IsValid = True
        End If
    End Sub

    Protected Sub Street_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Street_Textbox.CssClass = CssClassSuccess
        Street_Textbox.ToolTip = String.Empty
        args.IsValid = True
    End Sub

    Protected Sub PostalCode_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        PostalCode_Textbox.CssClass = CssClassSuccess
        PostalCode_Textbox.ToolTip = String.Empty
        args.IsValid = True
    End Sub

    Protected Sub City_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        City_Textbox.CssClass = CssClassSuccess
        City_Textbox.ToolTip = String.Empty
        args.IsValid = True
    End Sub

    Protected Sub Country_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Country_Textbox.CssClass = CssClassSuccess
        Country_Textbox.ToolTip = String.Empty
        args.IsValid = True
    End Sub

    Protected Sub SortOrder_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If SortOrder_Textbox.Text.Trim = String.Empty Then
            SortOrder_Textbox.CssClass = CssClassFailure
            SortOrder_Textbox.ToolTip = SortOrderValidationFailToolTip
            args.IsValid = False
        ElseIf Regex.IsMatch(SortOrder_Textbox.Text, "^\d{0,16}$") Then
            SortOrder_Textbox.CssClass = CssClassSuccess
            SortOrder_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            SortOrder_Textbox.CssClass = CssClassFailure
            SortOrder_Textbox.ToolTip = SortOrderValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Active_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Active_DropDownList.CssClass = CssClassSuccess
        Active_DropDownList.ToolTip = String.Empty
        args.IsValid = True
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
            TextBoxReadOnly(Name_Textbox)
            TextBoxReadOnly(Street_Textbox)
            TextBoxReadOnly(PostalCode_Textbox)
            TextBoxReadOnly(City_Textbox)
            TextBoxReadOnly(Country_Textbox)
            TextBoxReadOnly(SortOrder_Textbox)
            DropDownListDisabled(Active_DropDownList)
            Dim UpdateCommand As New SqlCommand("UPDATE Companies SET Name = @Name, Street = (CASE WHEN @Street = '' THEN NULL ELSE @Street END), PostalCode = (CASE WHEN @PostalCode = '' THEN NULL ELSE @PostalCode END), City = (CASE WHEN @City = '' THEN NULL ELSE @City END), Country = (CASE WHEN @Country = '' THEN NULL ELSE @Country END), SortOrder = @SortOrder, Active = @Active WHERE ID = @ID", Connection)
            UpdateCommand.Parameters.AddWithValue("@Name", Name_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@Street", Street_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@PostalCode", PostalCode_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@City", City_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@Country", Country_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@SortOrder", CType(SortOrder_Textbox.Text, Integer))
            UpdateCommand.Parameters.AddWithValue("@Active", Active_DropDownList.SelectedValue)
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
        End If
    End Sub
End Class
