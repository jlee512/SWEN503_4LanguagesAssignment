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
                            Console.WriteLine("Sorry, we weren't able to remove your contact from the list, please try again");
                            Console.WriteLine();
                        }
                        break;
                    case 3:
                        //Update a contact

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
            Console.WriteLine("\t 7) Please type the number of your selection and hit ENTER");
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
            int selection;

            bool inputCheck = Int32.TryParse(input, out selection);

            if (inputCheck)
            {
                if (selection < contacts.Count && selection > 0)
                {
                    string removedName = ContactsDAO.RemoveAContact(contacts[selection - 1]);
                    return removedName;
                }
            }
            return null;
        }
    }
}