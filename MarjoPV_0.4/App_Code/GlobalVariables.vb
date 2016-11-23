Imports Microsoft.VisualBasic
Imports System.Data.SqlClient

Public Class GlobalVariables
    Public Shared Connection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("MarjoPV_ConnectionString").ConnectionString)
    Public Shared LoggedIn_User_ID As Integer
    Public Shared Logged_In_Username As String
    Public Shared LoggedIn_User_Companies As ArrayList = New ArrayList()
    Public Shared LoggedIn_User_Name As String
    Public Shared LoggedIn_User_Session_ID As String
    Public Shared LoggedIn_User_CanViewCompanies As Boolean
    Public Shared Logged_In_User_Admin As Boolean
    Public Shared LoggedIn_User_CanCreateICSRs As Boolean
    Public Shared Login_Status As Boolean
    Public Shared CssClassUnsavedChanges As String = "form-control alert-warning"
    Public Shared CssClassSuccess As String = "form-control alert-success"
    Public Shared CssClassFailure As String = "form-control alert-danger"
    Public Shared Lockout_Text As String = "Sorry, you don't have adequate rights to view this page. If you think this is a mistake, please contact the site administration team"
    Public Shared DatabaseConnectionErrorString As String = " *** WARNING: Database Connection Error. Please document update(s) manually.*** "
    Public Shared HistoryDatabasebUpdateIntro As String = "<p><u>Database Update:</u></br>"
    Public Shared HistoryDatabasebUpdateOutro As String = "</p>"
    Public Shared HistoryManualEntryIntro As String = "<p><u>Manual User Entry:</u></br>"
    Public Shared HistoryManualEntryOutro As String = "</p>"
    Public Shared CannotEditControlText As String = "None of the roles currently allocated to your user has edit rights to this data field. If you think this is an error, please contact the administration team."
    Public Shared DiscrepancyStringIntro As String = "<p>The dataset you are editing was changed in the database after you opened the edit page and before you clicked the save button. Please reload the edit page and resubmit your changes. The disrepancies are the following:</br><ul>"
    Public Shared DiscrepancyStringOutro As String = "</ul></p>"
    Public Shared UsernameInputToolTip As String = "Please enter username."
    Public Shared UsernameValidationFailToolTip As String = "Please ensure you are entering a valid username in the format of an email address."
    Public Shared UserNameTakenValidationFailToolTip As String = "The user name you have entered is already taken. Please choose a different user name"
    Public Shared PasswordInputToolTip As String = "Please enter password."
    Public Shared ConfirmPasswordInputToolTip As String = "Please confirm password."
    Public Shared ConfirmPasswordValidationFailToolTip As String = "The password and password confirmation entries do not match. Please re-enter"
    Public Shared PasswordValidationFailToolTip As String = "Please ensure you are entering a valid password."
    Public Shared PatientInitialsInputToolTip As String = "Please enter patient initials"
    Public Shared PatientYearOfBirthInputToolTip As String = "Please select patient year of birth"
    Public Shared PatientGenderInputToolTip As String = "Please select patient gender"
    Public Shared ICSRSeriousnessSelectorInputToolTip As String = "Please select whether the ICSR is serious"
    Public Shared ICSRSeriousnessCriterionSelectorInputToolTip As String = "Please select a seriousness criterion if the ICSR is serious"
    Public Shared NarrativeInputToolTip As String = "Please enter a narrative text"
    Public Shared CompanyCommentInputToolTip As String = "Please enter a company comment text"
    Public Shared FullNameInputToolTip As String = "Please enter a full name"
    Public Shared CompanyNameInputToolTip As String = "Please enter company name"
    Public Shared CompanyNameTakenValidationFailToolTip As String = "The name you have entered is already taken. Please choose a different company name"
    Public Shared NameValidationFailToolTip As String = "Please ensure you are entering a valid name"
    Public Shared NameUniquenessValidationFailToolTip As String = "The name you have selected is already in the database. Please select a different name"
    Public Shared SortOrderValidationFailToolTip As String = "Please make sure you are specifying a valid sort order"
    Public Shared SelectUserValidationFailToolTip As String = "Please make sure you are selecting a user"
    Public Shared SelectCompanyValidationFailToolTip As String = "Please make sure you are selecting a company"
    Public Shared SelectRoleValidationFailToolTip As String = "Please make sure you are selecting a role"
    Public Shared SelectorInputToolTip As String = "Please select an entry."
    Public Shared PhoneInputToolTip As String = "Please enter a phone number (format e.g. +33-123-12345)."
    Public Shared EmailInputToolTip As String = "Please enter a valid email address."
    Public Shared NumberInputToolTip As String = "Please enter a numeric value."
    Public Shared DateInputToolTip As String = "Please enter a date (e.g. 01-Jan-2001)."
    Public Shared TextValidationFailToolTip As String = "Please ensure you are entering valid text"
    Public Shared NumberValidationFailToolTip As String = "Please ensure you are entering a valid numeric value."
    Public Shared DateValidationFailToolTip As String = "Please ensure you are entering a valid date."
    Public Shared PhoneValidationFailToolTip As String = "Please ensure you are entering a phone number (e.g. +33-123-12345)."
    Public Shared FaxValidationFailToolTip As String = "Please ensure you are entering a fax number (e.g. +33-123-12345)."
    Public Shared EmailValidationFailToolTip As String = "Please ensure you are entering a email address (e.g. joeshmoe@somewhere.com)."
    Public Shared SelectorValidationFailToolTip As String = "Please ensure you are selecting a valid entry."
    Public Shared MedDRATermValidationFailToolTip As String = "Please ensure you are selecting a valid MedDRA Low Level Term."
    Public Shared FileAttachValidationFailToolTip As String = "No file selected for upload"
    Public Shared DateInconsistencyValidationFailToolTip As String = "There are inconsistencies between the dates you have entered. Please make sure you are making consistent entries"
    Public Shared ExpeditedReportingConsistencyValidationFailToolTip As String = "There are inconsistencies in the entries related to expedited reporting. Please ensure you are making consistent entries"
    Public Shared ReportTypeUniquenessValidationFailToolTip As String = "The report type you have selected has already been selected for this ICSR. Please select another report type"
    Public Shared RelationDuplicationFoundMessage As String = "The relation you have specifid has already been defined. Please specify a different relation."
    Public Shared RelationCriteriaNotFullySpecified As String = "'Event' and 'Medication' must be specified for each relation. Please specify the missing value(s)"
    Public Shared MarketingCountryUniquenessValidationFailToolTip As String = "The marketing country you have selected already exists for this medication."
    Public Shared StatusUniquenessValidationFailToolTip As String = "The status name you have entered is already taken. Please choose a different status name"
    Public Shared DependencyFoundMessage As String = "The dataset you are trying to change is related to another dataset. Please change or delete the dependent dataset before changing this dataset."
    Public Shared SeriousnessConsistencyValidationFailToolTip As String = "The entries for 'Is Serious' and 'Seriousness Criterion' do not match. Please ensure that a seriousness criterion is selected only if the ICSR is rated as serious."
End Class
