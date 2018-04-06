using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Dto.SPDto
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Author { get; set; }
        public DateTime Created { get; set; }
        public int Editor { get; set; }
        public DateTime Modified { get; set; }
        public Guid Guid { get; set; }
        public int ProjectType { get; set; }
        public bool Archived { get; set; }
        public string ProjectURL { get; set; }
        public int? ParentId { get; set; }
        public int? OwnerId { get; set; }
    }
}
