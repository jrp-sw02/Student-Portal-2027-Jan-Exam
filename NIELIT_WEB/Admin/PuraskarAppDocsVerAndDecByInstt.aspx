<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true" CodeFile="PuraskarAppDocsVerAndDecByInstt.aspx.cs"
    Inherits="Admin_PuraskarAppDocsVerAndDecByInstt" Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<%@ Register src="../UserControl/NormalHeader.ascx" tagname="NormalHeader" tagprefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">         
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
   <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
        <tr>
            <td align="center" valign="top">
                <table width="1000px" border="0" cellspacing="0" cellpadding="0" align="center">
                    <tr>
                        <td colspan="3" valign="top">
                            <uc5:NormalHeader ID="NormalHeader2" runat="server" />
                        </td>
                    </tr>
                    </table>
                </td>
            </tr>
        </table>
    <asp:Label ID="lblHeading" runat="server" Text="Online Puraskar Application"></asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>  

    <div id="divGrid" runat="server" visible="true">
       
        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>

                <br />
                <asp:Label ID="lblAffiliatedInst" runat="server" ForeColor="blue" Font-Bold="true"
                    Text="Protsahan Puraskar App  Candidate Certificate Verification & Declaration By Institute " Visible="true"></asp:Label><br />
              
                 <table class="sample3" style="width: 100%; text-align: left" border="0" cellpadding="3"
            cellspacing="1">             
          
            <tr>
                <td align="center" colspan="2">&nbsp;                     
                </td>
            </tr>          

            <tr class="head1">
                <td align="left" colspan="3">Applicant's  Details
                </td>
            </tr>
                 <tr class="gdalternate1">
                <td width="36%"> <asp:Label ID="lblRegnNo1" runat="server" EnableTheming="True" Text="Regn No"></asp:Label>
                </td>
                <td width="44%">
                    <asp:Label ID="lblRegnNo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="36%">Name
                </td>
                <td width="44%">
                    <asp:Label ID="lblCanName" runat="server"></asp:Label>
                </td>
            </tr>
                      <tr class="gdalternate1">
                <td width="36%"> <asp:Label ID="lbllvvl1" runat="server" EnableTheming="True" Text="Level"></asp:Label>
                </td>
                <td width="44%">
                    <asp:Label ID="lbllvvl" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="36%">Parent / Guardian Details
                </td>
                <td width="44%">
                    <asp:Label ID="lblParent_GuardianDetails" runat="server"></asp:Label>
                </td>
            </tr>
         
           </table>

                  <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                    runat="server"></asp:Label>
                <br />
                <table style="width: 100%;">
                    <tr class="head1">
                            <td  colspan="2" id="tddeclarartion" runat="server">
                                <asp:Label ID="Label1" runat="server" text="Declaration:" Font-Bold="True" ForeColor="blue" /> 
                            </td>
                        </tr>
                                    <tr>
                                        <td class="style107">
                                        </td>
                                        <td style="text-align: justify;">
                                            <asp:CheckBox ID="chkdisclamier" runat="server" TabIndex="42" Text="<font color='RED'>*</font>" />
                                            I
                                            <asp:Label ID="LblName" runat="server" Text=""></asp:Label>,
                                            <asp:Label ID="Lblsalutation" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="LblDecMName" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="LblDecFname" runat="server" Text=""></asp:Label>
                                            hereby declare that the particulars submitted by the candidate in the online puraskar application
                                            form of
                                            <asp:Label runat="server" Text="" ID="lbldeccoursecode"></asp:Label>
                                            are cheked by me personally. I am aware that in case any information verified by me is false or 
                                            misleading, the Institute would be debarred form the conduction of DLC and / or debarred from
                                             accreditation for any or all O/A/B/C levels besides being subjected to any other action that may 
                                            be deemed fit by NIELIT, I will not held NIELIT responsible
                                            for any damages.                                            
                                        </td>
                                    </tr>
                                </table>

                <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                <asp:HiddenField ID="hfcode" runat="server" />
            </ContentTemplate>
            
        </asp:UpdatePanel>
    </div> 
    <div id="msg">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="false"></asp:Label>
                <asp:HiddenField ID="hcentreID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
  
    <div style="text-align: right; margin-top: 10px; height: 80px">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblRecord" runat="server" Font-Bold="True" ForeColor="#CC6600" /><br /><br /> 

                 <asp:Button ID="btnUpdateModules" runat="server" OnClick="btnUpdateModules_Click"
                    Text="Finalized Modules" Visible="false" /> 
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
   <div id="DivDocs" runat="server" visible="false">
        <asp:Label ID="Label161" runat="server" text="Details of Documnets:" Font-Bold="True" ForeColor="blue" />
       <br />
                <table class="sample3" style="width: 65%; text-align: left" border="0" cellpadding="1"
            cellspacing="1">
                    <tr><asp:CheckBox ID="chkRecords" runat="server" text="<b> Verify Documents</b><font color='RED'>*</font>"/></tr>
                    <thead>
                        
                        <tr style="background-color:#5499c7   ; text-align:center">
                            
                            <th><asp:Label ID="Label14" runat="server" text="Documents Name" Font-Bold="True" ForeColor="black" /> </th>
                            <th><asp:Label ID="Label15" runat="server" text="Documents Date" Font-Bold="True" ForeColor="black" /> </th>
                            <th><asp:Label ID="Label16" runat="server" text="Documents Type" Font-Bold="True" ForeColor="black" /> </th>
                           <%-- <th><asp:CheckBox ID="chkRelocate" runat="server" /></th>--%>

                        </tr> 
                    </thead>
                    <tr id="Income" runat="server">
                        
                     <td><asp:Label ID="lblIncom" runat="server"  ForeColor="black" Text="" /></td>
                    <td><asp:Label ID="lblIncomCerDate" runat="server"  ForeColor="black" /></td>
                    <td>
                        <%--<asp:Label ID="Label7" runat="server" text="Download" Font-Bold="True" ForeColor="black" />--%>
                        <%--<asp:Button ID="btnIncom" runat="server" OnClick="btnIncom_Click" Text="IncomCer Download" />--%>
                         <asp:LinkButton ID="btnIncom" runat="server" onclick="btnIncom_Click">Income Certificate View</asp:LinkButton>
                    </td>
                         <%--<td><asp:CheckBox ID="CheckBox3" runat="server" /></td>--%>
                       </tr>
                    <tr id="ph" runat="server">
                      
                     <td><asp:Label ID="lblPh" runat="server"  ForeColor="black" /></td>
                    <td><asp:Label ID="lblPhCerDate" runat="server"  ForeColor="black" /></td>
                    <td>
                        <%--<asp:Label ID="Label10" runat="server" text="Download" Font-Bold="True" ForeColor="black" />--%>
                         <%--<asp:Button ID="btnph" runat="server" OnClick="btnph_Click" Text="PhCer Download" />--%>
                        <asp:LinkButton ID="btnph" runat="server" onclick="btnph_Click">PWD Certificate View</asp:LinkButton>
                    </td>
                       <%--  <td><asp:CheckBox ID="CheckBox2" runat="server" /></td>--%>
                       </tr>
                    <tr id="caste" runat="server">
                       
                     <td><asp:Label ID="lblCaste" runat="server"  ForeColor="black" /></td>
                    <td><asp:Label ID="lblCasteDate" runat="server"  ForeColor="black" /></td>
                    <td>
                        <%--<asp:Label ID="Label13" runat="server" text="Download" Font-Bold="True" ForeColor="black" />--%>
                       <%-- <asp:Button ID="btnCaste" runat="server" OnClick="btnCaste_Click" Text="CasteCer Download" />--%>
                         <asp:LinkButton ID="btnCaste" runat="server" onclick="btnCaste_Click">Caste Certificate View</asp:LinkButton>
                    </td>
                       <%-- <td><asp:CheckBox ID="chkRelocate" runat="server" /></td>--%>
                       </tr>

                </table>
       

    </div>
                   </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID="btnIncom" />
             <asp:PostBackTrigger ControlID="btnph" />
            <asp:PostBackTrigger ControlID="btnCaste" />         
    </Triggers>
        </asp:UpdatePanel>
    <div id="Div2" style="text-align: right; margin-top: 10px; height: 480px">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" a>
            <ContentTemplate>
                <asp:Label ID="Label3" runat="server" ForeColor="Green" Font-Bold="false"></asp:Label>
                <asp:Button ID="btnSaveStatus" runat="server" OnClick="btnSaveStatus_Click" Text="Save Record" />
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" Visible="false" />
                <asp:HiddenField ID="HValueForPrevModulesExamId" runat="server" />
            </ContentTemplate>
            <Triggers>
                    <asp:PostBackTrigger ControlID="btnSaveStatus" />
                <asp:PostBackTrigger ControlID="btnBack" />
    </Triggers>
        </asp:UpdatePanel>
    </div>
   
   
    <div style="text-align:center">
        &nbsp;
    </div>

</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr>
            <td></td>
        </tr>
    </table>
</asp:Content>