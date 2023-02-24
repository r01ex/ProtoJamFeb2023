using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Spike : MonoBehaviour
{
    [SerializeField] float magnitude;
    [SerializeField] float basejump;
    [SerializeField] float fragilemagnitude;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {          
            Rigidbody2D PCrbody;
            PCrbody = playerScript.Instance.gameObject.GetComponent<Rigidbody2D>();
            PCrbody.velocity = new Vector2(PCrbody.velocity.x, basejump);
            PCrbody.AddForce(new Vector2(-collision.relativeVelocity.normalized.x * magnitude, 0));
            if (playerScript.Instance.stackedObjs.Count > 0)
            {
                StartCoroutine(playerScript.Instance.stackedObjs[playerScript.Instance.stackedObjs.Count-1].GetComponent<FragileScript>().dropbySpike(fragilemagnitude));
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D PCrbody;
            PCrbody = playerScript.Instance.gameObject.GetComponent<Rigidbody2D>();
            PCrbody.velocity = new Vector2(PCrbody.velocity.x, basejump);
            PCrbody.AddForce(new Vector2(-collision.relativeVelocity.normalized.x * magnitude, 0));
            if (playerScript.Instance.stackedObjs.Count > 0)
            {
                StartCoroutine(playerScript.Instance.stackedObjs[playerScript.Instance.stackedObjs.Count - 1].GetComponent<FragileScript>().dropbySpike(fragilemagnitude));
            }
        }
    }
}