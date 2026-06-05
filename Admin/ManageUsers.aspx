<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ManageUsers.aspx.cs"
    Inherits="WebApplication1.Admin.ManageUsers" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Users - REMS</title>
    <link rel="stylesheet" href="/css/AdminDashboard.css" />
    <style>
        .grid-style {
            width: 100%;
            border-collapse: collapse;
            margin-top: 15px;
            font-size: 14px;
            background-color: white;
        }
        .grid-style th {
            background-color: #f5f5f5;
            color: #333;
            text-align: left;
            padding: 12px;
            font-weight: 600;
            border-bottom: 2px solid #ddd;
        }
        .grid-style td {
            padding: 12px;
            border-bottom: 1px solid #eee;
            color: #555;
        }
        .grid-style tr:hover { background-color: #fafafa; }
        .action-btn {
            font-weight: 600;
            padding: 6px 12px;
            border-radius: 4px;
            margin-right: 5px;
            font-size: 12px;
            display: inline-block;
            cursor: pointer;
            border: none;
            text-decoration: none;
        }
        .text-edit   { background-color: #2196F3; color: white !important; }
        .text-delete { background-color: #f44336; color: white !important; }
        .text-update { background-color: #4CAF50; color: white !important; }
        .text-cancel { background-color: #9e9e9e; color: white !important; }
        .form-control-grid {
            padding: 6px;
            border: 1px solid #ccc;
            border-radius: 4px;
            width: 95%;
            box-sizing: border-box;
        }
        .grid-style .pager-style td {
            padding: 15px 5px;
            border-bottom: none;
            background-color: white;
        }
        .grid-style .pager-style a,
        .grid-style .pager-style span {
            display: inline-block;
            padding: 6px 12px;
            margin: 0 3px;
            border: 1px solid #ddd;
            border-radius: 4px;
            text-decoration: none;
            color: #2196F3;
            font-weight: 600;
        }
        .grid-style .pager-style span {
            background-color: #2196F3;
            color: white;
            border-color: #2196F3;
        }
    </style>
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
        <a href="FinancialManagement.aspx">Financial Management</a>
        <a href="../Logout.aspx">Logout</a>
    </div>

    <div class="main-content">
        <h1>Manage Users</h1>

        <asp:Label ID="lblMessage" runat="server" Style="font-weight:600; font-size:13px; display:block; margin-bottom:10px;" />

        <div class="card" style="width:100%; margin-top:20px; box-sizing:border-box; padding:20px;">
            <h3>&#10133; Add New User</h3>
            <hr style="border:0; border-top:1px solid #eee; margin:15px 0;" />

            <div style="display:flex; flex-wrap:wrap; gap:15px;">
                <div style="flex:1; min-width:200px;">
                    <label style="font-weight:600; font-size:13px; color:#666;">Email:</label>
                    <asp:TextBox ID="txtNewEmail" runat="server" placeholder="Enter Email"
                        Style="width:100%; padding:8px; box-sizing:border-box; margin-top:5px; border:1px solid #ccc; border-radius:4px;" />
                </div>
                <div style="flex:1; min-width:200px;">
                    <label style="font-weight:600; font-size:13px; color:#666;">Password:</label>
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" placeholder="Enter Password"
                        Style="width:100%; padding:8px; box-sizing:border-box; margin-top:5px; border:1px solid #ccc; border-radius:4px;" />
                </div>
                <div style="flex:1; min-width:150px;">
                    <label style="font-weight:600; font-size:13px; color:#666;">Role:</label>
                    <asp:DropDownList ID="ddlNewRole" runat="server"
                        Style="width:100%; padding:8px; box-sizing:border-box; margin-top:5px; border:1px solid #ccc; border-radius:4px;">
                        <asp:ListItem Value="Agent">Agent</asp:ListItem>
                        <asp:ListItem Value="User" Selected="True">User</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="flex:1; min-width:150px;">
                    <label style="font-weight:600; font-size:13px; color:#666;">Phone:</label>
                    <asp:TextBox ID="txtNewPhone" runat="server" placeholder="Enter Phone"
                        Style="width:100%; padding:8px; box-sizing:border-box; margin-top:5px; border:1px solid #ccc; border-radius:4px;" />
                </div>
            </div>

            <div style="margin-top:20px;">
                <asp:Button ID="btnAddUser" runat="server" Text="Add User"
                    OnClick="btnAddUser_Click"
                    Style="background-color:#4CAF50; color:white; border:none; padding:10px 20px; font-size:14px; border-radius:4px; cursor:pointer; font-weight:600;" />
            </div>
        </div>

        <div class="card" style="width:100%; box-sizing:border-box; padding:20px; margin-top:20px;">
            <h3>&#128101; Users List</h3>
            <hr style="border:0; border-top:1px solid #eee; margin:15px 0;" />

            <div style="overflow-x:auto;">
                <asp:GridView ID="gvUsers" runat="server"
                    CssClass="grid-style"
                    AutoGenerateColumns="False"
                    DataKeyNames="UserId"
                    AllowPaging="True"
                    PageSize="10"
                    OnRowEditing="gvUsers_RowEditing"
                    OnRowCancelingEdit="gvUsers_RowCancelingEdit"
                    OnRowUpdating="gvUsers_RowUpdating"
                    OnRowDeleting="gvUsers_RowDeleting"
                    OnPageIndexChanging="gvUsers_PageIndexChanging">

                    <PagerStyle CssClass="pager-style" HorizontalAlign="Center" />

                    <Columns>

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit"
                                    Text="Edit" CssClass="action-btn text-edit" />
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete"
                                    Text="Delete" CssClass="action-btn text-delete"
                                    OnClientClick="return confirm('Delete this user?');" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update"
                                    Text="Update" CssClass="action-btn text-update" />
                                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel"
                                    Text="Cancel" CssClass="action-btn text-cancel" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="UserId" HeaderText="User ID" ReadOnly="True" />

                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate><%# Eval("Email") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtGridEmail" runat="server"
                                    Text='<%# Bind("Email") %>' CssClass="form-control-grid" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Role">
                            <ItemTemplate><%# Eval("Role") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlGridRole" runat="server"
                                    SelectedValue='<%# Bind("Role") %>'
                                    CssClass="form-control-grid"
                                    Enabled='<%# Eval("Role").ToString() != "Admin" %>'>
                                    <asp:ListItem Value="Admin">Admin</asp:ListItem>
                                    <asp:ListItem Value="Agent">Agent</asp:ListItem>
                                    <asp:ListItem Value="User">User</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label runat="server"
                                    Visible='<%# Eval("Role").ToString() == "Admin" %>'
                                    Text="(Admin role cannot be changed)"
                                    Style="font-size:11px; color:#999;" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Phone">
                            <ItemTemplate><%# Eval("Phone") %></ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtGridPhone" runat="server"
                                    Text='<%# Bind("Phone") %>' CssClass="form-control-grid" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="CreatedAt" HeaderText="Created At"
                            ReadOnly="True" DataFormatString="{0:yyyy-MM-dd HH:mm}" />

                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</form>
</body>
</html>