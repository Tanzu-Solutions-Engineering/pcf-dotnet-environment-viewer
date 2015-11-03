using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for AttendeeRepository...I know, it's Spring-like
/// </summary>
public class AttendeeRepository
{
    public AttendeeRepository()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void checkDatabaseForTables()
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

    public static List<Attendee> getAttendees()
    {
        List<Attendee> attendees = new List<Attendee>();

        using (SqlConnection conn = new SqlConnection(CurrentEnvironment.DbConnectionString))
        // execute read
        using (SqlCommand command = new SqlCommand()
        {
            CommandText = "select id, first_name, last_name, address, city, state, zip_code, phone_number, email_address from attendee",
            Connection = conn,
            CommandType = CommandType.Text
        })
        {
            conn.Open();


            IDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                /*
                 CREATE TABLE attendee (
                 id bigint IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED,
                 address varchar(255) DEFAULT NULL,
                  city varchar(255) DEFAULT NULL,
                 email_address varchar(255) DEFAULT NULL,
                 first_name varchar(255) DEFAULT NULL,
                 last_name varchar(255) DEFAULT NULL,
                 phone_number varchar(255) DEFAULT NULL,
                 state varchar(255) DEFAULT NULL,
                 zip_code varchar(255) DEFAULT NULL
                 )
                 */
                var thisAttendee = new Attendee()
                {
                    ID = reader.GetInt64(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Address = reader.GetString(3),
                    City = reader.GetString(4),
                    State = reader.GetString(5),
                    ZipCode = reader.GetString(6),
                    Phone = reader.GetString(7),
                    EmailAddress = reader.GetString(8)
                };
                //address, city, state, zip_code, phone_number, email_address

                attendees.Add(thisAttendee);
            }

        }
        return attendees;
    }

}