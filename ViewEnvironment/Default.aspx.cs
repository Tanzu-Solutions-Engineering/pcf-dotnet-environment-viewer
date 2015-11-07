using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections.Generic;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Don't cache so a refresh will go to server
        // Stop Caching in IE
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        // Stop Caching in Firefox
        Response.Cache.SetNoStore();

        // Get environment variables and dump them
        IDictionary vars = System.Environment.GetEnvironmentVariables();
        System.Environment.GetEnvironmentVariables();
        foreach (DictionaryEntry entry in vars)
        {
            // add to querystring all to dump all environment variables
            if (Request.QueryString["all"] != null)
                Response.Write(entry.Key + " = " + entry.Value + "<br>");
        }

        lblTime.Text = CurrentEnvironment.CurrentTime;
        lblDotNetVersion.Text = CurrentEnvironment.CLRVersion;
        lblPort.Text = CurrentEnvironment.Port;
        lblInstanceID.Text = CurrentEnvironment.InstanceID;
        lblInstanceIndex.Text = CurrentEnvironment.InstanceIndex;
        lblInstanceStart.Text = CurrentEnvironment.Uptime;
        lblBoundServices.Text = CurrentEnvironment.BoundServices.ToString();
        lblDbEngine.Text = CurrentEnvironment.DbEngine.ToString();

        // if a database service is bound, show the attendees
        if (CurrentEnvironment.hasDbConnection)
        {            
            AttendeeDataSource.ConnectionString = CurrentEnvironment.DbConnectionString;
            // Read
            AttendeeDataSource.SelectCommand = "select * from attendee";

            // Delete
            AttendeeDataSource.DeleteCommand = "delete from attendee where id=@id";
            AttendeeDataSource.DeleteParameters.Add("id", System.Data.DbType.Int64, "0");

            

             // SQL Server
            if (CurrentEnvironment.DbEngine == CurrentEnvironment.DatabaseEngine.SqlServer)
            {
                // Create
                AttendeeDataSource.InsertCommand = @"INSERT INTO [attendee]
                    ([address]
                    ,[city]
                    ,[email_address]
                    ,[first_name]
                    ,[last_name]
                    ,[phone_number]
                    ,[state]
                    ,[zip_code])
                    VALUES
                    ('123 Main St.'
                    ,'Louisville'
                    ,'user1@example.com'
                    ,'Workshop'
                    ,'Participant'
                    ,'502-123-4567'
                    ,'KY'
                    ,'12345')";

                AttendeeDataSource.UpdateCommand = @"update [attendee]
                    set [address] = @address
                        ,[city] = @city
                        ,[email_address] = @email_address
                        ,[first_name] = @first_name
                        ,[last_name] = @last_name
                        ,[phone_number] = @phone_number
                        ,[state] = @state
                        ,[zip_code] = @zip_code
                    WHERE id=@id";
            }
            else if (CurrentEnvironment.DbEngine == CurrentEnvironment.DatabaseEngine.MySql)
            {
                AttendeeDataSource.ProviderName = "MySql.Data.MySqlClient";

                // Create
                AttendeeDataSource.InsertCommand = @"INSERT INTO `attendee`
			        (`address`
                    ,`city`
                    ,`email_address`
                    ,`first_name`
                    ,`last_name`
                    ,`phone_number`
                    ,`state`
                    ,`zip_code`)
                    VALUES
                    ('123 Main St.'
                    ,'Louisville'
                    ,'user1@example.com'
                    ,'Workshop'
                    ,'Participant'
                    ,'502-123-4567'
                    ,'KY'
                    ,'12345')";

                AttendeeDataSource.UpdateCommand = @"UPDATE `attendee`
                        SET
                            `address` = @address,
                            `city` = @city,
                            `email_address` = @email_address,
                            `first_name` = @first_name,
                            `last_name` = @last_name,
                            `phone_number` = @phone_number,
                            `state` = @state,
                            `zip_code` = @zip_code
                            WHERE `id` = @id;";
            }

            if (!IsPostBack)
            {
                // Hidden by default, display
                attendeePane.Visible = true;

                //gridAttendees.DataSource = AttendeeRepository.getAttendees();
                gridAttendees.DataKeyNames = new string[] { "id" };
                gridAttendees.DataBind();
            }
        }
    }
    protected void btnKill_Click(object sender, EventArgs e)
    {
        CurrentEnvironment.KillApp();
    }
    protected void btnAddAttendee_Click(object sender, EventArgs e)
    {
        AttendeeDataSource.Insert();
        Response.Redirect("/");
    }
}