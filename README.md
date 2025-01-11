# Comdirect

Zufällig bin ich darüber gestolpert, dass die Comdirect eine Kunden API zur Verfügung stellt. 
Das wollte ich mir genauer anschauen. Wenn man die Hürde des 5-Phasen-Logins überwunden hat, kann
man damit ganz viele Sachen abfragen.  
  
Ich habe mal einige der Endpunkte in die API übernommen und auch gleich eine kleine Demo-Anwendung mit dazu gepackt.

## Beschreibung
Das Projekt kapselt die API der Comdirect in .NET und stellt sie in einer einfachen Form zur Verfügung.
Bisher habe ich mich auf die Abfrage von Konten, Depots und die Postbox beschränkt.  
  
Die beigefügte Demo-Anwendung liefert ein Praxisnahes Beispiel, wie man die abgefragten Daten visualisieren kann.

<span style="color:red">
Achtung: Sollte man die TAN Eingabe mehrfach hintereinander abbrechen, kann der Zugang zum Konto gesperrt werden!
</span>

## Funktionen & Highlights
- Login über die Comdirect API
- Abfrage von Konten, Depots und Postbox
- Report über alle Konten, Depots & Karten anzeigen
- Konten: Transaktionen auflisten
- Depot: Transaktionen auflisten
- Depot: Positionen auflisten
- Postbox: Nachrichten auflisten
- Postbox: **Alle** Nachrichten auf einmal herunterladen

## Installation

Für die Verwendung der API muss man sich auf der Comdirect Seite freischalten lassen. 
Den entsprechenden Menüpunkt findet man, wenn man angemeldet ist, unter Verwaltung - Entwicklerzugang.  
  
Man bekommt dann eine ClientID und ein ClientSecret. Diese muss man in den 'UserSecrets' hinterlegen.  
Rechtsklick auf das Projekt 'Comdirect' -> Manage User Secrets (bzw. Geheime Benutzerschlüssel verwalten).  
```json
{
  "UserSettings": {
	"ClientID": "DIE_ERHALTENE_CLIENT_ID_",
	"ClientSecret": "DAS_ERHALTENE_CLIENT_SECRET_"
   }
}
```

In der appsettings.json kann man zusätzlich noch die Zugangsnummer und ein 
Standard-Verzeichnisses für Postbox-Downloads hinterlegen.
```json
{
  "UserSettings": {
	"Username": "IHRE_COMDIRECT_ZUGANGSNUMMER",
	"DefaultDownloadDirectory": "Verzeichnis für Postbox-Downloads"
   }
}
```

**Laufzeitumgebung:** .NET Core 9

## Einschränkungen

Folgende Einschränkungen sind zu beachten

### Auf Euro ausgelegt

Die Demo Applikation geht im Moment davon aus, dass die zurückgelieferten Werte in Euro sind. (Was sie im Regelfall auch sind)

## Geplante Änderungen

### API
- Doppelte Elemente in den Response Klassen zusammenfassen
- InstrumentViewModel Mapper hinzufügen (doppelten Code vermeiden)
- API-Requests Refactoring: Doppelte Code Blöcke zusammenfassen
- Exception Handling hinzufügen
- Kompiler-Warnungen beheben

### UI
- ToolTips im ListView für alle Spalten ermöglichen (https://stackoverflow.com/questions/2648281/listview-tooltip-only-in-first-cell-vb-net)
- Einstellungsdialog
- Postbox: Durchblättern aller Nachrichten
- Postbox: Filtermöglichkeiten
- Transaktionen & Positionen: Durchblättern & Filtern
- ListViews: Sortierungen hinzufügen
- Projekt mit Icons aufhübschen

## Changelog

Eine Übersicht über Änderungen und Updates im Projekt.

- 1.0.0
  - Erstveröffentlichung auf GitHub