Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_CreateMarketingCountry
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            MedicationID_HiddenField.Value = Request.QueryString("MedicationID")
            Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
            Title_Label.Text = "Medication " & CurrentMedication_ID & ": Add Marketing Country"
            'Check if user is logged in and redirect to login page if not
            If Login_Status = True Then
                'Check if user has create rights and lock out if not
                If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationsInCountries, fields.Create) = True Then
                    'Format Controls depending on user edit rights for each control
                    AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToMedicationOverview_Button)
                    If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationsInCountries, fields.Country_ID) = True Then
                        DropDownListEnabled(Countries_DropDownList)
                    Else
                        DropDownListDisabled(Countries_DropDownList)
                        Countries_DropDownList.ToolTip = CannotEditControlText
                    End If
                    'Populate Countries_DropDownList
                    Countries_DropDownList.DataSource = CreateDropDownListDatatable(tables.Countries)
                    Countries_DropDownList.DataValueField = "ID"
                    Countries_DropDownList.DataTextField = "Name"
                    Countries_DropDownList.DataBind()
                Else 'Lock out user if he/she does not have create rights
                    Title_Label.Text = Lockout_Text
                    ButtonGroup_Div.Visible = False
                    Main_Table.Visible = False
                End If
            Else 'Redirect user to login if he/she is not logged in
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/CreateMarketingCountry.aspx?MedicationID=" & CurrentMedication_ID)
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click() Handles Cancel_Button.Click, ReturnToMedicationOverview_Button.Click
        Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
        Response.Redirect("~/Application/MedicationOverview.aspx?MedicationID=" & CurrentMedication_ID)
    End Sub

    Protected Sub Countries_DropDownList_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If SelectionValidator(Countries_DropDownList) = True Then 'If country was selected
            'Check if slelcted marketing country already exists for current medication and fail validation if so 
            Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
            Dim MarketingCountryAlreadyExists As Boolean = False
            Dim MarketingCountriesReadCommand As New SqlCommand("SELECT Country_ID FROM MedicationsInCountries WHERE Medication_ID = @CurrentMedication_ID", Connection)
            MarketingCountriesReadCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
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
                Countries_DropDownList.CssClass = CssClassSuccess
                Countries_DropDownList.ToolTip = String.Empty
                args.IsValid = True
            Else
                Countries_DropDownList.CssClass = CssClassFailure
                Countries_DropDownList.ToolTip = MarketingCountryUniquenessValidationFailToolTip
                args.IsValid = False
            End If
        Else 'If no country was selected
            Countries_DropDownList.CssClass = CssClassFailure
            Countries_DropDownList.ToolTip = SelectorValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
            'Write New Marketing Country to database
            Dim InsertCommand As New SqlCommand("INSERT INTO MedicationsInCountries (Medication_ID, Country_ID) VALUES(@CurrentMedication_ID, CASE WHEN @Country_ID = 0 THEN NULL ELSE @Country_ID END)", Connection)
            InsertCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
            InsertCommand.Parameters.AddWithValue("@Country_ID", Countries_DropDownList.SelectedValue)
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
            Dim NewReadCommand As New SqlCommand("SELECT TOP 1 ID, Country_ID FROM MedicationsinCountries WHERE Medication_ID = @CurrentMedication_ID ORDER BY ID DESC", Connection)
            NewReadCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
            Dim NewMedicationInCountry_ID As Integer = Nothing
            Dim NewCountry_ID As Integer = Nothing
            Try
                Connection.Open()
                Dim NewReader As SqlDataReader = NewReadCommand.ExecuteReader()
                While NewReader.Read()
                    NewMedicationInCountry_ID = NewReader.GetInt32(0)
                    NewCountry_ID = NewReader.GetInt32(1)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            EntryString += NewReportIntro("Marketing Country", NewMedicationInCountry_ID)
            EntryString += HistoryEntryReferencedValue("Marketing Country", NewMedicationInCountry_ID, "Name", tables.Countries, fields.Name, Nothing, NewCountry_ID)
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
            DropDownListDisabled(Countries_DropDownList)
        End If
    End Sub

End Class
