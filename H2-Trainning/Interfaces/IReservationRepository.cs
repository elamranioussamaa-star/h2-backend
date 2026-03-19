using H2_Trainning.Models;

namespace H2_Trainning.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetByClientIdAsync(string clientId);
        Task<List<Reservation>> GetByCoachIdAsync(string coachId);
        Task<Reservation?> GetByIdAsync(int id);
        Task<Reservation> CreateAsync(Reservation reservation);
        Task<Reservation> UpdateAsync(Reservation reservation);
    }
}
