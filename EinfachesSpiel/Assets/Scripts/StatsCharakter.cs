using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sSpieler : MonoBehaviour
{
    //Hier kommen alle Charaktter bezogenen Variablen rein (Leben,Schaden usw.)
    public int bewegung;
    public int leben;
    // Start is called before the first frame update
    void Start()
    {
        bewegung = 5;
        leben = 100;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
