using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Agent
{
    public partial class Bookings : System.Web.UI.Page
    {
        private readonly string strcon = ConfigurationManager.ConnectionStrings["RealEstateDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Security check
            if (Session["email"] == null || Session["role"]?.ToString() != "Agent")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Handle Export API request via query parameter
            if (Request.QueryString["action"] == "export")
            {
                string exportStatus = Request.QueryString["status"] ?? "ALL";
                string exportSearch = Request.QueryString["search"] ?? "";
                DataTable dt = GetFilteredData(exportStatus, exportSearch);
                ExportToExcel(dt, "Bookings_Export.xlsx");
                return;
            }

            if (!IsPostBack)
            {
                // Retrieve filter and search from query parameter if present, otherwise default
                string status = Request.QueryString["status"] ?? "ALL";
                string search = Request.QueryString["search"] ?? "";

                status = status.ToUpper();

                // Select correct filter dropdown item
                ListItem item = ddlStatusFilter.Items.FindByValue(status);
                if (item != null)
                {
                    ddlStatusFilter.ClearSelection();
                    item.Selected = true;
                }
                else
                {
                    ddlStatusFilter.SelectedValue = "ALL";
                    status = "ALL";
                }

                txtSearch.Text = search;

                BindGrid();
                GetStatusCounts();
            }
        }

        private int GetAgentId()
        {
            if (Session["AgentId"] != null)
            {
                return Convert.ToInt32(Session["AgentId"]);
            }

            string email = Session["email"].ToString();
            using (SqlConnection con = new SqlConnection(strcon))
            {
                string query = "SELECT UserId FROM Users WHERE Email = @email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        int agentId = Convert.ToInt32(result);
                        Session["AgentId"] = agentId;
                        return agentId;
                    }
                }
            }

            Response.Redirect("~/Login.aspx");
            return 0;
        }

        // Base filtering and search query logic reused for GridView and Export
        private DataTable GetFilteredData(string status, string searchText)
        {
            int agentId = GetAgentId();

            string dbStatus = "ALL";
            if (status == "PENDING") dbStatus = "Pending";
            else if (status == "APPROVED") dbStatus = "Approved";
            else if (status == "REJECTED") dbStatus = "Rejected";

            string query = @"
                SELECT 
                    b.BookingId,
                    p.Title,
                    u.Email AS UserEmail,
                    b.BookingDate,
                    b.Status
                FROM Bookings b
                INNER JOIN Properties p ON b.PropertyId = p.PropertyId
                INNER JOIN Users u ON b.UserId = u.UserId
                WHERE p.AgentId = @agentId";

            if (dbStatus != "ALL")
            {
                query += " AND b.Status = @status";
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                query += " AND (CAST(b.BookingId AS VARCHAR) LIKE @search OR u.Email LIKE @search)";
            }

            query += " ORDER BY b.BookingId DESC";

            using (SqlConnection con = new SqlConnection(strcon))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@agentId", agentId);
                    if (dbStatus != "ALL")
                    {
                        cmd.Parameters.AddWithValue("@status", dbStatus);
                    }
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    }

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        private void BindGrid()
        {
            string status = ddlStatusFilter.SelectedValue;
            string search = txtSearch.Text.Trim();
            DataTable dt = GetFilteredData(status, search);
            gvBookings.DataSource = dt;
            gvBookings.DataBind();
        }

        private void GetStatusCounts()
        {
            int agentId = GetAgentId();

            string query = @"
                SELECT 
                    SUM(CASE WHEN b.Status = 'Pending' THEN 1 ELSE 0 END) AS PendingCount,
                    SUM(CASE WHEN b.Status = 'Approved' THEN 1 ELSE 0 END) AS ApprovedCount,
                    SUM(CASE WHEN b.Status = 'Rejected' THEN 1 ELSE 0 END) AS RejectedCount
                FROM Bookings b
                INNER JOIN Properties p ON b.PropertyId = p.PropertyId
                WHERE p.AgentId = @agentId";

            using (SqlConnection con = new SqlConnection(strcon))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@agentId", agentId);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblCountPending.Text = reader["PendingCount"] != DBNull.Value ? reader["PendingCount"].ToString() : "0";
                            lblCountApproved.Text = reader["ApprovedCount"] != DBNull.Value ? reader["ApprovedCount"].ToString() : "0";
                            lblCountRejected.Text = reader["RejectedCount"] != DBNull.Value ? reader["RejectedCount"].ToString() : "0";
                        }
                        else
                        {
                            lblCountPending.Text = "0";
                            lblCountApproved.Text = "0";
                            lblCountRejected.Text = "0";
                        }
                    }
                }
            }
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvBookings.PageIndex = 0;
            BindGrid();
            GetStatusCounts();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvBookings.PageIndex = 0;
            BindGrid();
            GetStatusCounts();
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            gvBookings.PageIndex = 0;
            BindGrid();
            GetStatusCounts();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string selectedStatus = ddlStatusFilter.SelectedValue;
            string searchText = txtSearch.Text.Trim();
            Response.Redirect("Bookings.aspx?action=export&status=" + selectedStatus + "&search=" + HttpUtility.UrlEncode(searchText));
        }

        protected void gvBookings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBookings.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ApproveBooking" || e.CommandName == "RejectBooking")
            {
                int bookingId = Convert.ToInt32(e.CommandArgument);
                string newStatus = e.CommandName == "ApproveBooking" ? "Approved" : "Rejected";

                using (SqlConnection con = new SqlConnection(strcon))
                {
                    string query = "UPDATE Bookings SET Status = @status WHERE BookingId = @bid AND Status = 'Pending'";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@status", newStatus);
                        cmd.Parameters.AddWithValue("@bid", bookingId);
                        con.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            lblMsg.Text = "Booking " + newStatus + " successfully.";
                            lblMsg.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            lblMsg.Text = "Error: Booking action failed or was not pending.";
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }

                BindGrid();
                GetStatusCounts();
            }
        }

        protected void btnBulkApprove_Click(object sender, EventArgs e)
        {
            ProcessBulkAction("Approved");
        }

        protected void btnBulkReject_Click(object sender, EventArgs e)
        {
            ProcessBulkAction("Rejected");
        }

        private void ProcessBulkAction(string newStatus)
        {
            int successCount = 0;
            int failCount = 0;

            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        foreach (GridViewRow row in gvBookings.Rows)
                        {
                            CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                            if (chk != null && chk.Checked)
                            {
                                HiddenField hdnId = (HiddenField)row.FindControl("hdnBookingId");
                                if (hdnId != null)
                                {
                                    int bookingId = Convert.ToInt32(hdnId.Value);

                                    // Enforce Pending status check in database before updating
                                    string currentStatus = GetCurrentBookingStatus(bookingId, con, transaction);
                                    if (currentStatus == "Pending")
                                    {
                                        string query = "UPDATE Bookings SET Status = @status WHERE BookingId = @bid AND Status = 'Pending'";
                                        using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                                        {
                                            cmd.Parameters.AddWithValue("@status", newStatus);
                                            cmd.Parameters.AddWithValue("@bid", bookingId);
                                            cmd.ExecuteNonQuery();
                                        }
                                        successCount++;
                                    }
                                    else
                                    {
                                        failCount++;
                                    }
                                }
                            }
                        }
                        transaction.Commit();
                        lblMsg.Text = $"Bulk action '{newStatus}' completed successfully. Updated: {successCount}. Skipped: {failCount} (non-pending).";
                        lblMsg.ForeColor = System.Drawing.Color.Green;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        lblMsg.Text = "Error executing bulk action: " + ex.Message;
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }

            BindGrid();
            GetStatusCounts();
        }

        private string GetCurrentBookingStatus(int bookingId, SqlConnection con, SqlTransaction transaction)
        {
            string query = "SELECT Status FROM Bookings WHERE BookingId = @id";
            using (SqlCommand cmd = new SqlCommand(query, con, transaction))
            {
                cmd.Parameters.AddWithValue("@id", bookingId);
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "";
            }
        }

        // Generates valid Excel (.xlsx) Open XML file format
        private void ExportToExcel(DataTable dt, string filename)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    // 1. Write [Content_Types].xml
                    ZipArchiveEntry contentTypesEntry = archive.CreateEntry("[Content_Types].xml");
                    using (StreamWriter writer = new StreamWriter(contentTypesEntry.Open(), Encoding.UTF8))
                    {
                        writer.Write(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Types xmlns=""http://schemas.openxmlformats.org/package/2006/content-types"">
  <Default Extension=""rels"" ContentType=""application/vnd.openxmlformats-package.relationships+xml""/>
  <Default Extension=""xml"" ContentType=""application/xml""/>
  <Override PartName=""/xl/workbook.xml"" ContentType=""application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml""/>
  <Override PartName=""/xl/worksheets/sheet1.xml"" ContentType=""application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml""/>
</Types>");
                    }

                    // 2. Write _rels/.rels
                    ZipArchiveEntry relsEntry = archive.CreateEntry("_rels/.rels");
                    using (StreamWriter writer = new StreamWriter(relsEntry.Open(), Encoding.UTF8))
                    {
                        writer.Write(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Relationships xmlns=""http://schemas.openxmlformats.org/package/2006/relationships"">
  <Relationship Id=""rId1"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"" Target=""xl/workbook.xml""/>
</Relationships>");
                    }

                    // 3. Write xl/workbook.xml
                    ZipArchiveEntry workbookEntry = archive.CreateEntry("xl/workbook.xml");
                    using (StreamWriter writer = new StreamWriter(workbookEntry.Open(), Encoding.UTF8))
                    {
                        writer.Write(@"<?xml version=""1.0"" encoding=""utf-8""?>
<workbook xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships"">
  <sheets>
    <sheet name=""Bookings"" sheetId=""1"" r:id=""rId1""/>
  </sheets>
</workbook>");
                    }

                    // 4. Write xl/_rels/workbook.xml.rels
                    ZipArchiveEntry workbookRelsEntry = archive.CreateEntry("xl/_rels/workbook.xml.rels");
                    using (StreamWriter writer = new StreamWriter(workbookRelsEntry.Open(), Encoding.UTF8))
                    {
                        writer.Write(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Relationships xmlns=""http://schemas.openxmlformats.org/package/2006/relationships"">
  <Relationship Id=""rId1"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet"" Target=""worksheets/sheet1.xml""/>
</Relationships>");
                    }

                    // 5. Write xl/worksheets/sheet1.xml
                    ZipArchiveEntry sheetEntry = archive.CreateEntry("xl/worksheets/sheet1.xml");
                    using (StreamWriter writer = new StreamWriter(sheetEntry.Open(), Encoding.UTF8))
                    {
                        writer.Write(@"<?xml version=""1.0"" encoding=""utf-8""?>
<worksheet xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"">
  <sheetData>");

                        // Header row
                        writer.Write(@"<row r=""1"">");
                        writer.Write(@"<c r=""A1"" t=""inlineStr""><is><t>Property</t></is></c>");
                        writer.Write(@"<c r=""B1"" t=""inlineStr""><is><t>User</t></is></c>");
                        writer.Write(@"<c r=""C1"" t=""inlineStr""><is><t>Date</t></is></c>");
                        writer.Write(@"<c r=""D1"" t=""inlineStr""><is><t>Status</t></is></c>");
                        writer.Write(@"</row>");

                        // Data rows
                        int rowIndex = 2;
                        foreach (DataRow row in dt.Rows)
                        {
                            writer.Write(string.Format(@"<row r=""{0}"">", rowIndex));

                            string title = EscapeXml(row["Title"]?.ToString());
                            string email = EscapeXml(row["UserEmail"]?.ToString());

                            string dateStr = "";
                            if (row["BookingDate"] != DBNull.Value)
                                dateStr = Convert.ToDateTime(row["BookingDate"]).ToString("dd-MMM-yyyy");
                            dateStr = EscapeXml(dateStr);

                            string statusVal = EscapeXml(row["Status"]?.ToString());

                            writer.Write(string.Format(@"<c r=""A{0}"" t=""inlineStr""><is><t>{1}</t></is></c>", rowIndex, title));
                            writer.Write(string.Format(@"<c r=""B{0}"" t=""inlineStr""><is><t>{1}</t></is></c>", rowIndex, email));
                            writer.Write(string.Format(@"<c r=""C{0}"" t=""inlineStr""><is><t>{1}</t></is></c>", rowIndex, dateStr));
                            writer.Write(string.Format(@"<c r=""D{0}"" t=""inlineStr""><is><t>{1}</t></is></c>", rowIndex, statusVal));

                            writer.Write(@"</row>");
                            rowIndex++;
                        }

                        writer.Write(@"</sheetData>
</worksheet>");
                    }
                }

                Response.BinaryWrite(memoryStream.ToArray());
            }
            Response.End();
        }

        private string EscapeXml(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return value.Replace("&", "&amp;")
                        .Replace("<", "&lt;")
                        .Replace(">", "&gt;")
                        .Replace("\"", "&quot;")
                        .Replace("'", "&apos;");
        }
    }
}