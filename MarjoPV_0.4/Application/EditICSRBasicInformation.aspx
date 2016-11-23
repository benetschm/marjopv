<%@ Page Title="Edit ICSR Basic Information" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="EditICSRBasicInformation.aspx.vb" Inherits="Application_EditICSRBasicInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <asp:Label ID="Title_Label" runat="server" class="h3"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <div id="ButtonGroup_Div" class="panel btn-group" runat="server">
                <asp:button ID="SaveUpdates_Button" runat="server" class="btn btn-primary" Text="Save Updates"></asp:button>
                <asp:button ID="Cancel_Button" runat="server" class="btn btn-primary" Text="Cancel"></asp:button>
                <asp:button ID="ReturnToICSROverview_Button" runat="server" class="btn btn-primary" Text="ICSR Overview"></asp:button>
            </div>
        </div>
    </div>
    <div class="col-lg-12">
        <asp:UpdatePanel ID="Main_Table" runat="server">
            <ContentTemplate>
                <table class="table table-responsive table-striped table-hover" runat="server">
                    <tr>
                        <td colspan="2"><asp:Label ID="Status_Label" runat="server" style="text-align: center" CssClass="form-control" autosize="true" ></asp:Label></td>
                    </tr>
                    <tr ID="Company_Row" runat="server" visible="false">
                        <td>Company:</td>
                        <td><asp:Textbox ID="CompanyName_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                    </tr>
                    <tr>
                        <th colspan="2" style="text-align: center; font-weight: bold">
                            <asp:Label ID="ProcessingSectionTitle_Label" runat="server" Text="Processing" style="text-align: center"></asp:Label>
                        </th>
                    </tr>
                    <tr id="Assignee_Row" runat="server">
                        <td>Assignee:</td>
                        <td>
                            <asp:DropDownList ID="Assignees_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:CustomValidator ID="Assignees_DropDownList_Validator" runat="server" OnServerValidate="Assignees_DropDownList_Validator_ServerValidate" ></asp:CustomValidator>
                            <asp:HiddenField ID="AtEditPageLoad_AssigneeID_HiddenField" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                    <tr id="ICSRStatus_Row" runat="server">
                        <td>ICSR Status:</td>
                        <td>
                            <asp:DropDownList ID="ICSRStatuses_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:CustomValidator ID="ICSRStatuses_DropDownList_Validator" runat="server" OnServerValidate="ICSRStatuses_DropDownList_Validator_ServerValidate" ></asp:CustomValidator>
                            <asp:HiddenField ID="AtEditPageLoad_ICSRStatusID_HiddenField" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                    <tr>
                        <th colspan="2" style="text-align: center; font-weight: bold">
                            <asp:Label ID="PatientSectionTitle_Label" runat="server" Text="Patient Details" style="text-align: center"></asp:Label>
                        </th>
                    </tr>
                    <tr id="PatientInitials_Row" runat="server">
                        <td>Patient Initials:</td>
                        <td>
                            <asp:TextBox ID="PatientInitials_TextBox" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:CustomValidator ID="PatientInitials_TextBox_Validator" runat="server" OnServerValidate="PatientInitials_TextBox_Validator_ServerValidate" ></asp:CustomValidator>
                            <asp:HiddenField ID="AtEditPageLoad_PatientInitials_HiddenField" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                    <tr id="PatientYearOfBirth_Row" runat="server">
                        <td>Year of Birth:</td>
                        <td>
                            <asp:DropDownList ID="PatientYearOfBirth_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:CustomValidator ID="PatientYearOfBirth_DropDownList_Validator" runat="server" OnServerValidate="PatientYearOfBirth_DropDownList_Validator_ServerValidate" ></asp:CustomValidator>
                            <asp:HiddenField ID="AtEditPageLoad_PatientYearOfBirthID_HiddenField" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                    <tr id="PatientGender_Row" runat="server">
                        <td>Gender:</td>
                        <td>
                            <asp:DropDownList ID="PatientGender_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:CustomValidator ID="PatientGender_DropDownList_Validator" runat="server" OnServerValidate="PatientGender_DropDownList_Validator_ServerValidate" ></asp:CustomValidator>
                            <asp:HiddenField ID="AtEditPageLoad_PatientGenderID_HiddenField" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                    <tr>
                        <th colspan="2" style="text-align: center; font-weight: bold">
                            <asp:Label ID="SeriousnessSectionTitle_Label" runat="server" Text="Seriousness" style="text-align: center"></asp:Label>
                        </th>
                    </tr>
                    <tr id="IsSerious_Row" runat="server">
                        <td>Is serious:</td>
                        <td>
                            <asp:DropDownList ID="IsSerious_DropDownList" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                            <asp:CustomValidator ID="IsSerious_DropDownList_Validator" runat="server" OnServerValidate="IsSerious_DropDownList_Validator_ServerValidate" ></asp:CustomValidator>
                            <asp:HiddenField ID="AtEditPageLoad_IsSerious_HiddenField" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                    <tr id="SeriousnessCriterion_Row" runat="server">
                        <td>Seriousness Criterion:</td>
                        <td>
                            <asp:DropDownList ID="SeriousnessCriteria_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:CustomValidator ID="SeriousnessCriteria_DropDownList_Validator" runat="server" OnServerValidate="SeriousnessCriteria_DropDownList_Validator_ServerValidate" ></asp:CustomValidator>
                            <asp:HiddenField ID="AtEditPageLoad_SeriousnessCriterionID_HiddenField" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                    <tr>
                        <th colspan="2" style="text-align: center; font-weight: bold">
                            <asp:Label ID="NarrativeCompanyCommentSectionTitle_Label" runat="server" Text="Narrative & Company Comment" style="text-align: center"></asp:Label>
                        </th>
                    </tr>
                    <tr id="Narrative_Row" runat="server">
                        <td>Narrative:</td>
                        <td>
                            <asp:Textbox ID="Narrative_Textbox" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:Textbox>
                            <asp:CustomValidator ID="Narrative_Textbox_Validator" runat="server" OnServerValidate="Narrative_Textbox_Validator_ServerValidate" ></asp:CustomValidator>
                            <asp:HiddenField ID="AtEditPageLoad_Narrative_HiddenField" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                    <tr id="CompanyComment_Row" runat="server">
                        <td>Company Comment:</td>
                        <td>
                            <asp:Textbox ID="CompanyComment_Textbox" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:Textbox>
                            <asp:CustomValidator ID="CompanyComment_Textbox_Validator" runat="server" OnServerValidate="CompanyComment_Textbox_Validator_ServerValidate" ></asp:CustomValidator>
                            <asp:HiddenField ID="AtEditPageLoad_CompanyComment_HiddenField" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- <asp:HiddenField ID ="CompanyID_HiddenField" runat="server" /> -->
    <asp:HiddenField ID="ICSRID_HiddenField" runat="server" />
</asp:Content>

