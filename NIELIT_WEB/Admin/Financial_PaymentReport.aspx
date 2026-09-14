<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="Financial_PaymentReport.aspx.cs" Inherits="Financial_PaymentReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1
        {
            text-align: center;
        }
        .auto-style2
        {

        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
    <asp:Label ID="lblHeading" Text="NSQF Course GST Report" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
    <div>
        <asp:Label runat="server" Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false">
        </asp:Label>

        <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
          <tr>
            <td>
                <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Payment Mode &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                    <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Settled Date On  From  &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>              
            </td>
         </tr>
            <tr class="even">
                <td class="style1">
                    <asp:DropDownList runat="server" ID ="ddlCourse" Height ="22px" Enabled ="false">
                        <asp:ListItem Value ="0" Text ="Short Term Course (NSQF Aligned)"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddlpaymentmode" runat="server" Height="22px" SkinID="ddl250" AutoPostBack="true" OnSelectedIndexChanged="ddlpaymentmode_SelectedIndexChanged">
                        <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Online"></asp:ListItem>
                        <asp:ListItem Value="2" Text="NEFT"></asp:ListItem>
                       <%-- // added by amit--%>
                        <asp:ListItem Value="3" Text="CSC SPV"></asp:ListItem>
                    </asp:DropDownList>

                             <%-- added by amit start --%>
          <asp:DropDownList 
             ID="ddlgateway" runat="server" 
             AutoPostBack="true" 
             Visible="false"
             Height="22px" SkinID="ddl250"
             >
             <asp:ListItem Value="0">--Select One--</asp:ListItem>
         </asp:DropDownList>
         <%-- added by amit end --%>

                </td>
                
                <td class="style1">
                    <asp:TextBox ID="txtDateFrom" runat="server" Height="16px" Width="148px"
                         OnKeyPress="return false" SkinID="txt210" ToolTip="Date From"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                        PopupButtonID="imgcal" TargetControlID="txtDateFrom">
                        </asp:CalendarExtender>
                    <img id="imgcal" runat="server" visible ="True" alt="Calender" src ="~/images/calendaricon.jpg" />                    
                </td>
            </tr>  
            <tr>
                <td>
                     <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Settled Date On  To  &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label> 
                </td>
                <td class="style1">

                </td>
                <td >

                </td>
            </tr> 
            <tr class ="even">
                <td class="style1">
                    <asp:TextBox ID="txtDateTo" runat="server" Height="16px" Width="148px"
                     OnKeyPress="return false" SkinID="txt210" ToolTip="Date To"></asp:TextBox>
                    <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                     PopupButtonID="imgdate" TargetControlID="txtDateTo">
                    </asp:CalendarExtender>
                    <img id="imgdate" runat="server" visible="True" alt="Calender" src="~/images/calendaricon.jpg" />
                </td>
                <td >

                </td>
                <td >

                </td>
            </tr>       
            </table>
        <div style="text-align: right; margin-top: 10px">
            <asp:Button ID="btn_exptopdf" runat="server" Text="Export to PDF" Width="140px" OnClick="btn_exptopdf_Click"  />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

