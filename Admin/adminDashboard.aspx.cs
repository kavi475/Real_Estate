using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace WebApplication1
{
    public partial class adminDashboard : System.Web.UI.Page
    {
        String strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null || Session["role"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (Session["role"].ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                Total_Property();
                Total_State();
                Total_city();
                Total_Users();
                Total_Agents();
            }
        }

        public void Total_Property()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Properties", con);
            con.Open();
            int totalProperty = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            lblProperty.Text = totalProperty.ToString();
        }

        public void Total_State()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM States", con);
            con.Open();
            int totalState = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            lblState.Text = totalState.ToString();
        }

        public void Total_city()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM CITIES", con);
            con.Open();
            int totalcity = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            lblCity.Text = totalcity.ToString();    
        }

        public void Total_Users()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users", con);
            con.Open();
            int totalUsers = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            lblUsers.Text = totalUsers.ToString();
        }

        public void Total_Agents()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Role = 'Agent'", con);
            con.Open();
            int totalUsers = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            lblAgents.Text = totalUsers.ToString();
        }



    }
}