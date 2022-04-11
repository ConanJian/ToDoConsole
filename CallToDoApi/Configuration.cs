using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallApi
{
    internal class Configuration
    {
        private readonly IConfiguration _config = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "config.json"))
            .Build();
        public Configuration() { }
        public string BaseUrl { get { return GetValue("baseUrl"); } }
        public string CompleteToDoListUrl
        {
            get { return GetValue("toDoUrlAdditions:GetEntireList"); }
        }
        public string PriorityToDoListUrl
        {
            get { return GetValue("toDoUrlAdditions:GetPriorityList"); }
        }
        public string CreateToDoListUrl
        {
            get { return GetValue("toDoUrlAdditions:CreateItem"); }
        }
        public string DeleteToDoListUrl
        {
            get { return GetValue("toDoUrlAdditions:DeleteItem"); }
        }
        public string GetLatestListNumUrl
        {
            get { return GetValue("toDoUrlAdditions:GetLatestListNum"); }
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
