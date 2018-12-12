using System;
using System.Linq;
using System.Collections.Generic;

namespace ScrumCity
{
    public class Sprint
    {
        public string ID { get; set; }
        public Release ParentRelease { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpCompletionDate { get; set; }
        public DateTime ActCompletionDate { get; set; }

        public List<Feature> Features { get; set; }

        public Sprint(Release parent, string id=null, string title=null)
        {
            ParentRelease = parent;
            ID = id;
            Title = title;
        }

        public IEnumerable<Feature> getRelatedFeatures(string qname) =>
            Features.Where(f => f.AllRefs.Contains(qname));

        public IEnumerable<string> getFeatureTitles() =>
            Features.Select(f => f.Title);
    }
}
