using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CallApi
{
    public class ToDoApiClient: IDisposable
    {
        private readonly HttpClient _client;
        public ToDoApiClient(string uri)
        { 
            _client = new HttpClient();
            _client.BaseAddress = new Uri(uri);
        }
        public async Task<List<ToDoModel>> GetCompleteToDoListAsync(string urlAddition)
        {
            //Calls the Api
            HttpResponseMessage response = await _client.GetAsync(urlAddition);

            //Converts the returned response into a JArray
            string temp = await response.RequestMessage.Content.ReadAsStringAsync();
            JArray responseMessageContent = JArray.Parse(temp);

            return ConvertJArrayToToDoList(responseMessageContent);
        }
        public async Task<List<ToDoModel>> GetPriorityToDoListAsync(string urlAddition, Priority priority)
        {
            //Adds the priority to Api as a queryParam
            string queryParams = $"?priorityNum={(int)priority}";
            HttpResponseMessage response = await _client.GetAsync(urlAddition+queryParams);

            //Turns Response into a JArray
            string temp = await response.RequestMessage.Content.ReadAsStringAsync();
            JArray responseMessageContent = JArray.Parse(temp);

            return ConvertJArrayToToDoList(responseMessageContent);
        }
        public async Task AddToDoItemAsync(string urlAddition, string toDoMessage, Priority priorityNum)
        {
            //await _client.PostAsync(urlAddition, content);
        }
        public async Task DeleteToDoItemAsync(string urlAddition, int listNum)
        {
            //await _client.PostAsync(urlAddition, content);
        }

        private List<ToDoModel> ConvertJArrayToToDoList(JArray responseMessageContent)
        {
            List<ToDoModel> toDoList = new List<ToDoModel>();
            for (int i = 0; i < responseMessageContent.Count; i++)
            {
                JObject toDoItem = (JObject)responseMessageContent[i];

                int listNum = (int)toDoItem["listNum"];
                string toDoMessage = toDoItem["message"].ToString();
                int priorityNum = (int)toDoItem["priorityNum"];

                toDoList.Add(new ToDoModel(listNum, toDoMessage, priorityNum));
            }
            return toDoList;
        }
        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
