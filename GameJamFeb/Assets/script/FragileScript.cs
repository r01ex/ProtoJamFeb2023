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

    [SerializeField] int HP;
    [SerializeField] List<Sprite> breakingSprites;
    [SerializeField] Sprite boxoutside;
    private int healthPoint
    {
        get
        {
            return HP;
        }
        set
        {


            if (HP > 0 && value <= 0)
            {
                HP = value;
                gameFlowManager.Instance.fragileBreak();
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
                //forPickup.SetActive(false); //완파된 fragile 못줍게 하기
                //playerScript.Instance.stackedObjs.Remove(this.gameObject);
                //Destroy(this.gameObject);
            }         
            else if(HP>0&&value>0)
            {
                HP = value;          
            }
            else if(HP<=0)
            {
                HP = 0;
            }
            if (isfollowing)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = breakingSprites[HP];
            }
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = boxoutside;
            }
        }
    }

    public void drop(bool isonground, int facing)
    {
        if (isfollowing)
        {
            StopCoroutine(follow);
            this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            playerScript.Instance.stackedObjs.Remove(this.gameObject);
            playerScript.Instance.gameObject.GetComponent<Animator>().SetInteger("PickedUpFragiles", playerScript.Instance.stackedObjs.Count);
            this.transform.SetParent(null);
            forPickup.SetActive(true);
            isfollowing = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = boxoutside;
            if (isonground)
            {
                this.gameObject.transform.position = playerScript.Instance.gameObject.transform.position + new Vector3(1.5f*facing, 0, 0);
            }
            else
            {
                this.gameObject.transform.position = playerScript.Instance.gameObject.transform.position + new Vector3(-1.5f*facing, 0, 0);
            }
        }
    }
    public IEnumerator dropbySpike(float fragileMagnitude)
    {
        while(true)
        {
            if(playerScript.Instance.gameObject.GetComponent<Rigidbody2D>().velocity.y<=0)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        if (isfollowing)
        {
            StopCoroutine(follow);
            this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            playerScript.Instance.stackedObjs.Remove(this.gameObject);
            playerScript.Instance.gameObject.GetComponent<Animator>().SetInteger("PickedUpFragiles", playerScript.Instance.stackedObjs.Count);
            this.transform.SetParent(null);
            forPickup.SetActive(true);
            isfollowing = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = boxoutside;
            Debug.Log("spike fragile force : " + new Vector2(-2 * Mathf.Sign(playerScript.Instance.gameObject.GetComponent<Transform>().localPosition.x), 1) * fragileMagnitude);
            //this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2 * Mathf.Sign(playerScript.Instance.gameObject.GetComponent<Transform>().localPosition.x), 1) * fragileMagnitude);
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-2 * Mathf.Sign(playerScript.Instance.gameObject.GetComponent<Transform>().localPosition.x), 1) * fragileMagnitude;
        }
    }
    public void UpdateSquarePosition(Transform followedSquare, bool isFollowStart)
    {
        forPickup.SetActive(false);
        isfollowing = true;
        follow = StartCoroutine(StartFollowingToLastSquarePosition(followedSquare, isFollowStart));
        this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = breakingSprites[healthPoint];
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
            this.gameObject.GetComponent<SpriteRenderer>().sprite = boxoutside;
            StopCoroutine(follow);
            this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            playerScript.Instance.stackedObjs.Remove(this.gameObject);
            playerScript.Instance.gameObject.GetComponent<Animator>().SetInteger("PickedUpFragiles", playerScript.Instance.stackedObjs.Count);
            this.transform.SetParent(null);
            forPickup.SetActive(true);
            isfollowing = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = boxoutside;
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
        if (collision.relativeVelocity.magnitude > durability * 2)
        {
            healthPoint-=2;
        }
        else if(collision.relativeVelocity.magnitude > durability)
        {
            healthPoint--;
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
