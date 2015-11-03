using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;

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
        // Not on cloud foundry so filling in the blanks.
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(INSTANCE_GUID_ENV_VARIABLE_NAME)))
        {
            Environment.SetEnvironmentVariable(BOUND_SERVICES_ENV_VARIABLE_NAME, "{}");
            Environment.SetEnvironmentVariable(PORT_ENV_VARIABLE_NAME, NOT_ON_CLOUD_FOUNDRY_MESSAGE);
            Environment.SetEnvironmentVariable(INSTANCE_GUID_ENV_VARIABLE_NAME, NOT_ON_CLOUD_FOUNDRY_MESSAGE);
            Environment.SetEnvironmentVariable(INSTANCE_INDEX_ENV_VARIABLE_NAME, NOT_ON_CLOUD_FOUNDRY_MESSAGE);
        }
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

    /// <summary>
    /// Detect a bound service for database, no database found will return an empty string.  Currently only supports SQL Server
    /// </summary>
    public static string DbConnectionString
    {
        get
        {
            // first check for sql server
            if (BoundServices.GetValue("mssql-dev") != null)
                return BoundServices["mssql-dev"][0]["credentials"]["connectionString"].ToString();
            else // no database
                return string.Empty;
        }
    }

    /// <summary>
    /// Looks to see if the connection string could be found in a bound service.
    /// </summary>
    public static bool hasDbConnection
    {
        get { return DbConnectionString != string.Empty ? true : false; }
    }

    /// <summary>
    /// Checks the database to see if it has the proper tables.  If not, add the table and an attendee.
    /// </summary>
    public static void CheckDBstructure()
    {
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

                    INSERT INTO [dbo].[attendee]
                    ([address]
                    ,[city]
                    ,[email_address]
                    ,[first_name]
                    ,[last_name]
                    ,[phone_number]
                    ,[state]
                    ,[zip_code])
                    VALUES
                    ('101 W. Fifth St.'
                    ,'Louisville'
                    ,'user1@example.com'
                    ,'Workshop'
                    ,'Participant'
                    ,'502-123-4567'
                    ,'KY'
                    ,'12345')
            
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

    public static void KillApp()
    {
        Console.WriteLine("Kaboom.");
        Environment.Exit(-1);
    }
}