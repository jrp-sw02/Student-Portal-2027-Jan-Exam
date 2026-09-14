<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DemandNoteCancellation.aspx.cs"
    Inherits="HO_DemandNoteCancellation" MasterPageFile="~/MasterPages/main.master" Debug="false" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    <asp:label id="lblHeading" runat="server" text="Demand Note Cancellation"></asp:label>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function ValidateFormFields2() 
        {
         if (!isBlank("<%=txtreason.ClientID %>", "Reason"))
             return false;
         if (!isBlank("<%=txtemailid.ClientID %>", "Email"))
             return false;
         if (!isValidEmail("<%=txtemailid.ClientID %>", "Invalid E-Mail ID"))
             return false;
        }

        function ValidateFormFields3() 
        {
            if (!isBlank("<%=txtdemandnoteno.ClientID %>", "Demand Note No"))
                return false;
            if (!isNumber("<%=txtdemandnoteno.ClientID %>", " Invalid Demand Note No"))
                return false;
            if (!isBlank("<%=txtamount.ClientID %>", "Amount"))
                return false;
            if (!isNumber("<%=txtamount.ClientID %>", "Invalid Amount"))
                return false;
        }
    </script>
        <div>
            <table class="sample2" id="Table1" runat="server" cellpadding="0" cellspacing="0"
                width="100%">
                <tr>
                    <td width="33%">
                        <asp:label id="Lbdemandnoteno" runat="server" text="Demand Note No. &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td width="33%">
                        <asp:label runat="server" text="Amount &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                        </asp:label>
                    </td>
                    <td width="33%">
                    </td>
                </tr>
                <tr class="even">
                    <td>
                        <asp:textbox runat="server" id="txtdemandnoteno" maxlength="9" skinid="txt248" onkeypress="checkNumber(this,9,0,event)">
                        </asp:textbox>
                    </td>
                    <td>
                        <asp:textbox runat="server" id="txtamount" maxlength="8" onkeypress="checkNumber(this,8,0,event)"
                            skinid="txt248">
                        </asp:textbox>
                    </td>
                    <td align="center">
                        <asp:button runat="server" text="Show Details" 
                            onclientclick="return ValidateFormFields3()" id="btnsearch" 
                            onclick="btnsearch_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divdetails" runat="server" visible="false">
            <table class="sample3" id="tblprint" style="width: 100%; text-align: left" border="0"
                cellpadding="3" cellspacing="1">
                <tr class="head1">
                    <td align="left" colspan="2">
                        Demand Note Details
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Demand Note Number
                    </td>
                    <td>
                        <asp:label id="lblDemandNoteNo" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="40%">
                        Demand Note Date
                    </td>
                    <td>
                        <asp:label id="lblDemandNoteDate" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Amount
                    </td>
                    <td>
                        <asp:label id="lblAmount" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="40%">
                        Demand Note Type
                    </td>
                    <td>
                        <asp:label id="lblDemandNoteType" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Application Type
                    </td>
                    <td>
                        <asp:label id="lblApplType" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="40%">
                        Fee Type
                    </td>
                    <td>
                        <asp:label id="lblFeeType" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Payment Mode
                    </td>
                    <td>
                        <asp:label id="lblPaymentMode" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="40%">
                        Payment Status
                    </td>
                    <td>
                        <asp:label id="lblPaymentStatus" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Created By
                    </td>
                    <td>
                        <asp:label id="lblcreatedby" runat="server"></asp:label>
                    </td>
                </tr>
                <tr class="gdalternate1">
                    <td width="40%" >
                        Reason For Cancellation
                    </td>
                    <td>
                        <asp:textbox runat="server" id="txtreason" maxlength="100" skinid="txt502"></asp:textbox>
                    </td>
                </tr>
                <tr class="gdrow1">
                    <td width="40%">
                        Request Person Email-ID
                    </td>
                    <td>
                        <asp:textbox runat="server" id="txtemailid" maxlength="60" skinid="txt248">
                        </asp:textbox>
                    </td>
                </tr>
            </table>
            <br />
            <div class="box" style="padding-top:2px;">
                <asp:label id="lblnote" runat="server" 
                    text= "Note:- Please First ensure that Demand Note which is to be cancelled is not reconcilled or paid." 
                    EnableTheming="False" width="100%" Font-Bold="True" Font-Italic="False" 
                    Font-Underline="False" ForeColor="Red"></asp:label>
            </div>
            <div style="text-align: right; margin-top: 10px">
                <asp:button id="btnsubmit" runat="server" text="Submit" 
                    onclientclick="return ValidateFormFields2()" onclick="btnsubmit_Click"
                     />
                <asp:button id="btnCancel" runat="server" text="Cancel" 
                    onclick="btnCancel_Click"/>
            </div>
        </div>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
