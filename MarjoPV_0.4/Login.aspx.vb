Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables

Partial Class Account_Login
    Inherits System.Web.UI.Page

    Protected Sub LogIn_Click(sender As Object, e As EventArgs) Handles LogIn.Click
        If Page.IsValid = True Then
            'Initialize session by storing current session ID in the session state
            Session("Current_Session") = Session.SessionID
            'Read account details of user logging in from database
            Dim UserReadCommand As New SqlCommand("SELECT ID, Name, Admin, CanViewCompanies FROM Users WHERE Username = @Username", Connection)
            UserReadCommand.Parameters.Add("@Username", SqlDbType.NVarChar, 200)
            UserReadCommand.Parameters("@Username").Value = Username_TextBox.Text
            Dim UserReader As SqlDataReader
            Dim User_ID As Integer = Nothing
            Dim User_Name As String = String.Empty
            Dim User_IsAdmin As Boolean = False
            Dim User_CanViewCompanies As Boolean = False
            Try
                Connection.Open()
                UserReader = UserReadCommand.ExecuteReader()
                While UserReader.Read()
                    Try
                        User_ID = UserReader.GetInt32(0)
                    Catch ex As Exception
                        User_ID = Nothing
                    End Try
                    Try
                        User_Name = UserReader.GetString(1)
                    Catch ex As Exception
                        User_Name = String.Empty
                    End Try
                    Try
                        User_IsAdmin = UserReader.GetBoolean(2)
                    Catch ex As Exception
                        User_IsAdmin = False
                    End Try
                    Try
                        User_CanViewCompanies = UserReader.GetBoolean(3)
                    Catch ex As Exception
                        User_CanViewCompanies = False
                    End Try
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Set global login variables for user logging in
            LoggedIn_User_ID = User_ID
            Logged_In_Username = Username_TextBox.Text
            LoggedIn_User_Session_ID = Session.SessionID
            LoggedIn_User_Name = User_Name
            Logged_In_User_Admin = User_IsAdmin
            LoggedIn_User_CanViewCompanies = User_CanViewCompanies
            Login_Status = True
            'Check if user has role allocation which permits creation of ICSRs and set Logged_In_User_CanCreateICSRs to True if so
            Dim CanCreateICSRsCommand As New SqlCommand("SELECT RoleAllocations.ID FROM RoleAllocations INNER JOIN Roles ON RoleAllocations.Role_ID = Roles.ID WHERE Roles.CanCreateICSRs = @CanCreateICSRs AND User_ID = @ID", Connection)
            CanCreateICSRsCommand.Parameters.AddWithValue("@CanCreateICSRs", 1)
            CanCreateICSRsCommand.Parameters.AddWithValue("@ID", LoggedIn_User_ID)
            Dim CanCreateICSRsReader As SqlDataReader
            Dim Db_RoleAllocation_ID As Integer = Nothing
            Try
                Connection.Open()
                CanCreateICSRsReader = CanCreateICSRsCommand.ExecuteReader()
                While CanCreateICSRsReader.Read()
                    Db_RoleAllocation_ID = CanCreateICSRsReader.GetInt32(0)
                    If Db_RoleAllocation_ID > 0 Then
                        LoggedIn_User_CanCreateICSRs = True
                    End If
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Write the session ID And the login timepoint to user dataset
            Dim UpdateCommand As New SqlCommand("UPDATE Users SET CurrentSession = @CurrentSession, LastLogin = @LastLogin WHERE ID = @ID", Connection)
            UpdateCommand.Parameters.Add("@CurrentSession", SqlDbType.NVarChar, 200, "CurrentSession")
            UpdateCommand.Parameters("@CurrentSession").Value = Session.SessionID
            UpdateCommand.Parameters.Add("@LastLogin", SqlDbType.DateTime, 200, "LastLogin")
            UpdateCommand.Parameters("@LastLogin").Value = Now()
            UpdateCommand.Parameters.Add("@ID", SqlDbType.Int, 32, "ID")
            UpdateCommand.Parameters("@ID").Value = LoggedIn_User_ID
            Try
                Connection.Open()
                UpdateCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Redirect user to ICSRs or to ReturnUrl 
            If Request.QueryString("ReturnUrl") = String.Empty Then
                Response.Redirect("~/Application/ICSRs.aspx")
            Else
                Response.Redirect(Request.QueryString("ReturnUrl"))
            End If
        End If
    End Sub

    Protected Sub Username_TextBox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if username was entered and fail validation if not
        If Username_TextBox.Text = String.Empty Then
            Username_TextBox.CssClass = "form-control alert-danger"
            Username_TextBox.ToolTip = "Please make sure you are entering a valid user name"
            args.IsValid = False
        Else
            Username_TextBox.CssClass = "form-control alert-success"
            Username_TextBox.ToolTip = String.Empty
            args.IsValid = True
        End If
    End Sub

    Protected Sub Password_TextBox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        'Check if username was entered and if entered password matches username and fail validation if not
        If Password_TextBox.Text = String.Empty Then
            Password_TextBox.CssClass = "form-control alert-danger"
            Password_TextBox.ToolTip = "Please make sure you are entering a valid password"
            args.IsValid = False
        Else
            Dim UserReadCommand As New SqlCommand("SELECT ID, Username, Password FROM Users WHERE Active = @Active", Connection)
            UserReadCommand.Parameters.AddWithValue("@Active", 1)
            Dim UserReader As SqlDataReader
            Dim ID As Integer = Nothing
            Dim Username As String = String.Empty
            Dim Password As String = String.Empty
            Try
                Connection.Open()
                UserReader = UserReadCommand.ExecuteReader()
                While UserReader.Read()
                    Try
                        ID = UserReader.GetInt32(0)
                    Catch ex As Exception
                        ID = Nothing
                    End Try
                    Try
                        Username = UserReader.GetString(1)
                    Catch ex As Exception
                        Username = String.Empty
                    End Try
                    Try
                        Password = UserReader.GetString(2)
                    Catch ex As Exception
                        Password = String.Empty
                    End Try
                    If Username = Username_TextBox.Text Then
                        If Password = FormsAuthentication.HashPasswordForStoringInConfigFile(Password_TextBox.Text, System.Web.Configuration.FormsAuthPasswordFormat.SHA1.ToString()) Then
                            Connection.Close()
                            Username_TextBox.CssClass = "form-control alert-success"
                            Username_TextBox.ToolTip = String.Empty
                            Password_TextBox.CssClass = "form-control alert-success"
                            Password_TextBox.ToolTip = String.Empty
                            args.IsValid = True
                            Exit Sub
                        End If
                    Else
                        Username_TextBox.CssClass = "form-control alert-danger"
                        Username_TextBox.ToolTip = "Username and password do not match. Please re-enter"
                        Password_TextBox.CssClass = "form-control alert-danger"
                        Password_TextBox.ToolTip = "Username and password do not match. Please re-enter"
                        args.IsValid = False
                    End If
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
        End If
    End Sub
End Class
