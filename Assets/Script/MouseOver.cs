using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver : MonoBehaviour
{
    public GameObject Vessel1;

    void OnMouseOver()
    {

        Vessel1.SetActive(true);

    }

    void OnMouseExit()
    {

        Vessel1.SetActive(false);

    }
    
}
