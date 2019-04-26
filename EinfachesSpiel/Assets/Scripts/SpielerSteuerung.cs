using UnityEngine;

public class SpielerSteuerung : MonoBehaviour
{

    //Variablendeklaration

    public enum Cursor {bewegen, zielauswählen};
    public Cursor aktuellerCursor;

    public GameObject[] spieler1; //Liste aller Charaktere Spieler1
    public GameObject[] spieler2; //Liste aller Charaktere Spieler2
    public GameObject[] spieler; //Liste aller Charaktere des Spieler, der gerade an der Reihe ist
    public int anzahlSpieler;
    public int aktuellerSpieler; //Int für das Array des aktuellen Spielers
    public int angreifenderSpieler = -1; //vorm zum Angreifen gewechselten, ausgewählter Spielers
    public int aktuellesZiel;
    public GameObject marker;
    public GameObject angriffsMarker;
    public bool spielzug; //FALSE Spieler 1 (Spieler0) ist am Zug
    public Vector3 altPosition;
    public GameObject[] hindernisse;
    public GameObject[] hindernissChild;
    // Start is called before the first frame update

    void Start()
    {
        spielzug = false;
        //Sucht alle GameObjects mit dem Spieler Tag und speichert sie in einem Array
        spieler1 =  GameObject.FindGameObjectsWithTag("Spieler0");

        //Sucht alle GameObjects mit dem Spieler2 Tag und speichert sie in einem Array
        spieler2 = GameObject.FindGameObjectsWithTag("Spieler1");

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

        //falls benötigt
        //StatsCharakter script = spieler[aktuellerSpieler].GetComponent<StatsCharakter>();

        //Bewegung von Spieler
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            move(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            move(0, -1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move(-1, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            move(1, 0);
        }

        zeigerWechsel(true);

        //Beenden der Runde
        endRound();


    }

    void ZielAussuchen()
    {
        zeigerWechsel(false);

        if (angreifenderSpieler >= 0)
        {
            //hole Attacke
            Attack attacke = spieler1[angreifenderSpieler].GetComponent<StatsCharakter>().getAttack();

            //prüfe Treffer
            Vector3 posGeg = spieler2[aktuellerSpieler].GetComponent<Transform>().position;
            Vector3 posAng = spieler1[angreifenderSpieler].GetComponent<Transform>().position;

            //debug
            Debug.Log(attacke.hit(0, new Vector2(posGeg[0] - posAng[0], posGeg[1] - posAng[1])));
        }
    }

    void endRound()
    {
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
    
    void zeigerWechsel(bool ownTeam)
    {
        //wessen Spieler
        if (spielzug ^ ownTeam)
        {
            spieler = spieler1;
        }
        else
        {
            spieler = spieler2;
        }
        anzahlSpieler = spieler.Length;

        //Wechsel aktueller Charakter
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            aktuellerSpieler--;
            if (aktuellerSpieler < 0)
            {
                aktuellerSpieler = anzahlSpieler - 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            aktuellerSpieler++;
            aktuellerSpieler = aktuellerSpieler % anzahlSpieler;
        }

        //Modus wechseln

        //Wechsel zur Zielauswahl
        if (ownTeam && Input.GetKeyDown(KeyCode.F))
        {
            marker.GetComponent<SpriteRenderer>().enabled = false;
            aktuellerCursor = Cursor.zielauswählen;
            angreifenderSpieler = aktuellerSpieler;
        }
        //Rückkehr zum Bewegen
        if (!ownTeam && Input.GetKeyDown(KeyCode.F))
        {
            angriffsMarker.GetComponent<SpriteRenderer>().enabled = false;
            aktuellerCursor = Cursor.bewegen;
            angreifenderSpieler = -1;
        }

        //Setzt (potentiell aktualisierten) Marker über aktuellen Charakter
        if (ownTeam)
        {
            marker.transform.position = spieler[aktuellerSpieler].GetComponent<Transform>().position;
            marker.transform.Translate(0, 0.66f + 0.05f * Mathf.Sin(3.5f * Time.time), 0);
        }
        else
        {
            angriffsMarker.transform.position = spieler[aktuellerSpieler].GetComponent<Transform>().position;
        }
    }

    void move(int x, int y)
    {
        StatsCharakter script = spieler[aktuellerSpieler].GetComponent<StatsCharakter>();
        if (script.bewegung > 0)
        {
            Vector3 position = spieler[aktuellerSpieler].transform.position;

            //Falls Kollision auftritt zurück setzten
            if (!KollisionUeberpruefung(new Vector2(position[0] + x, position[1] + y)))
            {
                spieler[aktuellerSpieler].transform.Translate(x, y, 0);
                script.bewegung--;
            }
        }
    }

    bool KollisionUeberpruefung(Vector2 pos)
    {
        bool kollision = false;

        //Sucht nach allen Hindernissen auf der Karte
        hindernissChild = GameObject.FindGameObjectsWithTag("Hinderniss");
        hindernisse = new GameObject[hindernissChild.Length];
        for (int i = 0; i < hindernissChild.Length; i++)
        {
            hindernisse[i] = hindernissChild[i].transform.parent.gameObject;
        }

        //helfender Hilfs-Vektor ist hilfreich am helfen
        Vector2 hilfsV;
        for (int i = 0; i < hindernisse.Length; i++)
        {
            //Entfernung von 0.1 lässt ein bisschen Spielraum, falls Mittelpunkt nicht exakt in der Mitte 
            //Und nicht das eigene Objekt ist

            hilfsV = new Vector2(hindernisse[i].transform.position[0], hindernisse[i].transform.position[1]);
            
            if (Vector2.Distance(pos, hilfsV) < 0.5)
            {
                kollision = true; ;
            }
        }
        
        return kollision;
    }
}

