<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="frmexamhistory.aspx.cs" Inherits="frmexamhistory"%>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc3" %>
<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Exam Details"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <div id="div_O" class="summary_block" runat="server">
        <span>Exam Details</span>
        <table align="center" width="100%" cellpadding="2" cellspacing="1">
            <tr>
                <td width="25%" valign="top">
                    Examination Form Status
                </td>
                <td align="left" id="tdexamformstatus" runat="server">
                </td>
            </tr>
            <tr>
                <td valign="top">
                   Payment Status
                </td>
                <td align="left" id="tdpaymentstatus" runat="server">
                </td>
            </tr>
            <tr>
                <td valign="top">
                    Time Table Status
                </td>
                <td align="left" id="tdtimetable" runat="server">
                </td>
            </tr>
            <tr>
                <td valign="top">
                    Admit Card Status
                     
                </td>
                <td align="left" id="tdadmitcard" runat="server">
               
                </td>
            </tr>
            <tr>
                <td valign="top">
                   Practical Admit Card Status
                </td>
                <td align="left" id="tdpracticaladmitcard" runat="server">
                </td>
            </tr>
            <tr>
                <td valign="top">
                    Result Status
                </td>
                <td align="left" id="tdResult" runat="server">
                </td>
            </tr>
            <tr id= "trpayment" runat="server" visible="false">
                <td valign="top" colspan="2">
                    <asp:Label Visible="false" ID="Lbpaymentsource" runat="server" Text="" Style="font-size: small;
                        color: Red;"></asp:Label>
                    <asp:LinkButton Visible="false" ID="Lnkpaymentsourcechange" runat="server" OnClick="Lnkpaymentsourcechange_Click"> Change Payment Option</asp:LinkButton>
                        <br />
                    <asp:Label ID="Lbappchange" Visible="false" runat="server" Text="" Style="font-size: small;
                        color: Red;"></asp:Label>
                    <asp:LinkButton ID="Lnkappchange" Visible ="false" runat="server" onclick="Lnkappchange_Click"> 
                    Cancel / Edit Application</asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <div style="margin-top: 10px" id="divradio" visible="false" runat="server" class="error">
        <table cellpadding="3" cellspacing="0" width="100%">
            <tr class="gdalternate1">
                <td valign="top">
                    <asp:Label ID="lbfilter" runat="server" Text="" Style="text-align: right;"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td align="right">
                    <asp:Button ID="Btnyes" runat="server" Text="Yes" onclick="Btnyes_Click" />
                    <asp:Button ID="Btnno" runat="server" Text="No" OnClick="Btnno_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div style="margin-top: 10px" id="divradio1" visible="false" runat="server" class="error">
        <table cellpadding="3" cellspacing="0" width="100%">
            <tr class="gdalternate1">
                <td valign="top">
                    <asp:Label ID="lbfilter1" runat="server" Text="" Style="text-align: right;"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td align="right">
                    <asp:Button ID="Btnappyes" runat="server" Text="Yes" 
                        onclick="Btnappyes_Click"  />
                    <asp:Button ID="Btnappno" runat="server" Text="No" onclick="Btnappno_Click"  />
                </td>
            </tr>
        </table>
    </div>
    <div style="margin-top: 20px" width="100%" id="divoutput" runat="server" visible="false"
        class="error">
        <table cellpadding="3" cellspacing="0" width="100%">
            <tr class="gdalternate1">
                <td valign="top">
                    <asp:Label ID="lboutput" runat="server" Text="" Style="text-align: right;"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="margin-top: 20px" width="100%" id="divoutput1" runat="server" visible="false"
        class="error">
        <table cellpadding="3" cellspacing="0" width="100%">
            <tr class="gdalternate1">
                <td valign="top">
                    <asp:Label ID="lboutput1" runat="server" Text="" Style="text-align: right;"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="background-color: #c7dded; width: 100%; padding: 10px 10px 4px 10px;
        color: #666666; margin-top: 8px; min-height: 20px;">
        <span style="font: normal 18px arial; color: #003366;">Exam Summary</span>
    </div>
    <div id="divGrid" runat="server" width="100%">
        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
            Width="100%" OnRowDataBound="gvMain_RowDataBound" AllowPaging="false" PageSize="50">
            <Columns>
                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                    <HeaderStyle Width="2%" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:HyperLinkField HeaderStyle-Width="13%" HeaderText="Code" DataTextField="Code"
                    Target="_self" DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="FrmModuleHistory.aspx?CourseID={0}&ModuleID={1}">
                    <HeaderStyle Width="10%" />
                </asp:HyperLinkField>
                <asp:HyperLinkField HeaderStyle-Width="33%" HeaderText="Module Name" DataTextField="name"
                    Target="_self" DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="FrmModuleHistory.aspx?CourseID={0}&ModuleID={1}">
                    <HeaderStyle Width="48%" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:HyperLinkField>
                <asp:HyperLinkField HeaderText="ModuleType" HeaderStyle-Width="13%" DataTextField="MType"
                    DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="FrmModuleHistory.aspx?CourseID={0}&ModuleID={1}">
                    <HeaderStyle Width="13%" />
                </asp:HyperLinkField>
                <asp:HyperLinkField HeaderText="Result" HeaderStyle-Width="13%" DataTextField="Result"
                    DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="FrmModuleHistory.aspx?CourseID={0}&ModuleID={1}">
                    <HeaderStyle Width="10%" />
                </asp:HyperLinkField>
                <asp:HyperLinkField HeaderText="Grade" HeaderStyle-Width="13%" DataTextField="Grade"
                    DataNavigateUrlFields="CourseID,ID" DataNavigateUrlFormatString="FrmModuleHistory.aspx?CourseID={0}&ModuleID={1}">
                    <HeaderStyle Width="5%" />
                </asp:HyperLinkField>
            </Columns>
            <PagerSettings Visible="False" />
        </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc2:SideLink ID="Sidelink" runat="server" />
  <uc3:sidelink ID="Sidelink1" runat="server" />    
</asp:Content>
