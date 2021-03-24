<%@ Application Language="C#" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        // if there is a database bound, check to make sure it has the proper tables
        if (CurrentEnvironment.hasDbConnection)
        {
            CurrentEnvironment.CheckDBstructure();
        }
    }

    void Application_Error(object sender, EventArgs e) 
    {
        Exception lastError = Server.GetLastError();
        Console.WriteLine("Unhandled exception: " + lastError.Message + lastError.StackTrace);
    }
</script>
