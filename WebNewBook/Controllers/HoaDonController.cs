using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using WebNewBook.Model;
using X.PagedList;

namespace WebNewBook.Controllers
{
    public class HoaDonController : Controller
    {
        Uri link = new Uri("https://localhost:7266/api");
        HttpClient client;
        public HoaDonController()
        {
            client = new HttpClient();
            client.BaseAddress = link;
        }
        public ActionResult Index(int? page, int? pagesize )
        {
            //if(page == null)
            //{
            //    page = 1;
            //}
            //if (pagesize == null)
            //{
            //    pagesize = 3;
            //}
            List<ViewHoadon> lissttl = new List<ViewHoadon>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/HoaDon").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                lissttl = JsonConvert.DeserializeObject<List<ViewHoadon>>(data);
            }
            //return View("IndexHD", lissttl.ToPagedList((int)page,(int)pagesize));
            return View("IndexHD", lissttl);
        }
    }
}
