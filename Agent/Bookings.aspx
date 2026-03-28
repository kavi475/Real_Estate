<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bookings.aspx.cs" Inherits="WebApplication1.Agent.Bookings" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bookings</title>
    <link rel="stylesheet" href="/CSS/agent-dashboard.css">
    <link rel="stylesheet" href="/CSS/AgentDashboardCommon.css">
    <link rel="stylesheet" href="/CSS/AgentComponent.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="sidebar">
            <h2 class="logo">REMS</h2>
            <a href="/Agent/Dashboard.aspx">Dashboard</a>
            <a href="/Agent/Add-property.aspx">Add Property</a>
            <a href="/Agent/MyProperties.aspx">My Properties</a>
            <a href="/Agent/Bookings.aspx" class="active">Bookings</a>
            <a href="/Agent/Profile.aspx">Profile</a>
            <a href="../Logout.aspx">Logout</a>
        </div>
        <div class="main">
            <div class="content">
                <h1>Bookings</h1>

                <asp:GridView ID="gvBookings" runat="server"
                    CssClass="table"
                    AutoGenerateColumns="false"
                    OnRowCommand="gvBookings_RowCommand">
                    <Columns>

                        <asp:BoundField DataField="Title" HeaderText="Property" />
                        <asp:BoundField DataField="UserEmail" HeaderText="User" />
                        <asp:BoundField DataField="BookingDate" HeaderText="Date"
                            DataFormatString="{0:dd-MMM-yyyy}" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnApprove" runat="server"
                                    CommandName="ApproveBooking"
                                    CommandArgument='<%# Eval("BookingId") %>'
                                    Visible='<%# Eval("Status").ToString() == "Pending" %>'
                                    style="color:green; margin-right:8px;">
                                    Approve
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnReject" runat="server"
                                    CommandName="RejectBooking"
                                    CommandArgument='<%# Eval("BookingId") %>'
                                    Visible='<%# Eval("Status").ToString() == "Pending" %>'
                                    OnClientClick="return confirm('Reject this booking?');"
                                    style="color:red;">
                                    Reject
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>

                <asp:Label ID="lblMsg" runat="server"
                    style="color:green; margin-top:10px; display:block;">
                </asp:Label>

            </div>
        </div>
    </form>
</body>
</html>