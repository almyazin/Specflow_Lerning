using Midaxo.Event.Test.Dto;
using Midaxo.Event.Test.Client;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Test_Testing
{
    [TestFixture]
    public class SPEventsTests
    {
        private MidaxoRestClient restClient;

        [SetUp]
        public void TestSetup()
        {
            restClient = new MidaxoRestClient(Constants.BaseUrl);
        }

        [Test]
        public async Task Test_Self()
        {
            await restClient.MidaxoSignIn("am.testing1@midevxo.com", "Passw0rd");
            var session = await restClient.MidaxoSelf();
            //Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            //var self_str = await res.Content.ReadAsStringAsync();
            //var self_json = JObject.Parse(self_str);
            //Console.WriteLine(self_str);
            //Console.WriteLine(self_json);
        }

        [Test]
        [TestCase(138322, 7, 3)]
        [TestCase(138322, 7, 2)]
        public async Task Test_Get_Event(int spSiteId, int projId, int eventId)
        {
            await restClient.MidaxoSignIn("am.testing1@midevxo.com", "Passw0rd");
            var @event = await restClient.GetEventById_SP(spSiteId, projId, eventId);

        }

        [Test]
        [TestCase(138322, 7)]
        public async Task Test_Query_Prohect_Events(int spSiteId, int projId)
        {
            await restClient.MidaxoSignIn("am.testing1@midevxo.com", "Passw0rd");
            var events = await restClient.ListProjectEvents_SP(spSiteId, projId);
        }

        [Test]
        [TestCase(138322, 7, 1)]
        public async Task Test_Query_Task_Events(int spSiteId, int projId, int taskId)
        {
            await restClient.MidaxoSignIn("am.testing1@midevxo.com", "Passw0rd");
            var events = await restClient.ListTaskEvents_SP(spSiteId, projId, taskId);
        }

        [Test]
        [TestCase(138322, 7, new int[] { 1, 2})]
        public async Task Test_Query_Tasks_Events(int spSiteId, int projId, IEnumerable<int> tasks)
        {
            await restClient.MidaxoSignIn("am.testing1@midevxo.com", "Passw0rd");
            var events = await restClient.ListTasksEvents_SP(spSiteId, projId, tasks);
        }

        [Test]
        [TestCase("7337c372-887a-4cc9-b402-9d60de633c1d")]
        public async Task Test_List_Process_Users(string processId)
        {
            await restClient.MidaxoSignIn("am.testing1@midevxo.com", "Passw0rd");
            var users = await restClient.ListProcessUsers(new Guid(processId));
        }

        [Test]
        [TestCase(138322)]
        public async Task Test_List_Process_Projects(int spSiteId)
        {
            await restClient.MidaxoSignIn("am.testing1@midevxo.com", "Passw0rd");
            var projects = await restClient.ListAllProcessProjects_SP(spSiteId);
        }

        [Test]
        [TestCase(138322)]
        public async Task Test_List_Active_Process_Projects(int spSiteId)
        {
            await restClient.MidaxoSignIn("am.testing1@midevxo.com", "Passw0rd");
            var projects = await restClient.ListActiveProcessProjects_SP(spSiteId);
        }

        [Test]
        [TestCase(138322, 7)]
        public async Task Test_List_Project_Tasks(int spSiteId, int projId)
        {
            await restClient.MidaxoSignIn("am.testing1@midevxo.com", "Passw0rd");
            var tasks = await restClient.ListProjectTasks_SP(spSiteId, projId);
        }

        [Test]
        [TestCase(138322, 7)]
        public async Task Test_Get_Project_Tasks_Permissions(int spSiteId, int projId)
        {
            await restClient.MidaxoSignIn("am.testing1@midevxo.com", "Passw0rd");
            var tasks = await restClient.GetProjectTasksPermissions_SP(spSiteId, projId);
        }

        [Test]
        [TestCase(138322, "am.testing1@midevxo.com", "Passw0rd")]
        public async Task Export_Events_From_SP(int spSite, string user, string password)
        {
            await restClient.MidaxoSignIn(user, password);

            var customerId = (await restClient.MidaxoSelf()).CustomerId;
            var processes = await restClient.ListCustomerProcesses(customerId);

            var evseEvents = new List<Event>();

            foreach (var process in processes)
            {
                var users = await restClient.ListProcessUsers(process.Id);
                var mapSettings = new MapProjectEventSettings()
                {
                    CustomerId = customerId,
                    ProcessId = process.Id,
                    Users = users
                };
                var projects = await restClient.ListAllProcessProjects_SP(process.SharepointSiteNumber);
                Console.WriteLine($"Process: {process.Title} (projects: {projects.Count()})");
                foreach (var project in projects)
                {
                    var events = await restClient.ListAllProjectEvents_SP(process.SharepointSiteNumber, project.Id);
                    if (events.Count() == 0)
                        continue;

                    var tasks = await restClient.ListProjectTasks_SP(process.SharepointSiteNumber, project.Id);
                    Console.WriteLine($"Project: {project.Title} (tasks: {tasks.Count()} events: {events.Count()})");
                    mapSettings.ProjectId = project.Guid;
                    mapSettings.Tasks = tasks;
                    var evseProjEvents = Mappers.MapProjectEvents(events, mapSettings);
                    evseEvents.AddRange(evseProjEvents);
                }
            }

            Console.WriteLine(evseEvents.Count);
        }
    }
}
