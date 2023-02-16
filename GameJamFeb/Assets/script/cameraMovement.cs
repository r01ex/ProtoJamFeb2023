using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    GameObject playerObject;
    [SerializeField] float xmin;
    [SerializeField] float ymin;
    [SerializeField] float xmax;
    [SerializeField] float ymax;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = playerScript.Instance.gameObject;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        float x = Mathf.Clamp(playerObject.transform.position.x, xmin, xmax);
        float y = Mathf.Clamp(playerObject.transform.position.y, ymin, ymax);
        this.transform.position = new Vector3(x, y, this.transform.position.z);
    }
}
