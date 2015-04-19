using UnityEngine;
using System.Collections;

public class DestroyAfterDelay : MonoBehaviour
{
    public float delay = 1f;

    void OnEnable()
    {
        Destroy(gameObject, delay);
    }

}
