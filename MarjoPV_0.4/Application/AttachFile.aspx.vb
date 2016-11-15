Imports System.Data
Imports System.IO
Imports System.Data.SqlClient
Imports GlobalVariables
Imports GlobalCode

Partial Class Application_AttachFile
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            'Store querystring in hiddenfield to prevent issues through URL tampering
            ICSRID_HiddenField.Value = Request.QueryString("ICSRID")
            MedicationID_HiddenField.Value = Request.QueryString("MedicationID")
            Dim CurrentICSR_ID As Integer = Nothing
            Dim CurrentMedication_ID As Integer = Nothing
            Dim Context As tables
            If ICSRID_HiddenField.Value <> String.Empty Then
                CurrentICSR_ID = ICSRID_HiddenField.Value
                Context = tables.ICSRs
                ReturnToOverview_Button.Text = "ICSR Overview"
            End If
            If MedicationID_HiddenField.Value <> String.Empty Then
                CurrentMedication_ID = MedicationID_HiddenField.Value
                Context = tables.Medications
                ReturnToOverview_Button.Text = "Medication Overview"
            End If
            If Context = tables.ICSRs Then
                Title_Label.Text = "ICSR " & CurrentICSR_ID & ": Attach File"
                'Check if user is logged in and redirect to login page if not
                If Login_Status = True Then
                    'Check if user has create rights and lock out if not
                    If CanEdit(tables.ICSRs, CurrentICSR_ID, tables.ICSRsAttachedFiles, fields.Create) = True Then
                        'Format Controls depending on user edit rights for each control
                        AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToOverview_Button)
                        AttachedFileUpload.Enabled = True
                    Else 'Lock out user if he/she does not have create rights
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                    End If
                Else 'Redirect user to login if he/she is not logged in
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/AttachFile.aspx?ICSRID=" & CurrentICSR_ID)
                End If
            ElseIf Context = tables.Medications Then
                Title_Label.Text = "Medication " & CurrentMedication_ID & ": Attach File"
                'Check if user is logged in and redirect to login page if not
                If Login_Status = True Then
                    'Check if user has create rights and lock out if not
                    If CanEdit(tables.Medications, CurrentMedication_ID, tables.MedicationsAttachedFiles, fields.Create) = True Then
                        'Format Controls depending on user edit rights for each control
                        AtEditPageLoadButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToOverview_Button)
                        AttachedFileUpload.Enabled = True
                    Else 'Lock out user if he/she does not have create rights
                        Title_Label.Text = Lockout_Text
                        ButtonGroup_Div.Visible = False
                        Main_Table.Visible = False
                    End If
                Else 'Redirect user to login if he/she is not logged in
                    Response.Redirect("~/Login.aspx?ReturnUrl=" & VirtualPathUtility.ToAbsolute("~/") & "Application/AttachFile.aspx?MedicationID=" & CurrentMedication_ID)
                End If
            End If
        End If
    End Sub

    Protected Sub Cancel_Button_Click() Handles Cancel_Button.Click, ReturnToOverview_Button.Click
        'Populate CurrentICSR_ID / CurrentMedicationID from hidden fields
        Dim Association_ID As Integer = Nothing
        'Redirect user to ICSROverview or MedicationOverview 
        If ICSRID_HiddenField.Value <> String.Empty Then
            Association_ID = ICSRID_HiddenField.Value
            Response.Redirect("~/Application/ICSROverview.aspx?ICSRID=" & Association_ID)
        End If
        If MedicationID_HiddenField.Value <> String.Empty Then
            Association_ID = MedicationID_HiddenField.Value
            Response.Redirect("~/Application/MedicationOverview.aspx?MedicationID=" & Association_ID)
        End If
    End Sub

    Protected Sub AttachedFileUpload_Validator_ServerValidate(source As Object, args As ServerValidateEventArgs)
        If FileAttachValidator(AttachedFileUpload) = True Then 'if a file was selected for upload
            AttachedFileUpload.CssClass = CssClassSuccess
            AttachedFileUpload.ToolTip = String.Empty
            args.IsValid = True
        Else 'if no file was selected for upload
            AttachedFileUpload.CssClass = CssClassFailure
            AttachedFileUpload.ToolTip = FileAttachValidationFailToolTip
            args.IsValid = False
        End If
    End Sub

    Protected Sub SaveUpdates_Button_Click(sender As Object, e As EventArgs) Handles SaveUpdates_Button.Click
        If Page.IsValid = True Then
            'Populate Association_ID, Context, HistoryTable and HistoryFiels depending on context (Medications or ICSRs)
            Dim Association_ID As Integer = Nothing
            Dim Context As tables
            Dim HistoryTable As tables
            Dim HistoryField As fields
            If ICSRID_HiddenField.Value <> String.Empty Then
                Association_ID = ICSRID_HiddenField.Value
                Context = tables.ICSRs
                HistoryTable = tables.ICSRHistories
                HistoryField = fields.ICSR_ID
            End If
            If MedicationID_HiddenField.Value <> String.Empty Then
                Association_ID = MedicationID_HiddenField.Value
                Context = tables.Medications
                HistoryTable = tables.MedicationHistories
                HistoryField = fields.Medication_ID
            End If
            'Upload attached file
            Dim virtualFolder As String = "~/AttachedFiles/" & GetCompanyID(Association_ID, Context) & "/" & [Enum].GetName(GetType(tables), Context) & "/" & Association_ID & "/"
            Dim physicalFolder As String = Server.MapPath(virtualFolder)
            Dim fileGuid As String = Guid.NewGuid().ToString()
            Dim fileName As String = AttachedFileUpload.FileName
            Dim extension As String = System.IO.Path.GetExtension(AttachedFileUpload.FileName)
            If (Not System.IO.Directory.Exists(physicalFolder)) Then
                System.IO.Directory.CreateDirectory(physicalFolder)
            End If
            AttachedFileUpload.SaveAs(System.IO.Path.Combine(physicalFolder, fileGuid + extension))
            'Create dataset for new attached file in table AttachedFiles 
            Dim InsertCommand As New SqlCommand("INSERT INTO AttachedFiles(Association_Table, Association_ID, GUID, Name, Extension, Added) VALUES(@Association_Table, @Association_ID, @GUID, @Name, @Extension, @Added)", Connection)
            InsertCommand.Parameters.AddWithValue("@Association_Table", [Enum].GetName(GetType(tables), Context))
            InsertCommand.Parameters.AddWithValue("@Association_ID", Association_ID)
            InsertCommand.Parameters.AddWithValue("@GUID", fileGuid)
            InsertCommand.Parameters.AddWithValue("@Name", fileName)
            InsertCommand.Parameters.AddWithValue("@Extension", extension)
            InsertCommand.Parameters.AddWithValue("@Added", Now())
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
            Dim NewReadCommand As New SqlCommand("SELECT TOP 1 ID, Name, Extension, Added FROM AttachedFiles WHERE Association_Table = @Association_Table AND Association_ID = @Association_ID ORDER BY ID DESC", Connection)
            NewReadCommand.Parameters.AddWithValue("@Association_Table", [Enum].GetName(GetType(tables), Context))
            NewReadCommand.Parameters.AddWithValue("@Association_ID", Association_ID)
            Dim NewAttachedFile_ID As Integer = Nothing
            Dim NewAttachedFile_Name As String = String.Empty
            Dim NewAttachedFile_Extension As String = String.Empty
            Dim NewAttachedFile_Added As DateTime = Date.MinValue
            Try
                Connection.Open()
                Dim NewReader As SqlDataReader = NewReadCommand.ExecuteReader()
                While NewReader.Read()
                    NewAttachedFile_ID = NewReader.GetInt32(0)
                    NewAttachedFile_Name = NewReader.GetString(1)
                    NewAttachedFile_Extension = NewReader.GetString(2)
                    NewAttachedFile_Added = NewReader.GetDateTime(3)
                End While
            Catch ex As Exception
                Response.Redirect("~/Errors/DatabaseConnectionError.aspx")
                Exit Sub
            Finally
                Connection.Close()
            End Try
            Dim EntryString As String = String.Empty
            EntryString = HistoryDatabasebUpdateIntro
            EntryString += NewReportIntro("Attached File", NewAttachedFile_ID)
            EntryString += HistoryEnrtyPlainValue("Attached File", NewAttachedFile_ID, "Name", String.Empty, NewAttachedFile_Name)
            EntryString += HistoryEnrtyPlainValue("Attached File", NewAttachedFile_ID, "Date Added", Date.MinValue, NewAttachedFile_Added)
            EntryString += HistoryDatabasebUpdateOutro
            Dim InsertHistoryEntryCommandString As String = "INSERT INTO " & [Enum].GetName(GetType(tables), HistoryTable) & " (" & [Enum].GetName(GetType(fields), HistoryField) & ", User_ID, Timepoint, Entry) VALUES (@Association_ID, @User_ID, @Timepoint, @Entry)"
            Dim InsertHistoryEntryCommand As New SqlCommand(InsertHistoryEntryCommandString, Connection)
            InsertHistoryEntryCommand.Parameters.AddWithValue("@Association_ID", Association_ID)
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
            AtSaveButtonClickButtonsFormat(Status_Label, SaveUpdates_Button, Nothing, Nothing, Cancel_Button, ReturnToOverview_Button)
            AttachedFileUpload.Enabled = False
        End If
    End Sub
End Class
