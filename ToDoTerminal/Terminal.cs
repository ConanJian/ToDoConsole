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
            string prompt = $"ToDoList by Priority\n" +
                $"Options: (1, 2, 3)\n" +
                $"Priority: ";
            string failurePrompt = "Incorrect Input try again: ";

            string userInput = GetInput(prompt, failurePrompt,
                (priorityInput) =>
                {
                    int priority = -1;
                    bool isInteger = int.TryParse(priorityInput, out priority);
                    if (!isInteger)
                    {
                        //"priorityInput not an integer
                        return false;
                    }
                    if (priority > 3 || priority < 1)
                    {
                        //priorityInput not 1, 2, or 3
                        return false;
                    }
                    return true;
                });

            int priority = int.Parse(userInput);
            var toDoList = await _client.GetPriorityToDoListAsync((Priority)priority);
            PrintToDoList(toDoList);
        }
        //Issue with deletion: how do you know what to delete?
        //Maybe send list with listNums to help with deletion, would make it unambigious
        //How would I maintain cache if done this way?

        public async Task CreateToDoItem()
        {
            string message = GetInput("ToDoItem Content: ", "Error, Invalid Content. Please try again: ",
                (string messageInput) =>
                {
                    return true;
                });

            string priorityInput = GetInput("Priority: ", "Invalid priority. Should be numbers (1, 2, 3): ",
                (input) =>
                {
                    int priority = -1;
                    bool isInt = int.TryParse(input, out priority);

                    if (!isInt)
                        return false;
                    else if (priority < 4 && priority > 0)
                        return true;
                    else
                        return false;
                });

            int priority = int.Parse(priorityInput);

            await _client.AddToDoItemAsync(message, (Priority)priority);
            try
            {
                //when add is successful add to cache
                int listNum = await _client.GetMostRecentListNum();
                toDoListCache.Add(new ToDoModel(listNum, message, (Priority)priority));
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine("Error, get most recent list num failed to work");
#endif
                //I couldn't get latest listNum, so I couldn't just add to cache
                //instead we just get everything all over again
                toDoListCache = await _client.GetCompleteToDoListAsync();
            }
        }
        public async Task DeleteToDoItem()
        {
            Console.WriteLine("Current ToDOList");
            await PrintCompleteToDoList();

            string prompt1 = "Input the number of the toDoList you wish to delete: ";
            string prompt2 = "Invalid number, try again (Use number format like 1 or 2): ";
            Func<string, bool> validation = (input) =>
            {
                int validInput = -1;
                bool isInt = int.TryParse(input, out validInput);
                if (isInt && validInput <= toDoListCache.Count && validInput > 0)
                    return true;
                else
                    return false;
            };
            int input = int.Parse(GetInput(prompt1, prompt2, validation));

            await _client.DeleteToDoItemAsync(toDoListCache[input - 1].ListNum);

        }
        private string GetInput(string prompt, string failurePrompt, Func<string, bool> isValid)
        {
            Console.Write(prompt);
            string userInput = "";
            bool notCompleted = true;
            while (notCompleted)
            {
                //Reads in input and validates if it is valid
                userInput = Console.ReadLine();
                if (isValid(userInput))
                {
                    notCompleted = false;
                }
                else
                {
                    Console.Write(failurePrompt);
                }
            }
            //Used to keep retrival of input separate from whatever happens next
            Console.WriteLine();
            return userInput;
        }
        private void PrintToDoList(List<ToDoModel> toDoList)
        {
            for (int i = 0; i < toDoList.Count; i++)
            {
                ToDoModel toDoItem = toDoList[i];
                string output = $"  {i + 1}) {toDoItem.Message}" +
                    $"\nPriority: {(Priority)toDoItem.Priority}({toDoItem.Priority})";
                Console.WriteLine(output);
            }
        }
        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
