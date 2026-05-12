namespace SmartPark.Models;

public enum StatusRezervacije
{
    Ustvarjena,
    Potrjena,
    Preklicana,
    Zakljucena
}

public class Rezervacija
{
    public int Id { get; set; }

    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? Uporabnik { get; set; }

    public int ParkirisceId { get; set; }
    public Parkirisce? Parkirisce { get; set; }

    public int? ParkirnoMestoId { get; set; }
    public ParkirnoMesto? ParkirnoMesto { get; set; }

    public DateTime Zacetek { get; set; }
    public DateTime Konec { get; set; }
    public StatusRezervacije Status { get; set; }

    // Sledenje – analogija z Owner/DateCreated/DateEdited iz navodil
    public string? OwnerId { get; set; }
    public ApplicationUser? Owner { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateEdited { get; set; }
}
