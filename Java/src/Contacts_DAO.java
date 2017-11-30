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
                    if (allContacts.size() > 0) {
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

    public static Contact getByName(String name) {
        //TODO
        return null;
    }

    public static Contact getByPhoneNumber(String phone_number) {
        //TODO
        return null;
    }

    public static Contact getByEmail(String email) {
        //TODO
        return null;
    }

}
