using System.Threading.Tasks;
using FizeRegistration.Shared.PaymentContracts;

namespace FizeRegistration.Services.PaymentServices.Contracts;

public interface ILiqPayService
{
    Task GetStatus(PaymentStatusContract paymentStatusContract);
}