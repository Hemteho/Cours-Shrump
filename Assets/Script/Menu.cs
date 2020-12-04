using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject Vessel1;
    public GameObject Vessel2;

    public GameObject GameMenu;

    public GameObject Sequencer;

    public void VesselSelect1()
    {
        Vessel1.SetActive(true);
        Vessel2.SetActive(false);
        Sequencer.SetActive(true);
        GameMenu.SetActive(false);
    }
    public void VesselSelect2()
    {
        Vessel1.SetActive(false);
        Vessel2.SetActive(true);
        Sequencer.SetActive(true);
        GameMenu.SetActive(false);
    }
}
