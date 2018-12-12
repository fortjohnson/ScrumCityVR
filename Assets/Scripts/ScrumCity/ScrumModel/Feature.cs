using System;
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
        public FeatureType Type { get; set; }
        public FeaturePriority Priority { get; set; }
        public string Category { get; set; }
        public Sprint ParentSprint { get; private set; }
        public float OgWorkEstimate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompleteDate { get; set; }

        public List<string> MethodRefs { get; set; }
        public List<string> ClassRefs { get; set; }
        public List<string> AllRefs { get { return MethodRefs.Concat(ClassRefs).ToList(); } }
        public List<WorkEntry> RemainWorkEntries { get; set; }
        public List<WorkEntry> CompleteWorkEntries { get; set; }
        public List<string> Tasks { get; set; }

        public Feature(Sprint parentSprint, string id = null, string title = null, float workEstimate = 0)
        {
            ID = id;
            Title = title;
            OgWorkEstimate = workEstimate;
            ParentSprint = parentSprint;

            MethodRefs = new List<string>();
            ClassRefs = new List<string>();
            RemainWorkEntries = new List<WorkEntry>();
            CompleteWorkEntries = new List<WorkEntry>();
            Tasks = new List<string>();
        }
    }
}
