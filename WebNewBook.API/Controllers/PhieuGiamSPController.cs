using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    public class PhieuGiamSPController : Controller
    {
        private readonly IPhieuGiamSPService phieuGiamSPService;
        public PhieuGiamSPController(IPhieuGiamSPService phieuGiamSPService)
        {
            this.phieuGiamSPService = phieuGiamSPService;
        }

        [HttpGet]
        public async Task<IEnumerable<PhieuGiamGiaSP>> GetPhieuGiamGiaSPAsync()
        {
            var nv = await phieuGiamSPService.GetPhieuGiamGiaSPAsync();
            return nv;
        }

        [HttpGet("{id}")]
        public async Task<PhieuGiamGiaSP?> GetPhieuGiamGiaSPAsync(string id)
        {
            var nv = await phieuGiamSPService.GetPhieuGiamGiaSPAsync(id);
            return nv;
        }

        [HttpPost]
        public async Task<ActionResult> AddPhieuGiamGiaSPAsync(PhieuGiamGiaSP nv)
        {
            try
            {
                await phieuGiamSPService.AddPhieuGiamGiaSPAsync(nv);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return Ok();
        }
    }
}
