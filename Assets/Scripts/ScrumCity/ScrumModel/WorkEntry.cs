using System;
using System.Linq;
using System.Collections.Generic;


namespace ScrumCity
{ 
    public enum WorkEntryType {Completed, Remaining};

    public class WorkEntry
    {
        public string Qname { get; private set; }
        public float Hours { get; private set; }
        public DateTime Date { get; private set; }
        public WorkEntryType Type { get; private set; }

        public WorkEntry(String qName, DateTime date, float hours, WorkEntryType type)
        {
            Qname = qName;
            Date = date;
            Hours = hours;
            Type = type;
        }
    }
}