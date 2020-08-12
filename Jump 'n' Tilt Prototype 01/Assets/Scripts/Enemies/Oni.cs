//Author: Kevin Zielke

using UnityEngine;

public class Oni : Character
{
    [SerializeField] float startDirection = 1;      //The direction the ground enemy will start to walk in. 0 = left, 1 = right
    [SerializeField] int wallCheckPrecision = 3;        //read isWallAhead() comment for more information. 0 turns wall checks off.
    private float direction;
    public bool movingRight;
    private float wallCheckDistance = 0.1f;
    private float groundCheckDistance = 0.1f;
    public LayerMask whatIsGround, whatIsWall;
    private BoxCollider2D bc2d;


    protected override void OnEnable()
    {
        base.OnEnable();
        whatIsEnemy = LayerMask.GetMask("Player");
        whatIsGround = LayerMask.GetMask("Level");      //needs to be changed to Ground later
        whatIsWall = LayerMask.GetMask("Level");         //needs to be changed to Wall later
        bc2d = GetComponent<BoxCollider2D>();
        direction = startDirection;
        if (direction == 1)
            movingRight = true;
    }

    protected override void Start()
    {
        base.Start();        
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();        

        if (isGroundAhead() == false || IsWallAhead() == true || isSlopeAhead() == true)
        {
            if (movingRight == true)
            {
                direction = -1;
                movingRight = false;
            }
            else
            {
                direction = 1;
                movingRight = true;
            }
        }
        
        Movement(direction);
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
    }

    ///Detects walls in front of the ground enemy. Does not detect slopes. Use IsSlopeAhead() instead.
    ///wallCheckPrecision determines how many checks are done at evenly distributed heights in front of the enemy
    ///If the precision is to high slopes will be detected, too. If it is to low, hovering obstacles might not be
    ///detected.
    ///Correct precision depends on the height of the enemys BoxCollider2D. Un-comment Debug.DrawRay() for testing.
    private bool IsWallAhead()
    {
        RaycastHit2D wallAhead;
        Vector2 offset = transform.position;

        if(movingRight)
            offset.x += bc2d.bounds.extents.x;
        else
            offset.x -= bc2d.bounds.extents.x;

        offset.y -= bc2d.bounds.extents.y;
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

    private bool isSlopeAhead()
    {
        RaycastHit2D slopeAhead;
        Vector2 offset = transform.position;

        if (movingRight)
            offset.x += bc2d.bounds.extents.x;
        else
            offset.x -= bc2d.bounds.extents.x;

        offset.y -= bc2d.bounds.extents.y;

        slopeAhead = Physics2D.Raycast(offset, Vector2.right * direction, 0.1f, whatIsWall);
        Debug.DrawRay(offset, Vector2.right * direction * wallCheckDistance);

        return slopeAhead.collider;
    }

    ///returns true if there is ground in front of the ground enemy
    ///returns false if the ground enemy approaches a chasm
    private bool isGroundAhead()
    {
        Vector2 offset;

        if (movingRight)
            offset = new Vector2(transform.position.x + bc2d.bounds.extents.x, transform.position.y - bc2d.bounds.extents.y);
        else
            offset = transform.position - bc2d.bounds.extents;

        //Debug.DrawRay(offset, Vector2.down * groundCheckDistance);

        RaycastHit2D groundAhead = Physics2D.Raycast(offset, Vector2.down, groundCheckDistance, whatIsGround);

        return groundAhead.collider;
    }
}