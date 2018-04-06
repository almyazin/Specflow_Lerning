using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Dto
{
    public class Event
    {
        public Guid CustomerId { get; set; }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public EventType Type { get; set; }

        public string Note { get; set; }

        public int ReOccurance { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Done { get; set; }

        public bool Important { get; set; }

        public bool AddedToOutlook { get; set; }

        public IEnumerable<Guid> Responsible { get; set; }
        public IEnumerable<Guid> Documents { get; set; }
        public IEnumerable<string> Links { get; set; }

        public IEnumerable<Guid> GetEventLinkedProjects()
        {
            throw new NotImplementedException();
        }
    }
}
