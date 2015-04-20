using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoDraw : MonoBehaviour
{

    public static List<NoDraw> all = new List<NoDraw>();


    Collider2D col;
    void Awake()
    {
        this.col = GetComponent<Collider2D>();
        all.Add(this);
    }


    public static bool isInNoDraw(Vector2 point)
    {

        foreach (NoDraw n in all)
        {
            if (n.gameObject.activeSelf && n.col.OverlapPoint(point))
            {
                return true;
            }
        }
        return false;
    }
}
