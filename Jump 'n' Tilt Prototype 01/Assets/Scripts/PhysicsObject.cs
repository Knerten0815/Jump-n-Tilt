using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeControlls;
using LevelControlls;
public class PhysicsObject : MonoBehaviour
{
    //Author: Marvin Winkler

    //The values can be changed for every item indivdually in the editor. The value set in the editor will override the public variables in this script!

    public float minGroundNormalY;   //Considered grounded and dampening accours. 0 vertical, 1 horizontal
    public float minJumpNormalY;     //Maximum incline for jumps. 0 vertical, 1 horizontal
    public float gravityModifier;      //Gravity multiplication factor: Only change it for kinematic objects. Gravity of dynamic objects should be changed in Rigitbody 2D.
    public float dampening;         //Accours while grounded
    public float maxSpeed;             //Maximum movement speed

    protected bool grounded;                //Dampening accours
    protected bool jumpable;                //true if surface is jumpable

    protected Vector2 groundNormal;
    protected Vector2 velocity;
    protected Rigidbody2D rb2d;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitbuffer = new RaycastHit2D[16];

    protected const float minMoveDistance = 0.001f;     //Movement less than this gets ignored
    protected const float shellRadius = 0.02f;             //Prevents objects from falling thru colliders if they have to hight velocity
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected TimeController timeController;
    protected LevelControllerNew levelController;

    protected virtual void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnDisable()
    {

    }

    //Author: Marvin Winkler
    protected virtual void Start()
    {
        //Used for collision detection
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));

        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        levelController = GameObject.Find("LevelController").GetComponent<LevelControllerNew>(); // needed for the arrows

    }

    //Applied velocity gets updated e.g. player movement
    protected virtual void Update()
    {

    }
    protected virtual void ComputeVelocity()
    {

    }

    //Forces get applied
    //Author: Marvin Winkler
    private void FixedUpdate()
    {
        ComputeVelocity();

        float deltaTime = timeController.getSpeedAdjustedDeltaTime();
        Vector2 deltaPosition;

        velocity += gravityModifier * Physics2D.gravity * deltaTime;
        deltaPosition = velocity * deltaTime;

        grounded = false;
        jumpable = false;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);

        calculateDampening();

        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
        updateAnimations();
    }

    protected virtual void calculateDampening()
    {
        if (grounded)
        {
            if (timeController.getTimeSpeed() < 1)
            {
                velocity *= (dampening + (1 - dampening) * timeController.getTimeSpeed());
            }
            else
            {
                velocity *= dampening;
            }
        }
    }

    //Used for animations
    //Author Marvin Winkler
    protected virtual void updateAnimations()
    {

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

                //Debug.Log(currentNormal);
                //Checks if ground is jumpable
                //if (currentNormal.y > minJumpNormalY)
                //{
                //    jumpable = true;
                //    if (yMovement)
                //    {
                //        groundNormal = currentNormal;
                //    }
                //}
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

                //Debug.Log("NORMALE " + currentNormal);

                float projection = Vector2.Dot(velocity, currentNormal);

                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }
                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        rb2d.position += move.normalized * distance;
    }

    //Author: Kevin Zielke
    public Vector2 getVelocity()
    {
        return velocity;
    }
}
