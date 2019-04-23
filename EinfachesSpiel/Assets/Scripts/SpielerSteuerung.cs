using UnityEngine;

public class SpielerSteuerung : MonoBehaviour
{

    //Variablendeklaration

    public enum Cursor {bewegen, zielauswählen};
    public Cursor aktuellerCursor;

    public GameObject[] spieler1; //Liste aller Charaktere Spieler1
    public GameObject[] spieler2; //Liste aller Charaktere Spieler2
    public GameObject[] spieler; //Liste aller Charaktere des Spieler, der gerade an der Reihe ist
    public int anzahlSpieler1;
    public int anzahlSpieler2;
    public int anzahlSpieler;
    public int aktuellerSpieler; //Int für das Array des aktuellen Spielers
    public int aktuellesZiel;
    public GameObject marker;
    public GameObject angriffsMarker;
    public bool spielzug; //FALSE Spieler 1 ist am Zug
    public Vector3 altPosition;
    public GameObject[] hindernisse;
    public GameObject[] hindernissChild;
    public GameObject[] moeglicheZiele;
    // Start is called before the first frame update

    void Start()
    {
        spielzug = false;
        //Sucht alle GameObjects mit dem Spieler Tag und speichert sie in einem Array
        spieler1 =  GameObject.FindGameObjectsWithTag("Spieler0");
        anzahlSpieler1 = spieler1.Length;

        //Sucht alle GameObjects mit dem Spieler2 Tag und speichert sie in einem Array
        spieler2 = GameObject.FindGameObjectsWithTag("Spieler1");
        anzahlSpieler2 = spieler2.Length;

        aktuellerSpieler = 0;
        //Erzeugt die Marker
        marker = Instantiate(marker, new Vector3(0, 0, 0), Quaternion.identity);
        angriffsMarker = Instantiate(angriffsMarker, new Vector3(0, 0, 0), Quaternion.identity);
        angriffsMarker.GetComponent<SpriteRenderer>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Führt Funktion je nach aktuellem Cursor Zustand aus
        switch (aktuellerCursor)
        {
            case Cursor.bewegen:
                marker.GetComponent<SpriteRenderer>().enabled = true;
                SpielerBewegen();
                break;

            case Cursor.zielauswählen:
                angriffsMarker.GetComponent<SpriteRenderer>().enabled = true;
                ZielAussuchen();
                break;

            default:
                marker.GetComponent<SpriteRenderer>().enabled = true;
                aktuellerCursor = Cursor.bewegen;
                break;

        }

       
    }

    //Enthält Charakter bewegen, Charakter wechseln, Runde beenden
    void SpielerBewegen()
    {
        //Sucht nach allen Hindernissen auf der Karte
        hindernissChild = GameObject.FindGameObjectsWithTag("Hinderniss");
        hindernisse = new GameObject[hindernissChild.Length];
        for (int i = 0; i < hindernissChild.Length; i++)
        {
            hindernisse[i] = hindernissChild[i].transform.parent.gameObject;
        }


        //Überprüft, welcher Spieler gerade an der Reihe ist
        if (spielzug == false)
        {
            spieler = spieler1;
            anzahlSpieler = anzahlSpieler1;
        }
        else
        {
            spieler = spieler2;
            anzahlSpieler = anzahlSpieler2;
        }



        //Setzt Marker über aktuellen Charakter
        marker.transform.position = spieler[aktuellerSpieler].GetComponent<Transform>().position;
        marker.transform.Translate(0, 0.66f + 0.05f * Mathf.Sin(3.5f * Time.time), 0);
        //Bekommt das Script vom aktuellen Spieler(Um Variablen zu lesen/schreiben)
        StatsCharakter script = spieler[aktuellerSpieler].GetComponent<StatsCharakter>();

        //Bewegung von Spieler
        if (Input.GetKeyDown(KeyCode.W) && script.bewegung > 0)
        {
            //Speichert alte Position, falls zu dieser zurück gekehrt wird
            altPosition = spieler[aktuellerSpieler].transform.position;
            spieler[aktuellerSpieler].transform.Translate(0, 1, 0);

            //Falls Kollision auftritt zurück setzten
            if (KollisionUeberpruefung(spieler[aktuellerSpieler].transform.position))
            {
                spieler[aktuellerSpieler].transform.position = altPosition;
            }
            //Ansonsten Bewegung abziehen
            else
            {
                script.bewegung--;
            }
        }
        if (Input.GetKeyDown(KeyCode.S) && script.bewegung > 0)
        {
            altPosition = spieler[aktuellerSpieler].transform.position;
            spieler[aktuellerSpieler].transform.Translate(0, -1, 0);

            //Falls Kollision auftritt zurück setzten
            if (KollisionUeberpruefung(spieler[aktuellerSpieler].transform.position))
            {
                spieler[aktuellerSpieler].transform.position = altPosition;
            }
            //Ansonsten Bewegung abziehen
            else
            {
                script.bewegung--;
            }
        }
        if (Input.GetKeyDown(KeyCode.A) && script.bewegung > 0)
        {
            altPosition = spieler[aktuellerSpieler].transform.position;
            spieler[aktuellerSpieler].transform.Translate(-1, 0, 0);

            //Falls Kollision auftritt zurück setzten
            if (KollisionUeberpruefung(spieler[aktuellerSpieler].transform.position))
            {
                spieler[aktuellerSpieler].transform.position = altPosition;
            }
            //Ansonsten Bewegung abziehen
            else
            {
                script.bewegung--;
            }
        }
        if (Input.GetKeyDown(KeyCode.D) && script.bewegung > 0)
        {
            altPosition = spieler[aktuellerSpieler].transform.position;
            spieler[aktuellerSpieler].transform.Translate(1, 0, 0);

            //Falls Kollision auftritt zurück setzten
            if (KollisionUeberpruefung(spieler[aktuellerSpieler].transform.position))
            {
                spieler[aktuellerSpieler].transform.position = altPosition;
            }
            //Ansonsten Bewegung abziehen
            else
            {
                script.bewegung--;
            }
        }

        //Wechsel aktueller Charakter
        if (Input.GetKeyDown(KeyCode.Q))
        {
            aktuellerSpieler--;
            if (aktuellerSpieler < 0)
            {
                aktuellerSpieler = anzahlSpieler - 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            aktuellerSpieler++;
            aktuellerSpieler = aktuellerSpieler % anzahlSpieler;
        }

        //Wechsel zur Zielauswahl
        if (Input.GetKeyDown(KeyCode.F))
        {
            marker.GetComponent<SpriteRenderer>().enabled = false;
            aktuellerCursor = Cursor.zielauswählen;
        }

        //Beenden der Runde
        if (Input.GetKeyDown(KeyCode.Space))
        {

            for (int i = 0; i < anzahlSpieler; i++)
            {
                spieler[i].GetComponent<StatsCharakter>().bewegung = spieler[i].GetComponent<StatsCharakter>().BEWEGUNG_MAX;
            }
            //Wechselt, wer an der Reihe ist
            spielzug ^= true;
            //Setzt aktuellen Charakter zurück(Um Überlauf bei ungleich großen Teams zu verhindern)
            aktuellerSpieler = 0;
            aktuellesZiel = 0;
            aktuellerCursor = Cursor.bewegen;
        }



    }

    void ZielAussuchen()
    {

        if (spielzug)
        {
            moeglicheZiele = GameObject.FindGameObjectsWithTag("Spieler0");
        }
        else
        {
            moeglicheZiele = GameObject.FindGameObjectsWithTag("Spieler1");
        }

        //Wechsel aktuelles Ziel
        if (Input.GetKeyDown(KeyCode.Q))
        {
            aktuellesZiel--;
            if (aktuellesZiel < 0)
            {
                aktuellesZiel = moeglicheZiele.Length - 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            aktuellesZiel++;
            aktuellesZiel = aktuellesZiel % moeglicheZiele.Length;
        }


        //Setzt Marker über aktuellen Charakter
        angriffsMarker.transform.position = moeglicheZiele[aktuellesZiel].GetComponent<Transform>().position;
        //Bekommt das Script vom aktuellen Spieler(Um Variablen zu lesen/schreiben)
        StatsCharakter gegnerScript = moeglicheZiele[aktuellesZiel].GetComponent<StatsCharakter>();

        //Rückkehr zum Bewegen
        if (Input.GetKeyDown(KeyCode.F))
        {
            angriffsMarker.GetComponent<SpriteRenderer>().enabled = false;
            aktuellerCursor = Cursor.bewegen;
        }

    }

    bool KollisionUeberpruefung(Vector3 pos)
    {
        int kollision = 0;
       
        for(int i = 0; i < hindernisse.Length; i++)
        {
            //Entfernung von 0.1 lässt ein bisschen Spielraum, falls Mittelpunkt nicht exakt in der Mitte 
            //Und nicht das eigene Objekt ist
            if (Vector3.Distance(pos, hindernisse[i].transform.position) < 0.5)
            {
                
                kollision++;
            }
        }        
        return kollision > 1;
    }
}

