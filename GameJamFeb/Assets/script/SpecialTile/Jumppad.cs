using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumppad : MonoBehaviour
{
    [SerializeField] float magnitude;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("jumppad collision is : " + collision.gameObject);
        if(collision.collider.tag=="playerLeg")
        {
            Debug.Log("jumppad collide");
            playerScript.Instance.GetComponent<Rigidbody2D>().velocity = new Vector2(playerScript.Instance.GetComponent<Rigidbody2D>().velocity.x, magnitude);
            playerScript.Instance.coyoteTimeCounter = 0;
        }
    }
}
