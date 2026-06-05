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
        <a href="ApproveAgents.aspx">Approve Agents</a>
        <a href="FinancialManagement.aspx">Financial Mangment</a>
        <a href="../Logout.aspx">Logout</a>
    </div>

    <div class="main-content">
        <h1>Admin Dashboard</h1>
    </div>

    <!-- CARDS -->
    <div class="cards">

        <div class="card">
            <p>Total Property</p>
            <hr />
            <asp:Label ID="lblProperty" runat="server"></asp:Label>
            <hr />
            <a href="/Admin/ViewList.aspx?type=properties">View Details</a>
        </div>

        <div class="card">
            <p>Total State</p>
            <hr />
            <asp:Label ID="lblState" runat="server"></asp:Label>
            <hr />
            <a href="/Admin/ViewList.aspx?type=states">View Details</a>
        </div>

        <div class="card">
            <p>Total City</p>
            <hr />
            <asp:Label ID="lblCity" runat="server"></asp:Label>
            <hr />
            <a href="/Admin/ViewList.aspx?type=cities">View Details</a>
        </div>

        <div class="card">
            <p>Total Revenue</p>
            <hr />
            <asp:Label ID="lblRevenue" runat="server"></asp:Label>
            <hr />
            <a href="/Admin/ViewList.aspx?type=revenue">View Details</a>
        </div>

    </div>

    <div class="cards">

        <div class="card">
            <p>Total Users</p>
            <hr />
            <asp:Label ID="lblUsers" runat="server"></asp:Label>
            <hr />
            <a href="/Admin/ViewList.aspx?type=users">View Details</a>
        </div>

        <div class="card">
            <p>Total Listed Property</p>
            <hr />
            <asp:Label ID="lblListed" runat="server"></asp:Label>
            <hr />
            <a href="/Admin/ViewList.aspx?type=PropertyListed">View Details</a>
        </div>

        <div class="card">
            <p>Total Agent Listed</p>
            <hr />
            <asp:Label ID="lblAgents" runat="server"></asp:Label>
            <hr />
            <a href="/Admin/ViewList.aspx?type=agents">View Details</a>
        </div>

        <div class="charts-container">
            <h2>Dashboard Analytics</h2>

            <canvas id="bookingsChart"></canvas>
            <canvas id="revenueChart"></canvas>
            <canvas id="cityChart"></canvas>
        </div>

    </div>

    <!-- ACTIVITY -->
    <div class="card" style="width:100%; margin-top:20px;">
        <h3>📜 Activity Feed</h3>

        <asp:Repeater ID="rptActivity" runat="server">
            <ItemTemplate>
                <div style="padding:5px; border-bottom:1px solid #ddd;">
                    <b><%# Eval("Action") %></b><br />
                    <small><%# Eval("CreatedAt") %></small>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <%-- Shows only when ActivityLogs table is empty --%>
        <asp:Label ID="lblNoActivity" runat="server" 
                   Text="No activity recorded yet." 
                   ForeColor="Gray"
                   Visible="false">
        </asp:Label>

    </div>

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script>
        // ── Safe data injection from CodeBehind ──────────────────────
        var bookingLabels = [<%= BookingLabels %>];
        var bookingData = [<%= BookingData %>];

        var revenueLabels = [<%= RevenueLabels %>];
        var revenueData   = [<%= RevenueData %>];

        var cityLabels    = [<%= CityLabels %>];
        var cityData      = [<%= CityData %>];

        // ── Booking Chart ─────────────────────────────────────────────
        new Chart(document.getElementById('bookingsChart'), {
            type: 'line',
            data: {
                labels: bookingLabels,
                datasets: [{
                    label: 'Bookings',
                    data: bookingData,
                    fill: false,
                    borderColor: 'rgba(54,162,235,1)',
                    tension: 0.3
                }]
            },
            options: {
                responsive: true,
                plugins: { legend: { display: true } },
                scales: { y: { beginAtZero: true } }
            }
        });

        // ── Revenue Chart ─────────────────────────────────────────────
        new Chart(document.getElementById('revenueChart'), {
            type: 'bar',
            data: {
                labels: revenueLabels,
                datasets: [{
                    label: 'Revenue (₹)',
                    data: revenueData,
                    backgroundColor: 'rgba(75,192,192,0.6)',
                    borderColor: 'rgba(75,192,192,1)',
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                plugins: { legend: { display: true } },
                scales: { y: { beginAtZero: true } }
            }
        });

        // ── City Chart ────────────────────────────────────────────────
        new Chart(document.getElementById('cityChart'), {
            type: 'pie',
            data: {
                labels: cityLabels,
                datasets: [{
                    data: cityData,
                    backgroundColor: [
                        'rgba(255,99,132,0.7)',
                        'rgba(54,162,235,0.7)',
                        'rgba(255,206,86,0.7)',
                        'rgba(75,192,192,0.7)',
                        'rgba(153,102,255,0.7)'
                    ]
                }]
            },
            options: {
                responsive: true,
                plugins: { legend: { position: 'bottom' } }
            }
        });
    </script>

</form>
</body>
</html>