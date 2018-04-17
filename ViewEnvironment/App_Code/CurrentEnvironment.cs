using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

/// <summary>
/// Evaluates the running environment.
/// </summary>
public class CurrentEnvironment
{
    private static readonly string PORT_ENV_VARIABLE_NAME = "PORT";
    private static readonly string INSTANCE_GUID_ENV_VARIABLE_NAME = "INSTANCE_GUID";
    private static readonly string INSTANCE_INDEX_ENV_VARIABLE_NAME = "INSTANCE_INDEX";
    private static readonly string BOUND_SERVICES_ENV_VARIABLE_NAME = "VCAP_SERVICES";
    private static readonly string NOT_ON_CLOUD_FOUNDRY_MESSAGE = "Not running in cloud foundry.";

    /// <summary>
    /// static constructor to determine the state of the environment and set defaults 
    /// </summary>
    static CurrentEnvironment()
    {
        // Not on cloud foundry filling in the blanks.
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(INSTANCE_GUID_ENV_VARIABLE_NAME)))
        {
            Environment.SetEnvironmentVariable(BOUND_SERVICES_ENV_VARIABLE_NAME, "{}");
            Environment.SetEnvironmentVariable(PORT_ENV_VARIABLE_NAME, NOT_ON_CLOUD_FOUNDRY_MESSAGE);
            Environment.SetEnvironmentVariable(INSTANCE_GUID_ENV_VARIABLE_NAME, NOT_ON_CLOUD_FOUNDRY_MESSAGE);
            Environment.SetEnvironmentVariable(INSTANCE_INDEX_ENV_VARIABLE_NAME, NOT_ON_CLOUD_FOUNDRY_MESSAGE);
        }

        // check to see if DB is bound, if so...what type
        // SQL server first
        if (BoundServices.GetValue("azure-sqldb") != null) // Azure SQL Database (Azure Broker)
        {
            DbEngine = DatabaseEngine.SqlServer;
            SqlConnectionStringBuilder csbuilder = new SqlConnectionStringBuilder();
            csbuilder.Add("server", BoundServices["azure-sqldb"][0]["credentials"]["hostname"].ToString());
            csbuilder.Add("uid", BoundServices["azure-sqldb"][0]["credentials"]["username"].ToString());
            csbuilder.Add("pwd", BoundServices["azure-sqldb"][0]["credentials"]["password"].ToString());
            csbuilder.Add("database", BoundServices["azure-sqldb"][0]["credentials"]["name"].ToString());
            _connectionString = csbuilder.ToString();
        }
        else if(BoundServices.GetValue("azure-mysqldb") != null || BoundServices.GetValue("p.mysql") != null) // MySQL for PCF or Mysql for AZURE
        {
            string label = "p.mysql"; // MySQL Database.

            if (BoundServices.GetValue("azure-mysqldb") != null)
                label = "azure-mysqldb"; //Mysql Database on Azure (Mysql For Azure)
            
            DbEngine = DatabaseEngine.MySql;
            MySqlConnectionStringBuilder csbuilder = new MySqlConnectionStringBuilder();
            csbuilder.Add("server", BoundServices[label][0]["credentials"]["hostname"].ToString());
            csbuilder.Add("port", BoundServices[label][0]["credentials"]["port"].ToString());
            csbuilder.Add("uid", BoundServices[label][0]["credentials"]["username"].ToString());
            csbuilder.Add("pwd", BoundServices[label][0]["credentials"]["password"].ToString());
            csbuilder.Add("database", BoundServices[label][0]["credentials"]["name"].ToString());
            _connectionString = csbuilder.ToString();
        }
        else
            DbEngine = DatabaseEngine.None;

    
    }

    /// <summary>
    /// Current server time.
    /// </summary>
    public static string CurrentTime
    {
        get { return DateTime.Now.ToString(); }
    }

    /// <summary>
    /// .NET runtime version
    /// </summary>
    public static string CLRVersion
    {
        get { return Environment.Version.ToString(); }
    }

    /// <summary>
    /// Uptime.  This method appears to be machine wide and specific to the container.
    /// </summary>
    public static string Uptime
    {
        get {return DateTime.Now.Subtract(TimeSpan.FromMilliseconds(Environment.TickCount)).ToString(); }
    }

    /// <summary>
    /// The local port the container is listening on
    /// </summary>
    public static string Port
    {
        get { return Environment.GetEnvironmentVariable(PORT_ENV_VARIABLE_NAME); }
    }

    /// <summary>
    /// The instance GUID Cloud Foundry assigned to this app instance.
    /// </summary>
    public static string InstanceID
    {
        get { return Environment.GetEnvironmentVariable(INSTANCE_GUID_ENV_VARIABLE_NAME); }
    }

    /// <summary>
    /// The zero based index assigned to this instance of the app.
    /// </summary>
    public static string InstanceIndex
    {
        get { return Environment.GetEnvironmentVariable(INSTANCE_INDEX_ENV_VARIABLE_NAME); }
    }

    public static JObject BoundServices
    {
        get { return JObject.Parse(Environment.GetEnvironmentVariable(BOUND_SERVICES_ENV_VARIABLE_NAME)); }
    }

    private static string _connectionString = string.Empty;
    /// <summary>
    /// Detect a bound service for database, no database found will return an empty string.  Currently only supports SQL Server
    /// </summary>
    public static string DbConnectionString
    {
        get { return _connectionString; }
    }

    /// <summary>
    /// Looks to see if the connection string could be found in a bound service.
    /// </summary>
    public static bool hasDbConnection
    {
        get { return DbEngine != DatabaseEngine.None ? true : false; }
    }

    /// <summary>
    /// Tells which DB engine was detected during app startup.
    /// </summary>
    public static DatabaseEngine DbEngine
    {
        get;
        set;
    }

    /// <summary>
    /// Checks the database to see if it has the proper tables.  If not, add the table and an attendee.
    /// </summary>
    public static void CheckDBstructure()
    {
        Console.WriteLine("Checking DB structure.");
        if (DbEngine == DatabaseEngine.SqlServer)
        {
            Console.WriteLine("Detected an mssql-dev service binding.");
            // make sure tables exist
           
            using (SqlConnection conn = new SqlConnection(CurrentEnvironment.DbConnectionString))
            // if the table doesn't exist, create it
            using (SqlCommand command = new SqlCommand()
            {
                CommandText = @"IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'attendee'))
                BEGIN
                    CREATE TABLE attendee (
                    id bigint IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
                    address varchar(255) DEFAULT NULL,
                    city varchar(255) DEFAULT NULL,
                    email_address varchar(255) DEFAULT NULL,
                    first_name varchar(255) DEFAULT NULL,
                    last_name varchar(255) DEFAULT NULL,
                    phone_number varchar(255) DEFAULT NULL,
                    state varchar(255) DEFAULT NULL,
                    zip_code varchar(255) DEFAULT NULL)            
                END",
                Connection = conn,
                CommandType = CommandType.Text
            })
            {
                conn.Open();
                int rows = command.ExecuteNonQuery();
                if (rows > -1)
                    Console.WriteLine("table didn't exist, creating it: " + rows + " rows affected.");
            }
        }
        else if (DbEngine == DatabaseEngine.MySql)
        {
            using (MySqlConnection conn = new MySqlConnection(CurrentEnvironment.DbConnectionString))
            // if the table doesn't exist, create it
            using (MySqlCommand command = new MySqlCommand()
            {
                CommandText = @"CREATE TABLE IF NOT EXISTS `attendee` (
                  `id` bigint(20) NOT NULL AUTO_INCREMENT,
                  `address` varchar(255) DEFAULT NULL,
                  `city` varchar(255) DEFAULT NULL,
                  `email_address` varchar(255) DEFAULT NULL,
                  `first_name` varchar(255) DEFAULT NULL,
                  `last_name` varchar(255) DEFAULT NULL,
                  `phone_number` varchar(255) DEFAULT NULL,
                  `state` varchar(255) DEFAULT NULL,
                  `zip_code` varchar(255) DEFAULT NULL,
                  PRIMARY KEY (`id`)
                ) AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;",
                Connection = conn,
                CommandType = CommandType.Text
            })
            {
                conn.Open();
                int rows = command.ExecuteNonQuery();
                if (rows > 0)
                    Console.WriteLine("table didn't exist, creating it: " + rows + " rows affected.");
            };
        }
        else
            Console.WriteLine("No DB found.");
    }

    public static void KillApp()
    {
        Console.WriteLine("Kaboom.");
        Environment.Exit(-1);
    }

    public enum DatabaseEngine
    {
        None=0,
        SqlServer=1,
        MySql=2
    }
}