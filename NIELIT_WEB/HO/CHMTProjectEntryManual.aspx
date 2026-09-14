<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="CHMTProjectEntryManual.aspx.cs" Inherits="HO_CHMTProjectEntryManual" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script type="text/javascript" language="javascript">
        function validateFormFields() {
            if (!isBlankDate("<%=txtPRD.ClientID %>", "Project Receipt Date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("<%=txtPRD.ClientID %>", "Project Receipt Date", "dd-MMM-yyyy"))
                return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
    CHM(T) O  Level Manual Project  Entry
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
    <div>
        <asp:Label id ="lblError" runat ="server" CssClass="error" ForeColor="#FF3300"  Width="100%" Visible="false"> </asp:Label>
        <table class="sample2" id="tbls2" runat="server" cellpadding="0" cellspacing="0"
            width="100%">

             <tr  class ="gdalternate1 ">
                <td colspan="1" class="auto-style2">
                    <label id="Label2" runat="server">Registration No :</label>
                </td>
                <td>                  
                   <asp:TextBox ID="txtRegNo" runat="server" MaxLength="100" Height="22px" Width="191px"></asp:TextBox>
                </td>
            </tr>
            <tr class ="even">
                <td colspan="1" class="auto-style2">
                  
                </td>
                <td>                  
                    <asp:Button  runat="server" Text="View" ID ="btnView" Width="150px" OnClick="btnView_Click"  />

                </td>
            </tr>
        </table>
        <table runat="server" align="center" border="0" cellpadding="3" class="sample3" width="100%" id ="tblInfo"  visible ="false">
            <tr class ="gdalternate1 ">
                <td>
                     Name</td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtName" runat="server" Width="250px" disabled="true"  ></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr class ="gdrow1">
                <td>
                     Father Name
                </td>
                <td>
                    <asp:TextBox ID="txtFatherName" runat="server" Width="250px"  disabled="true"></asp:TextBox>

                </td>
            </tr>
            <tr class ="gdalternate1 ">
                <td>
                    <asp:label ID="lblreason" runat ="server" Text ="Date of Birth"></asp:label>
                </td>
                <td>
                    <asp:TextBox ID="txtDOB" runat="server" Width="250px"  disabled="true"></asp:TextBox>
                </td>
            </tr>
             
             <tr class ="gdrow1">
                <td>
                    <asp:label ID="Label1" runat ="server" Text ="Project Receipt Date"></asp:label>
                </td>
                <td>

                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" >
            <ContentTemplate>--%>
                    <asp:TextBox ID="txtPRD" runat="server" Width="250px"></asp:TextBox>
                    <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdatefrom" TargetControlID="txtPRD">
                        </asp:CalendarExtender>
                        <img id="imgdatefrom" alt="Calender" src="../images/calendaricon.jpg" />
              <%--  </ContentTemplate>
                         </asp:UpdatePanel>--%>
                </td>
            </tr>
            <tr class ="gdalternate1">
                <td>

                </td>
                <td>
                     <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" >
            <ContentTemplate>--%>
                     <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"  OnClientClick="return validateFormFields()" Width="89px"/>
                 <%--</ContentTemplate>--%>

                         <%-- <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
    </Triggers>--%>
                         <%--</asp:UpdatePanel>--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

