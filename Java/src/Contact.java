import java.util.ArrayList;

/**
 * Created by Julian on 30/11/2017.
 */
public class Contact {

    private String name;
    private String phone_number;
    private String email;
    private ArrayList<String> groups;

    public Contact(String name, String phone_number, String email) {
        this.name = name;
        this.phone_number = phone_number;
        this.email = email;
    }

    public void printToConsole() {
        System.out.println(name);
        System.out.println("\t Phone: " + phone_number);
        System.out.println("\t Email: " + email);

        if (groups != null) {
            System.out.print("\t Groups: ");
            if (groups.size() > 1) {
                for (int i = 0; i < groups.size(); i++) {
                    if (i == (groups.size() - 1)) {
                        System.out.println(groups.get(i));
                    } else {
                        System.out.print(groups.get(i) + ", ");
                    }
                }
            } else {
                System.out.println(groups.get(0));
            }
        }
        System.out.println();

    }

    public String getName() {
        return name;
    }

    public String getPhone_number() {
        return phone_number;
    }

    public String getEmail() {
        return email;
    }

    public ArrayList<String> getGroups() {
        return groups;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setPhone_number(String phone_number) {
        this.phone_number = phone_number;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public void setGroups(ArrayList<String> groups) {
        this.groups = groups;
    }
}
