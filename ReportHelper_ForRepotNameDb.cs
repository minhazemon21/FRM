var sPService = new SPService();
string query = "SELECT ReportName FROM ExcludeReport";  // Replace with your actual table and column names
DataSet excludedReportsDataSet = sPService.GetDataBySqlCommand(query);
DataTable excludedReportsTable = excludedReportsDataSet.Tables[0];

// Convert the DataTable to a list of report names
List<string> excludedReports = new List<string>();
foreach (DataRow row in excludedReportsTable.Rows)
{
    excludedReports.Add(row["ReportName"].ToString());
}