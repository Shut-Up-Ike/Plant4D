using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Plant4D
{
    internal enum PCETables
    {
        Databases,
        Tools,
        Project_Tool_Rights,
        Users
        //can add more as needed
    }
    public class PCE
    {
        public Server Server { get; private set; }
        public string DbName { get; private set; }
        public string Description { get; private set; }
        public string ConnectionString { get; private set; }
        private SqlDataAdapter SQLAdapter_Databases;
        private DataSet Dataset;
        public PCE(Server myServer)
        {
            Server = new Server(myServer);
            ConnectionString = BaseClass.GetPCEConnectionString(Server.ServerType, Server.P4DVersion);
            Description = $"PCE for {Server.Description}";
            DbName = BaseClass.GetPCEDbName(Server.ServerType, Server.P4DVersion);
            BuildDataSet();
        }

        /// <summary>
        /// Copy constructor. Creates a copy of the passed in PCE.
        /// </summary>
        /// <param name="oldPCE"></param>
        public PCE(PCE oldPCE) : this(oldPCE.Server)
        {

        }

        public List<PCEProject> GetPCEProjects()
        {
            List<PCEProject> projects = new List<PCEProject>();


            try
            {
                projects = (from DataRow dr in Dataset.Tables[PCETables.Databases.ToString()].Rows
                            select new PCEProject()
                            {
                                dbName = dr["DbName"].ToString(),
                                Description = dr["Description"].ToString(),
                                Year = dr["db_yr"].ToString()
                            }).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return projects;
        }

        public List<PCEProject> GetPCEProjectsByYear(int year)
        {
            List<PCEProject> projects = new List<PCEProject>();

            //Validate the year. Yeah, I'm ambitious...
            if (year < 2100 && year > 1999)
            {
                try
                {
                    projects = (from DataRow dr in Dataset.Tables[PCETables.Databases.ToString()].Rows
                                where dr.Field<string>("db_yr") == year.ToString()
                                select new PCEProject()
                                {
                                    dbName = dr["DbName"].ToString(),
                                    Description = dr["Description"].ToString(),
                                    Year = dr["db_yr"].ToString()
                                }).ToList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else throw new ArgumentException("Year must be between 2000 and 2099");

            return projects;
        }

        public string[] GetPCEProjectDBNamesByYear(int year)
        {
            string[] projects;

            //Validate the year. Yeah, I'm ambitious...
            if (year < 2100 && year > 1999)
            {
                try
                {
                    projects = (from DataRow dr in Dataset.Tables[PCETables.Databases.ToString()].Rows
                                where dr.Field<string>("db_yr") == year.ToString()
                                select dr["DbName"].ToString()).ToArray();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else throw new ArgumentException("Year must be between 2000 and 2099");

            return projects;
        }

        public string GetProjectYear(string dbname)
        {
            string year = string.Empty;
            try
            {
                //Find the row
                DataRow[] row = Dataset.Tables[PCETables.Databases.ToString()].Select($"dbname = '{dbname}'");
                if (row.Count() == 1)
                {
                    //We have the row. Get what we need...
                    foreach (DataRow dr in row)
                    {
                        year = dr["db_yr"].ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return year;
        }

        //Get a project's connection string from its PCE entry.
        //First we need to know how to find it in the databases table. DBName is a good field, so is dbid.
        //Once we know an identifier, we can get DBLocation and DBType (and DBName if we didn't already have it) and build our OWN connectionstring!
        //Let's try that out...
        internal string GetProjectConnectionString(string dbname)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            try
            {
                //Find the row
                DataRow[] row = Dataset.Tables[PCETables.Databases.ToString()].Select($"dbname = '{dbname}'");
                if (row.Count() == 1)
                {
                    //We have the row. Get what we need...
                    foreach (DataRow dr in row)
                    {
                        builder.DataSource = dr["DBLocation"].ToString();
                        builder.InitialCatalog = dr["dbname"].ToString();
                        //TODO: variablize the username and password:
                        builder.UserID = "p4d_admin";
                        builder.Password = "myadminpass1";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return builder.ConnectionString;
        }


        internal string GetProjectDescription(string dbname)
        {
            string desc = string.Empty;
            try
            {
                //Find the row
                DataRow[] row = Dataset.Tables[PCETables.Databases.ToString()].Select($"dbname = '{dbname}'");
                if (row.Count() == 1)
                {
                    //We have the row. Get what we need...
                    foreach (DataRow dr in row)
                    {
                        desc = dr["Description"].ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return desc;
        }

        private void BuildDataAdapter_Databases()
        {
            SQLAdapter_Databases = new SqlDataAdapter();

            //Build select portion
            SqlCommand comm = new SqlCommand
            {
                CommandType = CommandType.Text,
                CommandText = $"SELECT * FROM {PCETables.Databases} WHERE DBGROUP = 'PROJECT' ORDER BY db_yr, DBNAME",
                Connection = new SqlConnection(ConnectionString)
            };
            SQLAdapter_Databases.SelectCommand = comm;

            //TODO: Insert/Update commands?
        }
        private void BuildDataSet()
        {
            BuildDataAdapter_Databases();
            Dataset = new DataSet("PCEData");
            Dataset.Tables.Add(PCETables.Databases.ToString());
            try
            {
                SQLAdapter_Databases.FillSchema(Dataset, SchemaType.Source, PCETables.Databases.ToString());
                SQLAdapter_Databases.Fill(Dataset.Tables[PCETables.Databases.ToString()]);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
