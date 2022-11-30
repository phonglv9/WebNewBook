using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.API.Repository.Service;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FpointController : ControllerBase
    {
        public readonly IFpointService _FpointService;

        public FpointController(IFpointService fpointService)
        {
            _FpointService = fpointService;
        }
     
        [HttpGet]
        public async Task<IEnumerable<Fpoint>> GetPointAsync()
        {
           
           

                var model = await _FpointService.GetListFpoint();
                return model;
           
       
        }
        /// <summary>
        /// tích điểm poin 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="diemtichluy"></param>
        /// <param name="makh"></param>
        /// <returns></returns>
        [HttpPost("{id}/{diemtichluy}/{makh}")]
        public async Task<ActionResult> AddFpointAsync(string id , string diemtichluy , string makh)
        {
            try
            {
                await _FpointService.AddFoint(id,diemtichluy,makh);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
