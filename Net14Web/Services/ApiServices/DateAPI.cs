namespace Net14Web.Services.ApiServices
{
    public class DateApi
    {
        private HttpClient _httpClient;

        public DateApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<string> GetFactAboutDate(int Month = 6, int Day = 2)
        {
            return _httpClient.GetStringAsync($"/{Month}/{Day}/date");
        }

    }
}
