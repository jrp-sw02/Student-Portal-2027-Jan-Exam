<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportantNotificationnew.aspx.cs" Inherits="ImportantNotificationnew" Debug="True" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NIELIT Notifications</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            background: #fff !important;
            width: 100%;
        }

        h2 {
            color: #333;
            font-size: 18px;
            margin-bottom: 15px;
            text-align: center;
        }

        ul {
            list-style-type: none;
            padding: 0;
            margin: 0;
            width: 100%;
        }

        li {
            padding: 5px 0;
            border-bottom: 1px solid #eee;
        }

            li:last-child {
                border-bottom: none;
            }

        .notif-text {
            color: #000; /* Red color */
            font-family: 'Arial', sans-serif;
            font-size: 12px;
            text-decoration: none;
            font-weight: bold;
            line-height: 1.2;
        }

        .notif-text-link {
            color: #365ccc; /* #d32f2f Red color */
            font-family: 'Arial', sans-serif;
            font-size: 12px;
            text-decoration: none;
            font-weight: bold;
            line-height: 1.2;
        }

            a:hover {
                text-decoration: underline;
            }

        small {
            color: #555;
            font-size: 13px;
        }

        .older-notif {
            display: block;
            width: 100% !important;
            color: #fff;
            text-decoration : none;
            margin: 10px 0;
            background: red;
            text-align: center;
            padding: 5px 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <image id="loadingImgNotif" src="./images/indicator.gif" runat="server"></image>
          


        <asp:Repeater ID="rptEvents" runat="server">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>

            <ItemTemplate>
                <li>

                    <asp:Literal
                        ID="litTitle"
                        runat="server"
                        Text='<%# GetNotificationHtml(Eval("title"), Eval("link")) %>' />

                    <small><%# Eval("date") %></small>

                </li>
            </ItemTemplate>

            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>

        <br />

        <div id="oldnotifblock" runat="server">
         
            <asp:HyperLink runat="server"  Target="_blank" CssClass="older-notif" href="https://example.com">Other Important Notifications</asp:HyperLink>
        
        <asp:HyperLink
            ID="lnkOlderNotifications"
            CssClass="older-notif"
            runat="server"
            Target="_blank"
            Text="Older Notifications"
           />
       </div>

        <br />

        <asp:Label
            ID="lblError"
            runat="server"
            ForeColor="Red">
        </asp:Label>

        <asp:Button
            ID="btnreload"
            runat="server"
            Text="Reload Notifications"
            OnClientClick="window.location.reload(); return false;" />


    </form>
    <%--    <form id="form1" runat="server">
        <h2>NIELIT Latest Notifications</h2>
        
        <asp:Repeater ID="rptEvents" runat="server">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <a href='<%# Eval("link") %>' target="_blank">
                        <%# Eval("title") %>
                    </a>
                    <br />
                    <small><%# Eval("date") %></small>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
    </form>--%>
</body>
</html>
