using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Get environment variables and dump them
        IDictionary vars = System.Environment.GetEnvironmentVariables();
        System.Environment.GetEnvironmentVariables();
        foreach (DictionaryEntry entry in vars)
        {
            // add to querystring all to dump everything, not just the vcap items.
            if (Request.QueryString["all"]==null)
            {
                if (entry.Key.ToString().StartsWith("VCAP"))
                    Response.Write(entry.Key + " = " + entry.Value + "<br>");
            }
            else
                Response.Write(entry.Key + " = " + entry.Value + "<br>");
        }
    }
}