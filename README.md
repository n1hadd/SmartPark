# SmartPark

SmartPark je demo spletna aplikacija za iskanje parkirišč, ustvarjanje rezervacij in simulacijo plačil v Ljubljani.  
Uporablja OpenStreetMap (Overpass API) za uvoz parkirišč, Leaflet za prikaz zemljevida ter ASP.NET Core Identity za uporabnike in administratorski del.

> Plačila so simulirana (ni povezave z dejanskim plačilnim ponudnikom).

---

## Funkcionalnosti

### Uporabnik
- Zemljevid parkirišč (Leaflet) z označevalci
- Podrobnosti parkirišča (naslov, cena na uro, delovni čas, prosta mesta – demo)
- Rezervacija parkirnega mesta (izbira začetka/konca)
- Plačilo rezervacije (demo): kartica / plačilni nalog
- Stran **Moje rezervacije** + možnost preklica

### Administrator
- Administratorski pregled **vseh** rezervacij v bazi
- (Po potrebi) pregled plačil

---

## Tehnologije
- ASP.NET Core MVC (C#)
- Entity Framework Core + SQL Server / Azure SQL
- ASP.NET Core Identity
- Leaflet.js
- OpenStreetMap (Overpass API)

---

## Zagon (lokalno)

### Predpogoji
- .NET SDK (LTS)
- SQL Server (LocalDB/SQL Server/Azure SQL)

### 1) Kloniranje
```bash
git clone https://github.com/n1hadd/SmartPark.git
cd SmartPark
```

### 2) Connection string
V `appsettings.json` (ali `appsettings.Development.json`) nastavi povezavo na bazo, npr.:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=smartpark-db;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 3) Migracije
```bash
dotnet ef database update
```

Če `dotnet ef` ni nameščen:
```bash
dotnet tool install --global dotnet-ef
```

### 4) Zagon
```bash
dotnet run
```

---

## Administratorske poti (primer)
- `/admin/rezervacije` – seznam vseh rezervacij  
- `/admin/placila` – seznam plačil (če je implementirano)

---

## Opomba o podatkih (OSM)
OSM podatki pogosto nimajo zanesljivega podatka o kapaciteti (`capacity`), zato aplikacija po potrebi uporabi demo privzete vrednosti in generira parkirna mesta.

---

## Licenca
Projekt je narejen za izobraževalne/demo namene.
