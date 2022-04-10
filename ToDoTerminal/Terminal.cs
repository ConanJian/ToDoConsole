using CallApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoTerminal
{
    public class Terminal: IDisposable
    {
        private readonly ToDoApiClient _client;
        private List<ToDoModel> toDoListCache;
        public Terminal()
        {
            _client = new ToDoApiClient();
            toDoListCache = _client.GetCompleteToDoListAsync().Result;
        }
        public async Task PrintCompleteToDoList()
        {
            toDoListCache = await _client.GetCompleteToDoListAsync();
            PrintToDoList(toDoListCache);
        }
        public async Task PrintPriorityToDoList()
        {
            Console.Write($"ToDoList by Priority\n" +
                $"Options: (1, 2, 3)\n" +
                $"Priority: ");

            bool notCompleted = true;
            int priority = -1;
            while (notCompleted)
            {
                try
                {
                    //Reads in input and validates if it is valid
                    string priorityInput = Console.ReadLine();
                    bool isInteger = int.TryParse(priorityInput, out priority);
                    if (!isInteger)
                    {
                        throw new Exception("priorityInput not an integer");
                    }
                    if (priority > 3 || priority < 1)
                    {
                        throw new Exception("priorityInput not 1, 2, or 3");
                    }
                    notCompleted = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Incorrect Input try again: ");
                }
            }

            var toDoList = await _client.GetPriorityToDoListAsync((Priority)priority);
            PrintToDoList(toDoList);
        }
        private void PrintToDoList(List<ToDoModel> toDoList)
        {
            for (int i = 0; i < toDoList.Count; i++)
            {
                ToDoModel toDoItem = toDoList[i];
                string output = $"  {i + 1}) {toDoItem.Message}" +
                    $"\nPriority: {(Priority)toDoItem.Priority}";
                Console.WriteLine(output);
            }
        }
        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
