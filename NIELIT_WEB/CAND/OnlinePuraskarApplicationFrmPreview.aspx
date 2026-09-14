<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OnlinePuraskarApplicationFrmPreview.aspx.cs" Inherits="OnlinePuraskarApplicationFrmPreview"
    MasterPageFile="~/MasterPages/MyInfo.master" Debug="false" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" Text="Online Puraskar Application Form" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script src="../Script/jquery-1.7.1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function ValidateForm() {           

            return true;
        }
       
    </script>
    <script type="text/javascript">
        function confirmation() {
            if (confirm('Are you sure you want to Final Submit Form ?')) {
                return true;
            } else {
                return false;
            }
        }
   </script>
         

    <style>
            @page {
                size: auto;
                margin: 0;
            }
        </style>

   <div id="divToPrint">
    <div id="OnlinePuraskarAppForm" runat="server" visible="true">

        <table class="preview" style="width: 100%; text-align: left" border="0" cellpadding="3"
            cellspacing="1">
            <tr id="headerP" runat="server" visible="false" >
            <td colspan="3">
                <uc2:NormalHeader ID="NormalHeader1" runat="server" />
            </td>
        </tr>
            <tr >
                <td align="center" colspan="2">
                    <strong style="text-align: center" class="headfont">PURASKAR APPLICATION FORM                        
                    </strong>
                </td>
            </tr>
             <tr>
            <td align="center" colspan="3">
                <asp:Label ID="Lblerror" runat="server" EnableTheming="false" ForeColor="Red"></asp:Label>
            </td>
        </tr>
            <tr>
                <td align="center" colspan="2">&nbsp;
                     <asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; padding-right:20px;  padding-bottom: 10px;"  visible="false" 
                    OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"   
                    ImageUrl="~/images/print.gif" ToolTip="Print Form" />
                   
                </td>
            </tr>

            <tr class="head1">
                <td align="left" colspan="3">Applicant's Personal Details
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">Online Reference Number
                </td>
                <td width="44%" class="rightBorder">
                    <asp:Label ID="lblOnlineRefNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">Name
                </td>
                <td width="44%" class="rightBorder">
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">Parent / Guardian Details
                </td>
                <td width="44%" class="rightBorder">
                    <asp:Label ID="lblParent_GuardianDetails" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">Gender
                </td>
                <td width="44%" class="rightBorder">
                    <asp:Label ID="lblGender" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">Date of birth
                </td>
                <td width="44%" class="rightBorder">
                    <asp:Label ID="lblDOB" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">Caste
                </td>
                <td width="44%" class="rightBorder">
                    <asp:Label ID="LblCaste" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr class="normal" id="TrFatherName" runat="server">
                <td width="36%">Email-Id
                </td>
                <td width="44%" class="rightBorder">
                    <asp:Label ID="LblEmailId" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="normal" id="TrMotherName" runat="server">
                <td width="36%">Mobile Number
                </td>
                <td width="44%" class="rightBorder">
                    <asp:Label ID="LblMobileNumber" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="normal" id="trgender" runat="server">
                <td width="36%">
                    <asp:Label ID="Label8" runat="server" Text="Institute Details"></asp:Label>
                </td>
                <td width="44%" colspan="2" class="rightBorder">
                    <asp:Label ID="LblInstituteDetails" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="normal" id="trdob" runat="server">
                <td width="36%">
                    <asp:Label ID="Label12" runat="server" Text="Is-Handicapped "></asp:Label>
                </td>
                <td colspan="2" width="44%" class="rightBorder">
                    <asp:Label ID="LblIsHandicapped" runat="server"></asp:Label>

                   
                </td>
            </tr>
            <div id="PHCertDetailsInputView" runat="server" visible="false">
                <tr class="normal">
                    <td width="36%">
                        <asp:Label ID="LblPHCertNo" runat="server" Text="Physically handicapped Certificate Number"></asp:Label>
                    </td>
                    <td colspan="2" width="44%" class="rightBorder">

                        <asp:Label ID="LblPHCertNo1" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="normal">
                    <td width="36%">
                        <asp:Label ID="lblPHCertDate" runat="server" EnableTheming="True" Text="Physically handicapped Certificate Date "></asp:Label>
                    </td>
                    <td colspan="2" width="44%" class="rightBorder">                       
                        <asp:Label ID="lblPHCertDate1" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr class="normal">
                    <td width="36%">
                        <asp:Label ID="LblPHCertUpload" runat="server" EnableTheming="True" Text="Physically handicapped Certificate Uploaded"></asp:Label>
                    </td>
                    <td colspan="2" width="44%" class="rightBorder">                       
                        <asp:Label ID="LblPHCertUpload1" runat="server"></asp:Label>
                    </td>
                </tr>
            </div>
            <tr class="normal">
                <td width="36%">
                    <asp:Label ID="lblRegnNo1" runat="server" EnableTheming="True" Text="Regn No"></asp:Label>
                </td>
                <td colspan="2" width="44%" class="rightBorder">
                    <asp:Label ID="LblRegnNo" runat="server"></asp:Label>
                </td>
            </tr>           
            <tr class="normal">
                <td width="36%">
                    <asp:Label ID="LblAadhaarNumber" runat="server" Text="Aadhaar Number"></asp:Label>
                </td>
                <td colspan="2" width="44%" class="rightBorder">                   
                    <asp:label id="lblAadhar" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">
                    <asp:Label ID="LblAnnualIncome" runat="server" EnableTheming="True" Text="Annual Income"></asp:Label>
                </td>
                <td colspan="2" width="44%" class="rightBorder">
                   <asp:label id="LblAnnualIncome1" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">
                    <asp:Label ID="Label5" runat="server" Text="Exam"></asp:Label>
                </td>
                <td class="rightBorder" colspan="2" width="44%">
                    <asp:Label ID="LblExam" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="head1">
                <td align="left" colspan="3">Bank Details
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">
                    <asp:Label ID="LblAccountNo" runat="server" EnableTheming="True" Text="Account No"></asp:Label>
                </td>
                <td colspan="2" width="44%" class="rightBorder">
                    <asp:label id="LblAccountNo1" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">
                    <asp:Label ID="LblAccountHolderName" runat="server" Text="Account Holder Name"></asp:Label>
                </td>
                <td colspan="2" width="44%" class="rightBorder">

                   <asp:label id="LblAccountHolderName1" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">
                    <asp:Label ID="LblAccountType" runat="server" EnableTheming="True" Text="Account Type"></asp:Label>
                </td>
                <td colspan="2" width="44%" class="rightBorder">
                   <asp:label id="LblAccountType1" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">
                    <asp:Label ID="LblBankName" runat="server" Text="Bank Name"></asp:Label>
                </td>
                <td colspan="2" width="44%" class="rightBorder">
                     <asp:label id="LblBankName1" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">
                    <asp:Label ID="LblBankAddress" runat="server" EnableTheming="True" Text="Bank Address"></asp:Label>
                </td>
                <td colspan="2" width="44%">
                    <asp:label id="LblBankAddress1" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">
                    <asp:Label ID="LblBankIFSC" runat="server" Text="Bank IFSC"></asp:Label>
                </td>
                <td colspan="2" width="44%" class="rightBorder">
                   <asp:label id="LblBankIFSC1" runat="server"></asp:label>
                </td>
            </tr>            
            <div id="CasteCertDetailsInputView" runat="server" visible="false">
                <tr class="head1">
                    <td align="left" colspan="3">Caste Details
                    </td>
                </tr>
                <tr class="normal">
                    <td width="36%">
                        <asp:Label ID="LblCasteCertNo" runat="server" Text="Caste Certificate Number"></asp:Label>
                    </td>
                    <td colspan="2" width="44%" class="rightBorder">

                         <asp:label id="LblCasteCertNo1" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="normal">
                    <td width="36%">
                        <asp:Label ID="LblCasteCertDate" runat="server" EnableTheming="True" Text="Caste Certificate Date "></asp:Label>
                    </td>
                    <td colspan="2" width="44%" class="rightBorder">                      
                         <asp:label id="LblCasteCertDate1" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="normal">
                    <td width="36%">
                        <asp:Label ID="LblCasteCertUpload" runat="server" EnableTheming="True" Text="Caste Certificate Uploaded"></asp:Label>
                    </td>
                    <td colspan="2" width="44%" class="rightBorder">
                       
                        <asp:label id="LblCasteCertUpload1" runat="server"></asp:label>
                    </td>
                </tr>
            </div>
            <tr class="head1">
                <td align="left" colspan="3">Income Details
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">
                    <asp:Label ID="LblIncomeCertNo" runat="server" Text="Income Certificate Number"></asp:Label>
                </td>
                <td colspan="2" width="44%" class="rightBorder">

                  <asp:label id="LblIncomeCertNo1" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">
                    <asp:Label ID="LblIncomeCertDate" runat="server" EnableTheming="True" Text="Income Certificate Date "></asp:Label>
                </td>
                <td colspan="2" width="44%" class="rightBorder">                    
                     <asp:label id="LblIncomeCertDate1" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="normal">
                <td width="36%">
                    <asp:Label ID="LblIncomeCertUpload" runat="server" EnableTheming="True" Text="Income Certificate Uploaded"></asp:Label>
                </td>
                <td colspan="2" width="44%" class="rightBorder">
                     <asp:label id="LblIncomeCertUpload1" runat="server"></asp:label>

                </td>
            </tr>
            <tr class="head1">
                <td align="left" colspan="3">Details of Modules
                </td>
            </tr>
             <tr class="normal">
                <td width="36%">
                    <asp:Label ID="Label2" runat="server" EnableTheming="True" Text="Papers"></asp:Label>
                </td>
                <td colspan="2" width="44%" class="rightBorder">
                     <asp:Label ID="lblpaperaap1" runat="server"></asp:Label>

                </td>
            </tr>
        </table>
      
    </div>
       </div>
    <div id="divSumbitMsg" runat="server" visible="false">
         <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
            cellspacing="1">
             <tr>
                <td align="center" colspan="2">
                    &nbsp;
                       </td>
            </tr><tr>
                <td align="center" colspan="2">
                    &nbsp;
                       </td>
            </tr><tr>
                <td align="center" colspan="2">
                    &nbsp;
                       </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <strong style="text-align: center" class="headfont">
                        <asp:Label Width="99%" EnableTheming="False" ID="LblSubmitMessage"
            runat="server" ForeColor="Green" Font-Bold="True" Font-Size="Medium"></asp:Label>                       
                    </strong>
                </td>
            </tr>
             </table>
        
    </div>
      <div  style="text-align: center; margin-top: 10px; height: 80px" id="divfooter" runat="server">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                     <asp:Button ID="Btnsubmit" runat="server" Text="Final Submit" Width="100px" OnClientClick="return confirmation();"  OnClick="Btnsubmit_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                     <asp:Button ID="Button2" runat="server" Text="Back" Width="50px" OnClick="Btnback_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
                        
        </div>
</asp:Content>
