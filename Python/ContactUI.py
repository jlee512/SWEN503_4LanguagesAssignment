import sys
from MySqlDb import MySqlDb
from Contact import Contact
from ContactDAO import ContactDAO

contacts_in_focus = []
searchedContacts = []

def main(args):
    session = True
    initial_screen()

def initial_screen():
    print("*******************************************")
    print("\tWelcome to your contacts viewer")
    print("*******************************************")
    print_all_contacts()


def print_all_contacts():
    contacts_in_focus = ContactDAO.getAll()

    if contacts_in_focus is not None:
        print("Contacts: ")
        print("----------")
        for index, contact in enumerate(contacts_in_focus):
            print("" + str(index + 1) + ") ", end="")
            contact.print_to_console()
    else:
        print("Looks like you haven't added any contacts yet...")

    return contacts_in_focus


if __name__ == '__main__':
    main(sys.argv)