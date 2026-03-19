using H2_Trainning.Data;
using H2_Trainning.Dtos;
using H2_Trainning.Enums;
using H2_Trainning.Interfaces;
using H2_Trainning.Models;

namespace H2_Trainning.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repo;
        private readonly IAvailabilityRepository _slotRepo;
        private readonly ApplicationDbContext _context;

        public ReservationService(IReservationRepository repo, IAvailabilityRepository slotRepo, ApplicationDbContext context)
        {
            _repo = repo;
            _slotRepo = slotRepo;
            _context = context;
        }

        public async Task<List<ReservationDto>> GetByClientIdAsync(string clientId)
        {
            var reservations = await _repo.GetByClientIdAsync(clientId);
            return reservations.Select(MapToDto).ToList();
        }

        public async Task<List<ReservationDto>> GetByCoachIdAsync(string coachId)
        {
            var reservations = await _repo.GetByCoachIdAsync(coachId);
            return reservations.Select(MapToDto).ToList();
        }

        public async Task<ReservationDto> BookAsync(string clientId, CreateReservationDto dto)
        {
            var slot = await _slotRepo.GetByIdAsync(dto.SlotId);
            if (slot == null)
                throw new Exception("Slot not found.");
            if (slot.IsBooked)
                throw new Exception("This slot is already booked.");

            var reservation = new Reservation
            {
                SlotId = dto.SlotId,
                ClientId = clientId,
                CoachId = slot.CoachId,
                Status = ReservationStatus.Pending, // Pending until coach confirms
                CreatedAt = DateTime.UtcNow
            };

            // Mark slot as booked to prevent double-booking while pending
            slot.IsBooked = true;
            var created = await _repo.CreateAsync(reservation);
            await _context.SaveChangesAsync();

            // Re-fetch with includes
            var full = await _repo.GetByIdAsync(created.Id);
            return MapToDto(full!);
        }

        public async Task<ReservationDto?> ConfirmAsync(int id, string coachId)
        {
            var reservation = await _repo.GetByIdAsync(id);
            if (reservation == null) return null;
            if (reservation.CoachId != coachId) return null;
            if (reservation.Status != ReservationStatus.Pending) return null;

            reservation.Status = ReservationStatus.Confirmed;
            await _context.SaveChangesAsync();

            return MapToDto(reservation);
        }

        public async Task<ReservationDto?> RejectAsync(int id, string coachId)
        {
            var reservation = await _repo.GetByIdAsync(id);
            if (reservation == null) return null;
            if (reservation.CoachId != coachId) return null;
            if (reservation.Status != ReservationStatus.Pending) return null;

            reservation.Status = ReservationStatus.Rejected;

            // Free the slot so others can book
            var slot = await _slotRepo.GetByIdAsync(reservation.SlotId);
            if (slot != null)
            {
                slot.IsBooked = false;
            }

            await _context.SaveChangesAsync();
            return MapToDto(reservation);
        }

        public async Task<bool> CancelAsync(int id, string userId)
        {
            var reservation = await _repo.GetByIdAsync(id);
            if (reservation == null) return false;
            if (reservation.ClientId != userId && reservation.CoachId != userId) return false;

            reservation.Status = ReservationStatus.Cancelled;

            // Free the slot
            var slot = await _slotRepo.GetByIdAsync(reservation.SlotId);
            if (slot != null)
            {
                slot.IsBooked = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private static ReservationDto MapToDto(Reservation r) => new()
        {
            Id = r.Id,
            SlotId = r.SlotId,
            ClientId = r.ClientId,
            ClientName = r.Client?.FullName ?? "",
            CoachId = r.CoachId,
            CoachName = r.Coach?.FullName ?? "",
            Date = r.Slot?.Date.ToString("yyyy-MM-dd") ?? "",
            StartTime = r.Slot?.StartTime.ToString("HH:mm") ?? "",
            EndTime = r.Slot?.EndTime.ToString("HH:mm") ?? "",
            Status = r.Status.ToString(),
            CreatedAt = r.CreatedAt
        };
    }
}
