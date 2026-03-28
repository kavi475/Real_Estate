using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.User
{
    public partial class PropertyDetail : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;
        int propertyId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            if (Request.QueryString["id"] == null)
            {
                Response.Redirect("/User/UserDashboard.aspx");
                return;
            }

            propertyId = Convert.ToInt32(Request.QueryString["id"]);

            if (!IsPostBack)
            {
                lblWelcome.Text = "Hi, " + Session["email"].ToString();
                LoadPropertyDetail();
                LoadImages();
                CheckAlreadyBooked();
            }
        }

        void LoadPropertyDetail()
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT Title, Location, Price, Status, IsApproved FROM Properties WHERE PropertyId = @pid";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@pid", propertyId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                lblTitle.Text = dr["Title"].ToString();
                lblLocation.Text = dr["Location"].ToString();
                lblPrice.Text = dr["Price"].ToString();
                lblStatus.Text = dr["Status"].ToString();
                lblApproval.Text = dr["IsApproved"].ToString();

                string status = dr["Status"].ToString();
                lblStatusBadge.Text = status;
                if (status == "Available")
                    lblStatusBadge.CssClass = "badge-status badge-available";
                else if (status == "Rent")
                    lblStatusBadge.CssClass = "badge-status badge-rent";
                else
                    lblStatusBadge.CssClass = "badge-status badge-sold";
            }
            con.Close();
        }

        void LoadImages()
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT ImagePath FROM PropertyImages WHERE PropertyId = @pid";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@pid", propertyId);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                // Set first image as main image
                imgMain.ImageUrl = "/" + dt.Rows[0]["ImagePath"].ToString();
            }

            rptThumbs.DataSource = dt;
            rptThumbs.DataBind();
        }

        void CheckAlreadyBooked()
        {
            SqlConnection con = new SqlConnection(strcon);

            // Check if THIS user already booked it
            string myBookingQuery = @"
        SELECT COUNT(*) FROM Bookings 
        WHERE PropertyId = @pid 
        AND UserId = @uid
        AND Status != 'Cancelled'";

            SqlCommand myCmd = new SqlCommand(myBookingQuery, con);
            myCmd.Parameters.AddWithValue("@pid", propertyId);
            myCmd.Parameters.AddWithValue("@uid", GetUserId());
            con.Open();
            int myCount = Convert.ToInt32(myCmd.ExecuteScalar());
            con.Close();

            if (myCount > 0)
            {
                btnBook.Enabled = false;
                btnBook.Text = "Already Booked";
                lblMsg.Visible = true;
                lblMsg.CssClass = "msg-box msg-info";
                lblMsg.Text = "You have already submitted a booking request for this property.";
                return;
            }

            // Check if ANY other user has booked this property
            SqlConnection con2 = new SqlConnection(strcon);
            string anyBookingQuery = @"
        SELECT COUNT(*) FROM Bookings 
        WHERE PropertyId = @pid
        AND Status != 'Cancelled'";

            SqlCommand anyCmd = new SqlCommand(anyBookingQuery, con2);
            anyCmd.Parameters.AddWithValue("@pid", propertyId);
            con2.Open();
            int anyCount = Convert.ToInt32(anyCmd.ExecuteScalar());
            con2.Close();

            if (anyCount > 0)
            {
                btnBook.Enabled = false;
                btnBook.Text = "Not Available";
                lblMsg.Visible = true;
                lblMsg.CssClass = "msg-box msg-error";
                lblMsg.Text = "Sorry, this property has already been booked by someone else.";
            }
        }

        int GetUserId()
        {
            string email = Session["email"].ToString();
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT UserId FROM Users WHERE Email = @email";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", email);
            con.Open();
            int userId = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return userId;
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            // Final safety check before inserting
            SqlConnection conCheck = new SqlConnection(strcon);
            string checkQuery = @"
        SELECT COUNT(*) FROM Bookings 
        WHERE PropertyId = @pid
        AND Status != 'Cancelled'";
            SqlCommand checkCmd = new SqlCommand(checkQuery, conCheck);
            checkCmd.Parameters.AddWithValue("@pid", propertyId);
            conCheck.Open();
            int existing = Convert.ToInt32(checkCmd.ExecuteScalar());
            conCheck.Close();

            if (existing > 0)
            {
                btnBook.Enabled = false;
                btnBook.Text = "Not Available";
                lblMsg.Visible = true;
                lblMsg.CssClass = "msg-box msg-error";
                lblMsg.Text = "Sorry, this property was just booked by someone else.";
                return;
            }

            // Safe to book now
            int userId = GetUserId();
            SqlConnection con = new SqlConnection(strcon);
            string query = @"INSERT INTO Bookings (PropertyId, UserId, BookingDate, Status)
                     VALUES (@pid, @uid, @date, 'Pending')";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@pid", propertyId);
            cmd.Parameters.AddWithValue("@uid", userId);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            btnBook.Enabled = false;
            btnBook.Text = "Booking Requested";
            lblMsg.Visible = true;
            lblMsg.CssClass = "msg-box msg-success";
            lblMsg.Text = "Your booking request has been submitted successfully! Agent will contact you soon.";
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("/Login.aspx");
        }
    }
}