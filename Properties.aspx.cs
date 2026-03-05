using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication1
{
    public partial class Properties : System.Web.UI.Page
    {

        string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStates();
                LoadCities();
                LoadLatestProperties();
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

        void LoadLatestProperties()
        {
            SqlConnection con = new SqlConnection(strcon);

            SqlCommand cmd = new SqlCommand(
            "SELECT TOP 9 P.PropertyId,P.Title,P.Location,P.Price,PI.ImagePath FROM Properties P LEFT JOIN PropertyImages PI ON P.PropertyId = PI.PropertyId ORDER BY P.PropertyId DESC",
            con);

            con.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);

            rptProperties.DataSource = dt;
            rptProperties.DataBind();

            con.Close();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(strcon);

            string query = @"SELECT 
                     P.PropertyId,
                     P.Title,
                     P.Location,
                     P.Price,
                     PI.ImagePath
                     FROM Properties P
                     LEFT JOIN PropertyImages PI
                     ON P.PropertyId = PI.PropertyId
                     WHERE 1=1";

            if (txtKeyword.Text != "")
            {
                query += " AND P.Title LIKE '%" + txtKeyword.Text + "%'";
            }

            SqlCommand cmd = new SqlCommand(query, con);

            con.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            rptProperties.DataSource = dt;
            rptProperties.DataBind();

            con.Close();
        }

    }
}