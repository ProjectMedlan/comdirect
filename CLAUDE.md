# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Projektüberblick

.NET-10-Lösung, die die inoffizielle/undokumentierte Comdirect-Kunden-API kapselt und über eine WinForms-Demo-Anwendung visualisiert. Zwei Projekte:

- **Comdirect.API** (`Comdirect.API/`) – Klassenbibliothek, kapselt den gesamten HTTP-Verkehr mit `https://api.comdirect.de`. Kein UI-Bezug, keine Business-Logik der Anwendung.
- **Comdirect** (`Comdirect/`) – WinForms-App (`net10.0-windows7.0`), referenziert `Comdirect.API` und stellt die Demo-UI bereit.

## Build & Run

```
dotnet build Comdirect.slnx
dotnet run --project Comdirect/Comdirect.csproj
```

Es existieren keine automatisierten Tests in diesem Repository (kein Testprojekt vorhanden).

### Zugangsdaten (erforderlich zum Ausführen)

Die App benötigt bei der Comdirect freigeschaltete API-Zugangsdaten (Verwaltung → Entwicklerzugang), hinterlegt über User Secrets im Projekt `Comdirect`:

```json
{
  "UserSettings": {
    "ClientID": "...",
    "ClientSecret": "..."
  }
}
```

Optional zusätzlich in `Comdirect/appsettings.json` (`UserSettings.Username`, `UserSettings.DefaultDownloadDirectory`). Die Konfiguration wird in `Program.cs` per `ConfigurationBuilder` (appsettings.json + UserSecrets) geladen und über die statische `Program.Configuration` bereitgestellt.

$${\color{red}Achtung}$$: Wiederholtes Abbrechen der TAN-Eingabe kann den Comdirect-Kontozugang sperren – beim Testen des Login-Flows vorsichtig vorgehen.

## Architektur

### Comdirect.API – Login-Flow als Zustandsautomat

`ComdirectAPI` (`Comdirect.API/ComdirectAPI.cs`) ist eine zustandsbehaftete Wrapper-Klasse pro Session (eine Instanz = eine eingeloggte Sitzung). Die Methoden müssen in fester Reihenfolge aufgerufen werden, da jede auf dem Ergebnis der vorherigen aufbaut (Felder wie `_loginResponse`, `_serverSession`, `_tanChallengeResponse`, `_cdSecondaryResponse` werden sequenziell befüllt und von späteren Aufrufen geprüft):

1. `GetAccessToken()` – OAuth Password-Grant → `_loginResponse`
2. `GetSessionStatus()` – liefert Session-Identifier → `_serverSession`
3. `StartTanChallenge()` / `StartTanChallenge(forceChallengeType)` – triggert 2FA, liefert TAN-Challenge (`GetTanChallengeType()`, `GetTanChallenge()` zum Auslesen)
4. `ActivateSession(tan)` – bestätigt die Session mit der eingegebenen/gepushten TAN
5. `GetCDSecondaryFlowToken()` – tauscht gegen den eigentlichen API-Access-Token (`_cdSecondaryResponse`), löst `OnSessionTimeoutChanged` aus

Danach stehen die eigentlichen Datenabfragen zur Verfügung (Accounts, Depots, Postbox/Documents, Reports), die alle `_cdSecondaryResponse.access_token` voraussetzen. Session-Refresh/-Revoke über `RefreshSession()` / `RevokeSession()`.

Der komplette Fluss orientiert sich an der offiziellen Comdirect-API-Dokumentation; die Region-Kommentare in `ComdirectAPI.cs` (`2.Login`, `3. Token Flow`, `4. Accounts`, `5. Depot`, `9. Postbox`, `10. Reports`) spiegeln deren Nummerierung – bei Erweiterung um neue Endpunkte diese Nummerierung beibehalten. Undokumentierte Query-Parameter (`min-bookingDate`/`max-bookingDate`, `paging-first`/`paging-count`) sind oben in der Datei kommentiert.

HTTP-Requests laufen über einen gemeinsamen `HttpClient` mit `Microsoft.Extensions.Http.Resilience`-Retry-Pipeline (exponentielles Backoff, 3 Versuche); alle Requests fügen den `x-http-request-info`-Header über `ClientRequestId` (`DataModels/ClientSession.cs`) hinzu. Antworten werden 1:1 in `DataModels/*.cs` deserialisiert (rohe API-Response-Modelle, kein Mapping in der API-Schicht).

### Comdirect (WinForms) – Schichten

- **DataModels → ViewModels**: Die rohen API-Response-Objekte (`Comdirect.API.DataModels`) werden nie direkt ans UI gereicht. Für jeden Bereich existiert ein `*ViewModelConverter` (`Comdirect/ViewModelConverter/`), das API-Modelle in die zugehörigen `Comdirect/ViewModels/*` umwandelt (Formatierung, Nachschlagen von Klartext-Bezeichnungen über `Comdirect.API.Constants`, z. B. Kontotyp- oder Transaktionstyp-Mappings).
- **FrmMain.cs**: Einzige Hauptform, nach Regions gegliedert (`Login`, `Load Report`, `Account Transactions / Depot Transactions / Depot Positions`, `Postbox`, `ListView ToolTip Fix`, `Logging & Status`, `Session Handling`). Hält die `ComdirectAPI`-Instanz und orchestriert Login-Flow, TAN-Dialog (`FrmTanConfirmation`), Session-Timeout-Timer sowie das Befüllen der ListViews.
- **BLL/PostboxNavigator.cs**: Zustandsklasse für Client-seitiges Paging/Filtern der bereits geladenen Postbox-Dokumente (Cache aus `DocumentViewModel`s, Filter „nur ungelesene“, Seitenwechsel-Events). Lädt selbst keine Daten nach, sondern arbeitet ausschließlich auf `DocumentCache`.
- **Enumerations.cs**: UI-interne Enums (z. B. Log-Typen).

### Bekannte Einschränkungen (siehe README.md)

- Demo-App geht von EUR-Beträgen aus.
- Mögliches Bug bei der Seitenberechnung, wenn viele ungelesene Postbox-Nachrichten heruntergeladen werden.
- Geplant, aber noch nicht umgesetzt: `GetDepotPosition(depotId, positionId)` in `ComdirectAPI.cs` ist ein Stub (wirft `NotImplementedException`).
