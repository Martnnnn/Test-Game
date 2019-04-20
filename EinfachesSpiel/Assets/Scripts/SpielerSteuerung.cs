using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpielerSteuerung : MonoBehaviour
{
    
    public GameObject[] spieler;
    int anzahlSpieler;
    int aktuellerSpieler;
    // Start is called before the first frame update

    void Start()
    {
        spieler =  GameObject.FindGameObjectsWithTag("Spieler");
        anzahlSpieler = spieler.Length;
        aktuellerSpieler = 0;

    }

    // Update is called once per frame
    void Update()
    {
        sSpieler script = spieler[aktuellerSpieler].GetComponent<sSpieler>();

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            script.bewegung = 5;
        }
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
