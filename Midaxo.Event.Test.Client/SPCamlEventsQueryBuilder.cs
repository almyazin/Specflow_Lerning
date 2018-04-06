using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Test.Client
{
    public class SPCamlEventsQueryBuilder : IEventsQueryBuilder
    {
        private int? _is_recycled;
        private int? _done;

        public bool? IsRecycled
        {
            get => _is_recycled.HasValue ? Convert.ToBoolean(_is_recycled) : (bool?)null;
            set => _is_recycled = Convert.ToInt32(value);
        }

        public bool? Done {
            get => _done.HasValue ? Convert.ToBoolean(_done) : (bool?)null;
            set => _done = Convert.ToInt32(value);
        }

        public List<int> Tasks { get; set; }

        public SPCamlEventsQueryBuilder()
        {
        }

        public SPCamlEventsQueryBuilder(IEnumerable<int> tasks)
        {
            Tasks = tasks.ToList();
        }

        public SPCamlEventsQueryBuilder(int task)
        {
            Tasks = new List<int> { task };
        }

        public SPCamlEventsQueryBuilder(bool? isRecycled, bool? done, IEnumerable<int> tasks)
        {
            IsRecycled = isRecycled;
            Done = done;
            Tasks = tasks?.ToList();
        }

        public SPCamlEventsQueryBuilder(bool? isRecycled, bool? done)
        {
            IsRecycled = isRecycled;
            Done = done;
        }

        public void AddProperties(IEnumerable<string> properties)
        {
            throw new NotImplementedException();
        }

        public void AddProperty(string property)
        {
            throw new NotImplementedException();
        }

        public void AddTask(int id)
        {
            if (Tasks == null)
                Tasks = new List<int>();
            Tasks.Add(id);
        }

        public void AddTask(string id)
        {
            if (Tasks == null)
                Tasks = new List<int>();
            Tasks.Add(int.Parse(id));
        }

        public void AddTasks(IEnumerable<int> ids)
        {
            if (Tasks == null)
                Tasks = new List<int>();
            Tasks.AddRange(ids);
        }

        public void AddTasks(IEnumerable<string> ids)
        {
            if (Tasks == null)
                Tasks = new List<int>();
            Tasks.AddRange(ids.Select(id => int.Parse(id)));
        }

        public override string ToString()
        {
            const string caml_query_template = @"<Where>{0}{4}{2}{3}{1}</Where>";
            const string caml_tasks_template = @"<In><FieldRef Name=""ActionTasks"" LookupId=""True"" /><Values>{0}</Values></In>";
            const string caml_task_value = @"<Value Type=""Integer"">{0}</Value>";
            var is_recycled_query = _is_recycled.HasValue ? string.Format(@"<Eq><FieldRef Name=""IsRecycled"" /><Value Type=""Integer"">{0}</Value></Eq>", _is_recycled) : string.Empty;
            var done_query = _done.HasValue ? string.Format(@"<Eq><FieldRef Name=""Done"" /><Value Type=""Integer"">{0}</Value></Eq>", _done) : string.Empty;
            var tasks_caml_query = Tasks != null ? string.Format(caml_tasks_template, string.Join(string.Empty, Tasks.Select(id => string.Format(caml_task_value, id)).ToList())) : string.Empty;
            //var add_and = !string.IsNullOrEmpty(is_recycled_query) || !string.IsNullOrEmpty(done_query) || !string.IsNullOrEmpty(tasks_caml_query);
            var parts = 0;
            if (!string.IsNullOrEmpty(is_recycled_query)) parts++;
            if (!string.IsNullOrEmpty(done_query)) parts++;
            if (!string.IsNullOrEmpty(tasks_caml_query)) parts++;
            var events_query = string.Format(caml_query_template, parts > 1 ? "<And>" : string.Empty, parts > 1 ? "</And>" : string.Empty, is_recycled_query, done_query, tasks_caml_query);

            return events_query;
        }
    }
}
