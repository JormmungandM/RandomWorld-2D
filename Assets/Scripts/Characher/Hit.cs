using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{

    private float hitTime;

    // Start is called before the first frame update
    void Start()
    {
        hitTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if( hitTime > 0f)
        {
            hitTime -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(hitTime <= 0f)
            {
                Debug.Log("Hit");
                Slime enemy = other.GetComponent<Slime>();
                enemy.HealthPoints -= 10;
                enemy.EnemyHited = Color.red;
                hitTime = 0.15f;
            }
        }
    }

}
