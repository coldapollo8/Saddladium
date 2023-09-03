using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public GravityManager gravityManage;
    public LevelLoader loader;
    //Collision Checks
    public Transform groundCheck;
    public Transform wallCheck;

    private float horizontal;
    private bool isFacingRight = true;
    //Jump when the spacebar is held and jump variables
    public float jumpTime = 0.35f;
    private float jumpTimerCount = 0f;
    public bool isJumping = false;
    public float jumpForce = 2f;
    //Buffer before punishing players for missing a 1 frame off
    public float bufferTime = 0.3f; 
    public float bufferTimer = 0;
    private Rigidbody2D rigidbody;
    //Wall Sliding
    public float wallSlidingSpeed = 7f;
    //Wall Jumping
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(6f, 12f);

    //Animator
    private Animator animator;

    //Damping run
    public float horizontalDamping = 0.2f; 
    public float stopDamping = 0.6f;
    
    public Transform spawn;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;


  [SerializeField] private bool isWallSliding = false;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      if(gravityManage.getStart() == true){
      horizontal = Input.GetAxisRaw("Horizontal");


      rigidbody.gravityScale = gravityManage.gravity;

      if(isGround()){
        animator.SetBool("isJumping", false);
        animator.SetFloat("yVelocity", rigidbody.velocity.y);
        bufferTimer = bufferTime;
      }else{
        animator.SetBool("isJumping", true);
        bufferTimer -= Time.deltaTime;
      }

      if(Input.GetButtonDown("Jump") && bufferTimer > 0){
        isJumping = true;
        jumpTimerCount = jumpTime;
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
      }

      if(Input.GetButton("Jump") && isJumping){

        if(jumpTimerCount > 0){
            rigidbody.velocity = new Vector2(rigidbody.velocity.x,jumpForce);
            jumpTimerCount -= Time.deltaTime;
        }else{
            isJumping = false;
        }
      }
      if(Input.GetButtonUp("Jump")){
        isJumping = false;
      } 
      if (!isWallJumping)
      {
        Flip();
      }
      WallJump();


      if(isWalled() && !isGround() && horizontal != 0f){
        isWallSliding = true;
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, Mathf.Clamp(rigidbody.velocity.y, -wallSlidingSpeed, float.MaxValue));
      }else{
        isWallSliding = false;
      }

      Debug.Log(isWalled());
      }
    }
    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            float fHorizontalVelocity = rigidbody.velocity.x;
            fHorizontalVelocity += horizontal;
            if(Mathf.Abs(horizontal) < 0.01f){
                fHorizontalVelocity *= Mathf.Pow(1f-stopDamping, Time.deltaTime * 10f);
            }else{
                fHorizontalVelocity *= Mathf.Pow(1f-horizontalDamping, Time.deltaTime * 10f);
            }
            animator.SetFloat("Speed", Mathf.Abs(fHorizontalVelocity));
            rigidbody.velocity = new Vector2(fHorizontalVelocity, rigidbody.velocity.y);
        }
    }
    private bool isWalled(){
      return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private bool isGround(){
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rigidbody.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void OnTriggerEnter2D(Collider2D col){
      if(col.tag == "Death"){
         transform.position = spawn.position;
         rigidbody.velocity = new Vector2(0,0);
      }
    }
    private void OnCollisionEnter2D(Collision2D coll){
      if(coll.gameObject.tag == "End"){
        loader.LoadNextLevel();
        gravityManage.stopTimer();
      }
    }
}
