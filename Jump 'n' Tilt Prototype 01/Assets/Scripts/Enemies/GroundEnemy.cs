//Author: Kevin Zielke

using UnityEngine;
using LevelControlls;
using System;
using System.Collections;

public class GroundEnemy : Enemy
{
    [SerializeField] float startDirection = 1;      //The direction the ground enemy will start to walk in. -1 = left, 1 = right
    [SerializeField] int wallCheckPrecision = 5;        //read isWallAhead() comment for more information. 0 turns wall checks off.
    [SerializeField] LayerMask whatIsGround, whatIsWall;
    [SerializeField] public int touchAttackDamage = 1;             //amount of damage, that is distributed on touching the player
    [SerializeField] float attackCooldownTime;

    public bool hasAttacked = false;
    public float direction;
    public CapsuleCollider2D cc2d;
    private GameObject attackCircle;
    private Coroutine coolroutine;
    private float wallCheckDistance = 0.05f;
    private float groundCheckDistance = 0.3f;


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
        whatIsEnemy = LayerMask.GetMask("Player");
        whatIsGround = LayerMask.GetMask("Platform") | LayerMask.GetMask("Ground");
        whatIsWall = LayerMask.GetMask("Wall");

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
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, whatIsEnemy);

        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 dmgDirection = gameObject.transform.localPosition - enemies[i].GetComponent<Transform>().localPosition;
            Vector2 dmgDirection2D = new Vector2(dmgDirection.x, dmgDirection.y);
            dmgDirection2D.Normalize();
            groundEnemyAttack(enemies[i], dmgDirection2D);
            Debug.Log("Player im AttackRadius!");
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


    /// <summary>
    /// Detects walls in front of the GroundEnemy.
    /// wallCheckPrecision determines how many checks are done at evenly distributed heights in front of the enemy.
    /// If wallCheckPrecision is set to high, slopes will be detected, too.
    /// If it is to low, hovering obstacles might not be detected.
    /// Correct precision depends on the height of the BoxCollider2D: Un-comment Debug.DrawRay() inside the for-loop for testing.
    /// </summary>
    /// <param name="slopesAreWalls">Upward slopes are detected as walls if set true</param>
    /// <returns></returns>
    public bool IsWallAhead()
    {
        RaycastHit2D wallAhead;
        Vector2 offset = transform.position;

        if (isFacingRight)
            offset.x += cc2d.bounds.extents.x;
        else
            offset.x -= cc2d.bounds.extents.x;

        offset.y -= cc2d.bounds.extents.y;
        offset.y += cc2d.bounds.size.y / (wallCheckPrecision);

        for (int i = 0; i < wallCheckPrecision; i++)
        {
            Debug.DrawRay(offset, Vector2.right * direction * wallCheckDistance);
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
    public bool isGroundAhead()
    {
        Vector2 offsetAhead, offsetBehind;
        Vector3 slopeOffset = cc2d.bounds.extents;

        slopeOffset.x = cc2d.bounds.extents.x / 2;
        groundCheckDistance = 1.1f;

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
        Debug.DrawRay(offsetAhead, Vector2.down * groundCheckDistance);

        slopeBehind = Physics2D.Raycast(offsetBehind, Vector2.right * -direction, 0.3f , whatIsGround);
        Debug.DrawRay(offsetBehind, Vector2.right * -direction * groundCheckDistance);

        return groundAhead.collider || (!groundAhead.collider && slopeBehind.collider);
    }

    public void groundEnemyAttack(Collider2D enemy, Vector2 dmgDirection2D)
    {
        if (hasAttacked == false)
        {
            enemy.GetComponent<PlayerCharacter>().TakeDamage(1, dmgDirection2D);
            //Debug.Log("GroundEnemy macht Schaden!!!!!!!!!!!!!!!!!!!1");
            hasAttacked = true;
            coolroutine = StartCoroutine(attackCooldown(attackCooldownTime));
        }
    }

    IEnumerator attackCooldown(float coolDownTime)
    {
        //Debug.Log(coolDownTime + " seconds Cooldown!");
        yield return new WaitForSeconds(coolDownTime);
        //Debug.Log("Cooldown end!");
        hasAttacked = false;
        StopCoroutine(coolroutine);
    }
}