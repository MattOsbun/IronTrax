using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Irontrax.Services
{
    public static class ExtensionMethods
    {
        public static void CleanAdd(this HttpClient httpClient, string headerName, string headerValue)
        {
            httpClient
                .DefaultRequestHeaders
                .Remove(headerName);
            httpClient
                .DefaultRequestHeaders
                .Add(headerName, headerValue);
        }
        public static async Task<T> ApiGet<T>(this HttpClient httpClient, string apiAddress)
        {
            if (Uri.TryCreate(apiAddress, UriKind.Absolute, out Uri uri))
            {
                using var httpResponseMessage = await httpClient.GetAsync(uri);
                httpResponseMessage.EnsureSuccessOrNotFound();
                return await httpResponseMessage
                    .Content
                    .ReadAsAsync<T>(new MediaTypeFormatter[] { new JsonMediaTypeFormatter() });
            }

            throw new ArgumentException("Invalid URI");
        }

        public static async Task<TReturn> ApiPost<TPost,TReturn>(this HttpClient httpClient, string apiAddress, TPost postData)
        {
            if (Uri.TryCreate(@$"{apiAddress}", UriKind.Absolute, out Uri uri))
            {
                using var httpResponseMessage = await httpClient.PostAsJsonAsync(
                    uri.ToString(),
                    postData);
                httpResponseMessage.EnsureSuccessStatusCode();
                return await httpResponseMessage
                    .Content
                    .ReadAsAsync<TReturn>(new MediaTypeFormatter[] { new JsonMediaTypeFormatter() });
            }

            throw new ArgumentException("Invalid URI");
        }

        public static async Task<TReturn> ApiPut<TPut, TReturn>(this HttpClient httpClient, string apiAddress, TPut postData)
        {
            if (Uri.TryCreate(@$"{apiAddress}", UriKind.Absolute, out Uri uri))
            {
                using var httpResponseMessage = await httpClient.PutAsJsonAsync(
                    uri.ToString(),
                    postData);
                httpResponseMessage.EnsureSuccessStatusCode();
                return await httpResponseMessage
                    .Content
                    .ReadAsAsync<TReturn>(new MediaTypeFormatter[] { new JsonMediaTypeFormatter() });
            }

            throw new ArgumentException("Invalid URI");
        }

        public static void EnsureSuccessOrNotFound(this HttpResponseMessage httpResponseMessage)
        {
            if(httpResponseMessage.IsSuccessStatusCode || httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return;
            }

            throw new HttpRequestException($"Azure function call returned status {httpResponseMessage.StatusCode}");
        }
    }
}
