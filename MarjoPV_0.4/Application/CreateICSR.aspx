<%@ Page Title="Create ICSR" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="CreateICSR.aspx.vb" Inherits="Application_CreateICSR" %>

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
            <div id="ButtonGroup_Div" runat="server" class="panel btn-group">
                <asp:button ID="ConfirmICSRInput_Button" runat="server" class="btn btn-primary" Text="Confirm New ICSR Details"></asp:button>
                <asp:button ID="SaveUpdates_Button" runat="server" class="btn btn-primary" Text="Save New ICSR"></asp:button>
                <asp:button ID="Cancel_Button" runat="server" class="btn btn-primary" Text="Cancel"></asp:button>
                <asp:button ID="ReturnToICSROverview_Button" runat="server" class="btn btn-primary" Text="ICSR Overview"></asp:button>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <table id="Main_Table" runat="server" class="table table-responsive table-striped table-hover">
                        <tr>
                            <td colspan="2"><asp:Label ID="Status_Label" runat="server" style="text-align: center" CssClass="form-control"></asp:Label></td>
                        </tr>
                        <tr ID="Company_Row" runat="server">
                            <td>Company:</td>
                            <td>
                                <asp:DropDownList ID="Companies_DropDownList" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:CustomValidator ID="Companies_DropDownList_Validator" runat="server" OnServerValidate="Companies_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:HiddenField ID="ICSRID_HiddenField" runat="server" />
</asp:Content>

