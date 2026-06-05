<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ApproveAgents.aspx.cs"
    Inherits="WebApplication1.Admin.ApproveAgents" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Approve Agents</title>
    <link rel="stylesheet" href="/css/AdminDashboard.css" />

    <style>
        body {
            margin: 0;
            font-family: Arial, sans-serif;
            background-color: #f0f4f8;
            display: flex;
        }

        .main-content {
            margin-left: 220px;
            padding: 30px 40px;
            width: calc(100% - 220px);
        }

        h2 {
            color: #1a2f4e;
            margin-bottom: 20px;
        }

        .grid-wrapper {
            background: #fff;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            overflow-x: auto;
        }

        .styled-grid {
            width: 100%;
            border-collapse: collapse;
        }

        .styled-grid th {
            background-color: #1a2f4e;
            color: #fff;
            padding: 12px;
            text-align: center;
            font-size: 14px;
        }

        .styled-grid td {
            padding: 10px;
            border-bottom: 1px solid #e8edf2;
            text-align: center;
            font-size: 14px;
        }

        .styled-grid tr:hover td {
            background-color: #f5f8fb;
        }

        .commission-box {
            width: 60px;
            padding: 4px;
            text-align: center;
        }

        .badge-pending {
            background: #fff3cd;
            color: #856404;
            padding: 4px 12px;
            border-radius: 14px;
            font-size: 12px;
            font-weight: 600;
            display: inline-block;
        }

        .btn-approve {
            background-color: #28a745;
            color: #fff;
            border: none;
            padding: 6px 18px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 13px;
        }

        .btn-approve:hover {
            background-color: #1e7e34;
        }

        .msg-box {
            margin-top: 18px;
            padding: 10px 14px;
            background: #e9f9ef;
            color: #1e7e34;
            border-left: 5px solid #28a745;
            font-weight: 600;
            border-radius: 4px;
            width: fit-content;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">

    <!-- SIDEBAR -->
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

    <!-- MAIN CONTENT -->
    <div class="main-content">
        <h2>Pending Agent Approvals</h2>

        <div class="grid-wrapper">
            <asp:GridView ID="gvAgents" runat="server"
                AutoGenerateColumns="False"
                CssClass="styled-grid"
                GridLines="None"
                OnRowCommand="gvAgents_RowCommand">

                <Columns>

                    <asp:BoundField DataField="UserId" HeaderText="User ID" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />

                    <asp:TemplateField HeaderText="Commission (%)">
                        <ItemTemplate>
                            <asp:TextBox ID="txtCommission"
                                runat="server"
                                CssClass="commission-box"
                                Text="10" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class="badge-pending">Pending</span>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button
                                runat="server"
                                Text="Approve"
                                CssClass="btn-approve"
                                CommandName="Approve"
                                CommandArgument='<%# Eval("UserId") %>'
                                OnClientClick="return confirm('Approve this agent with selected commission?');" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>

        <!-- MESSAGE (FIXED POSITION) -->
        <asp:Label ID="lblMessage"
            runat="server"
            CssClass="msg-box"
            Visible="false">
        </asp:Label>

    </div>
</form>
</body>
</html>