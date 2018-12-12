using System;
using System.Linq;
using System.Collections.Generic;

namespace ScrumCity
{
    public class Release
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpCompletionDate { get; set; }
        public DateTime ActCompletionDate { get; set; }
        public float TotalWorkTime { get; set; }

        public List<Sprint> Sprints { get; set; }

        public Release(string id, string title)
        {
            ID = id;
            Title = title;

            Sprints = new List<Sprint>();
        }

        public IEnumerable<Feature> getRelatedFeatures(string qname) =>
            Sprints.SelectMany(s => s.getRelatedFeatures(qname));

        public List<string> getSprintTitles() =>
            Sprints.Select(s => s.Title).ToList();
    }
}