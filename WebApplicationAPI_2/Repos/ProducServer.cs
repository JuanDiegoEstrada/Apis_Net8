using WebApplicationAPI_2.Modelos;
using System.Threading.Tasks;

namespace WebApplicationAPI_2.Repos
{
    public class ProducServer: IProductServer
    {
        private readonly List<Producto> productos = new()
        {
            new Producto {Id = 1, Name = "Carrito", Description = "Deportivo", DateAlta = new DateTime(), Price = 100, sku = "car1"},
            new Producto {Id = 2, Name = "Camioneta", Description = "Ford Raptor", DateAlta = new DateTime(), Price = 100, sku = "car2"},
            new Producto {Id = 3, Name = "Motocicleta", Description = "Yamaha r6", DateAlta = new DateTime(), Price = 100, sku = "car3"},
            new Producto {Id = 4, Name = "Trailer", Description = "De Carga", DateAlta = new DateTime(), Price = 100, sku = "car4"}

        };

        public async Task<IEnumerable<Producto>> GetProductosAsync() 
        {
            return await Task.FromResult(productos);
        }

        public async Task<Producto> GetProductoAsync(string sku) 
        {
            return await Task.FromResult(productos.Where(p => p.sku == sku).SingleOrDefault());
        }

        public async Task AddProductAsync(Producto p)
        {
            productos.Add(p);
            await Task.CompletedTask;
        }

        public async Task PutProductAsync(Producto p)
        {
            int id = productos.FindIndex(existeProduct => existeProduct.Id == p.Id);
            productos[id] = p;
            //productos.Insert(id, p);
            await Task.CompletedTask;
        }

        public async Task deleteProductAsync(Producto p)
        {
            int id = productos.FindIndex(existProduct => existProduct.Id == p.Id);
            productos.RemoveAt(id);
            await Task.CompletedTask;
        }
    }
}
