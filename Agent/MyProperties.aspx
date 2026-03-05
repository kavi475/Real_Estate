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
            <a href="/Agent/MyProperties.aspx">My Properties</a>
            <a href="/Agent/Bookings.aspx">Bookings</a>
            <a href="/Agent/Profile.aspx">Profile</a>
            <a href="Logout.aspx">Logout</a>
        </div>

        <div class="main">
            <div class="content">
                <h1>My Properties</h1>

                <table class="table">
                    <tr>
                        <th>Image</th>
                        <th>Title</th>
                        <th>Status</th>
                        <th>Action</th>
                    </tr>
                    <tr>
                        <td>IMG</td>
                        <td>2BHK Flat</td>
                        <td>Pending</td>
                        <td>Edit | View</td>
                    </tr>
                </table>
            </div>
        </div>

    </form>
</body>
</html>
