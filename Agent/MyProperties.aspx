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
            <div class="content" style="padding: 20px;">
                <h1>My Properties</h1>
                
                <asp:Label ID="lblMsg" runat="server" style="color:green; margin-bottom:15px; display:block; font-weight:bold;"></asp:Label>

                <!-- APPROVED PROPERTIES -->
                <h3 style="color: #2ecc71;">Approved Properties</h3>
                <asp:GridView ID="gvApprovedProperties" runat="server"
                    CssClass="table"
                    AutoGenerateColumns="false"
                    OnRowCommand="gvProperties_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Image">
                            <ItemTemplate>
                                <img src='/<%# Eval("ImagePath") %>'
                                     width="80" height="60"
                                     style="border-radius:6px; object-fit:cover;" 
                                     onerror="this.src='/images/no-image.png';" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="Location" HeaderText="Location" />
                        <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="₹{0:N2}" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <a href='PropertyDetail.aspx?id=<%# Eval("PropertyId") %>'
                                   style="color:green; margin-right:10px; font-weight:bold;">
                                    View
                                </a>
                                <a href='Add-property.aspx?propertyId=<%# Eval("PropertyId") %>'
                                   style="color:blue; margin-right:10px; font-weight:bold;">
                                    Edit
                                </a>
                                <asp:LinkButton ID="btnDelete" runat="server"
                                    CommandName="DeleteProperty"
                                    CommandArgument='<%# Eval("PropertyId") %>'
                                    OnClientClick="return confirm('Are you sure you want to delete this property?');"
                                    style="color:red; font-weight:bold;">
                                    Delete
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <br /><br />

                <!-- PENDING PROPERTIES -->
                <h3 style="color: #f39c12;">Pending Properties (Under Review)</h3>
                <asp:GridView ID="gvPendingProperties" runat="server"
                    CssClass="table"
                    AutoGenerateColumns="false"
                    OnRowCommand="gvProperties_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Image">
                            <ItemTemplate>
                                <img src='/<%# Eval("ImagePath") %>'
                                     width="80" height="60"
                                     style="border-radius:6px; object-fit:cover;" 
                                     onerror="this.src='/images/no-image.png';" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="Location" HeaderText="Location" />
                        <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="₹{0:N2}" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <a href='PropertyDetail.aspx?id=<%# Eval("PropertyId") %>'
                                   style="color:green; margin-right:10px; font-weight:bold;">
                                    View
                                </a>
                                <a href='Add-property.aspx?propertyId=<%# Eval("PropertyId") %>'
                                   style="color:blue; margin-right:10px; font-weight:bold;">
                                    Edit
                                </a>
                                <asp:LinkButton ID="btnDelete" runat="server"
                                    CommandName="DeleteProperty"
                                    CommandArgument='<%# Eval("PropertyId") %>'
                                    OnClientClick="return confirm('Are you sure you want to delete this property?');"
                                    style="color:red; font-weight:bold;">
                                    Delete
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <br /><br />

                <!-- REJECTED PROPERTIES -->
                <h3 style="color: #d9534f;">Rejected Properties (Action Required)</h3>
                <asp:GridView ID="gvRejectedProperties" runat="server"
                    CssClass="table"
                    AutoGenerateColumns="false"
                    OnRowCommand="gvProperties_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Image">
                            <ItemTemplate>
                                <img src='/<%# Eval("ImagePath") %>'
                                     width="80" height="60"
                                     style="border-radius:6px; object-fit:cover;" 
                                     onerror="this.src='/images/no-image.png';" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="Location" HeaderText="Location" />
                        <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="₹{0:N2}" />
                        <asp:BoundField DataField="RejectionReason" HeaderText="Rejection Reason" ItemStyle-ForeColor="Red" ItemStyle-Font-Bold="true" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <a href='Add-property.aspx?propertyId=<%# Eval("PropertyId") %>'
                                   style="color:blue; margin-right:10px; font-weight:bold;">
                                    Edit & Resubmit
                                </a>
                                <asp:LinkButton ID="btnDelete" runat="server"
                                    CommandName="DeleteProperty"
                                    CommandArgument='<%# Eval("PropertyId") %>'
                                    OnClientClick="return confirm('Are you sure you want to delete this property?');"
                                    style="color:red; font-weight:bold;">
                                    Delete
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>