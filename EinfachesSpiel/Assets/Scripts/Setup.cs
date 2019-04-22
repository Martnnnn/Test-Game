using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour
{

    public GameObject prefabBigDude; //Referenz aus Editor
    public GameObject prefabDefaultDude; //Referenz aus Editor

    //hier kommen die Charaktäre der Spieler rein
    readonly string[][] spieler = new string[][]
    {
        new string[]{ "DD", "DD", "DD" }, //Charaktäre von Spieler 1
        new string[]{ "BD", "DD" } //Charaktäre von Spieler 2
    };

    void Awake()
    {
        //Variablen, die in der Schleife gebraucht werden
        GameObject zuletztErstellterSpieler;
        Vector3 pos;

        for(int sp = 0; sp < spieler.Length; sp++)
        {
            for (int i = 0; i < spieler[sp].Length; i++)
            {
                zuletztErstellterSpieler = null;
                pos = new Vector3((int)(10 * Random.value - 5), (int)(10 * Random.value - 5), 0);
                switch (spieler[sp][i])
                {
                    case "BD":
                        zuletztErstellterSpieler = Instantiate(prefabBigDude, pos, Quaternion.identity);
                        break;
                    case "DD":
                        zuletztErstellterSpieler = Instantiate(prefabDefaultDude, pos, Quaternion.identity);
                        break;
                }
                if(zuletztErstellterSpieler != null)
                {
                    zuletztErstellterSpieler.tag = "Spieler" + sp;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
