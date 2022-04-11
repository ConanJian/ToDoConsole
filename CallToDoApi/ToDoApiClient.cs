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
        private readonly ApiClient _client;
        private static readonly Configuration _config = new Configuration();
        public ToDoApiClient()
        {
            _client = new ApiClient(_config.BaseUrl);
        }
        public async Task<List<ToDoModel>> GetCompleteToDoListAsync()
        {
            //Calls the Api
            HttpResponseMessage response = await _client.SuccessfulGetRequest(_config.CompleteToDoListUrl);

            //Converts the returned response into a JArray
            string temp = await response.Content.ReadAsStringAsync();
            JArray responseMessageContent = JArray.Parse(temp);

            return ConvertJArrayToToDoList(responseMessageContent);
        }
        public async Task<List<ToDoModel>> GetPriorityToDoListAsync(Priority priority)
        {
            //Adds the priority to Api as a queryParam
            string queryParams = $"?priority={(int)priority}";
            HttpResponseMessage response = await _client.SuccessfulGetRequest(_config.PriorityToDoListUrl+queryParams);

            //Turns Response into a JArray
            string temp = await response.Content.ReadAsStringAsync();
            JArray responseMessageContent = JArray.Parse(temp);

            return ConvertJArrayToToDoList(responseMessageContent);
        }
        public async Task AddToDoItemAsync( string toDoMessage, Priority priorityNum)
        {
            string queryParams = $"?message={toDoMessage}&priority={(int)priorityNum}";
            await _client.SuccessfulPostRequest(_config.CreateToDoListUrl+queryParams, new StringContent(""));
        }
        public async Task DeleteToDoItemAsync(int listNum)
        {
            string queryParams = $"?listNum={listNum}";
            await _client.SuccessfulPostRequest(_config.DeleteToDoListUrl+queryParams, new StringContent(""));
        }
        public async Task<int> GetMostRecentListNum()
        {
            HttpResponseMessage response = await _client.SuccessfulGetRequest(_config.GetLatestListNumUrl);
            string listNum =  await response.Content.ReadAsStringAsync();
            return int.Parse(listNum);
        }

        private List<ToDoModel> ConvertJArrayToToDoList(JArray responseMessageContent)
        {
            List<ToDoModel> toDoList = new List<ToDoModel>();
            for (int i = 0; i < responseMessageContent.Count; i++)
            {
                JObject toDoItem = (JObject)responseMessageContent[i];

                int listNum = (int)toDoItem["ListNum"];
                string toDoMessage = toDoItem["Message"].ToString();
                int priorityNum = (int)toDoItem["Priority"];

                toDoList.Add(new ToDoModel(listNum, toDoMessage, (Priority)priorityNum));
            }
            return toDoList;
        }
        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
