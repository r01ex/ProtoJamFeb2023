using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltItem : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float mult;
    [SerializeField] GameObject beltSpritePrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            playerScript.Instance.ItemFollowspeedMult = mult;
            this.gameObject.SetActive(false);
            Invoke("reset", duration);
            //add sprite
        }
    }
    void reset()
    {
        playerScript.Instance.ItemFollowspeedMult = 1;
        //remove sprite
        Destroy(this.gameObject);
    }
}
