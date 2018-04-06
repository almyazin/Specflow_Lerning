using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Dto
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public IEnumerable<ProcessMembership> ProcessMemberships { get; set; }

        public int GetSharepointIdByProcess(Guid processId) => ProcessMemberships.First(m => m.ProcessId == processId).SharepointId;
    }
}
