<%@ Page Title="Account" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="Account.aspx.vb" Inherits="Application_Account" %>

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
            <div class="panel btn-group">
                <asp:button ID="SaveAccountUpdates_Button" runat="server" class="btn btn-primary" Text="Save Account Updates"></asp:button>
                <asp:button ID="ChangePassword_Button" runat="server" class="btn btn-primary" Text="Change Password"></asp:button>
                <asp:button ID="Cancel_Button" runat="server" class="btn btn-primary" Text="Cancel"></asp:button>
            </div>
        </div>
    </div>
    <div class="row">
            <div class="col-lg-12">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <table class="table table-responsive table-striped table-hover">
                            <tr>
                                <td colspan="2"><asp:Label ID="Status_Label" runat="server" style="text-align: center" CssClass="form-control"></asp:Label></td>
                            </tr>
                            <tr ID="Username_Row" runat="server">
                                <td>Username:</td>
                                <td>
                                    <asp:Textbox ID="Username_Textbox" runat="server"  CssClass="form-control"></asp:Textbox>
                                    <asp:CustomValidator ID="Username_Textbox_Validator" runat="server" OnServerValidate="Username_Textbox_Validator_ServerValidate" ></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr id="Name_Row" runat="server">
                                <td>Full Name:</td>
                                <td>
                                    <asp:Textbox ID="Name_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                                    <asp:CustomValidator ID="Name_Textbox_Validator" runat="server" OnServerValidate="Name_Textbox_Validator_ServerValidate" ></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr id="Phone_Row" runat="server">
                                <td>Phone:</td>
                                <td>
                                    <asp:Textbox ID="Phone_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                                    <asp:CustomValidator ID="Phone_Textbox_Validator" runat="server" OnServerValidate="Phone_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
</asp:Content>

