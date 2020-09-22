using System.Net.Http;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace HMACAuthenticationWebApi.Models
{
    internal class ResultWithChallenge : IHttpActionResult
    {
        private readonly IHttpActionResult next;
        private readonly string authenticationScheme = "hmacauth";

        public ResultWithChallenge(IHttpActionResult next)
        {
            this.next = next;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await next.ExecuteAsync(cancellationToken);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                response.Headers.WwwAuthenticate.Add(new System.Net.Http.Headers.AuthenticationHeaderValue(authenticationScheme));
            }
            return response;
        }       
    }
}