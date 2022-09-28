using Microsoft.AspNetCore.Mvc;
using WebNewBook.Services;

namespace WebNewBook.ViewComponents
{
	public class FooterListViewComponent: ViewComponent
    {
        private IHeaderService _headerService;
        public FooterListViewComponent(IHeaderService headerService)
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
