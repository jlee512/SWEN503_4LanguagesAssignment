import java.util.ArrayList;

/**
 * Created by Julian on 30/11/2017.
 */
public class Contact {

    private String name;
    private String phone_number;
    private String email;
    private ArrayList<String> groups;

    public Contact(String name, String phone_number, String email, ArrayList<String> groups) {
        this.name = name;
        this.phone_number = phone_number;
        this.email = email;
        this.groups = groups;
    }

    public void printToConsole() {
        System.out.println();
        System.out.println(name);
        System.out.println("\t Phone: " + phone_number);
        System.out.println("\t Email: " + email);

        if (groups != null) {
            System.out.print("\t Groups: ");
            for (int i = 0; i < groups.size(); i++) {
                System.out.println(groups.get(i));
            }
        }

    }

}
