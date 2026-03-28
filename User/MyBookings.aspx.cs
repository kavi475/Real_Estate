using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebApplication1.User
{
    public partial class MyBookings : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null) { Response.Redirect("/Login.aspx"); return; }
            if (!IsPostBack)
            {
                lblWelcome.Text = "Hi, " + Session["email"].ToString();
                LoadBookings();
            }
        }

        int GetUserId()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("SELECT UserId FROM Users WHERE Email=@email", con);
            cmd.Parameters.AddWithValue("@email", Session["email"].ToString());
            con.Open();
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return id;
        }

        void LoadBookings()
        {
            SqlConnection con = new SqlConnection(strcon);

            string query = @"
        SELECT 
            b.BookingId,
            p.Title,
            p.Location,
            p.Price,
            b.BookingDate,
            b.Status,
            agent.Email AS AgentEmail,
            agent.Phone AS AgentPhone
        FROM Bookings b
        INNER JOIN Properties p  ON b.PropertyId = p.PropertyId
        INNER JOIN Users agent   ON p.AgentId    = agent.UserId
        WHERE b.UserId = @uid
        ORDER BY b.BookingId DESC";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@uid", GetUserId());
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Close();

            if (dt.Rows.Count == 0)
            {
                pnlNoBookings.Visible = true;
                rptBookings.Visible = false;
            }
            else
            {
                pnlNoBookings.Visible = false;
                rptBookings.Visible = true;
                rptBookings.DataSource = dt;
                rptBookings.DataBind();
            }
        }

        protected void rptBookings_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "CancelBooking")
            {
                int bookingId = Convert.ToInt32(e.CommandArgument);
                SqlConnection con = new SqlConnection(strcon);
                string query = "UPDATE Bookings SET Status='Cancelled' WHERE BookingId=@bid AND UserId=@uid";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@bid", bookingId);
                cmd.Parameters.AddWithValue("@uid", GetUserId());
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                lblMsg.Text = "Booking cancelled successfully.";
                lblMsg.Visible = true;
                LoadBookings();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear(); Session.Abandon();
            Response.Redirect("/Login.aspx");
        }
    }
}