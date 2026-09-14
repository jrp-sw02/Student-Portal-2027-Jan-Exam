<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" 
    CodeFile="InterRCExamReschedule.aspx.cs" Inherits="Admin_InterRCExamReschedule" debug ="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style2
        {
            width: 165px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
    Inter-Regional Centre Exam  Reschedule
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">

    <script type="text/javascript" language="javascript">
        function OpenWindow() {
       
            if (!isBlank("<%=txtCRollNum.ClientID %>", "Roll Number"))
                return false;

            if (!isBlank("<%=txtRcRollNum.ClientID %>", "Roll Number"))
                return false;

            if (!isBlank("<%=txtECC.ClientID %>", "Exam Centre Code"))
                return false;

            if (!isBlank("<%=txtECA.ClientID %>", "Exam centre Address"))
                return false;

            if (!isBlankDate("<%=txtRcED.ClientID %>", "Exam Date", "dd-MMM-yyyy"))
                return false;

            if (!isBlank("<%=txtRcBN.ClientID %>", "Batch Number"))
                return false;

            if (!isBlank("<%=txtRcRT.ClientID %>", "Reporting Time"))
                return false;

            if (!isBlank("<%=txtAppNum.ClientID %>", "Application Number"))
                return false;
        }
    </script>
    <div>
        <asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>
        <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Application Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAppNum" runat="server"></asp:TextBox>                  
                </td>
            </tr>            
        </table>
        <div  style="text-align: right; margin-top: 10px">
            <asp:Button ID="btnDetails" runat="server" Text=" Show Details" Width="140px" OnClick="btnDetails_Click" onClientClick =" return OpenWindow()" /> 
        </div>
    </div>
    </br>
    <div id ="divDetails" runat="server" visible ="false">     
        <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td colspan="2">
                    <strong>Old Admit Card Details</strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:label id="lblName" runat="server" text="Name "></asp:label>
                </td>
                <td>
                    <asp:textbox id="txtName" runat="server" enabled="false"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRC" runat="server" Text="Regional Centre "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRC" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:label id="Label2" runat="server" skinid="CaptionLabel" text="Roll Number "></asp:label>
                </td>
                <td>
                    <asp:textbox id="txtRollNo" runat="server" onkeypress="return false;" enabled="False"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:label id="Label3" runat="server" skinid="CaptionLabel" text="Exam Centre Code "></asp:label>
                </td>
                <td>
                    <asp:textbox id="txtExamCentreName" runat="server" enabled="false"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:label id="Label4" runat="server" skinid="CaptionLabel" text="Exam Centre Address "></asp:label>
                </td>
                <td>
                    <asp:textbox id="txtExamCentreAddress" runat="server" width="500px" enabled="false"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:label id="Label6" runat="server" skinid="CaptionLabel" text="Exam Date "></asp:label>
                </td>
                <td>
                    <asp:textbox id="txtExamDate" runat="server" onkeypress="return false" enabled="false"></asp:textbox>
                   
                </td>
            </tr>
            <tr>
                <td>
                    <asp:label id="Label7" runat="server" skinid="CaptionLabel" text="Batch Number "></asp:label>
                </td>
                <td>
                    <asp:textbox id="txtbatchNumber" runat="server" enabled="false"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:label id="Label8" runat="server" skinid="CaptionLabel" text="Reporting Time "></asp:label>
                </td>
                <td>
                    <asp:textbox id="txtReportingTime" runat="server" enabled="false"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:label id="Label1" runat="server" skinid="CaptionLabel" text="Result Grade ID "></asp:label>
                </td>
                <td>
                    <asp:textbox id="txtResultGradeId" runat="server" enabled="false"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:label id="Label9" runat="server" skinid="CaptionLabel" text=" Choose Update &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:label>
                </td>
                <td>
                    <asp:dropdownlist id="ddlCheck" runat="server" enabled="true" autopostback=" true" onselectedindexchanged="ddlCheck_SelectedIndexChanged">
                        <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        <asp:ListItem Value="1"> Inter RC Update </asp:ListItem>
                        <asp:ListItem Value="2"> Within RC Update </asp:ListItem>                       
                    </asp:dropdownlist>
                </td>
            </tr>
        </table>                                                           
     </br>
        <div id ="divRCDetails" runat ="server" visible ="false">
            <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <table id="tblRC" class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                <strong>Please fill following details for Inter RC Update</strong>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblRcRollNum" runat="server" SkinID="CaptionLabel" Text="Roll Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRcRollNum" runat="server" onkeypress="return true;" Enabled="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblECC" runat="server" SkinID="CaptionLabel" Text="Exam Centre Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtECC" runat="server" Enabled="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblECA" runat="server" SkinID="CaptionLabel" Text="Exam Centre Address &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtECA" runat="server" Width="500px" Enabled="true"></asp:TextBox>
                            </td>
                        </tr>
                        <caption>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRcED" runat="server" SkinID="CaptionLabel" Text="Exam Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRcED" runat="server" onKeyPress=" return false" ToolTip="Exam Date"></asp:TextBox>
                                    <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgcal" TargetControlID="txtRcED">
                                    </asp:CalendarExtender>
                                    <img id="imgcal" runat="server" visible="True" alt="Calender" src="~/images/calendaricon.jpg" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRcBN" runat="server" SkinID="CaptionLabel" Text="Batch Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRcBN" runat="server" Enabled="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRcRT" runat="server" SkinID="CaptionLabel" Text="Reporting Time &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRcRT" runat="server" Enabled="true"></asp:TextBox>
                                </td>
                            </tr>
                        </caption>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" runat="server" Text=" Save" Width="140px" OnClick="btnSave_Click" OnClientClick="return OpenWindow();" />
            </div>
        </div>

        <div id="divCentreDetails" runat="server" visible="false">
            <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <strong>Please fill following details for Within RC Update</strong>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="lblCRollNum" runat="server" SkinID="CaptionLabel" Text="Roll Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCRollNum" runat="server" onkeypress="return true;" Enabled="true" OnClientClick="return OpenWindow();"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSaveCr" runat="server" Text="Remove Admit Card" Width="140px" OnClick="btnSaveCr_Click" OnClientClick="return OpenWindow();" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

