using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsCharakter : MonoBehaviour
{
    //Hier kommen alle Charaktter bezogenen Variablen rein (Leben,Schaden usw.)
    public int bewegung;
    public int BEWEGUNG_MAX;
    public int leben;
    public int LEBEN_MAX;
    public GameObject hinderniss;
    GameObject eigenesHinderniss;
    // Start is called before the first frame update
    void Start()
    {
        //Erstellt ein stantiatenHinderniss GameObject als eigenes Child
       eigenesHinderniss = Instantiate(hinderniss,transform.position, Quaternion.identity);
       eigenesHinderniss.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
