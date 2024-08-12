using WebApplicationAPI_2.DTO;
using WebApplicationAPI_2.Modelos;

namespace WebApplicationAPI_2
{
    public static class Utilities
    {
        public static ProductDTO convertTransform(this Producto p)
        {
            return new ProductDTO
            {
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                sku = p.sku
            };
        }

        public static UserDTO convertTransform(this User u)
        {
            if (u != null)
            {
                return new UserDTO
                {
                    userToken = u.tokenUser,
                    userName = u.nameUser
                };
            }
            return null;
        }
    }
}
