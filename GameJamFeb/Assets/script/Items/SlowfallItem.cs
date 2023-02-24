using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowfallItem : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float mult;
    [SerializeField] GameObject SlowfallSpritePrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerScript.Instance.ItemGravMult = mult;
            this.gameObject.SetActive(false);
            Invoke("reset", duration);
            //add sprite
        }
    }
    void reset()
    {
        playerScript.Instance.ItemGravMult = 1;
        //remove sprite
        Destroy(this.gameObject);
    }
}
