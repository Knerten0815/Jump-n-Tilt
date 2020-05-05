using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    //Author: Marvin Winkler

    //The values can be changed for every item indivdually in the editor. The value set in the editor will override the public variables in this script!

    public float minGroundNormalY = 0.9f;   //Considered grounded and dampening accours. 0 vertical, 1 horizontal
    public float minJumpNormalY = 0.3f;     //Maximum incline for jumps. 0 vertical, 1 horizontal
    public float gravityModifier = 1f;      //Gravity multiplication factor
    public float dampening = 0.85f;         //Accours while grounded
    public float maxSpeed = 40;             //Maximum movement speed

    protected bool grounded;                //Dampening accours
    protected bool jumpable;                //true if surface is jumpable

    protected Vector2 groundNormal;
    protected Vector2 velocity;
    protected Rigidbody2D rb2d;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitbuffer = new RaycastHit2D[16];

    protected const float minMoveDistance = 0.00001f;   //Movement less than this gets ignored
    protected const float shellRadius = 0.005f;         //Prevents objects from falling thru colliders if they have to hight velocity
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //Used for collision detection
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }
    
    //Applied velocity gets updated e.g. player movement
    void Update()
    {
        ComputeVelocity();
    }
    protected virtual void ComputeVelocity()
    {

    }

    //Forces get applied
    //Author: Marvin Winkler
    private void FixedUpdate()
    {
        Vector2 deltaPosition;

        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        deltaPosition = velocity * Time.deltaTime;

        grounded = false;
        jumpable = false;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);

        if (grounded)
        {
            velocity = velocity * dampening;
        }

        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
    }

    //Applies movement, updates position
    //Author: Marvin Winkler
    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitbuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitbuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                //Checks if ground is jumpable
                if (currentNormal.y > minJumpNormalY)
                {
                    jumpable = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                    }
                }
                //Checks if object is grounded
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;    //Prevents sliding
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);

                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }
                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        rb2d.position = rb2d.position + move.normalized * distance;
    }
}
