<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/MyInfo.master" CodeFile="TrainingCalendarReportFilter.aspx.cs" Inherits="Common_TrainingCalendarReportFilter" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    Training Calendar Report   
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
      <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        function OpenWindow() {
            var rb = document.getElementById("<%=Rdsearchby.ClientID%>");
            var radio = rb.getElementsByTagName("input");
            var isChecked = false;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    isChecked = true;
                    if (i == 0) {
                        var itemvalue = "C";
                        var centreId
                        var centrname
                        if (!isSelected("<%=ddlCentre.ClientID %>", "Centre Name"))
                            return false;
                        else
                            centreId = document.getElementById('<%=ddlCentre.ClientID %>').value;
                        var value = document.getElementById("<%=ddlCentre.ClientID%>"); 
                        centrname = value.options[value.selectedIndex].text;
                        //centrname = document.getElementById('<%=ddlCentre.ClientID %>').value;
                        if (!isBlankDate("<%=txtdateFrom.ClientID %>", "Period Date From", "dd-MMM-yyyy"))
                            return false;
                        if (!isDate("<%=txtdateFrom.ClientID %>", "Period Date From", "dd-MMM-yyyy"))
                            return false;
                        if (!isBlankDate("<%=txtdateto.ClientID %>", "Period Date To", "dd-MMM-yyyy"))
                            return false;
                        if (!isDate("<%=txtdateto.ClientID %>", "Period Date To", "dd-MMM-yyyy"))
                            return false;
                        var periodFromDate = document.getElementById('<%=txtdateFrom.ClientID %>').value;
                        var periodToDate = document.getElementById('<%=txtdateto.ClientID %>').value;
                        if (!CompareDates(periodFromDate, periodToDate, "Period Date From should be less than Period Date To", true))
                            return false;

                       
                        var periodFromDate = document.getElementById('<%=txtdateFrom.ClientID %>').value;
                        var periodToDate = document.getElementById('<%=txtdateto.ClientID %>').value;
                            //var curr_Date= new SimpleDateFormat("dd/mm/yyyy");                           
                        var stDate = new Date(periodFromDate);
                        var enDate = new Date(periodToDate);
                            var compDate = enDate - stDate;
                            //var fdate=enDate-curr_Date;
                            var diffInDays = Math.round(compDate / 86400000);
                            if (diffInDays > 365) {
                                alert("Report of only one year duration may be generated.");
                                return false;
                            }
                        window.open("../HO/Rpt/TrainingCalendarReport.aspx?itemvalue=" + itemvalue + "&centreId=" + centreId + "&periodFromDate=" + periodFromDate + "&periodToDate=" + periodToDate + "&centrname=" + centrname);
                        return false;
                    }

                    if (i == 1) {
                        var itemvalue = "Co";
                        var courseId
                        var courseName
                        if (!isSelected("<%=ddlCourse.ClientID %>", "Course Name"))
                            return false;
                        else
                            courseId = document.getElementById('<%=ddlCourse.ClientID %>').value;
                        var value = document.getElementById("<%=ddlCourse.ClientID%>");
                        courseName = value.options[value.selectedIndex].text;
                        if (!isBlankDate("<%=txtdateFrom.ClientID %>", "Period Date From", "dd-MMM-yyyy"))
                            return false;
                        if (!isDate("<%=txtdateFrom.ClientID %>", "Period Date From", "dd-MMM-yyyy"))
                            return false;
                        if (!isBlankDate("<%=txtdateto.ClientID %>", "Period Date To", "dd-MMM-yyyy"))
                            return false;
                        if (!isDate("<%=txtdateto.ClientID %>", "Period Date To", "dd-MMM-yyyy"))
                            return false;
                        var periodFromDate = document.getElementById('<%=txtdateFrom.ClientID %>').value;
                        var periodToDate = document.getElementById('<%=txtdateto.ClientID %>').value;
                        if (!CompareDates(periodFromDate, periodToDate, "Period Date From should be less than Period Date To", true))
                            return false;

                        var periodFromDate = document.getElementById('<%=txtdateFrom.ClientID %>').value;
                        var periodToDate = document.getElementById('<%=txtdateto.ClientID %>').value;
                        //var curr_Date= new SimpleDateFormat("dd/mm/yyyy");                           
                        var stDate = new Date(periodFromDate);
                        var enDate = new Date(periodToDate);
                        var compDate = enDate - stDate;
                        //var fdate=enDate-curr_Date;
                        var diffInDays = Math.round(compDate / 86400000);
                        if (diffInDays > 365) {
                            alert("Duration of Date only 1 year will be allowed.");
                            return false;
                        }
                        window.open("../HO/Rpt/TrainingCalendarReport.aspx?itemvalue=" + itemvalue + "&courseId=" + courseId + "&periodFromDate=" + periodFromDate + "&periodToDate=" + periodToDate + "&courseName=" + courseName);
                        return false;
                    }

                    if (i == 2) {
                        var itemvalue = "A";                       

                        if (!isBlankDate("<%=txtdateFrom.ClientID %>", "Period Date From", "dd-MMM-yyyy"))
                            return false;
                        if (!isDate("<%=txtdateFrom.ClientID %>", "Period Date From", "dd-MMM-yyyy"))
                            return false;
                        if (!isBlankDate("<%=txtdateto.ClientID %>", "Period Date To", "dd-MMM-yyyy"))
                            return false;
                        if (!isDate("<%=txtdateto.ClientID %>", "Period Date To", "dd-MMM-yyyy"))
                            return false;
                        var periodFromDate = document.getElementById('<%=txtdateFrom.ClientID %>').value;
                        var periodToDate = document.getElementById('<%=txtdateto.ClientID %>').value;
                        if (!CompareDates(periodFromDate, periodToDate, "Period Date From should be less than Period Date To", true))
                            return false;

                        var periodFromDate = document.getElementById('<%=txtdateFrom.ClientID %>').value;
                        var periodToDate = document.getElementById('<%=txtdateto.ClientID %>').value;
                        //var curr_Date= new SimpleDateFormat("dd/mm/yyyy");                           
                        var stDate = new Date(periodFromDate);
                        var enDate = new Date(periodToDate);
                        var compDate = enDate - stDate;
                        //var fdate=enDate-curr_Date;
                        var diffInDays = Math.round(compDate / 86400000);
                        if (diffInDays > 365) {
                            alert("Duration of Date only 1 year will be allowed.");
                            return false;
                        }
                        window.open("../HO/Rpt/TrainingCalendarReport.aspx?itemvalue=" + itemvalue + "&periodFromDate=" + periodFromDate + "&periodToDate=" + periodToDate);
                        return false;
                    }

                }
            }

        }

    </script>
    <style type="text/css">
        .PromptCSS {
            color: Blue;
            font-size: small;
            font-style: italic;
            font-weight: bold;
            font-family: CourierNew;
            height: 20px;
            margin-left: 100px;
        }
    </style>
    <asp:Label ID="lblerror" runat="server"></asp:Label>
    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">             
        <tr class="gdalternate1">                                                
                                <td width="20%" valign="top">
                                     <asp:Label ID="lblReport" runat="server" SkinID="CaptionLabel" Text="Search By:- &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                </td>                    
            <td>             
                       <asp:RadioButtonList ID="Rdsearchby" runat="server" RepeatDirection="Horizontal"
                                TabIndex="2" Width="412px" AutoPostBack="True" Style="height: 27px" Font-Bold="True" OnSelectedIndexChanged="Rdsearchby_SelectedIndexChanged" >
                                <asp:ListItem Value="C">CentreWise</asp:ListItem>
                                <asp:ListItem Value="Co">CourseWise</asp:ListItem>
                                <asp:ListItem Value="A" Selected="True">All</asp:ListItem>                               
                           </asp:RadioButtonList>
                   
            </td>         
        </tr>
           <tr id="TrCentre" runat="server"  class="gdalternate1" visible="false">                                                
                                <td width="20%" valign="top">
                                     <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Select Nielit Centre:"></asp:Label>
                                </td>        
         
            <td> 
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>           
                        <asp:DropDownList ID="ddlCentre" runat="server" Height="22px" SkinID="ddl250" 
                            AutoPostBack="True"  >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>                            
                        </asp:DropDownList>
                        </ContentTemplate>
                </asp:UpdatePanel>
                   
            </td>
                    
        </tr>
        <tr id="TrcourseInput" runat="server"  class="gdalternate1" visible="false">
            <td width="20%" valign="top">
                                     <asp:Label ID="lblCourse" runat="server" SkinID="CaptionLabel" Text="Select Nielit Course:"></asp:Label>
                                </td> 
            <td> 
                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>                 
                        <asp:DropDownList ID="ddlCourse" runat="server" Height="22px" SkinID="ddl250" 
                            AutoPostBack="True" >
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>                            
                        </asp:DropDownList>
                        </ContentTemplate>
                </asp:UpdatePanel>
            </td>
                    
        </tr>

        <tr id="trBatchdate" runat="server">
            <td>
                <asp:Label ID="lbldatefrom" runat="server" SkinID="CaptionLabel" Text="Period Date From"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbldateto" runat="server" SkinID="CaptionLabel" Text="Period Date To"></asp:Label>
            </td>
            
        </tr>

        <tr id="trdatefrom">
            
            <td width="33%">
                <asp:TextBox ID="txtdateFrom" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                <img id="imgFrom" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtdateFrom"
                    Format="dd-MMM-yyyy" PopupButtonID="imgFrom">
                </asp:CalendarExtender>
            </td>
            <td width="33%">
                <asp:TextBox ID="txtdateto" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                <img id="imgto" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtdateto"
                    Format="dd-MMM-yyyy" PopupButtonID="imgto">
                </asp:CalendarExtender>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px">
        <asp:Button ID="btnView" runat="server" Text="View" OnClientClick="return OpenWindow();"  />
        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click"   />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                   
                    <asp:HiddenField ID="HNonAfflAfflInst" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
