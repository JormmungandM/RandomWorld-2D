using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;



public class Movement : MonoBehaviour
{
    enum PlayerState { run, attack }

    [SerializeField]
    private PlayerState playerState;

    [SerializeField]
    private float moveSpeed = 8;

    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;


    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        playerState = PlayerState.run;
        change = Vector3.zero;
    }


    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetMouseButtonDown(0) && playerState != PlayerState.attack && GameMenu.GameMenuIs == false)
        {
            StartCoroutine(AttackCO());
        }
        if (playerState == PlayerState.run)
        {
            UpdateAnimation();
        }

    }

    IEnumerator AttackCO()
    {
        animator.SetBool("isAttacking", true);
        playerState = PlayerState.attack;
        yield return null;
        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(.33f);
        playerState = PlayerState.run;
    }



    void UpdateAnimation()
    {
        if (change != Vector3.zero)
        {
            MoveHero();
            animator.SetFloat("Horizontal", change.x);
            animator.SetFloat("Vertical", change.y);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void MoveHero()
    {
        change.Normalize();
        myRigidbody.MovePosition(transform.position + change * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Soul"))
        {
            Destroy(other.gameObject);
            Stats.Souls += 1;
        }
    }

}
