<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadCourseDetails.aspx.cs" MasterPageFile="~/MasterPages/main.master" Inherits="HO_UploadCourseDetails" EnableEventValidation="false" Culture="en-GB" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>--%>
<script language="javascript" type="text/javascript" src="JavaScript/dates.js"></script>
<link href="~/Styles/Calender.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    function Compare_Dates(source, args) {

        var fromDate = new Date();
        var txtFromDate = document.getElementById('<%= txt_fr_date.ClientID %>').value;

        var aFromDate = txtFromDate.split("/");

        /*Start 'Date to String' conversion block, this block is required because javascript do not provide any direct function to convert 'String to Date' */

        var fdd = aFromDate[0]; //get the day part
        var fmm = aFromDate[1]; //get the month part
        var fyyyy = aFromDate[2]; //get the year part

        fromDate = Date.UTC(fyyyy, fmm, fdd);

        var toDate = new Date();
        var txtToDate = document.getElementById('<%= txt_to_date.ClientID %>').value;

            var aToDate = txtToDate.split("/");
            var tdd = aToDate[0]; //get the day part
            var tmm = aToDate[1]; //get the month part
            var tyyyy = aToDate[2]; //get the year part

            toDate = Date.UTC(tyyyy, tmm, tdd);

            if (toDate != null && fromDate >= toDate) {
                args.IsValid = false;
                //return false;
            }
            else {
                args.IsValid = true;
                //return true;
            }
        }
    </script>
    <div>
         <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%; text-align: left; top: 0; table-layout: auto"
            align="center" width="100%">
            <tr>
                <td colspan="6" align="center" style="height: 40px; width: 100%">
                    <asp:Label ID="lbl_header" runat="server" Text="UPLOAD COURSE DOCUMENT" Font-Bold="true" Font-Size="Large" Font-Underline="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="width: 100%;" valign="top" align="center">
                    <table cellpadding="4" cellspacing="0" style="width: 100%; table-layout: fixed; text-align: left; top: 0;"
                        align="center" frame="box" width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_course_category" runat="server" Text="Course Category *" />
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddl_course_category" runat="server" Width="95%"
                                    AutoPostBack="false" TabIndex="1" />
                                <asp:CascadingDropDown ID="cdd_course_category" runat="server" Category="CourseCategory" LoadingText="Loading Course Categories..."
                                    ServiceMethod="GetCourseCategoryList" ServicePath="~/Webservices/dropdowns.asmx" PromptText="[Select Course Category]"
                                    SelectedValue="" TargetControlID="ddl_course_category"/>
                                <asp:RequiredFieldValidator ID="rfv_course_cat" runat="server" ValidationGroup="s" SetFocusOnError="true"
                                    ErrorMessage="Select Course Category" ControlToValidate="ddl_course_category" Display="None" ToolTip="Select Course Category" />
                                <asp:ValidatorCalloutExtender ID="vce_centre" runat="server" Enabled="True" TargetControlID="rfv_course_cat" />

                            </td>
                            <td>
                                <asp:Label ID="lbl_courses" runat="server" Text="Select Course *" CssClass="form-label" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_course_name" runat="server" Width="95%"
                                    AutoPostBack="True" TabIndex="2" OnSelectedIndexChanged="ddl_course_name_SelectedIndexChanged" />
                                <asp:CascadingDropDown ID="cdd_course_name" runat="server" Category="CourseName" LoadingText="Loading Courses..."
                                    ServiceMethod="GetCoursesListForCategory" ServicePath="~/Webservices/dropdowns.asmx" PromptText="[Select Course]"
                                    SelectedValue="" TargetControlID="ddl_course_name" ParentControlID="ddl_course_category"/>
                                <asp:RequiredFieldValidator ID="rfv_course_name" runat="server" ValidationGroup="s" SetFocusOnError="true"
                                    ErrorMessage="Select Course" ControlToValidate="ddl_course_name" Display="None" ToolTip="Select Course Name" />
                                <asp:ValidatorCalloutExtender ID="vce_course_name" runat="server" Enabled="True" TargetControlID="rfv_course_name" />

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_fr_date" Text="From Date" runat="server" CssClass="form-label" />
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fr_date" runat="server" TabIndex="52" Width="100px" AutoCompleteType="Disabled" />
                                <img id="img2" runat="server" Visible="false" src="../images/calendaricon.jpg" alt="Calandar" style="width: 20px;
                                    height: 22px; vertical-align: top;" />
                                <br />
                                <asp:CalendarExtender ID="ce_fr_date" runat="server" TargetControlID="txt_fr_date" PopupButtonID="img2"
                                    Enabled="true" Format="dd-MMM-yyyy" CssClass="cal_Theme1" />
                                <asp:RequiredFieldValidator ID="rfv_txt_fr_date" runat="server" ValidationGroup="s" SetFocusOnError="true"
                                    ErrorMessage="Enter From Date" ControlToValidate="txt_fr_date" Display="None" ToolTip="Select From Date" />
                                <asp:ValidatorCalloutExtender ID="vce_fr_date" runat="server" Enabled="True"
                                    TargetControlID="rfv_txt_fr_date" />
                            </td>
                            <td >
                                <asp:Label ID="lbl_to_date" Text="To Date" runat="server" CssClass="form-label" />
                            </td>
                            <td>
                                <asp:TextBox ID="txt_to_date" runat="server" TabIndex="56" Width="100px" AutoCompleteType="Disabled" />
                                <img id="img1" runat="server" Visible="false" src="../images/calendaricon.jpg" alt="Calandar" style="width: 20px;
                                    height: 22px; vertical-align: top;" />
                                <br />
                                <asp:CalendarExtender ID="ce_to_date" runat="server" TargetControlID="txt_to_date" PopupButtonID="img1"
                                    Enabled="true" Format="dd-MMM-yyyy" CssClass="cal_Theme1" />
                                <%--<asp:RequiredFieldValidator ID="rfv_txt_to_date" runat="server" ValidationGroup="s" SetFocusOnError="true"
                                    ErrorMessage="Enter To Date" ControlToValidate="txt_to_date" Display="None" ToolTip="Select To Date" />--%>
                                <%--<asp:ValidatorCalloutExtender ID="vce_to_date" runat="server" Enabled="True"
                                    TargetControlID="rfv_txt_to_date" />--%>
                               
                                <asp:CustomValidator ID="cvd_txt_to_date" runat="server" ClientValidationFunction="Compare_Dates"
                                    ErrorMessage="To Date should be greater than From Date" ControlToValidate="txt_to_date"
                                    ValidateEmptyText="false" Display="None" ValidationGroup="s" SetFocusOnError="true" />
                                <asp:ValidatorCalloutExtender ID="vce_txt_to_date" runat="server" TargetControlID="cvd_txt_to_date"
                                    HighlightCssClass="validatorCalloutHighlight"/>
                            </td>
                         
                        </tr>
                        <tr>
                            <td align="center" style="width: 100%" colspan="4">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align: left; border-style: solid; border-color: ActiveBorder; padding-top: 5px; padding-bottom: 5px">
                                    <tr>
                                        <td style="width: 80%; padding-left: 10px" align="left">
                                            <table width="100%">
                                                <tr>
                                                    <td class="form-label" align="center" valign="middle" colspan="2">
                                                        <asp:Label ID="lbl_app_form" runat="server" Text="<small>Select Course Document File for Uploading <i>(Should be a PDF file of maximum size 5 MB)</i></i></small>"
                                                            Font-Underline="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="form-label-bold" align="center" width="60%">
                                                        <asp:FileUpload ID="fu_course_file" runat="server" Width="100%" Enabled="true" EnableViewState="true" TabIndex="5" />
                                                        <asp:RequiredFieldValidator ID="rfv_course_file" runat="server" ErrorMessage="Specify course file to be uploaded!"
                                                            ControlToValidate="fu_course_file" Display="None" SetFocusOnError="true" Enabled="true"
                                                            ValidationGroup="s" />
                                                        <asp:ValidatorCalloutExtender ID="vce_course_file" Enabled="true" runat="server" TargetControlID="rfv_course_file"/>
                                                        <asp:RegularExpressionValidator ID="rev_course_file" runat="server" Display="None"
                                                            SetFocusOnError="true" ControlToValidate="fu_course_file" ErrorMessage="Only PDF files allowed"
                                                            ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.[Pp][Dd][Ff])$" ValidationGroup="s" />
                                                        <asp:ValidatorCalloutExtender ID="vce_rev_course_file" runat="server" Enabled="True"
                                                            TargetControlID="rev_course_file" />
                                                    </td>
                                                    <td align="center" width="40%">
                                                        <asp:Button ID="btn_upload_course_file" runat="server" Font-Bold="true" Text="Upload Course File"
                                                            ValidationGroup="s" OnClick="btn_upload_course_file_Click" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td align="center" colspan="2" style="padding-top: 20px">
                                                        <asp:Label ID="lbl_fu_msg" runat="server" CssClass="label_msg_red"
                                                            EnableViewState="False" ForeColor="Red" Font-Bold="True" Font-Names="Arial" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        <tr>
                            <td align="center" colspan="4">
                                <asp:UpdateProgress ID="progress1" runat="server" DisplayAfter="10">
                                    <ProgressTemplate>
                                        <div class="progress">
                                            <asp:Image ID="imgProgress" ImageUrl="~/images/bert.gif" runat="server" />
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="width: 100%" valign="top">
                    <table cellpadding="4" cellspacing="0" style="width: 100%; table-layout: fixed; text-align: left; top: 0;"
                        align="center" frame="box">
                       <tr>
                            <td align="center" width="100%" colspan="4">
                                <asp:GridView ID="grd_course_file" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                    Font-Size="Small" EmptyDataText="No records" DataKeyNames="course_eligibility_id" ForeColor="#333333">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="h1" Target="_blank" runat="server" Text="View" NavigateUrl='<%#"~/ImageHandler/DisplayDocuments.aspx?ID="+Eval("course_eligibility_id")+"&download=0&TYP=1" %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="h2" runat="server" Text="Download" NavigateUrl='<%#"~/ImageHandler/DisplayDocuments.aspx?ID="+Eval("course_eligibility_id")+"&download=1&TYP=1" %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="from_dt" HeaderText="From Date" SortExpression="from_dt"
                                            ReadOnly="true" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="False" />
                                        <asp:BoundField DataField="to_dt" HeaderText="To Date" SortExpression="to_dt"
                                            ReadOnly="true" DataFormatString="{0:dd-MM-yyyy}" HtmlEncode="False" />
                                        <asp:BoundField DataField="is_latest" HeaderText="IsLatest Value" ItemStyle-HorizontalAlign="Center"/>
                                        <asp:BoundField DataField="entered_by" HeaderText="Uploaded By" />
                                        <asp:BoundField DataField="created_on" HeaderText="Uploaded Date" ReadOnly="true"
                                            DataFormatString="{0:dd-MM-yyyy hh:mm tt}" HtmlEncode="False" />
                                    </Columns>
                                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                                    <FooterStyle BackColor="#990000" ForeColor="White" Font-Bold="True" />
                                    <PagerStyle ForeColor="#333333" HorizontalAlign="Center" BackColor="#FFCC66" />
                                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
