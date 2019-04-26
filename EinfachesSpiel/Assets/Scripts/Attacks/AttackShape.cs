
using UnityEngine;

public class AttackShape //erbt nicht von MonoBehaviour
{
    private float[,] damagePattern = new float[0,0]; //Schadensverteilung in einem Gebiet; Länge und Breite müssen ungerade
    private int maxRepitions = -1; //-1 = keine Grenze
    private float repitionDampening = 1f; //damit wird pro Wiederholung der Schaden multipliziert

    private bool[,] repitionPattern = new bool[3,3];
    /* Erklärung zu  repitionPattern:
     * 
     *   Bei zB {{true, false, true}, {false, true, true}, {false, false, false}} als repitionPattern und
     *                [0 ][.5][0 ]
     *                [.1][.8][.1]
     *   einer Matrix [.6][1 ][.6] als damagePattern
     *                [.2][.5][.2]
     *                [0 ][0 ][0 ]                                           |     und für {{true, true, true}, {false, false, false}, {false, true, false}}
     *   wird folgendes Muster in Zielrichtung gedreht:                      |     ergibt sich folgendes Muster:
     *                                                                       |
     *  .                                .                                .  |  .                            .                            .
     *    .                              .                              .    |    .                          .                          .
     *      .                            .                            .      |      .                        .                        .
     *       |Mat|                                               |Mat|       |       |Mat||Mat||Mat||Mat|  |Mat|  |Mat||Mat||Mat||Mat|
     *       |rix|                                               |rix|       |       |rix||rix||rix||rix|  |rix|  |rix||rix||rix||rix|
     *            |Mat|                                     |Mat||Mat|       |            |Mat||Mat||Mat|  |Mat|  |Mat||Mat||Mat|
     *            |rix|                                     |rix||rix|       |            |rix||rix||rix|  |rix|  |rix||rix||rix|
     *                 |Mat|                           |Mat||Mat||Mat| . . . | . . .           |Mat||Mat|  |Mat|  |Mat||Mat|           . . .
     * . . .           |rix|                           |rix||rix||rix|       |                 |rix||rix|  |rix|  |rix||rix|
     *                      |Mat|                 |Mat||Mat||Mat||Mat|       |                      |Mat|  |Mat|  |Mat|
     *                      |rix|                 |rix||rix||rix||rix|       |                      |rix|  |rix|  |rix|
     *                           [0 ][  .5   ][0 ]|   ||   ||   ||   |       |                           {Spieler}
     *                           [.1][  .8   ][.1]|Mat||Mat||Mat||Mat|       |                             |Mat|
     * . . .                     [.6]{Spieler}[.6]|rix||rix||rix||rix| . . . | . . .                       |rix|                       . . .
     *                           [.2][  .5   ][.2]|   ||   ||   ||   |       |                             |Mat|
     *                           [0 ][   0   ][0 ]|   ||   ||   ||   |       |                             |rix|
     *                                                                       |                               .
     *                                                                       |                               .
     *                                                                       |                               .
     *                                                                       |
     */

    public AttackShape(float[,] damagePattern, bool[,] repitionPattern, int maxRepitions, float repitionDampening)
    {
        this.damagePattern = damagePattern;
        this.repitionPattern = repitionPattern;
        this.maxRepitions = maxRepitions;
        this.repitionDampening = repitionDampening;
    }

    public float hit(int direction, Vector2 relativePos) //relativePos als Differenz zwichen benutzender Spieler und potentiellem Ziel
                                                         //direction 0 : UP; 1 : RIGHT; 2 : DOWN; 3 : LEFT
    {
        //adjustiere für direction
        switch(direction)
        {
            case 0: break;
            case 1:
                relativePos = new Vector2(-relativePos[1], relativePos[0]);
                break;
            case 2:
                relativePos = new Vector2(-relativePos[0], -relativePos[1]);
                break;
            case 3:
                relativePos = new Vector2(relativePos[1], -relativePos[0]);
                break;
        }


        //wo ist relativePos
        int[] i = new int[2];
        //rechts / links
        if (relativePos[0] < -(damagePattern.GetLength(0) / 2)) //im 2. oder 3. Quadrant
        {
            i[1] = 0;
        }
        else if (relativePos[0] > damagePattern.GetLength(0) / 2) //im 1. oder 4. Quadrant
        {
            i[1] = 2;
        }
        else //auf vertikaler Linie
        {
            i[1] = 1;
        }
        //oben / unten
        if (relativePos[1] > damagePattern.GetLength(1) / 2) //im 1. oder 2. Quadrant
        {
            i[0] = 0;
        }
        else if (relativePos[1] < -(damagePattern.GetLength(1) / 2)) //im 3. oder 4. Quadrant
        {
            i[1] = 2;
        }
        else //auf horizontaler Linie
        {
            i[1] = 1;
        }
        

        //liegt in der Mitte
        if(i[0] == 1 && i[1] == 1)
        {
            //keine Wiederholungsberechnung nötig
            return damagePattern[(int) (relativePos[0] + 0.5f + (damagePattern.GetLength(0) / 2)),
                                 (int) (relativePos[1] + 0.5f + (damagePattern.GetLength(1) / 2))];
        }
        else if (i[0] == 1 || i[1] == 1) //liegt auf einer der Linien
        {
            //kann unabhängig von damagePattern nicht erreicht werden
            if (!repitionPattern[i[0], i[1]])
            {
                return 0;
            }
            //entsprechendes Element abfragen
            ////welche Achse ist nicht == 1
            int w = (i[0] == 1) ? 1 : 0;
            ////wievielte Wiederholung
            int times = (int) (relativePos[w] + 0.5f - (damagePattern.GetLength(w) / 2)) / damagePattern.GetLength(w);
            if (times > maxRepitions)
            {
                return 0;
            }
            ////effizient Pow
            float damp = 1;
            for(int c = 0; c < times; c++)
            {
                damp *= repitionDampening;
            }
            ////return
            if (w == 1)
            {
                return damp * damagePattern[(int)(relativePos[0] + 0.5f + (damagePattern.GetLength(0) / 2)),
                            (int)(relativePos[1] + 0.5f + (damagePattern.GetLength(0) / 2)) % damagePattern.GetLength(w)];
            }
            else if (w == 0)
            {
                return damp * damagePattern[(int)(relativePos[0] + 0.5f + (damagePattern.GetLength(0) / 2)) % damagePattern.GetLength(w),
                            (int)(relativePos[1] + 0.5f + (damagePattern.GetLength(0) / 2))];
            }
            else
            {
                return -1;
            }
        }
        else
        {
            //Wiederholungs Zahlen individuell
            int xI = (int)(relativePos[0] - 0.5f /*<- ergibt sich aus Rundungen*/ - (damagePattern.GetLength(0) / 2)) / damagePattern.GetLength(0);
            int yI = (int)(relativePos[1] - 0.5f /*<- ergibt sich aus Rundungen*/ - (damagePattern.GetLength(1) / 2)) / damagePattern.GetLength(1);
            
            //liegt es auf 45° Diagonalen
            if(xI == yI)
            {
                //ist diese Diagonale / Zelle verboten
                if(!repitionPattern[i[0], i[1]] || Mathf.Max(xI, yI) > maxRepitions)
                {
                    return 0;
                }
            }
            else
            {
                //ist aktueller Fall verboten
                xI = Mathf.Abs(xI);
                yI = Mathf.Abs(yI);
                if (Mathf.Max(xI, yI) > maxRepitions) return 0;
                if (xI < yI && i[0] == 0 && i[1] == 0 && !(repitionPattern[0, 0] && repitionPattern[0, 1])) return 0;
                if (xI > yI && i[0] == 0 && i[1] == 0 && !(repitionPattern[0, 0] && repitionPattern[1, 0])) return 0;
                if (xI > yI && i[0] == 0 && i[1] == 2 && !(repitionPattern[2, 0] && repitionPattern[1, 0])) return 0;
                if (xI < yI && i[0] == 0 && i[1] == 2 && !(repitionPattern[2, 0] && repitionPattern[2, 1])) return 0;
                if (xI < yI && i[0] == 2 && i[1] == 0 && !(repitionPattern[2, 2] && repitionPattern[2, 1])) return 0;
                if (xI > yI && i[0] == 2 && i[1] == 0 && !(repitionPattern[2, 2] && repitionPattern[1, 2])) return 0;
                if (xI > yI && i[0] == 2 && i[1] == 2 && !(repitionPattern[0, 2] && repitionPattern[1, 2])) return 0;
                if (xI < yI && i[0] == 2 && i[1] == 2 && !(repitionPattern[0, 2] && repitionPattern[0, 1])) return 0;
            }

            //effizient Pow
            float damp = 1;
            for (int c = 0; c < Mathf.Max(xI, yI); c++)
            {
                damp *= repitionDampening;
            }
            //hier aknn man nur ankommen, wenn Zelle erlaubt ist
            return damp * damagePattern[(int)(relativePos[0] + 0.5f + (damagePattern.GetLength(0) / 2)) % damagePattern.GetLength(0),
                    (int)(relativePos[1] + 0.5f + (damagePattern.GetLength(1) / 2)) % damagePattern.GetLength(1)];
        }
    }
}
