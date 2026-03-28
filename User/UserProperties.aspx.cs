using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1.User
{
    public partial class UserProperties : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            lblWelcome.Text = "Hi, " + Session["email"].ToString();
            if (!IsPostBack)
            {
               
                LoadStates();
                LoadCities();
                LoadProperties();
            }
        }

        void LoadStates()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("SELECT StateName FROM States", con);
            con.Open();
            ddlState.DataSource = cmd.ExecuteReader();
            ddlState.DataTextField = "StateName";
            ddlState.DataValueField = "StateName";
            ddlState.DataBind();
            ddlState.Items.Insert(0, "All States");
            con.Close();
        }

        void LoadCities()
        {
            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand("SELECT CityName FROM Cities", con);
            con.Open();
            ddlCity.DataSource = cmd.ExecuteReader();
            ddlCity.DataTextField = "CityName";
            ddlCity.DataValueField = "CityName";
            ddlCity.DataBind();
            ddlCity.Items.Insert(0, "All Cities");
            con.Close();
        }

        void LoadProperties()
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = @"SELECT P.PropertyId, P.Title, P.Location, P.Price,
                            (SELECT TOP 1 ImagePath FROM PropertyImages 
                             WHERE PropertyId = P.PropertyId) AS ImagePath
                            FROM Properties P
                            WHERE P.IsApproved = 'Approved'
                            ORDER BY P.PropertyId DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            rptProperties.DataSource = dt;
            rptProperties.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(strcon);
            string query = @"
        SELECT P.PropertyId, P.Title, P.Location, P.Price, P.Status,
               (SELECT TOP 1 ImagePath FROM PropertyImages WHERE PropertyId = P.PropertyId) AS ImagePath
        FROM Properties P
        WHERE P.IsApproved = 'Approved'
        AND (@keyword = '' OR P.Title LIKE '%' + @keyword + '%' OR P.Location LIKE '%' + @keyword + '%')
        AND (@state = 'All States' OR P.Location LIKE '%' + @state + '%')
        AND (@city = 'All Cities' OR P.Location LIKE '%' + @city + '%')
        AND (@status = '' OR P.Status = @status)
        AND (@minPrice = 0 OR P.Price >= @minPrice)
        AND (@maxPrice = 0 OR P.Price <= @maxPrice)
        ORDER BY P.PropertyId DESC";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@keyword", txtKeyword.Text.Trim());
            cmd.Parameters.AddWithValue("@state", ddlState.SelectedValue);
            cmd.Parameters.AddWithValue("@city", ddlCity.SelectedValue);
            cmd.Parameters.AddWithValue("@status", ddlStatus.SelectedValue);
            cmd.Parameters.AddWithValue("@minPrice", txtMinPrice.Text.Trim() == "" ? 0 : Convert.ToDecimal(txtMinPrice.Text.Trim()));
            cmd.Parameters.AddWithValue("@maxPrice", txtMaxPrice.Text.Trim() == "" ? 0 : Convert.ToDecimal(txtMaxPrice.Text.Trim()));

            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            rptProperties.DataSource = dt;
            rptProperties.DataBind();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("/Home.aspx");
        }
    }
}