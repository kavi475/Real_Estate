using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RealEstate.Admin
{
    public partial class FinancialManagement : System.Web.UI.Page
    {
        private readonly string _connStr =
            ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        // ================= PAGE LOAD =================
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["role"] == null || Session["role"].ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadDashboardKPIs();
                LoadAgentCommissionReport();
                LoadCancelledBookings();
            }
        }

        // ================= KPI =================
        private void LoadDashboardKPIs()
        {
            string sql = @"
                SELECT
                    COUNT(b.BookingId) AS TotalBookings,
                    ISNULL(SUM(CASE WHEN b.Status='Approved' THEN p.Price ELSE 0 END),0) AS TotalRevenue
                FROM Bookings b
                INNER JOIN Properties p ON b.PropertyId = p.PropertyId";

            using (SqlConnection con = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblTotalBookings.Text = dr["TotalBookings"].ToString();
                    lblTotalRevenue.Text = Convert.ToDecimal(dr["TotalRevenue"]).ToString("N2");
                    lblMonthlyRevenue.Text = "0.00";
                }
            }
        }

        // ================= AGENT COMMISSION =================
        private void LoadAgentCommissionReport()
        {
            string sql = @"
                SELECT
                    u.Email AS AgentName,
                    u.Email AS Email,
                    ISNULL(SUM(CASE WHEN b.Status='Approved' THEN p.Price ELSE 0 END),0) AS TotalRevenue,
                    ISNULL(SUM(CASE WHEN b.Status='Approved' THEN p.Price ELSE 0 END),0) * 0.10 AS CommissionAmount
                FROM Users u
                INNER JOIN Properties p ON p.AgentId = u.UserId
                LEFT JOIN Bookings b ON b.PropertyId = p.PropertyId
                GROUP BY u.Email
                ORDER BY TotalRevenue DESC";

            using (SqlConnection con = new SqlConnection(_connStr))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvAgentCommission.DataSource = dt;
                gvAgentCommission.DataBind();
            }
        }

        protected void gvAgentCommission_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAgentCommission.PageIndex = e.NewPageIndex;
            LoadAgentCommissionReport();
        }

        // ================= CANCELLED BOOKINGS =================
        private void LoadCancelledBookings()
        {
            string sql = @"
                SELECT
                    b.BookingId,
                    p.Title AS PropertyTitle,
                    u.Email AS UserEmail,
                    p.Price AS RefundAmount,
                    b.Status
                FROM Bookings b
                INNER JOIN Properties p ON b.PropertyId = p.PropertyId
                INNER JOIN Users u ON b.UserId = u.UserId
                WHERE b.Status IN ('Cancelled','Refunded')
                ORDER BY b.BookingId DESC";

            using (SqlConnection con = new SqlConnection(_connStr))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvCancelledBookings.DataSource = dt;
                gvCancelledBookings.DataBind();
            }
        }

        protected void gvCancelledBookings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCancelledBookings.PageIndex = e.NewPageIndex;
            LoadCancelledBookings();
        }

        // ================= EXPORT DATA =================
        private DataTable GetExportData()
        {
            string sql = @"
                SELECT
                    u.Email AS AgentName,
                    u.Email AS Email,
                    ISNULL(SUM(CASE WHEN b.Status='Approved' THEN p.Price ELSE 0 END),0) AS TotalRevenue,
                    ISNULL(SUM(CASE WHEN b.Status='Approved' THEN p.Price ELSE 0 END),0) * 0.10 AS CommissionAmount
                FROM Users u
                INNER JOIN Properties p ON p.AgentId = u.UserId
                LEFT JOIN Bookings b ON b.PropertyId = p.PropertyId
                GROUP BY u.Email";

            using (SqlConnection con = new SqlConnection(_connStr))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // ================= EXPORT EXCEL =================
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = GetExportData();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=FinancialReport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();

            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                gv.RenderControl(hw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            DataTable dt = GetExportData();

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 20f);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Financial_Report.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            PdfPTable table = new PdfPTable(dt.Columns.Count);
            table.WidthPercentage = 100;

            // Header
            foreach (DataColumn column in dt.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cell);
            }

            // Rows
            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    table.AddCell(item.ToString());
                }
            }

            pdfDoc.Add(table);
            pdfDoc.Close();

            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            // Required for export
        }
    }
}