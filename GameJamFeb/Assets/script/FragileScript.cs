using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileScript : MonoBehaviour
{
    [SerializeField] private float followSpeed;
    Coroutine follow;
    public bool isfollowing = false;
    [SerializeField] GameObject forPickup;
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
            transform.position = new Vector2((Mathf.Lerp(transform.position.x, followedSquare.position.x, followSpeed * Time.deltaTime)), transform.position.y);
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
    }
}
