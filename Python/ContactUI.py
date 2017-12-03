import sys
from MySqlDb import MySqlDb
from Contact import Contact
from ContactDAO import ContactDAO

contacts_in_focus = []
searchedContacts = []

def main(args):
    session = True
    initial_screen()

    while session:
        selection = top_menu_selection()

        if selection == 1:
            # Add contact to list
            contact_name = add_contact_to_list()
            if contact_name is not None:
                print("-------------------------------------------")
                print("Thanks " + contact_name + " has been added successfully")
                print("-------------------------------------------")
                print()
            else:
                print("Sorry, we weren't able to add your contact to the list, please try again")
                print()
        elif selection == 2:
            # Remove a contact
            removed_contact_name = remove_a_contact()
            if removed_contact_name is not None:
                print("-------------------------------------------")
                print("Thanks, " + removed_contact_name + " has been removed")
                print("-------------------------------------------")
            else:
                print("Sorry we weren't able to remove your contact from the list, please try again")
                print()
        elif selection == 3:
            # Update a contact
            updated_contact_name = update_a_contact()
            if updated_contact_name is not None:
                print("-------------------------------------------")
                print("Thanks, " + updated_contact_name + " has been updated successfully")
                print("-------------------------------------------")
                print()
            else:
                print("Sorry we weren't able to update your contact, please try again")
                print()
        elif selection == 4:
            # Search for a contact
            search_contact_by_name()
        elif selection == 5:
            # print all contacts
            print_all_contacts()
        elif selection == 6:
            # Quit
            session = False
            print("Thanks for using the contacts viewer")
            return
        elif selection == 7:
            print("You have not entered a number corresponding to a menu item, please try again")
            continue
        elif selection == 8:
            print("You have not entered a number within the list of menu items (1 to 6), please try again")
            continue

def initial_screen():
    print("*******************************************")
    print("\tWelcome to your contacts viewer")
    print("*******************************************")
    print_all_contacts()

def top_menu_selection():
    selection = -1

    print("-------------------------------------------")
    print("\t What would you like to do?")
    print("-------------------------------------------")

    print("\t 1) Add a new contact")
    print("\t 2) Delete a contact")
    print("\t 3) Update a contact")
    print("\t 4) Search your contacts list by name")
    print("\t 5) View all contacts")
    print("\t 6) Quit")
    print("\t Please type the number of your selection and hit ENTER")
    print("\t", end="")

    input_string = input('')

    try:
        selection = int(input_string)
        if selection < 7 and selection > 0:
            return selection
        else:
            return 8
    except:
        return 7

def print_all_contacts():
    contacts_in_focus = ContactDAO.get_all()

    if contacts_in_focus is not None:
        print("Contacts: ")
        print("----------")
        for index, contact in enumerate(contacts_in_focus):
            print("" + str(index + 1) + ") ", end="")
            contact.print_to_console()
    else:
        print("Looks like you haven't added any contacts yet...")

    return contacts_in_focus

def search_contact_by_name():
    print("-------------------------------------------")
    print("\t Search for a contact by name")
    print("-------------------------------------------")

    print("\t Please enter the name of the contact you would like to search for and hit ENTER")
    print("\t", end="")

    input_string = input("")

    search_results = print_search_results(input_string)

def print_search_results(input):
    searchedContacts = ContactDAO.search_by_name(input)

    if searchedContacts is not None:
        print("Contacts: ")
        print("----------")

        for index, contact in enumerate(searchedContacts):
            print("" + str(index + 1) + ") ", end="")
            contact.print_to_console()
    else:
        print("Sorry we couldn't find any contacts with that name")
        print("Please try different spelling or view all contacts at the main menu")

    return searchedContacts

def add_contact_to_list():

    name = ""
    name_check = False
    email = None
    phone_number = None
    group_to_add = ""
    groups = []
    groups_finished = False

    print("-------------------------------------------")
    print("\t Add a contact")
    print("-------------------------------------------")

    while not name_check:
        print("\t Please enter a contact name and hit ENTER")
        print("\t", end="")
        name = input('')

        if len(name) == 0:
            print("\t No contact name entered, please try again")
        else:
            name_check = True

    print("\t Please enter a contact email and hit ENTER")
    print("\t [To leave email empty, just hit ENTER]")
    print("\t", end="")
    email = input('')

    print("\t Please enter a contact phone number and hit ENTER")
    print("\t", end="")
    phone_number = input('')

    while not groups_finished:
        print("\t Please enter a group name and hit ENTER")
        print("\t", end="")
        group_to_add = input('')

        if len(group_to_add) == 0:
            print("\t No group name entered, please try again")
        else:
            groups.append(group_to_add)
            print("\t Group added successfully")
            print("\t To add another group, type '1' and hit ENTER, otherwise hit ENTER")
            print("\t", end="")

            add_more = input('')
            if add_more == "1":
                groups_finished = False
            else:
                groups_finished = True

    contact = Contact(name, email, phone_number)
    contact.set_groups(groups)
    add_status = ContactDAO.add_a_contact(contact)

    if add_status:
        return name
    else:
        return ""

def remove_a_contact():
    print("-------------------------------------------")
    print("\t Remove a contact")
    print("-------------------------------------------")

    contacts = print_all_contacts()

    print("\t Please enter the number of the contact you would like to remove and hit ENTER")
    print("\t", end="")

    input_string = input('')

    try:
        selection = int(input_string)
        if selection <= len(contacts) and selection > 0:
            removed_name = ContactDAO.remove_a_contact(contacts[selection - 1])
            return removed_name
        else:
            return None
    except:
        return None

def update_a_contact():
    print("-------------------------------------------")
    print("\t Update a contact")
    print("-------------------------------------------")

    contacts = print_all_contacts()

    print("\t Please enter the number of the contact you would like to update and hit ENTER")
    print("\t", end="")

    input_string = input('')

    try:
        selection = int(input_string)
        if selection <= len(contacts) and selection > 0:
            edit_contact = contacts[selection -1]
            print("\t Updating contact: " + edit_contact.get_name())
            print("\t", end="")

            name = ""
            name_check = False
            email = None
            phone_number = None
            group_to_add = ""
            groups = []
            groups_finished = False

            while not name_check:
                print("\t Please enter a contact name and hit ENTER")
                print("\t Just hit ENTER to leave name unchanged")
                print("\t", end="")
                name = input('')

                if len(name) == 0 or name == edit_contact.get_name():
                    print()
                    print("\t\t Name will not be changed")
                    print()
                    name_check = True
                    name = edit_contact.get_name()
                else:
                    print()
                    print("\t\t Name will be changed to: " + name)
                    print()
                    name_check = True

            print("\t Please enter a contact email and hit ENTER")
            print("\t [To leave unchanged, just hit ENTER]")
            print("\t", end="")
            email = input('')

            if len(email) == 0:
                print()
                print("\t\t Email will not be changed")
                print()
                email = edit_contact.get_email()
            else:
                print()
                print("\t\t Email will be changed to: " + email)
                print()

            print("\t Please enter a contact phone number and hit ENTER")
            print("\t [To leave unchanged, just hit ENTER]")
            print("\t", end="")
            phone_number = input('')

            if len(phone_number) == 0:
                print()
                print("\t\t Phone number will not be changed")
                print()
                phone_number = edit_contact.get_phone_number()
            else:
                print()
                print("\t\t Phone number will be changed to: " + phone_number)
                print()

            while not groups_finished:
                print("Current groups are: ")
                print("\t\t", end="")

                for group in edit_contact.get_groups():
                    print("| " + group + " |", end="")
                print()
                print()

                print("\t To add to these, press 1 and hit ENTER")
                print("\t To start with no groups, press 2 and hit ENTER")
                print("\t", end="")

                add_group_input = input("")

                try:
                    add_group_selection = int(add_group_input)
                    if add_group_selection < 3 and add_group_selection > 0:
                        if add_group_selection == 1:
                            groups = edit_contact.get_groups()
                            while not groups_finished:
                                print("\t Please enter a group name and hit ENTER")
                                print("\t", end="")
                                group_to_add = input('')
                                if len(group_to_add) == 0:
                                    print("\t No group name entered, please try again")
                                else:
                                    groups.append(group_to_add)
                                    print("\t Group added successfully")
                                    print("\t To add another group, type '1' and hit ENTER, otherwise hit ENTER")
                                    print("\t", end="")
                                    add_more = input('')
                                    if add_more == "1":
                                        groups_finished = False
                                    else:
                                        groups_finished = True
                        elif add_group_selection == 2:
                            while not groups_finished:
                                print("\t Please enter a group name and hit ENTER")
                                print("\t", end="")
                                group_to_add = input('')
                                if len(group_to_add) == 0:
                                    print("\t No group name entered, please try again")
                                else:
                                    groups.append(group_to_add)
                                    print("\t Group added successfully")
                                    print("\t To add another group, type '1' and hit ENTER, otherwise hit ENTER")
                                    print("\t", end="")
                                    add_more = input('')
                                    if add_more == "1":
                                        groups_finished = False
                                    else:
                                        groups_finished = True
                except:
                    print("\t Invalid input, please select a menu item number. Please try again...")

                contact = Contact(name, email, phone_number)
                contact.set_groups(groups)
                edited_contact_name = ContactDAO.update_a_contact(contact, edit_contact.get_name())

                if edited_contact_name is not None:
                    return edited_contact_name
                else:
                    return None
        else:
            return None
    except:
        return None



if __name__ == '__main__':
    main(sys.argv)