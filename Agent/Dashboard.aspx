<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WebApplication1.Agent.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agent Dashboard</title>
    <link rel="stylesheet" href="/css/agent-dashboard.css" />

</head>
<body>
    <form id="form1" runat="server">

        <!-- Sidebar -->
        <div class="sidebar">
            <h2 class="logo">REMS</h2>
            <a href="Dashboard.aspx" class="active">Dashboard</a>
            <a href="/Agent/Add-property.aspx">Add Property</a>
            <a href="/Agent/MyProperties.aspx">My Properties</a>
            <a href="/Agent/Bookings.aspx">Bookings</a>
            <a href="/Agent/Profile.aspx">Profile</a>
            <a href="/Logout.aspx">Logout</a>
        </div>

        <!-- Main Content -->
        <div class="main-content">

            <!-- Page Title -->
            <h1 class="page-title">Agent Dashboard</h1>

            <!-- Cards -->
            <div class="card-row">

                <div class="card">
                    <p class="card-title">My Properties</p>
                    <h2>
                        <asp:Label ID="lblTotalProperties" runat="server" Text="0" /></h2>
                </div>

                <div class="card approved">
                    <p class="card-title">Approved</p>
                    <h2>
                        <asp:Label ID="lblApproved" runat="server" Text="0" /></h2>
                </div>

                <div class="card pending">
                    <p class="card-title">Pending</p>
                    <h2>
                        <asp:Label ID="lblPending" runat="server" Text="0" /></h2>
                </div>

                <div class="card booking">
                    <p class="card-title">Bookings</p>
                    <h2>
                        <asp:Label ID="lblBookings" runat="server" Text="0" /></h2>
                </div>

            </div>

            <!-- Recent Properties -->
            <div class="section">
                <h3>Recent Properties</h3>
                <asp:GridView ID="gvRecentProperties" runat="server"
                    CssClass="table"
                    AutoGenerateColumns="true">
                </asp:GridView>
            </div>

            <!-- Recent Bookings -->
            <div class="section">
                <h3>Recent Bookings</h3>
                <asp:GridView ID="gvRecentBookings" runat="server"
                    CssClass="table"
                    AutoGenerateColumns="true">
                </asp:GridView>
            </div>

        </div>

    </form>
</body>
</html>
