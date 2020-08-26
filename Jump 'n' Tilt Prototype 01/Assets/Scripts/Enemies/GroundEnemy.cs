//Author: Kevin Zielke

using UnityEngine;
using LevelControlls;
using System;

public class GroundEnemy : Enemy
{
    [SerializeField] float startDirection = 1;      //The direction the ground enemy will start to walk in. -1 = left, 1 = right
    [SerializeField] int wallCheckPrecision = 5;        //read isWallAhead() comment for more information. 0 turns wall checks off.
    [SerializeField] LayerMask whatIsGround, whatIsWall;
    [SerializeField] int touchAttackDamage = 1;             //amount of damage, that is distributed on touching the player

    public float direction;
    private float wallCheckDistance = 0.05f;
    private float groundCheckDistance = 0.3f;
    public CapsuleCollider2D cc2d;
    private GameObject player;

    protected override void OnEnable()
    {
        base.OnEnable();
        LevelControllerNew.onWorldWasTilted += startSlide;
        LevelControllerNew.onWorldWasUntilted += stopSlide;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LevelControllerNew.onWorldWasTilted -= startSlide;
        LevelControllerNew.onWorldWasUntilted -= stopSlide;
    }

    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player");
        whatIsEnemy = LayerMask.GetMask("Player");
        whatIsGround = LayerMask.GetMask("Ground");      //needs to be changed to Ground later
        whatIsWall = LayerMask.GetMask("Wall");        //needs to be changed to Wall later
        cc2d = GetComponent<CapsuleCollider2D>();
        direction = startDirection;
        if (direction == 1)
            isFacingRight = true;
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();
    }

    private void stopSlide()
    {
        isSliding = false;
    }

    private void startSlide(float tiltDirection)
    {
        isSliding = true;
        direction = -tiltDirection;
        slideDirection = new Vector2(tiltDirection, 0);
    }


    /// <summary>
    /// Detects walls in front of the GroundEnemy. Also detects upward slopes, if slopesAreWalls is true.
    /// wallCheckPrecision determines how many checks are done at evenly distributed heights in front of the enemy.
    /// If wallCheckPrecision is set to high, slopes will be detected, even if slopesAreWalls is set to false.
    /// If it is to low, hovering obstacles might not be detected.
    /// Correct precision depends on the height of the enemys BoxCollider2D: Un-comment Debug.DrawRay() for testing.
    /// </summary>
    /// <param name="slopesAreWalls">Upward slopes are detected as walls if set true</param>
    /// <returns></returns>
    public bool IsWallAhead(bool slopesAreWalls)
    {
        RaycastHit2D wallAhead;
        Vector2 offset = transform.position;

        if(isFacingRight)
            offset.x +=cc2d.bounds.extents.x;
        else
            offset.x -= cc2d.bounds.extents.x;

        offset.y -= cc2d.bounds.extents.y;

        if(!slopesAreWalls)
            offset.y += cc2d.bounds.size.y / (wallCheckPrecision);        

        for (int i = 0; i < wallCheckPrecision; i++)
        {
            //Debug.DrawRay(offset, Vector2.right * direction * wallCheckDistance);
            wallAhead = Physics2D.Raycast(offset, Vector2.right * direction, wallCheckDistance, whatIsWall);
            if (wallAhead.collider)
                return true;
            offset.y += cc2d.bounds.size.y / (wallCheckPrecision);
        }

        return false;
    }

    /// <summary>
    /// Returns true if there is ground in front of the GroundEnemy. Returns false if it approaches a chasm.
    /// </summary>
    /// <param name="slopesAreGround">if true, downward slopes are detected as ground. Otherwise like a chasm.</param>
    public bool isGroundAhead(bool slopesAreGround)
    {
        Vector2 offsetAhead, offsetBehind;
        Vector3 slopeOffset = cc2d.bounds.extents;

        if (slopesAreGround)
        {
            slopeOffset.x = cc2d.bounds.extents.x / 2;
            groundCheckDistance = 1.1f;
        }
            

        if (isFacingRight)
        {
            offsetAhead = new Vector2(transform.position.x + slopeOffset.x, transform.position.y - slopeOffset.y);
            offsetBehind = transform.position - slopeOffset;
        }
        else
        {
            offsetAhead = transform.position - slopeOffset;
            offsetBehind = new Vector2(transform.position.x + slopeOffset.x, transform.position.y - slopeOffset.y);
        }

        RaycastHit2D groundAhead, slopeBehind;

        groundAhead = Physics2D.Raycast(offsetAhead, Vector2.down, groundCheckDistance, whatIsGround);
        //Debug.DrawRay(offsetAhead, Vector2.down * groundCheckDistance);

        slopeBehind = Physics2D.Raycast(offsetBehind, Vector2.right * -direction, 0.3f , whatIsGround);
        //Debug.DrawRay(offsetBehind, Vector2.right * -direction * groundCheckDistance);

        return groundAhead.collider || (!groundAhead.collider && slopeBehind.collider);
    }

    /// <summary>
    /// Returns a Vector2 pointing to the transform of the player.
    /// </summary>
    public Vector2 playerDirection()
    {
        return player.transform.position - transform.position;
    }

    /// <summary>
    /// distributes damgae when the player touches the GroundEnemy
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == player)
        {
            player.GetComponent<PlayerCharacter>().TakeDamage(touchAttackDamage, -playerDirection());
            //Debug.Log("Playerhealth: " + player.GetComponent<PlayerCharacter>().health);
        }
    }
}