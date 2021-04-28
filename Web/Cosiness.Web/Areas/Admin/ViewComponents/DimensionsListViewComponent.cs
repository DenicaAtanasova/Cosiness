namespace Cosiness.Web.Areas.Admin.ViewComponents
{
    using Cosiness.Models;
    using Cosiness.Services.Data;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class DimensionsListViewComponent : ViewComponent
    {
        private readonly IBaseNameOnlyEntityService<Dimension> _serice;

        public DimensionsListViewComponent(IBaseNameOnlyEntityService<Dimension> serice)
        {
            _serice = serice;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        => View(await _serice.GetAllAsync());
    }
}