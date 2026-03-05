<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminDashboard.aspx.cs" Inherits="WebApplication1.adminDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="/css/AdminDashboard.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="sidebar">
            <h2 class="logo">REMS</h2>

            <a href="AdminDashboard.aspx">Dashboard</a>
            <a href="ManageProperties.aspx">Properties</a>
            <a href="ManageUsers.aspx">Users</a>
            <a href="ManageBookings.aspx">Bookings</a>
           <a href="../Logout.aspx">Logout</a>


        </div>

        <!-- Main Content -->
        <div class="main-content">
            <h1>Admin Dashboard</h1>
        </div>

        <!-- Row 1 -->
        <div class="cards">
            <div class="card">
                <p>Total Property</p>
                <hr />
                <asp:Label ID="lblProperty" runat="server" Text="5"></asp:Label>
                <hr />
              <a href="/Admin/ViewList.aspx?type=properties">View Details</a>
            </div>

            <div class="card">
                <p>Total State</p>
                <hr />
                <asp:Label ID="lblState" runat="server" Text="5"></asp:Label>
                <hr />
                <a href="/Admin/ViewList.aspx?type=states">View Details</a>
            </div>

            <div class="card">
                <p>Total City</p>
                <hr />
                <asp:Label ID="lblCity" runat="server" Text="5"></asp:Label>
                <hr />
                <a href="/Admin/ViewList.aspx?type=cities">View Details</a>
            </div>
        </div>

        <!-- Row 2 -->
        <div class="cards">
            <div class="card">
                <p>Total Users</p>
                <hr />
                <asp:Label ID="lblUsers" runat="server" Text="5"></asp:Label>
                <hr />
                <a href="/Admin/ViewList.aspx?type=users">View Details</a>
            </div>

            <div class="card">
                <p>Total Property Listed</p>
                <hr />
                <asp:Label ID="lblListed" runat="server" Text="5"></asp:Label>
                <hr />
              <a href="/Admin/ViewList.aspx?type=PropertyListed">View Details</a>
            </div>

            <div class="card">
                <p>Total Agent Listed</p>
                <hr />
                <asp:Label ID="lblAgents" runat="server" Text="5"></asp:Label>
                <hr />
                <a href="/Admin/ViewList.aspx?type=agents">View Details</a>
            </div>
        </div>

    </form>
</body>
</html>



