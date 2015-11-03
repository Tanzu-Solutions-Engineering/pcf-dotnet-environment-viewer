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

        // if a database service is bound, show the attendees
        if (CurrentEnvironment.hasDbConnection)
        {
            attendeePane.Visible = true;
            gridAttendees.DataSource = AttendeeRepository.getAttendees();
            gridAttendees.DataBind();
        }
    }
    protected void btnKill_Click(object sender, EventArgs e)
    {
        CurrentEnvironment.KillApp();
    }
}