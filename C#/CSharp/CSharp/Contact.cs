using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    class Contact
    {
        // Much faster to output getters/setters (ignoring keyboard shortcuts) in C#
        public string Name { get; set;  }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<string> Groups { get; set; }

        public Contact(string name, string email, string phoneNumber)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public void PrintToConsole()
        {
            Console.WriteLine(Name);
            Console.WriteLine("\t Phone: " + PhoneNumber);
            Console.WriteLine("\t Email: " + Email);

            if (Groups != null)
            {
                Console.Write("\t Groups: ");
                if (Groups.Count > 1)
                {
                    for (int i = 0; i < Groups.Count; i++)
                    {
                        if (i == (Groups.Count - 1))
                        {
                            Console.WriteLine(Groups[i]);
                        }
                        else
                        {
                            Console.Write(Groups[i] + ", ");
                        }
                    }
                }
                else
                {
                    Console.WriteLine(Groups[0]);
                }
            }
            Console.WriteLine();
        }

    }
}
