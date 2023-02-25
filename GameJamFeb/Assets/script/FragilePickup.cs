using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class FragilePickup : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject eUI;
    [SerializeField] GameObject euiPrefab;
    bool PlayerisinObj;
    private void Update()
    {
        if(eUI!=null)
        {
            eUI.transform.position = this.transform.position - new Vector3(0, 1, 0);
        }
        if (PlayerisinObj && playerScript.Instance.LockedFragilePickup == this)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject p = this.transform.parent.gameObject;
                Debug.Log("picking up " + p);
                playerScript.Instance.PickupFragile(p);
                playerScript.Instance.LockedFragilePickup = null;
                //Destroy(p);
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            if(playerScript.Instance.LockedFragilePickup == this)
            {
                playerScript.Instance.LockedFragilePickup = null;
            }
            PlayerisinObj = false;
            Destroy(eUI);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7 && playerScript.Instance.LockedFragilePickup == null)
        {
            PlayerisinObj = true;
            if (eUI == null)
            {
                eUI = Instantiate(euiPrefab, this.transform.position - new Vector3(0, 1, 0), Quaternion.identity);
            }
            playerScript.Instance.LockedFragilePickup = this;
        }
    }
}
