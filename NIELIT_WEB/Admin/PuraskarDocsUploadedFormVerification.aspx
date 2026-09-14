<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/PuraskarApp.master" AutoEventWireup="true" CodeFile="PuraskarDocsUploadedFormVerification.aspx.cs"
    Inherits="Admin_PuraskarDocsUploadedFormVerification" Debug="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Online Puraskar Application"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">

    <asp:Panel runat="server" ID="pnlFilter" Visible="true">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
            <div id="filterBox" align="left">
                <div id="filterPannel">
                    <asp:UpdatePanel EnableViewState="true" ID="filterPnal_upnlFilter" RenderMode="Inline"
                        UpdateMode="Conditional" runat="server">
                        <ContentTemplate>

                            <table cellpadding="0" id="body1" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblFiler" Width="60%" runat="server" Font-Bold="true" Font-Size="12pt"
                                                    Text="Filter Panel"></asp:Label>
                                                <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
                                                    Text="" OnClick="ResetFilterPanel" />
                                                <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                                    Text="" OnClick="AllyFilter" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="Label5" Width="100%" runat="server" Text="Verified Status"></asp:Label>
                                                <asp:DropDownList ID="ddlVerifiedStatus" runat="server" Width="100%" Visible="true">
                                                    <asp:ListItem Value="99">--Select One--</asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Verified"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="Rejected"></asp:ListItem>
                                                    <asp:ListItem Value="999" Text="Pending"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                 <tr >
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="Label6" Width="100%" runat="server" Text="Level"></asp:Label>
                                                <asp:DropDownList ID="drplvl" runat="server" Width="100%" Visible="true" AutoPostBack="true"
                                    OnSelectedIndexChanged="drplvl_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">--Select One--</asp:ListItem>
                                                    <asp:ListItem Value="1" Text="O Level"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="A Level"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="B Level"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="C Level"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                 <tr >
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="Label7" Width="100%" runat="server" Text="Reg No."></asp:Label>
                                              

                                                <asp:TextBox ID="txtRegNo" runat="server" 
                                    onkeypress="checkNumber(this,8,0,event);"
                                    Width="200px" MaxLength="8"></asp:TextBox>


                                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server"
                                    Enabled="True" TargetControlID="txtRegNo" WatermarkText="0" WatermarkCssClass="Watermark" >
                                </asp:TextBoxWatermarkExtender>
                                
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <script language="javascript" type="text/javascript">
                    var box = $('#filterBox');
                    shortcut.add("Ctrl+Shift+F", function () {
                        box.show();
                    });
                    shortcut.add("Esc", function () {
                        box.hide();
                    });
                   
                </script>
            </div>
        </div>
    </asp:Panel>
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Candidates Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"  visible="false"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {

        }
        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
    </script>
    <div id="divGrid" runat="server" visible="true">

        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>

                <br />
                <asp:Label ID="lblheading2" runat="server" ForeColor="blue" Font-Bold="true"
                    Text="Protsahan Puraskar Placement Candidates List for Verification by Finance" Visible="true"></asp:Label><br />
                <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                    runat="server"></asp:Label>
                <asp:GridView ID="gvMain" runat="server" DataKeyNames="Regno" OnSorting="gvMain_Sorting"
                    OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="600px" ShowHeader="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
                    <RowStyle Height="40px" />
                    <Columns>
                        <asp:BoundField HeaderStyle-Width="2%" HeaderText="SL">
                            <HeaderStyle Width="2%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>                      
                          <asp:HyperLinkField HeaderStyle-Width="16%"
                            DataTextField="RegLevel" HeaderText="Registration Number" SortExpression="Regno" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>                       
                        <asp:HyperLinkField HeaderStyle-Width="16%"
                            DataTextField="Name" HeaderText="Candidates Name" SortExpression="Name" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>                    

                         <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="CompanyDetails" HeaderText="CompanyDetails" SortExpression="CompanyDetails" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>

                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="PlacementDatesFromTo" HeaderText="PlacementDetails" SortExpression="PlacementDatesFromTo" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                      <%--  <asp:HyperLinkField HeaderStyle-Width="25%"
                            DataTextField="DoB" HeaderText="DOB" SortExpression="DOB"
                            Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Width="25%" />
                        </asp:HyperLinkField>--%>
                        <asp:HyperLinkField HeaderStyle-Width="15%"
                            DataTextField="Salary" HeaderText="Salary" SortExpression="Salary" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                         <asp:TemplateField HeaderText="WithHeld" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("isWithHeldFinance") %>' Width="100px"></asp:Label>

                                <asp:DropDownList ID="ddlisWithHeldFinance" runat="server" SkinID="ddl150" Visible="false" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlisWithHeldFinance_SelectedIndexChanged">
                                    <asp:ListItem Value="-1" Text="--Select One--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="YES"></asp:ListItem>
                                    <asp:ListItem Value="0" selected="true" Text="NO"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="RESOLVED"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("withHeldReason") %>' Width="110px" Style="word-wrap: break-word;" ForeColor="#ff6600"></asp:Label>
                                <asp:TextBox ID="txtwithHeldReason" runat="server" Text='<%# Eval("withHeldReason") %>'
                                    Width="100px" Visible="false" MaxLength="200" TextMode="MultiLine" ForeColor="Red"></asp:TextBox>
                                <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                                    Enabled="True" TargetControlID="txtwithHeldReason" WatermarkText="**Required" WatermarkCssClass="Watermark" >
                                </asp:TextBoxWatermarkExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtwithHeldReason"
                                    ErrorMessage="**required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="VerifiedStatus" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="110px">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("VerifiedStatus") %>' Width="150px"></asp:Label>

                                <asp:DropDownList ID="ddlVerifieds" runat="server" SkinID="ddl150" Visible="false" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlVerifieds_SelectedIndexChanged">
                                    <asp:ListItem Value="-1" Text="--Select One--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Verified"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Rejected"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("financeRejectionReason") %>' Width="150px" Style="word-wrap: break-word;" ForeColor="#ff6600"></asp:Label>
                                <asp:TextBox ID="txtfinanceRejectionReason" runat="server" Text='<%# Eval("financeRejectionReason") %>'
                                    Width="130px" Visible="false" MaxLength="200" TextMode="MultiLine" ForeColor="Red"></asp:TextBox>
                                <asp:TextBoxWatermarkExtender ID="txtName_TextBoxWatermarkExtender" runat="server"
                                    Enabled="True" TargetControlID="txtfinanceRejectionReason" WatermarkText="*Enter Reason for Rejection" WatermarkCssClass="Watermark">
                                </asp:TextBoxWatermarkExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtfinanceRejectionReason"
                                    ErrorMessage="**required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                           <asp:TemplateField Visible="false" HeaderText="lblIdVisFalse">
                            <ItemTemplate>
                                <asp:Label runat="server" Visible="true" ID="lblExamid" Text='<%# Eval("Examid") %>'></asp:Label>
                                <asp:Label runat="server" Visible="true" ID="lblRegno" Text='<%# Eval("Regno") %>'></asp:Label>                              
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" HeaderText="lblIdsVisFalse">
                            <ItemTemplate>                                
                                <asp:Label runat="server" Visible="true" ID="lblCourseId" Text='<%# Eval("CourseID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:ButtonField  Text="View" CommandName="Select" ItemStyle-Width="30"  />
                    </Columns>
                    <SelectedRowStyle BackColor="#87CEFA" ForeColor="Maroon" Font-Size="10" />
                    <PagerSettings Visible="False" />
                </asp:GridView>

                <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                <asp:HiddenField ID="hfcode" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divNavigation" runat="server">
        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="msg">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="false"></asp:Label>
                <asp:HiddenField ID="hcentreID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div style="text-align: right; margin-top: 10px; height: 80px">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnSave" runat="server" OnClick="SaveRecord"
                    Text="Save" Visible="false" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="false" />
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Visible="false" />
                
            </ContentTemplate></asp:UpdatePanel>
    </div>
   
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr>
            <td></td>
        </tr>
    </table>
</asp:Content>