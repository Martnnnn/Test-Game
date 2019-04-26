using UnityEngine;

public class AtSimpleLine : Attack
{
    AttackShape shape;
    float damage = 10f;

    public AtSimpleLine()
    {
        float[,] damagePattern = new float[,] { { 1 } };
        int maxRepitions = -1;
        float repitionDampening = 1f;
        bool[,] repitionPattern = new bool[,] { { false, true, false }, { false, false, false }, { false, false, false } };

        shape = new AttackShape(damagePattern, repitionPattern, maxRepitions, repitionDampening);
    }

    public float hit(int direction, Vector2 relativePos)
    {
        return damage * shape.hit(direction, relativePos);
    }
}
