using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebNewBook.Model;
using WebNewBook.Services;

namespace WebNewBook.Component
{
    
    public class HeaderListViewComponent : ViewComponent
    {
        private IHeaderService _headerService;
        public HeaderListViewComponent(IHeaderService headerService)
        {
            _headerService = headerService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headers = await _headerService.GetDMAsync();
            return View(headers);
        }
    }
}
