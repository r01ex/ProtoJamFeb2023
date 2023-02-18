using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    Rigidbody2D rbody;
    PolygonCollider2D collider2d;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask eggLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float speed;

    [SerializeField] GameObject eggPrefab;

    public List<GameObject> stackedObjs = new List<GameObject>();
    float stackHeight = 0;

    
    [SerializeField] float runMaxSpeed;
    [SerializeField]  float runAccelAmount;
    [SerializeField]  float runDeccelAmount;
    int direction;
    bool isFacingRight = true;

    bool longJump;
    float defaultgrav;
    [SerializeField] float gravityMult;
    [SerializeField] float jumpPower;
    float jumpedtime;

    public static playerScript Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        collider2d = this.GetComponent<PolygonCollider2D>();
        defaultgrav = rbody.gravityScale;
    }
    public bool isOnGround()
    {
        return collider2d.IsTouchingLayers(groundLayer);
    }
    void Update()
    {
        
        Debug.Log(collider2d.IsTouchingLayers(groundLayer));
        if (Input.GetKeyDown(KeyCode.W) && (isOnGround()))
        {
            jumpedtime = Time.time;
            rbody.velocity += new Vector2(0, 7);
            longJump = true;
        }
        else if (Input.GetKey(KeyCode.W) && (rbody.velocity.y > 0) && longJump == true)
        {
            rbody.AddForce(new Vector2(0, Time.deltaTime * jumpPower));
        }
        else if(!Input.GetKey(KeyCode.W) || (longJump == true && Time.time - jumpedtime>=0.2f))
        {
            longJump = false;
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


        if(Input.GetKeyDown(KeyCode.Q))
        {
            stackedObjs[stackedObjs.Count-1].GetComponent<FragileScript>().drop(isOnGround());
        }


        if (rbody.velocity.y<0)
        {
           rbody.gravityScale = defaultgrav * gravityMult;
        }
        else
        {
            rbody.gravityScale = defaultgrav;
        }

    }
    private void FixedUpdate()
    {
        Run();     
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
        //Convert to a vector and apply to rigidbody
        rbody.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    private void Turn(bool isMovingRight)
    {
        if (isMovingRight != isFacingRight)
        {
            //stores scale and flips the player along the x axis, 
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            isFacingRight = !isFacingRight;
        }
    }
    public void newObjectSpawn()
    {
        Debug.Log("new obj2 called");
        GameObject newegg;
        if (stackedObjs.Count==0)
        {
            newegg = Instantiate(eggPrefab);
            newegg.transform.position += this.transform.position + new Vector3(0, 1, 0);
            newegg.gameObject.GetComponent<FragileScript>().UpdateSquarePosition(transform, true);
        }
        else
        {
            newegg = Instantiate(eggPrefab);
            newegg.transform.position = stackedObjs[stackedObjs.Count - 1].transform.localPosition + Vector3.up;
            newegg.gameObject.GetComponent<FragileScript>().UpdateSquarePosition(stackedObjs[stackedObjs.Count - 1].transform, true);
        }
        stackedObjs.Add(newegg);
    }
}
