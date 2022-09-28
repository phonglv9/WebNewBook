using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebNewBook.Model;

namespace WebNewBook.Services
{
	public class HeaderService:IHeaderService
	{
        Uri baseAdress = new Uri("https://localhost:7266/api");
        HttpClient _httpClient;
        public HeaderService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAdress;
        }
        public async Task<List<DanhMucSach>> GetDMAsync()
        {
            //DanhMuc
            List<DanhMucSach> danhMucSaches = new List<DanhMucSach>();
            HttpResponseMessage responseDM = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/DanhMuc").Result;
            if (responseDM.IsSuccessStatusCode)
            {
                string jsonData = responseDM.Content.ReadAsStringAsync().Result;
                danhMucSaches = JsonConvert.DeserializeObject<List<DanhMucSach>>(jsonData);
               

            };
            return danhMucSaches;
        }
    }
}
