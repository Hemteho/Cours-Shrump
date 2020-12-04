using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Item otherItem = other.GetComponent<Item>();

            if (otherItem != null)
            {
                otherItem.SetDamage(damage);

                Destroy(gameObject);
                GetComponent<Explosion>()?.Explode();

                Score.EffectivScore += 15;

                if (Score.ScoreAmount > 400)
                {
                    return;
                }

                Score.ScoreAmount += 10;

            }
        }

    }

}
