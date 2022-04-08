using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoConsole
{
    internal class Configuration
    {
        private readonly IConfiguration _config = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "config.json"))
            .Build();
        public Configuration() { }
        public string GetToDoListUrl()
        {
            return GetValue("toDoUrlAdditions:ToDo/GetEntireToDoList");
        }
        public string GetPriorityToDoListUrl()
        {
            return GetValue("toDoUrlAdditions:ToDo/GetPriorityList");
        }
        public string GetCreateToDoListUrl()
        {
            return GetValue("toDoUrlAdditions:ToDo/CreateToDoItem");
        }
        public string GetDeleteToDoListUrl()
        {
            return GetValue("toDoUrlAdditions:ToDo/DeleteToDoItem");
        }

        private string GetValue(string jsonPath)
        {
            string value = _config[jsonPath];
            if (value != null)
            { 
                return value;
            }
            else
            {
                throw new Exception($"Setting [{jsonPath}] is not configured");
            }
        }
    }
}
