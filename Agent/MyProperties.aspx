<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyProperties.aspx.cs" Inherits="WebApplication1.Agent.MyProperties" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Properties</title>
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
            <a href="/Agent/MyProperties.aspx" class="active">My Properties</a>
            <a href="/Agent/Bookings.aspx">Bookings</a>
            <a href="/Agent/Profile.aspx">Profile</a>
            <a href="../Logout.aspx">Logout</a>
        </div>
        <div class="main">
            <div class="content">
                <h1>My Properties</h1>
                <asp:GridView ID="gvProperties" runat="server"
                    CssClass="table"
                    AutoGenerateColumns="false"
                    OnRowCommand="gvProperties_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Image">
                            <ItemTemplate>
                                <img src='/<%# Eval("ImagePath") %>'
                                     width="80" height="60"
                                     style="border-radius:6px; object-fit:cover;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="Location" HeaderText="Location" />
                        <asp:BoundField DataField="Price" HeaderText="Price" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="IsApproved" HeaderText="Approval" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <a href='PropertyDetail.aspx?id=<%# Eval("PropertyId") %>'
                                   style="color:green; margin-right:10px;">
                                    View
                                </a>
                                <a href='EditProperty.aspx?id=<%# Eval("PropertyId") %>'
                                   style="color:blue; margin-right:10px;">
                                    Edit
                                </a>
                                <asp:LinkButton ID="btnDelete" runat="server"
                                    CommandName="DeleteProperty"
                                    CommandArgument='<%# Eval("PropertyId") %>'
                                    OnClientClick="return confirm('Are you sure you want to delete this property?');"
                                    style="color:red;">
                                    Delete
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Label ID="lblMsg" runat="server" style="color:green; margin-top:10px; display:block;"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>