<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeCandidatePhoto.aspx.cs" Inherits="Admin_ChangeCandidatePhoto"  MasterPageFile="~/MasterPages/main.master" Debug="false" %>


<%@ Register src="../UserControl/BreadCrumb.ascx" tagname="BreadCrumb" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Change Candidate Images 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateForm() {

            if (!isSelected("<%=ddlphoto.ClientID %>", "Image"))
                return false;
            if (!isBlank("<%=ImgUpload.ClientID %>", "Upload Image"))
                return false;
            if (!isvalidImageFile("<%=ImgUpload.ClientID %>", "Image"))
                return false;
            return true;
        }
       
    </script>
    <asp:Label ID="lblError" runat="server" CssClass="error" EnableTheming="false" Visible="false"
        Width="99%"></asp:Label>
    <table style="text-align: left" class="sample3" border="1" cellpadding="3" cellspacing="0"
        width="100%" id="tbfilter" runat="server">
        <tr class="head1">
            <td colspan="2">
                <asp:Label ID="Label69" runat="server" Text="Change Candidate Images"></asp:Label>
            </td>
        </tr>
        <tr runat="server" id="trphoto" class="gdrow1">
            <td align="left" valign="top" width="40%">
                <asp:Label ID="Label3" runat="server" Text="Select image you want to change"></asp:Label>
            </td>
            <td align="left" valign="top" width="60%">
                <asp:Label ID="Lbname" runat="server" Text="Current View"></asp:Label>
            </td>
        </tr>
        <tr id="Trphotofile" runat="server" class="gdalternate1">
            <td>
                <asp:DropDownList ID="ddlphoto" runat="server" SkinID="ddl504" 
                    onselectedindexchanged="ddlphoto_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">---Select One---</asp:ListItem>
                    <asp:ListItem Value="1">Photo</asp:ListItem>
                    <asp:ListItem Value="2">Signature</asp:ListItem>
                    <asp:ListItem Value="3">Thumb Impression</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td rowspan="5" align="center" valign="middle">
                <asp:Image ID="ImgPhoto" runat="server" Height="135px" ImageUrl="../images/photo.jpg" />
            </td>
        </tr>
        <tr id="Tr1" runat="server" >
            <td>
                <asp:Label ID="Lbbrowse" runat="server" Text="Browse new image"></asp:Label>
            </td>
        </tr>
        <tr id="Tr2" runat="server" class="gdalternate1">
            <td>
                <asp:FileUpload ID="ImgUpload" runat="server" onkeypress="return false;" TabIndex="38"
                    Width="80%" />
            </td>
        </tr>
        <tr id="Tr3" runat="server" >
            <td>
                (JPG,JPEG,GIF,PNG image with size upto 50 KB)
            </td>
        </tr>
        <tr id="Tr4" runat="server" class="gdalternate1">
            <td align="center">
                    <asp:Button ID="BtnUpdate" runat="server" Text="Upload" 
                        OnClientClick="return ValidateForm();" onclick="BtnUpdate_Click" />
                    <asp:Button ID="BtnBack" runat="server" Text="Back" Width="50px" 
                        onclick="BtnBack_Click" />
            </td>
        </tr>
    </table>
    <div class="sample3" id="divfinal" runat="server" visible="false">
        <table cellpadding="3" cellspacing="0" class="box" style="margin-top: 20px" width="100%">
            <tr class="gdalternate1">
                <td align="left" style="color: #800000; font-size: 17px;" width="73%">
                    <asp:Label ID="Lbfinal" runat="server" Text=""></asp:Label>
                    <br />
                    Please click here to go
                    <asp:LinkButton ID="LnkBtnBacktoProfile1" runat="server" OnClick="LnkBtnBacktoProfile1_Click"> back to candidate profile page</asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
