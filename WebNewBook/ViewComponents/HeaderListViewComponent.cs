using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebNewBook.Model;
using WebNewBook.Models;
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
            List<CartItem> data = new List<CartItem>();
            //var opt = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
            var jsonData = Request.Cookies["Cart"];
            if (jsonData != null)
            {
                data = JsonConvert.DeserializeObject<List<CartItem>>(jsonData);
            }
            var b = data.Where(n => n.Tensp != null).Select(n => n.Tensp).ToList();
            ViewBag.giohang = data;
            ViewBag.thanhtien = data.Sum(a => a.ThanhTien);
            ViewBag.soluong = data.Sum(a => a.Soluong);
            ViewBag.count = b.Count;
            ViewBag.giohang = data;
            return View(headers);
        }
    }
}
