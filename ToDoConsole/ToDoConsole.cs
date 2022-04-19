using System;
using ToDoTerminal;

namespace ToDoConsole
{
    internal class ToDoConsole
    {
        public static async Task Main(string[] args)
        {
            using (Terminal terminal = new Terminal())
            {
                Console.WriteLine("Welcome to the Terminal!\n" +
                    "Your Current ToDo List is:");
                await terminal.PrintCompleteToDoList();

                bool running = true;
                while (running)
                {
                    try
                    {
                        Console.Write("\nCommand: ");
                        string command = Console.ReadLine().ToLower();

                        //Separate the next output from the command I read in.
                        Console.WriteLine();

                        switch (command)
                        {
                            case "list":
                                await terminal.PrintCompleteToDoList();
                                break;
                            case "priority list":
                                await terminal.PrintPriorityToDoList();
                                break;
                            case "add":
                                await terminal.CreateToDoItem();
                                break;
                            case "delete":
                                await terminal.DeleteToDoItem();
                                break;
                            case "help":
                                Console.WriteLine("These are the possible commands\n" +
                                    "--------------------------\n" +
                                    "list: list the entire to-do list\n" +
                                    "priority list: gives all items in the list of a certain priority\n" +
                                    "add: add a to-do item\n" +
                                    "delete: delete a to-do item\n" +
                                    "quit: end the program");
                                break;
                            case "quit":
                                running = false;
                                break;
                            default:
                                Console.WriteLine("Invalid Command. Type 'quit' to quit");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error Message: {e.Message}");
                    }
                }
            }
        }
    }
}