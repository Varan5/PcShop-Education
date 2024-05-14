namespace Net14Web.Services.ApiServices
{
    public class FoxApi
    {
        private HttpClient _httpClient;

        public FoxApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<FoxDto?> GetRandomFoxUrl()
        {
            return _httpClient.GetFromJsonAsync<FoxDto>($"/api/?i=random");
        }
    }
}
