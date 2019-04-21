using UnityEngine;

public class Stamina : MonoBehaviour
{

    SpielerSteuerung globalScript;

    // Start is called before the first frame update
    void Start()
    {
        globalScript = GameObject.Find("/Global").GetComponent<SpielerSteuerung>();
        if(globalScript == null)
        {
            Debug.Log("finde kein globales Script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //finde Script für den aktuellen Spieler
        StatsCharakter script = globalScript.spieler[globalScript.aktuellerSpieler].GetComponent<StatsCharakter>();
        Debug.Log(script.bewegung);
    }
}
