using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Admin
{
    public partial class ManageProperties : System.Web.UI.Page
    {
        private readonly string strcon =
            ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefreshAll();
            }
        }

        // ================= LOAD DATA =================

        private void LoadAvailableProperties()
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = @"
                    SELECT p.PropertyId, p.Title, p.Price, p.Status, p.RejectionReason,
                           u.Email AS AgentName
                    FROM Properties p
                    INNER JOIN Users u ON p.AgentId = u.UserId
                    WHERE p.Status <> 'Rejected'
                    ORDER BY p.PropertyId DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvAvailable.DataSource = dt;
                    gvAvailable.DataBind();
                }
            }
        }

        private void LoadBookedProperties()
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = @"
                    SELECT b.PropertyId, p.Title, u.Email, b.BookingDate
                    FROM Bookings b
                    INNER JOIN Properties p ON b.PropertyId = p.PropertyId
                    INNER JOIN Users u ON b.UserId = u.UserId
                    ORDER BY b.BookingId DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvBooked.DataSource = dt;
                    gvBooked.DataBind();
                }
            }
        }

        private void LoadRejectedProperties()
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = @"
                    SELECT p.PropertyId, p.Title, p.Price, p.Status, p.RejectionReason,
                           u.Email AS AgentName
                    FROM Properties p
                    INNER JOIN Users u ON p.AgentId = u.UserId
                    WHERE p.Status = 'Rejected'
                    ORDER BY p.PropertyId DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvRejected.DataSource = dt;
                    gvRejected.DataBind();
                }
            }
        }

        private void RefreshAll()
        {
            LoadAvailableProperties();
            LoadBookedProperties();
            LoadRejectedProperties();
        }

        // ================= GRID ACTIONS =================

        protected void gvAvailable_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sort" || e.CommandName == "Page") return;

            int propertyId;
            if (!int.TryParse(e.CommandArgument?.ToString(), out propertyId)) return;

            lblMessage.Text = "";

            switch (e.CommandName)
            {
                case "Approve":
                    ApproveProperty(propertyId);
                    RefreshAll();
                    break;

                case "DeleteProperty":
                    DeleteProperty(propertyId);
                    RefreshAll();
                    break;
            }
        }

        // ================= REJECT VIA HIDDEN BUTTON =================

        protected void btnDoReject_Click(object sender, EventArgs e)
        {
            string reason = hfRejectReason.Value.Trim();
            int propertyId;

            if (string.IsNullOrWhiteSpace(reason) ||
                !int.TryParse(hfRejectPropertyId.Value, out propertyId))
            {
                lblMessage.Text = "Invalid request. Please try again.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                RefreshAll();
                return;
            }

            RejectProperty(propertyId, reason);
            hfRejectPropertyId.Value = "";
            hfRejectReason.Value = "";
            RefreshAll();
        }

        // ================= ACTION METHODS =================

        private void ApproveProperty(int propertyId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();

                string currentStatus = GetStatus(propertyId, con);
                if (currentStatus != "Pending")
                {
                    lblMessage.Text = "Only Pending properties can be approved.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                SqlCommand cmd = new SqlCommand(
                    "UPDATE Properties SET Status='Approved', RejectionReason=NULL WHERE PropertyId=@id",
                    con);
                cmd.Parameters.AddWithValue("@id", propertyId);
                cmd.ExecuteNonQuery();

                lblMessage.Text = "Property approved successfully.";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void RejectProperty(int propertyId, string reason)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();

                string currentStatus = GetStatus(propertyId, con);
                if (currentStatus != "Pending")
                {
                    lblMessage.Text = "Only Pending properties can be rejected.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                SqlCommand cmd = new SqlCommand(
                    "UPDATE Properties SET Status='Rejected', RejectionReason=@reason WHERE PropertyId=@id",
                    con);
                cmd.Parameters.AddWithValue("@id", propertyId);
                cmd.Parameters.AddWithValue("@reason", reason);
                cmd.ExecuteNonQuery();

                lblMessage.Text = "Property rejected successfully.";
                lblMessage.ForeColor = System.Drawing.Color.OrangeRed;
            }
        }

        private void DeleteProperty(int propertyId)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    SqlCommand delBookings = new SqlCommand(
                        "DELETE FROM Bookings WHERE PropertyId=@id", con, trans);
                    delBookings.Parameters.AddWithValue("@id", propertyId);
                    delBookings.ExecuteNonQuery();

                    SqlCommand delImages = new SqlCommand(
                        "DELETE FROM PropertyImages WHERE PropertyId=@id", con, trans);
                    delImages.Parameters.AddWithValue("@id", propertyId);
                    delImages.ExecuteNonQuery();

                    SqlCommand delProp = new SqlCommand(
                        "DELETE FROM Properties WHERE PropertyId=@id", con, trans);
                    delProp.Parameters.AddWithValue("@id", propertyId);
                    delProp.ExecuteNonQuery();

                    trans.Commit();
                    lblMessage.Text = "Property deleted successfully.";
                    lblMessage.ForeColor = System.Drawing.Color.DarkRed;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    lblMessage.Text = "Error deleting property: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        // ================= HELPERS =================

        private string GetStatus(int propertyId, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT Status FROM Properties WHERE PropertyId=@id", con);
            cmd.Parameters.AddWithValue("@id", propertyId);
            object result = cmd.ExecuteScalar();
            return result != null ? result.ToString() : "";
        }
    }
}