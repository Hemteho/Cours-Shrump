using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public int LifeCount;

    void Update()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z)));
        newPos.z = transform.position.z;


        transform.position = newPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {

            LifeCount = -1;

        }
    }
}
