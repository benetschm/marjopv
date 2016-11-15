Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_EditMarketingCountry
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystrings in hidden fields to prevent issues through URL tampering
            MedicationInCountryID_HiddenField.Value = Request.QueryString("MedicationInCountryID")
            Dim CurrentMedicationInCountry_ID As Integer = MedicationInCountryID_HiddenField.Value
            Delete_HiddenField.Value = Request.QueryString("Delete")
            Dim Delete As Boolean = False
            If Delete_HiddenField.Value = "True" Then
                Delete = True
            End If
            Dim CurrentMedication_ID As Integer = ParentID(tables.Medications, tables.MedicationsInCountries, fields.Medication_ID, CurrentMedicationInCountry_ID)
            MedicationID_HiddenField.Value = CurrentMedication_ID
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                'Populate Title_Label
                If Delete = False Then
                    Title_Label.Text = "Medication " & CurrentMedication_ID & ": Edit Marketing Country " & CurrentMedicationInCountry_ID
                ElseIf Delete = True Then
                    Title_Label.Text = "Medication " & CurrentMedication_ID & ": Delete Marketing Country " & CurrentMedicationInCountry_ID
                End If
                'Check if user has edit rights and lock out if not
                If Delete = False Then
                    If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationsInCountries, fields.Edit) = True Then
                        'Format controls as per user edit rights
                        AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToMedicationOverview_Button)
                        If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationsInCountries, fields.Country_ID) = True Then
                            DropDownListEnabled(Countries_DropDownList)
                        Else
                            DropDownListDisabled(Countries_DropDownList)
                            Countries_DropDownList.ToolTip = CannotEditControlText
                        End If
                    Else 'Lock out user if he/she does not have edit rights
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
                ElseIf Delete = True Then
                    If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationsInCountries, fields.Edit) = True Then
                        'Format controls as per user edit rights
                        AtDeleteButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToMedicationOverview_Button)
                        DropDownListDisabled(Countries_DropDownList)
                    Else 'Lock out user if he/she does not have edit rights
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                        Exit Sub
                    End If
                End If
                'Populate Countries_DropDownList
                Countries_DropDownList.DataSource = CreateDropDownListDatatable(tables.Countries)
                Countries_DropDownList.DataValueField = "ID"
                Countries_DropDownList.DataTextField = "Name"
                Countries_DropDownList.DataBind()
                'Populate controls from database
                Dim AtEditPageLoadReadCommand As New SqlCommand("SELECT CASE WHEN Country_ID IS NULL THEN 0 ELSE Country_ID END AS Country_ID FROM MedicationsInCountries WHERE ID = @CurrentMedicationInCountry_ID", Connection)
                AtEditPageLoadReadCommand.Parameters.AddWithValue("@CurrentMedicationInCountry_ID", CurrentMedicationInCountry_ID)
                Try
                    Connection.Open()
                    Dim AtEditPageLoadReader As SqlDataReader = AtEditPageLoadReadCommand.ExecuteReader()
                    While AtEditPageLoadReader.Read()
                        Countries_DropDownList.SelectedValue = AtEditPageLoadReader.GetInt32(0)
                        AtEditPageLoad_Countries_HiddenField.Value = AtEditPageLoadReader.GetInt32(0)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            Else 'Redirect user to login if he/she is not logged in
                If Delete = False Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditMarketingCountry.aspx?MedicationInCountryID=" & CurrentMedicationInCountry_ID)
                ElseIf Delete = True Then
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/EditMarketingCountry.aspx?MedicationInCountryID=" & CurrentMedicationInCountry_ID & "&Delete=True")
                End If
            End If
        End If
    End Sub

    Protected Sub ReturnToMedicationOverview() Handles Cancel_Button.Click, ReturnToMedicationOverview_Button.Click
        Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
        Response.Redirect("~/Application/MedicationOverview.aspx?MedicationID=" & CurrentMedication_ID)
    End Sub

    Protected Sub Countries_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If Countries_DropDownList.SelectedValue <> 0 Then
            'Check if slelcted marketing country already exists in another entry for the current medication and fail validation if so 
            Dim CurrentMedicationInCountry_ID As Integer = MedicationInCountryID_HiddenField.Value
            Dim MarketingCountryAlreadyExists As Boolean = False
            Dim MarketingCountriesReadCommand As New SqlCommand("SELECT Country_ID FROM MedicationsInCountries WHERE Medication_ID = @CurrentMedication_ID AND ID <> @CurrentMedicationInCountry_ID", Connection)
            MarketingCountriesReadCommand.Parameters.AddWithValue("@CurrentMedication_ID", ParentID(tables.Medications, tables.MedicationsInCountries, fields.Medication_ID, CurrentMedicationInCountry_ID))
            MarketingCountriesReadCommand.Parameters.AddWithValue("@CurrentMedicationInCountry_ID", CurrentMedicationInCountry_ID)
            Try
                Connection.Open()
                Dim MarketingCountriesReader As SqlDataReader = MarketingCountriesReadCommand.ExecuteReader()
                While MarketingCountriesReader.Read()
                    If Countries_DropDownList.SelectedValue = MarketingCountriesReader.GetInt32(0) Then
                        MarketingCountryAlreadyExists = True
                    End If
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            If MarketingCountryAlreadyExists = False Then
                Countries_DropDownList.CssClass = "form-control alert-success"
                Countries_DropDownList.ToolTip = String.Empty
                args.IsValid = True
            Else
                Countries_DropDownList.CssClass = "form-control alert-danger"
                Countries_DropDownList.ToolTip = "The marketing country you have selected already exists for this medication."
                args.IsValid = False
            End If
        Else 'Fail validation if no country was selected
            Countries_DropDownList.CssClass = "form-control alert-danger"
            Countries_DropDownList.ToolTip = "Please ensure you are selecting a valid entry"
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click, ConfirmDeletion_Button.Click
        If Page.IsValid = True Then
            Dim CurrentMedicationInCountry_ID As Integer = MedicationInCountryID_HiddenField.Value
            Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
            'Retrieve values as present in database at edit page load and store in variables to use when checking for database update conflicts (see page load event)
            Dim AtEditPageLoad_Country_ID As Integer = AtEditPageLoad_Countries_HiddenField.Value
            'Store values as present in database when save button is clicked in variables
            Dim AtSaveButtonClickReadCommand As New SqlCommand("SELECT CASE WHEN Country_ID IS NULL THEN 0 ELSE Country_ID END AS Country_ID FROM MedicationsInCountries WHERE ID = @CurrentMedicationInCountry_ID", Connection)
            AtSaveButtonClickReadCommand.Parameters.AddWithValue("@CurrentMedicationInCountry_ID", CurrentMedicationInCountry_ID)
            Dim AtSaveButtonClick_Country_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim AtSaveButtonClickReader As SqlDataReader = AtSaveButtonClickReadCommand.ExecuteReader()
                While AtSaveButtonClickReader.Read()
                    AtSaveButtonClick_Country_ID = AtSaveButtonClickReader.GetInt32(0)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            'Check for discrepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked
            Dim DiscrepancyString As String = String.Empty
            DiscrepancyString += DiscrepancyCheck(AtEditPageLoad_Country_ID, AtSaveButtonClick_Country_ID, "Country")
            'If Discprepancies between database contents as present when edit page was loaded and database contents as present when save button is clicked are found, show warning and abort update
            If DiscrepancyString <> String.Empty Then
                AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToMedicationOverview_Button)
                Status_Label.Style.Add("text-align", "left")
                Status_Label.Style.Add("height", "auto")
                Status_Label.Text = DiscrepancyStringIntro & DiscrepancyString & DiscrepancyStringOutro
                Status_Label.CssClass = "form-control alert-danger"
                DropDownListDisabled(Countries_DropDownList)
                Exit Sub
            End If
            'If no discrepancies were found between database contents as present when edit page was loaded and database contents as present when save button is clicked, write updates to database
            Dim UpdateCommand As New SqlCommand
            UpdateCommand.Connection = Connection
            If sender Is SaveUpdates_Button Then
                UpdateCommand.CommandText = "UPDATE MedicationsInCountries SET Country_ID = (CASE WHEN @Country_ID = 0 THEN NULL ELSE @Country_ID END) WHERE ID = @CurrentMedicationInCountry_ID"
                UpdateCommand.Parameters.AddWithValue("@Country_ID", Countries_DropDownList.SelectedValue)
                UpdateCommand.Parameters.AddWithValue("@CurrentMedicationInCountry_ID", CurrentMedicationInCountry_ID)
            ElseIf sender Is ConfirmDeletion_Button Then
                UpdateCommand.CommandText = "DELETE FROM MedicationsInCountries WHERE ID = @CurrentMedicationInCountry_ID"
                UpdateCommand.Parameters.AddWithValue("@CurrentMedicationInCountry_ID", CurrentMedicationInCountry_ID)
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
            Dim Updated_Country_ID As Integer = Nothing
            If sender Is SaveUpdates_Button Then
                Dim UpdatedReadCommand As New SqlCommand("SELECT CASE WHEN Country_ID IS NULL THEN 0 ELSE Country_ID END AS Country_ID FROM MedicationsInCountries WHERE ID = @CurrentMedicationInCountry_ID", Connection)
                UpdatedReadCommand.Parameters.AddWithValue("@CurrentMedicationInCountry_ID", CurrentMedicationInCountry_ID)
                Try
                    Connection.Open()
                    Dim UpdatedReader As SqlDataReader = UpdatedReadCommand.ExecuteReader()
                    While UpdatedReader.Read()
                        Updated_Country_ID = UpdatedReader.GetInt32(0)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
            End If
            'Compare old and new variables to generate EntryString for Change History Entry
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            If sender Is ConfirmDeletion_Button Then
                EntryString += DeleteReportIntro("Marketing Country", CurrentMedicationInCountry_ID)
            End If
            EntryString += HistoryEntryReferencedValue("Marketing Country", CurrentMedicationInCountry_ID, "", tables.Countries, fields.Name, AtSaveButtonClick_Country_ID, Updated_Country_ID)
            EntryString += HistoryDatabasebUpdateOutro
            'Generate History Entry if any data was changed in the database
            If EntryString <> HistoryDatabasebUpdateIntro & HistoryDatabasebUpdateOutro Then
                Dim InsertHistoryEntryCommand As New SqlCommand("INSERT INTO MedicationHistories (Medication_ID, User_ID, Timepoint, Entry) VALUES (@Medication_ID, @User_ID, @Timepoint, CASE WHEN @Entry = '' THEN NULL ELSE @Entry END)", Connection)
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
            End If
            'Format Controls
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, ConfirmDeletion_Button, Cancel_Button, ReturnToMedicationOverview_Button)
            If sender Is SaveUpdates_Button Then
                DropDownListDisabled(Countries_DropDownList)
            ElseIf sender Is ConfirmDeletion_Button Then
                Name_Row.Visible = False
            End If
        End If
    End Sub

End Class
