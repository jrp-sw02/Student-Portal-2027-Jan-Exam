<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/MyInfo.master"
    CodeFile="ApplicationStatus.aspx.cs" Inherits="ApplicationStatus"  Debug="true" %>

<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/FormStatus.ascx" TagName="FormStatus" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
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
    <asp:Label ID="lblheading" runat="server" Text=""></asp:Label>
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

            if (!isBlank("<%=TxtAppno.ClientID %>", "Application No."))
                return false;
            if (!isBlank("<%=TxtDOB.ClientID %>", "Date of Birth"))
                return false;
            if (!isDate("<%=TxtDOB.ClientID%>", "Invalid Date of Birth"))
                return false;
            if (!isBlank("<%=txtcode.ClientID %>", "Captcha Code No."))
                return false;
        }
    </script>
    <asp:Label Width="99%" Style="background-color: #EACFCE; padding-top:4px; padding-bottom:4px; padding-left:4px; color: Red;
        border: 1px solid maroon; font-size: 11pt; font-variant: normal;" ID="Lblerror" Visible="false"
        runat="server" Text=""></asp:Label>
    <div id="divfilter" runat="server">
    <table id="Tblfilter" align="center" class="sample3" width="100%" runat="server" cellpadding="3" cellspacing="1">
        <tr class="gdalternate1">
            <td width="40%">
                <asp:Label ID="Lblsubheading" runat="server" Text=""></asp:Label>
            </td>
            <td width="60%" valign="bottom">
                <asp:Label ID="Lblcname" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td width="40%">
                Enter Application No.
            </td>
            <td>
                <asp:TextBox ID="TxtAppno" runat="server" Width="139px"  MaxLength="30" ></asp:TextBox>
                <span style="text-align:left; vertical-align:top; font-size:8pt;" >(Application No. printed on your application form)</span> 
            </td>
        </tr>
        <tr class="trgdalternate1calendar">
            <td width="40%" >
                Enter DOB(dd-mon-yyyy)</td>
            <td>
                <asp:TextBox ID="TxtDOB" runat="server" Width="139px"></asp:TextBox>
                <img id="imgDob" src="../images/calendaricon.jpg" style="width: 20px; height: 22px;
                    vertical-align: top;" />
                 <span style="text-align:left; vertical-align:top; font-size:8pt;" >(Date of Birth of the candidate)</span> 
                <asp:CalendarExtender ID="ceDOB" TargetControlID="txtDOB" PopupPosition="BottomLeft"
                    Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                </asp:CalendarExtender>
            </td>
        </tr>
        <tr class="gdrow1">
            <td valign="top" width="40%">
                Captcha Code
            </td>
            <td>
                <%--onkeypress="checkNumber(this,6,0,event)"--%>
                <asp:TextBox ID="txtcode" runat="server" Width="139px" MaxLength="6" 
                    autocomplete="off"></asp:TextBox>
                <span style="text-align:left; vertical-align:top; font-size:8pt;" >(Enter captcha code shown in image below)</span> 
            </td>
        </tr>
        <tr class="gdalternate1">
            <td valign="top" width="40%">
            </td>
            <td align="left"  valign="top" style="padding:0">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                    <table cellpadding="0" cellspacing="0" style="margin:0">
                        <tr>
                            <td width="42%" align="left">
                                <img id="imgcap" runat="server" alt="Capture Code" width="150" height="40" src="" />
                                <asp:ImageButton ID="ImgBtnRefresh" runat="server" CausesValidation="false" ImageUrl="~/images/refresh.gif"
                                Width="30px" OnClick="ImgBtnRefresh_Click" Style="vertical-align:baseline;"/>
                            </td>
                            <td width="58%">
                                <span style="text-align:left; vertical-align:top; font-size:8pt;" >(Click here to obtain new captcha code if are not able to see the captcha code in image.)</span>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="HfCaptcha" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <div id="dfooter" runat="server" style="text-align: right; margin-top: 10px;">
        <asp:Button ID="Btnview" runat="server" Text="View" class="even" OnClick="Btnview_Click"
            OnClientClick="return ValidateFormFields();" />
        <asp:Button ID="BtnReset" runat="server" Text="Reset" class="even" OnClick="BtnReset_Click" />
    </div>
    </div>
     <div id="divStatus" runat="server" visible="false">
        <table align="center" border="0" cellpadding="3" class="sample3" width="100%">
            <tr class="head1">
                <th colspan="3" align="left">
                    <asp:Label ID="Lblcourse" runat="server" Text="" Style="text-align: center;"></asp:Label>
                </th>
            </tr>
            <tr class="gdalternate1">
                <td width="40%">
                    Applicant Name
                </td>
                <td>
                    <asp:Label ID="Lblname" runat="server" Text=""></asp:Label>
                </td>
                <td width="10%" rowspan="4" align="center" valign="top">
                    <img id="imgcandphoto" runat="server" style="height: 111px; width: 90px" />
                </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    Date of Birth
                </td>
                <td>
                    <asp:Label ID="LblDOB" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1" id="trfathername" runat="server">
                <td width="40%">
                    Father's Name
                </td>
                <td>
                    <asp:Label ID="Lbfname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" id="trmothername" runat="server">
                <td width="40%">
                    Mother's Name
                </td>
                <td >
                    <asp:Label ID="Lbmname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1" runat="server" id="trgname" visble="false">
                <td width="40%">
                    Guardian Name
                </td>
                <td>
                    <asp:Label ID="Lgname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="head1">
                <td colspan="3">
                    Application Details:-
                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    Application No
                </td>
                <td colspan="2">
                    <asp:Label ID="LblAppno" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    Application Date
                </td>
                <td colspan="2">
                    <asp:Label ID="LblAppdate" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    Applicant Type
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblapptype" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    Course Name
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblccname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr  id="trexcycle" runat="server">
                <td>
                    Exam Cycle
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblecycle" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr  id="trcstatus" runat="server">
                <td>
                    Current Status
                </td>
                <td colspan="2">
                    <asp:Label ID="Lblss" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <div id="Lblnote" runat="server" visible="false" class="error" style=" width:99%;"></div>
        <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="Btnpback" runat="server" Text="Back" onclick="Btnpback_Click" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc2:SideLink ID="Sidelink" runat="server" />
    <uc3:SideLink ID="Sidelink1" runat="server" />
</asp:Content>
