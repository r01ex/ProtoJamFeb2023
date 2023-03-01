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
            Invoke("reset", duration);
            playerScript.Instance.slowfallItemSprite.SetActive(true);
            this.gameObject.SetActive(false);         
        }
    }
    void reset()
    {
        playerScript.Instance.ItemGravMult = 1;
        playerScript.Instance.slowfallItemSprite.SetActive(false);
        Destroy(this.gameObject);
    }
}
