using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Score.ScoreAmount += 150;
            Score.EffectivScore += 1500;
            Destroy(gameObject);
        }
    }

}
