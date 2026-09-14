<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CourseApplication.ascx.cs"
    Inherits="UserControl_CourseApplication"  %>
<table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
    cellspacing="1">
    <tr class="head1">
        <td align="left" colspan="3">
            Application Details
        </td>
    </tr>
    <tr class="gdrow1">
        <td width="36%">
            Application No.
        </td>
        <td width="44%">
            <asp:HyperLink ID="hlklink" runat="server"><asp:Label ID="lblAppno" runat="server"></asp:Label></asp:HyperLink>
        </td>
        <td style="width:20%; padding-top:25px; padding-bottom:25px;" align="center" rowspan="9" valign="top">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table width="90%" cellspacing="3">
                        <tr>
                            <td class="box" align="center" valign="middle" style="background-color: #c9d7e2;
                                height: 140px;">
                                <asp:Image ID="ImgCandidatePhoto" ImageUrl="../images/photo.jpg" Width="112" Height="130"
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
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr class="gdalternate1">
        <td width="36%">
            Application Date
        </td>
        <td width="44%">
            <asp:Label ID="lblDreceive" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdrow1">
        <td width="36%">
            Course
        </td>
        <td width="44%">
            <asp:Label ID="lblCourse" runat="server"></asp:Label>
        </td>
    </tr>
     <tr class="gdalternate1">
        <td width="36%">
            Exam Name
        </td>
        <td width="44%">
            <asp:Label ID="lblExamName" runat="server"></asp:Label>
        </td>
    </tr>
     <tr class="gdrow1">
        <td width="36%">
            Fee Amount
        </td>
        <td width="44%">
            <asp:Label ID="lblFeeAmt" runat="server"></asp:Label>
        </td>
    </tr>
    <%--Added_UP_Project_08_01_2025_Start Amit Added--%>
    <tr class="gdalternate1" runat="server" id="trProject" Visible="false">
        <td width="36%">Project
        </td>
        <td width="44%">
            <asp:Label ID="lblProject" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdrow1" runat="server" id="trField1" Visible="false">
        <td width="36%">
            <asp:Label ID="headField1" runat="server"></asp:Label>
        </td>
        <td width="44%">
            <asp:Label ID="lblField1" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdalternate1" runat="server" id="trField2" Visible="false">
    <td width="36%">
         <asp:Label ID="headField2" runat="server"></asp:Label>
    </td>
    <td width="44%">
        <asp:Label ID="lblField2" runat="server"></asp:Label>
    </td>
</tr>
<tr class="gdrow1" runat="server" id="trField3" Visible="false">
    <td width="36%">
        <asp:Label ID="headField3" runat="server"></asp:Label>
    </td>
    <td width="44%">
        <asp:Label ID="lblField3" runat="server"></asp:Label>
    </td>
</tr>
    <tr class="gdalternate1" runat="server" id="trField4" Visible="false">
    <td width="36%">
        <asp:Label ID="headField4" runat="server"></asp:Label>
    </td>
    <td width="44%" colspan="2">
        <asp:Label ID="lblField4" runat="server"></asp:Label>
    </td>
</tr>
<tr class="gdrow1" runat="server" id="trField5" Visible="false">
    <td width="36%">
        <asp:Label ID="headField5" runat="server"></asp:Label>
    </td>
    <td width="44%" colspan="2">
        <asp:Label ID="lblField5" runat="server"></asp:Label>
    </td>
</tr>

    <%--Added_UP_Project_08_01_2025_End Amit Added --%>
    <tr class="gdalternate1">
        <td width="36%">
            Application Status
        </td>
        <td width="44%" colspan="2">
            <asp:Label ID="lblStatus" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdrow1">
        <td width="36%">
            Applicant Name
        </td>
        <td width="44%" colspan="2">
            <asp:Label ID="Lblname" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr class="gdalternate1" id="TrFatherName" runat="server">
        <td width="36%">
            Father's Name
        </td>
        <td width="44%" colspan="2">
            <asp:Label ID="LblFatherName" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdrow1" id="TrMotherName" runat="server">
        <td width="36%">
            Mother's Name
        </td>
        <td width="44%" colspan="2">
            <asp:Label ID="LblMotherName" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdalternate1" id="TrGardianName" runat="server">
        <td width="36%">
            Guardian Name
        </td>
        <td width="44%" colspan="2">
            <asp:Label ID="LblGuardianName" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdalternate1" id="trgender" runat="server">
        <td width="36%">
            <asp:Label ID="Label8" runat="server" Text="Gender"></asp:Label>
        </td>
        <td width="44%" colspan="2">
            <asp:Label ID="lblGender" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdrow1" id="trmaritalstatus" runat="server">
        <td width="36%">
            <asp:Label ID="Label9" runat="server" Text="Marital Status"></asp:Label>
        </td>
        <td colspan="2" width="44%">
            <asp:Label ID="lblMstatus" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdalternate1" id="trdob" runat="server">
        <td width="36%">
            <asp:Label ID="Label12" runat="server" Text="Date of Birth "></asp:Label>
        </td>
        <td colspan="2" width="44%">
            <asp:Label ID="lblDob" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdalternate1" id="trRegVal" runat="server" visible="false">
        <td width="36%">
            Registration Validity</td>
        <td colspan="2" width="44%">
           <asp:Label ID="lblRegValidity" runat="server"></asp:Label>

        </td>
    </tr>
    <tr class="head1">
        <td colspan="3">
            <asp:Label ID="Label74" runat="server" Text="Correspondence Details"></asp:Label>
        </td>
    </tr>
    <tr class="gdrow1">
        <td width="36%">
            Address
        </td>
        <td colspan="2" width="44%">
            <asp:Label ID="lblCaddress" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdalternate1">
        <td width="36%">
            <asp:Label ID="Label18" runat="server" Text="Phone with STD Code"></asp:Label>
        </td>
        <td width="44%" colspan="2">
            <asp:Label ID="lblCcontact" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdrow1">
        <td width="36%">
            <asp:Label ID="Label40" runat="server" Text="Mobile"></asp:Label>
        </td>
        <td colspan="2" width="44%">
            <asp:Label ID="lblCmobile" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="gdalternate1">
        <td width="36%">
            <asp:Label ID="Label21" runat="server" EnableTheming="True" Text="Email "></asp:Label>
        </td>
        <td colspan="2" width="44%">
            <asp:Label ID="lblCemail" runat="server"></asp:Label>
        </td>
    </tr>
    <tr class="head1" id="trperaddress" runat="server">
        <td colspan="3">
            <asp:Label ID="Label2" runat="server" Text="Permanent Details"></asp:Label>
        </td>
    </tr>
    <tr class="gdrow1" id="trperaddressname" runat="server">
        <td width="36%">
            Address
        </td>
        <td colspan="2" width="44%">
            <asp:Label ID="lblPaddress" runat="server">N.A.</asp:Label>
        </td>
    </tr>
    <tr class="head1">
        <td colspan="3">
            Educational/Qualification Details
        </td>
    </tr>
    <tr valign="top" class="gdalternate1">
        <td width="36%">
            <asp:Label ID="Label60" runat="server" Text="Highest Educational Qualification"></asp:Label>
        </td>
        <td colspan="2" width="44%">
            <asp:Label ID="lblHqualification" runat="server"></asp:Label>
        </td>
    </tr>
</table>
