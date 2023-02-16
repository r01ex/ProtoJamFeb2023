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

    List<GameObject> stackedObjs = new List<GameObject>();
    float stackHeight = 0;

    
    [SerializeField] float runMaxSpeed;
    [SerializeField]  float runAccelAmount;
    [SerializeField]  float runDeccelAmount;
    int direction;
    bool isFacingRight = true;

    bool longJump;
    float defaultgrav;
    [SerializeField] float gravityMult;

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

    void Update()
    {
        
        Debug.Log(collider2d.IsTouchingLayers(groundLayer));
        if (Input.GetKeyDown(KeyCode.W) && (collider2d.IsTouchingLayers(groundLayer)))
        {
            rbody.velocity += new Vector2(0, 7);
            longJump = true;
        }
        else if (Input.GetKey(KeyCode.W) && (rbody.velocity.y > 0) && longJump == true)
        {
            rbody.velocity += new Vector2(0, 2);
            longJump = false;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            direction = -1;
            //transform.localScale = Vector3.Scale(this.transform.localScale, new Vector3(direction, 1, 1));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = 1;
            //transform.localScale = Vector3.Scale(this.transform.localScale, new Vector3(direction, 1, 1));
        }
        else
        {
            direction = 0;
        }

        if (direction != 0)
        {
            Turn(direction>0);
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
 
        //Calculate the direction we want to move in and our desired velocity
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
        Debug.Log("movement : " + movement);
        //Convert to a vector and apply to rigidbody
        rbody.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    public void newObjectSpawn()
    {
        Debug.Log("new obj called");
        GameObject newegg = Instantiate(eggPrefab, this.transform);
        newegg.transform.localPosition += new Vector3(0, 1+stackHeight, 0);
        stackHeight += 0.4f;
        newegg.transform.SetParent(null);
        stackedObjs.Add(newegg);
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
}
