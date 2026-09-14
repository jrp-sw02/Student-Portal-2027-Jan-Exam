<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeFile="CandidateUpdateRequestPreview.aspx.cs"
     Inherits="CAND_CandidateUpdateRequestPreview"  Debug="true" %>

<%@ Register Src="../UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">    <title></title>
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .DivImage
        {
            position: relative;
            right: -1px;
            top: -5px;
            z-index: -1;
            width: 168px;
        }
        .InnerTable
        {
            position: relative;
            right: -1px;
            top: -5px;
            z-index: -1;
            width: 100%;
            height: 100%;
        }
        .PhotoImage
        {
            position: relative;
            right: 3px;
            top: 5px;
            z-index: -1;
        }
        .BarCodeImage
        {
            position: relative;
            right: 5px;
            top: -1px;
            z-index: 0;
        }
        .auto-style1 {
            height: 24px;
        }
    </style>
  
     <script type="text/javascript">
         function preventBack() { window.history.forward(); }
         setTimeout("preventBack()", 0);
         window.onunload = function () { null };
    </script>
</head>
<body style="background-color: #ffffff;">
    <form id="form1" runat="server">
    <table align="center" border="0" cellpadding="0" cellspacing="0" width="977px" id="Tblpreview"
        runat="server">
        <tr>
            <td>
                <uc1:NormalHeader ID="NormalHeader1" runat="server" />
            </td>
        </tr>
       
         <tr>
            <td align="center">
                <asp:Label ID="Lblerror" runat="server" EnableTheming="false" ForeColor="Red"></asp:Label>
                <br />
            </td>
        </tr>
        <tr style="height: 20px;">
            <td align="center" style="border-bottom: 1px solid #000000;" valign="middle">
                <asp:Label ID="Lblhead" runat="server" Style="font-size: 22px;"></asp:Label>
                <asp:ImageButton ID="BtnPrint" runat="server" Style="float: right; padding-bottom: 10px;"
                    OnClientClick="this.style.visibility='hidden';window.print();this.style.visibility='visible';return false;"
                    ImageUrl="~/images/print.gif" ToolTip="Print Form" /> <br />
            </td>
        </tr>
         <tr>
            <td align="center">
                 <table style="width: 100%;" class="preview" cellpadding="1" cellspacing="0">
                <tr class="normal" style="font: bold 18px arial; border-right: 1px solid #000000;">
                    <td align="center" width="25%">
                            Request Number
                        </td>
                     <td align="center" width="25%">
                            Registration Number
                        </td>
                        <td align="center" width="25%">
                            Request Date 
                        </td>
                        <td align="center" width="25%">
                            Course Level&nbsp;
                        </td>
                    </tr>
                       <tr>
                            <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 119px;"
                            align="center">
                            <asp:Label ID="LblReqNumber" runat="server" Font-Size="Larger"></asp:Label>
                        </td>
                        <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 119px;"
                            align="center">
                            <asp:Label ID="LblRegnNumber" runat="server" Font-Size="Larger"></asp:Label>
                        </td>
                           <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; height: 119px;"
                            align="center">
                            <asp:Label ID="LblReqDataTime" runat="server" Font-Size="Larger"></asp:Label>
                        </td>
                           <td style="border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000;  height: 119px;"
                            align="center">
                            <asp:Label ID="LblCourseLevel" runat="server" Font-Size="Larger"></asp:Label>
                        </td>
                           </tr>
                     </table>
                </td>
             </tr>
         <tr>
            <td align="left" valign="top">
                <table style="width: 100%;" class="preview" border="0" cellspacing="0" cellpadding="2">
                    <tr class="head1">
                        <td align="left" width="35%">
                            &nbsp;<b>Request Details :- </b>
                        </td>
                        <td align="left" width="15%">
                        </td>
                        <td align="left" width="25%">
                            &nbsp;
                        </td>
                        <td align="left" width="25%">
                            &nbsp;
                        </td>
                    </tr>
                     <tr class="normal" id="TrHandicapped" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="1. Handicapped (Disability)"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblHandicapped" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr class="normal" id="TrMaritalStatus" runat ="server" visible="false">
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="2. Marital Status"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblMaritalStatus" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr class="normal" id="TrCategory" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="3. Category"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblCategory" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrMobile" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="4. Mobile"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblMobile" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr class="normal" id="TrEmail" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="5. Email"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblEmail" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1" id="TRheaderCorAdd" runat ="server" visible="false">
                        <td colspan="2">
                            6.
                            <asp:Label ID="Label6" runat="server" Text="Correspondence Details"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="normal" id="TrAdd1" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label7" runat="server" Text="Address Lin1"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblCorAddressLine1" runat="server"></asp:Label>
                        </td>
                    </tr>
                      <tr class="normal" id="TrAdd2" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Address Lin2"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblCorAddressLine2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrAdd3" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Address Lin3"></asp:Label>
                        </td>
                        <td class="rightBorder" colspan="3">
                            <asp:Label ID="LblCorAddressLine3" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="normal" id="TrCityDist" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="City Name"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCorCity" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" Text="District"></asp:Label>
                        </td>
                        <td class="rightBorder">
                            <asp:Label ID="LblDistrict" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr class="normal" id="TrStatePin" runat="server" visible="false">
                        <td class="auto-style1">
                            <asp:Label ID="Label12" runat="server" Text="State"></asp:Label>
                        </td>
                        <td class="auto-style1">
                            <asp:Label ID="LblCorState" runat="server"></asp:Label>
                        </td>
                        <td class="auto-style1">
                            <asp:Label ID="Label13" runat="server" Text="Pin Code"></asp:Label>
                        </td>
                        <td class="auto-style1">
                            <asp:Label ID="LblCorPinCode" runat="server"></asp:Label>
                        </td>
                    </tr>
                     <tr class="head1" id="TRheaderEdu" runat="server" visible="false">
                        <td colspan="2">
                            7. Highest Educational / Qualification Details 
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                      <tr id="TrHighEducation" class="normal" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label14" runat="server" Text="Highest Educational Qualification"></asp:Label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="LblHeighEducation" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="tblDocs" class="normal" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label15" runat="server" Text="8.Uploaded Documents Details :-"></asp:Label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="LblDocDetails" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="TrFeeDetails" class="normal" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label16" runat="server" Text="9.Fee Amount"></asp:Label>
                        </td>
                        <td colspan="3" class="rightBorder">
                            <asp:Label ID="LblFeeAmount" runat="server"></asp:Label>
                        </td></tr>                    
                                                
                   
                    </table>
                </td>
             </tr>
        </table>
         <div style="text-align: center; margin-top: 10px;" id="divfooter" runat="server">
        <asp:Button ID="Btnsubmit" runat="server" Text="Upload Documents" Width="123px" OnClick="Btnsubmit_Click" />
        <asp:Button ID="BtnProcess" runat="server" Text="Process" Width="123px" Visible="false" OnClick="BtnProcess_Click" />
        <asp:Button ID="Btnback" runat="server" Text="Edit" Width="50px" OnClick="Btnback_Click" />
    </div>
    <br />
    <asp:HiddenField ID="courseid" runat="server" />
    <asp:HiddenField ID="Reqid" runat="server" />
    </form>
</body>
</html>
