namespace SmartPark.Models;

public enum TipMesta
{
    Navadno,
    Invalidsko,
    ElektricnoVozilo
}

public class ParkirnoMesto
{
    public int Id { get; set; }

    public int ParkirisceId { get; set; }
    public int Stevilka { get; set; }
    public Parkirisce? Parkirisce { get; set; }

    public bool Zasedeno { get; set; }
    public TipMesta Tip { get; set; }
}
