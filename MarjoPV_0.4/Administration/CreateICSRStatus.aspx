<%@ Page Title="Administration" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="CreateICSRStatus.aspx.vb" Inherits="Administration_CreateICSRStatus" %>

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
            <div id="Buttons" class="panel btn-group" runat="server">
                <asp:button ID="SaveUpdates_Button" runat="server" class="btn btn-primary" Text="Save New ICSR Status"></asp:button>
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
                        <tr ID="Name_Row" runat="server">
                            <td>Name:</td>
                            <td>
                                <asp:Textbox ID="Name_Textbox" runat="server"  CssClass="form-control"></asp:Textbox>
                                <asp:CustomValidator ID="Name_Textbox_Validator" runat="server" OnServerValidate="Name_Textbox_Validator_ServerValidate" ></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr id="IsStatusNew_Row" runat="server">
                            <td>Is Status New:</td>
                            <td>
                                <asp:DropDownList ID="IsStatusNew_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                                <asp:CustomValidator ID="IsStatusNew_DropDownList_CustomValidator" runat="server" OnServerValidate="IsStatusNew_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr id="IsStatusClosed_Row" runat="server">
                            <td>Is Status Closed:</td>
                            <td>
                                <asp:DropDownList ID="IsStatusClosed_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                                <asp:CustomValidator ID="IsStatusClosed_DropDownList_Validator" runat="server" OnServerValidate="IsStatusClosed_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="SortOrder_Row" runat="server">
                            <td>Sort Order:</td>
                            <td>
                                <asp:Textbox ID="SortOrder_Textbox" runat="server"  CssClass="form-control"></asp:Textbox>
                                <asp:CustomValidator ID="SortOrder_Textbox_Validator" runat="server" OnServerValidate="SortOrder_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr id="Active_Row" runat="server">
                            <td>Active:</td>
                            <td>
                                <asp:DropDownList ID="Active_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem Selected="True">True</asp:ListItem>
                                    <asp:ListItem>False</asp:ListItem>
                                </asp:DropDownList>
                                <asp:CustomValidator ID="Active_DropDownList_Validator" runat="server" OnServerValidate="Active_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

