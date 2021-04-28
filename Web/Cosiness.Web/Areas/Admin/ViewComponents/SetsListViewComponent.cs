namespace Cosiness.Web.Areas.Admin.ViewComponents
{
    using Cosiness.Models;
    using Cosiness.Services.Data;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class SetsListViewComponent : ViewComponent
    {
        private readonly IBaseNameOnlyEntityService<Set> _serice;

        public SetsListViewComponent(IBaseNameOnlyEntityService<Set> serice)
        {
            _serice = serice;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        => View(await _serice.GetAllAsync());
    }
}