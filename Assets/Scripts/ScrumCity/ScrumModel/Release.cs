using System;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Collections.Generic;

namespace ScrumCity
{
    [XmlRootAttribute("Release", Namespace = "nz.ac.aut.scrumcity")]
    public class Release
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedCompletionDate { get; set; }
        public DateTime ActualCompletionDate { get; set; }
        public float TotalWorkTime { get; set; }

        public List<Sprint> Sprints { get; set; }

        public Release() { }

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