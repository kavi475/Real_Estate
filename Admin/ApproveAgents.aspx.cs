using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace WebApplication1.Admin
{
    public partial class ApproveAgents : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null || Session["role"] == null
                || Session["role"].ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
                LoadPendingAgents();
        }

        private void LoadPendingAgents()
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT u.UserId, u.Email, ap.Status
                    FROM Users u
                    INNER JOIN AgentProfiles ap ON u.UserId = ap.UserId
                    WHERE ap.Status = 'Pending'", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvAgents.DataSource = dt;
                gvAgents.DataBind();

                if (dt.Rows.Count == 0)
                    lblMessage.Text = "No pending agent approvals.";
            }
        }

        protected void gvAgents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve")
            {
                int userId = Convert.ToInt32(e.CommandArgument);

                GridViewRow row =
                    ((Button)e.CommandSource).NamingContainer as GridViewRow;

                TextBox txtCommission =
                    row.FindControl("txtCommission") as TextBox;

                decimal commission = 10; // default
                decimal.TryParse(txtCommission.Text, out commission);

                ApproveAgent(userId, commission);

                lblMessage.Text = "✔ Agent approved with commission set.";
                LoadPendingAgents();
            }
        }

        private void ApproveAgent(int userId, decimal commission)
        {
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();

                // 1️⃣ Approve agent profile
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE AgentProfiles
                    SET Status = 'Approved',
                        ApprovedAt = GETDATE()
                    WHERE UserId = @UserId", con);

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.ExecuteNonQuery();

                // 2️⃣ Set role + commission
                SqlCommand cmdUser = new SqlCommand(@"
                    UPDATE Users
                    SET Role = 'Agent',
                        CommissionPct = @Commission
                    WHERE UserId = @UserId", con);

                cmdUser.Parameters.AddWithValue("@UserId", userId);
                cmdUser.Parameters.AddWithValue("@Commission", commission);
                cmdUser.ExecuteNonQuery();
            }
        }
    }
}