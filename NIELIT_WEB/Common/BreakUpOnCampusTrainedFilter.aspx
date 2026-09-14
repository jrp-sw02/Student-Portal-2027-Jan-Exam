<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/MasterPages/MyInfo.master" CodeFile="BreakUpOnCampusTrainedFilter.cs" Inherits="Common_BreakUpOnCampusTrainedFilter" %>


<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Breakup of On-Campus Trained Report
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
            var centreId
            if (!isSelected("<%=ddlCentreName.ClientID %>", "Centre Name"))
                return false;
            else
                centreId = document.getElementById('<%=ddlCentreName.ClientID %>').value;
          
           
                if (!isBlankDate("<%=txtFromDate.ClientID %>", "Batch End From", "dd-MMM-yyyy"))
                    return false;
                if (!isDate("<%=txtFromDate.ClientID %>", "Batch End From", "dd-MMM-yyyy"))
                    return false;

            if (!isBlankDate("<%=txtToDate.ClientID %>", "Batch End To", "dd-MMM-yyyy"))
                    return false;
            if (!isDate("<%=txtToDate.ClientID %>", "Batch End To", "dd-MMM-yyyy"))
                return false;

            var FromDate = document.getElementById('<%=txtFromDate.ClientID %>').value;
            var ToDate = document.getElementById('<%=txtToDate.ClientID %>').value;

            if (!CompareDates(FromDate, ToDate, " Report From Date should be less than Report To Date", true))
                return false;

            window.open("../HO/Rpt/BreakupOnCampusTrained.aspx?centreId=" + centreId + "&FromDate=" + FromDate + "&ToDate=" + ToDate );
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
            <td colspan="2">
                <asp:Label ID="lblCentreName" runat="server" SkinID="CaptionLabel" Text="Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            </tr>
        <tr class="even">
            <td colspan="2">
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCentreName" runat="server" Height="22px" SkinID="ddl250" 
                            AutoPostBack="false">
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            </tr>
        <tr>
            <td>
                <asp:Label ID="lblFromdate" runat="server" SkinID="CaptionLabel" Text="From Date"></asp:Label>
            </td>
        <td>
                <asp:Label ID="lblToDate" runat="server" SkinID="CaptionLabel" Text="To Date"></asp:Label>
            </td>    
        </tr>
        
        <tr class="even">
            <td>
             
                         <asp:TextBox ID="txtFromDate" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                <img id="imgFrom" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"
                    Format="dd-MMM-yyyy" PopupButtonID="imgFrom">
                </asp:CalendarExtender>&nbsp;</td>
          
        
             
           
            <td>
             
                         <asp:TextBox ID="txtToDate" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                <img id="imgTo" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"
                    Format="dd-MMM-yyyy" PopupButtonID="imgTo">
                </asp:CalendarExtender>&nbsp;</td>
          
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
