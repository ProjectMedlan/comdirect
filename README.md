# Comdirect

Zufällig bin ich darüber gestolpert, dass die Comdirect eine Kunden API zur Verfügung stellt. 
Das wollte ich mir genauer anschauen. Wenn man die Hürde des 5-Phasen-Logins überwunden hat, kann
man damit ganz viele Sachen abfragen.  
  
Ich habe mal einige der Endpunkte in die API übernommen und auch gleich eine kleine Demo-Anwendung mit dazu gepackt.

## Beschreibung
Das Projekt kapselt die API der Comdirect in .NET und stellt sie in einer einfachen Form zur Verfügung.
Bisher habe ich mich auf die Abfrage von Konten, Depots und die Postbox beschränkt.  
  
Die beigefügte Demo-Anwendung liefert ein Praxisnahes Beispiel, wie man die abgefragten Daten visualisieren kann.

$${\color{red}Achtung:}$$  Sollte man die TAN Eingabe mehrfach hintereinander abbrechen, kann der Zugang zum Konto gesperrt werden!

## Funktionen & Highlights
- Login über die Comdirect API
- Abfrage von Konten, Depots und Postbox
- Report über alle Konten, Depots & Karten anzeigen
- Konten: Transaktionen auflisten
- Depot: Transaktionen auflisten
- Depot: Positionen auflisten
- Postbox: Nachrichten auflisten & durchblättern
- Postbox: Angezeigte Nachrichten auf einmal herunterladen

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

### Bekannte Bugs

Ich glaube das es ein Problem sein könnte, wenn man viele ungelesene Nachrichten hat und diese dann herunterlädt, dass
es dann bei der Berechnung der Seitenanzahl zu Fehler kommen könnte.

## Geplante Änderungen

### API
- Doppelte Elemente in den Response Klassen zusammenfassen (in Bearbeitung)
- API-Requests Refactoring: Doppelte Code Blöcke zusammenfassen
- Exception Handling hinzufügen
- Kompiler-Warnungen beheben

### UI
- ToolTips im ListView für alle Spalten ermöglichen (in Bearbeitung) (https://stackoverflow.com/questions/13069137/how-to-set-tooltip-for-a-listviewsubitem)
- Einstellungsdialog
- Transaktionen & Positionen: Durchblättern & Filtern
- ListViews: Sortierungen hinzufügen
- Projekt mit Icons aufhübschen

## Changelog

Eine Übersicht über Änderungen und Updates im Projekt.

- 1.0.0.3 (19.01.2025)
  - Code Optimierungen

- 1.0.0.2 (13.01.2025)
  - Unbekannte Postbox Kategorie sorgt nicht mehr für einen Absturz
  - Eingabefelder erhalten jetzt den Fokus beim Start
  - Navigieren durch die Postbox ermöglicht

- 1.0.0.1 (12.01.2025)
  - UserSecrets Datei ist jetzt optional
  - ClientId & CientSecret als Eingabe in der UI beim Login ermöglicht
  - Objekt für die ClientSessionId hinzugefügt & Login-Header ausgelagert
  - Kleinere UI Optmimerungen

- 1.0.0.0 (11.01.2025)
  - Erstveröffentlichung auf GitHub