using webapi.Dtos;
using webapi.ViewModels;

namespace webapi.Services
{
    public interface IUserService
    {
        string Authentication(int id, string token);
        object SignIn(string email, string password, string role);
        CustomerDto SignUp(CustomerVM customerVM);
        DelivererDto SignUp(DelivererVM delivererVM);
        SellerDto SignUp(SellerVM sellerVM);
    }
}