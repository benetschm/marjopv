Imports GlobalVariables

Partial Class MasterPages_SiteMaster
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If global variable LoggedIn_User_ID is not set or the current session ID does not match that stored during login 
        If LoggedIn_User_ID = Nothing Or Session.SessionID <> LoggedIn_User_Session_ID Then
            'Set Login_Status to False
            Login_Status = False
            'Clear LoggedIn_User Details
            LoggedIn_User_ID = Nothing
            LoggedIn_User_Name = String.Empty
            LoggedIn_User_Session_ID = String.Empty
            LoggedIn_User_Companies.Clear()
            Logged_In_User_Admin = False
            LoggedIn_User_CanCreateICSRs = False
            LoggedIn_User_Linkbutton.Visible = False
            LoginStatus_Linkbutton.Text = "Log In"
            'If global variable LoggedIn_User_ID is set and the current session ID matches that stored during login 
        Else
            'Set Login_Status to True
            Login_Status = True
            'Populate LoggedIn_User Details
            LoggedIn_User_Linkbutton.Text = LoggedIn_User_Name
            LoggedIn_User_Linkbutton.Visible = True
            LoggedIn_User_Linkbutton.PostBackUrl = "~/Application/Account.aspx"
            LoginStatus_Linkbutton.Text = "Log out"
        End If
        'Show/Hide navigation options depending on login status
        If Login_Status = True Then
            ICSRs_Link.Visible = True
            Medications_Link.Visible = True
            Reporting_Link.Visible = True
            If Logged_In_User_Admin = True Then
                Adminsitration_Link.Visible = True
            Else
                Adminsitration_Link.Visible = False
            End If
        Else
            ICSRs_Link.Visible = False
            Medications_Link.Visible = False
            Reporting_Link.Visible = False
            Adminsitration_Link.Visible = False
        End If
    End Sub

    Protected Sub LoginStatus_Linkbutton_Click(sender As Object, e As EventArgs) Handles LoginStatus_Linkbutton.Click
        'Check Login Status, redirect user to login if Login_Status = False and log out user if Login_Status = True
        If Login_Status = False Then 'If user is not logged in
            Response.Redirect("~/Login.aspx")
        Else 'If user is logged in
            'Clear LoggedIn_User Details
            Login_Status = False
            LoggedIn_User_ID = Nothing
            LoggedIn_User_Name = String.Empty
            LoggedIn_User_Session_ID = String.Empty
            LoggedIn_User_Companies.Clear()
            Logged_In_User_Admin = False
            'Redirect to Homepage
            Response.Redirect("~/Default.aspx")
        End If
    End Sub
End Class

