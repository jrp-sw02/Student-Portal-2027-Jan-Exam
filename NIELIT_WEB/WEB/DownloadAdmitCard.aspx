
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/MyInfo.master"
    CodeFile="DownloadAdmitCard.aspx.cs" Inherits="DownloadAdmitCard" Debug="true" %>

<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc3" %>
<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .sidetable a
        {
            color: #000000;
            text-decoration: none;
        }
        .sidetable a:hover
        {
            color: #3366CC;
            text-decoration: underline;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Download Admit Card"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {
            if (!isSelected("<%=Ddlexamcycle.ClientID%>", "Examination Name"))
                return false;
            if (!isBlank("<%=Txtappno.ClientID %>", "Application No / Registration No."))
                return false;
            if (!isBlank("<%=TxtDOB.ClientID %>", "Date of Birth"))
                return false;
            if (!isDate("<%=TxtDOB.ClientID%>", "Invalid Date of Birth"))
                return false;
            if (!isBlank("<%=txtcode.ClientID %>", "Captcha Code No."))
                return false;
        }
    </script>
    <asp:Label ID="Lblerror" Width="99%" class="error" EnableTheming="false" Visible="false"
        runat="server" Text=""></asp:Label>
    <table id="tbsearch" align="center" class="sample3" width="100%" runat="server">
        <tr class="head1">
            <td align="left" width="100%">
                <asp:RadioButtonList ID="Rdserachby" runat="server" RepeatDirection="Horizontal"
                    Width="100%" AutoPostBack="True" Style="text-align: left;" OnSelectedIndexChanged="Rdserachby_SelectedIndexChanged">
                    <asp:ListItem Value="1" Selected="True" enabled ="true" >Search By Application Number</asp:ListItem>
                    <asp:ListItem Value="2" >Search By Registration Number</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <div id="divfilter" runat="server">
    <table id="Table3" align="center" class="sample3" width="100%" runat="server" cellpadding="3"
        cellspacing="1">
           <tr class="gdrow1">
                <td class="even" width="30%" id="td1" runat="server">
                    Examination Year
                </td>
                <td width="70%" valign="bottom">
                    <asp:DropDownList ID="Ddlexamyear" runat="server" AutoPostBack="True" Height="22px"
                        Width="260px" onselectedindexchanged="Ddlexamyear_SelectedIndexChanged">
                    </asp:DropDownList>
                    <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Select Examination
                        Year you appeared in)</span>
                </td>
            </tr>
        <tr class="gdalternate1">
            <td width="30%">
                Examination Name
            </td>
            <td width="70%" valign="bottom">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                     <asp:DropDownList ID="Ddlexamcycle" runat="server" Height="22px" Width="260px">
                     <asp:ListItem Value="0">--Select One--</asp:ListItem>
                    </asp:DropDownList>
                      <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Select Examination
                        Name you appeared in)</span>
                    </ContentTemplate>
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Ddlexamyear" EventName="SelectedIndexChanged" />
                    </Triggers>
                    </asp:UpdatePanel>
            </td>
        </tr>
        <tr class="gdrow1">
            <td class="even" width="30%" id="tdfilter" runat="server">
                Enter Application No.</td>
             <td>
                    <asp:TextBox ID="Txtappno" runat="server" Width="248px" MaxLength="20" autocomplete="off"></asp:TextBox>
                    <span id="spfilter"  runat="server" style="text-align: left; vertical-align: top; font-size: 8pt;">(Application No.
                        printed on your application form)</span>
                </td>
        </tr>
        <tr class="trgdalternate1calendar">
            <td width="30%" class="odd">
                 Enter DOB(dd-Mon-yyyy)</td>
            <td class="odd">
                    <asp:TextBox ID="TxtDOB" runat="server" Width="248px" autocomplete="off"></asp:TextBox>
                    <img id="imgDob" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                        vertical-align: top;" />
                    <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Date of Birth
                        of the candidate)</span>
                    <asp:CalendarExtender ID="ceDOB" TargetControlID="txtDOB" PopupPosition="BottomLeft"
                        Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                    </asp:CalendarExtender>
                </td>
        </tr>
         <tr class="gdrow1">
                <td valign="top" width="30%">
                    Captcha Code
                </td>
                <td>
                     <%--onkeypress="checkNumber(this,6,0,event)"--%>
                    <asp:TextBox ID="txtcode" runat="server" Width="248px" MaxLength="6"
                        autocomplete="off"></asp:TextBox>
                    <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Enter captcha
                        code shown in image below)</span>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td valign="top" width="30%">
                </td>
                <td align="left" valign="top" style="padding: 0">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" style="margin: 0">
                                <tr>
                                    <td width="38%" align="left">
                                        <img id="imgcap" runat="server" alt="Capture Code" width="150" height="40" src="" />
                                        <asp:ImageButton ID="ImgBtnRefresh" runat="server" CausesValidation="false" ImageUrl="~/images/refresh.gif"
                                            Width="30px" Style="vertical-align: baseline;" 
                                            onclick="ImgBtnRefresh_Click" />
                                    </td>
                                    <td width="62%">
                                        <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Click here to
                                            obtain new captcha code if are not able to see the captcha code in image.)</span>
                                    </td>
                                </tr>
                            </table>                         
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="BtnView" runat="server" Text="View" class="even" 
            OnClientClick="return ValidateFormFields();" onclick="BtnView_Click" />
        <asp:Button ID="BtnReset" runat="server" Text="Reset" class="even" 
            onclick="BtnReset_Click" />
    </div>
    </div>
    <div id="divresult" runat="server" visible="false">
        <table align="center" border="0" cellpadding="3" class="sample3" width="100%">
            <tr class="head1">
                <th colspan="3" align="left">
                    <asp:Label ID="Lblcourse" runat="server" Text="" Style="text-align: center;"></asp:Label>
                </th>
            </tr>
            <tr class="gdalternate1">
                <td width="40%">
                    Candidate Name
                </td>
                <td>
                    <asp:Label ID="Lblname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" runat="server" id="trfathername">
                <td width="40%">
                    Father's Name
                </td>
                <td>
                    <asp:Label ID="Lbfname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1" runat="server" id="trmothername">
                <td width="40%">
                    Mother's Name
                </td>
                <td>
                    <asp:Label ID="Lbmname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" runat="server" id="trgname" visble="false">
                <td width="40%">
                    Guardian Name
                </td>
                <td>
                    <asp:Label ID="Lgname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" id="trdob" runat="server">
                <td>
                    Date of Birth
                </td>
                <td colspan="2">
                    <asp:Label ID="Lbldob" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    Course Name
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblccname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    Exam Name
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblexam" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1" id="trtheory" runat="server" visible="false">
                <td>
                    Exam Centre Name
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblexamcentre" runat="server" Text="NA (As not Applied For Theory Examination) "></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" id="trpractical" runat="server" visible="false">
                <td>
                   Practical Exam Centre Name
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblpracexamcentre" runat="server" Text="NA (As not Applied For Practical Examination) "></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:Label ID="Lberror1" runat="server" Text=""
        EnableTheming="false" CssClass="error" Width="99%" Visible="false"></asp:Label>
    <asp:Label ID="lbmessage" runat="server" Text="Your Admit Card is Ready to Be Printed. So Please Click on the below Button to Print / Download your Admit Card."
        EnableTheming="false" CssClass="error" Width="99%" Visible="false"></asp:Label>
    <div style="text-align: right; margin-top: 10px" runat="server" id="divfooter" visible="false">
        <asp:Button ID="Btndownload" runat="server" Text="Print Theory Offline Exam Admit Card" Width="230px" Visible="false"/>
        <asp:Button ID="Btndownload2" runat="server" Text="Print Theory Online Exam Admit Card" Width="230px" Visible="false"/>
        <asp:Button ID="Btndownload1" runat="server" Text="Print Practical Exam Admit Card" Width="220px" Visible="false"/>
        <asp:Button ID="Btnback" runat="server" Text="Back" Width="54px" 
            onclick="Btnback_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc2:SideLink ID="Sidelink" runat="server" />
 <uc3:SideLink ID="Sidelink1" runat="server" />
</asp:Content>
