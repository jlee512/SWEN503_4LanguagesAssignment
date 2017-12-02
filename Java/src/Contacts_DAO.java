import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;

/**
 * Created by Julian on 30/11/2017.
 */
public class Contacts_DAO {

    public static ArrayList<Contact> getAll() {
        //Gets all contacts currently in the contacts list
        ArrayList<Contact> allContacts = new ArrayList<>();

        MySQL db = new MySQL();

        //Connect to database and pull contacts list
        try (Connection c = db.connection()) {
            try (PreparedStatement stmt = c.prepareStatement("SELECT c.*, g.group_name FROM contact AS c, contact_group AS g, group_link AS gl WHERE c.contact_id = gl.contact_id AND g.group_id = gl.group_id;")) {

                //Process the results set
                try (ResultSet r = stmt.executeQuery()) {
                    String prev_name = "";
                    Contact new_contact = null;
                    ArrayList<String> groups = null;
                    while (r.next()) {
                        //If name is not blank and the name is new, process a new contact. Otherwise, most the contact is a repeat entry resulting from the contact being in multiple groups, process groups into an arraylist
                        if (prev_name.length() == 0 || !prev_name.equals(r.getString("name"))) {

                            //Check if contact was preceded by another, if so, add groups to contact and add contact to allContacts arraylist
                            if (prev_name.length() != 0) {
                                new_contact.setGroups(groups);
                                allContacts.add(new_contact);
                            }

                            String name = r.getString("name");
                            String email = r.getString("email");
                            String phone_number = r.getString("phone_number");
                            String group = r.getString("group_name");

                            groups = new ArrayList<>();
                            groups.add(group);

                            //Create new contact object and update prev_name variable to the just processed contact
                            prev_name = name;
                            new_contact = new Contact(name, email, phone_number);

                        } else {
                            //Get the group_name entry and add the new_contact
                            String group = r.getString("group_name");
                            groups.add(group);
                        }
                    }
                    if (new_contact != null) {
                        //Add last contact to list
                        new_contact.setGroups(groups);
                        allContacts.add(new_contact);
                    }
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        }

        // Process results, if some contacts are available, return the results as an arraylist, otherwise, return null
        if (allContacts.size() > 0) {
            return allContacts;
        } else {
            return null;
        }
    }

    public static ArrayList<Contact> searchByName(String search_name) {
        //Gets a list of contacts searched by name within the contacts list
        ArrayList<Contact> searchContacts = new ArrayList<>();

        MySQL db = new MySQL();

        //Connect to database and pull contacts list
        try (Connection c = db.connection()) {
            try (PreparedStatement stmt = c.prepareStatement("SELECT c.*, g.group_name FROM contact AS c, contact_group AS g, group_link AS gl WHERE c.contact_id = gl.contact_id AND g.group_id = gl.group_id AND c.name LIKE ?;")) {
                stmt.setString(1, search_name + "%");

                //Process the results set
                try (ResultSet r = stmt.executeQuery()) {
                    String prev_name = "";
                    Contact new_contact = null;
                    ArrayList<String> groups = null;
                    while (r.next()) {
                        //If name is not blank and the name is new, process a new contact. Otherwise, the contact is a repeat entry resulting from the contact being in multiple groups, process groups into an arraylist
                        if (prev_name.length() == 0 || !prev_name.equals(r.getString("name"))) {

                            //Check if contact was preceded by another, if so, add groups to contact and add contact to allContacts arraylist
                            if (prev_name.length() != 0) {
                                new_contact.setGroups(groups);
                                searchContacts.add(new_contact);
                            }

                            String name = r.getString("name");
                            String email = r.getString("email");
                            String phone_number = r.getString("phone_number");
                            String group = r.getString("group_name");

                            groups = new ArrayList<>();
                            groups.add(group);

                            //Create new contact object and update prev_name variable to the just processed contact
                            prev_name = name;
                            new_contact = new Contact(name, email, phone_number);

                        } else {
                            //Get the group_name entry and add the new_contact
                            String group = r.getString("group_name");
                            groups.add(group);
                        }
                    }
                    if (new_contact != null) {
                        //Add last contact to list
                        new_contact.setGroups(groups);
                        searchContacts.add(new_contact);
                    }
                }
            }
        } catch (SQLException e) {
            e.printStackTrace();
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        }

        // Process results, if some contacts are available, return the results as an arraylist, otherwise, return null
        if (searchContacts.size() > 0) {
            return searchContacts;
        } else {
            return null;
        }
    }

    public static String removeAContact(Contact contact) {

        MySQL db = new MySQL();

        try (Connection c = db.connection()) {

            try (PreparedStatement stmt = c.prepareStatement("DELETE FROM contact WHERE name = ?;")) {

                stmt.setString(1, contact.getName());
                stmt.executeUpdate();

                return contact.getName();

            }
        } catch (SQLException e) {
            e.printStackTrace();
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        }

        return "";
    }

    public static String updateAContact(Contact contact, String originalName) {

        MySQL db = new MySQL();

        try (Connection c = db.connection()) {
            for (int i = 0; i < contact.getGroups().size(); i++) {
                //Add any new groups to the group table
                try (PreparedStatement stmt = c.prepareStatement("INSERT IGNORE INTO contact_group(group_name) VALUES (?);")) {

                    stmt.setString(1, contact.getGroups().get(i));
                    stmt.executeUpdate();
                }
            }

            //Insert contact into contact table
            //NOTE: this feature will prevent duplicate contacts from being added. This would need to be fixed at a later date
            try (PreparedStatement stmt = c.prepareStatement("UPDATE contact SET name = ?, email = ? ,phone_number = ? WHERE name = ?;")) {

                stmt.setString(1, contact.getName());
                stmt.setString(2, contact.getEmail());
                stmt.setString(3, contact.getPhone_number());
                stmt.setString(4, originalName);
                stmt.executeUpdate();
            }

            try (PreparedStatement stmt = c.prepareStatement("DELETE FROM group_link WHERE contact_id = (SELECT contact_id FROM contact WHERE name = ?);")) {

                stmt.setString(1, contact.getName());
                stmt.executeUpdate();
            }

            //Insert contact id and group id's into group_link table
            for (int i = 0; i < contact.getGroups().size(); i++) {
                //Add any new groups to the group table
                try (PreparedStatement stmt = c.prepareStatement("INSERT IGNORE INTO group_link(contact_id, group_id) VALUES ((SELECT contact_id FROM contact WHERE name = ?), (SELECT group_id FROM contact_group WHERE group_name = ?));")) {

                    stmt.setString(1, contact.getName());
                    stmt.setString(2, contact.getGroups().get(i));
                    stmt.executeUpdate();
                }
            }

            return contact.getName();

        } catch (SQLException e) {
            e.printStackTrace();
            return null;
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
            return null;
        }
    }

    public static boolean addContact(Contact contact) {

        MySQL db = new MySQL();


        try (Connection c = db.connection()) {
            for (int i = 0; i < contact.getGroups().size(); i++) {
                //Add any new groups to the group table
                try (PreparedStatement stmt = c.prepareStatement("INSERT IGNORE INTO contact_group(group_name) VALUES (?);")) {

                    stmt.setString(1, contact.getGroups().get(i));
                    stmt.executeUpdate();
                }
            }

            //Insert contact into contact table
            //NOTE: this feature will prevent duplicate contacts from being added. This would need to be fixed at a later date
            try (PreparedStatement stmt = c.prepareStatement("INSERT IGNORE INTO contact(name, email, phone_number) VALUES (?, ? , ?);")) {

                stmt.setString(1, contact.getName());
                stmt.setString(2, contact.getEmail());
                stmt.setString(3, contact.getPhone_number());
                stmt.executeUpdate();
            }

            //Insert contact id and group id's into group_link table
            for (int i = 0; i < contact.getGroups().size(); i++) {
                //Add any new groups to the group table
                try (PreparedStatement stmt = c.prepareStatement("INSERT IGNORE INTO group_link(contact_id, group_id) VALUES ((SELECT contact_id FROM contact WHERE name = ?), (SELECT group_id FROM contact_group WHERE group_name = ?));")) {

                    stmt.setString(1, contact.getName());
                    stmt.setString(2, contact.getGroups().get(i));
                    stmt.executeUpdate();
                }
            }

            return true;

        } catch (SQLException e) {
            e.printStackTrace();
            return false;
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
            return false;
        }
    }
}
