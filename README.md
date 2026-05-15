# SmartPark

SmartPark je demo spletna aplikacija za iskanje parkirišč, ustvarjanje rezervacij in simulacijo plačil.  
Projekt uporablja OpenStreetMap (Overpass API) za uvoz parkirišč za Ljubljano ter ponuja prijavo uporabnikov in administratorski del.

> Gre za študentski/demo projekt. Plačilo je simulirano in ni povezano z dejanskim plačilnim ponudnikom.

---

## Funkcionalnosti

### Za uporabnike
- Interaktivni zemljevid (Leaflet) z označevalci parkirišč
- Podrobnosti parkirišča: naziv, cena na uro, delovni čas, prosta mesta (demo)
- Rezervacija parkirišča (izbira začetka/konca)
- Plačilni tok (demo):
  - plačilo z bančno kartico (osnovna validacija na odjemalcu)
  - plačilni nalog
- Stran **Moje rezervacije** s pregledom rezervacij
- Preklic rezervacije (v demo načinu sprosti parkirno mesto)

### Za administratorje
- Administratorski del (zaščiten z vlogo)
- Pregled **vseh** rezervacij v bazi (ne samo adminsko ustvarjenih)
- (Neobvezno / po potrebi) stran za pregled plačil

---

## Tehnologije

- **ASP.NET Core MVC** (C#)
- **Entity Framework Core** + **SQL Server / Azure SQL**
- **ASP.NET Core Identity** (avtentikacija, upravljanje uporabnikov)
- **Leaflet.js** (zemljevid)
- **Overpass API** (uvoz podatkov iz OpenStreetMap)

---

## Struktura projekta (na visoki ravni)

- `Controllers/` – MVC kontrolerji (rezervacije, plačila, ipd.)
- `Controllers/Admin/` – kontrolerji administratorskega dela
- `Models/` – EF Core modeli (Parkirisce, ParkirnoMesto, Rezervacija, Placilo, ...)
- `Data/` – DbContext + seed/uvoz (OverpassApiHelper)
- `Views/` – Razor pogledi
- `Areas/Identity/` – Identity strani (scaffoldane/prilagojene, če so vključene)

---

## Zagon projekta (lokalno)

### Predpogoji
- .NET SDK (priporočeno: zadnja LTS različica)
- SQL Server (LocalDB / SQL Server / Docker / Azure SQL)

### 1) Kloniranje repozitorija
```bash
git clone https://github.com/n1hadd/SmartPark.git
cd SmartPark
```

### 2) Nastavitev povezave na bazo
V `appsettings.json` (ali `appsettings.Development.json`) nastavi connection string, npr.:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=smartpark-db;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 3) Uporabi EF migracije
```bash
dotnet ef database update
```

Če `dotnet ef` ni nameščen:
```bash
dotnet tool install --global dotnet-ef
```

### 4) Zagon aplikacije
```bash
dotnet run
```

Odpri URL, ki ga izpiše aplikacija (npr.):
- `http://localhost:5143`

---

## Uvoz parkirišč (Overpass / OSM)

SmartPark uvaža parkirišča za Ljubljano preko Overpass API (OSM).  
OSM pogosto nima zanesljivega podatka o številu parkirnih mest (`capacity`), zato aplikacija lahko:
- uporabi `capacity`, kadar je na voljo,
- sicer nastavi **demo privzeto** število mest in generira demo `ParkirnoMesto` zapise.

To omogoča delovanje aplikacije tudi, ko zunanji podatki niso popolni.

---

## Logika “prostih mest” (pomembno)

Običajno sta 2 pristopa:

1. **Časovna zasedenost (pravilneje za realno uporabo)**  
   Prosto/zasedeno se izračuna iz rezervacij, ki se prekrivajo z izbranim intervalom.

2. **Enostaven bool (prijazno za demo)**  
   `ParkirnoMesto.Zasedeno` se ob rezervaciji nastavi na `true`, ob preklicu pa na `false`.

SmartPark uporablja demo pristop in/ali preverjanje prekrivanja rezervacij glede na konfiguracijo.

---

## Administratorski del

Administratorske strani so namenjene uporabnikom z vlogo `Administrator`.

Primeri poti:
- `/admin/rezervacije` – seznam vseh rezervacij

Če dodaš stran s plačili:
- `/admin/placila`

---

## Identity UI / prevod v slovenščino

Če želiš, da so tudi strani za upravljanje profila (sprememba e‑pošte, gesla itd.) v slovenščini:
- scaffoldaj Identity strani v `Areas/Identity`,
- prevedi naslove, oznake polj in gumbe,
- prevedi validacijska sporočila (DataAnnotations ali `.resx`).

---

## Odpravljanje težav

### “Invalid column name …” pri shranjevanju (EF Core)
To pomeni, da je shema baze starejša od tvojih modelov.
Zaženi:
```bash
dotnet ef migrations add <ImeMigracije>
dotnet ef database update
```

### Timeout pri Overpass API
Overpass je lahko počasen; projekt uporablja stabilnejši endpoint:
- `https://overpass.kumi.systems/api/interpreter`

Če še vedno timeouta, poskusi kasneje ali zmanjšaj obseg poizvedbe.

### “Ni prostih mest” / rezervacija ne uspe
Tipični vzroki:
- v bazi ni ustvarjenih `ParkirnoMesto` zapisov za parkirišče,
- neveljaven interval (konec <= začetek),
- logika zasedenosti zavrne prekrivajoče rezervacije.

---

## Ideje / Nadaljnji razvoj

- Prava lokalizacija (`sl-SI`) z `.resx` viri
- Filtri/iskanje (lokacija, cena, prosta mesta)
- Bolj realen model zasedenosti po časovnih intervalih
- Evidenca statusa plačil + administrativna poročila
- Admin CRUD za parkirišča in cene

---

## Licenca

Projekt je narejen za izobraževalne/demo namene.
