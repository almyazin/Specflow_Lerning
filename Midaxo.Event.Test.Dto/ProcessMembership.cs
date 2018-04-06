using System;

namespace Midaxo.Event.Test.Dto
{
    public class ProcessMembership
    {
        public Guid ProcessId { get; set; }

        public int SharepointSiteNumber { get; set; }

        public Guid SharepointSiteId { get; set; }

        public int SharepointId { get; set; }

        public int ProcessRole { get; set; }

        public Guid RoleId { get; set; }
    }
}