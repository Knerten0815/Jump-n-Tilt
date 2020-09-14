using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeControlls;
using LevelControlls;
public class PhysicsObject : MonoBehaviour
{
    //Author: Marvin Winkler

    //The values can be changed for every item indivdually in the editor. The value set in the editor will override the public variables in this script!

    public float minGroundNormalY;      //Considered grounded and dampening occurs. 0 vertical, 1 horizontal
    public float minJumpNormalY;        //Maximum incline for jumps. 0 vertical, 1 horizontal
    public float gravityModifier;       //Gravity multiplication factor: Only change it for kinematic objects.
                                        //Gravity of dynamic objects should be changed in Rigitbody 2D.
    public float dampening;             //Accours while grounded, slows down objects
    public float maxSpeed;              //Maximum movement speed

    protected bool grounded;                //Dampening accours
    protected bool jumpable;                //true if surface is jumpable

    protected Vector2 groundNormal;         
    protected Vector2 velocity;
    protected Rigidbody2D rb2d;
    protected ContactFilter2D contactFilter;    //Used for surface contact
    protected RaycastHit2D[] hitbuffer = new RaycastHit2D[16]; //Used for surface contact

    protected const float minMoveDistance = 0.001f;     //Movement less than this gets ignored
    protected const float shellRadius = 0.02f;             //Prevents objects from falling thru colliders if they have to hight velocity
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);    //Used for surface
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

    protected virtual void Update()
    {
        //Currently everything is in fixed update, because slide was bound by framerate. This is the easiest fix, additional input lag is not notable.
    }

    //Applied velocity gets updated e.g. player movement
    protected virtual void ComputeVelocity()
    {
        //Used by children like fixed update method
    }

    //Forces get applied
    //Author: Marvin Winkler
    private void FixedUpdate()
    {
        //Velocity of children gets calculated and variables get updated
        ComputeVelocity();

        //Time
        float deltaTime = timeController.getSpeedAdjustedDeltaTime();

        //Gravity
        Vector2 deltaPosition;
        velocity += gravityModifier * Physics2D.gravity * deltaTime;
        deltaPosition = velocity * deltaTime;

        grounded = false;
        jumpable = false;

        //Movement
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        //Movement in X
        Movement(move, false);

        //Movement in Y
        move = Vector2.up * deltaPosition.y;

        Movement(move, true);

        //Dampening
        calculateDampening();

        //Max speed
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }

        //Animation update
        updateAnimations();
    }

    //Dampening while grounded adjusted for time speed
    //Author: Marvin Winkler
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
        //Used by children
    }

    //Applies movement, updates position
    //Author: Marvin Winkler
    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            //Surface contact
            int count = rb2d.Cast(move, contactFilter, hitbuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitbuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                //Normal calculation for surface hits
                Vector2 currentNormal = hitBufferList[i].normal;

                //Sliding?
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;    //Prevents sliding
                    }
                }

                //Slide velocity calculation based on surface normal
                float projection = Vector2.Dot(velocity, currentNormal);

                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }
                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        //Position update
        rb2d.position += move.normalized * distance;
    }

    //Author: Kevin Zielke
    public Vector2 getVelocity()
    {
        return velocity;
    }
}
