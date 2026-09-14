<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/FullInfo.master" AutoEventWireup="true"
    CodeFile="nieletpaymentservices.aspx.cs" Inherits="nieletpaymentservices" Debug="false" %>

<%@ Register Src="UserControl/NormalHeader.ascx" TagName="NormalHeader" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {
            if (!isBlankNumber("<%=txtno.ClientID %>", "Demand note number"))
                return false;
            if (!isNumber("<%=txtno.ClientID %>", "Invalid demand note number. It should be numeric only."))
                return false;
        }

        function Validate() {
            if (!isBlankNumber("<%=TxtCscId.ClientID %>", "CSC/VLE Id"))
                return false;
            if (!isNumber("<%=TxtCscId.ClientID %>", "Invalid CSC/VLE Id. It should be numeric only."))
                return false;
         }
    </script>
    <style type="text/css">
        .style1
        {
            width: 163px;
            height: 94px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <uc1:NormalHeader ID="NormalHeader1" runat="server" />
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td width="55%" style="vertical-align: bottom;">
                <asp:Label runat="server" ID="lblHeading" Text="NIELIT Student Service For CSC SPV(NEW1)"
                    Style="font-size: 20px; font-weight: bold;"></asp:Label>
            </td>
            <td width="20%" style="vertical-align: bottom; text-align: right;">
                <a href="Index.aspx" title="Click here to view news/events and important dates" target="_blank"
                    style="font-size: small; text-decoration: none; color: Black; font-weight: bold;">
                    Important Dates</a>
            </td>
            <td width="25%">
                <img alt="" class="style1" src="images/csc.jpg" align="right" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphContents" runat="Server">
    <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <table id="tblOption" align="center" class="sample" cellspacing="0" cellpadding="3"
        width="100%" runat="server">
        <tr>
            <td class="odd" runat="server" id="tdError" colspan="3" style="width: 100%; color: red">
            </td>
        </tr>
        <tr id="trServices" runat="server">
            <td class="even" width="20%" align="left">
                Select NIELIT Service
            </td>
            <td class="even" width="1%" align="center">
                :
            </td>
            <td class="even" width="78%" align="left">
                <asp:DropDownList TabIndex="1" AutoPostBack="true" ID="ddlServices" runat="server"
                    Width="100%" OnSelectedIndexChanged="ddlServices_SelectedIndexChanged">
                    <asp:ListItem Value="0" Text="--Select NIELIT Service--"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Online Course Registration Application Form Submission and Fee Deposit (O/A/B/C Level)"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Online Certificate Exam Application Form Submission and Fee Deposit (BCC/CCC)"></asp:ListItem>
                    <asp:ListItem Value="4" Text="Deposit Examination/Registration Fee Against Demand Note"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trExams" runat="server" visible="false">
            <td class="odd" width="20%" align="left">
                Select NIELIT Courses
            </td>
            <td class="odd" width="1%" align="center">
                :
            </td>
            <td class="odd" width="78%" align="left">
                <asp:DropDownList ID="ddlexam" TabIndex="2" runat="server" Width="100%" OnSelectedIndexChanged="ddlexam_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trDemandNote" runat="server" visible="false">
            <td class="odd" width="20%">
                Enter Demand Note Number
            </td>
            <td class="odd" width="1%" align="center">
                :
            </td>
            <td class="odd" width="78%" valign="bottom">
                <asp:TextBox ID="txtno" TabIndex="3" onkeypress="checkNumber(this,10,0,event)" runat="server"
                    MaxLength="90" Width="300px"></asp:TextBox>
                &nbsp;
                <asp:Button ID="BtnSearch" TabIndex="4" runat="server" Text="Search" OnClientClick="return ValidateFormFields();"
                    OnClick="BtnSearch_Click" />
            </td>
        </tr>
        <tr id="trReset" runat="server" visible="false">
            <td colspan="3" class="odd" align="right">
                <asp:Button ID="BtnSubmit" TabIndex="5" runat="server" Text="Fill Form" Width="86px"
                    OnClick="BtnSubmit_Click" />
                <asp:Button ID="BtnCancel" TabIndex="6" runat="server" Text="Reset" OnClick="BtnCancel_Click" />
            </td>
        </tr>
    </table>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <%-- <asp:UpdatePanel UpdateMode="Conditional" ID="upResult" runat="server">
        <ContentTemplate>--%>
    <div id="d1" runat="server" visible="false">
        <table id="Table2" width="100%" align="center" class="sample" runat="server" style="background-color: #ffffff;">
            <tr>
                <td colspan="2" class="odd1">
                    Demand Note / Payment Details:-
                </td>
            </tr>
            <tr>
                <td class="odd" width="30%">
                    Demand Note Number and Date
                </td>
                <td id="tdDemandNumber" runat="server" class="odd" width="70%">
                </td>
            </tr>
            <tr>
                <td class="even" width="30%">
                    Payment Description
                </td>
                <td class="even" id="tdPaymentDescription" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td class="odd" width="30%">
                    Amount (in rupees)
                </td>
                <td class="odd" id="tdAmount" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td class="even" width="30%">
                    CSC SPV Processing Charges(in rupees)
                </td>
                <td class="even " id="tdcscamount" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td colspan="2" class="odd1">
                    Payee's Detail:-
                </td>
            </tr>
            <tr>
                <td id="tdNameCaption" runat="server" class="odd" width="30%">
                    Name
                </td>
                <td class="odd" id="tdPayeeName" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td id="tdFnameCaption" runat="server" class="even" width="30%">
                    Father's Name
                </td>
                <td class="even" id="tdPayeeFatherName" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td id="tdMNameCaption" runat="server" class="odd" width="30%">
                    Mother's Name
                </td>
                <td class="odd" id="tdPayeeMotherName" runat="server" width="70%">
                </td>
            </tr>
            <tr id="trextra" runat="server">
                <td id="tdDobCaption" runat="server" class="even" width="30%">
                    &nbsp;
                </td>
                <td class="even" id="tdPayeeDOB" runat="server" width="70%">
                </td>
            </tr>
            <tr>
                <td id="td1" runat="server" class="odd" width="30%">
                    CSC / VLE ID*
                </td>
                <td class="odd">
                    <asp:TextBox ID="TxtCscId" runat="server" MaxLength="12"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="even" align="right">
                    <asp:Button ID="BtnPay" TabIndex="5" runat="server" Text="Confirm & Pay" class="even"
                        OnClick="BtnPay_Click" OnClientClick="return Validate();" />
                    &nbsp;
                    <asp:Button ID="btnResetDemadnNote" TabIndex="6" runat="server" Text="Reset" class="even" OnClick="btnResetDemadnNote_Click" />
                </td>
            </tr>
        </table>
    </div>
    -
    <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
