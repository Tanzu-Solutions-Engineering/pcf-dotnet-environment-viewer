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


        <nav class="top-bar">
            <ul class="title-area">
                <li class="name">
                    <h1><a href="./">Pivotal CF</a></h1>
                </li>
            </ul>
        </nav>
        <div class="row">
            <div class="large-12 columns">
                <h3>Welcome to Pivotal CF</h3>
                <hr />
            </div>

            <div class="row" runat="server" visible="false" id="attendeePane">
                <div class="large-12 columns">
                    <h3>All Attendees</h3>
                    <hr />
                    <asp:GridView ID="gridAttendees" runat="server"
                        AutoGenerateColumns="false"
                        EmptyDataText="No attendees found."
                        DataSourceID="AttendeeDataSource"
                        EnableViewState="true">
                        <Columns>
                            <asp:BoundField HeaderText="Attendee ID" DataField="id" InsertVisible="false" ReadOnly="true" Visible="false" />
                            <asp:BoundField HeaderText="First Name" DataField="first_name" />
                            <asp:BoundField HeaderText="Last Name" DataField="last_name" />
                            <asp:BoundField HeaderText="Address" DataField="address" />
                            <asp:BoundField HeaderText="City" DataField="city" />
                            <asp:BoundField HeaderText="State" DataField="state" />
                            <asp:BoundField HeaderText="Zip Code" DataField="zip_code" />
                            <asp:BoundField HeaderText="Phone" DataField="phone_number" />
                            <asp:BoundField HeaderText="Email Address" DataField="email_address" />
                            <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                        </Columns>
                    </asp:GridView>
                    <asp:Button ID="btnAddAttendee" runat="server" Text="Add Attendee"  CssClass="medium button" OnClick="btnAddAttendee_Click" />
                </div>
                <asp:SqlDataSource
                    ID="AttendeeDataSource"
                    runat="server"
                    DataSourceMode="DataReader"></asp:SqlDataSource>
            </div>
            <div class="row">
                <div class="large-12 columns">
                    <div class="panel">
                        <h4>Environment Information:</h4>
                        <br />
                        <p>
                            The current server time is <em>
                                <asp:Label ID="lblTime" runat="server"></asp:Label></em>
                        </p>
                        <p>
                            The current .NET version is <em>
                                <asp:Label ID="lblDotNetVersion" runat="server"></asp:Label></em>
                        </p>
                        <p>
                            The application port is <em>
                                <asp:Label ID="lblPort" runat="server"></asp:Label></em>
                        </p>
                        <p>
                            The instance ID is <em>
                                <asp:Label ID="lblInstanceID" runat="server"></asp:Label></em>
                        </p>
                        <p>
                            The instance index is <em>
                                <asp:Label ID="lblInstanceIndex" runat="server"></asp:Label></em>
                        </p>
                        <p>
                            The instance was started at <em>
                                <asp:Label ID="lblInstanceStart" runat="server"></asp:Label></em>
                        </p>
                        <p>
                            The bound services are <em>
                                <asp:Label ID="lblBoundServices" runat="server"></asp:Label></em>
                        </p>
                        <p>
                            The detected DB engine is <em>
                                <asp:Label ID="lblDbEngine" runat="server"></asp:Label></em>
                        </p>
                    </div>
                    <hr />
                </div>
            </div>

            <div class="row">
                <div class="large-12 columns">
                    <!-- This simply refreshes the page to demonstrate changed (or unchanged) ports -->
                    <a href="javascript:window.location.reload();" class="medium button">Refresh</a>
                    <!-- This link should be changed to the application's kill action -->
                    <asp:Button ID="btnKill" runat="server" Text="Kill" CssClass="medium alert button" OnClick="btnKill_Click" />
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
