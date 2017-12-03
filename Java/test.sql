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
  group_name VARCHAR(150) UNIQUE
);

CREATE TABLE Group_Link (
  link_id INT PRIMARY KEY AUTO_INCREMENT,
  contact_id INT NOT NULL,
  group_id INT NOT NULL,
  CONSTRAINT UC_contactAndGroup UNIQUE (contact_id, group_id),
  FOREIGN KEY (contact_id) REFERENCES Contact(contact_id) ON UPDATE CASCADE ON DELETE CASCADE,
  FOREIGN KEY (group_id) REFERENCES Contact_Group(group_id) ON UPDATE CASCADE ON DELETE CASCADE
);

# Insert seed-data into tables
INSERT INTO Contact(name, email, phone_number) VALUES ("Julian", "julian@gmail.com", "0211234567"), ("Maryanne", "maryanne@gmail.com", "0212345678"), ("Michael","michael@gmail.com", "0213456789");

INSERT INTO Contact_Group(group_name) VALUES ("Family"), ("Friends"), ("Work"), ("MISC");

INSERT INTO Group_Link(contact_id, group_id) VALUES (1, 31), (11, 1) , (11, 11), (21, 21);

# Test queries
# SELECT c.*, g.group_name FROM contact AS c, contact_group AS g, group_link AS gl WHERE c.contact_id = gl.contact_id AND g.group_id = gl.group_id;
#
# SELECT c.*, g.group_name FROM contact AS c, contact_group AS g, group_link AS gl WHERE c.contact_id = gl.contact_id AND g.group_id = gl.group_id AND c.name LIKE 'Julian%';
#
# UPDATE contact SET name = 'test', email = 'test' ,phone_number = 'test' WHERE name = 'Julian';
#
# SELECT * FROM Contact;