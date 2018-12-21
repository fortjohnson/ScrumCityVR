using System;
using System.Xml.Serialization;
using System.Linq;
using System.Collections.Generic;

namespace ScrumCity
{
    public enum FeatureType { Feature, BugFix, Enhancement };
    public enum FeaturePriority { High, Normal, Low };

    public class Feature
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public FeatureType Type { get; set; }
        public FeaturePriority Priority { get; set; }
        public string Category { get; set; }
        public Sprint ParentSprint { get; set; }
        public float OriginalWorkEstimate { get; set; }
        public DateTime CommenceDateTime { get; set; }
        public DateTime CompletionDateTime { get; set; }

        [XmlArray]
        [XmlArrayItem(ElementName = "MethodRef")]
        public List<string> MethodRefs { get; set; }

        [XmlArray]
        [XmlArrayItem(ElementName = "ClassRef")]
        public List<string> ClassRefs { get; set; }

        public List<string> AllRefs { get { return MethodRefs.Concat(ClassRefs).ToList(); } }
        public List<WorkEntry> WorkEntries { get; set; }
        public List<WorkEntry> RemainingWorkEntries {
            get
            {
                return WorkEntries.Where(s => s.Type == WorkEntryType.Remaining).ToList();
            }
        }
        public List<WorkEntry> CompletedWorkEntries
        {
            get
            {
                return WorkEntries.Where(s => s.Type == WorkEntryType.Completed).ToList();
            }
        }
        public List<string> Tasks { get; set; }

        public Feature() { }

        public Feature(Sprint parentSprint, string id = null, string title = null, float workEstimate = 0)
        {
            ID = id;
            Title = title;
            OriginalWorkEstimate = workEstimate;
            ParentSprint = parentSprint;

            MethodRefs = new List<string>();
            ClassRefs = new List<string>();
            WorkEntries = new List<WorkEntry>();
            Tasks = new List<string>();
        }
    }
}
