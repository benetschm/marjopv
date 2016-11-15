Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_CreateMedicationIngredient
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            MedicationID_HiddenField.Value = Request.QueryString("MedicationID")
            Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
            Title_Label.Text = "Medication " & CurrentMedication_ID & ": Add Medication Ingredient"
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                'Check if user has create rights and lock out if not
                If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationIngredients, fields.Create) = True Then
                    'Format Controls depending on user edit rights for each control
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToMedicationOverview_Button)
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
                Else 'Lock out user if he/she does not have create rights
                    Title_Label.Text = Lockout_Text
                    ButtonGroup_Div.Visible = False
                    Main_Table.Visible = False
                End If
            Else 'Redirect user to login if he/she is not logged in
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/CreateMedicationIngredient.aspx?MedicationID=" & CurrentMedication_ID)
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click() Handles Cancel_Button.Click
        Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
        Response.Redirect("~/Application/MedicationOverview.aspx?MedicationID=" & CurrentMedication_ID)
    End Sub

    Protected Sub ReturnToMedicationOverview_Button_Click() Handles ReturnToMedicationOverview_Button.Click
        Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
        Response.Redirect("~/Application/MedicationOverview.aspx?MedicationID=" & CurrentMedication_ID)
    End Sub

    Protected Sub ActiveIngredients_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If SelectionValidator(ActiveIngredients_DropDownList) = True Then
            ActiveIngredients_DropDownList.CssClass = CssClassSuccess
            ActiveIngredients_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            ActiveIngredients_DropDownList.CssClass = CssClassFailure
            ActiveIngredients_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = True
        End If
    End Sub

    Protected Sub Quantity_Textbox_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If IntegerValidator(Quantity_Textbox) = True Then
            Quantity_Textbox.CssClass = CssClassSuccess
            Quantity_Textbox.ToolTip = String.Empty
            args.IsValid = True
        Else
            Quantity_Textbox.CssClass = CssClassFailure
            Quantity_Textbox.ToolTip = NumberValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub Units_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If SelectionValidator(Units_DropDownList) = True Then
            Units_DropDownList.CssClass = CssClassSuccess
            Units_DropDownList.ToolTip = String.Empty
            args.IsValid = True
        Else
            Units_DropDownList.CssClass = CssClassSuccess
            Units_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
            'Write New Medication Ingredient to database
            Dim InsertCommand As New SqlCommand("INSERT INTO MedicationIngredients (Medication_ID, ActiveIngredient_ID, Quantity, Unit_ID) VALUES(@CurrentMedication_ID, CASE WHEN @ActiveIngredient_ID = 0 THEN NULL ELSE @ActiveIngredient_ID END, CASE WHEN @Quantity = 0 THEN NULL ELSE @Quantity END, CASE WHEN @Unit_ID = 0 THEN NULL ELSE @Unit_ID END)", Connection)
            InsertCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
            InsertCommand.Parameters.AddWithValue("@ActiveIngredient_ID", ActiveIngredients_DropDownList.SelectedValue)
            InsertCommand.Parameters.AddWithValue("@Quantity", Quantity_Textbox.Text.Trim)
            InsertCommand.Parameters.AddWithValue("@Unit_ID", Units_DropDownList.SelectedValue)
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
            Dim NewReadCommand As New SqlCommand("SELECT TOP 1 ID, ActiveIngredient_ID, Quantity, Unit_ID FROM MedicationIngredients WHERE Medication_ID = @CurrentMedication_ID ORDER BY ID DESC", Connection)
            NewReadCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
            Dim NewActiveIngredientInMedication_ID As Integer = Nothing
            Dim NewActiveIngredient_ID As Integer = Nothing
            Dim NewQuantity As Integer = Nothing
            Dim NewUnit_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim NewReader As SqlDataReader = NewReadCommand.ExecuteReader()
                While NewReader.Read()
                    NewActiveIngredientInMedication_ID = NewReader.GetInt32(0)
                    NewActiveIngredient_ID = NewReader.GetInt32(1)
                    NewQuantity = NewReader.GetInt32(2)
                    NewUnit_ID = NewReader.GetInt32(3)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            EntryString += NewReportIntro("Medication Ingredient", NewActiveIngredientInMedication_ID)
            EntryString += HistoryEntryReferencedValue("Medication Ingredient", NewActiveIngredientInMedication_ID, "Name", tables.ActiveIngredients, fields.Name, Nothing, NewActiveIngredient_ID)
            EntryString += HistoryEnrtyPlainValue("Medication Ingredient", NewActiveIngredientInMedication_ID, "Quantity", Nothing, NewQuantity)
            EntryString += HistoryEntryReferencedValue("Medication Ingredient", NewActiveIngredientInMedication_ID, "Unit", tables.Units, fields.Name, Nothing, NewUnit_ID)
            EntryString += HistoryDatabasebUpdateOutro
            Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO MedicationHistories(Medication_ID, User_ID, Timepoint, Entry) VALUES (@Medication_ID, @User_ID, @Timepoint, @Entry)", Connection)
            InsertHistoryEntryCommand.Parameters.AddWithValue("@Medication_ID", CurrentMedication_ID)
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
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToMedicationOverview_Button)
            DropDownListDisabled(ActiveIngredients_DropDownList)
            TextBoxReadOnly(Quantity_Textbox)
            DropDownListDisabled(Units_DropDownList)
        End If
    End Sub

End Class
