<%@ Page Title="Add Medication to ICSR" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="CreateICSRMedication.aspx.vb" Inherits="Application_CreateICSRMedication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <asp:Label ID="Title_Label" runat="server" CssClass="h3"></asp:Label>
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
    <div class="row">
        <div class="col-lg-12">
            <asp:UpdatePanel id="Main_Table" runat="server">
                <ContentTemplate>
                    <table class="table table-responsive table-striped table-hover">
                        <tr>
                            <td colspan="2"><asp:Label ID="Status_Label" runat="server" style="text-align: center" CssClass="form-control"></asp:Label></td>
                        </tr>
                        <tr ID="MedicationRole_Row" runat="server">
                            <td>Role in ICSR:</td>
                            <td>
                                <asp:DropDownList ID="MedicationPerICSRRoles_DropDownList" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="Medications_DropDownList_SelectedIndexChanged"></asp:DropDownList>
                                <asp:CustomValidator ID="MedicationPerICSRRoles_DropDownList_Validator" runat="server" OnServerValidate="MedicationPerICSRRoles_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="MedicationName_Row" runat="server">
                            <td>Medication Name:</td>
                            <td>
                                <asp:DropDownList ID="Medications_DropDownList" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="Medications_DropDownList_SelectedIndexChanged"></asp:DropDownList>
                                <asp:CustomValidator ID="Medications_DropDownList_Validator" runat="server" OnServerValidate="Medications_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="TotalDailyDose_Row" runat="server">
                            <td><asp:Label ID="TotalDailyDose_Label" runat="server" Text="Total Daily Dose:"></asp:Label></td>
                            <td>
                                <asp:Textbox ID="TotalDailyDose_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                                <asp:CustomValidator ID="TotalDailyDose_Textbox_Validator" runat="server" OnServerValidate="TotalDailyDose_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="Allocations_Row" runat="server">
                            <td>Allocations per Day:</td>
                            <td>
                                <asp:Textbox ID="Allocations_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                                <asp:CustomValidator ID="Allocations_Textbox_Validator" runat="server" OnServerValidate="Allocations_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="Start_Row" runat="server">
                            <td>Start Date:</td>
                            <td>
                                <asp:Textbox ID="Start_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                                <asp:CustomValidator ID="Start_Textbox_Validator" runat="server" OnServerValidate="Start_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="Stop_Row" runat="server">
                            <td>Stop Date:</td>
                            <td>
                                <asp:Textbox ID="Stop_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                                <asp:CustomValidator ID="Stop_Textbox_Validator" runat="server" OnServerValidate="Stop_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="DrugAction_Row" runat="server">
                            <td>Action taken with Drug:</td>
                            <td>
                                <asp:DropDownList ID="DrugActions_DropDownList" runat="server" CssClass="form-control" ></asp:DropDownList>
                                <asp:CustomValidator ID="DrugActions_DropDownList_Validator" runat="server" OnServerValidate="DrugActions_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:HiddenField ID="ICSRID_HiddenField" runat="server" />
</asp:Content>

