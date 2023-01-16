using DataAccess.Helpers;
using DataAccess.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace DataAccess;

internal class Program
{
    private const string _CONNECTION_STRING = @"Server=DESKTOP-MLES57C;Database=PhoneBook;Integrated Security=true";
    static void Main(string[] args)
    {

        while (true)
        {
            Console.WriteLine("\na. Insert Data");
            Console.WriteLine("l. List all data");
            Console.WriteLine("s. Search data");
            Console.WriteLine("e. Exit\n");

            char userInput = Console.ReadKey(true).KeyChar;

            switch (userInput)
            {
                case 'a':
                    AddPerson();
                    break;
                case 'l':
                    ListAllPeople();
                    break;
                case 's':
                    FilteredList();
                    break;
                case 'e':
                    return;
                default:
                    Console.WriteLine("Try Again");
                    break;
            }
        }

        //var people = GetAllConnected();

        //foreach (var person in people)
        //{
        //    Console.WriteLine(new String('_',50));
        //    Console.WriteLine("\n");
        //    foreach (PropertyInfo prop in person.GetType().GetProperties())
        //    {
        //        Console.WriteLine($"{prop.Name, -20} : {prop.GetValue(person)}");
        //    }
        //    Console.WriteLine("\n");
        //}


    }

    public static void AddPerson()
    {
        Console.Write("Enter the name: ");
        string nameInput = Console.ReadLine();

        Console.Write("Enter the surname: ");
        string lastNameInput = Console.ReadLine();

        Console.Write("Enter the Phone: ");
        string phoneInput = Console.ReadLine();

        Console.Write("Enter the Email: ");
        string emailInput = Console.ReadLine();

        Console.WriteLine("Do you want to save? (Y/N)");

        ConsoleKey key = Console.ReadKey(true).Key;

        while(key != ConsoleKey.Y && key != ConsoleKey.N)
        {
            key = Console.ReadKey(true).Key;
        }

        if(key == ConsoleKey.N)
        {
            Console.WriteLine("Person draft deleted");
            return;
        }

        Person newPerson = new()
        {
            FirstName = nameInput,
            LastName = lastNameInput,
            Phone = phoneInput,
            Email = emailInput
        };

        Console.WriteLine(PersonHelper.InsertPerson(newPerson) ? "\nPerson added" : "\nProblems occured when inserting data");
    }

    public static void ListAllPeople(List<Person> people = null)
    {
        if (people is null)
        {
            people = PersonHelper.GetAllPeople();
        }

        Console.WriteLine();
        Console.Write(new String(' ', 10) + "Id" + new String(' ', 23));
        Console.Write("FirstName" + new String(' ', 16));
        Console.Write("LastName" + new String(' ', 17));
        Console.Write("Phone" + new String(' ', 20));
        Console.Write("Email" + new String(' ', 20));

        Console.WriteLine("\n" + new String('_', 150));

        int spaceSize = 25;
        foreach (var person in people)
        {
            Console.Write(new String(' ', 10));
            Console.Write(person.Id + new String(' ', spaceSize - person.Id.ToString().Length));

            Console.Write(person.FirstName + new String(' ', spaceSize - person.FirstName.ToString().Length));

            Console.Write(person.LastName + new String(' ', spaceSize - person.LastName.ToString().Length));

            Console.Write(person.Phone + new String(' ', spaceSize - person.Phone.ToString().Length));

            Console.Write(person.Email);

            Console.WriteLine();
            Console.WriteLine();
        }

        ConsoleKey listInputs;
        do
        {
            Console.WriteLine("\nd. Delete data");
            Console.WriteLine("m. Return to main menu\n");
            listInputs = Console.ReadKey(true).Key;
        }
        while (listInputs != ConsoleKey.D && listInputs != ConsoleKey.M);

        if (listInputs == ConsoleKey.D)
        {
            Console.Write("Enter Id: ");
            int deleteInput = int.Parse(Console.ReadLine());

            if (PersonHelper.DeletePerson(deleteInput)) Console.WriteLine("\nPerson deleted\n");
            else Console.WriteLine("\nPerson didn't found\n");
            return;
        }
        else if (listInputs == ConsoleKey.M)
        {
            Console.Clear();
            return;
        }
    }

    public static void FilteredList()
    {
        Console.Write("\nEnter filter input: ");
        string filterInput = Console.ReadLine();

        List<Person> people = PersonHelper.FilterPeople(filterInput);

        ListAllPeople(people);
    }


}