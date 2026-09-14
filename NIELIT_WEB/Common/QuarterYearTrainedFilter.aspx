<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/MasterPages/MyInfo.master" CodeFile="QuarterYearTrainedFilter.aspx.cs" Inherits="Common_QuarterYearTrainedFilter" %>


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
            var centreId, quarter, year;
            if (!isSelected("<%=ddlCentreName.ClientID %>", "Centre Name"))
                return false;
            else
                centreId = document.getElementById('<%=ddlCentreName.ClientID %>').value;

            if (!isSelected("<%=ddlQuarter.ClientID %>", "Quarter No."))
                return false;
            else
                quarter = document.getElementById('<%=ddlQuarter.ClientID %>').value;
          
            if (!isSelected("<%=ddlYear.ClientID %>", "Year"))
                return false;
            else
                year = document.getElementById('<%=ddlYear.ClientID %>').value;


            window.open("../HO/Rpt/QuarterYearTrained.aspx?centreId=" + centreId + "&quarter=" + quarter + "&year=" + year );
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
        .auto-style1 {
            width: 468px;
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
            <td class="auto-style1">
                <asp:Label ID="lblQuarter" runat="server" SkinID="CaptionLabel" Text="Quarter"></asp:Label>
            </td>
        <td>
                <asp:Label ID="lblYear" runat="server" SkinID="CaptionLabel" Text="Yesr"></asp:Label>
            </td>    
        </tr>
        
        <tr class="even">
            <td class="auto-style1">
             
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlQuarter" runat="server" Height="22px" SkinID="ddl250" 
                            AutoPostBack="false">
                            <asp:ListItem Value="99">Select</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>             
            </td>
          
        
             
           
            <td>
                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlYear" runat="server" Height="22px" SkinID="ddl250" 
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
