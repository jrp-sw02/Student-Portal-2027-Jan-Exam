<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="BulkInsttWithdrawal.aspx.cs" Inherits="BulkInsttWithdrawal" 
    Culture="auto" UICulture="auto" %>

<%@ Register Src="~/usercontrol/sidelink.ascx" TagPrefix="uc" TagName="SideLink" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Institutes List Upload" meta:resourcekey="lblHeadingResource1"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateLogin() {

            return true;
        }

        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
       
    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="New" runat="server">
            <table class="sample2" cellpadding="2" cellspacing="0">
                <tr class="even">
                    <td colspan="3">
                        <table width="100%">
                            <tr>
                                <td>
                                    <a href="../download/WithdrawalFormat.xlsx" target="_blank">
                                        <img src="../images/Export_Exl.jpg" />Click  to Download Format file</a>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Style="display: inline;"
                                        Text="Please upload the data as per format file" meta:resourcekey="Label4Resource1"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                   <td>
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="<b>Select File</b>" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                   <td>
                        <asp:FileUpload ID="fuInstitituteList" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align: right">
                        <asp:Button ID="btnUpload" runat="server" Text="Upload Withdrawal File" OnClick="btnUpload_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr class="even">
                    <td colspan="3" valign="top">
                        <asp:Label ID="lblCount" runat="server" SkinID="CaptionLabel" Text="" meta:resourcekey="Label4Resource1"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td colspan="3" valign="top">
                         <div id="divGrid" runat="server" visible="true">

        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>

                        <asp:GridView ID="gvMain" runat ="server" AutoGenerateColumns ="false" Width="521px">
                            <Columns>
        <asp:BoundField DataField="ccc_No" HeaderText="CCC No" ItemStyle-Width="150px" >
                                <ItemStyle Width="150px" />
                                </asp:BoundField>
        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="100px" >
                                <ItemStyle Width="100px" />
                                </asp:BoundField>
        <asp:BoundField DataField="remarks" HeaderText="Remarks" ItemStyle-Width="100px" >
                                <ItemStyle Width="200px" />
                                </asp:BoundField>
    </Columns>
</asp:GridView>
                 <asp:HiddenField ID="hfFileName" runat="server" Value="" />
                </ContentTemplate> 
            </asp:UpdatePanel>
                             </div>
                         <div id="divNavigation" runat="server">
        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" Visible="True" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
                    </td>

                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
    <uc:SideLink runat="server" ID="ucSideLink" />
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
