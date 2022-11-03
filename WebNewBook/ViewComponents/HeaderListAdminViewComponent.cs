using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebNewBook.Services;

namespace WebNewBook.ViewComponents
{
    public class HeaderListAdminViewComponent:ViewComponent
    {
        private IHeaderService _headerService;
        public HeaderListAdminViewComponent(IHeaderService headerService)
        {
            _headerService = headerService;
        }
        public async Task< IViewComponentResult> InvokeAsync()
        {
            HttpClient _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7266/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",Request.Cookies["token"]);
            return View();
        }
    }
}
