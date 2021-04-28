namespace Cosiness.Web.Areas.Admin.ViewComponents
{
    using Cosiness.Models;
    using Cosiness.Services.Data;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class MaterialsListViewComponent : ViewComponent
    {
        private readonly IBaseNameOnlyEntityService<Material> _serice;

        public MaterialsListViewComponent(IBaseNameOnlyEntityService<Material> serice)
        {
            _serice = serice;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        => View(await _serice.GetAllAsync());
    }
}