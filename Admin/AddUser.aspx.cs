using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1.Admin
{
    public partial class AddUser : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null || Session["role"]?.ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text.Trim() == "" || txtPassword.Text.Trim() == "")
            {
                lblMsg.Text = "All fields are required";
                return;
            }

            SqlConnection con = new SqlConnection(strcon);
            string query = "INSERT INTO Users (Email, Password, Role) VALUES (@email, @password, @role)";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());
            cmd.Parameters.AddWithValue("@role", "User");
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            Response.Redirect("/Admin/ViewList.aspx?type=users");
        }
    }
}