using WebApplicationAPI_2.Modelos;

namespace WebApplicationAPI_2.Repos
{
    public interface IUserSQL
    {
        Task<User> LoginUser(UserSession login);
    }
}
