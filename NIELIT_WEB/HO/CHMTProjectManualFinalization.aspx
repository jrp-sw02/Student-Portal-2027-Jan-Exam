<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true" CodeFile="CHMTProjectManualFinalization.aspx.cs" Inherits="HO_CHMTProjectManualFinalization" %>

<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
     CHMT O  Level  Manual Project Finalization
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
   
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">

    <script language="javascript" type="text/javascript">
        var dtgp = "<%= gvGrid.ClientID %>"

        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function SelectheaderCheckboxes(headerchk) {
            var gvcheck = document.getElementById("<%=gvGrid.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
                //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }

        function Selectchildcheckboxes(header) {
            var ck = header;
            var count = 0;
            var gvcheck = document.getElementById("<%=gvGrid.ClientID %>");
            var headerchk = document.getElementById(header);
            var rowcount = gvcheck.rows.length;
            //By using this for loop we will count how many checkboxes has checked
            for (i = 1; i < gvcheck.rows.length; i++) {
                var inputs = gvcheck.rows[i].getElementsByTagName('input');
                if (inputs[0].checked) {
                    count++;
                }
            }
            //Condition to check all the checkboxes selected or not
            if (count == rowcount - 1) {
                headerchk.checked = true;
            }
            else {
                headerchk.checked = false;
            }
        }

        function validateDateFormat(textBox) {
            var enteredDate = textBox.value.trim();
            var dateRegex = /^\d{4}-\d{2}-\d{2}$/;

            if (!dateRegex.test(enteredDate)) {
                alert("Date format must be yyyy-mm-dd");
                textBox.focus();
            }
        }


        //function isValidDate(e) {
        //    var key = e.keyCode || e.which;
        //    var char = String.fromCharCode(key);
        //    var dateValue = e.target.value;

        //    // Allow backspace, delete, and arrow keys
        //    if (key === 8 || key === 46 || key === 37 || key === 39) {
        //        return true;
        //    }

        //    // Allow digits and hyphen
        //    if (/\d|-/.test(char)) {
        //        return true;
        //    }

        //    // Prevent typing other characters
        //    e.preventDefault();
        //    return false;

        //    // Validate input format (yyyy-mm-dd)
        //    var dateFormat = /^\d{4}-\d{2}-\d{2}$/;
        //    if (!dateFormat.test(dateValue + char)) {
        //        e.preventDefault();
        //        return false;
        //    }

        //    // Validate date components
        //    var parts = dateValue.split('-');
        //    var year = parseInt(parts[0], 10);
        //    var month = parseInt(parts[1], 10);
        //    var day = parseInt(parts[2], 10);

        //    // Validate day, month, and year ranges
        //    if (isNaN(year) || isNaN(month) || isNaN(day) ||
        //        year < 1900 || year > 2100 || month < 1 || month > 12 ||
        //        day < 1 || day > 31) {
        //        e.preventDefault();
        //        return false;
        //    }

        //    return true;
        //}



        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }
    </script>


<div>
    <table width="100%">
          <tr >
                        <td  colspan="2"  >
                            <asp:Label ID="lblErrorMsg" runat="server" EnableTheming="False" CssClass="error"
                                Visible="False" Width="99%"></asp:Label>
                        </td>
                    </tr>
       <tr class="odd">
                <td colspan="1" class="auto-style2">
                    <label id="Label1" runat="server">From Date </label>
                </td>
                <td>

                    <asp:TextBox ID="txtFromDate" runat="server" MaxLength="100" SkinID="txt210" Height="22px" Width="166px"  style="border: 1px solid #ccc;"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgdatefrom" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                        <img id="imgdatefrom" alt="Calender" src="../images/calendaricon.jpg" />
                  
                   <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <asp:DropDownList ID="ddlExam" runat="server" Height="25px" Enabled="true" Width="209px" AutoPostBack="true"  onchange="handleClick()"  >
                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlExam" EventName="SelectedIndexChanged" />
                            <%--<asp:AsyncPostBackTrigger ControlID="rbtnTheoryMarks" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="rbtnPracticalMarks" EventName="CheckedChanged" />--%>
                       <%-- </Triggers>
                       
                    </asp:UpdatePanel>

                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1"
                        runat="server"
                        ControlToValidate="ddlExam"
                        InitialValue="0"
                        ErrorMessage="Please select a value from the list."
                        Display="Dynamic"
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>--%>
                </td>
            </tr>
              <tr class="odd">
                <td colspan="1" class="auto-style2">
                    <label id="Label3" runat="server">To  Date </label>
                </td>
                <td>
                     <asp:TextBox ID="txtToDate" runat="server" MaxLength="100" SkinID="txt210" Height="20px" Width="164px" style="border: 1px solid #ccc;"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="img1" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                        <img id="img1" alt="Calender" src="../images/calendaricon.jpg" />
                  
                    <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>

                            <asp:DropDownList ID="DropDownList1" runat="server" Height="25px" Enabled="true" Width="209px" AutoPostBack="true"  onchange="handleClick()"  >
                                <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <%--<Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlExam" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="rbtnTheoryMarks" EventName="CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="rbtnPracticalMarks" EventName="CheckedChanged" />
                        </Triggers>
                       
                    </asp:UpdatePanel>--%>

                  <%--  <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator3"
                        runat="server"
                        ControlToValidate="ddlExam"
                        InitialValue="0"
                        ErrorMessage="Please select a value from the list."
                        Display="Dynamic"
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>--%>
                </td>
            </tr>

        <tr>
            <td>
               
            </td>
            <td>
                 <asp:Button ID="btnShow" runat="server" Text="Show"  Width="100px" Height="26px" Enabled="true" OnClick="btnShow_Click"   />
            </td>
        </tr>


    </table>
</div>

     <div id="divGrid" runat="server"   >
                <table cellpadding="0" cellspacing="1" width="100%" id="tbl1" runat="server">
                  
                    <tr>
                        <td align="right" valign="bottom">
                             <asp:Button ID="btnFinalize" runat="server" Text="Finalize" Visible="false"
                                OnClientClick="return Validate_Checkbox('Are you sure you want to finalize selected applications!')"
                                Width="104px" OnClick="btnFinalize_Click" />
                           
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvGrid" runat="server" AutoGenerateColumns="False"
                                Width="100%" DataKeyNames="ID"  >
                                <Columns>
                                   <%-- <asp:BoundField HeaderText="#">
                                        <HeaderStyle Width="1%" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>--%>
                                 <asp:BoundField DataField="SrNo" HeaderText="Sr No." SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                                <%-- %> <asp:BoundField DataField="ID" HeaderText="Application No" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />--%>
                                 <asp:BoundField DataField="Registration_Number" HeaderText="Registration No" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                                 <asp:BoundField DataField="Candidate_Name" HeaderText="Candidate Name" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Institute_Name" HeaderText="Institute Name" SortExpression="SerialNumber" ItemStyle-HorizontalAlign="Center" />
                                 <%--   <asp:TemplateField HeaderText="Project Title">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtProjectTitle" runat="server" Width="200px" Visible="true" Enabled="false" Text='<%# Eval("Project_Title") %>' ></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="150px" />
                            <HeaderStyle Width="150px" />
                                            </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Project Receipt Date">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDate" runat="server" AutoPostBack="true"  ToolTip="yyyy-mm-dd" Placeholder="yyyy-mm-dd"  Width="120px" onblur="validateDateFormat(this);"  enabled="false" Text ='<%# Eval("Project_Receipt_Date") %>' ></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   <%-- <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hidApplicationStatusID" runat="server" Value='<%# Eval("ApplicationStatusID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>


                                    <asp:TemplateField HeaderText="">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk" runat="server" SkinID="CheckAllInGridView" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="2%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hfActionID" runat="server" Value="" />
            </div>
            <div id="divNavigation" runat="server">
                <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged"  />
            </div>
    <div>
        <table width="100%">
            <tr  >
                <td>

                </td>
            </tr>
            <tr align="right">
                <td>
                    <%--<asp:HyperLink ID="HyperLink1" runat="server"  NavigateUrl ="~/HO/CHMT_Project_Report.aspx "  Visible ="false">Click here to view the Report </asp:HyperLink>--%>

                </td>
                
            </tr>
        </table>
        
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
</asp:Content>

