using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

/// <summary>
/// Evaluates the running environment.
/// </summary>
public class CurrentEnvironment
{
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
        get { return Environment.GetEnvironmentVariable("PORT"); }
    }

    /// <summary>
    /// The instance GUID Cloud Foundry assigned to this app instance.
    /// </summary>
    public static string InstanceID
    {
        get { return Environment.GetEnvironmentVariable("INSTANCE_GUID"); }
    }

    /// <summary>
    /// The zero based index assigned to this instance of the app.
    /// </summary>
    public static string InstanceIndex
    {
        get { return Environment.GetEnvironmentVariable("INSTANCE_INDEX"); }
    }

    public static JObject BoundServices
    {
        get { return JObject.Parse(Environment.GetEnvironmentVariable("VCAP_SERVICES")); }
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
            {
                Console.WriteLine("Detected an mssql-dev service binding.");
                return BoundServices["mssql-dev"][0]["credentials"]["connectionString"].ToString();
            }
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

    public static void KillApp()
    {
        Console.WriteLine("Kaboom.");
        Environment.Exit(-1);
    }
}