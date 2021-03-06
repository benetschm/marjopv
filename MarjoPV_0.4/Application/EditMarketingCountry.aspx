﻿<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="EditMarketingCountry.aspx.vb" Inherits="Application_EditMarketingCountry" %>

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
                <asp:button ID="ReturnToMedicationOverview_Button" runat="server" class="btn btn-primary" Text="Medication Overview"></asp:button>
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
                        <tr ID="Name_Row" runat="server">
                            <td>Name:</td>
                            <td>
                                <asp:DropDownList ID="Countries_DropDownList" runat="server" CssClass="form-control" ></asp:DropDownList>
                                <asp:CustomValidator ID="Countries_DropDownList_Validator" runat="server" OnServerValidate="Countries_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                                <asp:HiddenField ID="AtEditPageLoad_Countries_HiddenField" runat="server"></asp:HiddenField>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:HiddenField ID="MedicationInCountryID_HiddenField" runat="server" />
    <asp:HiddenField ID="Delete_HiddenField" runat="server" />
    <asp:HiddenField ID="MedicationID_HiddenField" runat="server" />
</asp:Content>

