Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables

Partial Class Application_CreateICSR
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Login_Status = True Then
                If CanEdit(tables.ICSRs, Nothing, tables.ICSRs, fields.Create) = True Then
                    Title_Label.Text = "Create New ICSR"
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
                    ConfirmICSRInput_Button.Visible = False
                    Company_Row.Visible = True
                    Companies_DropDownList.Enabled = True
                    Companies_DropDownList.ToolTip = "Please select a company"
                    Companies_DropDownList.CssClass = "form-control"
                    'Populate Companies_Dropdownlist
                    Dim CompaniesReadCommand As New SqlCommand("SELECT RoleAllocations.Company_ID AS Company_ID, Companies.Name AS Company_Name FROM RoleAllocations INNER JOIN Roles ON RoleAllocations.Role_ID = Roles.ID INNER JOIN Companies ON RoleAllocations.Company_ID = Companies.ID WHERE Companies.Active = @Active AND Roles.CanCreateICSRs = @CanCreateICSRs AND User_ID = @ID ORDER BY Companies.SortOrder", Connection)
                    CompaniesReadCommand.Parameters.AddWithValue("@Active", 1)
                    CompaniesReadCommand.Parameters.AddWithValue("@CanCreateICSRs", 1)
                    CompaniesReadCommand.Parameters.AddWithValue("@ID", LoggedIn_User_ID)
                    Try
                        Connection.Open()
                        Companies_DropDownList.DataSource = CompaniesReadCommand.ExecuteReader()
                        Companies_DropDownList.DataValueField = "Company_ID"
                        Companies_DropDownList.DataTextField = "Company_Name"
                        Companies_DropDownList.DataBind()
                        Connection.Close()
                    Catch ex As Exception
                        Connection.Close()
                        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                        Exit Sub
                    End Try
                Else
                    Title_Label.Text = Lockout_Text
                    ButtonGroup_Div.Visible = False
                    Main_Table.Visible = False
                End If
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/CreateICSR.aspx")
            End If
        End If
    End Sub

    Protected Sub ReturnToICSROverview() Handles Cancel_Button.Click, ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Response.Redirect("~/Application/ICSRs.aspx")
    End Sub

    Protected Sub Companies_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(Companies_DropDownList) = True Then
            Companies_DropDownList.CssClass = CssClassSuccess
            Companies_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            Companies_DropDownList.CssClass = CssClassFailure
            Companies_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveNewICSR_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            ConfirmICSRInput_Button.Visible = True
            SaveUpdates_Button.Visible = False
            Cancel_Button.Visible = True
            Status_Label.Visible = True
            Status_Label.CssClass = "form-control alert-danger"
            Status_Label.Text = "WARNING: Company cannot be altered after saving a New ICSR. Please verify that your input is correct"
            Company_Row.Visible = True
            Companies_DropDownList.Enabled = False
            Companies_DropDownList.ToolTip = String.Empty
            Companies_DropDownList.CssClass = "form-control"
        End If
    End Sub

    Protected Sub ConfirmICSRInput_Button_Click(sender As Object, e As EventArgs) Handles ConfirmICSRInput_Button.Click
        'Determine active ICSRStatus_ID of ICSRStatus where IsStatusNew = True
        Dim ICSRStatus_ID As Integer = Nothing
        Dim ReadCommand As New SqlCommand("SELECT TOP 1 ID From ICSRStatuses WHERE Active = @Active AND IsStatusNew = @IsStatusNew", Connection)
        ReadCommand.Parameters.AddWithValue("@IsStatusNew", 1)
        ReadCommand.Parameters.AddWithValue("@Active", 1)
        Try
            Connection.Open()
            Dim Reader As SqlDataReader = ReadCommand.ExecuteReader()
            While Reader.Read()
                ICSRStatus_ID = Reader.GetSqlInt32(0)
            End While
            Connection.Close()
        Catch ex As Exception
            Connection.Close()
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        End Try
        'Write New ICSR to database
        Dim InsertICSRCommand As New SqlCommand("INSERT INTO ICSRs(Company_ID, ICSRStatus_ID) VALUES(@Company_ID, @ICSRStatus_ID)", Connection)
        InsertICSRCommand.Parameters.Add("@Company_ID", SqlDbType.Int, 32)
        InsertICSRCommand.Parameters("@Company_ID").Value = Companies_DropDownList.SelectedValue
        InsertICSRCommand.Parameters.Add("@ICSRStatus_ID", SqlDbType.Int, 32)
        InsertICSRCommand.Parameters("@ICSRStatus_ID").Value = ICSRStatus_ID 'uses ICSRStatus where IsStatusNew = True
        Try
            Connection.Open()
            InsertICSRCommand.ExecuteNonQuery()
            Connection.Close()
        Catch ex As Exception
            Connection.Close()
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        End Try
        'Add case history entry with database updates
        Dim NewICSRReadCommand As New SqlCommand("SELECT TOP 1 ICSRs.ID AS ICSR_ID, ICSRStatuses.Name AS ICSRStatus_Name FROM ICSRs INNER JOIN ICSRStatuses ON ICSRs.ICSRStatus_ID = ICSRStatuses.ID ORDER BY ICSR_ID DESC", Connection)
        Dim NewICSR_ICSRID As Integer = Nothing
        Dim NewICSRStatus_Name As String = String.Empty
        Try
            Connection.Open()
            Dim NewICSRReader As SqlDataReader = NewICSRReadCommand.ExecuteReader()
            While NewICSRReader.Read()
                NewICSR_ICSRID = NewICSRReader.GetInt32(0)
                NewICSRStatus_Name = NewICSRReader.GetString(1)
            End While
            Connection.Close()
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
        Dim EntryString As String = String.Empty
        EntryString = HistoryDatabasebUpdateIntro
        EntryString += "New Individual Case Safety Report created.</br>"
        EntryString += "<b>ICSR ID</b> set to '" & NewICSR_ICSRID & "'</br>"
        EntryString += "<b>ICSR Status</b> set to '" & NewICSRStatus_Name & "' by default</br>"
        EntryString += "<b>All other values</b> set to 'Nothing' by default</br>"
        EntryString += HistoryDatabasebUpdateOutro
        Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO ICSRHistories(ICSR_ID, User_ID, Timepoint, Entry) VALUES (@ICSR_ID, @User_ID, @Timepoint, @Entry)", Connection)
        InsertHistoryEntryCommand.Parameters.Add("@ICSR_ID", SqlDbType.Int, 32)
        InsertHistoryEntryCommand.Parameters("@ICSR_ID").Value = NewICSR_ICSRID
        InsertHistoryEntryCommand.Parameters.Add("@User_ID", SqlDbType.Int, 32)
        InsertHistoryEntryCommand.Parameters("@User_ID").Value = LoggedIn_User_ID
        InsertHistoryEntryCommand.Parameters.Add("@Timepoint", SqlDbType.DateTime, 8)
        InsertHistoryEntryCommand.Parameters("@Timepoint").Value = Now()
        InsertHistoryEntryCommand.Parameters.Add("@Entry", SqlDbType.NVarChar, -1)
        InsertHistoryEntryCommand.Parameters("@Entry").Value = EntryString
        Try
            Connection.Open()
            InsertHistoryEntryCommand.ExecuteNonQuery()
            Connection.Close()
        Catch ex As Exception
            Connection.Close()
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        End Try
        'Write NewICSR_ID to CurrentICSRID_Hiddenfield
        ICSRID_HiddenField.Value = NewICSR_ICSRID
        'Format Page Controls
        AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
        ConfirmICSRInput_Button.Visible = False
        Company_Row.Visible = True
        Companies_DropDownList.Enabled = False
        Companies_DropDownList.ToolTip = String.Empty
        Companies_DropDownList.CssClass = "form-control alert-success"
    End Sub
End Class
