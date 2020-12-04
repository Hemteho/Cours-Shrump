using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{
    private float Lifetime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Lifetime);
    }

}
