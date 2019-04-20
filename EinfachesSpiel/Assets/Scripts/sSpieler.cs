using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sSpieler : MonoBehaviour
{
    int bewegung;
    // Start is called before the first frame update
    void Start()
    {
        bewegung = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) && bewegung > 0)
        {
            transform.Translate(0, 1, 0);
            bewegung--;
        }
        if(Input.GetKeyDown(KeyCode.S) && bewegung > 0)
        {
            transform.Translate(0, -1, 0);
            bewegung--;
        }
        if(Input.GetKeyDown(KeyCode.A) && bewegung > 0)
        {
            transform.Translate(-1, 0, 0);
            bewegung--;
        }
        if(Input.GetKeyDown(KeyCode.D) && bewegung > 0)
        {
            transform.Translate(1, 0, 0);
            bewegung--;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            bewegung = 5;
        }
    }
}
