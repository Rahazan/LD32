using UnityEngine;
using System.Collections;

public class ExplosiveTrack : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        Game.instance.ExplodePlayer();
    }



}
