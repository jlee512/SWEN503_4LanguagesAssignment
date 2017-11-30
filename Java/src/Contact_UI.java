import java.util.ArrayList;

/**
 * Created by Julian on 30/11/2017.
 */
public class Contact_UI {

    public static ArrayList<Contact> contacts_in_focus = new ArrayList<>();

    public static void main(String[] args) {
        //Present welcome screen and initial list of all contacts available
        System.out.println("*******************************************");
        System.out.println("\t Welcome to your contacts viewer \t");
        System.out.println("*******************************************");

        contacts_in_focus = Contacts_DAO.getAll();

        if (contacts_in_focus != null) {
            System.out.println("Contacts: ");
            System.out.println("--------");
            for (int i = 0; i < contacts_in_focus.size(); i++) {
                Contact contact = contacts_in_focus.get(i);
                System.out.print("" + (i + 1) + ") ");
                contact.printToConsole();
                System.out.println();
            }
        } else {
            System.out.println("Looks like you haven't added any contacts yet...");
            System.out.println("To add some contacts please type '+' and hit enter");
        }

    }
}
