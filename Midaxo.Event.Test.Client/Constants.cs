using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Test.Client
{
    public static class Constants
    {
        public const string BaseUrl = @"https://test.midevxo.net";
        public const string AuthUri = @"api/auth";
        public const string SignInUri = AuthUri + @"/sign_in";
        public const string SelfUri = AuthUri + @"/self";

        public const string SPEventServiceUri = @"spapi/{0}/{1}/_vti_bin/Midaxo/v2/MidaxoActionServiceREST.svc";
        public const string SPGetEventEndpoint = SPEventServiceUri + @"/Get";

        public const string ProcessesEndpoint = @"api/customers/{0}/processes";

        public const string CustomerUsersEndpointUri = @"api/customers/{0}/users";
        public const string ProcessUsersEndpointUri = @"api/processes/{0}/users";

        public const string SPProjectServiceUri = @"spapi/{0}/_vti_bin/Midaxo/v2/MidaxoProjectServiceREST.svc";
        public const string SPTaskServiceUri = @"spapi/{0}/{1}/_vti_bin/Midaxo/v2/MidaxoTaskServiceREST.svc/ListTasksWithUserRole";
        public const string SPProjectTaskPermissionsEndpoint = @"spapi/{0}/{1}/_vti_bin/Midaxo/v2/MidaxoProjectServiceREST.svc/GetProjectTaskPermissions";
    }
}
