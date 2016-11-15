Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables

Partial Class Administration_CreateReportSource
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If Logged_In_User_Admin = True Then
                    Title_Label.Text = "Create New Report Source"
                    SaveNewReportSource_Button.Visible = True
                    Cancel_Button.Visible = True
                    Status_Label.Visible = True
                    Status_Label.CssClass = "form-control alert-warning"
                    Status_Label.Text = "Changes not saved"
                    Name_Row.Visible = True
                    Name_Textbox.ReadOnly = False
                    Name_Textbox.ToolTip = "Please enter a report source name"
                    Name_Textbox.CssClass = "form-control"
                    SortOrder_Row.Visible = True
                    SortOrder_Textbox.ReadOnly = False
                    SortOrder_Textbox.ToolTip = "Please enter a sort order"
                    SortOrder_Textbox.CssClass = "form-control"
                    Active_Row.Visible = True
                    Active_DropDownList.Enabled = True
                    Active_DropDownList.ToolTip = "Please select 'True' or 'False'"
                    Active_DropDownList.CssClass = "form-control"
                Else
                    Title_Label.Text = Lockout_Text
                    SaveNewReportSource_Button.Visible = False
                    Cancel_Button.Visible = False
                    Status_Label.Visible = False
                    Name_Row.Visible = False
                    SortOrder_Textbox.Visible = False
                    Active_Row.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Administration/CreateReportSource.aspx")
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Administration/Administration.aspx")
    End Sub

    Protected Sub Name_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if entered report Source name is already taken and fail validation if not
        Dim Command As New SqlCommand("SELECT Name FROM ReportSources", Connection)
        Dim Reader As SqlDataReader
        Connection.Open()
        Reader = Command.ExecuteReader()
        Dim Db_Name As String = String.Empty
        While Reader.Read()
            Db_Name = Reader.GetString(0)
            If Db_Name = Name_Textbox.Text Then
                Name_Textbox.CssClass = "form-control alert-danger"
                Name_Textbox.ToolTip = "The report source name you have entered is already taken. Please choose a different report source name"
                Connection.Close()
                args.IsValid = False
                Exit Sub
            End If
        End While
        Connection.Close()
        'Check if report source name was entered and fail validation if not
        If Name_Textbox.Text.Trim <> String.Empty Then
            Name_Textbox.CssClass = "form-control alert-success"
            Name_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Name_Textbox.CssClass = "form-control alert-danger"
            Name_Textbox.ToolTip = "Please make sure you are entering a valid report source name"
            args.IsValid = False
        End If
    End Sub

    Protected Sub SortOrder_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if entered sort order is already taken and fail validation if not
        Dim Command As New SqlCommand("SELECT SortOrder FROM ReportSources", Connection)
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

    Protected Sub SaveNewReportSource_Button_Click(sender As Object, e As EventArgs) Handles SaveNewReportSource_Button.Click
        If Page.IsValid = True Then
            Title_Label.Text = "Create New Report Source"
            SaveNewReportSource_Button.Visible = False
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
            Dim Command As New SqlCommand("INSERT INTO ReportSources(Name, SortOrder, Active) VALUES(@Name, @SortOrder, @Active)", Connection)
            Command.Parameters.Add("@Name", SqlDbType.NVarChar, 200)
            Command.Parameters("@Name").Value = Name_Textbox.Text
            Command.Parameters.Add("@SortOrder", SqlDbType.Int, 32)
            Command.Parameters("@SortOrder").Value = CType(SortOrder_Textbox.Text, Integer)
            Command.Parameters.Add("@Active", SqlDbType.Bit, 1)
            Command.Parameters("@Active").Value = Active_DropDownList.SelectedValue
            Connection.Open()
            Command.ExecuteNonQuery()
            Connection.Close()
        End If
    End Sub
End Class
