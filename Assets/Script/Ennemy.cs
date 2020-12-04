using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    public GameObject Explosion;

    public Transform SpawnPoint;

    private bool ApplicationisClosed;

    private void OnApplicationQuit()
    {
        ApplicationisClosed = true;
    }

    void OnDestroy()
    {
        if(ApplicationisClosed == false)
        {
            Instantiate(Explosion, SpawnPoint.position, SpawnPoint.rotation);
        }
    }

}
