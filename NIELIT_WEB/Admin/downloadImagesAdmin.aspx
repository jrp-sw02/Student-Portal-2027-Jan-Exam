<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="downloadImagesAdmin.aspx.cs" Inherits="downloadImagesAdmin" Debug="true" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 
</asp:Content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    Download Candidate Photographs
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server"> 

    <script type="text/javascript" language="javascript">

        function OpenWindow() {


            
            //Course Category
            if (!isSelected("<%=ddlCourseCategry.ClientID %>", "Course Category"))
                
                return false;

            //Course Name
            var CourseId;
            if (!isSelected("<%=ddlCourseName.ClientID %>", "Course Name"))
                return false;
            else
                CourseId = document.getElementById('<%=ddlCourseName.ClientID %>').value;

            //Application Type
            var TypeId;
            if (!isSelected("<%=ddlAppType.ClientID %>", "Applicationsudha Type"))
                return false;
            else
                TypeId = document.getElementById('<%=ddlAppType.ClientID %>').value;

            //Exam cycle
            if (document.getElementById('<%=ddlExamCycle.ClientID %>')) {
                if (!isSelected("<%=ddlExamCycle.ClientID %>", "Exam Cycle"))
                    return false;
            }
            if (!isSelected("<%=ddlExamYear.ClientID %>", "Exam Year"))
                return false;
           
            // validation for input Application number, Roll number and Registration Number
            var appnumber = document.getElementById('<%=ddlNameFormat.ClientID %>').value;
            if (appnumber == 1) {
                if (!isLess("<%=txtRegnoTo.ClientID %>", "<%=txtRegnoFrom.ClientID %>", "Application Number To", "Application Number From"))
                    return false;
            }
            else if (appnumber == 2) {
                if (!isLess("<%=txtRegnoTo.ClientID %>", "<%=txtRegnoFrom.ClientID %>", "Roll Number To", "Roll Number From"))
                    return false;
            }
            else if (appnumber == 3) {
                if (!isLess("<%=txtRegnoTo.ClientID %>", "<%=txtRegnoFrom.ClientID %>", "Registration Number To", "Registration From"))
                    return false;
            }
           
            //Exam Name
            var ExamId;
           if (document.getElementById('<%=ddlExamName.ClientID %>')) {
                if (!isSelected("<%=ddlExamName.ClientID %>", "Exam Name"))
                    return false;
               else
                    ExamId = document.getElementById('<%=ddlExamName.ClientID %>').value;
           }
            else
                ExamId = "0";

             }
    </script>
     <style type="text/css">
        .PromptCSS
        {
            color: Blue;
            font-size: small;
            font-style: italic;
            font-weight: bold;
            font-family: CourierNew;
            height: 20px;
            margin-left: 100px;
        }
    </style>
    <script type="text/javascript">

        function OnTextKeyUp(txtRegnoFrom) {
            document.getElementById('<%=txtRegnoTo.ClientID%>').value = txtRegnoFrom.value;
        }

</script>    
    <script language="javascript" type="text/javascript">

        function isCheckInputNumber(txtRegnoTo,txtRegFrom) {
            var regnoTo = txtRegnoTo.value;
            var regnoFrom = document.getElementById('<%=txtRegnoFrom.ClientID%>').value;
            var a = regnoTo.localeCompare(regnoFrom);
            var msg = "Number should be equal or greater";
            var integervalue = parseInt(a, 10);
            if (integervalue < 0) {
                alert(msg);
                return false;
            }
            else
               return true;       
        }

        function isLess(ctrl, ctrl2, msg, msg2) {
            if (document.getElementById(ctrl)) {
                var Regto = document.getElementById(ctrl).value;
                var RegFrom = document.getElementById(ctrl2).value;
                var a = Regto.localeCompare(RegFrom);
                var integervalue = parseInt(a, 10);
                if (integervalue < "0") {
                    CallDiv(ctrl, msg + " can not be less than " + msg2)
                    document.getElementById(ctrl).value = Regto
                    if (document.getElementById(ctrl).disabled == false)
                        document.getElementById(ctrl).focus();
                    return false;
                }
            }
            return true;
        }
        </script>

    <asp:label id="Lblerror" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Application Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
        </tr>
        <tr class="even">
            <td>
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseCategry" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseCategry_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlCourseName" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlAppType" runat="server" Height="22px" SkinID="ddl250" OnSelectedIndexChanged="ddlAppType_SelectedIndexChanged"
                            AutoPostBack="True">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="TrExamHead" runat="server">
            <td>
 
                <asp:label id="Label8" runat="server" skinid="CaptionLabel" text="Exam Cycle  &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
                <asp:label id="Label3" runat="server" skinid="CaptionLabel" text="Exam Year  &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td>
               <asp:label id="Label9" runat="server" skinid="CaptionLabel" text="Exam Name  &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label> 
            </td>
        </tr>
        <tr id="TrExamInput" runat="server" class="even">
            <td>

                 <asp:updatepanel id="UpdatePanel4" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlExamCycle" runat="server" AutoPostBack="True" 
                            Height="22px" OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged" 
                            SkinID="ddl250">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
            <td>
               <asp:updatepanel id="UpdatePanel7" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlExamYear" runat="server" SkinID="ddl250" 
                            AutoPostBack="true" onselectedindexchanged="ddlExamYear_SelectedIndexChanged"
                            >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlExamCycle" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
            <td>
                 <asp:updatepanel id="UpdatePanel1" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlExamName" runat="server" SkinID="ddl250" AutoPostBack="true" onselectedindexchanged="ddlExamName_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                        </asp:DropDownList>
                    </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlapplicationtype" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
        </tr>
         <tr id="Tr1" runat="server">
            <td>
    <asp:label id="Label4" runat="server" skinid="CaptionLabel" text="Applicant Type">
                </asp:label>
            </td>
            <td colspan="2">
                <asp:label id="Label2" runat="server" skinid="CaptionLabel" text="Institute Name">
                </asp:label>
            </td>
            
        </tr>
         <tr id="Tr4" runat="server" class="even">
            <td>
                 <asp:updatepanel id="UpdatePanel5" runat="server">
                    <contenttemplate>
                        <asp:DropDownList ID="ddlapplicationtype" runat="server" Height="22px" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlapplicationtype_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="0">--Both--</asp:ListItem>
                        </asp:DropDownList>
                    </contenttemplate>                   
                </asp:updatepanel>
                </td>

             <td colspan="2">
                  <asp:updatepanel id="UpdatePanel2" runat="server">              
                      <ContentTemplate>
                        <asp:DropDownList ID="Ddlinstitutes" runat="server" Height="22px" SkinID="ddl504">
                            <asp:ListItem Value="0">--All--</asp:ListItem>
                        </asp:DropDownList>
                        <asp:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="Ddlinstitutes"
                            PromptText="Type accredited institute name to search from the institute list"
                            PromptPosition="Top" QueryPattern="Contains" IsSorted="true" PromptCssClass="PromptCSS">
                        </asp:ListSearchExtender>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlAppType" EventName="SelectedIndexChanged" />
                    </Triggers>                  
                </asp:updatepanel>
                </td>

             <td>
                </td>
             </tr>
        <tr id="Tr3" runat="server">
            <td>
                 <asp:label id="lblTotal1" runat="server" skinid="CaptionLabel" text="Record Selection">
                </asp:label>
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel8" runat="server">
                    <contenttemplate>
              <asp:label id="Label7" runat="server" skinid="CaptionLabel" text="">
                </asp:label>
                        </contenttemplate>
                    </asp:updatepanel>

            </td>
            <td>
                <asp:updatepanel id="UpdatePanel9" runat="server">
                    <contenttemplate>
                              <asp:label id="Label10" runat="server" skinid="CaptionLabel" text="">
                </asp:label>
                </contenttemplate>
                </asp:updatepanel>
            </td>
        </tr>

        
        <tr id="Tr2" runat="server" class="even">
            <td>
                                    <asp:updatepanel id="UpdatePanel6" runat="server">
                    <ContentTemplate>
                <asp:dropdownlist id="ddlNameFormat" runat="server" skinid="ddl250" Height="22px" AutoPostBack="true" OnSelectedIndexChanged="ddlNameFormat_SelectedIndexChanged">
                     <asp:ListItem Value="0">--Select One--</asp:ListItem>
                </asp:dropdownlist>
                        </ContentTemplate>
                    </asp:updatepanel>          
            </td>
            <td>
                <asp:updatepanel id="UpdatePanel10" runat="server">
                    <contenttemplate>
                <asp:TextBox ID="txtRegnoFrom" runat="server" MaxLength="140" SkinID="txt248" Enabled="False" onkeyup="OnTextKeyUp(this);"  ></asp:TextBox>  
                                        </contenttemplate>
                    </asp:updatepanel>
            </td>
            <td>

                <asp:updatepanel id="UpdatePanel11" runat="server">
                    <contenttemplate>
         <asp:TextBox ID="txtRegnoTo" runat="server" MaxLength="140" SkinID="txt248" Enabled="False"  ></asp:TextBox>
                                                </contenttemplate>
                    </asp:updatepanel>
            </td>
        </tr>
       
        <tr>
            <td >
                <asp:updatepanel id="up2" runat="server">
                    <contenttemplate>
                <div id="divbatches" runat="server">

                </div>
                 </contenttemplate>
                    <triggers>
                        <asp:AsyncPostBackTrigger ControlID="Ddlinstitutes" EventName="SelectedIndexChanged" />
                    </triggers>
                </asp:updatepanel>
            </td>
            <td colspan="2">
                <asp:updatepanel id="Updatepanel13" runat="server">
                    <contenttemplate>
                        <asp:label id="lblNoRecord" runat="server"   visible="False" Font-Bold="True" ForeColor="Red"></asp:label>
                 </contenttemplate> 
                                      
                </asp:updatepanel>
            </td>
        </tr>
    </table>
        <%--<asp:label id="lblNoRecord" runat="server" enabletheming="false" cssclass="error" width="99%"
        visible="false"></asp:label>--%>
    <div style="text-align: right; margin-top: 10px">
         
        <asp:button id="btnView" runat="server" text="Download Images" onclientclick="return OpenWindow();" 
            onclick="btnView_Click" />
                       
        <asp:button id="btnReset" runat="server" text="Reset" onclick="btnReset_Click" /></div>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
