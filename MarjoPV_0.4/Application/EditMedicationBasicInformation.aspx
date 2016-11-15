<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="EditMedicationBasicInformation.aspx.vb" Inherits="Application_EditMedicationBasicInformation" %>

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
                <asp:button ID="ReturnToMedicationOverview_Button" runat="server" class="btn btn-primary" Text="Medication Overview"></asp:button>
            </div>
        </div>
    </div>
    <div class="col-lg-12">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <table id="Main_Table" class="table table-responsive table-striped table-hover" runat="server">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Status_Label" runat="server" style="text-align: center" CssClass="form-control" />
                        </td>
                    </tr>
                    <tr id="Company_Row" runat="server" Visible="false">
                        <td>Company:</td>
                        <td>
                            <asp:TextBox ID="Company_Textbox" runat="server" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Generic Name:</td>
                        <td>
                            <asp:Textbox ID="Name_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                            <asp:CustomValidator ID="Name_Textbox_Validator" runat="server" OnServerValidate="Name_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            <asp:HiddenField ID="AtEditPageLoad_Name_HiddenField" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                    <tr>
                        <td>Type:</td>
                        <td>
                            <asp:DropDownList ID="MedicationTypes_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:CustomValidator ID="MedicationTypes_DropDownList_Validator" runat="server" OnServerValidate="MedicationTypes_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                            <asp:HiddenField ID="AtEditPageLoad_MedicationType_HiddenField" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                    <tr>
                        <td>Administration Route:</td>
                        <td>
                            <asp:DropDownList ID="AdministrationRoutes_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:CustomValidator ID="AdministrationRoutes_DropDownList_Validator" runat="server" OnServerValidate="AdministrationRoutes_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                            <asp:HiddenField ID="AtEditPageLoad_AdministrationRoute_HiddenField" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                    <tr>
                        <td>Dose Type:</td>
                        <td>
                            <asp:DropDownList ID="DoseTypes_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:CustomValidator ID="DoseTypes_DropDownList_Validator" runat="server" OnServerValidate="DoseTypes_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                            <asp:HiddenField ID="AtEditPageLoad_DoseType_HiddenField" runat="server"></asp:HiddenField>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:HiddenField ID="MedicationID_HiddenField" runat="server" />
</asp:Content>

