using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileScript : MonoBehaviour
{
    public float followSpeed;
    public float stopSpeed;
    public float durability;
    Coroutine follow;
    public bool isfollowing = false;
    [SerializeField] GameObject forPickup;
    bool isinSafe = false;
    public void drop(bool isonground)
    {
        if (isfollowing)
        {
            StopCoroutine(follow);
            this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            playerScript.Instance.stackedObjs.Remove(this.gameObject);
            this.transform.SetParent(null);
            forPickup.SetActive(true);
            isfollowing = false;
            if (isonground)
            {
                this.gameObject.transform.position = playerScript.Instance.gameObject.transform.position + new Vector3(1.5f, 0, 0);
            }
            else
            {
                this.gameObject.transform.position = playerScript.Instance.gameObject.transform.position + new Vector3(-1.5f, 0, 0);
            }
        }
    }
    public void UpdateSquarePosition(Transform followedSquare, bool isFollowStart)
    {
        forPickup.SetActive(false);
        isfollowing = true;
        follow = StartCoroutine(StartFollowingToLastSquarePosition(followedSquare, isFollowStart));
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    IEnumerator StartFollowingToLastSquarePosition(Transform followedSquare, bool isFollowStart)
    {

        while (isFollowStart)
        {
            yield return new WaitForEndOfFrame();
            transform.position = new Vector2((Mathf.Lerp(transform.position.x, followedSquare.position.x, Time.deltaTime * (playerScript.Instance.calculateFollowSpeed(this)))), transform.position.y);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag=="leaveArea" && isfollowing)
        {
            StopCoroutine(follow);
            this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            playerScript.Instance.stackedObjs.Remove(this.gameObject);
            this.transform.SetParent(null);
            forPickup.SetActive(true);
            isfollowing = false;
        }
        if(collision.tag=="safeArea"&&isinSafe==true)
        {
            isinSafe = false;
            gameFlowManager.Instance.changeCurrentSuccessCount(-1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "safeArea" && isinSafe == false)
        {
            Debug.Log("safe entered");
            isinSafe = true;
            gameFlowManager.Instance.changeCurrentSuccessCount(1);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision impact : " + collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude > durability)
        {
            gameFlowManager.Instance.fragileBreak();
            playerScript.Instance.stackedObjs.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        gameFlowManager.Instance.stageEndEvent.AddListener(onStageEnd);
    }
    public void onStageEnd()
    {
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
}
