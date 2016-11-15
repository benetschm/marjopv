<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="Role.aspx.vb" Inherits="Administration_Role" %>

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
            <div class="panel btn-group">
                <asp:button ID="EditRoleDetails_Button" runat="server" class="btn btn-primary" Text="Edit Role Details"></asp:button>
                <asp:button ID="DeleteRole_Button" runat="server" class="btn btn-primary" Text="Permanently Delete Role"></asp:button>
                <asp:button ID="SaveRoleUpdates_Button" runat="server" class="btn btn-primary" Text="Save Role Updates"></asp:button>
                <asp:button ID="ConfirmRoleDeletion_Button" runat="server" class="btn btn-primary" Text="Confirm Role Deletion"></asp:button>
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
                        <tr ID="Name_Row" runat="server">
                            <td>Name:</td>
                            <td>
                                <asp:Textbox ID="Name_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                                <asp:CustomValidator ID="Name_Textbox_Validator" runat="server" OnServerValidate="Name_Textbox_Validator_ServerValidate" ></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr ID="Description_Row" runat="server">
                            <td>Description:</td>
                            <td>
                                <asp:Textbox ID="Description_Textbox" runat="server" CssClass="form-control"></asp:Textbox>
                                <asp:CustomValidator ID="Description_Textbox_Validator" runat="server" OnServerValidate="Description_Textbox_Validator_ServerValidate" ></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr id="CanViewICSRs_Row" runat="server">
                            <td>Can View ICSRs:</td>
                            <td>
                                <asp:DropDownList ID="CanViewICSRs_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem>False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanCreateICSRs_Row" runat="server">
                            <td>Can Create ICSRs:</td>
                            <td>
                                <asp:DropDownList ID="CanCreateICSRs_Dropdownlist" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem>False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanEditICSRs_Row" runat="server">
                            <td>Can Edit ICSRs:</td>
                            <td>
                                <asp:DropDownList ID="CanEditICSRs_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanEditICSRAssignee_Row" runat="server">
                            <td>Can Edit ICSR Assignee:</td>
                            <td>
                                <asp:DropDownList ID="CanEditICSRAssignee_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanEditICSRSeriousness_Row" runat="server">
                            <td>Can Edit ICSR Seriousness:</td>
                            <td>
                                <asp:DropDownList ID="CanEditICSRSeriousness_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanEditICSRCompanyComment_Row" runat="server">
                            <td>Can Edit ICSR Company Comment:</td>
                            <td>
                                <asp:DropDownList ID="CanEditICSRCompanyComment_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanEditReportComplexity_Row" runat="server">
                            <td>Can Edit Report Complexity:</td>
                            <td>
                                <asp:DropDownList ID="CanEditReportComplexity_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanEditReportType_Row" runat="server">
                            <td>Can Edit Report Type:</td>
                            <td>
                                <asp:DropDownList ID="CanEditReportType_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanEditReportStatus_Row" runat="server">
                            <td>Can Edit Report Status:</td>
                            <td>
                                <asp:DropDownList ID="CanEditReportStatus_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanEditReportDue_Row" runat="server">
                            <td>Can Edit Report Due Date:</td>
                            <td>
                                <asp:DropDownList ID="CanEditReportDue_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanEditReportExpeditedReportingRequired_Row" runat="server">
                            <td>Can Edit Report Expedited Reporting Required:</td>
                            <td>
                                <asp:DropDownList ID="CanEditReportExpeditedReportingRequired_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanEditReportExpeditedReportingDone_Row" runat="server">
                            <td>Can Edit Report Expedited Reporting Done:</td>
                            <td>
                                <asp:DropDownList ID="CanEditReportExpeditedReportingDone_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanEditReportExpeditedReportingDate_Row" runat="server">
                            <td>Can Edit Report Expedited Reporting Date:</td>
                            <td>
                                <asp:DropDownList ID="CanEditReportExpeditedReportingDate_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="CanEditRelations_Row" runat="server">
                            <td>Can Edit Relations:</td>
                            <td>
                                <asp:DropDownList ID="CanEditRelations_DropDownList" runat="server" CssClass="form-control">
                                    <asp:ListItem>True</asp:ListItem>
                                    <asp:ListItem Selected="True">False</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

