namespace SmartPark.Models;

public class Placilo
{
    public int Id { get; set; }

    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? Uporabnik { get; set; }

    public int RezervacijaId { get; set; }
    public Rezervacija? Rezervacija { get; set; }

    public decimal Znesek { get; set; }
    public string NacinPlacila { get; set; } = string.Empty;
    public DateTime Datum { get; set; } = DateTime.UtcNow;

    public PlaciloStatus Status { get; set; } = PlaciloStatus.Pending;
    public string? TransactionId { get; set; }
    public string? FailureReason { get; set; }
}
