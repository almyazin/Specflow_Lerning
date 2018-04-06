using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Dto
{
    public class Session
    {
        public Guid UserId { get; set; }

        public Guid CustomerId { get; set; }
    }
}
