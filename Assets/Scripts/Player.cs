using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float smoothing = 100;
    public float sleepTime = 0.12f;
    public float sleepTimer = 0;

    [HideInInspector] public Vector2 tagPosition = new Vector2(1, 1);
    private Rigidbody2D body;
    private BoxCollider2D myCollider;
    private Animator animator;

    // Start is called before the first fram e update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        body.MovePosition(Vector2.Lerp(transform.position, tagPosition, smoothing * Time.deltaTime));

        sleepTimer += Time.deltaTime;
        if (sleepTimer < sleepTime) return;

        if (ActionManager.Instance.food <= 0 || ActionManager.Instance.Clearance) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h != 0)
        {
            v = 0;
        }
        if (h != 0 || v != 0)
        {
            ActionManager.Instance.Reducefood(1);
            Vector2 posNow = new Vector2(h, v);

            myCollider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(tagPosition, tagPosition + posNow);
            myCollider.enabled = true;

            if (hit.transform == null)
            {
                tagPosition += posNow;         
            }
            else
            {
                switch (hit.collider.tag){
                    case "OutWall":
                        break;
                    case "Wall":
                        animator.SetTrigger("Attack");
                        hit.collider.SendMessage("TakeDamage");
                        break;
                    case "Food":
                        ActionManager.Instance.Addfood(10);
                        tagPosition += posNow;
                        Destroy(hit.transform.gameObject);
                        break;
                    case "Soda":
                        ActionManager.Instance.Addfood(20);
                        tagPosition += posNow;
                        Destroy(hit.transform.gameObject);
                        break;
                    case "Enemy":
                        break;
                }

            }
            ActionManager.Instance.OnPlayerMove();
            sleepTimer = 0;
        }
        
    }

    public void TakeDamage(int lossFood)
    {
        ActionManager.Instance.Reducefood(lossFood);
        animator.SetTrigger("Hit");
    }
}
