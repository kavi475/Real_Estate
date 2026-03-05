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
    public partial class Dashboard : System.Web.UI.Page
    {

        String strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string type = Request.QueryString["type"];
                ViewState["type"] = type;
                switch (type)
                {
                    case "properties":
                        LoadProperties();
                        break;

                    case "states":
                        LoadStates();
                        break;

                    case "cities":
                        LoadCities();
                        break;

                    case "users":
                        LoadUsers();
                        break;

                    case "agents":
                        LoadAgents();
                        break;

                    case "pending":
                        LoadPendingProperties();
                        break;
                }
            }
        }


        void LoadProperties()
        {
            SqlConnection con = new SqlConnection(strcon);
            String query = "SELECT * FROM Properties";

            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            gv_viewList.DataKeyNames = new string[] { "PropertyId" };
            gv_viewList.DataSource = ds;    
            gv_viewList.DataBind(); 
            con.Close();
            
        }

        void LoadStates()
        {
            SqlConnection con = new SqlConnection(strcon);
            String query = "SELECT * FROM States";

            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            gv_viewList.DataKeyNames = new string[] { "StateId" };
            gv_viewList.DataSource = ds;
            gv_viewList.DataBind();
            con.Close();
        }

        void LoadCities()
        {
            SqlConnection con = new SqlConnection(strcon);
            String query = "SELECT * FROM Cities";

            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            gv_viewList.DataKeyNames = new string[] { "CityId" };
            gv_viewList.DataSource = ds;
            gv_viewList.DataBind();
            con.Close();
        }

        void LoadUsers()
        {
            SqlConnection con = new SqlConnection(strcon);
            String query = "SELECT * FROM Users";

            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            gv_viewList.DataKeyNames = new string[] { "UserId" };
            gv_viewList.DataSource = ds;
            gv_viewList.DataBind();
            con.Close();
        }

        void LoadAgents()
        {
            SqlConnection con = new SqlConnection(strcon);

            string query = "SELECT UserId, Email, Role FROM Users WHERE Role = 'Agent'";

            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            gv_viewList.DataKeyNames = new string[] { "UserId" };
            gv_viewList.DataSource = ds;
            gv_viewList.DataBind();

            con.Close();
        }

        void LoadPendingProperties()
        {

        }


        protected void gv_viewList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gv_viewList.EditIndex = e.NewEditIndex;
            ReloadData();
        }

        protected void gv_viewList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gv_viewList.EditIndex = -1;
            ReloadData();
        }
        protected void gv_viewList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string type = ViewState["type"].ToString();
            int id = Convert.ToInt32(gv_viewList.DataKeys[e.RowIndex].Value);

            string query = "";
            string value = "";

            if (type == "states")
            {
                value = e.NewValues["StateName"].ToString();
                query = "UPDATE States SET StateName=@val WHERE StateId=@id";
            }
            else if (type == "cities")
            {
                value = e.NewValues["CityName"].ToString();
                query = "UPDATE Cities SET CityName=@val WHERE CityId=@id";
            }
            else if (type == "properties")
            {
                value = e.NewValues["Title"].ToString();
                query = "UPDATE Properties SET Title=@val WHERE PropertyId=@id";
            }
            else if (type == "users")
            {
                value = e.NewValues["Email"].ToString();
                query = "UPDATE Users SET Email=@val WHERE UserId=@id";
            }

            if (string.IsNullOrEmpty(query))
            {
                gv_viewList.EditIndex = -1;
                ReloadData();
                return;
            }

            using (SqlConnection con = new SqlConnection(strcon))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@val", value);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gv_viewList.EditIndex = -1;
            ReloadData();
        }


        protected void gv_viewList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gv_viewList.DataKeys[e.RowIndex].Value);
            string type = ViewState["type"].ToString();

            string query = "";

            if (type == "states")
                query = "DELETE FROM States WHERE StateId=@id";
            else if (type == "cities")
                query = "DELETE FROM Cities WHERE CityId=@id";
            else if (type == "properties")
                query = "DELETE FROM Properties WHERE PropertyId=@id";
            else if (type == "users")
                query = "DELETE FROM Users WHERE UserId=@id";

            if (query == "") return;


            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            ReloadData();
        }

        void ReloadData()
        {
            string type = ViewState["type"].ToString();

            if (type == "properties") LoadProperties();
            else if (type == "states") LoadStates();
            else if (type == "cities") LoadCities();
            else if (type == "users") LoadUsers();
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;
            string type = ViewState["type"].ToString();

            string query = "";

            if (type == "cities")
                query = "SELECT * FROM Cities WHERE CityName LIKE @search";
            else if (type == "states")
                query = "SELECT * FROM States WHERE StateName LIKE @search";
            else if (type == "properties")
                query = "SELECT * FROM Properties WHERE Title LIKE @search";
            else if (type == "users")
                query = "SELECT * FROM Users WHERE Email LIKE @search";

            if (query == "") return;


            SqlConnection con = new SqlConnection(strcon);
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            sda.SelectCommand.Parameters.AddWithValue("@search", "%" + keyword + "%");

            DataTable dt = new DataTable();
            sda.Fill(dt);

            gv_viewList.DataSource = dt;
            gv_viewList.DataBind();
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string type = ViewState["type"].ToString();

            if (type == "cities")
                Response.Redirect("/Admin/AddCity.aspx");
            else if (type == "states")
                Response.Redirect("/Admin/AddState.aspx");
            else if (type == "properties")
                Response.Redirect("/Admin/AddProperty.aspx");
        }

    }
}
