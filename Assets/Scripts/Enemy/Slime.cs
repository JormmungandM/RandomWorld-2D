using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{

    [SerializeField]
    private GameObject soul;

    private Transform target;
    private float chaseRadius = 6;
    private float attackRadius = 1;

    private Color color;
    private float hitedTime;

    public int HealthPoints
    {
        get { return health; }
        set { health = value; }
    }

    public Color EnemyHited
    {
        set 
        { 
            color = value;
            hitedTime = 0.6f;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        color = Color.white;
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (health <= 0)
        {
            soul.transform.position = transform.position;
            Instantiate(soul);
            Destroy(gameObject);
        }
       
            

        
        gameObject.GetComponent<SpriteRenderer>().color = color;
        if(hitedTime <= 0f)
        {
            color = Color.white;
            CheckDistance();
        }
        else
        {
            hitedTime -= Time.deltaTime;       
        }
         
    }




    void CheckDistance()
    {

        if(Vector3.Distance(target.position, transform.position) <= chaseRadius &&
           Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }



}
