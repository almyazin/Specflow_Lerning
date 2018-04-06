using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Test.Dto
{

    public class Process
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SharepointSiteNumber { get; set; }
        public Guid SharepointSiteId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsVdrEnabled { get; set; }
    }

}
