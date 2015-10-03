<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Pivotal CF Workshop | Welcome</title>
    <link rel="stylesheet" href="css/foundation.css" />
    <script src="js/modernizr.js"></script>
</head>
<body>
    <form id="form1" runat="server">

   
    <nav class="top-bar" >
        <ul class="title-area">
            <li class="name">
                <h1><a href="./">Pivotal CF</a></h1>
            </li>
        </ul>
    </nav>

    <div class="row">
        <div class="large-12 columns">
            <h3>Welcome to Pivotal CF</h3>
            <hr/>
        </div>

        <div class="row">
            <div class="large-12 columns">
                <div class="panel">
                    <h4>Environment Information:</h4><br/>
                    <p>The current server time is <em>
                        <asp:Label ID="lblTime" runat="server"></asp:Label></em></p>
                    <p>The current .NET version is <em>
                        <asp:Label ID="lblDotNetVersion" runat="server"></asp:Label></em></p>
                    <p>The application port is <em>
                        <asp:Label ID="lblPort" runat="server"></asp:Label></em></p>
                    <p>The instance ID is <em>
                        <asp:Label ID="lblInstanceID" runat="server"></asp:Label></em></p>
                    <p>The instance index is <em>
                        <asp:Label ID="lblInstanceIndex" runat="server"></asp:Label></em></p>
                    <p>The instance was started at <em>
                        <asp:Label ID="lblInstanceStart" runat="server"></asp:Label></em></p>
                    <p>The bound services are <em>
                        <asp:Label ID="lblBoundServices" runat="server"></asp:Label></em></p>
                </div>
                <hr/>
            </div>
        </div>

        <div class="row">
            <div class="large-12 columns">
                <!-- This simply refreshes the page to demonstrate changed (or unchanged) ports -->
                <a href="javascript:window.location.reload();" class="medium button">Refresh</a>
                <!-- This link should be changed to the application's kill action -->
                <asp:Button ID="btnKill" runat="server" Text="Kill" CssClass="medium alert button" OnClick="btnKill_Click"/>
            </div>
        </div>

        <script src="js/jquery.js"></script>
        <script src="js/foundation.min.js"></script>
        <script>
            $(document).foundation();
        </script>


    </div>
    </form>
</body>
</html>
