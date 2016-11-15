Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_MedicationOverview
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            MedicationID_HiddenField.Value = Request.QueryString("MedicationID")
            Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
            If Login_Status = True Then
                Title_Label.Text = "Medication " & CurrentMedication_ID & " Overview"
                'Populate Basic Information Tab Controls
                Dim BasicInformationReadCommand As New SqlCommand("SELECT Medications.ID AS Medication_ID, Companies.Name AS Company_Name, Medications.Name, MedicationTypes.Name, AdministrationRoutes.Name AS AdministrationRoute_Name, DoseTypes.Name As DoseType_Name FROM Medications INNER JOIN Companies ON Medications.Company_ID = Companies.ID LEFT JOIN MedicationTypes ON Medications.MedicationType_ID = MedicationTypes.ID LEFT JOIN AdministrationRoutes ON Medications.AdministrationRoute_ID = AdministrationRoutes.ID LEFT JOIN DoseTypes ON DoseTypes.ID = Medications.DoseType_ID WHERE Medications.ID = @CurrentMedication_ID", Connection)
                BasicInformationReadCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
                Try
                    Connection.Open()
                    Dim BasicInformationReader As SqlDataReader = BasicInformationReadCommand.ExecuteReader()
                    While BasicInformationReader.Read()
                        ID_Textbox.Text = BasicInformationReader.GetInt32(0)
                        CompanyName_Textbox.Text = BasicInformationReader.GetString(1)
                        Name_Textbox.Text = BasicInformationReader.GetString(2)
                        MedicationType_Textbox.Text = BasicInformationReader.GetString(3)
                        AdministrationRoute_Textbox.Text = BasicInformationReader.GetString(4)
                        DoseType_Textbox.Text = BasicInformationReader.GetString(5)
                    End While
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
                'Format Basic Information Tab Controls
                If CanEdit(tables.Medications, CurrentMedication_ID, tables.Medications, fields.Edit) = True Then 'Enable Controls if LoggedIn_User has corresponding rights
                    EditMedicationbBasicInformation_Button.Visible = True
                Else
                    EditMedicationbBasicInformation_Button.Visible = False
                End If
                If LoggedIn_User_CanViewCompanies = True Then 'Enable Control if LoggedIn_User has corresponding rights
                    Company_Row.Visible = True
                End If
                TextBoxReadOnly(ID_Textbox)
                TextBoxReadOnly(CompanyName_Textbox)
                TextBoxReadOnly(Name_Textbox)
                TextBoxReadOnly(MedicationType_Textbox)
                TextBoxReadOnly(AdministrationRoute_Textbox)
                TextBoxReadOnly(DoseType_Textbox)
                'Populate Active Ingredients Tab Controls
                Dim MedicationIngredientsReadCommand As New SqlCommand("SELECT MedicationIngredients.ID AS MedicationIngredient_ID, ActiveIngredients.Name AS MedicationIngredient_Name, MedicationIngredients.Quantity AS MedicationIngredient_Quantity, Units.Name AS MedicationIngredientUnit_Name FROM MedicationIngredients INNER JOIN Medications ON MedicationIngredients.Medication_ID = Medications.ID INNER JOIN ActiveIngredients ON MedicationIngredients.ActiveIngredient_ID = ActiveIngredients.ID INNER JOIN Units ON MedicationIngredients.Unit_ID = Units.ID WHERE Medications.ID = @CurrentMedication_ID ORDER BY MedicationIngredients.ID", Connection)
                MedicationIngredientsReadCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
                Try
                    Connection.Open()
                    MedicationIngredients_Repeater.DataSource = MedicationIngredientsReadCommand.ExecuteReader()
                    MedicationIngredients_Repeater.DataBind()
                    Connection.Close()
                Catch ex As Exception
                    Connection.Close()
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                End Try
                'Format Active Ingredients Tab Controls
                Dim MedicationIngredients_Repeater_HeaderTemplate As Control = MedicationIngredients_Repeater.Controls(0).Controls(0)
                Dim AddMedicationIngredient_Button As Button = TryCast(MedicationIngredients_Repeater_HeaderTemplate.FindControl("AddMedicationIngredient_Button"), Button)
                If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationIngredients, fields.Create) = True Then
                    AddMedicationIngredient_Button.Visible = True
                Else
                    AddMedicationIngredient_Button.Visible = False
                End If
                Dim CurrentUserCanEditMedicationIngredient As Boolean = False
                If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationIngredients, fields.Edit) = True Then
                    CurrentUserCanEditMedicationIngredient = True
                End If
                Dim CurrentUserCanDeleteMedicationIngredient As Boolean = False
                If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationIngredients, fields.Delete) = True Then
                    CurrentUserCanDeleteMedicationIngredient = True
                End If
                For Each item In MedicationIngredients_Repeater.Items
                    If CurrentUserCanEditMedicationIngredient = True Or CurrentUserCanDeleteMedicationIngredient = True Then
                        item.findcontrol("MedicationIngredientTitle").colspan = "1"
                        item.findcontrol("MedicationIngredient_Buttons_Header").visible = True
                        If CurrentUserCanEditMedicationIngredient = True Then
                            item.findcontrol("EditMedicationIngredient_Button").Visible = True
                        Else
                            item.findcontrol("EditMedicationIngredient_Button").Visible = False
                        End If
                        If CurrentUserCanDeleteMedicationIngredient = True Then
                            item.findcontrol("DeleteMedicationIngredient_Button").Visible = True
                        Else
                            item.findcontrol("DeleteMedicationIngredient_Button").Visible = False
                        End If
                    Else
                        item.findcontrol("MedicationIngredientTitle").colspan = "2"
                        item.findcontrol("MedicationIngredient_Buttons_Header").Visible = False
                    End If
                    item.findcontrol("MedicationIngredientName_TextBox").ReadOnly = True
                    item.findcontrol("MedicationIngredientQuantity_TextBox").ReadOnly = True
                    item.findcontrol("MedicationIngredientUnit_TextBox").ReadOnly = True
                Next
                'Populate Marketing Countries Tab Controls
                Dim MarketingCountriesReadCommand As New SqlCommand("SELECT MedicationsInCountries.ID AS MarketingCountry_ID, Countries.Name AS MarketingCountry_Name FROM MedicationsInCountries INNER JOIN Medications ON MedicationsInCountries.Medication_ID = Medications.ID INNER JOIN Countries ON MedicationsInCountries.Country_ID = Countries.ID WHERE Medications.ID = @CurrentMedication_ID ORDER BY Countries.ID", Connection)
                MarketingCountriesReadCommand.Parameters.AddWithValue("@CurrentMedication_ID", CurrentMedication_ID)
                Try
                    Connection.Open()
                    MarketingCountries_Repeater.DataSource = MarketingCountriesReadCommand.ExecuteReader()
                    MarketingCountries_Repeater.DataBind()
                Catch ex As Exception
                    Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                    Exit Sub
                Finally
                    Connection.Close()
                End Try
                'Format Marketing Countries Tab Controls
                Dim MarkeTingCountries_Repeater_HeaderTemplate As Control = MarketingCountries_Repeater.Controls(0).Controls(0)
                Dim AddMarketingCountry_Button As Button = TryCast(MarkeTingCountries_Repeater_HeaderTemplate.FindControl("AddMarketingCountry_Button"), Button)
                If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationsInCountries, fields.Create) = True Then
                    AddMarketingCountry_Button.Visible = True
                Else
                    AddMarketingCountry_Button.Visible = False
                End If
                Dim CurrentUserCanEditMarketingCountryInCurrentMedication As Boolean = False
                If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationsInCountries, fields.Edit) = True Then
                    CurrentUserCanEditMarketingCountryInCurrentMedication = True
                End If
                Dim CurrentUserCanDeleteMarketingCountryInCurrentMedication As Boolean = False
                If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationsInCountries, fields.Delete) = True Then
                    CurrentUserCanDeleteMarketingCountryInCurrentMedication = True
                End If
                For Each item In MarketingCountries_Repeater.Items
                    If CurrentUserCanEditMarketingCountryInCurrentMedication = True Or CurrentUserCanDeleteMarketingCountryInCurrentMedication = True Then
                        item.findcontrol("MarketingCountryTitle").colspan = "1"
                        item.findcontrol("MarketingCountry_Buttons_Header").visible = True
                        If CurrentUserCanEditMarketingCountryInCurrentMedication = True Then
                            item.findcontrol("EditMarketingCountry_Button").Visible = True
                        Else
                            item.findcontrol("EditMarketingCountry_Button").Visible = False
                        End If
                        If CurrentUserCanDeleteMarketingCountryInCurrentMedication = True Then
                            item.findcontrol("DeleteMarketingCountry_Button").Visible = True
                        Else
                            item.findcontrol("DeleteMarketingCountry_Button").Visible = False
                        End If
                    Else
                        item.findcontrol("MarketingCountryTitle").colspan = "2"
                        item.findcontrol("MarketingCountry_Buttons_Header").Visible = False
                    End If
                    item.findcontrol("MarketingCountryName_TextBox").ReadOnly = True
                Next
                'Populate Change History Tab Controls
                ChangeHistory_Repeater.DataSource = History(tables.MedicationHistories, fields.Medication_ID, CurrentMedication_ID)
                ChangeHistory_Repeater.DataBind()
                'Populate Attached Files List
                AttachedFiles_Repeater.DataSource = AttachedFiles(tables.Medications, CurrentMedication_ID)
                AttachedFiles_Repeater.DataBind()
                'Format Attached Files Tab Controls
                Dim AttachedFiles_Repeater_HeaderTemplate As Control = AttachedFiles_Repeater.Controls(0).Controls(0)
                Dim AttachFile_Button As Button = TryCast(AttachedFiles_Repeater_HeaderTemplate.FindControl("AttachFile_Button"), Button)
                If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationsAttachedFiles, fields.Create) = True Then 'Enable Controls if LoggedIn_User has corresponding rights
                    AttachFile_Button.Visible = True
                Else
                    AttachFile_Button.Visible = False
                End If
                Dim CurrentUserCanDeleteAttachedFilesInCurrentMedication As Boolean = False
                If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationsAttachedFiles, fields.Delete) = True Then
                    CurrentUserCanDeleteAttachedFilesInCurrentMedication = True
                End If
                For Each item In AttachedFiles_Repeater.Items
                    If CurrentUserCanDeleteAttachedFilesInCurrentMedication = True Then
                        item.findcontrol("DeleteAttachedFile_Button").Visible = True
                    Else
                        item.findcontrol("DeleteAttachedFile_Button").Visible = False
                    End If
                    item.findcontrol("Filename_Textbox").ReadOnly = True
                    item.findcontrol("DateAdded_Textbox").ReadOnly = True
                Next
            Else
                Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/MedicationOverview.aspx?MedicationID=" & CurrentMedication_ID)
            End If
        End If
    End Sub

    Protected Sub EditMedicationbBasicInformation_Button_Click(sender As Object, e As EventArgs) Handles EditMedicationbBasicInformation_Button.Click
        Response.Redirect("~/Application/EditMedicationBasicInformation.aspx?MedicationID=" & MedicationID_HiddenField.Value)
    End Sub

    Protected Sub AddMedicationIngredient_Button_Click(sender As Object, e As EventArgs)
        Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
        Dim HeaderTemplate As Control = MedicationIngredients_Repeater.Controls(0).Controls(0)
        Dim CreateMedicationIngredient_Button As Button = TryCast(HeaderTemplate.FindControl("CreateMedicationIngredient_Button"), Button)
        Response.Redirect("~/Application/CreateMedicationIngredient.aspx?MedicationID=" & CurrentMedication_ID)
    End Sub

    Protected Sub EditMedicationIngredient_Button_Click(sender As Object, e As EventArgs)
        Dim EditMedicationIngredient_Button As Button = CType(sender, Button)
        Dim MedicationIngredient_ID As String = EditMedicationIngredient_Button.CommandArgument
        Response.Redirect("~/Application/EditMedicationIngredient.aspx?MedicationIngredientID=" & MedicationIngredient_ID)
    End Sub

    Protected Sub DeleteMedicationIngredient_Button_Click(sender As Object, e As EventArgs)
        Dim DeleteMedicationIngredient_Button As Button = CType(sender, Button)
        Dim MedicationIngredient_ID As String = DeleteMedicationIngredient_Button.CommandArgument
        Response.Redirect("~/Application/EditMedicationIngredient.aspx?MedicationIngredientID=" & MedicationIngredient_ID & "&Delete=True")
    End Sub

    Protected Sub AddMarketingCountry_Button_Click(sender As Object, e As EventArgs)
        Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
        Dim HeaderTemplate As Control = MarketingCountries_Repeater.Controls(0).Controls(0)
        Dim CreateMarketingCountry_Button As Button = TryCast(HeaderTemplate.FindControl("CreateMarketingCountry_Button"), Button)
        Response.Redirect("~/Application/CreateMarketingCountry.aspx?MedicationID=" & CurrentMedication_ID)
    End Sub

    Protected Sub EditMarketingCountry_Button_Click(sender As Object, e As EventArgs)
        Dim EditMarketingCountry_Button As Button = CType(sender, Button)
        Dim MarketingCountry_ID As String = EditMarketingCountry_Button.CommandArgument
        Response.Redirect("~/Application/EditMarketingCountry.aspx?MedicationInCountryID=" & MarketingCountry_ID)
    End Sub

    Protected Sub DeleteMarketingCountry_Button_Click(sender As Object, e As EventArgs)
        Dim DeleteMarketingCountry_Button As Button = CType(sender, Button)
        Dim MarketingCountry_ID As String = DeleteMarketingCountry_Button.CommandArgument
        Response.Redirect("~/Application/EditMarketingCountry.aspx?MedicationInCountryID=" & MarketingCountry_ID & "&Delete=True")
    End Sub

    Protected Sub CreateMedicationHistoryEntry_Button_Click(sender As Object, e As EventArgs)
        Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
        Dim CreateMedicationsHistoryEntry_Button As Button = CType(sender, Button)
        Response.Redirect("~/Application/CreateMedicationHistoryEntry.aspx?MedicationID=" & CurrentMedication_ID)
    End Sub

    Protected Sub AttachFile_Button_Click(sender As Object, e As EventArgs)
        Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
        Dim AttachFile_Button As Button = CType(sender, Button)
        Response.Redirect("~/Application/AttachFile.aspx?MedicationID=" & CurrentMedication_ID)
    End Sub

    Protected Sub DownloadAttachedFile_Button_Click(sender As Object, e As EventArgs)
        Dim VirtualPath As String = sender.commandargument
        Dim DownloadAttachedFile_Button As Button = CType(sender, Button)
        Dim AttachedFileItem As RepeaterItem = CType(DownloadAttachedFile_Button.NamingContainer, RepeaterItem)
        Dim FileName_Textbox As TextBox = AttachedFileItem.FindControl("FileName_Textbox")
        Dim FileName As String = FileName_Textbox.Text
        Dim path As String = Server.MapPath(VirtualPath) 'get file object as FileInfo
        Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)
        Try
            Response.Clear()
            Response.AddHeader("content-disposition", "attachment; filename=""" & FileName & """") '-- added double at end ensure that whitespaces in FileName are recognized correctly 
            Response.AddHeader("Content-Length", file.Length.ToString())
            Response.ContentType = "application/octet-stream"
            Response.WriteFile(file.FullName)
        Catch ex As Exception
            Response.Redirect("~/Errors/OtherError.aspx")
        Finally
            Response.End()
        End Try
    End Sub

    Protected Sub DeleteAttachedFile_Button_Click(sender As Object, e As EventArgs)
        Dim DeleteAttachedFile_Button As Button = CType(sender, Button)
        Dim ClickedItem As RepeaterItem = CType(DeleteAttachedFile_Button.NamingContainer, RepeaterItem)
        ClickedItem.FindControl("DeleteAttachedFile_Button").Visible = False
        ClickedItem.FindControl("ConfirmDeleteAttachedFile_Button").Visible = True
    End Sub

    Protected Sub ConfirmDeleteAttachedFile_Button_Click(sender As Object, e As EventArgs)
        Dim CurrentMedication_ID As Integer = MedicationID_HiddenField.Value
        Dim VirtualPath As String = sender.CommandArgument
        Dim path As String = Server.MapPath(VirtualPath)
        Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)
        Dim GUID As String = System.IO.Path.GetFileNameWithoutExtension(file.Name)
        'Update 'Removed' in dataset of deleted file in AttachedFiles to NOW()
        Dim FileDeletionUpdateCommand As New SqlCommand("UPDATE AttachedFiles SET Removed = @Removed WHERE GUID = @GUID", Connection)
        FileDeletionUpdateCommand.Parameters.AddWithValue("@Removed", Now())
        FileDeletionUpdateCommand.Parameters.AddWithValue("@GUID", GUID)
        Try
            Connection.Open()
            FileDeletionUpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
        'Add change history entry with database updates
        Dim RemovedReadCommand As New SqlCommand("SELECT ID, CASE WHEN Removed IS NULL THEN '' ELSE Removed END AS Removed FROM AttachedFiles WHERE GUID = @GUID", Connection)
        RemovedReadCommand.Parameters.AddWithValue("@GUID", GUID)
        Dim RemovedAttachedFile_ID As Integer = Nothing
        Dim RemovedAttachedFile_Removed As DateTime = Date.MinValue
        Try
            Connection.Open()
            Dim RemovedReader As SqlDataReader = RemovedReadCommand.ExecuteReader()
            While RemovedReader.Read()
                RemovedAttachedFile_ID = RemovedReader.GetInt32(0)
                RemovedAttachedFile_Removed = DateOrDateMinValue(RemovedReader.GetDateTime(1))
            End While
        Catch ex As Exception
            Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
            Exit Sub
        Finally
            Connection.Close()
        End Try
        Dim EntryString As String = String.Empty
        EntryString = HistoryDatabasebUpdateIntro
        EntryString += DeleteReportIntro("Attached File", RemovedAttachedFile_ID)
        EntryString += HistoryEnrtyPlainValue("Attached File", RemovedAttachedFile_ID, "Removed", Date.MinValue, RemovedAttachedFile_Removed)
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
        'Refresh CaseHistoryRepeater
        ChangeHistory_Repeater.DataSource = History(tables.MedicationHistories, fields.Medication_ID, CurrentMedication_ID)
        ChangeHistory_Repeater.DataBind()
        'Refresh AttachedFiles_Repeater Contents
        AttachedFiles_Repeater.DataSource = AttachedFiles(tables.Medications, CurrentMedication_ID)
        AttachedFiles_Repeater.DataBind()
    End Sub

End Class
