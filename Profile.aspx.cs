using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1.User
{
    public partial class Profile : System.Web.UI.Page
    {
        String strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadProfile();

            }
        }

        void LoadProfile()
        {
            SqlConnection con = new SqlConnection(strcon);

            string query = "SELECT Email, Role FROM Users WHERE Email=@email";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", Session["email"].ToString());

            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                txtEmail.Text = dr["Email"].ToString();
                txtRole.Text = dr["Role"].ToString();
            }

            con.Close();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserDashboard.aspx");
        }
    }
}