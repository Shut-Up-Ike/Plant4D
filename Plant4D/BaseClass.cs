using System;

namespace Plant4D
{
    /// <summary>
    /// Combination of enums ServerType and P4DVersion
    /// </summary>
    public enum ServerTypeAndP4DVersion
    {
        None,
        ProductionAthena,
        DevelopmentAthena,
        ProductionRome,
        DevelopmentRome
    }

    /// <summary>
    /// Enumeration of types of servers (Production or Development)
    /// </summary>
    public enum ServerType
    {
        None,
        Production,
        Development
    }

    /// <summary>
    /// Enumeration of versions of Plant-4D (currently Athena or Rome)
    /// </summary>
    public enum P4DVersion
    {
        None,
        Athena,
        Rome
    }


    /// <summary>
    /// Struct used to pass data about projects out of PCE class
    /// </summary>
    public struct PCEProject
    {
        public string dbName;
        public string Description { get; set; }
        public string Year;
        public PCEProject(string dbname, string description, string year)
        {
            dbName = dbname;
            Description = description;
            Year = year;
        }
        public override string ToString()
        {
            return Description;
        }
    }

    internal enum P4DUserType
    {
        None,
        Basic,
        Elevated,
        Admin
    }
    class BaseClass
    {
        /// <summary>
        /// Get the sql connection string of the PCE database used by the supplied ServerType and P4DVersion.
        /// </summary>
        /// <param name="serverType">enum ServerType</param>
        /// <param name="p4dVersion">enum P4DVersion</param>
        /// <returns>string SQLConnectionString</returns>
        internal static string GetPCEConnectionString(ServerType serverType, P4DVersion p4dVersion)
        {
            return GetPCEConnectionString(CombineServerTypeAndP4DVersion(serverType, p4dVersion));
        }

        /// <summary>
        /// Get the sql connection string of the PCE database used by the supplied combination of ServerType and P4DVersion.
        /// </summary>
        /// <param name="typeAndVersion">enum ServerTypeAndP4DVersion</param>
        /// <returns>string SQLConnectionString</returns>
        internal static string GetPCEConnectionString(ServerTypeAndP4DVersion typeAndVersion)
        {
            //TODO: make userid and password variables somehow.
            return $"Data Source={GetServerName(typeAndVersion)};Initial Catalog={GetPCEDbName(typeAndVersion)};Persist Security Info=False;User ID=p4d_admin;Password=myadminpass1";
        }

        /// <summary>
        /// Returns the name of the server used by the supplied combination of ServerType and P4DVersion.
        /// </summary>
        /// <param name="typeAndVersion">enum ServerTypeAndP4DVersion</param>
        /// <returns>string name of server</returns>
        internal static string GetServerName(ServerTypeAndP4DVersion typeAndVersion)
        {
            string myServerName = string.Empty;

            switch (typeAndVersion)
            {
                case ServerTypeAndP4DVersion.ProductionAthena:
                    myServerName = "AthenaProdDbName";
                    break;
                case ServerTypeAndP4DVersion.DevelopmentAthena:
                    myServerName = "AthenaDevDbName";
                    break;
                case ServerTypeAndP4DVersion.ProductionRome:
                    myServerName = "RomeProdDbName";
                    break;
                case ServerTypeAndP4DVersion.DevelopmentRome:
                    myServerName = "RomeDevDbName";
                    break;
                default:
                    break;
            }

            return myServerName;
        }

        /// <summary>
        /// Returns the name of the database used by the supplied combination of ServerType and P4DVersion 
        /// </summary>
        /// <param name="serverType">enum ServerType</param>
        /// <param name="p4DVersion">enum P4DVersion</param>
        /// <returns>string of database name</returns>
        internal static string GetPCEDbName(ServerType serverType, P4DVersion p4DVersion)
        {
            return GetPCEDbName(CombineServerTypeAndP4DVersion(serverType, p4DVersion));
        }

        /// <summary>
        /// Returns the name of the database used by the supplied combination of ServerTypeAndP4DVersion.
        /// </summary>
        /// <param name="typeAndVersion">enum ServerTypeAndP4DVersion</param>
        /// <returns>string of database name</returns>
        internal static string GetPCEDbName(ServerTypeAndP4DVersion typeAndVersion)
        {
            string myServerName;

            switch (typeAndVersion)
            {
                //At this point in time, all share the same name. That could change (and has before) at any moment...
                case ServerTypeAndP4DVersion.ProductionAthena:
                case ServerTypeAndP4DVersion.DevelopmentAthena:
                case ServerTypeAndP4DVersion.ProductionRome:
                case ServerTypeAndP4DVersion.DevelopmentRome:
                default:
                    myServerName = "PCE";
                    break;
            }

            return myServerName;
        }

        /// <summary>
        /// Converts individual values for ServerType and P4DVersion to a combined ServerTypeAndP4DVersion enum.
        /// </summary>
        /// <param name="servertype"></param>
        /// <param name="p4dversion"></param>
        /// <returns>enum ServerTypeAndP4DVersion</returns>
        internal static ServerTypeAndP4DVersion CombineServerTypeAndP4DVersion(ServerType servertype, P4DVersion p4dversion)
        {
            //Default return value is ProductionRome.
            switch (servertype)
            {
                case ServerType.Development:
                    switch (p4dversion)
                    {
                        case P4DVersion.Athena:
                            return ServerTypeAndP4DVersion.DevelopmentAthena;
                        default:
                            return ServerTypeAndP4DVersion.DevelopmentRome;
                    }
                default:
                    switch (p4dversion)
                    {
                        case P4DVersion.Athena:
                            return ServerTypeAndP4DVersion.ProductionAthena;
                        default:
                            return ServerTypeAndP4DVersion.ProductionRome;
                    }
            }
        }


        /// <summary>
        /// Returns the ServerType from a combined ServerTypeAndP4DVersion enum. Default value is ServerType.Production.
        /// </summary>
        /// <param name="combined"></param>
        /// <returns>enum ServerType</returns>
        internal static ServerType GetServerTypeFromCombination(ServerTypeAndP4DVersion combined)
        {
            switch (combined)
            {
                case ServerTypeAndP4DVersion.ProductionAthena:
                case ServerTypeAndP4DVersion.ProductionRome:
                    return ServerType.Production;
                case ServerTypeAndP4DVersion.DevelopmentAthena:
                case ServerTypeAndP4DVersion.DevelopmentRome:
                    return ServerType.Development;
                default:
                    return ServerType.None;
            }
        }


        /// <summary>
        /// Returns the P4DVersion from a combined ServerTypeAndP4DVersion enum.
        /// </summary>
        /// <param name="combined"></param>
        /// <returns>enum P4DVersion</returns>
        internal static P4DVersion GetP4DVersionFromCombination(ServerTypeAndP4DVersion combined)
        {
            switch (combined)
            {
                case ServerTypeAndP4DVersion.ProductionAthena:
                case ServerTypeAndP4DVersion.DevelopmentAthena:
                    return P4DVersion.Athena;
                case ServerTypeAndP4DVersion.DevelopmentRome:
                case ServerTypeAndP4DVersion.ProductionRome:
                    return P4DVersion.Rome;
                default:
                    return P4DVersion.None;
            }
        }
    }


    /// <summary>
    /// Class to convert Enums into usable strings for use with various other objects/classes
    /// </summary>
    public class Convert
    {
        public static string[] GetServerTypesAsStringArray()
        {
            return Enum.GetNames(typeof(ServerType));
        }


        public static string[] GetP4DVersionsAsStringArray()
        {
            return Enum.GetNames(typeof(P4DVersion));
        }


        public static string[] GetServerTypeAndP4DVersionAsStringArray()
        {
            return Enum.GetNames(typeof(ServerTypeAndP4DVersion));
        }

        public static ServerType GetServerTypeFromString(string servertype)
        {
            servertype = servertype.ToLower();
            switch (servertype)
            {
                case "development":
                    return ServerType.Development;
                case "production":
                    return ServerType.Production;
                default:
                    return ServerType.None;
            }
        }

        public static P4DVersion GetP4DVersionFromString(string p4dversion)
        {
            p4dversion = p4dversion.ToLower();
            switch (p4dversion)
            {
                case "athena":
                    return P4DVersion.Athena;
                case "rome":
                    return P4DVersion.Rome;
                default:
                    return P4DVersion.None;
            }
        }

        public static ServerTypeAndP4DVersion GetServerTypeAndP4DVersionFromString(string servertypeandp4dversion)
        {
            servertypeandp4dversion = servertypeandp4dversion.ToLower();
            switch (servertypeandp4dversion)
            {
                case "productionathena":
                    return ServerTypeAndP4DVersion.ProductionAthena;
                case "productionrome":
                    return ServerTypeAndP4DVersion.ProductionRome;
                case "developmentathena":
                    return ServerTypeAndP4DVersion.DevelopmentAthena;
                case "developmentrome":
                    return ServerTypeAndP4DVersion.DevelopmentRome;
                default:
                    return ServerTypeAndP4DVersion.None;
            }
        }
    }

}
