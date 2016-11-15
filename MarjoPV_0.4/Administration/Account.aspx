<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="Account.aspx.vb" Inherits="Administration_Account" %>

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
                <asp:button ID="Cancel_Button" runat="server" class="btn btn-primary" Text="Cancel"></asp:button>
            </div>
        </div>
    </div>
     <div class="row">
        <div class="col-lg-12">
            <asp:UpdatePanel ID="Main_Table" runat="server">
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
                        <tr id="Active_Row" runat="server">
                            <td>Active:</td>
                            <td>
                                <asp:DropDownList ID="Active_Dropdownlist" runat="server" cssclass="form-control">
                                    <asp:ListItem Value=True>True</asp:ListItem>
                                    <asp:ListItem Value=False>False</asp:ListItem>
                                </asp:DropDownList>
                                <asp:CustomValidator ID="Active_Dropdownlist_Validator" runat="server" OnServerValidate="Active_Dropdownlist_Validator_ServerValidate" ></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr id="CanViewCompanies_Row" runat="server">
                            <td>Can View Companies:</td>
                            <td>
                                <asp:DropDownList ID="CanViewCompanies_Dropdownlist" runat="server" cssclass="form-control">
                                    <asp:ListItem Value=True>True</asp:ListItem>
                                    <asp:ListItem Value=False>False</asp:ListItem>
                                </asp:DropDownList>
                                <asp:CustomValidator ID="CanViewCompanies_Dropdownlist_Validator" runat="server" OnServerValidate="CanViewCompanies_Dropdownlist_Validator_ServerValidate" ></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr id="Admin_Row" runat="server">
                            <td>Administrator:</td>
                            <td>
                                <asp:DropDownList ID="Admin_DropDownList" runat="server" cssclass="form-control">
                                    <asp:ListItem Value=True>True</asp:ListItem>
                                    <asp:ListItem Value=False>False</asp:ListItem>
                                </asp:DropDownList>
                                <asp:CustomValidator ID="Admin_DropDownList_Validator" runat="server" OnServerValidate="Admin_DropDownList_Validator_ServerValidate" ></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

