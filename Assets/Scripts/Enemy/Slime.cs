using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{

    [SerializeField]
    private GameObject soul;

    private Animator animator;

    private Transform target;
    private Rigidbody2D myRigiddody;
    private float chaseRadius = 4;
    private float attackRadius = 1.2f;

    private Color color;
    private float hitedTime;

    public int Damage
    {
        get { return damage; }
    }

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
        myRigiddody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();    
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


        gameObject.transform.Find("View").gameObject.GetComponent<SpriteRenderer>().color = color;
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
        Vector3 temp;
        if (Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isRunning", false);          
            temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            ChangeAnim(temp - transform.position);
        }
        
        else if (Vector3.Distance(target.position, transform.position) <= chaseRadius &&
           Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isRunning", true);

            temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            ChangeAnim(temp - transform.position);
            myRigiddody.MovePosition(temp);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void ChangeAnim(Vector3 direction)
    {
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0)
            {
                SetAnimValue(Vector2.right);
            }
            else if(direction.x < 0)
            {
                SetAnimValue(Vector2.left);
            }

        }else if(Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                SetAnimValue(Vector2.up);
            }
            else if (direction.y < 0)
            {
                SetAnimValue(Vector2.down);
            }
        }
    }

    void SetAnimValue(Vector2 value)
    {
        animator.SetFloat("SlimeX", value.x);
        animator.SetFloat("SlimeY", value.y);
    }

}
