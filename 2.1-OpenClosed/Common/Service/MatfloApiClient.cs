using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using _2._1_OpenClosed.Common.ApiModels;

namespace _2._1_OpenClosed.Common.Service
{
    public class MatfloApiClient : IMatfloApi
    {
        private static string _iwtApiUrl = string.Empty;

        public MatfloApiClient(string iwtApiUrl)
        {
            _iwtApiUrl = iwtApiUrl;
        }

        public async Task<MatfloResponse> Callback(StockCallbackRequest request)
        {
            var response = await MakeRequest(request).ConfigureAwait(false);

            response.RequestMessage = request;

            return response;
        }

        private static async Task<MatfloResponse> MakeRequest(StockCallbackRequest request)
        {
            using (var httpClient = new HttpClient { BaseAddress = new Uri(_iwtApiUrl) })
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var httpResponse = await httpClient.PostAsJsonAsync("callback", request).ConfigureAwait(false);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return default(MatfloResponse);
                }

                var content = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<MatfloResponse>(content);
            }

        }
    }
}
