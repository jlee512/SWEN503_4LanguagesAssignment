using System;
using System.Collections.Generic;

namespace CSharp
{
    class ContactUi
    {
        public static List<Contact> contactsInFocus = new List<Contact>();
        public static List<Contact> searchedContacts = new List<Contact>();

        public static void Main(string[] args)
        {
            bool session = true;

            InitialScreen();

            while (session)
            {
                int selection = TopMenuSelection();

                switch (selection)
                {
                    case 1:
                        //Add a contact
                        string contactName = AddContactToList();
                        if (contactName != null)
                        {
                            Console.WriteLine("-------------------------------------------------");
                            Console.WriteLine("Thanks, " + contactName + " has been added successfully");
                            Console.WriteLine("-------------------------------------------------");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Sorry we weren't able to add your contact to the list, please try again");
                            Console.WriteLine();
                        }
                        break;
                    case 2:
                        //Remove a contact
                        string removeContactName = RemoveAContact();
                        if (removeContactName != null)
                        {
                            Console.WriteLine("-------------------------------------------------");
                            Console.WriteLine("Thanks, " + removeContactName + " has been removed");
                            Console.WriteLine("-------------------------------------------------");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine(
                                "Sorry, we weren't able to remove your contact from the list, please try again");
                            Console.WriteLine();
                        }
                        break;
                    case 3:
                        //Update a contact
                        string updatedContactName = UpdateAContact();
                        if (updatedContactName != null)
                        {
                            Console.WriteLine("-------------------------------------------------");
                            Console.WriteLine("Thanks, " + updatedContactName + " has been updated successfully");
                            Console.WriteLine("-------------------------------------------------");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine("Sorry we weren't able to update your contact, please try again");
                            Console.WriteLine();
                        }
                        break;
                    case 4:
                        //Search for a contact
                        searchAContactByName();
                        break;
                    case 5:
                        //Print all contacts
                        PrintAllContacts();
                        break;
                    case 6:
                        //Quit
                        session = false;
                        Console.WriteLine("Thanks for using the contacts viewer");
                        return;
                    case 7:
                        Console.WriteLine(
                            "You have not entered a number corresponding to a menu item, please try again");
                        continue;
                    case 8:
                        Console.WriteLine(
                            "You have not entered a number within the list of menu items (1 to 6), please try again");
                        continue;
                }
            }
        }

        public static List<Contact> PrintAllContacts()
        {
            contactsInFocus = ContactsDAO.getAll();

            if (contactsInFocus != null)
            {
                Console.WriteLine("Contacts: ");
                Console.WriteLine("---------");

                for (int i = 0; i < contactsInFocus.Count; i++)
                {
                    Contact contact = contactsInFocus[i];

                    Console.Write("" + (i + 1) + ") ");
                    contact.PrintToConsole();
                }
            }
            else
            {
                Console.WriteLine("Looks like you haven't added any contacts yet...");
            }

            return contactsInFocus;
        }

        public static List<Contact> PrintSearchResults(string name)
        {
            searchedContacts = ContactsDAO.searchByName(name);

            if (searchedContacts != null)
            {
                Console.WriteLine("Contacts: ");
                Console.WriteLine("---------");

                for (int i = 0; i < searchedContacts.Count; i++)
                {
                    Contact contact = searchedContacts[i];
                    Console.Write("" + (i + 1) + ") ");
                    contact.PrintToConsole();
                }
            }
            else
            {
                Console.WriteLine("Sorry we couldn't find any contacts with that name");
                Console.WriteLine("Please try a different spelling or view all contacts at the main menu");
            }

            return searchedContacts;
        }

        static void InitialScreen()
        {
            Console.WriteLine("*************************************************");
            Console.WriteLine("\tWelcome to your contacts viewer");
            Console.WriteLine("*************************************************");

            PrintAllContacts();
        }

        public static int TopMenuSelection()
        {
            int selection = -1;

            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("\t What would you like to do?");
            Console.WriteLine("-------------------------------------------------");

            Console.WriteLine("\t 1) Add a new contact");
            Console.WriteLine("\t 2) Delete a contact");
            Console.WriteLine("\t 3) Update a contact");
            Console.WriteLine("\t 4) Search your contacts list by name");
            Console.WriteLine("\t 5) View all contacts");
            Console.WriteLine("\t 6) Quit");
            Console.WriteLine("\t Please type the number of your selection and hit ENTER");
            Console.WriteLine("\t");

            string input = Console.ReadLine();

            bool inputCheck = Int32.TryParse(input, out selection);

            if (inputCheck)
            {
                if (selection < 7 && selection > 0)
                {
                    return selection;
                }
                return 8;
            }
            return 7;
        }

        public static bool searchAContactByName()
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("\t Search for a contact by name");
            Console.WriteLine("-------------------------------------------------");

            Console.WriteLine("\t Please enter the name of the contact you would like to search for and hit ENTER");
            Console.WriteLine("\t");

            string input = Console.ReadLine();

            List<Contact> searchResults = PrintSearchResults(input);

            if (searchResults != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string RemoveAContact()
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("\t Remove a contact");
            Console.WriteLine("-------------------------------------------------");

            List<Contact> contacts = PrintAllContacts();

            Console.WriteLine("\t Please enter the number of the contact you would like to remove and hit ENTER");
            Console.WriteLine("\t");

            string input = Console.ReadLine();

            bool inputCheck = Int32.TryParse(input, out var selection);

            if (inputCheck)
            {
                if (selection <= contacts.Count && selection > 0)
                {
                    string removedName = ContactsDAO.RemoveAContact(contacts[selection - 1]);
                    return removedName;
                }
            }
            return null;
        }

        public static string AddContactToList()
        {
            string name = "";
            bool nameCheck = false;
            string email;
            string phoneNumber;
            string groupToAdd = "";
            List<String> groups = new List<String>();
            bool groupsFinished = false;

            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("\t Add a contact");
            Console.WriteLine("-------------------------------------------------");

            while (!nameCheck)
            {
                Console.WriteLine("\t Please enter a name and hit ENTER");

                Console.WriteLine("\t");
                name = Console.ReadLine();
                if (name.Length == 0)
                {
                    Console.WriteLine("\t No contact name entered, please try again");
                }
                else
                {
                    nameCheck = true;
                }
            }

            Console.WriteLine("\t Please enter a contact email and hit ENTER");
            Console.WriteLine("\t [To leave email empty, just hit ENTER]");
            Console.WriteLine("\t");

            email = Console.ReadLine();

            Console.WriteLine("\t Please enter a contact phone number and hit ENTER");
            Console.WriteLine("\t [To leave phone number empty, just hit ENTER]");
            Console.WriteLine("\t");

            phoneNumber = Console.ReadLine();

            while (!groupsFinished)
            {
                Console.WriteLine("\t Please enter a group name and hit ENTER");
                Console.WriteLine("\t");

                groupToAdd = Console.ReadLine();
                if (groupToAdd.Length == 0)
                {
                    Console.WriteLine("\t No group name entered, please try again");
                }
                else
                {
                    groups.Add(groupToAdd);
                    Console.WriteLine("\t Group added successfully");
                    Console.WriteLine("\t To add another group, type '1' and hit ENTER, otherwise hit ENTER");
                    Console.WriteLine("\t");

                    string addMore = Console.ReadLine();

                    if (addMore.Equals("1"))
                    {
                        groupsFinished = false;
                    }
                    else
                    {
                        groupsFinished = true;
                    }
                }
            }

            Contact contact = new Contact(name, email, phoneNumber);
            contact.Groups = groups;
            bool addStatus = ContactsDAO.AddContact(contact);

            if (addStatus)
            {
                return name;
            }
            else
            {
                return "";
            }

        }

        public static string UpdateAContact()
        {

            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("\t Update a contact");
            Console.WriteLine("-------------------------------------------------");

            List<Contact> contacts = PrintAllContacts();

            Console.WriteLine("\t Please enter the number of the contact you would like to update and hit ENTER");
            Console.WriteLine("\t");

            string input = Console.ReadLine();

            bool inputCheck = Int32.TryParse(input, out var selection);

            if (inputCheck)
            {
                if (selection <= contacts.Count && selection > 0)
                {
                    Contact editContact = contacts[selection - 1];
                    Console.WriteLine("\t Updating contact: " + editContact.Name);
                    Console.WriteLine("\t");

                    string name = "";
                    bool nameCheck = false;
                    string email;
                    string phoneNumber;
                    string groupToAdd = "";
                    List<String> groups = new List<string>();

                    bool groupsFinished = false;

                    while (!nameCheck)
                    {
                        Console.WriteLine("\t Please enter a contact name and hit ENTER");
                        Console.WriteLine("\t Just hit ENTER to leave name unchanged");
                        Console.WriteLine("\t");

                        name = Console.ReadLine();
                        if (name.Length == 0 || name.Equals(editContact.Name))
                        {
                            Console.WriteLine();
                            Console.WriteLine("\t\t Name will not be changed");
                            Console.WriteLine();
                            nameCheck = true;
                            name = editContact.Name;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("\t\t Name will be changed to: " + name);
                            Console.WriteLine();
                            nameCheck = true;
                        }
                    }

                    Console.WriteLine("\t Please enter a contact email and hit ENTER");
                    Console.WriteLine("\t [To leave unchanged, just hit ENTER]");
                    Console.WriteLine("\t");

                    email = Console.ReadLine();

                    if (email.Length == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("\t\t Email will not be changed");
                        Console.WriteLine();
                        email = editContact.Email;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("\t\t Email will be changed to: " + email);
                        Console.WriteLine();
                    }

                    Console.WriteLine("\t Please enter a contact phone number and hit ENTER");
                    Console.WriteLine("\t [To leave unchanged, just hit ENTER]");
                    Console.WriteLine("\t");

                    phoneNumber = Console.ReadLine();

                    if (phoneNumber.Length == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("\t\t Phone number will not be changed");
                        Console.WriteLine();
                        phoneNumber = editContact.PhoneNumber;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("\t\t Phone number will be changed to: " + phoneNumber);
                        Console.WriteLine();
                    }

                    while (!groupsFinished)
                    {
                        Console.WriteLine("\t Current groups are: ");
                        Console.Write("\t\t");

                        for (int i = 0; i < editContact.Groups.Count; i++)
                        {
                            Console.Write("| " + editContact.Groups[i] + " |");
                        }
                        Console.WriteLine();
                        Console.WriteLine();

                        Console.WriteLine("\t To add to these, press 1 and hit ENTER");
                        Console.WriteLine("\t To start with no groups, press 2 and hit ENTER");
                        Console.WriteLine("\t");

                        input = Console.ReadLine();

                        bool inputCheckGroups = Int32.TryParse(input, out var selectionGroups);

                        if (inputCheckGroups)
                        {
                            if (selectionGroups < 3 && selectionGroups > 0)
                            {
                                switch (selectionGroups)
                                {
                                    case 1:
                                        groups = editContact.Groups;
                                        while (!groupsFinished)
                                        {
                                            Console.WriteLine("\t Please enter a group name and hit ENTER");
                                            Console.WriteLine("\t");

                                            groupToAdd = Console.ReadLine();
                                            if (groupToAdd.Length == 0)
                                            {
                                                Console.WriteLine("\t No group name entered, please try again");
                                            }
                                            else
                                            {
                                                groups.Add(groupToAdd);
                                                Console.WriteLine("\t Group added sucessfully");
                                                Console.WriteLine(
                                                    "\t To add another group, type '1' and hit ENTER, otherwise hit ENTER");
                                                Console.WriteLine("\t");

                                                string addMore = Console.ReadLine();

                                                if (addMore.Equals("1"))
                                                {
                                                    groupsFinished = false;
                                                }
                                                else
                                                {
                                                    groupsFinished = true;
                                                }
                                            }
                                        }
                                        break;
                                    case 2:
                                        while (!groupsFinished)
                                        {
                                            Console.WriteLine("\t Please enter a group name and hit ENTER");
                                            Console.WriteLine("\t");

                                            groupToAdd = Console.ReadLine();
                                            if (groupToAdd.Length == 0)
                                            {
                                                Console.WriteLine("\t No group name entered, please try again");
                                            }
                                            else
                                            {
                                                groups.Add(groupToAdd);
                                                Console.WriteLine("\t Group added successfully");
                                                Console.WriteLine(
                                                    "\t To add another group, type '1' and hit ENTER, otherwise hit ENTER");
                                                Console.WriteLine("\t");

                                                string addMore = Console.ReadLine();

                                                if (addMore.Equals("1"))
                                                {
                                                    groupsFinished = false;
                                                }
                                                else
                                                {
                                                    groupsFinished = true;
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("\t Invalid input, please select a menu item number. Please try again...");
                        }
                    }

                    Contact contact = new Contact(name, email, phoneNumber);
                    contact.Groups = groups;
                    string editedContactName = ContactsDAO.UpdateAContact(contact, editContact.Name);

                    if (editedContactName != null)
                    {
                        return editedContactName;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}