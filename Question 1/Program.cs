namespace ITPCA_Assignemnet;
using System;
using System.Collections.Generic;

public class Program
{
    static List<string> users = new List<string>();
    static List<string> rooms = new List<string>();
    static List<string> dates = new List<string>();
    static List<string> availableRooms = new List<string> { "single", "double", "suite" };

    public static void Main()
    {
        bool state = true;

        do
        {
            Console.WriteLine("1. Book Room");
            Console.WriteLine("2. Check-in Guest");
            Console.WriteLine("3. Check-out Guest");
            Console.WriteLine("4. View Available Rooms");
            Console.WriteLine("5. View Reservation History");
            Console.WriteLine("6. Exit");

            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 1:
                    Bookings();
                    break;
                case 2:
                    Registration(rooms);
                    break;
                case 3:
                    if (rooms.Count != 0)
                    {
                        Console.WriteLine($"Enter Room to check out: ");
                        string room = Console.ReadLine();

                        int index = rooms.IndexOf(room);
                        rooms.RemoveAt(index);
                        users.RemoveAt(index);
                        dates.RemoveAt(index);
                        availableRooms.Add(room);

                        Console.WriteLine($"Room type: {room} is now available for new guests");
                    }

                    break;
                case 4:
                    Console.WriteLine("Available rooms: ");
                    foreach (string room in availableRooms)
                    {
                        Console.WriteLine(room);
                    }
                    break;
                case 5:

                    int n = 0;
                    int cal = 0;
                    foreach (string item in rooms)
                    {
                        Console.WriteLine($"Room {rooms[n]}, Guest: {users[n]}, From: {dates[cal]} To: {dates[cal + 1]} \n");
                        n++;
                        cal += 2;
                    };

                    break;
                case 6:
                    Console.WriteLine("Exiting the Program. Goodbye!");
                    state = false;
                    break;

            }
        } while (state);

    }
    static void Bookings()
    {
        Console.WriteLine("Enter Guest Name: ");
        string name = Console.ReadLine();
        users.Add(name);

        Console.WriteLine("Enter Room Type (Single/Double/Suite): ");
        string n = Console.ReadLine();
        string room = n.ToLower();
        Validation(room, rooms);
        int index = availableRooms.IndexOf(room);
        availableRooms.RemoveAt(index);


        Console.WriteLine("Enter Check-in Date (MM/DD/YYYY): ");
        string checkIn = Console.ReadLine();
        Validation(checkIn, dates);

        Console.WriteLine("Enter Check-out Date (MM/DD/YYYY): ");
        string checkOut = Console.ReadLine();
        Validation(checkOut, dates);

    }

    static void Validation(string entry, List<string> list)
    {

        while (list.Contains(entry))
        {
            Console.WriteLine("This already exists, Please try again: ");
            string n = Console.ReadLine();
            entry = n.ToLower();
        }
        list.Add(entry);

    }

    static void Registration(List<string> list)
    {
        Console.WriteLine($"Enter Guest name: ");
        string name = Console.ReadLine();
        Console.WriteLine($"Enter Room Type (Single/Double/Suite):");
        string room = Console.ReadLine();

        foreach (string entry in list)
        {
            if (list.Contains(room))
            {
                Console.WriteLine($"{name} has been checked into room {room}");
            }
            else
            {
                Console.WriteLine("This room does not exist or hasnt been booked.");
            }
        }
    }

}

