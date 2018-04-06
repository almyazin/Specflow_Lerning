using Midaxo.Event.Test.Client.MessageHandlers;
using Midaxo.Event.Test.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Test.Client
{
    public class MidaxoRestClient
    {
        private MidaxoHttpClient _client;

        public MidaxoRestClient(string baseUrl)
        {
            _client = CreateHttpClient(baseUrl);
        }

        public async Task MidaxoSignIn(string username, string password)
        {
            var authData = new { username = username, password = password };
            var authContent = GetHttpContent(authData);

            var resp = await _client.PostAsync(Constants.SignInUri, authContent);
            resp.EnsureSuccessStatusCode();
        }

        public async Task<Session> MidaxoSelf()
        {
            var resp = await _client.GetAsync(Constants.SelfUri);
            resp.EnsureSuccessStatusCode();

            return await GetEntityFromResponse<Session>(resp);
        }

        public async Task<Dto.SPDto.Event> GetEventById_SP(int spSiteId, int projId, int id)
        {
            var uri = string.Format(Constants.SPEventServiceUri, spSiteId, projId) + string.Format(@"/Get?id={0}", id);
            var resp = await _client.GetAsync(uri);

            return await GetEntityFromResponse<Dto.SPDto.Event>(resp);
        }

        public async Task<IEnumerable<Dto.SPDto.Event>> QueryEvents_SP(int spSiteId, int projId, string query)
        {
            var uri = string.Format(Constants.SPEventServiceUri, spSiteId, projId) + @"/Query";

            var resp = await _client.PostAsync(uri, GetHttpContent(MakeQueryDictionary(query)));

            return await GetEntitiesListFromQuery<Dto.SPDto.Event>(resp);
        }

        public async Task<IEnumerable<Dto.SPDto.Event>> ListEvents_SP(int spSiteId, int projId, bool? is_recycled, bool? done, IEnumerable<int> tasks)
        {
            var query = new SPCamlEventsQueryBuilder(is_recycled, done, tasks).ToString();

            return await QueryEvents_SP(spSiteId, projId, query);
        }

        /// <summary>
        /// Lists all project events including Recycled and Done
        /// </summary>
        /// <param name="spSiteId"></param>
        /// <param name="projId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Dto.SPDto.Event>> ListAllProjectEvents_SP(int spSiteId, int projId)
        {
            var query = new SPCamlEventsQueryBuilder().ToString();

            return await QueryEvents_SP(spSiteId, projId, query);
        }

        public async Task<IEnumerable<Dto.SPDto.Event>> ListProjectEvents_SP(int spSiteId, int projId, bool is_recycled = false)
        {
            var query = new SPCamlEventsQueryBuilder() { IsRecycled = is_recycled }.ToString();

            return await QueryEvents_SP(spSiteId, projId, query);
        }

        public async Task<IEnumerable<Dto.SPDto.Event>> ListTaskEvents_SP(int spSiteId, int projId, int taskId, bool is_recycled = false)
        {
            var query = new SPCamlEventsQueryBuilder(taskId) { IsRecycled = is_recycled }.ToString();

            return await QueryEvents_SP(spSiteId, projId, query);
        }

        public async Task<IEnumerable<Dto.SPDto.Event>> ListTasksEvents_SP(int spSiteId, int projId, IEnumerable<int> tasks, bool is_recycled = false)
        {
            var query = new SPCamlEventsQueryBuilder(tasks) { IsRecycled = is_recycled }.ToString();

            return await QueryEvents_SP(spSiteId, projId, query);
        }

        public async Task<IEnumerable<User>> ListProcessUsers(Guid processId)
        {
            var uri = string.Format(Constants.ProcessUsersEndpointUri, processId);
            var resp = await _client.GetAsync(uri);

            var users = await GetEntityFromResponse<IEnumerable<Dto.User>>(resp);

            return users;
        }

        public async Task<IEnumerable<Dto.Process>> ListCustomerProcesses(string customerId)
        {
            var uri = string.Format(Constants.ProcessesEndpoint, customerId);

            var resp = await _client.GetAsync(uri);

            return await GetEntityFromResponse<IEnumerable<Dto.Process>>(resp);
        }

        public async Task<IEnumerable<Dto.Process>> ListCustomerProcesses(Guid customerId)
        {
            return await ListCustomerProcesses(customerId.ToString());
        }

        public async Task<IEnumerable<Dto.SPDto.Project>> QueryProcessProjects_SP(int spSiteId, string query)
        {
            var uri = string.Format(Constants.SPProjectServiceUri, spSiteId) + @"/Query";

            var resp = await _client.PostAsync(uri, GetHttpContent(MakeQueryDictionary(query)));

            var res = await GetEntitiesListFromQuery<Dto.SPDto.Project>(resp);
            return res;
        }

        /// <summary>
        /// Lists all projects in process including archived and templates
        /// </summary>
        /// <param name="spSiteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Dto.SPDto.Project>> ListAllProcessProjects_SP(int spSiteId)
        {
            var query = @"<Query></Query>";

            return await QueryProcessProjects_SP(spSiteId, query);
        }

        /// <summary>
        /// List active projects in process (excluding archived and templates)
        /// </summary>
        /// <param name="spSiteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Dto.SPDto.Project>> ListActiveProcessProjects_SP(int spSiteId)
        {
            var query = @"<Where><And><Eq><FieldRef Name='ProjectType' /><Value Type='Integer'>0</Value></Eq><Neq><FieldRef Name='Archived' /><Value Type='Integer'>1</Value></Neq></And></Where>";

            return await QueryProcessProjects_SP(spSiteId, query);
        }

        public async Task<IEnumerable<Dto.SPDto.Task>> ListProjectTasks_SP(int spSiteId, int projId)
        {
            var uri = string.Format(Constants.SPTaskServiceUri, spSiteId, projId);

            var resp = await _client.GetAsync(uri);

            return await GetEntitiesListFromQuery<Dto.SPDto.Task>(resp);
        }

        public async Task<IDictionary<string, IEnumerable<Dto.SPDto.TaskPA>>> GetProjectTasksPermissions_SP(int spSiteId, int projId)
        {
            var uri = string.Format(Constants.SPProjectTaskPermissionsEndpoint, spSiteId, projId);

            var resp = await _client.GetAsync(uri);

            return await GetEntityFromResponse<IDictionary<string, IEnumerable<Dto.SPDto.TaskPA>>>(resp);
        }

        #region Content methods
        private async Task<T> GetEventFromResponse<T>(HttpResponseMessage resp) => await GetEntityFromResponse<T>(resp);

        private async Task<IEnumerable<T>> GetEntitiesListFromQuery<T>(HttpResponseMessage resp) => (await GetEntityFromResponse<IDictionary<string, T>>(resp)).Values.ToList();

        private async Task<T> GetEntityFromResponse<T>(HttpResponseMessage resp) => JsonConvert.DeserializeObject<T>(await resp.Content.ReadAsStringAsync());

        private HttpContent GetHttpContent(object data) => new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, new MediaTypeHeaderValue("application/json").MediaType);
        #endregion

        #region Private methods
        private MidaxoHttpClient CreateHttpClient(string baseUrl)
        {
            CookieContainer cookies_ = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler()
            {
                CookieContainer = cookies_,
                Proxy = new WebProxy(@"http://localhost:8888", false),
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            //var handlersPipeline = new XSRFAuthMessageHandler(cookies_)
            //{ InnerHandler = handler };
            var handlersPipeline = handler
                .DecorateWith(new XSRFAuthMessageHandler(cookies_));

            return new MidaxoHttpClient(handlersPipeline)
            {
                BaseAddress = new Uri(baseUrl)
            };
        }
        private IDictionary<string, string> MakeQueryDictionary(string query, string properties = null)
        {
            var res = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(query))
                res.Add("query", query);
            if (!string.IsNullOrEmpty(properties))
                res.Add("properties", properties);

            return res;
        }
        #endregion
    }
}
