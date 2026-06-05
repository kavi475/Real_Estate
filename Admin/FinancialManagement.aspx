<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="FinancialManagement.aspx.cs"
    Inherits="RealEstate.Admin.FinancialManagement" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Financial Management</title>

    <!-- SAME CSS AS ADMIN DASHBOARD -->
    <link rel="stylesheet" href="/css/AdminDashboard.css" />
    <style>
        /* =========================
   FINANCIAL MANAGEMENT FIX
   ========================= */

/* Card grid alignment */
.cards {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
    gap: 20px;
    margin-top: 20px;
}

/* Card polish */
.card {
    background: #ffffff;
    padding: 18px;
    border-radius: 8px;
    box-shadow: 0 2px 6px rgba(0,0,0,0.08);
    text-align: center;
}

.card p {
    font-weight: 600;
    color: #444;
}

.card hr {
    margin: 10px 0;
}

/* =========================
   GRIDVIEW TABLE STYLING
   ========================= */
.table {
    width: 100%;
    border-collapse: collapse;
    margin-top: 15px;
}

.table th {
    background: #f5f5f5;
    padding: 10px;
    text-align: left;
    border-bottom: 2px solid #ddd;
}

.table td {
    padding: 10px;
    border-bottom: 1px solid #e0e0e0;
}

/* Hover effect */
.table tr:hover {
    background-color: #fafafa;
}

/* =========================
   EXPORT BUTTON ALIGNMENT
   ========================= */
.btn {
    padding: 8px 16px;
    margin-right: 10px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    background: #2d89ef;
    color: #fff;
}

.btn:hover {
    background: #1b5fc1;
}

/* Button group spacing */
.card .btn {
    margin-top: 10px;
}
    </style>
</head>
<body>

<form id="form1" runat="server">

    <!-- ================= SIDEBAR ================= -->
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

    <!-- ================= MAIN CONTENT ================= -->
    <div class="main-content">

        <h1>Financial Management</h1>
        <p><asp:Label ID="lblCurrentMonth" runat="server" /></p>

        <!-- ================= KPI CARDS ================= -->
        <div class="cards">

            <div class="card">
                <p>Total Bookings</p>
                <hr />
                <asp:Label ID="lblTotalBookings" runat="server" Text="0"></asp:Label>
            </div>

            <div class="card">
                <p>Total Revenue</p>
                <hr />
                ₹ <asp:Label ID="lblTotalRevenue" runat="server" Text="0.00"></asp:Label>
            </div>

            <div class="card">
                <p>Monthly Revenue</p>
                <hr />
                ₹ <asp:Label ID="lblMonthlyRevenue" runat="server" Text="0.00"></asp:Label>
            </div>

        </div>

        <!-- ================= AGENT COMMISSION ================= -->
        <div class="card" style="width:100%; margin-top:20px;">
            <h3>Agent Commission Report</h3>

            <asp:GridView ID="gvAgentCommission" runat="server"
                AutoGenerateColumns="True"
                AllowPaging="true"
                PageSize="10"
                CssClass="table"
                OnPageIndexChanging="gvAgentCommission_PageIndexChanging">
            </asp:GridView>

            <br />

            <asp:Button ID="btnExportExcel" runat="server"
                Text="Export Excel"
                CssClass="btn"
                OnClick="btnExportExcel_Click" />

            <asp:Button ID="btnExportPDF" runat="server"
                Text="Export PDF"
                CssClass="btn"
                OnClick="btnExportPDF_Click" />
        </div>

        <!-- ================= CANCELLED BOOKINGS ================= -->
        <div class="card" style="width:100%; margin-top:20px;">
            <h3>Cancelled / Refunded Bookings</h3>

            <asp:GridView ID="gvCancelledBookings" runat="server"
                AutoGenerateColumns="True"
                AllowPaging="true"
                PageSize="10"
                CssClass="table"
                OnPageIndexChanging="gvCancelledBookings_PageIndexChanging">
            </asp:GridView>
        </div>

    </div>

</form>

</body>
</html>