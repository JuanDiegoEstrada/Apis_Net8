using WebApplicationAPI_2.Modelos;
using System.Threading.Tasks;

namespace WebApplicationAPI_2.Repos
{
    public interface IProductServer
    {
        Task <IEnumerable<Producto>> GetProductosAsync();
        Task <Producto> GetProductoAsync(string sku);
        Task AddProductAsync(Producto p);
        Task PutProductAsync(Producto p);
        Task deleteProductAsync(Producto p);
    }
}
