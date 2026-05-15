# SmartPark

SmartPark je demo spletna aplikacija za odkrivanje parkirnih lokacij, ustvarjanje rezervacij in simulacijo plačil.  
Projekt uporablja OpenStreetMap (Overpass API) za uvoz parkirnih lokacij za Ljubljano in ponuja pregledno uporabniški vmesnik z uporabniškimi računi in administrativnim panelom.

> To je študentski/demo projekt. Plačila so simulirana in niso povezana z dejanskim plačilnim ponudnikom.

---

## Funkcionalnosti

### Uporabniške funkcionalnosti
- Interaktivni zemljevid (Leaflet) z označenimi parkirišči
- Podrobnosti parkirišča: ime, cena na uro, delovni čas, prosta mesta (demo)
- Ustvarjanje rezervacije (izbira začetnega in končnega časa)
- Plačilni proces (demo):
  - Plačilo s kreditno kartico (osnovna validacija na strani odjemalca)
  - Plačilo z naročilnico
- Stran »Moje rezervacije« s pregledom statusov
- Preklic rezervacije (v demo načinu sprosti parkirno mesto)

### Administratorske funkcionalnosti
- Administrativni panel (zaščiten z vlogo)
- Pregled **vseh** rezervacij v bazi (ne samo lastnih)
- (Opcijsko/če omogočeno) stran za pregled plačil

---

## Tehnologije

- **ASP.NET Core MVC** (C#)
- **Entity Framework Core** + **SQL Server / Azure SQL**
- **ASP.NET Core Identity** (avtentikacija in upravljanje uporabnikov)
- **Leaflet.js** (zemljevidni vmesnik)
- **Overpass API** (uvoz OpenStreetMap podatkov)

---

## Struktura projekta (pregledno)

- `Controllers/` – MVC kontrolerji (rezervacije, plačila itd.)
- `Controllers/Admin/` – administrativni kontrolerji
- `Models/` – EF Core modeli (Parkirišče, ParkirnoMesto, Rezervacija, Plačilo, …)
- `Data/` – DbContext + začetni seed/import koda (Overpass API helper)
- `Views/` – MVC pogledi
- `Areas/Identity/` – Identity UI strani (scaffoldane/prilagojene, če omogočeno)

---

## Namestitev in zagon (lokalno)

### Predpogoji
- .NET SDK (priporočeno: zadnja LTS različica)
- SQL Server (LocalDB / SQL Server / Docker / Azure SQL)

### 1) Kloniranje
```bash
git clone https://github.com/n1hadd/SmartPark.git
cd SmartPark
