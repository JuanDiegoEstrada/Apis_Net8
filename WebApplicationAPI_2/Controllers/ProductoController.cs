using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI_2.DTO;
using WebApplicationAPI_2.Modelos;
using WebApplicationAPI_2.Repos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApplicationAPI_2.Controllers
{
    [Route("api/productos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductoController : ControllerBase
    {
        private readonly IProductServer respositorio;
        public ProductoController(IProductServer r)
        {
            this.respositorio = r;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<ProductDTO>> GetProductos()
        {
            var listProduct = (await respositorio.GetProductosAsync()).Select(p=>p.convertTransform());
            return listProduct;
        }

        [HttpGet("{sku}")]
        [Authorize]
        public async Task<ActionResult<ProductDTO>> GetProducto(string sku)
        {
            var item = (await respositorio.GetProductoAsync(sku)).convertTransform();
            return item is null ? NotFound() : item;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductDTO>> AddProduct(ProductDTO p)
        {
            Producto producto = new Producto
            {
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                sku = p.sku,
            };
            await respositorio.AddProductAsync(producto);

            return producto.convertTransform();
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(string skuProduct, ProductUpdateDTO p)
        {
            Producto existProducto = await respositorio.GetProductoAsync(skuProduct);
            
            if(existProducto == null)
            {
                return NotFound();    
            }

            existProducto.Name = p.Name;
            existProducto.Description = p.Description;
            existProducto.Price = p.Price;
            existProducto.sku = p.sku;

            await respositorio.PutProductAsync(existProducto);

            return existProducto.convertTransform();
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteProduct(string skuProduct)
        {
            Producto existProduc = await respositorio.GetProductoAsync(skuProduct);
            
            if(existProduc is null) {
                return NotFound();
            }

            await respositorio.deleteProductAsync(existProduc);
            return NoContent();
        }
    }
}
