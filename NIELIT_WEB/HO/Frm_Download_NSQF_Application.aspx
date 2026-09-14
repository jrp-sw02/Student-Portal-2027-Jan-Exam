<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/main.master"
    CodeFile="Frm_Download_NSQF_Application.aspx.cs" Inherits="HO_Frm_Download_NSQF_Application"  Debug="false" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Download NSQF Application"></asp:Label>
</asp:Content>

<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <asp:updatepanel enableviewstate="true" id="upBread" updatemode="Conditional" runat="server">
        <contenttemplate>
            <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
        </contenttemplate>
    </asp:updatepanel>
</asp:content>

<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function validatefilter() {

            if (document.getElementById("<%=ddl_year.ClientID %>").disabled == false) {
                if (!isSelected("<%=ddl_year.ClientID %>", "year"))
                    return false;
            }
            if (!isSelected("<%=ddl_exam_month.ClientID %>", "exam month"))
                return false;
            if (!isSelected("<%=ddl_course_name.ClientID %>", "Course Name"))
                return false;         
        }
        function OpenWindow() {
        }
    </script>
    <div class="box" id="DivSearch" runat="server">
        <%--<table id="tblShow" visible="true" runat="server" border="0" class="sample3" style="width: 100%; text-align: left">
           <tr class="gdrow1">
            <td style="width: 35%;" valign="top">
                <asp:Label ID="lbldemandnote" runat="server" SkinID="CaptionLabel" Text="Enter Demand Note Number"></asp:Label>
            </td>
            <td style="width: 35%;" valign="top">
                <asp:TextBox ID="txtdemandnote" runat="server"></asp:TextBox>
            </td>
            <td align="center" style="width: 30%;">
                <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnShow_Click" />
            </td>
               </tr>
           <tr id="Tr1" class="gdrow1" runat="server" border="0" cellpadding="3" cellspacing="1">
            <td style="width: 35%;" valign="top">
                <asp:Label ID="lbl_amount_text" runat="server" SkinID="CaptionLabel" Text="Amount"></asp:Label>
            </td>
            <td style="width: 35%;" valign="top">
                <asp:Label ID="lbl_amount" runat="server"></asp:Label>
            </td>
            <td align="center" style="width: 30%;">
                &nbsp;</td>
               </tr>
        </table>--%>

           <table class="sample2" width="100%" border="0">
           <tr id="Tr2" class="gdrow1" runat="server" border="0">
               <td style="width: 100%;" valign="top">
        <asp:label id="Lblerror" runat="server" enabletheming="False" cssclass="error" width="100%"
        visible="False"></asp:label>
                   </td>
               </tr>
               </table>

    <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td class="auto-style1">
                <asp:label id="lbl_exam_year" runat="server" skinid="CaptionLabel" text="Exam Year &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td class="auto-style1">
                <asp:label id="lbl_exam_month" runat="server" skinid="CaptionLabel" text="Exam Month &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td class="auto-style1">
                <asp:label id="lbl_course_type" runat="server" skinid="CaptionLabel" text="Course Type &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>

            </td>
        </tr>
        <tr class="even">
            <td class="auto-style3">
                <asp:dropdownlist id="ddl_year" runat="server" Enabled="true"  height="22px" skinid="ddl250" AutoPostBack="true" OnSelectedIndexChanged="ddl_year_SelectedIndexChanged1" >
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
            </td>
            <td class="auto-style3">
                <asp:dropdownlist id="ddl_exam_month" runat="server" AutoPostBack="true" height="22px" Enabled="false"
                    onselectedindexchanged="ddl_exam_month_SelectedIndexChanged" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                    <asp:ListItem Value="1">January</asp:ListItem>
                    <asp:ListItem Value="2">February</asp:ListItem>
                    <asp:ListItem Value="3">March</asp:ListItem>
                    <asp:ListItem Value="4">April</asp:ListItem>
                    <asp:ListItem Value="5">May</asp:ListItem>
                    <asp:ListItem Value="6">June</asp:ListItem>
                    <asp:ListItem Value="7">July</asp:ListItem>
                    <asp:ListItem Value="8">August</asp:ListItem>
                    <asp:ListItem Value="9">September</asp:ListItem>
                    <asp:ListItem Value="10">October</asp:ListItem>
                    <asp:ListItem Value="11">November</asp:ListItem>
                    <asp:ListItem Value="12">December</asp:ListItem>
                </asp:dropdownlist>
            </td>
            <td class="auto-style3">
                <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>--%>
                <%--</ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCourseCategry" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>--%>
                <asp:DropDownList id="ddl_course_type" runat="server" AutoPostBack="true" height="22px" Enabled="false" 
                    skinid="ddl250"   OnSelectedIndexChanged="ddl_course_type_SelectedIndexChanged">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                    <asp:listitem value="1">Theory</asp:listitem>
                    <asp:listitem value="2">Practical</asp:listitem>
                    <asp:listitem value="3">Both Theory and Practical</asp:listitem>
                    <asp:listitem value="4">Only Practical</asp:listitem>
                </asp:DropDownList>
            </td>
        </tr>
        <%--<tr id="TrExamHead" runat="server">
            <td class="auto-style1">
            </td>
            <td class="auto-style1">
            </td>
            <td class="auto-style1">
            </td>
        </tr>      --%>  
       
        
        <tr>
            <td class="auto-style1">
                <asp:label id="lbl_course_name" runat="server" skinid="CaptionLabel" text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;">
                </asp:label>
            </td>
            <td class="auto-style1">
                &nbsp;</td>
            <td class="auto-style1">
                &nbsp;</td>
        </tr>
        <tr class="even">
            <td class="auto-style3">
                <asp:dropdownlist id="ddl_course_name" runat="server" AutoPostBack="true" Enabled="false"
                    onselectedindexchanged="ddl_course_name_SelectedIndexChanged" skinid="ddl250">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>
            </td>
            <td class="auto-style3">
                &nbsp;</td>
            <td class="auto-style3">
                &nbsp;</td>
        </tr>
               
        
        </table>

        <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr id="Tr5" runat="server">
            <td class="auto-style4">
                <asp:label runat="server" id="lblApplicationsDetails" cssclass="error" text="" visible="False" Width="99%" enabletheming="False"></asp:label>
                </td>                      
        </tr>
            <tr id="Tr4" runat="server" visible="false">
            <td class="auto-style4">
                <asp:label runat="server" id="lbl_download_date" cssclass="error" text="" visible="False" Width="99%" enabletheming="False"></asp:label>
                </td>                      
        </tr>
       </table>
           
        <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
        <tr id="Tr6" runat="server" align="center">
            <td class="auto-style4" >                             
                <asp:Button ID="btn_download" runat="server" Enabled="false" OnClick="btn_download_Click" Text="Download" />                           
            </td>    
            <td class="auto-style5" >                             
                                           
                <asp:Button ID="btn_tentative_date" runat="server" Text="Add Tentative Date" OnClick="btn_tentative_date_Click" Width="141px" />
                                           
            </td>     
            <td >                             
                                           
                <asp:Button ID="btn_reset" runat="server" Text="Reset Data" OnClick="btn_reset_Click" />
                                           
            </td>       
        </tr>
    </table>

    <table id="tbl_tent" class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0" runat="server" visible="false">
       <tr id="tr" runat="server">
            <td class="auto-style4" align="left">                             
                <%--<asp:label id="lbl_course" runat="server" skinid="CaptionLabel" text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:label>--%>   
                <asp:label id="lbl_tentative_date" runat="server" skinid="CaptionLabel" text="Tentative Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:label>                        
                </td>
               
            <td class="auto-style5" align="left">                    
                                                       
                </td>     
            <td >                             
                                           
            </td>       
        </tr>

            <tr id="tr1" runat="server" align="center" cellpadding="2" cellspacing="0">
            <td class="auto-style4" align="left" >                             
                <%--<asp:dropdownlist id="ddl_course" runat="server" height="22px" skinid="ddl250" AutoPostBack="true" 
                    OnSelectedIndexChanged="ddl_year_SelectedIndexChanged">
                    <asp:listitem value="0">--Select One--</asp:listitem>
                </asp:dropdownlist>--%>                           
                 <asp:TextBox ID="txt_add_date" runat="server" SkinID="txt210" MaxLength="11"></asp:TextBox>
                    <img runat="server" id="imgFrom" src="../images/calendaricon.jpg" style="width: 20px;
                        height: 22px; vertical-align: top;" /></td>
               
            <td class="auto-style5" align="centre" >                    
                 &nbsp;<asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_add_date"
                        Format="dd-MMM-yyyy" PopupButtonID="imgFrom">
                    </asp:CalendarExtender>                                                           
                                           
                <asp:Button ID="btn_add" runat="server" Text="Add" OnClick="btn_add_Click" Width="95px" />
                                           
            </td>     
            <td align="left">                             
                                           
                &nbsp;</td>       
        </tr>

        <tr id="tr3" runat="server" align="center" cellpadding="2" cellspacing="0">
            <td class="auto-style4" align="left">   
                  <asp:label runat="server" id="lbl_add" cssclass="error" text="" visible="False" Width="99%" enabletheming="False"></asp:label>                        
                </td>
               
            <td class="auto-style5" >                    
                                                    
            </td>     
            <td >                             
                                           
            </td>       
        </tr>
    </table>
        

    </div>
</asp:Content>

<asp:Content ID="Content7" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1
        {
            height: 23px;
        }
        .error
        {}
        .auto-style3
    {
        height: 26px;
    }
        .auto-style4
        {
            width: 249px;
        }
        .auto-style5
        {
            width: 247px;
        }
    </style>
</asp:Content>
