import sys


class Contact(object):
    __name = None
    __phone_number = None
    __email = None
    __groups = None

    def __init__(self, name, email, phone_number):
        self.__name = name
        self.__phone_number = phone_number
        self.__email = email

    def print_to_console(self):
        print(self.__name)
        print("\t Phone: " + self.__phone_number)
        print("\t Email: " + self.__email)

        if self.__groups is not None:
            print("\t Groups: ", end="")
            if len(self.__groups) > 1:
                for group in self.__groups[:-1]:
                    print(group + ", ", end="")
                print(self.__groups[-1])
            else:
                print(self.__groups[0])
        print()

    def get_name(self):
        return self.__name

    def get_phone_number(self):
        return self.__phone_number

    def get_email(self):
        return self.__email

    def get_groups(self):
        return self.__groups

    def set_name(self, name):
        self.__name = name

    def set_phone_number(self, phone_number):
        self.__phone_number = phone_number

    def set_email(self, email):
        self.__email = email

    def set_groups(self, groups):
        self.__groups = groups
