using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private Vector2 tagPosition;
    private Rigidbody2D body;
    private BoxCollider2D myCollider;
    private Animator myAnimator;

    public float smoothing = 100;
    public int lossFood = 10;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Protag").transform;
        body = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        tagPosition = transform.position;
        ActionManager.Instance.enemyList.Add(this);

    }

    void Update()
    {
        body.MovePosition(Vector2.Lerp(transform.position, tagPosition, smoothing * Time.deltaTime));
    }


    public void Move()
    {
        Vector2 offset = player.position - transform.position;
        if (offset.magnitude < 1.1f)
        {
            myAnimator.SetTrigger("Attack");
            player.SendMessage("TakeDamage", lossFood);
        }
        else
        {
            float x = 0, y = 0;
            // Move along the y axis
            if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x))
            {
                if (offset.y < 0)
                {
                    y = -1;
                }
                else
                {
                    y = 1;
                }
            }
            // Move along the x axis
            else
            {
                if (offset.x < 0)
                {
                    x = -1;
                }
                else
                {
                    x = 1;
                }
            }
            
            Vector2 posNow = new Vector2(x, y);
            // Collision Detection
            myCollider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(tagPosition, tagPosition + posNow);
            myCollider.enabled = true;
            if (hit.transform == null)
            {
                tagPosition += posNow;
            }
            else
            {
                if(hit.collider.tag == "Food" || hit.collider.tag == "Soda")
                {
                    tagPosition += posNow;
                }
            }
            

        }
    }
}
