<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="RegistrationStatisticsFilter.aspx.cs" Inherits="HO_Registration_StatisticsFilter"  Debug="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Registration Statistics
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../../Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        var globalvar = "0";

        function OpenWindow() {
            var DateFrom = 0;
            var DateTo = 0;
            var CourseId;

            // Registration Date
            
            if (!isBlankDate("<%=txtDateFrom.ClientID %>", "Registration From Date", "dd-MMM-yyyy"))
                return false;
            else {
                if (!isDate("<%=txtDateFrom.ClientID %>", "Invalid Registration From Date", "dd-MMM-yyyy"))
                    return false;
                else
                    DateFrom = document.getElementById("<%=txtDateFrom.ClientID %>").value;
            }
            if (!isBlankDate("<%= txtToDate.ClientID %>", "Registration To Date", "dd-MMM-yyyy"))
                return false;
            else {
                if (!isDate("<%=txtToDate.ClientID %>", "Invalid Registration To Date", "dd-MMM-yyyy"))
                    return false;
                else {
                    DateTo = document.getElementById('<%= txtToDate.ClientID %>').value;
                }
            }

            if (!CompareDates(DateFrom, DateTo, "From date should be less than To Date", true))
                return false;
            //ddlDisplay Criteria 
            var DisplayCriteria;
            var SubCriteria;
            if (document.getElementById('<%=ddlDisplayCriteria.ClientID %>') && document.getElementById('<%=ddlDisplayCriteria.ClientID %>').value == "0") {
                DisplayCriteria = "0";
            }
            else {
                DisplayCriteria = document.getElementById('<%=ddlDisplayCriteria.ClientID %>').value;
                if (document.getElementById('<%=ddlSubCriteria.ClientID %>')) {
                    if (document.getElementById('<%=ddlSubCriteria.ClientID %>').value == "0")
                        SubCriteria = "0";
                    else
                        SubCriteria = document.getElementById('<%=ddlSubCriteria.ClientID %>').value;
                }
                else if (!document.getElementById('<%=ddlSubCriteria.ClientID %>'))
                    SubCriteria = "0";
            }
            //Course Name
            var RegStatusId;
            var RegTypeId = 0;// by default all.
                       if (document.getElementById('<%=ddlCourseName.ClientID %>') && document.getElementById('<%=ddlCourseName.ClientID %>').value == "0") 
                            CourseId = 0;
                        else
                            CourseId = document.getElementById('<%=ddlCourseName.ClientID %>').value;
                        if (document.getElementById('<%=HfRegStatusId.ClientID %>').value != "")
                            RegStatusId = document.getElementById('<%=HfRegStatusId.ClientID %>').value;
                        else {
                           
                            alert("Please Check at least one Registration Statistics..!");
                            return false;
                        }
//                        if (document.getElementById('HfRegTypeId.ClientID').value != "")
//                            RegTypeId = document.getElementById('HfRegTypeId.ClientID').value;
//                        else {

//                           alert("Please Check at least one Registration Type..!");
//                           return false;
//                      }
                       var ApplicantType = "institute";
                        var instituteID = 0;
                        if (document.getElementById('<%=ddlDisplayCriteria.ClientID %>').value == "AT") {
                           
                            if (document.getElementById('<%=HfApplicantType.ClientID %>').value == "Ins") {
                               
                                if (document.getElementById("<%=TrIns1.ClientID %>").style.display != 'none' && document.getElementById("<%=TrIns2.ClientID %>").style.display != 'none') {
                                    if (document.getElementById("<%=ChkAllInstitute.ClientID %>").checked == false) {
                                        if (document.getElementById('<%=TxtInstituteName.ClientID %>').value != globalvar) {
                                            callErrorMsg('<%=TxtInstituteName.ClientID %>', "Please enter Institute Name again");
                                            return false;
                                        }
                                        else {
                                            instituteID = document.getElementById("<%=HfInstitute.ClientID %>").value;
                                        }
                                    }
                                    else
                                    { instituteID = 0; }
                                }
                            }
                        }
                        //View report
                        window.open("RegistrationStatisticsReport.aspx?CourseId=" + CourseId + "&DateFrom=" + DateFrom + "&DateTo=" + DateTo + "&DisplayCriteria=" + DisplayCriteria + "&SubCriteria=" + SubCriteria + "&RegStatusId=" + RegStatusId + "&instituteID=" + instituteID + "&RegTypeId=" + RegTypeId, 'report', 'width=1100,height=600,menubar=no,titlebar=no,toolbar=no,status=no,scrollbars=yes,dependent=yes,resizable=yes', false);
                        return false;

                    }

                    function OnInstituteSelected(source, eventArgs) {
                        var results = eval('(' + eventArgs.get_value() + ')');
                        if (results.ID == "0")
                            return false;                       
                        document.getElementById("<%=HfInstitute.ClientID %>").value = results.ID;
                        globalvar = results.Name;                   
                    }
    </script>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
             <td style="width: 33%;" valign="top">
                 <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Course &lt;b class='mandatory'&gt;&lt;/b&gt;" Width="100%"></asp:Label>
             </td>
             <td style="width: 33%;" valign="top">
                 <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Registration Date From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
             </td>
             <td style="width: 33%;" valign="top">
                 <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Registration Date To &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
             </td>
        </tr>
        <tr class="even">
             <td style="width: 33%;" valign="top">
                <asp:DropDownList ID="ddlCourseName" runat="server" SkinID="ddl250" AutoPostBack="True" onselectedindexchanged="ddlCourseName_SelectedIndexChanged">
                    <asp:ListItem Value="0">--ALL--</asp:ListItem>
                </asp:DropDownList>
            </td>
             <td style="width: 33%;" valign="top">
                <asp:TextBox ID="txtDateFrom" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                <img id="imgFrom" src="../../images/calendaricon.jpg" style="width: 20px; height: 22px;
                    vertical-align: top;" />
                 <asp:CalendarExtender ID="calendar1" TargetControlID="txtDateFrom" PopupPosition="BottomLeft"
                     Format="dd-MMM-yyyy" PopupButtonID="imgFrom" runat="server">
                 </asp:CalendarExtender>
             </td>
            <td style="width: 33%;" valign="top">
                <asp:TextBox ID="txtToDate" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                <img id="imgTo" src="../../images/calendaricon.jpg" style="width: 20px; height: 22px;
                    vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" PopupPosition="BottomLeft"
                    Format="dd-MMM-yyyy" PopupButtonID="imgTo" runat="server">
                </asp:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td >
                <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" 
                    Text="Display Criteria "></asp:Label>
            </td>
            <td>
                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>--%>
                        <asp:Label ID="LblSubCriteria" runat="server" SkinID="CaptionLabel" Visible="False"></asp:Label>
                <%-- </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDisplayCriteria" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr class="even" >
            <td>
                <asp:DropDownList ID="ddlDisplayCriteria" runat="server" SkinID="ddl250"
                    AutoPostBack="True" OnSelectedIndexChanged="ddlDisplayCriteria_SelectedIndexChanged"
                    Width="250px">
                    <asp:ListItem Value="0">--ALL--</asp:ListItem>
                    <asp:ListItem Value="G">Gender</asp:ListItem>
                    <asp:ListItem Value="CC">Cast Category</asp:ListItem>
                    <asp:ListItem Value="S">State</asp:ListItem>
                    <asp:ListItem Value="AT">Applicant Type</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>--%>
                        <asp:DropDownList ID="ddlSubCriteria" runat="server" Height="22px" SkinID="ddl250"
                            Visible="False" AutoPostBack="True" 
                            onselectedindexchanged="ddlSubCriteria_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem>Male</asp:ListItem>
                            <asp:ListItem>Female</asp:ListItem>
                        </asp:DropDownList>
                <%-- </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDisplayCriteria" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr id="TrIns1" runat="server" visible="false">
        <td colspan="3">Institute Name
                        <asp:CheckBox ID="ChkAllInstitute" runat="server" 
                AutoPostBack="True" OnCheckedChanged="ChkAllInstitute_CheckedChanged"
                    Text="All Institutes" />                        
                    </td>
        </tr>
         <tr class="even" id="TrIns2" runat="server" visible="false">
         <td colspan="3">
             <%--<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional" Visible="false">
                 <ContentTemplate>--%>
                     <asp:TextBox ID="TxtInstituteName" runat="server" SkinID="txt756" Width="700px"></asp:TextBox>
                      <asp:AutoCompleteExtender ID="aceSearch" ServiceMethod="GetInstitutes" FirstRowSelected="true" 
                      OnClientItemSelected="OnInstituteSelected"  ServicePath="~/WS/Common.asmx"  TargetControlID="TxtInstituteName" 
                      UseContextKey ="true" ContextKey="0"
                      runat="server" MinimumPrefixLength="1"  CompletionInterval="0" EnableCaching="false"  CompletionSetCount="10" >
                                            </asp:AutoCompleteExtender>
                     <asp:HiddenField ID="HfInstitute" runat="server" Value=""  />
             <%--</ContentTemplate>
                 <Triggers>
                 <asp:AsyncPostBackTrigger ControlID="ChkAllInstitute" EventName="CheckedChanged" />
                 <asp:AsyncPostBackTrigger ControlID="ddlCourseName" EventName="SelectedIndexChanged" />
                 </Triggers>
             </asp:UpdatePanel>--%>
             </td>
         </tr>
        <tr runat="server">
            <td width="25%" colspan="3" align="left">
           
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                     <asp:Label ID="Label9" runat="server" Text="Registration Status &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                        <asp:CheckBox ID="ChkRegStatusAll" runat="server" AutoPostBack="True" OnCheckedChanged="ChkRegStatusAll_CheckedChanged"
                    Text="ALL" />                        
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="CheckBoxRegStatusList" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                
                
            </td>
        </tr>
        <tr runat="server" class="even">
            <td width="25%" colspan="3">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>

                        <asp:CheckBoxList ID="CheckBoxRegStatusList" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" AutoPostBack="True" 
                            onselectedindexchanged="CheckBoxRegStatusList_SelectedIndexChanged">
                        </asp:CheckBoxList>
                        <asp:HiddenField ID="HfRegStatusId" runat="server"  ViewStateMode="Enabled" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ChkRegStatusAll" EventName="CheckedChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <%--<tr id="Tr1" runat="server">
            <td width="25%" colspan="3" align="left">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="Label1" runat="server" Text="Registration Type"></asp:Label>
                        <asp:CheckBox ID="chkRegTypeAll" runat="server" AutoPostBack="True" 
                            Text="ALL" oncheckedchanged="chkRegTypeAll_CheckedChanged" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="CheckBoxRegTypeList" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>--%>
        <%--<tr id="Tr2" runat="server" class="even">
            <td width="25%" colspan="3">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:CheckBoxList ID="CheckBoxRegTypeList" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow" AutoPostBack="True" 
                            onselectedindexchanged="CheckBoxRegTypeList_SelectedIndexChanged" >
                        </asp:CheckBoxList>
                        <asp:HiddenField ID="HfRegTypeId" runat="server" ViewStateMode="Enabled" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="chkRegTypeAll" EventName="CheckedChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>--%>
    </table>
    <div style="text-align: right; margin-top: 10px">
             <asp:HiddenField ID="HfApplicantType" runat="server" />
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();" />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" /></div>
    
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
