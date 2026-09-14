<%@ Page Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="myprofile.aspx.cs" Inherits="myprofile" Debug="false" %>

<%@ Register Src="~/UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="My Profile"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
    <asp:Button ID="LnkBtnLockDetail" runat="server" ToolTip="Click here to lock personal details"
        Text="Lock Profile" Style="float: right;" 
        OnClick="LnkBtnLockDetail_Click" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <table style="text-align: left" class="sample3" border="0" cellpadding="3" cellspacing="1"
        width="100%">
        <tr class="head1">
            <td colspan="3">
                <asp:Label ID="Label69" runat="server" Text="Personal Details"></asp:Label>
                <asp:LinkButton ID="LnkBtnPersonalDetail" Style="float: right; padding-right: 5px; height: 19px;"
                    runat="server" ForeColor="#C9D7E2" OnClick="LnkBtnPersonalDetail_Click" 
                    ToolTip="Please click to change the Personal Details">Update Detail</asp:LinkButton>
            </td>
        </tr>
        <tr class="gdalternate1" id="TrName" runat="server">
            <td style="width: 36%">
                <asp:Label ID="Label70" runat="server" Text=" Full Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
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
                                    <asp:Image ID="ImgCandidatePhoto" ImageUrl="../images/photo.jpg" Width="112" Height="130"
                                        runat="server" Style="text-align: center;" ImageAlign="Middle" />
                                    <%--  <img src="../images/photo.jpg" style="height: 135px; width: 116px" 
                    id="ImgCandidatePhoto" runat="server" />--%>
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
                                    <asp:LinkButton ID="LnkBtnPhotoDetail" runat="server" ForeColor="#003366" ToolTip="Please click to change the Correspondence Address Details"
                                        OnClick="LnkBtnPhotoDetail_Click">Upload</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr class="gdrow1" id="TrFatherName" runat="server">
            <td style="width: 36%">
                <asp:Label ID="Label2" runat="server" Text="Father's Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblFatherName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="TrMotherName" runat="server">
            <td style="width: 36%">
                <asp:Label ID="Label3" runat="server" Text="Mother's Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblMotherName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="TrGardianName" runat="server" visible="false">
            <td style="width: 36%">
                Guardian&#39;s Name
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblGuardianName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label4" runat="server" Text="Gender &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td align="left" style="width: 44%">
                <asp:Label ID="LblGender" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label5" runat="server" Text="Marital Status"></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblMaritalStatus" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label6" runat="server" Text="Date of Birth "></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblDob" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label7" runat="server" Text="Cast Category"></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblCategory" runat="server" Text=""></asp:Label>
            </td>
        </tr>
       <%-- <tr class="gdrow1">
            <td>
                <asp:Label ID="Label84" runat="server" Text="Occuption"></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblOccuption" runat="server"></asp:Label>
            </td>
        </tr>--%>
        <tr class="gdalternate1">
            <td>
                <asp:Label ID="Label85" runat="server" Text="Is Handicaped"></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblIsHandicaped" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>
                <asp:Label ID="Label86" runat="server" Text="Is Ex-Servicemane"></asp:Label>
            </td>
            <td style="width: 44%">
                <asp:Label ID="LblIsExServicemane" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="head1">
            <td colspan="3">
                <asp:Label ID="Label83" runat="server" Text="Course Registration Details"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                Registration Number
            </td>
            <td style="width: 44%" colspan="2">
                <asp:Label ID="LblRegNo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                Registration Date
            </td>
            <td style="width: 44%" colspan="2">
                <asp:Label ID="LblRegDate" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                Valid Upto
            </td>
            <td style="width: 44%" colspan="2">
                <asp:Label ID="LblValidUpto" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                Current Course
            </td>
            <td style="width: 44%" colspan="2">
                <asp:Label ID="LblCourse" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                Candidate Type
            </td>
            <td style="width: 44%" colspan="2">
                <asp:Label ID="LblCandidateType" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1" id="TrAccCentre" runat="server">
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
                <asp:LinkButton ID="LnkBtnContactDetail" runat="server" Style="float: right;" ForeColor="#C9D7E2"
                    OnClick="LnkBtnContactDetail_Click" ToolTip="Please click to change the Contact Details">Update Detail</asp:LinkButton>
            </td>
        </tr>
        <tr class="gdrow1">
            <td>
                <asp:Label ID="Label18" runat="server" Text="Phone Number with STD Code"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPhone" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label40" runat="server" Text="Mobile Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblMobile" runat="server" Text="NA"></asp:Label>
                <asp:LinkButton ID="LnkBtnVerifyMobile" Style="float: right; padding-right: 15px;"
                    runat="server" OnClick="LnkBtnVerifyMobile_Click">Verify Now</asp:LinkButton>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label21" runat="server" EnableTheming="True" Text="Email Address &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblEmail" runat="server" Text="NA"></asp:Label>
                <asp:LinkButton ID="LnkBtnVerifyEmail" Style="float: right; padding-right: 15px;"
                    runat="server" OnClick="LnkBtnVerifyEmail_Click">Verify Now</asp:LinkButton>
            </td>
        </tr>
        <tr class="head1">
            <td colspan="3">
                <asp:Label ID="Label74" runat="server" Text="Correspondence Address Details"></asp:Label>
                <asp:LinkButton ID="LnkBtnCorAddressDetail" Style="float: right;" runat="server"
                    ForeColor="#C9D7E2" OnClick="LnkBtnCorAddressDetail_Click" ToolTip="Please click to change the Correspondence Address Details">Update Detail</asp:LinkButton>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label32" runat="server" Text="Address Line1 &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblCorAddressLine1" runat="server">NA</asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label20" runat="server" Text="Address Line2"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LbCorlAddressLine2" runat="server">NA</asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label22" runat="server" Text="Address Line3"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblCorAddressLine3" runat="server">NA</asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label9" runat="server" Text="City &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblCityName" runat="server">NA</asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label34" runat="server" Text="District &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblDistrict" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label33" runat="server" Text="State &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblState" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label39" runat="server" Text="Pin Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPincode" runat="server" Text="NA"></asp:Label>
            </td>
        </tr>
        <tr class="head1">
            <td colspan="3">
                <asp:Label ID="Label1" runat="server" Text="Permanent Address  Details"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label11" runat="server" Text="Address Line1"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPerAddressLine1" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label12" runat="server" Text="Address Line2"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPerAddressLine2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label13" runat="server" Text="Address Line3"></asp:Label>
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
                <asp:Label ID="Label14" runat="server" Text="District"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPerDistrict" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdrow1">
            <td style="width: 36%">
                <asp:Label ID="Label15" runat="server" Text="State"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPerState" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="gdalternate1">
            <td style="width: 36%">
                <asp:Label ID="Label16" runat="server" Text="Pin Code"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblPerPinCode" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr class="head1">
            <td colspan="3">
                Educational/Qualification Details
            </td>
        </tr>
        <tr class="gdrow1">
            <td valign="top" style="width: 36%">
                <asp:Label ID="Label10" runat="server" Text="DOEACC Qualification"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="LblDoeaccQualification" runat="server"></asp:Label>
            </td>
        </tr>
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
    </table>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc2:SideLink ID="Sidelink" runat="server" />
    <fieldset id="FldStSideLockMessage" runat="server" align="center" class="NoticeBoard">
        <legend><u>Notification:</u></legend><font color="red"><i>
            <br />
            Your profile details are completely updated and locked on
            <asp:Label ID="LblLockedOn" runat="server" Text=""></asp:Label>
            by you.</i></font><br />
        <br />
    </fieldset>
    <fieldset id="FldStSideincompleteMessage" runat="server" class="NoticeBoard">
        <legend style="color: Navy"><u>Notification:</u></legend><font color="red"><i>
            <br />
            Your Profile details are not completed yet. Please update your Profile details.
        </i></font>
        <br />
        <br />
    </fieldset>
    <fieldset id="Fieldsetnotmobilenumberverified" runat="server" class="NoticeBoard">
        <legend style="color: Navy"><u>Notification:</u></legend><font color="red"><i>
            <br />
            If you are not able to verify your mobile number due to any reason (e.g. SMS service is blocked in your region for security purpose), you can lock your profile without verifying your mobile number.
            But we recommend you to verify your mobile number before locking the profile details so that we can communicate with you through SMS.
             </i></font>
        <br />
        <br />
    </fieldset>
    <fieldset id="FldStSideNotLockMessage" runat="server" class="NoticeBoard">
        <legend style="color: Navy"><u>Notification:</u></legend><font color="red"><i>
            <br />
            You have completely updated your incomplete profile details but you have not locked
            yet. If you do not want to update your profile any more, please lock your profile
            by clicking on 'Lock Profile' button.</i></font>
        <br />
        <br />
    </fieldset>
    <fieldset id="FldStSideVrifiedMessage" runat="server" class="NoticeBoard">
        <legend style="color: Navy"><u>Notification:</u></legend><font color="red"><i>
            <br />
            Your profile details have been verified successfully on.
            <asp:Label ID="lblVerifiedOn" runat="server"></asp:Label>
        </i></font>
        <br />
        <br />
    </fieldset>
</asp:Content>
