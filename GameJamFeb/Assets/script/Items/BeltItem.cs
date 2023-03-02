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
            playerScript.Instance.BeltitemdurationLeft = duration;
            playerScript.Instance.ItemFollowspeedMult = mult;
            Invoke("reset", duration);
            /*
            foreach (GameObject g in playerScript.Instance.stackedObjs)
            {
                g.GetComponent<FragileScript>().BeltItem(duration);
            }
            */
            playerScript.Instance.beltItemSprite.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
    void reset()
    {
        if (playerScript.Instance.BeltitemdurationLeft <= 0.1f)
        {
            playerScript.Instance.beltItemSprite.SetActive(false);
            playerScript.Instance.ItemFollowspeedMult = 1;
            Destroy(this.gameObject);
        }
    }
}
