<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/MasterPages/MyInfo.master" CodeFile="PhysicalProgressReportFilter.aspx.cs" Inherits="Common_PhysicalProgressReportFilter" %>


<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Physical Progress Report 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">

        function OpenWindow() {
            //Project Name
            var centreId, quarter, year;
            if (!isSelected("<%=ddlCentreName.ClientID %>", "Centre Name"))
                return false;
            else
                centreId = document.getElementById('<%=ddlCentreName.ClientID %>').value;

            
            window.open("../HO/Rpt/PhysicalProgressReport.aspx?centreId=" + centreId );
                return false;
            }
        
    </script>
    <style type="text/css">
        .PromptCSS {
            color: Blue;
            font-size: small;
            font-style: italic;
            font-weight: bold;
            font-family: CourierNew;
            height: 20px;
            margin-left: 100px;
        }
        </style>
    <asp:Label ID="lblerror" runat="server"></asp:Label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="lblCentreName" runat="server" SkinID="CaptionLabel" Text="Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCentreName" runat="server" Height="22px" SkinID="ddl250" 
                            AutoPostBack="false">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            </tr>
        </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();"  />
        <asp:Button ID="btnReset" runat="server" Text="Reset"  OnClick="btnReset_Click" />
    </div>
   
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
