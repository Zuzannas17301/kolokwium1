using Kolokwium1.DTOs.Responses;

namespace Kolokwium1.Services
{
    public interface IDbService
    {
        GetMedicamentResponse GetMedicament(int id);
    }
}