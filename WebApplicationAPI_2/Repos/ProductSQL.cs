using Microsoft.Data.SqlClient;
using System.Data;
using WebApplicationAPI_2.Connections;
using WebApplicationAPI_2.Modelos;
using System.Threading.Tasks;

namespace WebApplicationAPI_2.Repos
{
    public class ProductSQL : IProductServer
    {
        private string CadConexion;
        private readonly ILogger<ProductSQL> log;
        public ProductSQL(AccessSQL cadConexion, ILogger<ProductSQL> log)
        {
            CadConexion = cadConexion.CadConnection;
            this.log = log;
        }
        private SqlConnection conexion()
        {
            return new SqlConnection(CadConexion);
        }
        public async Task AddProductAsync(Producto p)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand comm = null;

            try
            {
                sqlConexion.Open();
                comm = sqlConexion.CreateCommand();
                comm.CommandText = "dbo.AddProduct";
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@Name", SqlDbType.VarChar).Value = p.Name;
                comm.Parameters.Add("@Description", SqlDbType.VarChar).Value = p.Description;
                comm.Parameters.Add("@Price", SqlDbType.Float).Value = p.Price;
                comm.Parameters.Add("@sku", SqlDbType.VarChar).Value = p.sku;
                await comm.ExecuteNonQueryAsync();
            } catch(Exception ex)
            {
                throw new Exception("Error to loader new Prodcut to DB" + ex.ToString());
            }
            finally
            {
                comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            await Task.CompletedTask;
        }

        public async Task deleteProductAsync(Producto p)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand comm = null;

            try
            {
                sqlConexion.Open();
                comm = sqlConexion.CreateCommand();
                comm.CommandText = "dbo.DeleteProduct";
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@sku", SqlDbType.VarChar).Value = p.sku;
                await comm.ExecuteNonQueryAsync();
            } catch(Exception ex)
            {
                throw new Exception("Error to delete Product from DB" + ex.ToString());
            }
            finally
            {
                comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            await Task.CompletedTask;
        }

        public async Task<Producto> GetProductoAsync(string sku)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand comm = null;
            Producto p = null;

            try
            {
                sqlConexion.Open();
                comm = sqlConexion.CreateCommand();
                comm.CommandText = "dbo.GetProduc";
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@sku", SqlDbType.VarChar).Value = sku;
                comm.ExecuteNonQuery();
                SqlDataReader reader = await comm.ExecuteReaderAsync();

                if (reader.Read())
                {
                    p = new Producto
                    {
                        Name = reader["nameProduct"].ToString(),
                        Description = reader["descriptionProduct"].ToString(),
                        Price = Convert.ToDouble(reader["price"].ToString()),
                        sku = reader["SKU"].ToString() 
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error to get by Product from DB" + ex.ToString());
            }
            finally
            {
                comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return p;
        }

        public async Task<IEnumerable<Producto>> GetProductosAsync()
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand comm = null;
            List<Producto> productos = new List<Producto>();
            Producto p = null;

            try
            {
                sqlConexion.Open();
                comm = sqlConexion.CreateCommand();
                comm.CommandText = "dbo.GetProduct";
                comm.CommandType = CommandType.StoredProcedure;
                comm.ExecuteNonQuery();
                SqlDataReader reader = await comm.ExecuteReaderAsync();

                while (reader.Read())
                {
                    p = new Producto
                    {
                        Name = reader["nameProduct"].ToString(),
                        Description = reader["descriptionProduct"].ToString(),
                        Price = Convert.ToDouble(reader["price"].ToString()),
                        sku = reader["SKU"].ToString()
                    };

                    productos.Add(p);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw new Exception("Error to get all Product from DB" + ex.ToString());
            }
            finally
            {
                comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return productos;
        }

        public async Task PutProductAsync(Producto p)
        {
            SqlConnection sqlConexion = conexion();
            SqlCommand comm = null;

            try
            {
                sqlConexion.Open();
                comm = sqlConexion.CreateCommand();
                comm.CommandText = "dbo.UpdateProduc";
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@Name", SqlDbType.VarChar).Value = p.Name;
                comm.Parameters.Add("@Description", SqlDbType.VarChar).Value = p.Description;
                comm.Parameters.Add("@Price", SqlDbType.Float).Value = p.Price;
                comm.Parameters.Add("@sku", SqlDbType.VarChar).Value = p.sku;
                await comm.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error to loader new Prodcut to DB" + ex.ToString());
            }
            finally
            {
                comm.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            await Task.CompletedTask;
        }
    }
}
