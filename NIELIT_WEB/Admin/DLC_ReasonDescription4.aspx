<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="DLC_ReasonDescription4.aspx.cs" Inherits="Admin_DLC_ReasonDescription4" Debug="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">

<asp:Label ID="Label1" runat="server" Text="Bulk Application Status Entry (Rejection)"></asp:Label>
&nbsp;
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">

    <div>
        <asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>
        <table runat="server" align="center" border="0" cellpadding="3" class="sample3" width="100%"   visible ="true">           
            <tr class ="gdrow1">
                <td>
                     <asp:Label ID="lblCourseName" runat="server" Text="Course Name"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" Width="260px"
                                AutoPostBack="true">
                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class ="gdalternate1">
                <td>
                     <asp:Label ID="lblMonth" runat="server" Text="Exam Month"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" Height="22px" Width="260px">
                                <asp:ListItem Value="0"> -- Select One --</asp:ListItem>
                                <asp:ListItem Value="1"> January </asp:ListItem>
                                <asp:ListItem Value="2"> February</asp:ListItem>
                                <asp:ListItem Value="3"> March</asp:ListItem>
                                <asp:ListItem Value="4"> April</asp:ListItem>
                                <asp:ListItem Value="5"> May</asp:ListItem>
                                <asp:ListItem Value="6"> June</asp:ListItem>
                                <asp:ListItem Value="7"> July</asp:ListItem>
                                <asp:ListItem Value="8"> August</asp:ListItem>
                                <asp:ListItem Value="9"> September</asp:ListItem>
                                <asp:ListItem Value="10"> October</asp:ListItem>
                                <asp:ListItem Value="11"> November</asp:ListItem>
                                <asp:ListItem Value="12"> December</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class ="gdrow1">
                <td>
                     <asp:Label ID="lblYear" runat ="server" Text="Exam Year"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack=" true" Height="22px" Width="260px">
                                <asp:ListItem Value="0"> -- Select One --</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class ="gdalternate1">
                <td colspan="2">
                     &nbsp;</td>
            </tr>
            <tr class ="gdrow1">
                <td colspan="2">
                    <asp:RadioButtonList ID="Rdoownertype" runat="server" RepeatDirection="Horizontal"
                        Width="518px" AutoPostBack="True" TabIndex="1" OnSelectedIndexChanged="Rdoownertype_SelectedIndexChanged">
                        <asp:ListItem Value="I" Selected="true"> Individual </asp:ListItem>
                        <asp:ListItem Value="B"> Bulk    </asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>            
        </table>

        <table runat="server" align="center" border="0" cellpadding="3" class="sample3" width="100%" id ="tblInd"  visible ="true">
            <tr class ="gdalternate1 ">
                <td>
                     <asp:Label ID ="lblAppNo" runat="server" Text ="Application Number"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtAppNo" runat="server" Width="250px"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class ="gdrow1">
                <td>
                    <asp:label ID="lblreason" runat ="server" Text ="Select Reason for Rejection"></asp:label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlReason" runat="server" Height="22px" Width="260px" AutoPostBack="true" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged">
                        <asp:ListItem Value="0">-- Select One --</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class ="gdalternate1">
                <td>

                </td>
                <td>
                     <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"  />
                </td>
            </tr>
        </table>
        <table runat="server" align="center" border="0" cellpadding="3" class="sample3" width="100%" id="tblBulk"  visible ="false">
            <tr class ="gdalternate1">
                <td>
                    <asp:label ID="Label5" runat ="server" Text ="Select Reason for Rejection"></asp:label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlReasonB" runat="server" Height="22px" Width="260px" AutoPostBack="true">
                                <asp:ListItem Value="0">-- Select One --</asp:ListItem>
                                <%--<asp:ListItem Value="1"> Others</asp:ListItem>--%>
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class ="gdrow1">
                <td></td>
                 <td style="width: 70%;" valign="top">
                    <asp:FileUpload ID="flUpload" runat="server" Width="485px" />
                    <asp:HiddenField ID="flpath" runat="server" />
                </td>
            </tr>
            <tr class ="gdalternate1">
                <td>

                </td>
                <td>
                    <asp:Button ID="btnUpload" runat="server"  Text="Upload" Width="91px" OnClick="btnUpload_Click" />
                </td>
            </tr>
        </table>
    </div>

     <div id="divValidateData" runat="server" visible="false" height="150px" width="600px"
        style="overflow: scroll;">
        <table class="sample3" id="tblValidateData" style="width: 100%; text-align: left"
            border="0" cellpadding="2" cellspacing="1">
            <tr class="head1">
                <td align="left" colspan="2" width="100%">
                    Validate Data
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="30%">
                    Total Records
                </td>
                <td width="70%">
                    <asp:label id="lblTotalRecords" runat="server"></asp:label>
                </td>
            </tr>
           <%-- <tr class="gdalternate1">
                <td>
                    Settled Records
                </td>
                <td>
                    <asp:label id="lblSettledRecords" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    Manually Settled Records
                </td>
                <td>
                    <asp:label id="lblManSet" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td valign="top">
                    Refund Records
                </td>
                <td>
                    <asp:label id="lblRefundRecords" runat="server"></asp:label>
                </td>
            </tr>--%>
            <tr class="gdrow1">
                <td valign="top">
                    Updated Records
                </td>
                <td>
                    <asp:label id="lblUpdatedRecords" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td valign="top">
                    Failed Records 
                </td>
                <td>
                    <asp:label id="lblFailedRecords" runat="server"></asp:label>
                </td>
            </tr>
             <tr class="gdrow1">
                <td valign="top">
                    Already Updated Records
                </td>
                <td>
                    <asp:label id="lblAlreadyUpdatedCount" runat="server"></asp:label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td colspan="2">
                    <asp:label id="Label12" runat="server" text=" If failure,it may be due to Unavailablility/Invalid Records."
                     forecolor="Red"></asp:label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

