<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="FilledForm.aspx.cs" Inherits="FilledForm" Debug = "true"%>

<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="Lblheading" runat="server" Text=""></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function showForm(url) {
            window.open(url, "AppForm", "width=980,height=450,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes");
            return false;
        }
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
    <asp:Label class="error" EnableTheming="false" Width="99%"
        ID="Lblerror" Visible="false" runat="server" Text=""></asp:Label>
    <div id="divFilter" runat="server">
        <table id="tblFilter" align="center" class="sample3" runat="server" cellpadding="3"
            cellspacing="1" width="100%;">
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
                    <asp:TextBox ID="TxtAppno" runat="server" Width="139px" MaxLength="30" ></asp:TextBox>
                    <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Application No.
                        printed on your application form)</span>
                </td>
            </tr>
            <tr class="trgdalternate1calendar">
                <td class="odd" width="40%">
                    Enter DOB(dd-mon-yyyy)</td>
                <td class="odd">
                    <asp:TextBox ID="TxtDOB" runat="server" Width="139px"></asp:TextBox>
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
                <td valign="top" width="40%">
                    Captcha Code
                </td>
                <td>
                    <%--onkeypress="checkNumber(this,6,0,event)"--%>
                    <asp:TextBox ID="txtcode" runat="server" Width="139px" MaxLength="6" ></asp:TextBox>
                    <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Enter captcha
                        code shown in image below)</span>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td valign="top" width="40%">
                </td>
                <td align="left" valign="top" style="padding: 0">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" style="margin: 0">
                                <tr>
                                    <td width="42%" align="left">
                                        <img id="imgcap" runat="server" alt="Capture Code" width="150" height="40" src="" />
                                        <asp:ImageButton ID="ImgBtnRefresh" runat="server" CausesValidation="false" ImageUrl="~/images/refresh.gif"
                                            Width="30px" OnClick="ImgBtnRefresh_Click" Style="vertical-align: baseline;" />
                                    </td>
                                    <td width="58%">
                                        <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Click here to
                                            obtain new captcha code if are not able to see the captcha code in image.)</span>
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField ID="HfCaptcha" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <div style="text-align: right; margin-top: 10px">
            <asp:Button ID="BtnView" runat="server" Text="View" OnClick="BtnView_Click" OnClientClick=" return ValidateFormFields();" />
            <asp:Button ID="BtnReset" runat="server" Text="Reset" OnClick="BtnReset_Click" />
        </div>
    </div>
    <div id="divpreview" runat="server" visible="false">
        <table id="tblResult" align="center" class="sample3" runat="server" style="background-color: #ffffff;
            width: 100%;">
            <tr class="head1">
                <th colspan="2" align="left">
                    <asp:Label ID="Lblhead" runat="server" Text=""></asp:Label>
                </th>
            </tr>
            <tr class="gdalternate1">
                <td class="even" width="40%">
                    Application No.
                </td>
                <td width="60%">
                    <asp:Label ID="LblAppno" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td class="even" width="40%">
                    Application Date
                </td>
                <td width="60%">
                    <asp:Label ID="Lblappdate" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="40%">
                    Course Name
                </td>
                <td width="60%">
                    <asp:Label ID="Lbcname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="40%" valign="top">
                    Name of Applicant
                </td>
                <td width="60%">
                    <asp:Label ID="Lblappname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1" id="trfathername" runat="server">
                <td width="40%" valign="top">
                    Father's Name
                </td>
                <td width="60%">
                    <asp:Label ID="Lblfname" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" id="trmothername" runat="server">
                <td width="40%">
                    Mother's Name
                </td>
                <td>
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
        </table>
        <div style="text-align: right; margin-top: 10px">
            <asp:Button ID="BtnPrint" runat="server" Text="View"/>
            <asp:Button ID="Btnpback" runat="server" Text="Back " OnClick="Btnpback_Click" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc2:SideLink ID="Sidelink" runat="server" />
    <uc3:SideLink ID="Sidelink1" runat="server" />
</asp:Content>
