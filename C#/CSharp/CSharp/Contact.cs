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
        public List<string> groups { get; set; }

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

            if (groups != null)
            {
                Console.Write("\t Groups: ");
                if (groups.Count > 1)
                {
                    for (int i = 0; i < groups.Count; i++)
                    {
                        if (i == (groups.Count - 1))
                        {
                            Console.WriteLine(groups[i]);
                        }
                        else
                        {
                            Console.Write(groups[i] + ", ");
                        }
                    }
                }
                else
                {
                    Console.WriteLine(groups[0]);
                }
            }
            Console.WriteLine();
        }

    }
}
