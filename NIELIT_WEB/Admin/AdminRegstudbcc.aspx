<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminRegstudbcc.aspx.cs"
    Inherits="Admin_AdminRegstudbcc" MasterPageFile="~/MasterPages/main.master" Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/CourseApplication.ascx" TagName="CourseApplication"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Registered Students"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
        Visible="False" />
    <asp:Panel runat="server" ID="pnlFilter" Visible="false">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
            <div id="filterBox" align="left">
                <div id="filterPannel">
                    <asp:UpdatePanel EnableViewState="true" RenderMode="Inline" ID="filterPnal_upnlFilter"
                        UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <table cellpadding="0" id="body1" cellspacing="0" width="100%">
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <script language="javascript" type="text/javascript">
                    var box = $('#filterBox');
                    shortcut.add("Ctrl+Shift+F", function () {
                        box.show();
                    });
                    shortcut.add("Esc", function () {
                        box.hide();
                    });
                </script>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {

            return true;

        }

        
    </script>
    <asp:Label Width="99%" Style="background-color: #EACFCE; padding-top: 4px; padding-bottom: 4px;
        padding-left: 4px; color: Red; border: 1px solid maroon; font-size: 11pt; font-variant: normal;"
        ID="Lblerror" Visible="false" runat="server" Text=""></asp:Label>
    <div style="text-align: right; margin-top: 10px">
        <table style="text-align: left" class="sample3" border="0" cellpadding="3" cellspacing="1"
            width="100%">
            <tr class="head1">
                <td colspan="3">
                    <asp:Label ID="Label69" runat="server" Text="Personal Details"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td style="width: 36%">
                    <asp:Label ID="Label70" runat="server" Text=" Full Name"></asp:Label>
                </td>
                <td style="width: 44%">
                    <asp:Label ID="LblAppName" runat="server" Text=""></asp:Label>
                </td>
                <td style="width: 20%" align="center" rowspan="11" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table width="90%" cellspacing="3">
                                <tr>
                                    <td class="box" align="center" valign="middle" style="background-color: #c9d7e2;
                                        height: 140px;">
                                        <asp:Image ID="ImgCandidatePhoto" ImageUrl="~/images/photo.jpg" Width="112" Height="130"
                                            runat="server" Style="text-align: center;" ImageAlign="Middle" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" id="TdPhotoCaption" class="box" style="background-color: #c9d7e2;">
                                        <asp:ImageButton ID="ImgBtnPrevious" runat="server" Style="float: left; width: 20px;"
                                            ImageUrl="~/images/previous.gif" CommandArgument="P" Enabled="False" OnClick="ToggleImage" />
                                        <asp:Label ID="LblPhotoCaption" runat="server" Text="Photograph"></asp:Label>
                                        <asp:ImageButton ID="ImgBtnNext" ToolTip="Click to view signature" runat="server"
                                            ImageUrl="~/images/next.gif" Style="float: right;" CommandArgument="N" OnClick="ToggleImage" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:LinkButton ID="LnkBtnPhotoDetail" runat="server" ForeColor="#003366" Font-Bold="true"
                                            ToolTip="Please click to change the Images" OnClick="LnkBtnPhotoDetail_Click"
                                            Visible="true">Change Images</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class="gdrow1" id="TrGardianName" runat="server" visible="false">
                <td style="width: 36%">
                    <asp:Label ID="Label9" runat="server" Text="Guardian's Name"></asp:Label>
                </td>
                <td style="width: 44%">
                    <asp:Label ID="LblGuardianName" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" id="TrFatherName" runat="server" visible="false">
                <td style="width: 36%">
                    <asp:Label ID="Label2" runat="server" Text="Father's Name"></asp:Label>
                </td>
                <td style="width: 44%">
                    <asp:Label ID="LblFatherName" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1" runat="server" id="TrMotherName" visible="false">
                <td style="width: 36%">
                    <asp:Label ID="Label3" runat="server" Text="Mother's Name"></asp:Label>
                </td>
                <td style="width: 44%">
                    <asp:Label ID="LblMotherName" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" runat="server" id="TrName">
                <td style="width: 36%">
                    <asp:Label ID="Label4" runat="server" Text="Gender"></asp:Label>
                </td>
                <td align="left" style="width: 44%">
                    <asp:Label ID="LblGender" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td style="width: 36%">
                    <asp:Label ID="Label6" runat="server" Text="Date of Birth "></asp:Label>
                </td>
                <td style="width: 44%">
                    <asp:Label ID="LblDob" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td style="width: 36%">
                    <asp:Label ID="Label7" runat="server" Text="Cast Category"></asp:Label>
                </td>
                <td style="width: 44%">
                    <asp:Label ID="LblCategory" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    <asp:Label ID="Label84" runat="server" Text="Occuption"></asp:Label>
                </td>
                <td style="width: 44%">
                    <asp:Label ID="LblOccuption" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Course Category"></asp:Label>
                </td>
                <td style="width: 44%">
                    <asp:Label ID="lblcoursecategory" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="head1">
                <td colspan="3">
                    <asp:Label ID="Label83" runat="server" Text="Certificate Examination Details"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td style="width: 36%">
                    Application Number
                </td>
                <td style="width: 44%" colspan="2">
                    <asp:Label ID="LblRegNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td style="width: 36%">
                    Application Date
                </td>
                <td style="width: 44%" colspan="2">
                    <asp:Label ID="LblRegDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" runat="server">
                <td style="width: 36%">
                    Exam Applied For
                </td>
                <td style="width: 44%" colspan="2">
                    <asp:Label ID="Lblexamname" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td style="width: 36%">
                    Result Grade
                </td>
                <td style="width: 44%" colspan="2">
                    <asp:Label ID="Lblresultgrade" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td style="width: 36%">
                    Result Published Date
                </td>
                <td style="width: 44%" colspan="2">
                    <asp:Label ID="Lblresultdate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td style="width: 36%">
                    Regional Centre
                </td>
                <td style="width: 44%" colspan="2">
                    <asp:Label ID="Lblregcentre" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td style="width: 36%">
                    Course
                </td>
                <td style="width: 44%" colspan="2">
                    <asp:Label ID="LblCourse" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td style="width: 36%">
                    Candidate Type
                </td>
                <td style="width: 44%" colspan="2">
                    <asp:Label ID="LblCandidateType" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1" id="TrAccCentre" runat="server">
                <td style="width: 36%">
                    Accredited Centre
                </td>
                <td style="width: 44%" colspan="2">
                    <asp:Label ID="LblAccCentre" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="head1">
                <td colspan="3">
                    <asp:Label ID="Label8" runat="server" Text="Contact Details"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    <asp:Label ID="Label18" runat="server" Text="Phone Number with STD Code"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Label ID="LblPhone" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td style="width: 36%">
                    <asp:Label ID="Label40" runat="server" Text="Mobile Number"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Label ID="LblMobile" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td style="width: 36%">
                    <asp:Label ID="Label21" runat="server" EnableTheming="True" Text="Email Address"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Label ID="LblEmail" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="head1">
                <td colspan="3">
                    <asp:Label ID="Labelp" runat="server" Text="Correspondence Address  Details"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td style="width: 36%">
                    <asp:Label ID="Label20" runat="server" Text="Address Line1"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Label ID="LblPerAddressLine1" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td style="width: 36%">
                    <asp:Label ID="Label22" runat="server" Text="Address Line2"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Label ID="LblPerAddressLine2" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td style="width: 36%">
                    <asp:Label ID="Label25" runat="server" Text="Address Line3"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Label ID="LblPerAddressLine3" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td style="width: 36%">
                    City
                </td>
                <td colspan="2">
                    <asp:Label ID="LblPerCityName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td style="width: 36%">
                    <asp:Label ID="Label27" runat="server" Text="District"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Label ID="LblPerDistrictName" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td style="width: 36%">
                    <asp:Label ID="Label31" runat="server" Text="State"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Label ID="LblPerStateName" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td style="width: 36%">
                    <asp:Label ID="Label33" runat="server" Text="Pin Code"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Label ID="LblPerPinCodeNumber" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="head1">
                <td colspan="3">
                    Educational/Qualification Details
                </td>
            </tr>
            <%-- <tr class="gdrow1">
                        <td valign="top" style="width: 36%">
                            <asp:Label ID="Label10" runat="server" Text="DOEACC Qualification"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblDoeaccQualification" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
            <tr valign="top" class="gdalternate1">
                <td style="width: 36%">
                    <asp:Label ID="Label60" runat="server" Text="Highest Educational Qualification"></asp:Label>
                </td>
                <td colspan="2" style="width: 44%">
                    <asp:Label ID="LblHeighestEducation" runat="server"></asp:Label>
                </td>
            </tr>
            <tr valign="top" class="gdrow1">
                <td style="width: 36%">
                    <asp:Label ID="Label82" runat="server" Text="Year of Passing"></asp:Label>
                </td>
                <td colspan="2" style="width: 44%">
                    <asp:Label ID="LblYrOfPassing" runat="server"></asp:Label>
                </td>
            </tr>
            <tr valign="top" class="gdalternate1">
                <td style="width: 36%">
                    <asp:Label ID="Label5" runat="server" Text="Download Image"></asp:Label>
                </td>
                <td colspan="2" style="width: 44%">
                     <a id="CandPhotoApp" runat="server" href="javascript:void(0);" download>AppNo Photo</a>
                      <a id="CandPhotoRoll" runat="server" href="javascript:void(0);" download>RollNo Photo</a>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc6:SideLink ID="SideLink1" runat="server" />
</asp:Content>
