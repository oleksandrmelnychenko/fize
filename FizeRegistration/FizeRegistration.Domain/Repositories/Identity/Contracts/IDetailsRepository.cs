using FizeRegistration.Domain.Entities.Identity;

namespace FizeRegistration.Domain.Repositories.Identity.Contracts;

public interface IDetailsRepository
{
    long NewDetails(Details details);
}