using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Plant4D
{
    public class Project
    {
        public PCE PCE { get; private set; }
        public string Description { get; private set; }
        public string DbName { get; private set; }
        public string ConnectionString { get; private set; }
        //private int myYear;
        //public int Year
        //{
        //    get { return myYear; }
        //    private set
        //    {
        //        int.TryParse(PCE.GetProjectYear(DbName), out int year);
        //        myYear = year;
        //    }
        //}
        private string myYear;
        public string Year
        {
            get
            {
                return myYear;
            }
            private set
            {
                myYear = PCE.GetProjectYear(DbName);
            }
        }

        private DataSet Dataset;

        public Project(PCE pce, string projectdbname)
        {
            try
            {
                PCE = new PCE(pce);
                DbName = projectdbname;
                ConnectionString = PCE.GetProjectConnectionString(DbName);
                Description = PCE.GetProjectDescription(DbName);
                Year = PCE.GetProjectYear(DbName);
                BuildDataSet();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Project(Project oldproject) : this(oldproject.PCE, oldproject.DbName) { }

        private void BuildDataSet()
        {
            Dataset = new DataSet("ProjectData");
            try
            {
                SqlDataAdapter SQLDataAdapter_Settings;
                SqlDataAdapter SQLDataAdapter_Linktable;

                SQLDataAdapter_Settings = BuildProjectTableDataAdapter(Tables.Settings);
                SQLDataAdapter_Linktable = BuildProjectTableDataAdapter(Tables.LinkTable);

                Dataset.Tables.Add(Tables.Settings.ToString());
                SQLDataAdapter_Settings.FillSchema(Dataset, SchemaType.Source);
                SQLDataAdapter_Settings.Fill(Dataset.Tables[Tables.Settings.ToString()]);

                Dataset.Tables.Add(Tables.LinkTable.ToString());
                SQLDataAdapter_Linktable.FillSchema(Dataset, SchemaType.Source);
                SQLDataAdapter_Linktable.Fill(Dataset.Tables[Tables.LinkTable.ToString()]);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Returns a data adapter for the project table. Optional filter should be formatted like the WHERE portion of a query.
        /// </summary>
        /// <param name="table">Tables enumerator value</param>
        /// <param name="filter">WHERE portion for query string</param>
        /// <returns>SqlDataAdapter set up to run on the project.</returns>
        public SqlDataAdapter BuildProjectTableDataAdapter(Tables table, string filter = "")
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter();

                SqlCommand comm = new SqlCommand
                {
                    CommandType = CommandType.Text,
                    CommandText = $"SELECT * FROM {table} {filter}",
                    Connection = new SqlConnection(ConnectionString)
                };
                adapter.SelectCommand = comm;

                return adapter;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the value for PROJNUMBER from Settings table
        /// </summary>
        /// <returns>string representing the Project number</returns>
        public string GetProjectNumber()
        {
            string projnum;
            try
            {
                //Pass this to the variable query method:
                projnum = QuerySettingsTable("project", "setup", "PROJNUMBER");
                return projnum;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the value for PROJNUMBER from Settings table
        /// </summary>
        /// <returns>string representing the Project number</returns>
        public string GetProjectUnits()
        {
            string projunit;
            try
            {
                //Pass this to the variable query method:
                projunit = QuerySettingsTable("project", "setup", "SYSTEM");
                return projunit;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Performs a query on the Project Settings table with the provided Root, Section, and Keyname values. Returns the value from the Value column that is ruturned.
        /// </summary>
        /// <param name="root">value to find for Root column</param>
        /// <param name="section">value to find for Section column</param>
        /// <param name="keyname">value to find for Keyname column</param>
        /// <returns>value of resultant Value column</returns>
        public string QuerySettingsTable(string root, string section, string keyname)
        {
            string value = string.Empty;
            string filter = $"root = '{root}' and section = '{section}' and keyname = '{keyname}'";
            try
            {
                //Find the row
                DataRow[] row = QueryProjectTable(Tables.Settings, filter);
                if (row.Count() == 1)
                {
                    //We have the row. Get what we need...
                    foreach (DataRow dr in row)
                    {
                        value = dr["value"].ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return value;
        }

        /// <summary>
        /// Performs a .Select(FilterExpression) against the table in the DataSet.
        /// </summary>
        /// <param name="tablename">Name of the table in the DataSet (must exist as Tables enum)</param>
        /// <param name="filterExpression">Expression to pass to DataTables.Select(FilterExpress)</param>
        /// <returns>Array of DataRows that match filterExpression</returns>
        private DataRow[] QueryProjectTable(Tables tablename, string filterExpression)
        {

            try
            {
                DataRow[] row = Dataset.Tables[tablename.ToString()].Select(filterExpression);
                return row;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
