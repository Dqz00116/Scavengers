using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int wallHP = 2;

    public Sprite damageSprite; 

    public void TakeDamage()
    {
        wallHP -= 1;
        GetComponent<SpriteRenderer>().sprite = damageSprite;
        if (wallHP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
