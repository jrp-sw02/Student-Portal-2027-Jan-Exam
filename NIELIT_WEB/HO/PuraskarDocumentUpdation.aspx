<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="PuraskarDocumentUpdation.aspx.cs" Inherits="HO_PuraskarDocumentUpdation" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
     <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">

            <table class="sample3" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr >
                <td style="width: 20%;" valign="top" colspan="3">
                        <div id="divGrid" runat="server" visible="true">

        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>

                <br />
                <asp:Label ID="lblheading2" runat="server" ForeColor="blue" Font-Bold="true"
                    Text="Protsahan Puraskar Document Updation" Visible="true"></asp:Label><br />
                
                 <asp:Label Width="99%" Style="background-color: #EACFCE; padding-top:4px; padding-bottom:4px; padding-left:4px; color: Red;
        border: 1px solid maroon; font-size: 11pt; font-variant: normal;" ID="Lblerror" Visible="false"
        runat="server" Text=""></asp:Label>
                

                <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                <asp:HiddenField ID="hfcode" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
                        </td>              
            </tr>
            <tr class="gdalternate1">
                <td style="width: 20%;" valign="top" >
                    <asp:Label ID="Label7" Width="100%" runat="server" Text="Registration Number"></asp:Label>

                </td>
                <td style="width: 20%;" valign="top" colspan="3">
                    <asp:TextBox ID="txtRegNo" runat="server"
                        Width="280px" MaxLength="15" onpaste="return false;" oncopy="return false;"
                      onkeypress="checkNumber(this,15,0,event);"  oncut="return false;"></asp:TextBox>
                </td>
            </tr>

                 <tr class="gdalternate1">
                <td style="width: 20%;" valign="top" >
                    <asp:Label ID="Label1" Width="100%" runat="server" Text="Exam Name"></asp:Label>

                </td>
                <td style="width: 20%;" valign="top" colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlflExam" Width="100%" runat="server" Enabled="true" >   
                                                    <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                            <Triggers>                                                
                                            </Triggers>
                                        </asp:UpdatePanel>
                </td>
            </tr>
                <tr class="gdrow1">
                <td style="width: 20%;" valign="top" >
                    <asp:Label ID="Label2" Width="100%" runat="server" Text="Aadhaar Number"></asp:Label>

                </td>
                <td style="width: 20%;" valign="top" colspan="3">
                    <asp:TextBox ID="TxtaadharEnc" runat="server"
                        Width="150px" MaxLength="12"  oncopy="return false;"
                      onkeypress="checkNumber(this,12,0,event);"  oncut="return false;"></asp:TextBox> <br />
                    &nbsp;<asp:Label ID="lblaadhar" Width="100%" ForeColor="Magenta" Font-Size="medium" runat="server" Text=""></asp:Label>
                    &nbsp;<br />
                     <asp:Button ID="btnAadhaar" runat="server" OnClick="EncryptAadhar"  Style="height: 26px"
                        Text="View Encrypted Aadhar" />

                </td>
            </tr>
                <tr>
                    <td colspan="2">
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
                    Online Reference ID</td>
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
                <tr class="head1">
                <td colspan="3">
                  Protsahan Puruskar  Document Updation:-
                    <asp:Label ID="lblerrorddlcouse" runat="server" ForeColor="Violet" Font-Size="Small" Font-Italic="true" Text=" [ Choose one of them or all ]" Width="60%"></asp:Label>
                </td>
            </tr>
               <tr class="gdalternate1">
                    <td width="36%">
                        <asp:Label ID="LblPHCertUpload" runat="server" EnableTheming="True" Text="Physically handicapped Certificate Upload"></asp:Label>
                    </td>
                    <td colspan="2" width="44%">                       
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>                                
                                <asp:FileUpload ID="PHCertUpload" runat="server" onkeypress="return false;" TabIndex="47"
                                    Width="360px" /><br />
                                ( PDF file with size upto 100 KB )   
                                <asp:Label ID="lblPHCertiUpload" runat="server" Visible="false"></asp:Label>
                                <asp:RegularExpressionValidator ID="regPHCertUpload" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                                    ControlToValidate="PHCertUpload" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                                    Display="Dynamic" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSave" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>

                <tr class="gdrow1">
                    <td width="36%">
                        <asp:Label ID="LblCasteCertUpload" runat="server" EnableTheming="True" Text="Caste Cert Upload"></asp:Label>
                    </td>
                    <td colspan="2" width="44%">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>

                                <asp:FileUpload ID="CasteCertFileUpload" runat="server" onkeypress="return false;" TabIndex="45" /><br />
                                ( PDF file with size upto 100 KB  )  
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                ControlToValidate="CasteCertFileUpload" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                Display="Dynamic" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSave" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>

               <tr class="gdalternate1">
                <td width="36%">
                    <asp:Label ID="LblIncomeCertUpload" runat="server" EnableTheming="True" Text="Income Certificate File"></asp:Label>
                    <br />
                </td>
                <td colspan="2" width="44%">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>

                            <asp:FileUpload ID="IncomeCerFileUpload" runat="server" onkeypress="return false;" TabIndex="45" /><br />
                            ( PDF file with size upto 100 KB  ) 
            <asp:RegularExpressionValidator ID="IncomeCerFileU" ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.PDF|.pdf)$"
                ControlToValidate="IncomeCerFileUpload" runat="server" ForeColor="Red" ErrorMessage="Please select a valid PDF file."
                Display="Dynamic" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSave" />
                        </Triggers>
                    </asp:UpdatePanel>

                </td>
            </tr>
        </table>
        <br />
        <div id="Lblnote" runat="server" visible="false" class="error" style=" width:99%;"></div>
        <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="BtnUpdate" runat="server" Text="Update Record" onclick="BtnUpdate_Click" />
        </div>
    </div>
                  <br />
                               <div style="text-align: right; margin-top: 10px; height: 80px">
           <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>--%>
                    <asp:Button ID="btnSave" runat="server" OnClick="ViewRecord"  Style="height: 26px"
                        Text="View Record" />
             <%--   </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>
                    </td>
                </tr>
        </table>

</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

