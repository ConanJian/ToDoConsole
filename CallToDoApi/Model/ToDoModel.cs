﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CallApi
{
    public readonly struct ToDoModel
    {
        private readonly int listNum;
        private readonly string message;
        private readonly int priority;

        public int ListNum { get { return listNum; } }
        public string Message { get { return message; } }
        public int Priority { get { return priority; } }
        public ToDoModel(int listNum, string message, Priority priority)
        { 
            this.listNum = listNum;
            this.message = message;
            this.priority = (int)priority;
        }
    }
    public enum Priority
    {
        Utmost = 1,
        Do_Soon = 2,
        Can_Wait = 3
    }
}
