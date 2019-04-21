using UnityEngine;

public class Stamina : MonoBehaviour
{

    public GameObject prefabStaminaBar; //Referenz aus Editor
    public GameObject[] staminaBars = new GameObject[0];
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

        //erzeuge neue Stamina Bars falls mehr benötigt als
        //erwartet und füge ans array dran
        if (staminaBars.Length < script.bewegung)
        {
            GameObject[] newBars = new GameObject[script.bewegung];
            for (int i = 0; i < staminaBars.Length; i++)
            { //erstmal alles alte ins neue
                newBars[i] = staminaBars[i];
            }
            for (int i = staminaBars.Length; i < script.bewegung; i++)
            { //dann zusätzliche ins neue
                newBars[i] = Instantiate(prefabStaminaBar, new Vector3(9, -4 + (1.2f * i), 0), Quaternion.identity);
            }
            staminaBars = newBars; //pointer aufs neue umlegen
        }

        //eigentliches Anzeigen
        for(int i = 0; i < staminaBars.Length; i++)
        {
            staminaBars[i].SetActive(script.bewegung > i);
        }
    }
}
