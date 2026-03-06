<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageProperties.aspx.cs" Inherits="WebApplication1.Admin.ManageProperties" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Properties - REMS</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; font-family: Arial, sans-serif; }
        .sidebar {
            width: 220px;
            height: 100vh;
            background: #1e1e2f;
            position: fixed;
            top: 0;
            left: 0;
            padding-top: 20px;
        }
        .logo {
            color: white;
            text-align: center;
            margin-bottom: 30px;
            font-size: 22px;
            font-weight: bold;
        }
        .sidebar a {
            display: block;
            color: #ddd;
            padding: 12px 20px;
            text-decoration: none;
            font-size: 15px;
        }
        .sidebar a:hover, .sidebar a.active {
            background: #667eea;
            color: white;
        }
        .main-content {
            margin-left: 220px;
            padding: 30px;
            background: #f4f6f9;
            min-height: 100vh;
        }
        .page-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 25px;
        }
        .page-header h2 {
            font-size: 26px;
            color: #2f3640;
        }
        .btn-add {
            background: #667eea;
            color: white;
            border: none;
            padding: 10px 22px;
            border-radius: 6px;
            font-size: 14px;
            cursor: pointer;
            text-decoration: none;
        }
        .btn-add:hover {
            background: #5a67d8;
            color: white;
        }
        .search-bar {
            background: white;
            padding: 15px 20px;
            border-radius: 10px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.07);
            margin-bottom: 20px;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        .search-input {
            flex: 1;
            padding: 10px 16px;
            border: 1px solid #ddd;
            border-radius: 6px;
            font-size: 14px;
            height: 42px;
        }
        .search-input:focus {
            outline: none;
            border-color: #667eea;
        }
        .btn-search {
            background: #667eea;
            color: white;
            border: none;
            padding: 10px 24px;
            border-radius: 6px;
            cursor: pointer;
            font-size: 14px;
            font-weight: bold;
            height: 42px;
            width: auto;
            white-space: nowrap;
        }
        .btn-search:hover { background: #5a67d8; color: white; }
        .table-card {
            background: white;
            border-radius: 10px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.07);
            overflow: hidden;
        }
        .table-card table {
            width: 100%;
            border-collapse: collapse;
        }
        .table-card thead {
            background: #1e1e2f;
            color: white;
        }
        .table-card thead th {
            padding: 14px 16px;
            font-size: 14px;
            font-weight: 600;
        }
        .table-card tbody tr {
            border-bottom: 1px solid #f0f0f0;
            transition: 0.2s;
        }
        .table-card tbody tr:hover { background: #f8f9ff; }
        .table-card tbody td {
            padding: 13px 16px;
            font-size: 14px;
            color: #444;
        }
        .badge-pending {
            background: #fff3cd;
            color: #856404;
            padding: 4px 10px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: bold;
        }
        .badge-approved {
            background: #d4edda;
            color: #155724;
            padding: 4px 10px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: bold;
        }
        .badge-rejected {
            background: #f8d7da;
            color: #721c24;
            padding: 4px 10px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: bold;
        }
        .btn-approve {
            background: #2ecc71;
            color: white;
            border: none;
            padding: 5px 12px;
            border-radius: 5px;
            cursor: pointer;
            font-size: 12px;
            margin-right: 3px;
        }
        .btn-reject {
            background: #e67e22;
            color: white;
            border: none;
            padding: 5px 12px;
            border-radius: 5px;
            cursor: pointer;
            font-size: 12px;
            margin-right: 3px;
        }
        .btn-delete {
            background: #e74c3c;
            color: white;
            border: none;
            padding: 5px 12px;
            border-radius: 5px;
            cursor: pointer;
            font-size: 12px;
        }
        .btn-approve:hover { background: #27ae60; }
        .btn-reject:hover { background: #d35400; }
        .btn-delete:hover { background: #c0392b; }
    </style>
</head>
<body>
<form id="form1" runat="server">

    <!-- SIDEBAR -->
    <div class="sidebar">
        <h2 class="logo">REMS</h2>
        <a href="AdminDashboard.aspx">Dashboard</a>
        <a href="ManageProperties.aspx" class="active">Properties</a>
        <a href="ManageUsers.aspx">Users</a>
        <a href="ManageBookings.aspx">Bookings</a>
        <a href="../Logout.aspx">Logout</a>
    </div>

    <!-- MAIN CONTENT -->
    <div class="main-content">

        <!-- HEADER -->
        <div class="page-header">
            <h2>Manage Properties</h2>
            <a href="AddProperty.aspx" class="btn-add">+ Add Property</a>
        </div>

        <!-- SEARCH BAR -->
        <div class="search-bar">
            <asp:TextBox ID="txtSearch" runat="server"
                CssClass="search-input"
                placeholder="Search by title or location..."></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server"
                Text="🔍 Search"
                CssClass="btn-search"
                OnClick="btnSearch_Click" />
        </div>

        <!-- TABLE -->
        <div class="table-card">
            <asp:GridView ID="gvProperties" runat="server"
                AutoGenerateColumns="false"
                CssClass="table"
                OnRowCommand="gvProperties_RowCommand"
                DataKeyNames="PropertyId"
                EmptyDataText="No properties found.">
                <Columns>
                    <asp:BoundField DataField="PropertyId" HeaderText="ID" />
                    <asp:BoundField DataField="Title" HeaderText="Title" />
                    <asp:BoundField DataField="Location" HeaderText="Location" />
                    <asp:BoundField DataField="Price" HeaderText="Price (₹)" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:TemplateField HeaderText="Approval">
                        <ItemTemplate>
                            <span class='<%# Eval("IsApproved").ToString() == "Approved" ? "badge-approved" : Eval("IsApproved").ToString() == "Rejected" ? "badge-rejected" : "badge-pending" %>'>
                                <%# Eval("IsApproved") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnApprove" runat="server"
                                Text="Approve"
                                CssClass="btn-approve"
                                CommandName="Approve"
                                CommandArgument='<%# Eval("PropertyId") %>' />
                            <asp:Button ID="btnReject" runat="server"
                                Text="Reject"
                                CssClass="btn-reject"
                                CommandName="Reject"
                                CommandArgument='<%# Eval("PropertyId") %>' />
                            <asp:Button ID="btnDelete" runat="server"
                                Text="Delete"
                                CssClass="btn-delete"
                                CommandName="DeleteProp"
                                CommandArgument='<%# Eval("PropertyId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

    </div>

</form>
</body>
</html>