//Author: Kevin Zielke

using UnityEngine;
using LevelControlls;
using System.Collections;

/// <summary>
/// Base class for all ground enemies (Oni, Kappa, HumanEnemy, Kitsune).
/// </summary>

public class GroundEnemy : Enemy
{
    [SerializeField] float startDirection = 1;                  //The direction the ground enemy will start to walk in upon awake. -1 = left, 1 = right
    [SerializeField] int wallCheckPrecision = 5;                //read isWallAhead() comment for more information. 0 turns wall checks off.
    [SerializeField] LayerMask whatIsGround, whatIsWall;        //information about the level structure. Used for AI
    [SerializeField] public int touchAttackDamage = 1;          //amount of damage, that is distributed on touching the player
    [SerializeField] float attackCooldownTime;                  //time between attacks

    public bool hasAttacked = false;                            //has this enemy just attacked?
    public float direction;                                     //movement direction of the ground enemy
    public CapsuleCollider2D cc2d;                              //the collider of the ground enemy
    private GameObject attackCircle;
    private Coroutine coolroutine;                              //takes the award for greatest variable name of all time and is used for cooldowns
    public float wallCheckDistance = 0.05f;                     //distance for wall checks
    public float groundCheckDistance = 2.5f;                    //distance for ground checks. Needs correct adjustment, especially for slopes.

    //subscribe to events
    protected override void OnEnable()
    {
        base.OnEnable();
        LevelControllerNew.onWorldWasTilted += startSlide;
        LevelControllerNew.onWorldWasUntilted += stopSlide;
    }

    //unsubscribe from events
    protected override void OnDisable()
    {
        base.OnDisable();
        LevelControllerNew.onWorldWasTilted -= startSlide;
        LevelControllerNew.onWorldWasUntilted -= stopSlide;
    }

    //initializing variables
    protected override void Start()
    {
        base.Start();        
        whatIsEnemy = LayerMask.GetMask("Player");
        whatIsGround = LayerMask.GetMask("Ground") | LayerMask.GetMask("Platform");
        whatIsWall = LayerMask.GetMask("Wall") | LayerMask.GetMask("Platform") | LayerMask.GetMask("StaticObstacle");

        attackCircle = new GameObject();
        attackCircle.transform.SetParent(transform);
        attackCircle.transform.localPosition = Vector3.zero;
        attackPos = attackCircle.transform;

        cc2d = GetComponent<CapsuleCollider2D>();
        direction = startDirection;
        if (direction == 1)
            isFacingRight = true;
    }

    protected override void Update()
    {
        base.Update();

        //check if the ground enemy touches the player and distribute damage if so
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, whatIsEnemy);

        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 dmgDirection = gameObject.transform.localPosition - enemies[i].GetComponent<Transform>().localPosition;
            Vector2 dmgDirection2D = new Vector2(dmgDirection.x, dmgDirection.y);
            dmgDirection2D.Normalize();
            groundEnemyAttack(enemies[i], dmgDirection2D);
        }
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

    //Author: Kevin Zielke
    /// <summary>
    /// Detects walls in front of the GroundEnemy.
    /// wallCheckPrecision determines how many checks are done at evenly distributed heights in front of the enemy.
    /// If it is to low, hovering obstacles such as platforms might not be detected.
    /// Correct precision depends on the height of the BoxCollider2D: Un-comment Debug.DrawRay() inside the for-loop for testing.
    /// </summary>
    public bool IsWallAhead()
    {
        RaycastHit2D wallAhead;
        Vector2 offset = transform.position;

        //set the right offset, dependent on the direction the enemy is facing
        if (isFacingRight)
            offset.x += cc2d.bounds.extents.x;
        else
            offset.x -= cc2d.bounds.extents.x;

        offset.y -= cc2d.bounds.extents.y;
        offset.y += cc2d.bounds.size.y / (wallCheckPrecision);

        //check for walls on multiple height levels in front of the enemy
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

    //Author: Kevin Zielke
    /// <summary>
    /// Returns true if there is ground in front of the GroundEnemy. Returns false if it approaches a chasm.
    /// Un-comment the Debug.DrawRay calls for testing.
    /// </summary>
    public bool isGroundAhead()
    {
        Vector2 offsetBehind;
        Vector2 offsetAhead = transform.position;
        Vector3 slopeOffset = cc2d.bounds.extents;

        //set the right offset, dependent on the direction the enemy is facing
        if (isFacingRight)
        {
            offsetAhead.x += cc2d.bounds.extents.x;
            offsetBehind = transform.position - slopeOffset;
        }
        else
        {
            offsetAhead.x -= cc2d.bounds.extents.x;
            offsetBehind = new Vector2(transform.position.x + slopeOffset.x, transform.position.y - slopeOffset.y);
        }

        RaycastHit2D groundAhead, slopeBehind;

        groundAhead = Physics2D.Raycast(offsetAhead, Vector2.down, groundCheckDistance, whatIsGround);
        //Debug.DrawRay(offsetAhead, Vector2.down * groundCheckDistance);

        slopeBehind = Physics2D.Raycast(offsetBehind, Vector2.right * -direction, 0.3f , whatIsGround);
        //Debug.DrawRay(offsetBehind, Vector2.right * -direction * groundCheckDistance);

        return groundAhead.collider || (!groundAhead.collider && slopeBehind.collider);
    }

    //Author: Kevin Zielke
    /// <summary>
    /// This method is called, when the ground enemy touches the player.
    /// It will hurt the player, apply a knockback to the enemy and start the attack cooldown
    /// </summary>
    /// <param name="enemy">The collider of the enemy. In this case most definitly the player</param>
    /// <param name="dmgDirection2D">The direction from which the collision came</param>
    public virtual void groundEnemyAttack(Collider2D enemy, Vector2 dmgDirection2D)
    {
        if (!hasAttacked && !isSliding && getPlayerScript().health > 0)
        {
            enemy.GetComponent<PlayerCharacter>().TakeDamage(1, -dmgDirection2D);
            hasAttacked = true;
            velocity = new Vector2(-dmgDirection2D.x * knockback, knockup);
            coolroutine = StartCoroutine(attackCooldown(attackCooldownTime));
        }
    }

    //Author: Kevin Zielke
    /// <summary>
    /// Waits for the coolDownTime and the resets the cooldown
    /// </summary>
    /// <param name="coolDownTime">the amount of time between attacks</param>
    IEnumerator attackCooldown(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        hasAttacked = false;
        StopCoroutine(coolroutine);
    }
}