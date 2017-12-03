from MySqlDb import MySqlDb
from Contact import Contact

class ContactDAO(object):

    @staticmethod
    def add_a_contact(contact):
        db = MySqlDb()

        for group in contact.get_groups():
            result = db.insert("INSERT IGNORE INTO contact_group(group_name) VALUES (%s);", [group])

        result = db.insert("INSERT IGNORE INTO contact(name, email, phone_number) VALUES (%s, %s , %s);", [contact.get_name(), contact.get_email(), contact.get_phone_number()])

        for group in contact.get_groups():
            result = db.insert("INSERT IGNORE INTO group_link(contact_id, group_id) VALUES ((SELECT contact_id FROM contact WHERE name = %s), (SELECT group_id FROM contact_group WHERE group_name = %s));", [contact.get_name, group])

        db.close_connection()

        return True

    @staticmethod
    def remove_a_contact(contact):
        db = MySqlDb()

        result = db.update("DELETE FROM contact WHERE name = %s;", [contact.get_name()])
        db.close_connection()

        if result:
            return contact.get_name
        else:
            return ""

    @staticmethod
    def search_by_name(search_name):
        searchContacts = []
        db = MySqlDb()

        rows = db.query('SELECT c.*, g.group_name FROM contact AS c, contact_group AS g, group_link AS gl WHERE c.contact_id = gl.contact_id AND g.group_id = gl.group_id AND c.name LIKE %s;', [search_name + "%"])
        db.close_connection()

        prev_name = ""
        new_contact = None
        groups = None

        for entry in rows:
            if len(prev_name) == 0 or prev_name != entry['name']:
                if len(prev_name) != 0:
                    new_contact.set_groups(groups)
                    searchContacts.append(new_contact)

                name = entry['name']
                email = entry['email']
                phone_number = entry['phone_number']
                group = entry['group_name']

                groups = []
                groups.append(group)

                prev_name = name

                new_contact = Contact(name, email, phone_number)
            else:
                group = entry['group_name']
                groups.append(group)

        if new_contact is not None:
            new_contact.set_groups(groups)
            searchContacts.append(new_contact)

        if len(searchContacts) > 0:
            return searchContacts
        else:
            return None

    @staticmethod
    def get_all():
        allContacts = []

        db = MySqlDb()
        rows = db.query('SELECT c.*, g.group_name FROM contact AS c, contact_group AS g, group_link AS gl WHERE c.contact_id = gl.contact_id AND g.group_id = gl.group_id;', [])
        db.close_connection()

        prev_name = ""
        new_contact = None
        groups = None

        for entry in rows:
            if len(prev_name) == 0 or prev_name != entry['name']:
                if len(prev_name) != 0:
                    new_contact.set_groups(groups)
                    allContacts.append(new_contact)

                name = entry['name']
                email = entry['email']
                phone_number = entry['phone_number']
                group = entry['group_name']

                groups = []
                groups.append(group)

                prev_name = name

                new_contact = Contact(name, email, phone_number)
            else:
                group = entry['group_name']
                groups.append(group)

        if new_contact is not None:
            new_contact.set_groups(groups)
            allContacts.append(new_contact)

        if len(allContacts) > 0:
            return allContacts
        else:
            return None

