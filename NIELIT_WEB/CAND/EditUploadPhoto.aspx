<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="EditUploadPhoto.aspx.cs" Inherits="CAND_EditUploadPhoto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
Upload Photo
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
<script language="javascript" type="text/javascript">

    function ValidateForm() {

       
        if (!isBlank("<%=ImgUpload.ClientID %>", " Upload Image"))
            return false;
        if (!isvalidImageFile("<%=ImgUpload.ClientID %>", "Photo"))
            return false;
        if (!isBlank("<%=ImgUploadSignature.ClientID %>", "Upload Signature"))
            return false;
        if (!isvalidImageFile("<%=ImgUploadSignature.ClientID %>", "Signature"))
            return false;
        if (!isBlank("<%=ImgUploadThumb.ClientID %>", "Upload Thumb Impression"))
            return false;
        if (!isvalidImageFile("<%=ImgUploadThumb.ClientID %>", "Thumb"))
            return false;



        return true;

    }
       
    </script>

    <asp:Label ID="lblError" runat="server" CssClass="error" EnableTheming="false" 
                    Visible="false" Width="99%"></asp:Label>
    <asp:MultiView ID="MultiView1" runat="server">
    <asp:View ID="View1" runat="server">
    
    <div class="box">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%" class="sample3">
         <tr class="head1">
                                <td colspan="2" class="style3">
                                    <asp:Label ID="Label5" runat="server" Text="Change Photo Detail "></asp:Label>
                                </td>
                            </tr>
                            <tr ID="TrPhotoFile" runat="server" class="gdalternate1">
                                <td width="30%" class="style4" height="45px">
                                    Upload Photo <b class="mandatory">*</b>
                                </td>
                                <td height="50px">
                                    <asp:FileUpload ID="ImgUpload" runat="server" onkeypress="return false;" 
                                        TabIndex="35" Width="360px" />
                                    <br />
                                    ( JPG,JPEG,GIF,PNG image with size upto 50 KB )
                                </td>
            </tr>
                            <tr class="gdrow1"  id="TrSignatureFile" runat="server">
                                <td width="30%">
                                    <asp:Label ID="Label28" runat="server" Text="Upload Signature <b class='mandatory'>*</b>"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="ImgUploadSignature" runat="server" onkeypress="return false;"
                                        TabIndex="36" Width="360px" />
                                    <br />
                                    ( JPG,JPEG,GIF,PNG image with size upto 50KB )
                                </td>
                            </tr>
                            <tr class="gdalternate1" id="TrLeftThumbFile" runat="server">
                                <td width="30%">
                                    <asp:Label ID="Label29" runat="server" 
                                        Text="Upload Left Hand Thumb Impression<b class='mandatory'>*</b>"></asp:Label>
                                </td>
                                 <td align="left" valign="top">
                                <asp:FileUpload ID="ImgUploadThumb" runat="server" onkeypress="return false;" TabIndex="38"
                                    Width="360px" /><br />
                                (JPG,JPEG,GIF,PNG image with size upto 
                                50 KB)
                            </td>
                            </tr>

        </table>
        </div>
         <div style="text-align: center; margin-top: 10px">
                <asp:Button ID="BtnPreview" runat="server" Text="Preview"  OnClientClick="return ValidateForm();"
                   onclick="BtnPreview_Click" />
                <asp:Button ID="BtnCancelPreview" runat="server" Text="Cancel" Width="60px" 
                    onclick="BtnCancelPreview_Click"  />
            </div>
        </asp:View>
        <asp:View ID="View3" runat="server">
        <table style="text-align: left" class="sample3" border="1" cellpadding="3" cellspacing="0"
        width="100%">
        <tr class="head1">
            <td colspan="3">
                <asp:Label ID="Label69" runat="server" Text="Preview"></asp:Label>
            </td>
        </tr>
        <tr >
            <td width="30%" align="center">
                <asp:Image ID="ImgCandidatePhoto" runat="server" Height="135px" 
                    ImageUrl="../images/photo.jpg" />
            </td>
            <td align="center" valign="bottom" width="30%">
                <img id="imgSignature" runat="server" src="../images/sign.jpg" style="height: 27px;
                                width: 111px" />
            </td>
            <td align="center"  valign="bottom" width="30%">
                <img id="imgThumb" runat="server" src="~/images/thumb.jpg" style="height: 27px;
                                width: 111px" />
            </td>
        </tr>
            <tr>
                <td align="center">
                    Photograph</td>
                <td align="center">
                    Signature</td>
                    <td align="center">
                    Left Thumb Impression</td>
            </tr>
        </table>
        <div class="sample3">
                <table width="100%" class="box" style="margin-top: 20px" cellpadding="3" cellspacing="0">
                    <tr class="gdalternate1" style="height: 5px;">
                        <td width="73%" style="color: #FF0000" align="left">
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left" style="color: #800000; font-size: 17px;" width="73%">
                            &nbsp;Before updating,please make sure that your photograph detail is correct and 
                            displaying properly.</td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left" style="color: #800000; font-size: 17px;" width="73%">
                            Once updated you cannot change your photo detail in future.</td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left" style="color: #800000; font-size: 17px;" width="73%">
                            If you are confirmed with this detail , please&nbsp; to
                            <asp:LinkButton ID="LnkBtnGoBack" runat="server" onclick="LnkBtnGoBack_Click"> go back</asp:LinkButton>
                            &nbsp;and update the detail.</td>
                    </tr>
                    <tr class="gdalternate1" style="height: 5px;">
                        <td align="left" style="color: #FF0000" width="73%">
                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: center; margin-top: 10px">
               
                <asp:Button ID="BtnUpdate" runat="server" 
                     Text="Update" onclick="BtnUpdate_Click" OnClientClick="return ValidateForm();" />
                <asp:Button ID="BtnBack" runat="server" Text="Back" Width="50px" 
                    onclick="BtnCancelUpdate_Click"  />
            </div>
        </asp:View>
        <asp:View ID="View2" runat="server">
        <div class="sample3">
                <table cellpadding="3" cellspacing="0" class="box" style="margin-top: 20px" width="100%">
                    <tr class="head1" style="height: 5px;">
                        <td align="left" style="color: #FFFFFF" width="73%">
                            <asp:Label ID="LblUpdateHeading" runat="server" Text=""></asp:Label>
                            &nbsp;is successfully updated.
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left" style="color: #800000; font-size: 17px;" width="73%">
                            <asp:Label ID="LblWelcome" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left" style="color: #800000; font-size: 17px;" width="73%">
                            You have successfully changed your&nbsp; Photo Detail.</td>
                    </tr>
                    <tr class="gdalternate1">
                        <td align="left" style="color: #800000; font-size: 17px;" width="73%">
                            Please go 
                            <asp:LinkButton ID="LnkBtnBacktoProfile1" runat="server" 
                                onclick="LnkBtnBacktoProfile1_Click" > back to dashboard</asp:LinkButton> 
                             &nbsp;to see the profile status.
                        </td>
                    </tr>
                    <tr class="gdalternate1" style="height: 5px;">
                        <td align="right" style="color: #FF0000" width="73%">
                            <asp:LinkButton ID="LnkBtnBacktoProfil2" runat="server" 
                                onclick="LnkBtnBacktoProfil2_Click" >Back to dashboard</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
        
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

