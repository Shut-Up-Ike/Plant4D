using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Plant4D
{
    public class Plant4D
    {
        private const string USERNAMEELEVATED = "p4d_admin";

        private const string PASSWORDELEVATED = "myadminpass1";

        private const string USERNAMEDEFAULT = "p4d_user";

        private const string PASSWORDDEFAULT = "p4duser1";

        private const string ROMESERVERNAME_PROD = "RomeProdServerName";

        private const string ATHENASERVERNAME_PROD = "AthenaProdServerName";

        private const string ROMESERVERNAME_DEV = "RomeDevServerName";

        private const string ATHENASERVERNAME_DEV = "AthenaDevServerName";

        private const string ROMEPCEDBNAME_PROD = "Production_Rome_PCEDbName";

        private const string ATHENAPCEDBNAME_PROD = "Production_Athena_PCEDbName";

        private const string ROMEPCEDBNAME_DEV = "Development_Rome_PCEDbName";

        private const string ATHENAPCEDBNAME_DEV = "Development_Athena_PCEDbName";

        public static string GetProjectConnectionString(string ProjectDatabase, string Environment_ProductionOrDevelopment = "Production", string P4DVersion_RomeOrAthena = "Athena")
        {
            if (string.IsNullOrWhiteSpace(ProjectDatabase))
            {
                throw new ArgumentNullException(nameof(ProjectDatabase), "ProjectDatabase cannot be null or empty.");
            }
            if (string.IsNullOrWhiteSpace(Environment_ProductionOrDevelopment))
            {
                throw new ArgumentNullException(nameof(Environment_ProductionOrDevelopment), "Environment_ProductionOrDevelopment cannot be null or empty.");
            }
            if (string.IsNullOrWhiteSpace(P4DVersion_RomeOrAthena))
            {
                throw new ArgumentNullException(nameof(P4DVersion_RomeOrAthena), "P4DVersion_RomeOrAthena cannot be null or empty.");
            }

            //Check.NotNull(Environment_ProductionOrDevelopment, "Environment_ProductionOrDevelopment");
            //Check.NotNull(P4DVersion_RomeOrAthena, "P4DVersion_RomeOrAthena");
            //Check.NotNull(ProjectDatabase, "ProjectDatabase");
            SqlConnection Connection = GetPCEConnection(Environment_ProductionOrDevelopment, P4DVersion_RomeOrAthena);
            new SqlConnectionStringBuilder();
            string result = ProjectConnectionString(ProjectCommandByDatabase(ref Connection, ProjectDatabase));
            Connection.Dispose();
            return result;
        }

        public static string GetProjectConnectionStringByID(int ProjectID, string Environment_ProductionOrDevelopment = "Production", string P4DVersion_RomeOrAthena = "Athena")
        {
            if (string.IsNullOrWhiteSpace(Environment_ProductionOrDevelopment))
            {
                throw new ArgumentNullException(nameof(Environment_ProductionOrDevelopment), "Environment_ProductionOrDevelopment cannot be null or empty.");
            }
            if (string.IsNullOrWhiteSpace(P4DVersion_RomeOrAthena))
            {
                throw new ArgumentNullException(nameof(P4DVersion_RomeOrAthena), "P4DVersion_RomeOrAthena cannot be null or empty.");
            }
            //Check.NotNull(Environment_ProductionOrDevelopment, "Environment_ProductionOrDevelopment");
            //Check.NotNull(P4DVersion_RomeOrAthena, "P4DVersion_RomeOrAthena");
            SqlConnection Connection = GetPCEConnection(Environment_ProductionOrDevelopment, P4DVersion_RomeOrAthena);
            string result = ProjectConnectionString(ProjectCommandByID(ref Connection, ProjectID));
            Connection.Dispose();
            return result;
        }

        private static string ProjectConnectionString(SqlCommand Command)
        {
            string dataSource = string.Empty;
            string text = string.Empty;
            string text2 = string.Empty;
            string text3 = string.Empty;
            string initialCatalog = string.Empty;
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
            Command.Connection.Open();
            using (SqlDataReader sqlDataReader = Command.ExecuteReader())
            {
                if (sqlDataReader.Read())
                {
                    initialCatalog = sqlDataReader["dbname"].ToString();
                    dataSource = sqlDataReader["dblocation"].ToString();
                    text = sqlDataReader["connectionstring"].ToString();
                }

                sqlDataReader.Close();
            }

            string[] array = text.Split(';');
            for (int i = 0; i < array.Length; i++)
            {
                string[] array2 = array[i].Split('=');
                if (array2.Length == 2)
                {
                    switch (array2[0].ToLower())
                    {
                        case "userid":
                            text2 = array2[1];
                            break;
                        case "user id":
                            text2 = array2[1];
                            break;
                        case "password":
                            text3 = array2[1];
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text3))
            {
                text2 = "p4d_user";
                text3 = "p4duser";
            }

            sqlConnectionStringBuilder.DataSource = dataSource;
            sqlConnectionStringBuilder.InitialCatalog = initialCatalog;
            sqlConnectionStringBuilder.UserID = text2;
            sqlConnectionStringBuilder.Password = text3;
            return sqlConnectionStringBuilder.ConnectionString;
        }

        public static SqlConnection GetProjectConnectionByID(int ProjectID, string Environment_ProductionOrDevelopment = "Production", string P4DVersion_RomeOrAthena = "Athena")
        {
            if (string.IsNullOrWhiteSpace(Environment_ProductionOrDevelopment))
            {
                throw new ArgumentNullException(nameof(Environment_ProductionOrDevelopment), "Environment_ProductionOrDevelopment cannot be null or empty.");
            }
            if (string.IsNullOrWhiteSpace(P4DVersion_RomeOrAthena))
            {
                throw new ArgumentNullException(nameof(P4DVersion_RomeOrAthena), "P4DVersion_RomeOrAthena cannot be null or empty.");
            }
            //Check.NotNull(Environment_ProductionOrDevelopment, "Environment_ProductionOrDevelopment");
            //Check.NotNull(P4DVersion_RomeOrAthena, "P4DVersion_RomeOrAthena");
            return new SqlConnection(GetProjectConnectionStringByID(ProjectID, Environment_ProductionOrDevelopment, P4DVersion_RomeOrAthena));
        }

        private static SqlCommand ProjectCommandByID(ref SqlConnection Connection, int ProjectID)
        {
            SqlCommand sqlCommand = Connection.CreateCommand();
            sqlCommand.CommandText = "select top 1 dbname, dblocation, connectionstring from databases where databaseid = @id;";
            sqlCommand.Parameters.AddWithValue("@id", ProjectID);
            return sqlCommand;
        }

        private static SqlCommand ProjectCommandByDatabase(ref SqlConnection Connection, string Database)
        {
            SqlCommand sqlCommand = Connection.CreateCommand();
            sqlCommand.CommandText = "select top 1 dbname, dblocation, connectionstring from databases where dbname = @db;";
            sqlCommand.Parameters.AddWithValue("@db", Database);
            return sqlCommand;
        }

        public static SqlConnection GetProjectConnection(string ProjectDatabase, string Environment_ProductionOrDevelopment = "Production", string P4DVersion_RomeOrAthena = "Athena")
        {
            if (string.IsNullOrWhiteSpace(ProjectDatabase))
            {
                throw new ArgumentNullException(nameof(ProjectDatabase), "ProjectDatabase cannot be null or empty.");
            }
            if (string.IsNullOrWhiteSpace(Environment_ProductionOrDevelopment))
            {
                throw new ArgumentNullException(nameof(Environment_ProductionOrDevelopment), "Environment_ProductionOrDevelopment cannot be null or empty.");
            }
            if (string.IsNullOrWhiteSpace(P4DVersion_RomeOrAthena))
            {
                throw new ArgumentNullException(nameof(P4DVersion_RomeOrAthena), "P4DVersion_RomeOrAthena cannot be null or empty.");
            }
            //Check.NotNull(ProjectDatabase, "ProjectDatabase");
            //Check.NotNull(Environment_ProductionOrDevelopment, "Environment_ProductionOrDevelopment");
            //Check.NotNull(P4DVersion_RomeOrAthena, "P4DVersion_RomeOrAthena");
            return new SqlConnection(GetProjectConnectionString(ProjectDatabase, Environment_ProductionOrDevelopment, P4DVersion_RomeOrAthena));
        }

        public static SqlConnection GetPCEConnection(string Environment_ProductionOrDevelopment = "Production", string P4DVersion_RomeOrAthena = "Athena")
        {
            if (string.IsNullOrWhiteSpace(Environment_ProductionOrDevelopment))
            {
                throw new ArgumentNullException(nameof(Environment_ProductionOrDevelopment), "Environment_ProductionOrDevelopment cannot be null or empty.");
            }
            if (string.IsNullOrWhiteSpace(P4DVersion_RomeOrAthena))
            {
                throw new ArgumentNullException(nameof(P4DVersion_RomeOrAthena), "P4DVersion_RomeOrAthena cannot be null or empty.");
            }
            //Check.NotNull(Environment_ProductionOrDevelopment, "Environment_ProductionOrDevelopment");
            //Check.NotNull(P4DVersion_RomeOrAthena, "P4DVersion_RomeOrAthena");
            string text = (Environment_ProductionOrDevelopment.ToLower() + P4DVersion_RomeOrAthena.ToLower()) switch
            {
                "productionathena" => "Data Source=" + ATHENASERVERNAME_PROD + ";Initial Catalog=" + ATHENAPCEDBNAME_PROD + ";Persist Security Info=False;User ID=p4d_admin;Password=Adm4D*sys",
                "developmentathena" => "Data Source=" + ATHENASERVERNAME_DEV + ";Initial Catalog=" + ATHENAPCEDBNAME_DEV + ";Persist Security Info=False;User ID=p4d_admin;Password=Adm4D*sys",
                "productionrome" => "Data Source=" + ROMESERVERNAME_PROD + ";Initial Catalog=" + ROMEPCEDBNAME_PROD + ";Persist Security Info=False;User ID=p4d_admin;Password=Adm4D*sys",
                "developmentrome" => "Data Source=" + ROMESERVERNAME_DEV + ";Initial Catalog=" + ROMEPCEDBNAME_DEV + ";Persist Security Info=False;User ID=p4d_admin;Password=Adm4D*sys",
                _ => "",
            };
            if (text == "")
            {
                throw new ArgumentException("Invalid values passed in.");
            }

            return new SqlConnection(text);
        }

        public static List<Plant4DServer> GetP4DServers(string P4DVersion_Rome_Athena_Mixed = "Mixed")
        {
            List<Plant4DServer> list = new List<Plant4DServer>
        {
            new Plant4DServer("Production", "Athena", ATHENASERVERNAME_PROD),
            new Plant4DServer("Development", "Athena", ATHENASERVERNAME_DEV)
        };
            List<Plant4DServer> list2 = new List<Plant4DServer>
        {
            new Plant4DServer("Production", "Rome", ROMESERVERNAME_PROD),
            new Plant4DServer("Development", "Rome", ROMESERVERNAME_DEV)
        };
            List<Plant4DServer> list3;
            switch (P4DVersion_Rome_Athena_Mixed.ToLower())
            {
                case "rome":
                    list3 = list2;
                    break;
                case "athena":
                    list3 = list;
                    break;
                case "mixed":
                    list3 = list;
                    list3.AddRange(list2);
                    break;
                default:
                    list3 = new List<Plant4DServer>();
                    break;
            }

            return list3;
        }
    }
}
