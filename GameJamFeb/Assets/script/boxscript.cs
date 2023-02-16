using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class boxscript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject fUI;
    [SerializeField] GameObject fuiPrefab;
    bool PlayerisinObj;

    private void Update()
    {
        Debug.Log("in update");
        if (PlayerisinObj)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("f pressed in update");
                playerScript.Instance.newObjectSpawn();
                Destroy(this.gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            PlayerisinObj = true;
            fUI = Instantiate(fuiPrefab, this.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            PlayerisinObj = false;
            Destroy(fUI);
        }
    }
}
