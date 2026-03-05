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
    public partial class AddCity : System.Web.UI.Page
    {

        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null || Session["role"]?.ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadStates();
            }
        }


        void LoadStates()
        {
            SqlConnection con = new SqlConnection(strcon);

            string query = "SELECT StateId, StateName FROM States";
            SqlCommand cmd = new SqlCommand(query, con);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            ddlState.DataSource = dr;
            ddlState.DataTextField = "StateName";   // display text
            ddlState.DataValueField = "StateId";    // value stored
            ddlState.DataBind();

            con.Close();

            ddlState.Items.Insert(0,
                new System.Web.UI.WebControls.ListItem("-- Select State --", ""));
        }


        protected void btnAddCity_Click(object sender, EventArgs e)
        {
            if (ddlState.SelectedValue == "" || txtCityName.Text.Trim() == "")
            {
                lblMsg.Text = "All fields are required";
                return;
            }

            SqlConnection con = new SqlConnection(strcon);
            string query = "INSERT INTO Cities (CityName, StateId) VALUES (@city, @state)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@city", txtCityName.Text.Trim());
            cmd.Parameters.AddWithValue("@state", ddlState.SelectedValue);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            Response.Redirect("/Admin/ViewList.aspx?type=cities");
        }
    }
}