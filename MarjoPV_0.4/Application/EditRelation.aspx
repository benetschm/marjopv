<%@ Page Title="Edit Relation" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="EditRelation.aspx.vb" Inherits="Application_EditRelation" %>

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
                <asp:button ID="ConfirmDeletion_Button" runat="server" class="btn btn-danger" Text="Confirm Deletion" ></asp:button>
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
                       <tr ID="AE_Row" runat="server">
                            <td>Event:</td>
                            <td>
                                <asp:DropDownList ID="AEs_DropDownList" runat="server" CssClass="form-control" ></asp:DropDownList>
                                <asp:CustomValidator ID="AEs_DropDownList_Validator" runat="server" OnServerValidate="RelationCriteria_Validator_ServerValidate"></asp:CustomValidator>
                                <asp:HiddenField ID="AtEditPageLoad_AE_HiddenField" runat="server"></asp:HiddenField>
                            </td>
                        </tr>
                        <tr ID="ICSRMedication_Row" runat="server">
                            <td>Medication:</td>
                            <td>
                                <asp:DropDownList ID="MedicationsPerICSR_DropDownList" runat="server" CssClass="form-control" ></asp:DropDownList>
                                <asp:CustomValidator ID="MedicationsPerICSR_DropDownList_Validator" runat="server" OnServerValidate="RelationCriteria_Validator_ServerValidate"></asp:CustomValidator>
                                <asp:HiddenField ID="AtEditPageLoad_MedicationPerICSR_HiddenField" runat="server"></asp:HiddenField>
                            </td>
                        </tr> 
                        <tr ID="RelatednessReporter_Row" runat="server">
                            <td>Relatedness as per Reporter:</td>
                            <td>
                                <asp:DropDownList ID="RelatednessCriteriaReporter_DropDownList" runat="server" CssClass="form-control" ></asp:DropDownList>
                                <asp:CustomValidator ID="RelatednessCriteriaReporter_DropDownList_Validator" runat="server" OnServerValidate="RelatednessCriteriaReporter_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                                <asp:HiddenField ID="AtEditPageLoad_RelatednessCriterionReporter_HiddenField" runat="server"></asp:HiddenField>
                            </td>
                        </tr>
                        <tr ID="RelatednessManufacturer_Row" runat="server">
                            <td>Relatedness as per Manufacturer:</td>
                            <td>
                                <asp:DropDownList ID="RelatednessCriteriaManufacturer_DropDownList" runat="server" CssClass="form-control" ></asp:DropDownList>
                                <asp:CustomValidator ID="RelatednessCriteriaManufacturer_DropDownList_Validator" runat="server" OnServerValidate="RelatednessCriteriaManufacturer_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                                <asp:HiddenField ID="AtEditPageLoad_RelatednessCriteriaManufacturer_HiddenField" runat="server"></asp:HiddenField>
                            </td>
                        </tr>
                        <tr ID="Expectendess_Row" runat="server">
                            <td>Expectedness:</td>
                            <td>
                                <asp:DropDownList ID="ExpectendessCriteria_DropDownList" runat="server" CssClass="form-control" ></asp:DropDownList>
                                <asp:CustomValidator ID="ExpectendessCriteria_DropDownList_Validator" runat="server" OnServerValidate="ExpectendessCriteria_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                                <asp:HiddenField ID="AtEditPageLoad_ExpectendessCriterion_HiddenField" runat="server"></asp:HiddenField>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:HiddenField ID="RelationID_HiddenField" runat="server" />
    <asp:HiddenField ID="Delete_HiddenField" runat="server" />
    <asp:HiddenField ID="ICSRID_HiddenField" runat="server" />
</asp:Content>

