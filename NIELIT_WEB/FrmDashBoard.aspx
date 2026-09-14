
<%@ Page Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true"
    CodeFile="FrmdashBoard.aspx.cs" Inherits="FrmdashBoard" Debug="true" %>

<%@ Register Src="UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc1" %>
<%@ Register Src="UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    &nbsp;<asp:Label ID="lblHeading" runat="server" Text="My Dashboard"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">


        function showForm(url) {
            window.open(url, "AppForm", "width=980,height=450,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes");
            return false;
        }
        if (window.top.location.href.indexOf('FrmdashBoard.aspx') >= 0)
            window.top.location.href = 'MainPage.aspx'


    </script>
    <div id="divInstitute" runat="server" visible="false">
        <div class="summary_block1" id="Coursediv" runat="server" visible="false">
            <span> Course Registration Application Notification </span>
            <br />
            <asp:UpdatePanel EnableViewState="true" ID="UpdatePanelcoursereg" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblCourseMsg" class="error" EnableTheming="False" runat="server" ForeColor="#FF3300"
                        Visible="False"></asp:Label>
                    <br />
                    <asp:GridView ID="gvCourse" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                        Width="100%" OnRowDataBound="gvCourse_RowDataBound" OnSorting="gvCourse_Sorting"
                        HeaderStyle-Font-Size="12px">
                        <Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle Width="1%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Name" HeaderText="Course">
                                <HeaderStyle Width="13%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="Prejected" HeaderText="Rejected" Target="_self">
                                <HeaderStyle HorizontalAlign="left" Width="10%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="Pverification" HeaderText="Pending to Verify" Target="_self">
                                <HeaderStyle HorizontalAlign="left" Width="16%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="PPstatus" HeaderText="Pending To Pay" Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&st=mnp&Examid={4}&Status="
                                DataTextField="PNotPaid" HeaderText="Marked Not Paid" Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="PSubmit" HeaderText="Pending Dispatch" Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="PWaiting" HeaderText="Waiting Ackn." Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                <asp:LinkButton ID="lnkCourse" runat="server" OnClick="lnkCourse_Click">View Detail</asp:LinkButton>
            </div>
            <div id="div1" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="UpdatePanelcoursereg1" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar2" runat="server" OnPageIndexChanged="PageIndexChanged2" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="summary_block1" style="width: 100%;" id="givInstitute" runat="server"
            visible="false">
            <span>Certificate Exam Application Notification </span>
            <br />
            <asp:UpdatePanel EnableViewState="true" ID="UpdatePanelcertificateexam" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblCertificateMsg" class="error" EnableTheming="False" runat="server"
                        ForeColor="#FF3300" Visible="False"></asp:Label>
                    <asp:GridView ID="gvCertificate" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                        Width="100%" OnRowDataBound="gvCertificate_RowDataBound" OnSorting="gvCertificate_Sorting"
                        HeaderStyle-Font-Size="12px">
                        <Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle Width="1%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Name" HeaderText="Course">
                                <HeaderStyle Width="10%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="Prejected" HeaderText="Rejected" Target="_self">
                                <HeaderStyle HorizontalAlign="left" Width="8%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="Pverification" HeaderText="Pending to Verify" Target="_self">
                                <HeaderStyle HorizontalAlign="left" Width="14%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="PPstatus" HeaderText="Pending To Pay" Target="_self">
                                <HeaderStyle HorizontalAlign="left" Width="13%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&st=mnp&Examid={4}&Status="
                                DataTextField="PNotPaid" HeaderText="Marked Not Paid" Target="_self">
                                <HeaderStyle HorizontalAlign="left" Width="13%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="PSubmit" HeaderText="Pending Dispatch" Target="_self">
                                <HeaderStyle HorizontalAlign="left" Width="14%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="PWaiting" HeaderText="Waiting Ackn." Target="_self">
                                <HeaderStyle HorizontalAlign="left" Width="11%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                <asp:LinkButton ID="lnkCertificate" runat="server" OnClick="lnkCertificate_Click">View Detail</asp:LinkButton>
            </div>
            <div id="div3" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="UpdatePanelcertificateexam1" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar4" runat="server" OnPageIndexChanged="PageIndexChanged4" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="summary_block1" id="CourseExamdiv" runat="server" visible="false">
            <span>Course Exam Application Notification </span>
            <br />
            <br />
            <asp:UpdatePanel EnableViewState="true" ID="UpdatePanelcourseexam" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblCourseExam" class="error" EnableTheming="False" runat="server"
                        ForeColor="#FF3300" Visible="False"></asp:Label>
                    <br />
                    <asp:GridView ID="gvCourseExam" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                        Width="100%" OnSorting="gvCourseExam_Sorting" OnRowDataBound="gvCourseExam_RowDataBound"
                        HeaderStyle-Font-Size="12px">
                        <Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle Width="1%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Name" HeaderText="Name">
                                <HeaderStyle Width="13%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="Prejected" HeaderText="Rejected" Target="_self">
                                <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="Pverification" HeaderText="Pending to Verify" Target="_self">
                                <HeaderStyle HorizontalAlign="left" Width="16%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="PPstatus" HeaderText="Pending To Pay" Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&st=mnp&Examid={4}&Status="
                                DataTextField="PNotPaid" HeaderText="Marked Not-Paid" Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="PSubmit" HeaderText="Pending Submit" Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="PWaiting" HeaderText="Waiting Ackn." Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                <%--<asp:LinkButton  ID="LinkButton3" runat="server">View Detail</asp:LinkButton>--%>
                <asp:LinkButton ID="lnkCourseExam" runat="server" OnClick="lnkCourseExam_Click">View Detail</asp:LinkButton>
            </div>
            <div id="div2" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="UpdatePanelcourseexam1" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar3" runat="server" OnPageIndexChanged="PageIndexChanged3" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
		
		<div class="summary_block1" id="divProject" runat="server" visible="false">
            <span>Course Project Application Notification </span>
            <br />
            <br />
            <asp:UpdatePanel EnableViewState="true" ID="UpdatePanel3" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblCourseProject" class="error" EnableTheming="False" runat="server"
                        ForeColor="#FF3300" Visible="False"></asp:Label>
                    <br />
                    <asp:GridView ID="gvCourseProject" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                        Width="100%" OnSorting="gvCourseProject_Sorting" OnRowDataBound="gvCourseProject_RowDataBound"
                        HeaderStyle-Font-Size="12px">
                        <%--<Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle Width="1%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Name" HeaderText="Name">
                                <HeaderStyle Width="13%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Status="
                                DataTextField="Prejected" HeaderText="Rejected" Target="_self">
                                <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Status="
                                DataTextField="Pverification" HeaderText="Pending to Verify" Target="_self">
                                <HeaderStyle HorizontalAlign="left" Width="16%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Status="
                                DataTextField="PPstatus" HeaderText="Pending To Pay" Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&st=mnp&Status="
                                DataTextField="PNotPaid" HeaderText="Marked Not-Paid" Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Status="
                                DataTextField="PSubmit" HeaderText="Pending Submit" Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Status="
                                DataTextField="PWaiting" HeaderText="Waiting Ackn." Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                        </Columns>--%>
                        <Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle Width="1%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Name" HeaderText="Name">
                                <HeaderStyle Width="13%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>
                             <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="Prejected" HeaderText="Rejected" Target="_self">
                                <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="Pverification" HeaderText="Pending to Verify" Target="_self">
                                <HeaderStyle HorizontalAlign="left" Width="16%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="PPstatus" HeaderText="Pending To Pay" Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&st=mnp&Examid={4}&Status="
                                DataTextField="PNotPaid" HeaderText="Marked Not-Paid" Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="PSubmit" HeaderText="Pending Submit" Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="ID,CourseCategoryID,ServiceID,ApplType,Examid"
                                DataNavigateUrlFormatString="Admin/CourseRegStudentStatus.aspx?CourseID={0}&CourseCategoryID={1}&ServiceID={2}&ApplTypeID={3}&Examid={4}&Status="
                                DataTextField="PWaiting" HeaderText="Waiting Ackn." Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="left" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:HyperLinkField>
                        </Columns>

                        <PagerSettings Visible="False" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                <%--<asp:LinkButton  ID="LinkButton3" runat="server">View Detail</asp:LinkButton>--%>
                <asp:LinkButton ID="lnkProjectExam" runat="server" OnClick="lnkProjectExam_Click">View Detail</asp:LinkButton>
            </div>
            <div id="div5" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="UpdatePanel4" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar5" runat="server" OnPageIndexChanged="PageIndexChanged3" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

    </div>

    <div class="summary_block1" id="regCentre" runat="server" visible="false">
        <span>Certificate Exam Application Notification </span>
        <br />
        <div id="divGrid" runat="server">
            <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Label ID="Label1" class="error" EnableTheming="False" runat="server" ForeColor="#FF3300"
                        Visible="False"></asp:Label>
                    <asp:GridView ID="gvRegCentre" runat="server" DataKeyNames="ID" AutoGenerateColumns="False"
                        Width="100%" OnRowDataBound="gvRegCentre_RowDataBound" OnSorting="gvRegCentre_Sorting">
                        <Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle Width="1%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
                                <HeaderStyle Width="1%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExamName" HeaderText="ExamName" SortExpression="ExamName">
                                <HeaderStyle Width="20%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PBC" HeaderStyle-Width="25%" HeaderText="Paid By Candidate"
                                SortExpression="PBC"></asp:BoundField>
                            <asp:BoundField DataField="Dipetched" HeaderText="Dispatched" SortExpression="Dipetched">
                                <HeaderStyle Width="2%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Received" HeaderText="Received" SortExpression="Received">
                                <HeaderStyle Width="2%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DDpending" HeaderText="DD Verify Pending" SortExpression="DDpending">
                                <HeaderStyle Width="25%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="KIA" HeaderText="KIA" SortExpression="KIA">
                                <HeaderStyle Width="2%" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Verify" HeaderText="Verified" SortExpression="Verify">
                                <HeaderStyle Width="2%" />
                            </asp:BoundField>
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                    <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divNavigation" runat="server">
            <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="summary_block" id="divprofile" runat="server" visible="false">
        <span>Profile Notification </span>
        <ul>
            <li id="liphoto" runat="server" visible="false">Your profile photo/signature/left thumb
                impression not uploaded yet.
                <asp:LinkButton ID="Lnkphoto" runat="server" OnClick="Lnkphoto_Click">Please upload your profile photo/signature/left thumb impression.</asp:LinkButton></li>
            <li id="lipersonal" runat="server" visible="false">Your personal details are not completed
                yet.
                <asp:LinkButton ID="Lnkpersonal" runat="server">Please update your personal details.</asp:LinkButton></li>
            <li id="licontact" runat="server" visible="false">Your contact details are not completed
                yet.
                <asp:LinkButton ID="Lnkcontact" runat="server">Please update your contact details.</asp:LinkButton></li>
            <li id="limobno" runat="server" visible="false">Your mobile number not verified yet.
                <asp:LinkButton ID="Lnkmobno" runat="server" OnClick="Lnkmobno_Click">Please verify your mobile number.</asp:LinkButton>
                <asp:Label ID="lbmessage" runat="server" Text="" Style="font-size: small; color: Red;"></asp:Label></li>
            <li id="liemail" runat="server" visible="false">Your email address not verified yet<asp:LinkButton
                ID="Lnkemail" runat="server" OnClick="Lnkemail_Click">Please verify your email address</asp:LinkButton></li>
            <li id="liaddress" runat="server" visible="false">Your correspondence address details
                are not completed yet.
                <asp:LinkButton ID="Lnkaddress" runat="server">Please update your correspondence address details.</asp:LinkButton></li>
            <li id="lilocked" runat="server" visible="false">Your profile details are completely
                updated and locked by you on
                <asp:Label ID="Lbllockeddate" runat="server" Text="" Style="font-size: small; color: #000000;"></asp:Label>.</li>
                 <%--Commented on 13 Feb 2018
               .NIELIT
                will not provide you the course certificate until you send the printed application
                form signed by you.If you have not sent it to the NIELIT, please
                <asp:LinkButton ID="Lnklocked" runat="server"> print profile updation request form</asp:LinkButton>
                &nbsp;by clicking on this link and send it to the NIELIT.</li>--%>
            <li id="liResultsheetDownload" runat="server">
                <asp:LinkButton ID="LinkReusltSheetDownload" runat="server" OnClick="LinkReusltSheetDownload_Click"
                    Visible="False">Click here for Result Sheet Download </asp:LinkButton>
                &nbsp;
                <asp:Label ID="lblresultsheetmsg" runat="server" ForeColor="#FF3300"></asp:Label>
            </li>
            <li id="liverified" runat="server" visible="false"></li>
            <li id="linotlocked" runat="server" visible="false">You have completely updated your
                incomplete profile details but you have not locked yet. If you do not want to update
                your profile any more and <strong>also you cannot apply for the Examination form until
                    you lock your profile</strong>, please lock your profile by clicking on this
                link
                <asp:LinkButton ID="Lnknotlocked" runat="server"> lock your profile.</asp:LinkButton>
            </li>
        </ul>
    </div>
    <div class="summary_block" id="divcourse" runat="server" visible="false">
        <span> Exam Notification </span>
        <ul>
            <li id="licutoffdates1" runat="server" visible="false"></li>
            <li id="licutoffdates2" runat="server" visible="false"></li>
            <li id="licutoffdates3" runat="server" visible="false"></li>
            <li id="linocutoffdates" runat="server" visible="false"></li>
            <li id="liexamtimetable" runat="server" visible="false">
                <asp:Label ID="Lbtimetable" runat="server" Text="" Style="font-size: small; color: #000000;"></asp:Label>
                <asp:LinkButton ID="lnktimetable" runat="server" Text="Click here to View/Print the Time-Table." />
            </li>
            <li id="linoexamtimetable" runat="server" visible="false"></li>
            <li id="lidownloadadmitcard" runat="server" visible="false">
                <asp:Label ID="Lbdownload" runat="server" Text="" Style="font-size: small; color: #000000;" Visible="false"></asp:Label>
                <asp:LinkButton ID="Lnkdownload" runat="server" Text="Click here to View/Print the Admit Card." Visible="false" /><br/>
             
            </li>
             <li id="Onlinelidownloadadmitcard" runat="server" visible="false">
                <asp:Label ID="OnlineLbdownload" runat="server" Text="" Style="font-size: small; color: #000000;" Visible="false"></asp:Label>
                <asp:LinkButton ID="OnlineLnkadmitcard" runat="server" Text="Click here to View/Print the Exam Admit Card." Visible="false" />
              </li>
            <li id="lipracadmitcard" runat="server" visible="false">
                <asp:Label ID="Lbpractical" runat="server" Text="" Style="font-size: small; color: #000000;"></asp:Label>
                <asp:LinkButton ID="Lnkpracadmitcard" runat="server" Text="Click here to View/Print the Practical Examination Admit Card." />
             </li>
            <li id="liviewresult" runat="server" visible="false">
                <asp:Label ID="Lbresult" runat="server" Text="" Style="font-size: small; color: #000000;"></asp:Label>
                <asp:LinkButton ID="Lnkresult" runat="server" Text="Click here to View/Print the Result." />
            </li>
            <li id="liexam" runat="server" visible="false">
                <asp:Label ID="lbstatus" runat="server" Text="" Style="font-size: small; color: #000000;"></asp:Label><br />
                Click here to print the form
                <asp:LinkButton ID="LnkBtnPrintForm" runat="server" Text="Print Examination Form"
                    visble="false" /><br />
                <div id="Lblexam" runat="server" visible="false" style="text-align: left;">
                </div>
                <asp:Label ID="Lbpaymentsource" runat="server" Text="" Style="font-size: small; color: Red;"
                    Visible="false"></asp:Label>
                <asp:LinkButton ID="Lnkpaymentsourcechange" runat="server" Text=" Change Payment Option"
                    Visible="false" OnClick="Lnkpaymentsourcechange_Click"></asp:LinkButton>
                <asp:Label ID="Lbappsource" runat="server" Text="" Style="font-size: small; color: Red;"
                    Visible="false"></asp:Label>
                <asp:LinkButton ID="Lnkappchange" runat="server" Text=" Cancel / Edit Application"
                    Visible="false" OnClick="Lnkappchange_Click"></asp:LinkButton><br />
                <asp:Label ID="Lblfeenotification" runat="server" Text="" Style="font-size: small;
                    color: #000000;" Visible="false"></asp:Label>
            </li>
            <li id="linopaymentfee" runat="server" visible="false"></li>
            <li id="linotexam" runat="server" visible="false">You have not yet applied for the Exam.
                <asp:LinkButton ID="lnknotexam" runat="server">Click on this link to apply for Exam</asp:LinkButton></li>
            <li id="linotexamEdit" runat="server" visible="false" Style="font-size:larger;font-weight:bold;color:red">You have already applied for the Exam.
                <asp:LinkButton ID="lnknotexamEdit" runat="server">Click on this link to Edit applied Exam Form</asp:LinkButton></li>
            <li id="linotexamfound" runat="server" visible="false"></li>
              <li id="liprojectfee" runat="server" visible="true">Click here to Apply/View Project Fee Details.
                <asp:LinkButton ID="lnkprojectfee" runat="server">Click on this link to apply/view for Project Fee</asp:LinkButton></li>
        </ul>
    </div>
    <div class="summary_block" id="divregistration" runat="server" visible="false">
        <span>Registration Notification </span>
        <ul>
            <li id="linotregfound" runat="server" visible="false"></li>
            <li id="liregfound" runat="server" visible="false">
                <asp:Label ID="lbtext" runat="server" Text="" Style="font-size: small; color: #000000;"></asp:Label></li>
            <li id="liPaymentNote" runat="server" visible="false">
                <asp:Label ID="Lblnote" runat="server" Text="" Style="font-size: small; color: #000000;"></asp:Label></li>
            <li id="liPrintCourseReg" runat="server" visible="false">Click here to print the course
                registration form
                <asp:LinkButton ID="lnkPrntCourseReg" runat="server" Text="Print Course Registration Form"
                    visble="false" />
            </li>
            <li id="liPrintRegID" runat="server" visible="false">Click here to print the Registration
                ID Card
                <asp:LinkButton ID="lnkPrintRegID" runat="server" Text="Print Registration ID Card"
                    visble="false" />
            </li>
        </ul>
    </div>
    <div class="summary_block" id="divscholarship" runat="server" visible="false">
        <span>Scholarship Notification </span>
        <ul>
            <li id="lischolar" runat="server" visible="false">You have successfully filled the Aadhaar
                & Bank Account Details Form to avail Scholarship on
                <asp:Label ID="Lblscholarshipdate" runat="server" Text="" Style="font-size: small;
                    color: #000000;">
                </asp:Label>.NIELIT will not provide you the scholarship until you send the printed
                Aadhaar & Bank Account Details Form signed by you.If you have not sent it to the
                NIELIT, please
                <asp:LinkButton ID="lnkscholar" runat="server"> print  Aadhaar & Bank Account Details Form </asp:LinkButton>
                &nbsp;by clicking on this link and send it to the NIELIT.</li>
        </ul>
    </div>
    <div id="divHO" runat="server" visible="false">
        <div class="summary_block1" id="divDLC" runat="server" visible="false">
            <span>Digital Literacy Courses >> New Applications </span>
            <br />
            <asp:UpdatePanel EnableViewState="true" ID="UpdatePanel1" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>                
                    <asp:GridView ID="DlcNewApplicationGrid" runat="server" DataKeyNames="ExamId" AutoGenerateColumns="False"
                        Width="100%" OnRowDataBound="DlcGrid_RowDataBound" HeaderStyle-Font-Size="12px" CellPadding="20">
                        <Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle Width="1%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Left"/>
                            </asp:BoundField>
                            <asp:BoundField DataField="CourseName" HeaderText="Course">
                                <HeaderStyle Width="4%" />
                                <ItemStyle HorizontalAlign="Left" Font-Size="9" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExamName" HeaderText="Exam">
                                <HeaderStyle Width="6%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="CourseId,ExamId" DataNavigateUrlFormatString="Admin/NewApplicationStatusRC.aspx?CourseId={0}&ExamId={1}"
                                DataTextField="FinalSubmitted" HeaderText="Submitted" Target="_self">
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="CourseId,ExamId" DataNavigateUrlFormatString="Admin/NewApplicationStatusRC.aspx?CourseId={0}&ExamId={1}"
                                DataTextField="Direct" HeaderText="Direct" Target="_self">
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="CourseId,ExamId" DataNavigateUrlFormatString="Admin/NewApplicationStatusRC.aspx?CourseId={0}&ExamId={1}"
                                DataTextField="ViaInstitute" HeaderText="Institute: Not-Verified | Verified | Paid"
                                Target="_self">
                                <HeaderStyle Width="16%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="CourseId,ExamId" DataNavigateUrlFormatString="Admin/NewApplicationStatusRC.aspx?CourseId={0}&ExamId={1}"
                                DataTextField="FeePaid" HeaderText="All Paid" Target="_self">
                                <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="CourseId,ExamId" DataNavigateUrlFormatString="Admin/NewApplicationStatusRC.aspx?CourseId={0}&ExamId={1}"
                                DataTextField="Disability" HeaderText="Disability" Target="_self">
                                <HeaderStyle Width="3%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:HyperLinkField>
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

         <div class="summary_block1" id="divOABC" runat="server" visible="false">
            <span>Information Technology Courses >> New Exam Applications </span>
            <br />
            <asp:UpdatePanel EnableViewState="true" ID="UpdatePanel2" UpdateMode="Conditional"
                runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GridView1" runat="server" DataKeyNames="ExamId" AutoGenerateColumns="False"
                        Width="100%" OnRowDataBound="DlcGrid_RowDataBound" HeaderStyle-Font-Size="12px">
                        <Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle Width="1%"></HeaderStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="CourseName" HeaderText="Course">
                                <HeaderStyle Width="5%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ExamName" HeaderText="Exam">
                                <HeaderStyle Width="5%" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="9" />
                            </asp:BoundField>                           
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc1:SideLink ID="SideLink1" runat="server" />
    <asp:Panel ID="pnlCourses" runat="server">
    </asp:Panel>
</asp:Content>
