<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MyInfo.master" AutoEventWireup="true" CodeFile="frmmodulelistforexam.aspx.cs" Inherits="frmmodulelistforexam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 240px;
        }
        .style3
        {
            width: 251px;
        }
        .style4
        {
            width: 25px;
        }
        .style5
        {
            width: 27px;
        }
        .style6
        {
            width: 447px;
        }
        .style7
        {
            width: 596px;
        }
    </style>
    
    <script language="javascript" type="text/javascript">
// <![CDATA[

        function Radio1_onclick() {
            if (document.getElementById["radio1"].value = 1) {
                document.getElementById["radio2"].value = 0;
            }
        }

// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" Runat="Server">
<asp:Label ID="lblHeading" runat="server" Text="List Of Candidates For Examination"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" Runat="Server">
<ul class="crumbs">
	<li class="first"><a id="a1" runat="server" href="#" style="z-index:9;"><span></span></a></li>
    <li><a id="a3" runat="server" href="#" style="z-index:8;"><%=Request.QueryString["course"].ToString() %>: Student List</a></li>
	<li><a href="#" style="z-index:7;"><%=Request.QueryString["R"].ToString() %></a></li>
</ul>  
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" Runat="Server">
<div id="lst" width="100%" runat="server">
                 <table class="sample2" cellpadding="0" cellspacing="1" width="100%">
                <tr class="heading">
                    <td colspan="3">
                        Modules Appearing For</td>
                </tr>
                </table> 
                <table class="gdbody" cellspacing="1" cellpadding="4" id="Table1" width="100%">
                    <tr class="gdheader">
                        <th scope="col" class="style5">
                            #
                        </th>
                        <th scope="col" class="style2">
                            Module Code
                        </th>
                        <th scope="col" class="style6">
                            Module Name
                        </th>
                        <th scope="col" class="style7">
                            Course Duration
                        </th>
                        <th scope="col" class="style3">
                            Amount</th>
                        <th scope="col" class="style4">
                            &nbsp;
                        </th>
                    </tr>
                    <tr class="gdrow">
                        <td align="right">
                            1
                        </td>
                        <td align="left">
                            M1-R4
                        </td>
                        <td class="style6">
                            Informatin Technology
                        </td>
                        <td align="center" class="style7">
                            <asp:DropDownList ID="DropDownList1" runat="server" Height="20px" Width="51px">
                                <asp:ListItem>MM</asp:ListItem>
                                <asp:ListItem>July</asp:ListItem>
                                <asp:ListItem>January</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList33" runat="server" Height="22px" Width="57px">
                                <asp:ListItem>Year</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="Label1" runat="server" Text="to" style="vertical-align:top;"></asp:Label>
                            <asp:DropDownList ID="DropDownList3" runat="server" Height="20px" Width="51px">
                                <asp:ListItem>MM</asp:ListItem>
                                <asp:ListItem>July</asp:ListItem>
                                <asp:ListItem>January</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList4" runat="server" Height="22px" Width="57px">
                                <asp:ListItem>Year</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                                <asp:ListItem>2015</asp:ListItem>
                                <asp:ListItem>2016</asp:ListItem>
                                <asp:ListItem>2017</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            500</td>
                        <td>
                            <input id="Checkbox7" type="checkbox" />
                        </td>
                    </tr>
                    <tr class="gdalternate">
                        <td align="right" class="style5">
                            2
                        </td>
                        <td align="left" class="style2">
                            M2-R4
                        </td>
                        <td class="style6">
                            Internet Technology
                        </td>
                        <td align="center" class="style7">
                            <asp:DropDownList ID="DropDownList34" runat="server" Height="20px" Width="51px">
                                <asp:ListItem>MM</asp:ListItem>
                                <asp:ListItem>July</asp:ListItem>
                                <asp:ListItem>January</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList35" runat="server" Height="22px" Width="57px">
                                <asp:ListItem>Year</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="Label9" runat="server" Text="to" style="vertical-align:top;"></asp:Label>
                            <asp:DropDownList ID="DropDownList36" runat="server" Height="20px" Width="51px">
                                <asp:ListItem>MM</asp:ListItem>
                                <asp:ListItem>July</asp:ListItem>
                                <asp:ListItem>January</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList37" runat="server" Height="22px" Width="57px">
                                <asp:ListItem>Year</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                                <asp:ListItem>2015</asp:ListItem>
                                <asp:ListItem>2016</asp:ListItem>
                                <asp:ListItem>2017</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            500</td>
                        <td>
                            <input id="Checkbox8" type="checkbox" />
                        </td>
                    </tr>
                    <tr class="gdrow">
                        <td align="right" class="style5">
                            3
                        </td>
                        <td align="left" class="style2">
                            M3-R4
                        </td>
                        <td class="style6">
                            Programing With C
                        </td>
                        <td align="center" class="style7">
                            <asp:DropDownList ID="DropDownList38" runat="server" Height="20px" Width="51px">
                                <asp:ListItem>MM</asp:ListItem>
                                <asp:ListItem>July</asp:ListItem>
                                <asp:ListItem>January</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList39" runat="server" Height="22px" Width="57px">
                                <asp:ListItem>Year</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="Label10" runat="server" Text="to" style="vertical-align:top;"></asp:Label>
                            <asp:DropDownList ID="DropDownList40" runat="server" Height="20px" Width="51px">
                                <asp:ListItem>MM</asp:ListItem>
                                <asp:ListItem>July</asp:ListItem>
                                <asp:ListItem>January</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList41" runat="server" Height="22px" Width="57px">
                                <asp:ListItem>Year</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                                <asp:ListItem>2015</asp:ListItem>
                                <asp:ListItem>2016</asp:ListItem>
                                <asp:ListItem>2017</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            500</td>
                        <td>
                            <input id="Checkbox9" type="checkbox" />
                        </td>
                    </tr>
                    <tr class="gdalternate">
                        <td align="right" class="style5">
                            4
                        </td>
                        <td align="left" class="style2">
                            M4.1-R4
                        </td>
                        <td class="style6">
                            .Net Technology</td>
                        <td align="center" class="style7">
                            <asp:DropDownList ID="DropDownList42" runat="server" Height="20px" Width="51px">
                                <asp:ListItem>MM</asp:ListItem>
                                <asp:ListItem>July</asp:ListItem>
                                <asp:ListItem>January</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList43" runat="server" Height="22px" Width="57px">
                                <asp:ListItem>Year</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="Label11" runat="server" Text="to" style="vertical-align:top;"></asp:Label>
                            <asp:DropDownList ID="DropDownList44" runat="server" Height="20px" Width="51px">
                                <asp:ListItem>MM</asp:ListItem>
                                <asp:ListItem>July</asp:ListItem>
                                <asp:ListItem>January</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList45" runat="server" Height="22px" Width="57px">
                                <asp:ListItem>Year</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                                <asp:ListItem>2015</asp:ListItem>
                                <asp:ListItem>2016</asp:ListItem>
                                <asp:ListItem>2017</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            500</td>
                        <td>
                           <input id="Checkbox12" type="checkbox" /></td>
                    </tr>
                    <tr class="gdrow">
                        <td align="right" class="style5">
                            7</td>
                        <td align="left" class="style2">
                            M5-R5</td>
                        <td class="style6">
                            Practical</td>
                        <td align="center" class="style7">
                            <asp:DropDownList ID="DropDownList54" runat="server" Height="20px" Width="51px">
                                <asp:ListItem>MM</asp:ListItem>
                                <asp:ListItem>July</asp:ListItem>
                                <asp:ListItem>January</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList55" runat="server" Height="22px" Width="57px">
                                <asp:ListItem>Year</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="Label14" runat="server" Text="to" style="vertical-align:top;"></asp:Label>
                            <asp:DropDownList ID="DropDownList56" runat="server" Height="20px" Width="51px">
                                <asp:ListItem>MM</asp:ListItem>
                                <asp:ListItem>July</asp:ListItem>
                                <asp:ListItem>January</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList57" runat="server" Height="22px" Width="57px">
                                <asp:ListItem>Year</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                                <asp:ListItem>2015</asp:ListItem>
                                <asp:ListItem>2016</asp:ListItem>
                                <asp:ListItem>2017</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            400</td>
                        <td>
                            <input id="Checkbox10" type="checkbox" /></td>
                    </tr>
                    <tr class="gdalternate">
                        <td align="right" class="style5">
                            8</td>
                        <td align="left" class="style2">
                            M6-R6</td>
                        <td class="style6">
                            Project</td>
                        <td align="center" class="style7">
                            <asp:DropDownList ID="DropDownList58" runat="server" Height="20px" Width="51px">
                                <asp:ListItem>MM</asp:ListItem>
                                <asp:ListItem>July</asp:ListItem>
                                <asp:ListItem>January</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList59" runat="server" Height="22px" Width="57px">
                                <asp:ListItem>Year</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="Label15" runat="server" Text="to" style="vertical-align:top;"></asp:Label>
                            <asp:DropDownList ID="DropDownList60" runat="server" Height="20px" Width="51px">
                                <asp:ListItem>MM</asp:ListItem>
                                <asp:ListItem>July</asp:ListItem>
                                <asp:ListItem>January</asp:ListItem>
                            </asp:DropDownList>
                            <asp:DropDownList ID="DropDownList61" runat="server" Height="22px" Width="57px">
                                <asp:ListItem>Year</asp:ListItem>
                                <asp:ListItem>2013</asp:ListItem>
                                <asp:ListItem>2014</asp:ListItem>
                                <asp:ListItem>2015</asp:ListItem>
                                <asp:ListItem>2016</asp:ListItem>
                                <asp:ListItem>2017</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            700</td>
                        <td>
                            <input id="Checkbox11" type="checkbox" /></td>
                    </tr>
                </table>
                  <div style="text-align: right; margin-top: 10px">
                    <asp:Button ID="btnPrevious" runat="server" Text="Previous"  Width="86px" />
                    <asp:Button ID="btnapproved" runat="server" Text="Approve"  Width="86px" />
                    <asp:Button ID="btnreject" runat="server" Text="Reject"  Width="86px" />
                    <asp:Button ID="btnmodify" runat="server" Text="Skip"  Width="86px" />
                    <asp:Button ID="btncancel" runat="server" Text="Back" OnClick="btnCancel_Click" Width="81px" />
                    <asp:Button ID="btnNext" runat="server" Text="Next"  Width="86px" />
                    
                </div>
                
            </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" Runat="Server">
<table align="center" class="nav" cellspacing="0" cellpadding="0" id="tblNavLinks" width="97%">
        
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <a href="#">Preview</a>
            </td>
        </tr>
        
    </table>
</asp:Content>

