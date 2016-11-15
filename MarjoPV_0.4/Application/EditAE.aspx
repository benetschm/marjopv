<%@ Page Title="Edit Event" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="EditAE.aspx.vb" Inherits="Application_EditAE" %>

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
                        <tr ID="MedDRATerm_Row" runat="server">
                            <td>MedDRA LLT:</td>
                            <td>
                                <asp:Textbox ID="MedDRATerm_Textbox" runat="server" CssClass="form-control" ></asp:Textbox>
                                <asp:CustomValidator ID="MedDRATerm_Textbox_Validator" runat="server" OnServerValidate="MedDRATerm_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                                <asp:HiddenField ID="AtEditPageLoad_MedDRATerm_HiddenField" runat="server"></asp:HiddenField>
                            </td>
                        </tr>
                        <tr ID="Start_Row" runat="server">
                            <td>Start Date:</td>
                            <td>
                                <asp:Textbox ID="Start_Textbox" runat="server" CssClass="form-control" ></asp:Textbox>
                                <asp:CustomValidator ID="Start_Textbox_Validator" runat="server" OnServerValidate="Start_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                                <asp:HiddenField ID="AtEditPageLoad_Start_HiddenField" runat="server"></asp:HiddenField>
                            </td>
                        </tr>
                        <tr ID="Stop_Row" runat="server">
                            <td>Stop Date:</td>
                            <td>
                                <asp:Textbox ID="Stop_Textbox" runat="server" CssClass="form-control" ></asp:Textbox>
                                <asp:CustomValidator ID="Stop_Textbox_Validator" runat="server" OnServerValidate="Stop_Textbox_Validator_ServerValidate"></asp:CustomValidator>
                                <asp:HiddenField ID="AtEditPageLoad_Stop_HiddenField" runat="server"></asp:HiddenField>
                            </td>
                        </tr>
                        <tr ID="Outcome_Row" runat="server">
                            <td>Outcome:</td>
                            <td>
                                <asp:DropDownList ID="Outcomes_DropDownList" runat="server" CssClass="form-control" ></asp:DropDownList>
                                <asp:CustomValidator ID="Outcomes_DropDownList_Validator" runat="server" OnServerValidate="Outcomes_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                                <asp:HiddenField ID="AtEditPageLoad_Outcome_HiddenField" runat="server"></asp:HiddenField>
                            </td>
                        </tr>
                        <tr ID="DechallengeResult_Row" runat="server">
                            <td>Dechallenge Result:</td>
                            <td>
                                <asp:DropDownList ID="DechallengeResults_DropDownList" runat="server" CssClass="form-control" ></asp:DropDownList>
                                <asp:CustomValidator ID="DechallengeResults_DropDownList_Validator" runat="server" OnServerValidate="DechallengeResults_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                                <asp:HiddenField ID="AtEditPageLoad_DechallengeResult_HiddenField" runat="server"></asp:HiddenField>
                            </td>
                        </tr>
                        <tr ID="RechallengeResult_Row" runat="server">
                            <td>Rechallenge Result:</td>
                            <td>
                                <asp:DropDownList ID="RechallengeResults_DropDownList" runat="server" CssClass="form-control" ></asp:DropDownList>
                                <asp:CustomValidator ID="RechallengeResults_DropDownList_Validator" runat="server" OnServerValidate="RechallengeResults_DropDownList_Validator_ServerValidate"></asp:CustomValidator>
                                <asp:HiddenField ID="AtEditPageLoad_RechallengeResult_HiddenField" runat="server"></asp:HiddenField>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:HiddenField ID="AEID_HiddenField" runat="server" />
    <asp:HiddenField ID="Delete_HiddenField" runat="server" />
    <asp:HiddenField ID="ICSRID_HiddenField" runat="server" />
</asp:Content>

