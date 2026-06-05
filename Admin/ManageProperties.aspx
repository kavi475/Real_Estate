<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ManageProperties.aspx.cs"
    Inherits="WebApplication1.Admin.ManageProperties" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Properties</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

    <!-- Your Dashboard CSS -->
    <link rel="stylesheet" href="/css/AdminDashboard.css" />

    <style>
        .main { margin-left: 260px; padding: 25px; }

        .btn-approve { background:#2ecc71; color:#fff; border:none; padding:6px 12px; border-radius:5px; }
        .btn-reject  { background:#f39c12; color:#fff; border:none; padding:6px 12px; border-radius:5px; }
        .btn-delete  { background:#e74c3c; color:#fff; border:none; padding:6px 12px; border-radius:5px; }

        /* Custom Reject Modal Overlay */
        #rejectOverlay {
            display: none;
            position: fixed;
            top: 0; left: 0;
            width: 100%; height: 100%;
            background: rgba(0,0,0,0.55);
            z-index: 9999;
            justify-content: center;
            align-items: center;
        }
        #rejectOverlay.show {
            display: flex;
        }
        #rejectBox {
            background: #fff;
            border-radius: 8px;
            padding: 30px;
            width: 450px;
            box-shadow: 0 10px 40px rgba(0,0,0,0.3);
        }
        #rejectBox h5 {
            margin-bottom: 15px;
            font-weight: 600;
        }
        #rejectBox textarea {
            width: 100%;
            height: 100px;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            resize: vertical;
            font-size: 14px;
        }
        #rejectBox .modal-actions {
            margin-top: 15px;
            display: flex;
            justify-content: flex-end;
            gap: 10px;
        }
    </style>
</head>

<body>
<form runat="server">

    <!-- ================= SIDEBAR ================= -->
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
    <!-- =========================================== -->

    <div class="main">

        <h2>Manage Properties</h2>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>

        <!-- Hidden Fields -->
        <asp:HiddenField ID="hfRejectPropertyId" runat="server" />
        <asp:HiddenField ID="hfKeepModalOpen"    runat="server" Value="0" />

        <hr />

        <!-- AVAILABLE / PENDING -->
        <h4>Available &amp; Pending Properties</h4>

        <asp:GridView ID="gvAvailable" runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-bordered"
            DataKeyNames="PropertyId"
            OnRowCommand="gvAvailable_RowCommand">
            <Columns>
                <asp:BoundField DataField="PropertyId" HeaderText="ID" />
                <asp:BoundField DataField="Title"      HeaderText="Title" />
                <asp:BoundField DataField="AgentName"  HeaderText="Agent Name" />
                <asp:BoundField DataField="Price"      HeaderText="Price" DataFormatString="₹{0:N2}" />
                <asp:BoundField DataField="Status"     HeaderText="Status" />

                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:LinkButton runat="server"
                            Text="Approve"
                            CssClass="btn-approve"
                            CommandName="Approve"
                            CommandArgument='<%# Eval("PropertyId") %>' />

                        <%-- Reject: pure client-side, NO postback --%>
                        <button type="button"
                            class="btn-reject"
                            onclick="openRejectBox('<%# Eval("PropertyId") %>')">
                            Reject
                        </button>

                        <asp:LinkButton runat="server"
                            Text="Delete"
                            CssClass="btn-delete"
                            CommandName="DeleteProperty"
                            CommandArgument='<%# Eval("PropertyId") %>'
                            OnClientClick="return confirm('Are you sure you want to delete this property?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <hr />

        <!-- BOOKED -->
        <h4>Booked Properties</h4>
        <asp:GridView ID="gvBooked" runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-bordered">
            <Columns>
                <asp:BoundField DataField="PropertyId"  HeaderText="ID" />
                <asp:BoundField DataField="Title"       HeaderText="Title" />
                <asp:BoundField DataField="Email"       HeaderText="Booked By" />
                <asp:BoundField DataField="BookingDate" HeaderText="Date" />
            </Columns>
        </asp:GridView>

        <hr />

        <!-- REJECTED -->
        <h4>Rejected Properties</h4>
        <asp:GridView ID="gvRejected" runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-bordered">
            <Columns>
                <asp:BoundField DataField="PropertyId"    HeaderText="ID" />
                <asp:BoundField DataField="Title"         HeaderText="Title" />
                <asp:BoundField DataField="AgentName"     HeaderText="Agent Name" />
                <asp:BoundField DataField="Price"         HeaderText="Price" DataFormatString="₹{0:N2}" />
                <asp:BoundField DataField="Status"        HeaderText="Status" />
                <asp:BoundField DataField="RejectionReason" HeaderText="Rejection Reason" />
            </Columns>
        </asp:GridView>

    </div>

    <!-- ================= REJECT OVERLAY (Pure HTML — no Bootstrap modal) ================= -->
    <div id="rejectOverlay">
        <div id="rejectBox">
            <h5>Reject Property</h5>
            <label>Rejection Reason <span style="color:red">*</span></label>
            <textarea id="txtReasonClient" placeholder="Enter reason..."></textarea>
            <p id="rejectClientError" style="color:red; margin-top:6px; display:none;">
                Rejection reason is required.
            </p>
            <div class="modal-actions">
                <button type="button"
                    class="btn btn-secondary"
                    onclick="closeRejectBox()">Cancel</button>
                <button type="button"
                    class="btn btn-danger"
                    onclick="submitReject()">Reject Property</button>
            </div>
        </div>
    </div>
    <!-- =================================================================================== -->

    <%-- Hidden ASP controls to carry reason + trigger server postback --%>
    <asp:HiddenField ID="hfRejectReason" runat="server" />
    <asp:Button ID="btnDoReject"
        runat="server"
        Style="display:none"
        OnClick="btnDoReject_Click" />

</form>

<script>
    function openRejectBox(propertyId) {
        document.getElementById('<%= hfRejectPropertyId.ClientID %>').value = propertyId;
        document.getElementById('txtReasonClient').value = '';
        document.getElementById('rejectClientError').style.display = 'none';
        document.getElementById('rejectOverlay').classList.add('show');
    }

    function closeRejectBox() {
        document.getElementById('rejectOverlay').classList.remove('show');
    }

    function submitReject() {
        var reason = document.getElementById('txtReasonClient').value.trim();
        if (!reason) {
            document.getElementById('rejectClientError').style.display = 'block';
            return;
        }
        // Pass reason to hidden field then click the hidden server button
        document.getElementById('<%= hfRejectReason.ClientID %>').value = reason;
        document.getElementById('<%= btnDoReject.ClientID %>').click();
    }
</script>

</body>
</html>