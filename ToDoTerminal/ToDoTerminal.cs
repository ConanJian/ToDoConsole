using CallApi;
using System;

namespace ToDoTerminal
{
    public class ToDoTerminal
    {
        private readonly ToDoApiClient _client;
        public ToDoTerminal(string uri)
        {
            _client = new ToDoApiClient(uri);
        }
        public void PrintCompleteToDoList()
        { 
            
        }
    }
}
