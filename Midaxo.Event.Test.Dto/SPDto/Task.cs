using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Test.Dto.SPDto
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Author { get; set; }
        public DateTime Created { get; set; }
        public int Editor { get; set; }
        public DateTime Modified { get; set; }
        public Guid Guid { get; set; }
        public int ChildOrder { get; set; }
        public int ParentId { get; set; }
        public IEnumerable<int> Responsible { get; set; }
        public int? TaskType { get; set; }
        public bool IsRecycled { get; set; }
        public int Role { get; set; }
    }
}
