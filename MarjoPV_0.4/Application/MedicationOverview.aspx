<%@ Page Title="Medication Overview" Language="VB" MasterPageFile="~/MasterPages/SiteMaster.master" AutoEventWireup="false" CodeFile="MedicationOverview.aspx.vb" Inherits="Application_MedicationOverview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="col-lg-12">
        <h3><asp:Label ID="Title_Label" runat="server"></asp:Label></h3>
    </div>
    <div id="Content_Tabs" class="row" runat="server">
        <div class="col-lg-12">
            <div class="panel" id="Tabs" role="tabpanel">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs nav-justified" role="tablist">
                    <li>
                        <a  href="#Tab1" aria-controls="Tab1" role="tab" data-toggle="tab">
                            Basic Information
                        </a>
                    </li>
                    <li>
                        <a href="#Tab2" aria-controls="Tab2" role="tab" data-toggle="tab">
                            Medication Ingredients
                        </a>
                    </li>
                    <li>
                        <a href="#Tab3" aria-controls="Tab3" role="tab" data-toggle="tab">
                            Marketing Countries
                        </a>
                    </li>
                    <li>
                        <a href="#Tab4" aria-controls="Tab4" role="tab" data-toggle="tab">
                            Change History
                        </a>
                    </li>
                    <li>
                        <a href="#Tab5" aria-controls="Tab5" role="tab" data-toggle="tab">
                            Attached Files
                        </a>
                    </li>
                </ul>
                <!-- Tab panes -->
                <div class="tab-content Tab_Border">
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab1">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="panel btn-group">
                                                        <asp:button ID="EditMedicationbBasicInformation_Button" runat="server" class="btn btn-primary" Text="Edit Basic Information"></asp:button>
                                                    </div>
                                                </div>
                                            </div>
                                            <table class="table table-responsive table-striped table-hover" runat="server">
                                                <tr>
                                                    <td>ID:</td>
                                                    <td><asp:Textbox ID="ID_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                                <tr id="Company_Row" runat="server"  Visible="false">
                                                    <td>Company:</td>
                                                    <td><asp:Textbox ID="CompanyName_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <td>Generic Name:</td>
                                                    <td><asp:Textbox ID="Name_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <td>Type:</td>
                                                    <td><asp:Textbox ID="MedicationType_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <td>Administration Route:</td>
                                                    <td><asp:Textbox ID="AdministrationRoute_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <td>Dose Type:</td>
                                                    <td><asp:Textbox ID="DoseType_Textbox" runat="server" CssClass="form-control"></asp:Textbox></td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab2">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="MedicationIngredients_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <div class="panel btn-group">
                                                                <asp:button ID="AddMedicationIngredient_Button" runat="server" class="btn btn-primary" OnClick="AddMedicationIngredient_Button_Click" Text="Add Medication Ingredient"></asp:button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-responsive table-striped table-hover">
                                                        <tr id="MedicationIngredientHeader_Row" runat="server" visible="true">
                                                            <th id="MedicationIngredientTitle" style="text-align: center; vertical-align: middle">
                                                                <asp:Label ID="MedicationIngredient_Label" runat="server" Text='<%# "Medication Ingredient ID " & Eval("MedicationIngredient_ID") %>'></asp:Label>
                                                            </th>
                                                            <th id="MedicationIngredient_Buttons_Header" runat="server" style="text-align: center; vertical-align: middle">
                                                                <div class="btn-group">
                                                                    <asp:Button id="EditMedicationIngredient_Button" runat="server" Text="Edit" CssClass="btn btn-sm btn-primary" OnClick="EditMedicationIngredient_Button_Click" CommandArgument='<%# Eval("MedicationIngredient_ID") %>'/>
                                                                    <asp:Button id="DeleteMedicationIngredient_Button" runat="server" Text="Delete" CssClass="btn btn-sm btn-primary" OnClick="DeleteMedicationIngredient_Button_Click" CommandArgument='<%# Eval("MedicationIngredient_ID") %>'/>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td>Name:</td>
                                                            <td><asp:TextBox ID="MedicationIngredientName_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("MedicationIngredient_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Quantity:</td>
                                                            <td><asp:TextBox ID="MedicationIngredientQuantity_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("MedicationIngredient_Quantity") %>'></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Unit:</td>
                                                            <td><asp:TextBox ID="MedicationIngredientUnit_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("MedicationIngredientUnit_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab3">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="MarketingCountries_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <div class="panel btn-group">
                                                                <asp:button ID="AddMarketingCountry_Button" runat="server" class="btn btn-primary" OnClick="AddMarketingCountry_Button_Click" Text="Add Marketing Country"></asp:button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-responsive table-striped table-hover">
                                                        <tr id="MarketingCountryHeader_Row" runat="server" visible="true">
                                                            <th id="MarketingCountryTitle" style="text-align: center; vertical-align: middle">
                                                                <asp:Label ID="MarketingCountry_Label" runat="server" Text='<%# "Marketing Country ID " & Eval("MarketingCountry_ID") %>'></asp:Label>
                                                            </th>
                                                            <th id="MarketingCountry_Buttons_Header" runat="server" style="text-align: center; vertical-align: middle">
                                                                <div class="btn-group">
                                                                    <asp:Button id="EditMarketingCountry_Button" runat="server" Text="Edit" CssClass="btn btn-sm btn-primary" OnClick="EditMarketingCountry_Button_Click" CommandArgument='<%# Eval("MarketingCountry_ID") %>'/>
                                                                    <asp:Button id="DeleteMarketingCountry_Button" runat="server" Text="Delete" CssClass="btn btn-sm btn-primary" OnClick="DeleteMarketingCountry_Button_Click" CommandArgument='<%# Eval("MarketingCountry_ID") %>'/>
                                                            </th>
                                                        </tr>
                                                        <tr>
                                                            <td>Country:</td>
                                                            <td><asp:TextBox ID="MarketingCountryName_TextBox" runat="server" CssClass="form-control" Text='<%# Eval("MarketingCountry_Name") %>'></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab4">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">  
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:Repeater ID="ChangeHistory_Repeater" runat="server">
                                                <HeaderTemplate>
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <div class="panel btn-group">
                                                                <asp:button ID="CreateMedicationHistoryEntryButton" runat="server" 
                                                                   OnClick="CreateMedicationHistoryEntry_Button_Click" class="btn btn-primary" Text="Add Change History Entry"></asp:button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-responsive table-striped table-hover">
                                                        <tr>
                                                            <th style="text-align: center"><asp:Label ID="ChangeHistoryEntryTitle_Label" runat="server" Text='<%# "Update made by " + Eval("User_Name") + " on " + Eval("Timepoint", "{0:dd-MMM-yyyy}") + " at " + Eval("Timepoint", "{0:HH:mm}") + " (UTC " + Eval("Timepoint", "{0:zzz}") + ")" %>'></asp:Label></th>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Literal ID="Entry_TextBox" runat="server" Text='<%# Eval("Entry") %>'></asp:Literal></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane Tab_Body active" id="Tab5">
                        <div class="panel Tab_Body_Panel">  
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Repeater ID="AttachedFiles_Repeater" runat="server">
                                        <HeaderTemplate>
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="panel btn-group">
                                                        <asp:button ID="AttachFile_Button" runat="server" class="btn btn-primary" Text="Attach File" OnClick="AttachFile_Button_Click"></asp:button>
                                                    </div>
                                                </div>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table class="table table-responsive table-striped table-hover">
                                                <tr id="AttachedFileHeader_Row" runat="server" visible="true">
                                                    <th id="AttachedFileTitle" style="text-align: center; vertical-align: middle">
                                                        <asp:Label ID="AttachedFileTitle_Label" runat="server" Text='<%# "Attached File ID " + SqlIntDisplay(Eval("AttachedFile_ID")) %>'></asp:Label>
                                                    </th>
                                                    <th id="AttachedFile_Buttons_Header" runat="server" style="text-align: center; vertical-align: middle">
                                                        <div class="btn-group">
                                                            <asp:Button ID="DownloadAttachedFile_Button" runat="server" Text="Download" CssClass="btn btn-sm btn-primary" OnClick="DownloadAttachedFile_Button_Click" CommandArgument='<%# " ~/AttachedFiles/" + Convert.ToString(Eval("Company_ID")) + "/" + [Enum].GetName(GetType(tables), tables.Medications) + "/" + Convert.ToString(Eval("Association_ID")) + "/" + Convert.ToString(Eval("GUID")) + Convert.ToString(Eval("Extension")) %>' ></asp:Button>
                                                            <asp:Button id="DeleteAttachedFile_Button" runat="server" Text="Delete" CssClass="btn btn-sm btn-primary" OnClick="DeleteAttachedFile_Button_Click" />
                                                            <asp:Button id="ConfirmDeleteAttachedFile_Button" runat="server" Text="Confirm Deletion" CssClass="btn btn-sm btn-primary btn-danger" OnClick="ConfirmDeleteAttachedFile_Button_Click" CommandArgument='<%# " ~/Attached_Files/" + Convert.ToString(Eval("Company_ID")) + "/" + Convert.ToString(Eval("Association_ID")) + "/" + Convert.ToString(Eval("GUID")) + Convert.ToString(Eval("Extension")) %>' Visible="false"/>
                                                        </div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>File Name:</td>
                                                    <td><asp:Textbox ID="FileName_Textbox" runat="server" CssClass="form-control" Text='<%# Eval("Name") %>' ></asp:Textbox></td>
                                                </tr>
                                                <tr>
                                                    <td>Date Added:</td>
                                                    <td><asp:Textbox ID="DateAdded_Textbox" runat="server" CssClass="form-control" Text='<%# Eval("Added", "{0:dd-MMM-yyyy}") %>' ></asp:Textbox></td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="TabName" runat="server" />
    <asp:HiddenField ID="MedicationID_HiddenField" runat="server" />
</asp:Content>

