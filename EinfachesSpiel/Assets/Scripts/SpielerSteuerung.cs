using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpielerSteuerung : MonoBehaviour
{
    //Variablendeklaration
    public GameObject[] spieler;
    int anzahlSpieler;
    int aktuellerSpieler;
    public GameObject marker;
    // Start is called before the first frame update

    void Start()
    {
        //Sucht alle GameObjects mit dem Spieler Tag und speichert sie in einem Array
        spieler =  GameObject.FindGameObjectsWithTag("Spieler");
        anzahlSpieler = spieler.Length;
        aktuellerSpieler = 0;
        marker = Instantiate(marker, new Vector3(0, 0, 0), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        marker.transform.position = spieler[aktuellerSpieler].GetComponent<Transform>().position;
        marker.transform.Translate(0, 0.66f+0.08f*Mathf.Sin(3f*Time.time), 0);
        //Bekommt das Script vom aktuellen Spieler(Um Variablen zu lesen/schreiben)
        sSpieler script = spieler[aktuellerSpieler].GetComponent<sSpieler>();

        //Bewegung von Spieler
        if (Input.GetKeyDown(KeyCode.W) && script.bewegung > 0)
        {
            spieler[aktuellerSpieler].transform.Translate(0, 1, 0);
            script.bewegung--;
        }
        if (Input.GetKeyDown(KeyCode.S) && script.bewegung > 0)
        {
            spieler[aktuellerSpieler].transform.Translate(0, -1, 0);
            script.bewegung--;
        }
        if (Input.GetKeyDown(KeyCode.A) && script.bewegung > 0)
        {
            spieler[aktuellerSpieler].transform.Translate(-1, 0, 0);
            script.bewegung--;
        }
        if (Input.GetKeyDown(KeyCode.D) && script.bewegung > 0)
        {
            spieler[aktuellerSpieler].transform.Translate(1, 0, 0);
            script.bewegung--;
        }
        //Beenden der Runde
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            for(int i = 0; i<anzahlSpieler; i++){
                spieler[i].GetComponent<sSpieler>().bewegung = 5;
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
    }
}
