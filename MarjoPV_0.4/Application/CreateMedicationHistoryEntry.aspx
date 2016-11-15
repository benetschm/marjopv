<%@ Page Title="Add Medication History Entry" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="CreateMedicationHistoryEntry.aspx.vb" Inherits="Application_CreateMedicationHistoryEntry" %>

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
                        <tr ID="MedicationHistoryEntry_Row" runat="server">
                            <td>Medication History Entry:</td>
                            <td>
                                <asp:Textbox ID="MedicationHistoryEntry_Textbox" runat="server" CssClass="form-control" ></asp:Textbox>
                                <asp:CustomValidator ID="MedicationHistoryEntry_Textbox_Validator" runat="server" OnServerValidate="MedicationHistoryEntry_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:HiddenField ID="MedicationID_HiddenField" runat="server" />
</asp:Content>

