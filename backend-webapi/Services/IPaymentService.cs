using webapi.Dtos;

namespace webapi.Services
{
    public interface IPaymentService
    {
        double CalculateOrderPrice(int customerId, int orderId);
        PaymentDto CreateNewPayment(int orderId, double price);
        bool UpdatePayment(int id, int status);
    }
}  