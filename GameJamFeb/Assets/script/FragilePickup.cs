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
        if (PlayerisinObj)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("e pressed in update");
                playerScript.Instance.newObjectSpawn();
                Destroy(this.transform.parent.gameObject);
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            PlayerisinObj = true;
            if (eUI == null)
            {
                eUI = Instantiate(euiPrefab, this.transform.position-new Vector3(0,1,0), Quaternion.identity);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            PlayerisinObj = false;
            Destroy(eUI);
        }
    }
}
