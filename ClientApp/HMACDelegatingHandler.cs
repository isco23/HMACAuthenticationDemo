using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ClientApp
{
    public class HMACDelegatingHandler : DelegatingHandler
    {
        private string APPId = "b9132ed1-4c9f-4a6e-a278-b21865c81df3";
        private string APIKey = "b3die3TkS8UJ7HMGlU5RCv/JmwaOJPL2gcLEnphF6w0=";
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request , CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            string requestContentBase64String = string.Empty;
            string requestUri = HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());
            string requestHttpMethod = request.Method.Method;
            DateTime start = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - start;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();
            string nonce = Guid.NewGuid().ToString("N");
            if (request.Content != null)
            {
                byte[] content = await request.Content.ReadAsByteArrayAsync();
                MD5 md5 = MD5.Create();
                byte[] requestContentHash = md5.ComputeHash(content);
                requestContentBase64String = Convert.ToBase64String(requestContentHash);
            }
            string signatureRawData = String.Format("{0}{1}{2}{3}{4}{5}",APPId,requestHttpMethod,requestUri,requestTimeStamp,nonce,requestContentBase64String);

            var secretKeyByteArray = Convert.FromBase64String(APIKey);
            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);
            using(HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("hmacauth", string.Format("{0}:{1}:{2}:{3}", APPId, requestSignatureBase64String, nonce, requestTimeStamp));
            }
            response = await base.SendAsync(request, cancellationToken);
            return response;            
        }
    }
}
