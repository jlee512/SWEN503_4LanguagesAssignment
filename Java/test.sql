# Create tables (Contact and Group)

DROP TABLE IF EXISTS Group_Link;
DROP TABLE IF EXISTS Contact;
DROP TABLE IF EXISTS Contact_Group;

CREATE TABLE Contact (
  contact_id INT PRIMARY KEY AUTO_INCREMENT,
  name VARCHAR(150),
  email VARCHAR(150),
  phone_number VARCHAR(15)
);

CREATE TABLE Contact_Group (
  group_id INT PRIMARY KEY AUTO_INCREMENT,
  group_name VARCHAR(150)
);

CREATE TABLE Group_Link (
  link_id INT PRIMARY KEY AUTO_INCREMENT,
  contact_id INT NOT NULL,
  group_id INT NOT NULL,
  FOREIGN KEY (contact_id) REFERENCES Contact(contact_id) ON UPDATE CASCADE ON DELETE CASCADE,
  FOREIGN KEY (group_id) REFERENCES Contact_Group(group_id) ON UPDATE CASCADE ON DELETE CASCADE
);

INSERT INTO Contact(name, email, phone_number) VALUES ("Julian", "julian@gmail.com", "0211234567"), ("Maryanne", "maryanne@gmail.com", "0212345678"), ("Michael","michael@gmail.com", "0213456789");

INSERT INTO Contact_Group(group_name) VALUES ("Family"), ("Friends"), ("Work");

INSERT INTO Group_Link(contact_id, group_id) VALUES (11, 11), (21, 21);