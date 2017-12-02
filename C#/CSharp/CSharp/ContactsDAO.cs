using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace CSharp
{
    class ContactsDAO
    {
        public static List<Contact> getAll()
        {
            List<Contact> allContacts = new List<Contact>();

            MySqlDb db = new MySqlDb();

            db.Connect();

            var sqlCommand =
                new MySqlCommand(
                    "SELECT c.*, g.group_name FROM contact AS c, contact_group AS g, group_link AS gl WHERE c.contact_id = gl.contact_id AND g.group_id = gl.group_id;",
                    db.Connection);
            try
            {
                var resultsReader = sqlCommand.ExecuteReader();
                string prevName = "";
                Contact newContact = null;
                List<string> groups = null;
                while (resultsReader.Read())
                {
                    if (prevName.Length == 0 || !prevName.Equals(resultsReader.GetString("name")))
                    {
                        if (prevName.Length != 0)
                        {
                            newContact.Groups = groups;
                            allContacts.Add(newContact);
                        }

                        string name = resultsReader.GetString("name");
                        string email = resultsReader.GetString("email");
                        string phoneNumber = resultsReader.GetString("phone_number");
                        string group = resultsReader.GetString("group_name");

                        groups = new List<string>();
                        groups.Add(group);

                        prevName = name;
                        newContact = new Contact(name, email, phoneNumber);
                    }
                    else
                    {
                        string group = resultsReader.GetString("group_name");
                        groups.Add(group);
                    }
                }
                if (newContact != null)
                {
                    newContact.Groups = groups;
                    allContacts.Add(newContact);
                }
            }
            catch (InvalidOperationException e)
            {
                //String formatting difference
                Console.WriteLine("Exception caught: {0}", e);
            }
            finally
            {
                db.Close();
            }

            if (allContacts.Count > 0)
            {
                return allContacts;
            }
            else
            {
                return null;
            }
        }

        public static List<Contact> searchByName(string searchName)
        {
            List<Contact> searchContacts = new List<Contact>();

            MySqlDb db = new MySqlDb();

            db.Connect();

            var sqlCommand =
                new MySqlCommand(
                    "SELECT c.*, g.group_name FROM contact AS c, contact_group AS g, group_link AS gl WHERE c.contact_id = gl.contact_id AND g.group_id = gl.group_id AND c.name LIKE @SEARCH;",
                    db.Connection);
            sqlCommand.Parameters.Add("@SEARCH", MySqlDbType.String);
            sqlCommand.Parameters["@SEARCH"].Value = searchName + "%";

            try
            {
                var resultsReader = sqlCommand.ExecuteReader();
                string prevName = "";
                Contact newContact = null;
                List<string> groups = null;
                while (resultsReader.Read())
                {
                    if (prevName.Length == 0 || !prevName.Equals(resultsReader.GetString("name")))
                    {
                        if (prevName.Length != 0)
                        {
                            newContact.Groups = groups;
                            searchContacts.Add(newContact);
                        }

                        string name = resultsReader.GetString("name");
                        string email = resultsReader.GetString("email");
                        string phoneNumber = resultsReader.GetString("phone_number");
                        string group = resultsReader.GetString("group_name");

                        groups = new List<string>();
                        groups.Add(group);

                        prevName = name;
                        newContact = new Contact(name, email, phoneNumber);
                    }
                    else
                    {
                        string group = resultsReader.GetString("group_name");
                        groups.Add(group);
                    }
                }
                if (newContact != null)
                {
                    newContact.Groups = groups;
                    searchContacts.Add(newContact);
                }
            }
            catch (InvalidOperationException e)
            {
                //String formatting difference
                Console.WriteLine("Exception caught: {0}", e);
            }
            finally
            {
                db.Close();
            }

            if (searchContacts.Count > 0)
            {
                return searchContacts;
            }
            else
            {
                return null;
            }
        }

        public static string RemoveAContact(Contact contact)
        {
            MySqlDb db = new MySqlDb();

            db.Connect();


            var sqlCommand = new MySqlCommand("DELETE FROM contact WHERE name = @DELETENAME;", db.Connection);
            sqlCommand.Parameters.Add("@DELETENAME", MySqlDbType.String);
            sqlCommand.Parameters["@DELETENAME"].Value = contact.Name;

            try
            {
                var executeUpdate = sqlCommand.ExecuteNonQuery();

                return contact.Name;
            }
            catch (InvalidOperationException e)
            {
                //String formatting difference
                Console.WriteLine("Exception caught: {0}", e);
            }
            finally
            {
                db.Close();
            }
            return "";
        }

        public static bool AddContact(Contact contact)
        {
            MySqlDb db = new MySqlDb();

            db.Connect();
            MySqlCommand sqlCommand;

            try
            {
                for (int i = 0; i < contact.Groups.Count; i++)
                {
                    sqlCommand = new MySqlCommand("INSERT IGNORE INTO contact_group(group_name) VALUES (@GROUPNAME);",
                        db.Connection);
                    sqlCommand.Parameters.Add("@GROUPNAME", MySqlDbType.String);
                    sqlCommand.Parameters["@GROUPNAME"].Value = contact.Groups[i];

                    try
                    {
                        var executeUpdate = sqlCommand.ExecuteNonQuery();
                    }
                    catch (InvalidOperationException e)
                    {
                        //String formatting difference
                        Console.WriteLine("Groups");
                        Console.WriteLine("Exception caught: {0}", e);
                    }
                }

                sqlCommand =
                    new MySqlCommand(
                        "INSERT IGNORE INTO contact(name, email, phone_number) VALUES (@CONTACTNAME, @CONTACTEMAIL, @CONTACTPHONE);",
                        db.Connection);
                sqlCommand.Parameters.Add("@CONTACTNAME", MySqlDbType.String);
                sqlCommand.Parameters["@CONTACTNAME"].Value = contact.Name;
                sqlCommand.Parameters.Add("@CONTACTEMAIL", MySqlDbType.String);
                sqlCommand.Parameters["@CONTACTEMAIL"].Value = contact.Email;
                sqlCommand.Parameters.Add("@CONTACTPHONE", MySqlDbType.String);
                sqlCommand.Parameters["@CONTACTPHONE"].Value = contact.PhoneNumber;

                try
                {
                    var executeUpdate = sqlCommand.ExecuteNonQuery();
                }
                catch (InvalidOperationException e)
                {
                    //String formatting difference
                    Console.WriteLine("Contact");
                    Console.WriteLine("Exception caught: {0}", e);
                }

                for (int i = 0; i < contact.Groups.Count; i++)
                {
                    sqlCommand = new MySqlCommand(
                        "INSERT IGNORE INTO group_link(contact_id, group_id) VALUES ((SELECT contact_id FROM contact WHERE name = @CONTACTNAME), (SELECT group_id FROM contact_group WHERE group_name = @GROUPNAME));",
                        db.Connection);
                    sqlCommand.Parameters.Add("@CONTACTNAME", MySqlDbType.String);
                    sqlCommand.Parameters["@CONTACTNAME"].Value = contact.Name;
                    sqlCommand.Parameters.Add("@GROUPNAME", MySqlDbType.String);
                    sqlCommand.Parameters["@GROUPNAME"].Value = contact.Groups[i];

                    try
                    {
                        var executeUpdate = sqlCommand.ExecuteNonQuery();
                    }
                    catch (InvalidOperationException e)
                    {
                        //String formatting difference
                        Console.WriteLine("Group linkages");
                        Console.WriteLine("Exception caught: {0}", e);
                    }
                }

                return true;
            } finally
            {
                db.Close();
            }
        }

        public static string UpdateAContact(Contact contact, string originalName)
        {

            MySqlDb db = new MySqlDb();

            db.Connect();
            MySqlCommand sqlCommand;

            try
            {

                for (int i = 0; i < contact.Groups.Count; i++)
                {

                    sqlCommand = new MySqlCommand("INSERT IGNORE INTO contact_group(group_name) VALUES (@GROUPNAME);",
                        db.Connection);
                    sqlCommand.Parameters.Add("@GROUPNAME", MySqlDbType.String);
                    sqlCommand.Parameters["@GROUPNAME"].Value = contact.Groups[i];

                    try
                    {
                        var executeUpdate = sqlCommand.ExecuteNonQuery();
                    }
                    catch (InvalidOperationException e)
                    {
                        //String formatting difference
                        Console.WriteLine("Groups");
                        Console.WriteLine("Exception caught: {0}", e);
                    }

                }

                sqlCommand =
                    new MySqlCommand(
                        "UPDATE contact SET name = @CONTACTNAME, email = @CONTACTEMAIL, phone_number = @CONTACTPHONE WHERE name = @ORIGINALNAME;",
                        db.Connection);
                sqlCommand.Parameters.Add("@CONTACTNAME", MySqlDbType.String);
                sqlCommand.Parameters["@CONTACTNAME"].Value = contact.Name;
                sqlCommand.Parameters.Add("@CONTACTEMAIL", MySqlDbType.String);
                sqlCommand.Parameters["@CONTACTEMAIL"].Value = contact.Email;
                sqlCommand.Parameters.Add("@CONTACTPHONE", MySqlDbType.String);
                sqlCommand.Parameters["@CONTACTPHONE"].Value = contact.PhoneNumber;
                sqlCommand.Parameters.Add("@ORIGINALNAME", MySqlDbType.String);
                sqlCommand.Parameters["@ORIGINALNAME"].Value = originalName;

                try
                {
                    var executeUpdate = sqlCommand.ExecuteNonQuery();
                }
                catch (InvalidOperationException e)
                {
                    //String formatting difference
                    Console.WriteLine("Contact");
                    Console.WriteLine("Exception caught: {0}", e);
                }

                sqlCommand =
                    new MySqlCommand(
                        "DELETE FROM group_link WHERE contact_id = (SELECT contact_id FROM contact WHERE name = @CONTACTNAME);",
                        db.Connection);
                sqlCommand.Parameters.Add("@CONTACTNAME", MySqlDbType.String);
                sqlCommand.Parameters["@CONTACTNAME"].Value = originalName;

                try
                {
                    var executeUpdate = sqlCommand.ExecuteNonQuery();
                }
                catch (InvalidOperationException e)
                {
                    //String formatting difference
                    Console.WriteLine("Remove Contact Group Linkages");
                    Console.WriteLine("Exception caught: {0}", e);
                }

                for (int i = 0; i < contact.Groups.Count; i++)
                {
                    sqlCommand = new MySqlCommand(
                        "INSERT IGNORE INTO group_link(contact_id, group_id) VALUES ((SELECT contact_id FROM contact WHERE name = @CONTACTNAME), (SELECT group_id FROM contact_group WHERE group_name = @GROUPNAME));",
                        db.Connection);
                    sqlCommand.Parameters.Add("@CONTACTNAME", MySqlDbType.String);
                    sqlCommand.Parameters["@CONTACTNAME"].Value = contact.Name;
                    sqlCommand.Parameters.Add("@GROUPNAME", MySqlDbType.String);
                    sqlCommand.Parameters["@GROUPNAME"].Value = contact.Groups[i];

                    try
                    {
                        var executeUpdate = sqlCommand.ExecuteNonQuery();
                    }
                    catch (InvalidOperationException e)
                    {
                        //String formatting difference
                        Console.WriteLine("Group linkages");
                        Console.WriteLine("Exception caught: {0}", e);
                    }
                }
                return contact.Name;
            }
            finally
            {
                db.Close();
            }

            return null;

        }
    }
}