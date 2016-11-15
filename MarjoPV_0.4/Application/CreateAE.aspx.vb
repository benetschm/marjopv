Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_CreateAE
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Add Event"
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                'Check if user has create rights and lock out if not
                If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.Create) = True Then
                    'Format Controls depending on user edit rights for each control
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.MedDRATerm) = True Then
                        TextBoxReadWrite(MedDRATerm_Textbox)
                        MedDRATerm_Textbox.ToolTip = "Please enter a valid MedDRA Low Level Term"
                    Else
                        TextBoxReadOnly(MedDRATerm_Textbox)
                        MedDRATerm_Textbox.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.Start) = True Then
                        TextBoxReadWrite(Start_Textbox)
                        Start_Textbox.ToolTip = DateInputToolTip
                    Else
                        TextBoxReadOnly(Start_Textbox)
                        Start_Textbox.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.Stop) = True Then
                        TextBoxReadWrite(Stop_Textbox)
                        Stop_Textbox.ToolTip = DateInputToolTip
                    Else
                        TextBoxReadOnly(Stop_Textbox)
                        Stop_Textbox.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.Outcome_ID) = True Then
                        DropDownListEnabled(Outcomes_DropDownList)
                    Else
                        DropDownListDisabled(Outcomes_DropDownList)
                        Outcomes_DropDownList.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.DechallengeResult_ID) = True Then
                        DropDownListEnabled(DechallengeResults_DropDownList)
                    Else
                        DropDownListDisabled(DechallengeResults_DropDownList)
                        DechallengeResults_DropDownList.ToolTip = CannotEditControlText
                    End If
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.AEs, fields.RechallengeResult_ID) = True Then
                        DropDownListEnabled(RechallengeResults_DropDownList)
                    Else
                        DropDownListDisabled(RechallengeResults_DropDownList)
                        RechallengeResults_DropDownList.ToolTip = CannotEditControlText
                    End If
                    'Populate Outcomes_DropDownList
                    Outcomes_DropDownList.DataSource = CreateDropDownListDatatable(tables.Outcomes)
                    Outcomes_DropDownList.DataValueField = "ID"
                    Outcomes_DropDownList.DataTextField = "Name"
                    Outcomes_DropDownList.DataBind()
                    'Populate DechallengeResults_DropDownList
                    DechallengeResults_DropDownList.DataSource = CreateDropDownListDatatable(tables.DechallengeResults)
                    DechallengeResults_DropDownList.DataValueField = "ID"
                    DechallengeResults_DropDownList.DataTextField = "Name"
                    DechallengeResults_DropDownList.DataBind()
                    'Populate RechallengeResults_DropDownList
                    RechallengeResults_DropDownList.DataSource = CreateDropDownListDatatable(tables.RechallengeResults)
                    RechallengeResults_DropDownList.DataValueField = "ID"
                    RechallengeResults_DropDownList.DataTextField = "Name"
                    RechallengeResults_DropDownList.DataBind()
                Else 'Lock out user if he/she does not have create rights
                    Title_Label.Text = Lockout_Text
                    ButtonGroup_Div.Visible = False
                    Main_Table.Visible = False
                End If
            Else 'Redirect user to login if he/she is not logged in
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/CreateMarketingCountry.aspx?MedicationID=" & CurrentICSR_ID)
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click() Handles Cancel_Button.Click, ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub MedDRATerm_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If TextValidator(MedDRATerm_Textbox) = True Then
            MedDRATerm_Textbox.CssClass = CssClassSuccess
            MedDRATerm_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            MedDRATerm_Textbox.CssClass = CssClassFailure
            MedDRATerm_Textbox.ToolTip = MedDRATermValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Start_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If DateOrEmptyValidator(Start_Textbox) = True Then
            Start_Textbox.CssClass = CssClassSuccess
            Start_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Start_Textbox.CssClass = CssClassFailure
            Start_Textbox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Stop_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If DateOrEmptyValidator(Stop_Textbox) = True Then
            Stop_Textbox.CssClass = CssClassSuccess
            Stop_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Stop_Textbox.CssClass = CssClassFailure
            Stop_Textbox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Outcomes_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(Outcomes_DropDownList) = True Then
            Outcomes_DropDownList.CssClass = CssClassSuccess
            Outcomes_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            Outcomes_DropDownList.CssClass = CssClassFailure
            Outcomes_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub DechallengeResults_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(DechallengeResults_DropDownList) = True Then
            DechallengeResults_DropDownList.CssClass = CssClassSuccess
            DechallengeResults_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            DechallengeResults_DropDownList.CssClass = CssClassFailure
            DechallengeResults_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub RechallengeResults_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If NoValidator(RechallengeResults_DropDownList) = True Then
            RechallengeResults_DropDownList.CssClass = CssClassSuccess
            RechallengeResults_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            RechallengeResults_DropDownList.CssClass = CssClassFailure
            RechallengeResults_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
            'Write New dataset to database
            Dim InsertCommand As New SqlCommand("INSERT INTO AEs (ICSR_ID, MedDRATerm, Start, Stop, Outcome_ID, DechallengeResult_ID, RechallengeResult_ID) VALUES(@CurrentICSR_ID, @MedDRATerm, CASE WHEN @Start = 0 THEN NULL ELSE @Start END, CASE WHEN @Stop = 0 THEN NULL ELSE @Stop END, CASE WHEN @Outcome_ID = 0 THEN NULL ELSE @Outcome_ID END, CASE WHEN @DechallengeResult_ID = 0 THEN NULL ELSE @DechallengeResult_ID END, CASE WHEN @RechallengeResult_ID = 0 THEN NULL ELSE @RechallengeResult_ID END)", Connection)
            InsertCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            InsertCommand.Parameters.AddWithValue("@MedDRATerm", MedDRATerm_Textbox.Text.Trim)
            InsertCommand.Parameters.AddWithValue("@Start", DateStringOrEmpty(Start_Textbox.Text.Trim))
            InsertCommand.Parameters.AddWithValue("@Stop", DateStringOrEmpty(Stop_Textbox.Text.Trim))
            InsertCommand.Parameters.AddWithValue("@Outcome_ID", Outcomes_DropDownList.SelectedValue)
            InsertCommand.Parameters.AddWithValue("@DechallengeResult_ID", DechallengeResults_DropDownList.SelectedValue)
            InsertCommand.Parameters.AddWithValue("@RechallengeResult_ID", RechallengeResults_DropDownList.SelectedValue)
            Try
                Connection.Open()
                InsertCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Add change history entry with database updates
            Dim NewReadCommand As New SqlCommand("SELECT TOP 1 ID, CASE WHEN MedDRATerm IS NULL THEN '' ELSE MedDRATerm END AS MedDRATerm, CASE WHEN Start IS NULL THEN 0 ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN 0 ELSE Stop END AS Stop, CASE WHEN Outcome_ID IS NULL THEN 0 ELSE Outcome_ID END AS Outcome_ID, CASE WHEN DechallengeResult_ID IS NULL THEN 0 ELSE DechallengeResult_ID END AS DechallengeResult_ID, CASE WHEN RechallengeResult_ID IS NULL THEN 0 ELSE RechallengeResult_ID END AS RechallengeResult_ID FROM AEs WHERE ICSR_ID = @CurrentICSR_ID ORDER BY ID DESC", Connection)
            NewReadCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            Dim NewAE_ID As Integer = Nothing
            Dim NewMedDRATerm As String = String.Empty
            Dim NewStart As DateTime = Date.MinValue
            Dim NewStop As DateTime = Date.MinValue
            Dim NewOutcome_ID As Integer = Nothing
            Dim NewDechallengeResult_ID As Integer = Nothing
            Dim NewRechallengeResult_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim NewReader As SqlDataReader = NewReadCommand.ExecuteReader()
                While NewReader.Read()
                    NewAE_ID = NewReader.GetInt32(0)
                    NewMedDRATerm = NewReader.GetString(1)
                    NewStart = DateOrDateMinValue(NewReader.GetDateTime(2))
                    NewStop = DateOrDateMinValue(NewReader.GetDateTime(3))
                    NewOutcome_ID = NewReader.GetInt32(4)
                    NewDechallengeResult_ID = NewReader.GetInt32(5)
                    NewRechallengeResult_ID = NewReader.GetInt32(6)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            EntryString += NewReportIntro("Event", NewAE_ID)
            EntryString += HistoryEnrtyPlainValue("Event", NewAE_ID, "MedDRA LLT", String.Empty, NewMedDRATerm)
            EntryString += HistoryEnrtyPlainValue("Event", NewAE_ID, "Start Date", Date.MinValue, NewStart)
            EntryString += HistoryEnrtyPlainValue("Event", NewAE_ID, "Stop Date", Date.MinValue, NewStop)
            EntryString += HistoryEntryReferencedValue("Event", NewAE_ID, "Outcome", tables.Outcomes, fields.Name, Nothing, NewOutcome_ID)
            EntryString += HistoryEntryReferencedValue("Event", NewAE_ID, "Dechallenge Result", tables.DechallengeResults, fields.Name, Nothing, NewDechallengeResult_ID)
            EntryString += HistoryEntryReferencedValue("Event", NewAE_ID, "Rechallenge Result", tables.RechallengeResults, fields.Name, Nothing, NewRechallengeResult_ID)
            EntryString += HistoryDatabasebUpdateOutro
            Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO ICSRHistories(ICSR_ID, User_ID, Timepoint, Entry) VALUES (@ICSR_ID, @User_ID, @Timepoint, @Entry)", Connection)
            InsertHistoryEntryCommand.Parameters.AddWithValue("@ICSR_ID", CurrentICSR_ID)
            InsertHistoryEntryCommand.Parameters.AddWithValue("@User_ID", LoggedIn_User_ID)
            InsertHistoryEntryCommand.Parameters.AddWithValue("@Timepoint", Now())
            InsertHistoryEntryCommand.Parameters.AddWithValue("@Entry", EntryString)
            Try
                Connection.Open()
                InsertHistoryEntryCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Format Page Controls
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToICSROverview_Button)
            TextBoxReadOnly(MedDRATerm_Textbox)
            TextBoxReadOnly(Start_Textbox)
            TextBoxReadOnly(Stop_Textbox)
            DropDownListDisabled(Outcomes_DropDownList)
            DropDownListDisabled(DechallengeResults_DropDownList)
            DropDownListDisabled(RechallengeResults_DropDownList)
        End If
    End Sub

End Class
