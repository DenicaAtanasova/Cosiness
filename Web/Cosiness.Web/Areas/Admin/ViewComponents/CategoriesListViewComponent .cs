namespace Cosiness.Web.Areas.Admin.ViewComponents
{
    using Cosiness.Models;
    using Cosiness.Services.Data;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class CategoriesListViewComponent : ViewComponent
    {
        private readonly IBaseNameOnlyEntityService<Category> _serice;

        public CategoriesListViewComponent(IBaseNameOnlyEntityService<Category> serice)
        {
            _serice = serice;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        => View(await _serice.GetAllAsync());
    }
}