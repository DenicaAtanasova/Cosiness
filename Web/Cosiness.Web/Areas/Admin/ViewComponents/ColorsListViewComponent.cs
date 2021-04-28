namespace Cosiness.Web.Areas.Admin.ViewComponents
{
    using Cosiness.Models;
    using Cosiness.Services.Data;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class ColorsListViewComponent : ViewComponent
    {
        private readonly IBaseNameOnlyEntityService<Color> _serice;

        public ColorsListViewComponent(IBaseNameOnlyEntityService<Color> serice)
        {
            _serice = serice;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        => View(await _serice.GetAllAsync());
    }
}