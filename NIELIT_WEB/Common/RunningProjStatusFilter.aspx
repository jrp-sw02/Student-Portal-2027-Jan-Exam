<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/MasterPages/MyInfo.master" CodeFile="RunningProjStatusFilter.cs" Inherits="Common_RunningProjStatusFilter" %>


<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Status of Currently Running Projects Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">

        function OpenWindow() {
            //alert('test');
            //Project Name
            var centreId
            if (!isSelected("<%=ddlCentreName.ClientID %>", "Centre Name"))
                return false;
            else
                centreId = document.getElementById('<%=ddlCentreName.ClientID %>').value;
          
           
                if (!isBlankDate("<%=txtAsOnDate.ClientID %>", "Batch Start From", "dd-MMM-yyyy"))
                    return false;
                if (!isDate("<%=txtAsOnDate.ClientID %>", "Batch Start From", "dd-MMM-yyyy"))
                    return false;
               
                 var AsOnDate = document.getElementById('<%=txtAsOnDate.ClientID %>').value;
               
            //alert(AsOnDate);
            window.open("../HO/Rpt/RunningProjStatus.aspx?centreId=" + centreId + "&AsOnDate=" + AsOnDate );
                return false;
            }
           

           




            //View report
          
        
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
            <td>
                <asp:Label ID="lblAsOndate" runat="server" SkinID="CaptionLabel" Text="As on Date"></asp:Label>
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
            <td>
             
                         <asp:TextBox ID="txtAsOnDate" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                <img id="imgFrom" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtAsOndate"
                    Format="dd-MMM-yyyy" PopupButtonID="imgFrom">
                </asp:CalendarExtender>&nbsp;</td>
          
        </tr>

        </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();"   />
        <asp:Button ID="btnReset" runat="server" Text="Reset"  OnClick="btnReset_Click" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                   
                    <asp:HiddenField ID="HNonAfflAfflInst" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
