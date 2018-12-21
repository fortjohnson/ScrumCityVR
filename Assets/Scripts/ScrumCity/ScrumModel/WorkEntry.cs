using System;
using System.Linq;
using System.Collections.Generic;


namespace ScrumCity
{ 
    public enum WorkEntryType {Completed, Remaining};

    public class WorkEntry
    {
        public string QName { get; set; }
        public float Hours { get; set; }
        public DateTime Date { get; set; }
        public WorkEntryType Type { get; set; }

        public WorkEntry() { }

        public WorkEntry(String qName, DateTime date, float hours, WorkEntryType type)
        {
            QName = qName;
            Date = date;
            Hours = hours;
            Type = type;
        }
    }
}