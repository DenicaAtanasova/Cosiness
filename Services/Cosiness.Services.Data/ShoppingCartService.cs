namespace Cosiness.Services.Data
{
    using Cosiness.Data;
    using Cosiness.Services.Data.Helpers;

    using System.Linq;
    using System.Threading.Tasks;

    public class ShoppingCartService : IShoppingCartService, IValidator
    {
        private readonly CosinessDbContext _context;

        public ShoppingCartService(CosinessDbContext context)
        {
            _context = context;
        }

        public async Task ClearAsync(string id)
        {
            this.ThrowIfIncorrectId(_context.ShoppingCarts, id);

            var shoppingCartProducts = _context.ShoppingCartsProducts
                .Where(x => x.ShoppingCartId == id);

            _context.ShoppingCartsProducts.RemoveRange(shoppingCartProducts);

            await _context.SaveChangesAsync();
        }
    }
}