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
                int listNum = await _client.GetMostRecentListNum();
                toDoListCache.Add(new ToDoModel(listNum, message, (Priority)priority));
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine("Error, get most recent list num failed to work");
#endif
                toDoListCache = await _client.GetCompleteToDoListAsync();
            }
        }
        //public async Task DeleteToDoItem(int uiListNum)
        //{
        //    //The number that the client sees is the indexNum of the cache +1
        //    int cacheIndexNum = uiListNum - 1;
        //    await _client.DeleteToDoItemAsync(cacheIndexNum);
        //    toDoListCache.RemoveAt(cacheIndexNum);
        //}
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
