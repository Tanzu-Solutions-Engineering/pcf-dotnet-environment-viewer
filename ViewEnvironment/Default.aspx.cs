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
        // Get environment variables and dump them
        IDictionary vars = System.Environment.GetEnvironmentVariables();
        System.Environment.GetEnvironmentVariables();
        foreach (DictionaryEntry entry in vars)
        {
            // add to querystring all to dump all environment variables
            if (Request.QueryString["all"]!=null)
                Response.Write(entry.Key + " = " + entry.Value + "<br>");
        }

        lblTime.Text = DateTime.Now.ToString();
        lblDotNetVersion.Text = Environment.Version.ToString();
        lblPort.Text = Environment.GetEnvironmentVariable("PORT");
        lblInstanceID.Text = Environment.GetEnvironmentVariable("INSTANCE_GUID");
        lblInstanceIndex.Text = Environment.GetEnvironmentVariable("INSTANCE_INDEX");
        lblInstanceStart.Text =  DateTime.Now.Subtract(TimeSpan.FromMilliseconds(Environment.TickCount)).ToString();
        lblBoundServices.Text = Environment.GetEnvironmentVariable("VCAP_SERVICES");
    }
    protected void btnKill_Click(object sender, EventArgs e)
    {
        log("Kaboom.");
        Environment.Exit(-1);
    }

    private void log(string message)
    {
        Console.WriteLine(message);
    }
}