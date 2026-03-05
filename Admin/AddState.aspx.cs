using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace WebApplication1.Admin
{
    public partial class AddState : System.Web.UI.Page
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

        protected void btnAddState_Click(object sender, EventArgs e)
        {
            if (txtStateName.Text.Trim() == "")
            {
                lblMsg.Text = "State name is required";
                return;
            }

            SqlConnection con = new SqlConnection(strcon);

            string query = "INSERT INTO States (StateName) VALUES (@name)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@name", txtStateName.Text.Trim());

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


            Response.Redirect("/Admin/ViewList.aspx?type=states");
        }
    }
}