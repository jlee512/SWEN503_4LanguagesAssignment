/**
 * Created by Julian on 30/11/2017.
 */
public class MySQL extends AbstractDB {

    public MySQL() {
        super("com.mysql.jdbc.Driver",
                "mysql",
                DBConfiguration.MYSQL_HOSTNAME,
                DBConfiguration.MYSQL_DBNAME,
                DBConfiguration.MYSQL_PORT,
                DBConfiguration.MYSQL_USERNAME,
                DBConfiguration.MYSQL_PASSWORD);
    }

}
