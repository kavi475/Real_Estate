<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bookings.aspx.cs" Inherits="WebApplication1.Agent.Bookings" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bookings</title>
    <link rel="stylesheet" href="/CSS/agent-dashboard.css">
    <link rel="stylesheet" href="/CSS/AgentDashboardCommon.css">
    <link rel="stylesheet" href="/CSS/AgentComponent.css">
    <style>
        .filter-export-container {
            display: flex;
            justify-space: space-between;
            justify-content: space-between;
            align-items: center;
            background: #ffffff;
            border: 1px solid #e0e0e0;
            padding: 15px 20px;
            border-radius: 8px;
            margin-bottom: 20px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.05);
        }
        .filter-section {
            display: flex;
            align-items: center;
            gap: 10px;
            flex-wrap: wrap;
        }
        .filter-label {
            font-weight: bold;
            color: #333;
            font-size: 14px;
        }
        .filter-dropdown {
            padding: 8px 12px;
            border-radius: 4px;
            border: 1px solid #ccc;
            background-color: #fff;
            color: #333;
            font-size: 14px;
            outline: none;
            cursor: pointer;
        }
        .filter-dropdown:focus {
            border-color: #007bff;
        }
        .btn-export {
            background-color: #28a745;
            color: white;
            border: none;
            padding: 8px 16px;
            border-radius: 4px;
            font-size: 14px;
            font-weight: bold;
            cursor: pointer;
            transition: background-color 0.2s;
        }
        .btn-export:hover {
            background-color: #218838;
        }
        /* Custom Status Badge Styling */
        .status-badge {
            padding: 4px 10px;
            border-radius: 4px;
            font-size: 12px;
            font-weight: bold;
            text-transform: capitalize;
            display: inline-block;
        }
        .status-pending {
            background-color: #ffeeba;
            color: #856404;
            border: 1px solid #ffeeba;
        }
        .status-approved {
            background-color: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
        }
        .status-rejected {
            background-color: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
    </style>
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

                <!-- Real-time status count badges -->
                <div class="status-counts-container" style="margin-bottom: 15px; display: flex; gap: 15px;">
                    <span class="status-badge status-pending" style="font-size: 14px; padding: 6px 12px;">
                        Pending (<asp:Label ID="lblCountPending" runat="server" Text="0"></asp:Label>)
                    </span>
                    <span class="status-badge status-approved" style="font-size: 14px; padding: 6px 12px;">
                        Approved (<asp:Label ID="lblCountApproved" runat="server" Text="0"></asp:Label>)
                    </span>
                    <span class="status-badge status-rejected" style="font-size: 14px; padding: 6px 12px;">
                        Rejected (<asp:Label ID="lblCountRejected" runat="server" Text="0"></asp:Label>)
                    </span>
                </div>

                <div class="filter-export-container">
                    <div class="filter-section">
                        <span class="filter-label">Filter by Status:</span>
                        <asp:DropDownList ID="ddlStatusFilter" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged" CssClass="filter-dropdown">
                            <asp:ListItem Text="All" Value="ALL"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="PENDING"></asp:ListItem>
                            <asp:ListItem Text="Approved" Value="APPROVED"></asp:ListItem>
                            <asp:ListItem Text="Rejected" Value="REJECTED"></asp:ListItem>
                        </asp:DropDownList>

                        <span class="filter-label" style="margin-left: 15px;">Search:</span>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="filter-dropdown" placeholder="Search ID or Name..."></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" CssClass="filter-dropdown" style="background-color: #007bff; color: white; border: none; font-weight: bold;" />
                        <asp:Button ID="btnClearSearch" runat="server" OnClick="btnClearSearch_Click" Text="Clear" CssClass="filter-dropdown" style="background-color: #6c757d; color: white; border: none; font-weight: bold;" />
                    </div>
                    <div class="export-section">
                        <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click"
                            Text="Export to Excel" CssClass="btn-export" />
                    </div>
                </div>

                <div style="margin-bottom: 15px;">
                    <asp:Button ID="btnBulkApprove" runat="server" OnClick="btnBulkApprove_Click"
                        Text="Approve Selected" CssClass="filter-dropdown" style="background-color: #28a745; color: white; border: none; font-weight: bold; margin-right: 10px;" />
                    <asp:Button ID="btnBulkReject" runat="server" OnClick="btnBulkReject_Click"
                        OnClientClick="return confirm('Reject selected bookings?');"
                        Text="Reject Selected" CssClass="filter-dropdown" style="background-color: #dc3545; color: white; border: none; font-weight: bold;" />
                </div>

                <asp:Label ID="lblMsg" runat="server"
                    style="color:green; margin-bottom:10px; display:block;">
                </asp:Label>

                <asp:GridView ID="gvBookings" runat="server"
                    CssClass="table"
                    AutoGenerateColumns="false"
                    AllowPaging="true"
                    PageSize="10"
                    OnPageIndexChanging="gvBookings_PageIndexChanging"
                    OnRowCommand="gvBookings_RowCommand">
                    <Columns>
                        
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input type="checkbox" id="chkAll" onclick="SelectAllCheckboxes(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" Enabled='<%# Eval("Status").ToString() == "Pending" %>' />
                                <asp:HiddenField ID="hdnBookingId" runat="server" Value='<%# Eval("BookingId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Title" HeaderText="Property" />
                        <asp:BoundField DataField="UserEmail" HeaderText="User" />
                        <asp:BoundField DataField="BookingDate" HeaderText="Date"
                            DataFormatString="{0:dd-MMM-yyyy}" />

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='<%# "status-badge status-" + Eval("Status").ToString().ToLower() %>'>
                                    <%# Eval("Status") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

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

            </div>
        </div>
    </form>
    <script type="text/javascript">
        function SelectAllCheckboxes(headerChk) {
            var gv = document.getElementById('<%= gvBookings.ClientID %>');
            if (!gv) return;
            var checkboxes = gv.getElementsByTagName("input");
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == "checkbox" && checkboxes[i].id.indexOf("chkSelect") !== -1 && !checkboxes[i].disabled) {
                    checkboxes[i].checked = headerChk.checked;
                }
            }
        }
    </script>
</body>
</html>