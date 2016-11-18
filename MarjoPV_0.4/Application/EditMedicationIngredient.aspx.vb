Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditMedicationIngredient
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            MedicationIngredientID_HiddenField.Value = Request.QueryString("MedicationIngredientID")
            Dim CurrentMedicationIngredient_ID As Integer = MedicationIngredientID_HiddenField.Value
            Delete_HiddenField.Value = Request.QueryString("Delete")
            Dim Delete As Boolean = False
            If Delete_HiddenField.Value = "True" Then
                Delete = True
            End If
            Dim CurrentMedication_ID As Integer = ParentID(tables.Medications, tables.MedicationIngredients, fields.Medication_ID, CurrentMedicationIngredient_ID)
            MedicationID_HiddenField.Value = CurrentMedication_ID
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                If Delete = False Then
                    Title_Label.Text = "Medication " & CurrentMedication_ID & ": Edit Medication Ingredient " & CurrentMedicationIngredient_ID & " Details"
                ElseIf Delete = True Then
                    Title_Label.Text = "Medication " & CurrentMedication_ID & ": Delete Medication Ingredient " & CurrentMedicationIngredient_ID & " Details"
                End If
                If Delete = False Then
                    If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationIngredients, fields.Edit) = True Then
                        AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToMedicationOverview_Button)
                        If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationIngredients, fields.ActiveIngredient_ID) = True Then
                            DropDownListEnabled(ActiveIngredients_DropDownList)
                        Else
                            DropDownListDisabled(ActiveIngredients_DropDownList)
                            ActiveIngredients_DropDownList.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationIngredients, fields.Quantity) = True Then
                            TextBoxReadWrite(Quantity_Textbox)
                            Quantity_Textbox.ToolTip = "Please enter a numeric value (e.g. 500)"
                        Else
                            TextBoxReadOnly(Quantity_Textbox)
                            Quantity_Textbox.ToolTip = CannotEditControlText
                        End If
                        If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationIngredients, fields.Unit_ID) = True Then
                            DropDownListEnabled(Units_DropDownList)
                        Else
                            DropDownListDisabled(Units_DropDownList)
                            Units_DropDownList.ToolTip = CannotEditControlText
                        End If
                    Else 'Lock out user if he/she does not have edit rights
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
                ElseIf Delete = True Then
                    If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationIngredients, fields.Delete) = True Then
                        AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToMedicationOverview_Button)
                        DropDownListDisabled(ActiveIngredients_DropDownList)
                        TextBoxReadOnly(Quantity_Textbox)
                        DropDownListDisabled(Units_DropDownList)
                    Else 'Lock out user if he/she does not have edit rights
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
                End If
                'Populate ActiveIngredients_DropDownList
                ActiveIngredients_DropDownList.DataSource = CreateDropDownListDatatable(tables.ActiveIngredients)
                ActiveIngredients_DropDownList.DataValueField = "ID"
                ActiveIngredients_DropDownList.DataTextField = "Name"
                ActiveIngredients_DropDownList.DataBind()
                'Populate Units_DropDownList
                Units_DropDownList.DataSource = CreateDropDownListDatatable(tables.Units)
                Units_DropDownList.DataValueField = "ID"
                Units_DropDownList.DataTextField = "Name"
                Units_DropDownList.DataBind()
                Dim AtEditPageLoadReadCommand As New SqlCommand("SELECT CASE WHEN ActiveIngredient_ID IS NULL THEN 0 ELSE ActiveIngredient_ID END AS ActiveIngredient_ID, CASE WHEN Quantity IS NULL THEN 0 ELSE Quantity END AS Quantity, CASE WHEN Unit_ID IS NULL THEN 0 ELSE Unit_ID END AS Unit_ID FROM MedicationIngredients WHERE ID = @CurrentMedicationIngredient_ID", Connection)
                AtEditPageLoadReadCommand.Parameters.AddWithValue("@CurrentMedicationIngredient_ID", CurrentMedicationIngredient_ID)
                Try
                    Connection.Open()
                    Dim AtEditPageLoadReader As SqlDataReader = AtEditPageLoadReadCommand.ExecuteReader()
                    While AtEditPageLoadReader.Read()
                        ActiveIngredients_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(0)
                        AtEditPageLoad_ActiveIngredientID_HiddenField.Value = AtEditPageLoadReader.GetInt32(0)
                        Quantity_Textbox.Text = AtEditPageLoadReader.GetInt32(1)
                        AtEditPageLoad_Quantity_HiddenField.Value = AtEditPageLoadReader.GetInt32(1)
                        Units_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(2)
                        AtEditPageLoad_UnitID_HiddenField.Value = AtEditPageLoadReader.GetInt32(2)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            Else 'Redirect user to login if he/she is not logged in
                If Delete = False Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditMedicationIngredient.aspx?MedicationIngredientID=" & CurrentMedicationIngredient_ID)
                ElseIf Delete = True Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditMedicationIngredient.aspx?MedicationIngredientID=" & CurrentMedicationIngredient_ID & "&Delete=True")
                End If
            End If
        End If
    End Sub

    Protected Sub ReturnToMedicationOverview() Handles Cancel_Button.Click, ReturnToMedicationOverview_Button.Click
        Dim CurrentMedication_ID = MedicationID_HiddenField.Value
        Response.Redirect("~/Application/MedicationOverview.aspx?MedicationID=" & CurrentMedication_ID)
    End Sub

    Protected Sub ActiveIngredients_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If ActiveIngredients_DropDownList.SelectedValue <> 0 Then
            ActiveIngredients_DropDownList.CssClass = "form-control alert-success"
            ActiveIngredients_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            ActiveIngredients_DropDownList.CssClass = "form-control alert-danger"
            ActiveIngredients_DropDownList.ToolTip = "Please ensure you are selecting a valid entry"
            args.IsValid = True
        End If
    End Sub

    Protected Sub Quantity_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        Dim Result As Integer = Nothing
        Try
            Result = CInt(Quantity_Textbox.Text.Trim)
            Quantity_Textbox.CssClass = "form-control alert-success"
            Quantity_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Catch ex As Exception
            Quantity_Textbox.CssClass = "form-control alert-danger"
            Quantity_Textbox.ToolTip = "Please ensure you are entering a valid numeric value (e.g. 500)"
            args.IsValid = False
        End Try
    End Sub

    Protected Sub Units_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Units_DropDownList.SelectedValue <> 0 Then
            Units_DropDownList.CssClass = "form-control alert-success"
            Units_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            Units_DropDownList.CssClass = "form-control alert-danger"
            Units_DropDownList.ToolTip = "Please ensure you are selecting a valid entry"
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click, ConfirmDeletion_Button.Click
        If Page.IsValid = True Then
            Dim CurrentMedicationIngredient_ID As Integer = MedicationIngredientID_HiddenField.Value
            Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
            'Retrieve values as present in database at edit page load and store in variables to use when checking for database update conflicts (see page load event)
            Dim AtEditPageLoad_ActiveIngredient_ID As Integer = AtEditPageLoad_ActiveIngredientID_HiddenField.Value
            Dim AtEditPageLoad_Quantity As Integer = AtEditPageLoad_Quantity_HiddenField.Value
            Dim AtEditPageLoad_Unit_ID As Integer = AtEditPageLoad_UnitID_HiddenField.Value
            'Store report values as present in database when save button is clicked in variables
            Dim AtSaveButtonClickReadCommand As New SqlCommand("SELECT CASE WHEN ActiveIngredient_ID IS NULL THEN 0 ELSE ActiveIngredient_ID END AS ActiveIngredient_ID, CASE WHEN Quantity IS NULL THEN 0 ELSE Quantity END AS Quantity, CASE WHEN Unit_ID IS NULL THEN 0 ELSE Unit_ID END AS Unit_ID FROM MedicationIngredients WHERE ID = @CurrentMedicationIngredient_ID", Connection)
            AtSaveButtonClickReadCommand.Parameters.AddWithValue("@CurrentMedicationIngredient_ID", CurrentMedicationIngredient_ID)
            Dim AtSaveButtonClick_ActiveIngredient_ID As Integer = Nothing
            Dim AtSaveButtonClick_Quantity As Integer = Nothing
            Dim AtSaveButtonClick_Unit_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim AtSaveButtonClickReader As SqlDataReader = AtSaveButtonClickReadCommand.ExecuteReader()
                While AtSaveButtonClickReader.Read()
                    AtSaveButtonClick_ActiveIngredient_ID = AtSaveButtonClickReader.GetInt32(0)
                    AtSaveButtonClick_Quantity = AtSaveButtonClickReader.GetInt32(1)
                    AtSaveButtonClick_Unit_ID = AtSaveButtonClickReader.GetInt32(2)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Check for discrepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked
            'Dim DiscrepancyString As String = String.Empty
            'DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_ActiveIngredient_ID, AtSaveButtonClick_ActiveIngredient_ID, "Ingredient Name")
            'DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Quantity, AtSaveButtonClick_Quantity, "Quantity")
            'DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Unit_ID, AtSaveButtonClick_Unit_ID, "Unit")
            'If Discprepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked are found, show warning and abort update
            'If DiscrepancyString <> String.Empty Then
            '    AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToMedicationOverview_Button)
            '    Status_Label.Style.Add("text-align", "left")
            '    Status_Label.Style.Add("height", "auto")
            '    Status_Label.Text = DiscrepancyStringIntro & DiscrepancyString & DiscrepancyStringOutro
            '    Status_Label.CssClass = "form-control alert-danger"
            '    DropDownListDisabled(ActiveIngredients_DropDownList)
            '    TextBoxReadOnly(Quantity_Textbox)
            '    DropDownListDisabled(Units_DropDownList)
            '    Exit Sub
            'End If
            'If no discrepancies were found between database contents as present when edit page was loaded and database contents as present when save button is clicked, write updates to database
            Dim UpdateCommand As New SqlCommand
            UpdateCommand.Connection = Connection
            If sender Is SaveUpdates_Button Then
                UpdateCommand.CommandText = "UPDATE MedicationIngredients SET ActiveIngredient_ID = (CASE WHEN @ActiveIngredient_ID = 0 THEN NULL ELSE @ActiveIngredient_ID END), Quantity = (CASE WHEN @Quantity = 0 THEN NULL ELSE @Quantity END), Unit_ID = (CASE WHEN @Unit_ID = 0 THEN NULL ELSE @Unit_ID END) WHERE ID = @CurrentMedicationIngredient_ID"
                UpdateCommand.Parameters.AddWithValue("@ActiveIngredient_ID", ActiveIngredients_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@Quantity", Quantity_Textbox.Text.Trim)
                UpdateCommand.Parameters.AddWithValue("@Unit_ID", Units_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@CurrentMedicationIngredient_ID", CurrentMedicationIngredient_ID)
            ElseIf sender Is ConfirmDeletion_Button Then
                UpdateCommand.CommandText = "DELETE FROM MedicationIngredients WHERE ID = @CurrentMedicationIngredient_ID"
                UpdateCommand.Parameters.AddWithValue("@CurrentMedicationIngredient_ID", CurrentMedicationIngredient_ID)
            End If
            Try
                Connection.Open()
                UpdateCommand.ExecuteNonQuery()
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Read updated values from database and store in variables
            Dim Updated_ActiveIngredient_ID As Integer = Nothing
            Dim Updated_Quantity As Integer = Nothing
            Dim Updated_Unit_ID As Integer = Nothing
            If sender Is SaveUpdates_Button Then
                Dim UpdatedReadCommand As New SqlCommand("SELECT CASE WHEN ActiveIngredient_ID IS NULL THEN 0 ELSE ActiveIngredient_ID END AS ActiveIngredient_ID, CASE WHEN Quantity IS NULL THEN 0 ELSE Quantity END AS Quantity, CASE WHEN Unit_ID IS NULL THEN 0 ELSE Unit_ID END AS Unit_ID FROM MedicationIngredients WHERE ID = @CurrentMedicationIngredient_ID", Connection)
                UpdatedReadCommand.Parameters.AddWithValue("@CurrentMedicationIngredient_ID", CurrentMedicationIngredient_ID)
                Try
                    Connection.Open()
                    Dim UpdatedReader As SqlDataReader = UpdatedReadCommand.ExecuteReader()
                    While UpdatedReader.Read()
                        Updated_ActiveIngredient_ID = UpdatedReader.GetInt32(0)
                        Updated_Quantity = UpdatedReader.GetInt32(1)
                        Updated_Unit_ID = UpdatedReader.GetInt32(2)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            End If
            'Compare old and new variables to generate EntryString for Change History Entry
            'Dim EntryString As String = String.Empty
            'EntryString = HistoryDatabasebUpdateIntro
            'If sender Is ConfirmDeletion_Button Then
            '    EntryString += DeleteReportIntro("Medication Ingredient", CurrentMedicationIngredient_ID)
            'End If
            'EntryString += HistoryEntryReferencedValue("Medication Ingredient", CurrentMedicationIngredient_ID, "Type", tables.ActiveIngredients, fields.Name, AtSaveButtonClick_ActiveIngredient_ID, Updated_ActiveIngredient_ID)
            'EntryString += HistoryEntryPlainValue("Medication Ingredient", CurrentMedicationIngredient_ID, "Quantity", AtSaveButtonClick_Quantity, Updated_Quantity)
            'EntryString += HistoryEntryReferencedValue("Medication Ingredient", CurrentMedicationIngredient_ID, "Unit", tables.Units, fields.Name, AtSaveButtonClick_Unit_ID, Updated_Unit_ID)
            'EntryString += HistoryDatabasebUpdateOutro
            ''Generate History Entry if any data was changed in the database
            'If EntryString <> HistoryDatabasebUpdateIntro & HistoryDatabasebUpdateOutro Then
            '    Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO MedicationHistories (Medication_ID, User_ID, Timepoint, Entry) VALUES (@Medication_ID, @User_ID, @Timepoint, CASE WHEN @Entry = '' THEN NULL ELSE @Entry END)", Connection)
            '    InsertHistoryEntryCommand.Parameters.AddWithValue("@Medication_ID", CurrentMedication_ID)
            '    InsertHistoryEntryCommand.Parameters.AddWithValue("@User_ID", LoggedIn_User_ID)
            '    InsertHistoryEntryCommand.Parameters.AddWithValue("@Timepoint", Now())
            '    InsertHistoryEntryCommand.Parameters.AddWithValue("@Entry", EntryString)
            '    Try
            '        Connection.Open()
            '        InsertHistoryEntryCommand.ExecuteNonQuery()
            '    Catch ex As Exception
            '        Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            '        Exit Sub
            '    Finally
            '        Connection.Close()
            '    End Try
            'End If
            'Format Controls
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToMedicationOverview_Button)
            If sender Is SaveUpdates_Button Then
                DropDownListDisabled(ActiveIngredients_DropDownList)
                TextBoxReadOnly(Quantity_Textbox)
                DropDownListDisabled(Units_DropDownList)
            ElseIf sender Is ConfirmDeletion_Button Then
                Name_Row.Visible = False
                Quantity_Row.Visible = False
                Unit_Row.Visible = False
            End If
        End If
    End Sub

End Class
