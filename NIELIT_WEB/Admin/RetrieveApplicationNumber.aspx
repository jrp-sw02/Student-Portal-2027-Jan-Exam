<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" 
    AutoEventWireup="true" CodeFile="RetrieveApplicationNumber.aspx.cs" Inherits="Admin_RetrieveApplicationNumber" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Retrieve Application Number"></asp:Label>
&nbsp;(Only for DLC Courses)
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">

     <script type="text/javascript" language="javascript">
         function OpenWindow() {

             if (!isBlank("<%=txtDOB.ClientID %>", "Candidate's Name "))
                return false;

            if (!isBlank("<%=txtFatherName.ClientID %>", "Father's Name"))
                return false;

            if (!isBlank("<%=txtMotherName.ClientID %>", "Mother's Name"))
                return false;

            if (!isBlank("<%=txtGuardianName.ClientID %>", "Guardian's Name"))
                return false;

            if (!isBlankDate("<%=txtDOB.ClientID %>", "Date ofd Birth", "dd-MMM-yyyy"))
                return false;

            
        }
    </script>


    <div>
        <table id="tbsearch" align="center" class="sample3" width="100%" runat="server">
        <tr class="head1">
            <td align="left" width="100%">
                <%--asp:RadioButtonList ID="Rdserachby" runat="server" RepeatDirection="Horizontal"
                    Width="100%" AutoPostBack="True" Style="text-align: left;" OnSelectedIndexChanged="Rdserachby_SelectedIndexChanged">
                    <asp:ListItem Value="1" Selected="True" enabled ="true" >Search By Application Number</asp:ListItem>
                    <asp:ListItem Value="2" >Search By Registration Number</asp:ListItem>
                </asp:RadioButtonList>--%>
                Please enter your details below               
            </td>
        </tr>
    </table>
        <table id="tblRAN" runat="server" align="center" border="0" cellpadding="3" class="sample3" width="100%">
            <tr  class="gdrow1">
                <td>
                    <asp:Label ID="lblName" runat="server" Text="Name of the Candidate <font color='RED'>*</font>"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtName" runat="server"  onkeypress="return true;" Enabled="true" OnClientClick="return OpenWindow();"></asp:TextBox>
                    <%--<span style="text-align: left; vertical-align: top; font-size: 8pt;">(Enter Candidate's Name with correct spelling)</span>--%>
                </td>
            </tr >
            <tr class="gdalternate1">
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Care Of /  देखभाल <font color='RED'>*</font>"></asp:Label>
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="Rdoownertype" runat="server" RepeatDirection="Horizontal"
                        Width="300px" AutoPostBack="True" TabIndex="1" OnSelectedIndexChanged="Rdoownertype_SelectedIndexChanged">
                        <asp:ListItem Value="P" Selected="true"> Parents / माता पिता </asp:ListItem>
                        <asp:ListItem Value="G"> Guardian / संरक्षक    </asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>

            <tr class="gdrow1" id="trfather" runat="server" >
                <td>
                  <asp:Label ID ="lblFatherName" runat="server" Text="Father's Name <font color='RED'>*</font>"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFatherName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr class="gdrow1" id="trmother" runat="server">
                <td>
                    <asp:Label ID="lblMotherName" runat="server" Text ="Mother's Name <font color='RED'>*</font>"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID ="txtMotherName" runat="server" ></asp:TextBox>
                </td>
            </tr>
            <tr  class="gdrow1" id="trguardian" runat="server" visible ="false">
                <td>
                    <asp:Label ID ="lblGuardianName" runat ="server" Text ="Guardian's Name <font color='RED'>*</font>"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtGuardianName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    <asp:Label ID="lblDOB" runat="server" Text="Date of Birth <font color='RED'>*</font>"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDOB" runat ="server"   ></asp:TextBox>
                    <img id="imgDob" runat="server" src="../images/calendaricon.jpg" style="width: 20px;
                                    height: 22px; vertical-align: top;" />
                                <br />
                              <%--  ( As per high school certificate in 'dd-Mon-yyyy' format. i.e. '01-Jan-1990' )--%>
                                <asp:CalendarExtender ID="ceDOB" TargetControlID="txtDob" PopupPosition="BottomLeft"
                                    Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                                </asp:CalendarExtender>
                </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    <asp:label id="lblCourseName" runat="server" text="Course Name <font color='RED'>*</font>"></asp:label>
                </td>
                <%--<td>
                    <asp:TextBox ID="txtCourseName" runat="server" ></asp:TextBox>
                </td--%>

                
                <td>
                <%--<asp:UpdatePanel ID="UpdatePanel12" runat="server">
                    <ContentTemplate>--%>
                        <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px"   Width="260px" 
                            AutoPostBack="false" >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList> 
                    <span style="text-align: left; vertical-align: top; font-size: 8pt;">(Please select correct Course Name )</span>
                   <%--</ContentTemplate>--%>
                    <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDataDownloadedSequence" EventName="SelectedIndexChanged" />
                    </Triggers>--%>
                <%--</asp:UpdatePanel>--%>

                </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    <asp:Label id="lblExamMonth" runat="server" text-="Exam Month <font color='RED'>*</font>"></asp:Label>
                </td>
                <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        
                        <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" Height="22px"   Width="260px">
                            <asp:ListItem Value ="0"> -- Select One --</asp:ListItem>
                            <asp:ListItem Value =" 1"> January </asp:ListItem>
                            <asp:ListItem Value ="2"> February</asp:ListItem>
                            <asp:ListItem Value="3"> March</asp:ListItem>
                            <asp:ListItem Value ="4"> April</asp:ListItem>
                            <asp:ListItem Value="5"> May</asp:ListItem>
                            <asp:ListItem Value ="6"> June</asp:ListItem>
                            <asp:ListItem Value ="7"> July</asp:ListItem>
                            <asp:ListItem Value="8"> August</asp:ListItem>
                            <asp:ListItem Value ="9"> September</asp:ListItem>
                            <asp:ListItem Value ="10"> October</asp:ListItem>
                            <asp:ListItem Value ="11"> November</asp:ListItem>
                            <asp:ListItem Value ="12"> December</asp:ListItem>

                        </asp:DropDownList><span style="text-align: left; vertical-align: top; font-size: 8pt;">(Please select Examination Month you appeared in )</span>                  
                    </ContentTemplate>
                    <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDataDownloadedSequence" EventName="SelectedIndexChanged" />
                    </Triggers>--%>
                </asp:UpdatePanel>
            </td>
            </tr>
            <tr class="gdrow1">
                <td>
                    <asp:label id="lblExamYear" runat="server" text="Exam Year <font color='RED'>*</font>"></asp:label>
                </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                      <asp:DropDownList  ID="ddlYear" runat="server" AutoPostBack =" true" Height="22px"   Width="260px">
                          <asp:ListItem Value="0"> -- Select One --</asp:ListItem>
                      </asp:DropDownList><span style="text-align: left; vertical-align: top; font-size: 8pt;">(Please select Examination Year you appeared in)</span>
                          </ContentTemplate>
                    <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDataDownloadedSequence" EventName="SelectedIndexChanged" />
                    </Triggers>--%>
                </asp:UpdatePanel>
                
            </td>
            </tr>
            <tr class="gdalternate1">
                <td>
                    <asp:Label ID="lblCode" runat="server" Text ="Captcha Code <font color='RED'>*</font>"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr class="gdrow1">
                <td></td>
                <td>
                    <img id="imgcap" runat="server" alt="Capture Code" width="150" height="40" src="" />
                                        <asp:ImageButton ID="ImgBtnRefresh" runat="server" CausesValidation="false" ImageUrl="~/images/refresh.gif"
                                            Width="30px" Style="vertical-align: baseline;" 
                                            onclick="ImgBtnRefresh_Click" />
                </td>
            </tr>
            <tr>
                <td>

                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text ="Submit" Width="119px" OnClick="btnSubmit_Click"  OnClientClick =" return OpenWindow()"/>
                </td>
            </tr>
            <tr id ="trResult" runat ="server"  visible ="false">
                <td>

                </td>
                <td>
                    <asp:Label ID="lblResult" runat="server" enabletheming="false" cssclass="error" width="99%" ></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

