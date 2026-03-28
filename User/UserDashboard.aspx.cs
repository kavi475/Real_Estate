using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WebApplication1.User
{
    public partial class UserDashboard : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblWelcome.Text = "Welcome, " + Session["email"].ToString();
                LoadCities();
                LoadProperties("", "", "");
                LoadStats();
            }
        }

        void LoadCities()
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = "SELECT CityId, CityName FROM Cities ORDER BY CityName";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            ddlCity.DataSource = dr;
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityName";
            ddlCity.DataBind();
            con.Close();
            ddlCity.Items.Insert(0, new ListItem("All Cities", ""));
        }

        void LoadProperties(string city, string type, string status)
        {
            SqlConnection con = new SqlConnection(strcon);

            string query = @"
                SELECT 
                    p.PropertyId, p.Title, p.Location, p.Price, p.Status,
                    (SELECT TOP 1 ImagePath FROM PropertyImages 
                     WHERE PropertyId = p.PropertyId) AS ImagePath
                FROM Properties p
                WHERE p.IsApproved = 'Approved'
                AND (@city = '' OR p.Location LIKE '%' + @city + '%')
                AND (@status = '' OR p.Status = @status)
                ORDER BY p.PropertyId DESC";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@city", city);
            cmd.Parameters.AddWithValue("@status", status);

            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Close();

            rptProperties.DataSource = dt;
            rptProperties.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string city = ddlCity.SelectedValue;
            string type = ddlType.SelectedValue;
            string status = ddlStatus.SelectedValue;
            LoadProperties(city, type, status);
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("/Home.aspx");
        }

        void LoadStats()
        {
            int userId = GetUserId();
            SqlConnection con = new SqlConnection(strcon);
            string query = @"
        SELECT 
            COUNT(*) AS Total,
            SUM(CASE WHEN Status='Approved' THEN 1 ELSE 0 END) AS Approved,
            SUM(CASE WHEN Status='Pending'  THEN 1 ELSE 0 END) AS Pending,
            SUM(CASE WHEN Status='Rejected' THEN 1 ELSE 0 END) AS Rejected
        FROM Bookings WHERE UserId = @uid";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@uid", userId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                lblTotal.Text = dr["Total"].ToString();
                lblApproved.Text = dr["Approved"].ToString();
                lblPending.Text = dr["Pending"].ToString();
                lblRejected.Text = dr["Rejected"].ToString();
            }
            con.Close();
        }

        int GetUserId()
        {
            string email = Session["email"].ToString();
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("SELECT UserId FROM Users WHERE Email=@email", con);
            cmd.Parameters.AddWithValue("@email", email);
            con.Open();
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return id;
        }
    }
}