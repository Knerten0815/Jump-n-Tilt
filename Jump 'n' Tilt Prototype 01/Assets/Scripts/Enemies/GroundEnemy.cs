//Author: Kevin Zielke

using UnityEngine;
using LevelControlls;
using System;

public class GroundEnemy : Character
{
    [SerializeField] float startDirection = 1;      //The direction the ground enemy will start to walk in. -1 = left, 1 = right
    [SerializeField] int wallCheckPrecision = 5;        //read isWallAhead() comment for more information. 0 turns wall checks off.
    [SerializeField] LayerMask whatIsGround, whatIsWall;

    public float direction;
    private float wallCheckDistance = 0.1f;
    private float groundCheckDistance = 0.1f;
    private BoxCollider2D bc2d;
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
        whatIsGround = LayerMask.GetMask("Level");      //needs to be changed to Ground later
        whatIsWall = LayerMask.GetMask("Level");        //needs to be changed to Wall later
        bc2d = GetComponent<BoxCollider2D>();
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
            offset.x += bc2d.bounds.extents.x;
        else
            offset.x -= bc2d.bounds.extents.x;

        offset.y -= bc2d.bounds.extents.y;

        if(!slopesAreWalls)
            offset.y += bc2d.bounds.size.y / (wallCheckPrecision);
        

        for (int i = 0; i < wallCheckPrecision; i++)
        {
            //Debug.DrawRay(offset, Vector2.right * direction * wallCheckDistance);
            wallAhead = Physics2D.Raycast(offset, Vector2.right * direction, wallCheckDistance, whatIsWall);
            if (wallAhead.collider)
                return true;
            offset.y += bc2d.bounds.size.y / (wallCheckPrecision);
        }

        return false;
    }

    /// <summary>
    /// Returns true if there is ground in front of the GroundEnemy. Returns false it approaches a chasm.
    /// </summary>
    /// <param name="slopesAreGround">if true, downward slopes are detected as ground. Otherwise like a chasm.</param>
    public bool isGroundAhead(bool slopesAreGround)
    {
        Vector2 offset;

        if (isFacingRight)
            offset = new Vector2(transform.position.x + bc2d.bounds.extents.x, transform.position.y - bc2d.bounds.extents.y);
        else
            offset = transform.position - bc2d.bounds.extents;

        RaycastHit2D groundAhead;

        if (slopesAreGround)
            groundAhead = Physics2D.Raycast(offset, Vector2.down, bc2d.bounds.size.x, whatIsGround);
        else
            groundAhead = Physics2D.Raycast(offset, Vector2.down, groundCheckDistance, whatIsGround);

        return groundAhead.collider;
    }

    /// <summary>
    /// Returns a Vector2 pointing to the transform of the player.
    /// </summary>
    public Vector2 playerDirection()
    {
        return player.transform.position - transform.position;
    }
}