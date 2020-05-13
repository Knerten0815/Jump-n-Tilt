using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : PhysicsObject
{

    //Author: Nicole Mynarek

    protected float moveDirection;              // Gets value between -1 and 1 for direction left or right if Input.GetAxisRaw is used

    // Values can be adjusted in inspector
    public float moveSpeed = 5f;                // Movement Speed
    public float jumpHeight = 16f;
    public float jumpHeightReducer = 0.5f;      // Reduces jump height, if button is pressed shortly

    private Vector2 slideDirection;

    public bool isFacingRight = true;
    public bool isSliding;

    protected override void ComputeVelocity()
    {
        CharacterFacingDirection();
        //Slide();
    }

    // Author: Nicole Mynarek
    // Method for basic horizontal movement 
    protected virtual void Movement()
    {
        if (grounded)
        {
            //Debug.Log("Player kann laufen");
            velocity = new Vector2(moveDirection * moveSpeed, velocity.y);
        }

    }

    // Author: Nicole Mynarek
    // Method for basic jump
    protected virtual void Jump()
    {
        if (jumpable)
        {
            // Gravity Modifier of PhysicsObject.class needs to be adjusted according to jumpHeight for good game feeling
            velocity = new Vector2(velocity.x, jumpHeight);
        }
    }

    // Author: Nicole Mynarek
    // Method for flipping character sprites according to moving direction
    void CharacterFacingDirection()
    {
        if (isFacingRight && moveDirection < 0)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (!isFacingRight && moveDirection > 0)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    // Author: Nicole Mynarek
    // Method for sliding. Does not work yet.
    /*protected virtual void Slide()
    {
        Vector2 normal;

        if (grounded)
        {
            if (groundNormal != new Vector2(0f, 1f))
            {
                isSliding = true;
                Debug.Log("Sliding? " + isSliding);

                for (int i = 0; i < hitBufferList.Count; i++)
                {
                    normal = hitBufferList[i].normal;
                    slideDirection = Vector2.Perpendicular(-normal);
                    Debug.Log("SlideDirection: " + slideDirection);
                }
                //velocity += slideDirection;
            }
        }
    }*/
}
