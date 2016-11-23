Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditMedicalHistory
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        'Carry out Sub only on initial page load
        If Page.IsPostBack = True Then Exit Sub

        'Redirect to login if user is not logged in
        If Login_Status = False Then
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditMedicalHistory.aspx?" & Convert.ToString(Request.QueryString))
        End If

        'Determine call reason, store reason in hidden field and redirect to parent page if query string is invalid
        Dim CallReason As CallReasons = GetCallReason(Me, "MedicalHistoryID", CallReason_HiddenField)

        'Store query string values in hidden fields and variables
        Dim CurrentICSR_ID As Integer = Nothing
        Dim CurrentMedicalHistory_ID As Integer = Nothing
        Dim Delete As Boolean = False
        If CallReason = CallReasons.Create Then
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            MedicalHistoryID_HiddenField.Value = Request.QueryString("MedicalHistoryID")
            CurrentMedicalHistory_ID = MedicalHistoryID_HiddenField.Value
            ICSRID_HiddenField.Value = ParentID(tables.ICSRs, tables.MedicalHistories, fields.ICSR_ID, CurrentMedicalHistory_ID)
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Delete Then
            Delete_HiddenField.Value = Request.QueryString("Delete")
        End If

        'Populate Title_Label
        If CallReason = CallReasons.Create Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Add Medical History Entry"
        ElseIf CallReason = CallReasons.Update Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Edit Medical History Entry " & CurrentMedicalHistory_ID
        ElseIf CallReason = CallReasons.Delete Then
            Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Delete Medical History Entry " & CurrentMedicalHistory_ID
        End If

        'Lock out if user does not have adequate edit rights
        LockoutCheck(CallReason, CurrentICSR_ID, tables.MedicalHistories, Title_Label, ButtonGroup_Div, Main_Table)

        'Format controls based on edit rights
        If CallReason = CallReasons.Create Or CallReason = CallReasons.Update Then
            AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
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
        ElseIf CallReason = CallReasons.Delete Then
            AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
            TextBoxReadOnly(MedDRATerm_Textbox)
            TextBoxReadOnly(Start_Textbox)
            TextBoxReadOnly(Stop_Textbox)
        End If

        'Populate DropDownLists
        'Not applicable for this page

        'Populate data fields
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            Dim CurrentValuesDataTable As DataTable = SqlRead(Me, "SELECT ID, MedDRATerm, CASE WHEN Start IS NULL THEN '' ELSE Start END AS Start, CASE WHEN Stop IS NULL THEN '' ELSE Stop END AS Stop FROM MedicalHistories WHERE ID = " & CurrentMedicalHistory_ID)
            MedDRATerm_Textbox.Text = CurrentValuesDataTable.Rows(0)(1)
            AtEditPageLoad_MedDRATerm_HiddenField.Value = CurrentValuesDataTable.Rows(0)(1)
            Start_Textbox.Text = SqlDateDisplay(CurrentValuesDataTable.Rows(0)(2))
            AtEditPageLoad_Start_HiddenField.Value = CurrentValuesDataTable.Rows(0)(2)
            Stop_Textbox.Text = SqlDateDisplay(CurrentValuesDataTable.Rows(0)(3))
            AtEditPageLoad_Stop_HiddenField.Value = CurrentValuesDataTable.Rows(0)(3)
        End If

    End Sub

    Protected Sub ReturnToICSROverview() Handles Cancel_Button.Click, ReturnToICSROverview_Button.Click
        Dim CurrentICSR_ID As Integer = ICSRID_HiddenField.Value
        Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & CurrentICSR_ID)
    End Sub

    Protected Sub MedDRATerm_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If MedDRATerm_Textbox.Text.Trim <> String.Empty Then
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
        If Validation(Start_Textbox.Text, InputTypes.Date) = True Then
            Start_Textbox.CssClass = CssClassSuccess
            Start_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf Start_Textbox.Text = String.Empty Then
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
        If Validation(Stop_Textbox.Text, InputTypes.Date) = True Then
            Stop_Textbox.CssClass = CssClassSuccess
            Stop_Textbox.ToolTip = String.Empty
            args.IsValid = True
        ElseIf Stop_Textbox.Text = String.Empty Then
            Stop_Textbox.CssClass = CssClassSuccess
            Stop_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Stop_Textbox.CssClass = CssClassFailure
            Stop_Textbox.ToolTip = DateValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub StartStop_Textbox_Consistency_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Validation(Stop_Textbox.Text, InputTypes.Date) = True And Validation(Stop_Textbox.Text, InputTypes.Date) = True And DateOrDateMinValue(Stop_Textbox.Text) < DateOrDateMinValue(Start_Textbox.Text) Then
            Start_Textbox.CssClass = CssClassFailure
            Start_Textbox.ToolTip = DateInconsistencyValidationFailToolTip
            Stop_Textbox.CssClass = CssClassFailure
            Stop_Textbox.ToolTip = DateInconsistencyValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click, ConfirmDeletion_Button.Click

        'Carry out Sub only if input is valid
        If Page.IsValid = False Then Exit Sub

        'Redirect to login if user is not logged in
        If Login_Status = False Then
            Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditMedicalHistory.aspx?" & Convert.ToString(Request.QueryString))
        End If

        'Store values from hidden fields in variables
        Dim CallReason As CallReasons
        Dim CurrentICSR_ID As Integer = Nothing
        Dim CurrentMedicalHistory_ID As Integer = Nothing
        Dim Delete As Boolean = False
        CallReason = CallReason_HiddenField.Value
        If CallReason = CallReasons.Create Then
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            CurrentMedicalHistory_ID = MedicalHistoryID_HiddenField.Value
            CurrentICSR_ID = ICSRID_HiddenField.Value
        End If
        If CallReason = CallReasons.Delete Then
            Delete = Delete_HiddenField.Value
        End If

        'Lock out if user does not have adequate edit rights
        LockoutCheck(CallReason, CurrentICSR_ID, tables.MedicalHistories, Title_Label, ButtonGroup_Div, Main_Table)

        'Format Controls
        AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToICSROverview_Button)
        If CallReason = CallReasons.Create Or CallReason = CallReasons.Update Then
            TextBoxReadOnly(MedDRATerm_Textbox)
            TextBoxReadOnly(Start_Textbox)
            TextBoxReadOnly(Stop_Textbox)
        ElseIf CallReason = CallReasons.Delete Then
            MedDRATerm_Row.Visible = False
            Start_Row.Visible = False
            Stop_Row.Visible = False
        End If

        'Warn & abort if there are discrepancies between the data as shown at edit page load and as stored at save button click
        If CallReason = CallReasons.Update Or CallReason = CallReasons.Delete Then
            Dim DiscrepancyString As String = String.Empty
            DiscrepancyString += DiscrepancyCheck(tables.MedicalHistories, fields.MedDRATerm, InputTypes.String, CurrentMedicalHistory_ID, AtEditPageLoad_MedDRATerm_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicalHistories, fields.Start, InputTypes.Date, CurrentMedicalHistory_ID, AtEditPageLoad_Start_HiddenField)
            DiscrepancyString += DiscrepancyCheck(tables.MedicalHistories, fields.Stop, InputTypes.Date, CurrentMedicalHistory_ID, AtEditPageLoad_Stop_HiddenField)
            If DiscrepancyString <> String.Empty Then
                ShowDiscrepancyWarning(Status_Label, DiscrepancyString)
                Exit Sub
            End If
        End If

        'Store updates in database
        Dim UpdateCommand As New SqlCommand
        UpdateCommand.Connection = Connection
        If CallReason = CallReasons.Create Then
            UpdateCommand.CommandText = "INSERT INTO MedicalHistories (ICSR_ID, MedDRATerm, Start, Stop) VALUES(@CurrentICSR_ID, @MedDRATerm, CASE WHEN @Start = '' THEN NULL ELSE @Start END, CASE WHEN @Stop = '' THEN NULL ELSE @Stop END)"
            UpdateCommand.Parameters.AddWithValue("@CurrentICSR_ID", CurrentICSR_ID)
            UpdateCommand.Parameters.AddWithValue("@MedDRATerm", MedDRATerm_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@Start", DateStringOrEmpty(Start_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@Stop", DateStringOrEmpty(Stop_Textbox.Text.Trim))
        ElseIf CallReason = CallReasons.Update Then
            UpdateCommand.CommandText = "UPDATE MedicalHistories SET MedDRATerm = @MedDRATerm, Start = (CASE WHEN @Start = '' THEN NULL ELSE @Start END), Stop = (CASE WHEN @Stop = '' THEN NULL ELSE @Stop END) WHERE ID = @CurrentMedicalHistory_ID"
            UpdateCommand.Parameters.AddWithValue("@MedDRATerm", MedDRATerm_Textbox.Text.Trim)
            UpdateCommand.Parameters.AddWithValue("@Start", (Start_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@Stop", DateStringOrEmpty(Stop_Textbox.Text.Trim))
            UpdateCommand.Parameters.AddWithValue("@CurrentMedicalHistory_ID", CurrentMedicalHistory_ID)
        ElseIf CallReason = CallReasons.Delete Then
            UpdateCommand.CommandText = "DELETE FROM MedicalHistories WHERE ID = @CurrentMedicalHistory_ID"
            UpdateCommand.Parameters.AddWithValue("@CurrentMedicalHistory_ID", CurrentMedicalHistory_ID)
        End If
        SqlUpdate(Me, UpdateCommand)

        'Add audit trail entry
        Dim EntryString As String = HistoryDatabasebUpdateIntro
        If CallReason = CallReasons.Create Then
            Dim NewMedicalHistory_ID As DataTable = SqlRead(Me, "SELECT TOP 1 ID FROM MedicalHistories WHERE ICSR_ID = " & TryCType(CurrentICSR_ID, InputTypes.Integer) & " ORDER BY ID DESC")
            CurrentMedicalHistory_ID = NewMedicalHistory_ID.Rows(0)(0)
            EntryString += NewReportIntro("Medical History Entry", CurrentMedicalHistory_ID)
        ElseIf CallReason = CallReasons.Delete Then
            EntryString += DeleteReportIntro("Medical History Entry", CurrentMedicalHistory_ID)
        End If
        EntryString += HistoryEntryPlainValue(CallReason, "Medical History Entry", CurrentMedicalHistory_ID, "MedDRA LLT", AtEditPageLoad_MedDRATerm_HiddenField.Value, MedDRATerm_Textbox.Text.Trim)
        EntryString += HistoryEntryPlainValue(CallReason, "Medical History Entry", CurrentMedicalHistory_ID, "Start Date", TryCType(AtEditPageLoad_Start_HiddenField.Value, InputTypes.Date), TryCType(Start_Textbox.Text, InputTypes.Date))
        EntryString += HistoryEntryPlainValue(CallReason, "Medical History Entry", CurrentMedicalHistory_ID, "Stop Date", TryCType(AtEditPageLoad_Stop_HiddenField.Value, InputTypes.Date), TryCType(Stop_Textbox.Text, InputTypes.Date))
        SaveAuditTrailEntry(Me, CurrentICSR_ID, LoggedIn_User_ID, EntryString)

    End Sub

End Class
