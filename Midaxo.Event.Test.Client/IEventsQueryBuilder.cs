using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Test.Client
{
    interface IEventsQueryBuilder
    {
        void AddTask(int id);
        void AddTask(string id);
        void AddTasks(IEnumerable<int> ids);
        void AddTasks(IEnumerable<string> ids);
        void AddProperty(string property);
        void AddProperties(IEnumerable<string> properties);
    }
}
