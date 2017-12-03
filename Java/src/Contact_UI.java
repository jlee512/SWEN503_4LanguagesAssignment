
import com.mysql.fabric.xmlrpc.base.Array;

import java.util.ArrayList;

/**
 * Created by Julian on 30/11/2017.
 */
public class Contact_UI {

    public static ArrayList<Contact> contacts_in_focus = new ArrayList<>();
    public static ArrayList<Contact> searchedContacts = new ArrayList<>();

    public static void main(String[] args) {
        boolean session = true;
        //Present welcome screen and initial list of all contacts available
        initialScreen();

        while (session) {
            int selection = topMenuSelection();

            switch (selection) {
                case 1:
                    // Add a contact
                    String contact_name = addContactToList();
                    if (contact_name != null) {
                        System.out.println("--------------------------------------------------");
                        System.out.println("Thanks, " + contact_name + " has been added successfully");
                        System.out.println("--------------------------------------------------");
                        System.out.println();
                    } else {
                        System.out.println("Sorry we weren't able to add your contact to the list, please try again");
                        System.out.println();
                    }
                    break;
                case 2:
                    // Remove a contact
                    String removed_contact_name = removeAContact();
                    if (removed_contact_name != null) {
                        System.out.println("--------------------------------------------------");
                        System.out.println("Thanks, " + removed_contact_name + " has been removed");
                        System.out.println("--------------------------------------------------");
                        System.out.println();
                    } else {
                        System.out.println("Sorry we weren't able to remove your contact from the list, please try again");
                        System.out.println();
                    }
                    break;
                case 3:
                    // Update a contact
                    String updatedContactName = updateAContact();
                    if (updatedContactName != null) {
                        System.out.println("--------------------------------------------------");
                        System.out.println("Thanks, " + updatedContactName + " has been updated successfully");
                        System.out.println("--------------------------------------------------");
                        System.out.println();
                    } else {
                        System.out.println("Sorry we weren't able to update your contact, please try again");
                        System.out.println();
                    }
                    break;
                case 4:
                    // Search for a contact
                    searchAContactByName();
                    break;
                case 5:
                    //Print all contacts
                    printAllContacts();
                    break;
                case 6:
                    //Quit
                    session = false;
                    System.out.println("Thanks for using the contacts viewer");
                    return;
                case 7:
                    System.out.println("You have not entered a number corresponding to a menu item, please try again");
                    continue;
                case 8:
                    System.out.println("You have not entered a number within the list of menu items (1 to 6), please try again");
                    continue;
            }

        }
    }

    public static ArrayList<Contact> printAllContacts() {
        contacts_in_focus = Contacts_DAO.getAll();

        if (contacts_in_focus != null) {
            System.out.println("Contacts: ");
            System.out.println("--------");
            for (int i = 0; i < contacts_in_focus.size(); i++) {
                Contact contact = contacts_in_focus.get(i);
                System.out.print("" + (i + 1) + ") ");
                contact.printToConsole();
            }
        } else {
            System.out.println("Looks like you haven't added any contacts yet...");
        }

        return contacts_in_focus;
    }

    public static ArrayList<Contact> printSearchResults(String name) {
        searchedContacts = Contacts_DAO.searchByName(name);

        if (searchedContacts != null) {
            System.out.println("Contacts: ");
            System.out.println("--------");
            for (int i = 0; i < searchedContacts.size(); i++) {
                Contact contact = searchedContacts.get(i);
                System.out.print("" + (i + 1) + ") ");
                contact.printToConsole();
            }
        } else {
            System.out.println("Sorry we couldn't find any contacts with that name");
            System.out.println("Please try different spelling or view all contacts at the main menu");
        }

        return searchedContacts;
    }

    public static void initialScreen() {

        System.out.println("*******************************************");
        System.out.println("\t Welcome to your contacts viewer \t");
        System.out.println("*******************************************");

        printAllContacts();

    }

    public static int topMenuSelection() {

        int selection = -1;

        System.out.println("-------------------------------------------");
        System.out.println("\t What would you like to do? \t");
        System.out.println("-------------------------------------------");

        System.out.println("\t 1) Add a new contact \t");
        System.out.println("\t 2) Delete a contact \t");
        System.out.println("\t 3) Update a contact \t");
        System.out.println("\t 4) Search your contacts list by name \t");
        System.out.println("\t 5) View all contacts \t");
        System.out.println("\t 6) Quit \t");
        System.out.println("\t Please type the number of your selection and hit ENTER \t");
        System.out.print("\t");

        String input = Keyboard.readInput();

        //Process the user's menu selection -> if less than 6 (and above 0), a menu item has been selected, otherwise return error messaging codes 6 (invalid input e.g. string) or 7 (invalid integer option)
        try {
            selection = Integer.parseInt(input);
            if (selection < 7 && selection > 0) {
                return selection;
            } else {
                return 8;
            }
        } catch (NumberFormatException e) {
            return 7;
        }
    }

    public static String updateAContact() {

        System.out.println("-------------------------------------------");
        System.out.println("\t Update a contact \t");
        System.out.println("-------------------------------------------");

        ArrayList<Contact> contacts = printAllContacts();

        System.out.println("\t Please enter the number of the contact you would like to update and hit ENTER \t");
        System.out.print("\t");

        String input = Keyboard.readInput();
        int selection;

        try {
            selection = Integer.parseInt(input);
            if (selection <= contacts.size() && selection > 0) {
                Contact editContact = contacts.get(selection - 1);
                System.out.println("\t Updating contact: " + editContact.getName());
                System.out.println("\t ");

                String name = "";
                boolean name_check = false;
                String email;
                String phone_number;
                String group_to_add = "";
                ArrayList<String> groups = new ArrayList<>();
                boolean groups_finished = false;

                //Get name input
                while (!name_check) {
                    System.out.println("\t Please enter a contact name and hit ENTER \t");
                    System.out.println("\t Just hit ENTER to leave name unchanged \t");
                    System.out.print("\t");
                    name = Keyboard.readInput();
                    if (name.length() == 0) {
                        System.out.println();
                        System.out.println("\t\t Name will not be changed");
                        System.out.println();
                        name_check = true;
                        name = editContact.getName();

                    } else {
                        System.out.println();
                        System.out.println("\t\t Name will be changed to: " + name);
                        System.out.println();
                        name_check = true;
                    }
                }

                //Get email input
                System.out.println("\t Please enter a contact email and hit ENTER \t");
                System.out.println("\t [To leave unchanged, just hit ENTER] \t");
                System.out.print("\t");
                email = Keyboard.readInput();

                if (email.length() == 0) {
                    System.out.println();
                    System.out.println("\t\t Email will not be changed");
                    System.out.println();
                    email = editContact.getEmail();
                } else {
                    System.out.println();
                    System.out.println("\t\t Email will be changed to: " + email);
                    System.out.println();
                }

                //Get phone number input
                System.out.println("\t Please enter a contact phone_number and hit ENTER \t");
                System.out.println("\t [To leave unchanged, just hit ENTER] \t");
                System.out.print("\t");
                phone_number = Keyboard.readInput();

                if (phone_number.length() == 0) {
                    System.out.println();
                    System.out.println("\t\t Phone number will not be changed");
                    System.out.println();
                    phone_number = editContact.getPhone_number();
                } else {
                    System.out.println();
                    System.out.println("\t\t Phone number will be changed to: " + phone_number);
                    System.out.println();
                }

                //Get groups input
                while(!groups_finished) {
                    System.out.println("\t Current groups are: ");
                    System.out.print("\t\t");
                    for (int i = 0; i < editContact.getGroups().size(); i++) {
                        System.out.print("| " + editContact.getGroups().get(i) + " |");
                    }
                    System.out.println();
                    System.out.println();

                    System.out.println("\t To add to these, press 1 and hit ENTER \t");
                    System.out.println("\t To start with no groups, press 2 and hit ENTER \t");
                    System.out.print("\t");

                    input = Keyboard.readInput();

                    try {
                        selection = Integer.parseInt(input);
                        if (selection < 3 && selection > 0) {
                            switch (selection) {
                                case 1:
                                    groups = editContact.getGroups();
                                    //Get groups input
                                    while (!groups_finished) {
                                        System.out.println("\t Please enter a group name and hit ENTER \t");
                                        try {
                                            System.out.print("\t");
                                            group_to_add = Keyboard.readInput();
                                            if (group_to_add.length() == 0) {
                                                throw new InvalidInputException("name length is zero");
                                            } else {
                                                groups.add(group_to_add);
                                                System.out.println("\t Group added successfully \t");
                                                System.out.println("\t To add another group, type '1' and hit ENTER, otherwise hit ENTER");
                                                System.out.print("\t");
                                                String add_more = Keyboard.readInput();

                                                if (add_more.equals("1")) {
                                                    groups_finished = false;
                                                } else {
                                                    groups_finished = true;
                                                }
                                            }
                                        } catch (InvalidInputException e) {
                                            System.out.println("\t No group name entered, please try again \t");
                                        }
                                    }
                                    break;
                                case 2:
                                    //Get groups input
                                    while (!groups_finished) {
                                        System.out.println("\t Please enter a group name and hit ENTER \t");
                                        try {
                                            System.out.print("\t");
                                            group_to_add = Keyboard.readInput();
                                            if (group_to_add.length() == 0) {
                                                throw new InvalidInputException("name length is zero");
                                            } else {
                                                groups.add(group_to_add);
                                                System.out.println("\t Group added successfully \t");
                                                System.out.println("\t To add another group, type '1' and hit ENTER, otherwise hit ENTER");
                                                System.out.print("\t");
                                                String add_more = Keyboard.readInput();

                                                if (add_more.equals("1")) {
                                                    groups_finished = false;
                                                } else {
                                                    groups_finished = true;
                                                }
                                            }
                                        } catch (InvalidInputException e) {
                                            System.out.println("\t No group name entered, please try again \t");
                                        }
                                    }
                                    break;
                            }

                        } else {
                            System.out.println("\t Invalid number entered, please try again...");
                        }
                    } catch (NumberFormatException e) {
                        System.out.println("\t Invalid input, please select a menu item number. Please try again...");
                    }
                }

                Contact contact = new Contact(name, phone_number, email);
                contact.setGroups(groups);
                String editedContactName = Contacts_DAO.updateAContact(contact, editContact.getName());

                if (editedContactName != null) {
                    return editedContactName;
                } else {
                    return null;
                }
            } else {
                return null;
            }
        } catch (NumberFormatException e) {
            return null;
        }

    }

    public static boolean searchAContactByName() {

        System.out.println("-------------------------------------------");
        System.out.println("\t Search for a contact by name \t");
        System.out.println("-------------------------------------------");

        System.out.println("\t Please enter the name of the contact you would like to search for and hit ENTER \t");
        System.out.print("\t");

        String input = Keyboard.readInput();

        ArrayList<Contact> searchResults = printSearchResults(input);

        if (searchResults != null) {
            return true;
        } else {
            return false;
        }
    }

    public static String removeAContact() {

        System.out.println("-------------------------------------------");
        System.out.println("\t Remove a contact \t");
        System.out.println("-------------------------------------------");

        ArrayList<Contact> contacts = printAllContacts();

        System.out.println("\t Please enter the number of the contact you would like to remove and hit ENTER \t");
        System.out.print("\t");

        String input = Keyboard.readInput();
        int selection;

        try {
            selection = Integer.parseInt(input);
            if (selection <= contacts.size() && selection > 0) {
                String removed_name = Contacts_DAO.removeAContact(contacts.get(selection - 1));
                return removed_name;
            } else {
                return null;
            }
        } catch (NumberFormatException e) {
            return null;
        }
    }

    public static String addContactToList() {

        String name = "";
        boolean name_check = false;
        String email;
        String phone_number;
        String group_to_add = "";
        ArrayList<String> groups = new ArrayList<>();
        boolean groups_finished = false;

        System.out.println("-------------------------------------------");
        System.out.println("\t Add a contact \t");
        System.out.println("-------------------------------------------");

        //Get name input
        while (!name_check) {
            System.out.println("\t Please enter a contact name and hit ENTER \t");
            try {
                System.out.print("\t");
                name = Keyboard.readInput();
                if (name.length() == 0) {
                    throw new InvalidInputException("name length is zero");
                } else {
                    name_check = true;
                }
            } catch (InvalidInputException e) {
                System.out.println("\t No contact name entered, please try again \t");
            }
        }

        //Get email input
        System.out.println("\t Please enter a contact email and hit ENTER \t");
        System.out.println("\t [To leave email empty, just hit ENTER] \t");
        System.out.print("\t");
        email = Keyboard.readInput();

        //Get phone number input
        System.out.println("\t Please enter a contact phone_number and hit ENTER \t");
        System.out.println("\t [To leave phone_number empty, just hit ENTER] \t");
        System.out.print("\t");
        phone_number = Keyboard.readInput();

        //Get groups input
        while (!groups_finished) {
            System.out.println("\t Please enter a group name and hit ENTER \t");
            try {
                System.out.print("\t");
                group_to_add = Keyboard.readInput();
                if (group_to_add.length() == 0) {
                    throw new InvalidInputException("name length is zero");
                } else {
                    groups.add(group_to_add);
                    System.out.println("\t Group added successfully \t");
                    System.out.println("\t To add another group, type '1' and hit ENTER, otherwise hit ENTER");
                    System.out.print("\t");
                    String add_more = Keyboard.readInput();

                    if (add_more.equals("1")) {
                        groups_finished = false;
                    } else {
                        groups_finished = true;
                    }
                }
            } catch (InvalidInputException e) {
                System.out.println("\t No group name entered, please try again \t");
            }
        }

        Contact contact = new Contact(name, email, phone_number);
        contact.setGroups(groups);
        boolean add_status = Contacts_DAO.addContact(contact);

        if (add_status) {
            return name;
        } else {
            return "";
        }
    }
}
