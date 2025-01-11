# Comdirect

Zuf�llig bin ich dar�ber gestolpert, dass die Comdirect eine Kunden API zur Verf�gung stellt. 
Das wollte ich mir genauer anschauen. Wenn man die H�rde des 5-Phasen-Logins �berwunden hat, kann
man damit ganz viele Sachen abfragen.  
  
Ich habe mal einige der Endpunkte in die API �bernommen und auch gleich eine kleine Demo-Anwendung mit dazu gepackt.

## Beschreibung
Das Projekt kapselt die API der Comdirect in .NET und stellt sie in einer einfachen Form zur Verf�gung.
Bisher habe ich mich auf die Abfrage von Konten, Depots und die Postbox beschr�nkt.  
  
Die beigef�gte Demo-Anwendung liefert ein Praxisnahes Beispiel, wie man die abgefragten Daten visualisieren kann.

<span style="color:red">
Achtung: Sollte man die TAN Eingabe mehrfach hintereinander abbrechen, kann der Zugang zum Konto gesperrt werden!
</span>

## Funktionen & Highlights
- Login �ber die Comdirect API
- Abfrage von Konten, Depots und Postbox
- Report �ber alle Konten, Depots & Karten anzeigen
- Konten: Transaktionen auflisten
- Depot: Transaktionen auflisten
- Depot: Positionen auflisten
- Postbox: Nachrichten auflisten
- Postbox: **Alle** Nachrichten auf einmal herunterladen

## Installation

F�r die Verwendung der API muss man sich auf der Comdirect Seite freischalten lassen. 
Den entsprechenden Men�punkt findet man, wenn man angemeldet ist, unter Verwaltung - Entwicklerzugang.  
  
Man bekommt dann eine ClientID und ein ClientSecret. Diese muss man in den 'UserSecrets' hinterlegen.  
Rechtsklick auf das Projekt 'Comdirect' -> Manage User Secrets (bzw. Geheime Benutzerschl�ssel verwalten).  
```json
{
  "UserSettings": {
	"ClientID": "DIE_ERHALTENE_CLIENT_ID_",
	"ClientSecret": "DAS_ERHALTENE_CLIENT_SECRET_"
   }
}
```

In der appsettings.json kann man zus�tzlich noch die Zugangsnummer und ein 
Standard-Verzeichnisses f�r Postbox-Downloads hinterlegen.
```json
{
  "UserSettings": {
	"Username": "IHRE_COMDIRECT_ZUGANGSNUMMER",
	"DefaultDownloadDirectory": "Verzeichnis f�r Postbox-Downloads"
   }
}
```

**Laufzeitumgebung:** .NET Core 9

## Einschr�nkungen

Folgende Einschr�nkungen sind zu beachten

### Auf Euro ausgelegt

Die Demo Applikation geht im Moment davon aus, dass die zur�ckgelieferten Werte in Euro sind. (Was sie im Regelfall auch sind)

## Geplante �nderungen

### API
- Doppelte Elemente in den Response Klassen zusammenfassen
- InstrumentViewModel Mapper hinzuf�gen (doppelten Code vermeiden)
- API-Requests Refactoring: Doppelte Code Bl�cke zusammenfassen
- Exception Handling hinzuf�gen
- Kompiler-Warnungen beheben

### UI
- ToolTips im ListView f�r alle Spalten erm�glichen (https://stackoverflow.com/questions/2648281/listview-tooltip-only-in-first-cell-vb-net)
- Einstellungsdialog
- Postbox: Durchbl�ttern aller Nachrichten
- Postbox: Filterm�glichkeiten
- Transaktionen & Positionen: Durchbl�ttern & Filtern
- ListViews: Sortierungen hinzuf�gen
- Projekt mit Icons aufh�bschen

## Changelog

Eine �bersicht �ber �nderungen und Updates im Projekt.

- 1.0.0
  - Erstver�ffentlichung auf GitHub