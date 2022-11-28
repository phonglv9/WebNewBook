using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;

namespace WebNewBook.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PhieuGiamSPController : Controller
    {
        private readonly IPhieuGiamSPService phieuGiamSPService;
        public PhieuGiamSPController(IPhieuGiamSPService phieuGiamSPService)
        {
            this.phieuGiamSPService = phieuGiamSPService;
        }
    }
}
