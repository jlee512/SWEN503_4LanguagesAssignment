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

            var sqlCommand = new MySqlCommand("SELECT c.*, g.group_name FROM contact AS c, contact_group AS g, group_link AS gl WHERE c.contact_id = gl.contact_id AND g.group_id = gl.group_id;", db.Connection);
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
                            newContact.groups = groups;
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
                    newContact.groups = groups;
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

    }
}
