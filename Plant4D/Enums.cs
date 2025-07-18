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
    public enum Tables
    {
        Commondata,
        Solodata,
        Components,
        Settings,
        LinkTable,
        NE_Settings
    }
}
