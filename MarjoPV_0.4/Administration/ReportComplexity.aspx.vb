Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalCode
Imports GlobalVariables

Partial Class Administration_ReportComplexity
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "Edit Report Complexity"
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
                    TextBoxReadOnly(Name_Textbox)
                    Name_Textbox.ToolTip = "Report complexity names cannot be changed. Instead, deactivate the current report status and create a new report status with the desired name."
                    TextBoxReadWrite(SortOrder_Textbox)
                    DropDownListEnabled(Active_DropDownList)
                    Dim ReadCommand As New SqlCommand("SELECT Name, SortOrder, Active FROM ReportComplexities WHERE ID = @ID", Connection)
                    ReadCommand.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
                    Try
                        Dim Reader As SqlDataReader = ReadCommand.ExecuteReader()
                        While Reader.Read()
                            Name_Textbox.Text = Reader.GetString(0)
                            SortOrder_Textbox.Text = Reader.GetString(1)
                            Active_DropDownList.SelectedValue = Reader.GetBoolean(2)
                        End While
                    Catch ex As Exception
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Finally
                        Connection.Close()
                    End Try
                Else
                    Title_Label.Text = Lockout_Text
                    Buttons.Visible = False
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/ReportComplexity.aspx?ID=" & Request.QueryString("ID"))
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub SortOrder_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if sort order was entered as numeric characters only and fail validation if not
        If IntegerValidator(SortOrder_Textbox) = True Then
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
        Active_DropDownList.CssClass = "form-control alert-success"
        Active_DropDownList.ToolTip = String.Empty
        args.IsValid = True
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, Nothing)
            TextBoxReadOnly(Name_Textbox)
            TextBoxReadOnly(SortOrder_Textbox)
            DropDownListDisabled(Active_DropDownList)
            Dim UpdateCommand As New SqlCommand("UPDATE ReportComplexities SET Name = @Name, SortOrder = @SortOrder, Active = @Active WHERE ID = @ID", Connection)
            UpdateCommand.Parameters.AddWithValue("@Name", Name_Textbox.Text)
            UpdateCommand.Parameters.AddWithValue("@SortOrder", CType(SortOrder_Textbox.Text, Integer))
            UpdateCommand.Parameters.AddWithValue("@Active", Active_DropDownList.SelectedValue)
            UpdateCommand.Parameters.AddWithValue("@ID", Request.QueryString("ID"))
            Try
                Connection.Open()
                UpdateCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Finally
                Connection.Close()
            End Try
        End If
    End Sub

End Class
