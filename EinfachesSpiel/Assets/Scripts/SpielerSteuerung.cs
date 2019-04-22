using UnityEngine;

public class SpielerSteuerung : MonoBehaviour
{
    //Variablendeklaration
    public GameObject[] spieler1; //Liste aller Charaktere Spieler1
    public GameObject[] spieler2; //Liste aller Charaktere Spieler2
    public GameObject[] spieler; //Liste aller Charaktere des Spieler, der gerade an der Reihe ist
    public int anzahlSpieler1;
    public int anzahlSpieler2;
    public int anzahlSpieler;
    public int aktuellerSpieler; //Int für das Array des aktuellen Spielers
    public GameObject marker;
    public bool spielzug; //FALSE Spieler 1 ist am Zug
    public Vector3 altPosition;
    public GameObject[] hindernisse;
    public GameObject[] hindernissChild;
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
        //Erzeugt einen Marker
        marker = Instantiate(marker, new Vector3(0, 0, 0), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        //Sucht nach allen Hindernissen auf der Karte
        hindernissChild = GameObject.FindGameObjectsWithTag("Hinderniss");
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
        marker.transform.Translate(0, 0.66f+0.05f*Mathf.Sin(3.5f*Time.time), 0);
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
            if(aktuellerSpieler < 0)
            {
                aktuellerSpieler = anzahlSpieler - 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            aktuellerSpieler++;
            aktuellerSpieler = aktuellerSpieler % anzahlSpieler;
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
        }
    }
    bool KollisionUeberpruefung(Vector3 pos)
    {
        bool kollision = false;
       
        for(int i = 0; i < hindernisse.Length; i++)
        {
            //Entfernung von 0.1 lässt ein bisschen Spielraum, falls Mittelpunkt nicht exakt in der Mitte 
            if (Vector3.Distance(pos, hindernisse[i].transform.position) < 0.1)
            {
                kollision = true;
            }
        }
        return kollision;
    }
}

