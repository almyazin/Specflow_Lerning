using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Dto.SPDto
{

    public class Event
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public EventType Type { get; set; }

        public string Note { get; set; }

        public bool IsRecycled { get; set; }

        public IDictionary<string, string> Tasks { get; set; }

        public IDictionary<string, string> Documents { get; set; }

        public IEnumerable<int> ResponsibleIds { get; set; }

        public int ReOccurance { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Done { get; set; }

        public bool Important { get; set; }

        public bool AddedToOutlook { get; set; }

        public int CreatedById { get; set; }

        public int ModifiedById { get; set; }

        public int ProjectId { get; set; }

        public string ProjectTitle { get; set; }

        public object CreatedBy { get; set; }

        public object ModifiedBy { get; set; }

        public DateTime Modified { get; set; }

        public DateTime Created { get; set; }

        public int Role { get; set; }
    }

}
