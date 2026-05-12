namespace SmartPark.Models;

public class Parkirisce
{
    public int Id { get; set; }
    public string Naslov { get; set; } = string.Empty;
    public int SteviloMest { get; set; }
    public decimal CenaNaUro { get; set; }
    public string DelovniCas { get; set; } = string.Empty;

    public double Latitude { get; set; }   // dodano
    public double Longitude { get; set; }  // dodano

    public ICollection<ParkirnoMesto> ParkirnaMesta { get; set; } = new List<ParkirnoMesto>();
}