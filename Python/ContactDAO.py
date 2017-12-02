from MySqlDb import MySqlDb
from Contact import Contact

class ContactDAO(object):

    @staticmethod
    def getAll():
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

