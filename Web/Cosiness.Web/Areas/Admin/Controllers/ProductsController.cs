namespace Cosiness.Web.Areas.Admin.Controllers
{
    using Cosiness.Services.Data;
    using Cosiness.Web.InputModels.Products;
    using Cosiness.Web.ViewModels.Admin.Products;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class ProductsController : AdminController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            this._productService = productService;
        }

        public async Task<IActionResult> All()
        {
            var products = await _productService.GetAllAsync<ProductAllViewModel>();

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductInputModel inputModel)
        {
            await _productService.CreateAsync(inputModel);
            return RedirectToAction(nameof(All));
        }
    }
}