using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

/// <summary>
/// Summary description for Attendee
/// </summary>
public class Attendee
{
    public Attendee()
    {
    }
    public long ID
    {
        get;
        set;
    }
    public String FirstName
    {
        get;
        set;
    }
    public String LastName
    {
        get;
        set;
    }
    public string Address
    {
        get;
        set;
    }
    public string City
    {
        get;
        set;
    }

    public string State
    {
        get;
        set;
    }

    [DisplayName("Zip Code")]
    public string ZipCode
    {
        get;
        set;
    }

    public string Phone
    {
        get;
        set;
    }
    
    [DisplayName("Email Address")]
    public string EmailAddress
    {
        get;
        set;
    }
}