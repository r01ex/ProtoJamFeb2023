using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class playerScript : MonoBehaviour
{
    Rigidbody2D rbody;
    Collider2D collider2d;
    [SerializeField] Collider2D legcollider;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask eggLayer;
    [SerializeField] LayerMask playerLayer;

    public List<GameObject> stackedObjs = new List<GameObject>();
    public FragilePickup LockedFragilePickup;
    
    [SerializeField] float runMaxSpeed;
    [SerializeField]  float runAccelAmount;
    [SerializeField]  float runDeccelAmount;
    int direction;
    bool isFacingRight = true;

    bool longJump;
    float defaultgrav;
    [SerializeField] float gravityMult;
    [SerializeField] float jumpPower;
    [SerializeField] float coyoteTime;
    float jumpedtime;
    float coyoteTimeCounter;

    public float ItemFollowspeedMult = 1;
    public float ItemGravMult = 1;
    public GameObject slowfallItemSprite;
    public GameObject beltItemSprite;

    public bool spikehitRecent = false;

    public static playerScript Instance { get; private set; }

    [SerializeField] GameObject leaveArea;
    [SerializeField] Collider2D frontDropPos;
    [SerializeField] Collider2D backDropPos;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        collider2d = this.GetComponent<Collider2D>();
        defaultgrav = rbody.gravityScale;
        gameFlowManager.Instance.stageEndEvent.AddListener(onStageEnd);
    }
    public bool isOnGround()
    {
        this.GetComponent<Animator>().SetBool("isOnGround", legcollider.IsTouchingLayers(groundLayer) || legcollider.IsTouchingLayers(eggLayer));
        return legcollider.IsTouchingLayers(groundLayer) || legcollider.IsTouchingLayers(eggLayer);
    }
    void Update()
    {
        if (isOnGround())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (spikehitRecent == false)
        {
            if (Input.GetKeyDown(KeyCode.W) && coyoteTimeCounter > 0)
            {
                Debug.Log("w");
                jumpedtime = Time.time;
                rbody.velocity = new Vector2(rbody.velocity.x, 7);
                longJump = true;
                coyoteTimeCounter = 0;
            }
            else if (Input.GetKey(KeyCode.W) && (rbody.velocity.y > 0) && longJump == true)
            {
                rbody.AddForce(new Vector2(0, Time.deltaTime * jumpPower));
            }
            if (Input.GetKeyUp(KeyCode.W) || (longJump == true && Time.time - jumpedtime >= 0.2f))
            {
                longJump = false;
                coyoteTimeCounter = 0;
            }
        }
        if (rbody.velocity.y < 0)
        {
            rbody.gravityScale = defaultgrav * gravityMult * ItemGravMult;
        }
        else
        {
            rbody.gravityScale = defaultgrav;
        }


        if (Input.GetKey(KeyCode.A))
        {
            direction = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = 1;
        }
        else
        {
            direction = 0;
        }
        if (direction != 0)
        {
            Turn(direction>0);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (stackedObjs.Count >= 1)
            {
                if (isOnGround() == true && this.transform.localScale.x > 0 && frontDropPos.IsTouchingLayers(groundLayer) == false)
                {
                    stackedObjs[stackedObjs.Count - 1].GetComponent<FragileScript>().drop(isOnGround(), MathF.Sign(this.transform.localScale.x));
                }
                else if (isOnGround() == true && this.transform.localScale.x < 0 && backDropPos.IsTouchingLayers(groundLayer) == false)
                {
                    stackedObjs[stackedObjs.Count - 1].GetComponent<FragileScript>().drop(isOnGround(), MathF.Sign(this.transform.localScale.x));
                }
                else if (isOnGround() == false && backDropPos.IsTouchingLayers(groundLayer) == false)
                {
                    stackedObjs[stackedObjs.Count - 1].GetComponent<FragileScript>().drop(isOnGround(), MathF.Sign(this.transform.localScale.x));
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (spikehitRecent == false)
        {
            Run();
        }
    }
    private void Run()
    {
        //Calculate the direction to move in and our desired velocity
        float targetSpeed = direction * runMaxSpeed;

        #region Calculate AccelRate
        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
        #endregion      

        #region Conserve Momentum
        if (Mathf.Abs(rbody.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rbody.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
        {          
            accelRate = 0;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - rbody.velocity.x;
        //Calculate force along x-axis to apply to player

        float movement = speedDif * accelRate;
        //Debug.Log("movement : " + movement);
        //Convert to vector and apply to rigidbody
        rbody.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    private void Turn(bool isMovingRight)
    {
        if (isMovingRight != isFacingRight && rbody.bodyType != RigidbodyType2D.Static)
        {
            //stores scale and flips the player along the x axis, 
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            isFacingRight = !isFacingRight;
        }
    }
    public void PickupFragile(GameObject fragile)
    {
        if (stackedObjs.Count==0)
        {
            fragile.transform.position = this.transform.position + new Vector3(0, 1.2f, 0);
            fragile.gameObject.GetComponent<FragileScript>().UpdateSquarePosition(transform, true);
        }
        else
        {
            fragile.transform.position = stackedObjs[stackedObjs.Count - 1].transform.localPosition + Vector3.up;
            fragile.gameObject.GetComponent<FragileScript>().UpdateSquarePosition(stackedObjs[stackedObjs.Count - 1].transform, true);
        }
        stackedObjs.Add(fragile);

        changeLeaveArea(1);

        this.gameObject.GetComponent<Animator>().SetInteger("PickedUpFragiles", stackedObjs.Count);
    }
    public void changeLeaveArea(int mult)
    {
        leaveArea.GetComponent<SpriteRenderer>().size += new Vector2(0, 1.4f * mult);
        leaveArea.GetComponent<BoxCollider2D>().offset = new Vector2(0, leaveArea.GetComponent<BoxCollider2D>().offset.y + 0.7f * mult);
        leaveArea.GetComponent<BoxCollider2D>().size = new Vector2(2.15f, leaveArea.GetComponent<BoxCollider2D>().size.y + 1.4f * mult);
    }
    public float calculateFollowSpeed(FragileScript fragile)
    {
        float normal = (Mathf.Abs(rbody.velocity.x) / runMaxSpeed);
        int constant = 10;
        Debug.Log("normal : " + Mathf.Round(normal*1000)/1000);

        return ItemFollowspeedMult*(((fragile.followSpeed - fragile.stopSpeed)/2) * (float)(Math.Tanh(constant * (normal - 0.5f))) + ((fragile.followSpeed + fragile.stopSpeed)/2)); //시그모이드 따라서 fs비율변화
    }

    public void onStageEnd()
    {
        rbody.bodyType = RigidbodyType2D.Static;
    }
   
}
