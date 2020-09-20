using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioControlling;

// Author: Nicole Mynarek
public class KappaNotFollowing : Kappa
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

    }

    //overridden method to get the Kappa to jump only from left to right, not following the player
    protected override void Jump()
    {
        isSliding = false;
        isIdle = false;
        isJumping = true;

        if (isFacingRight)
        {
            direction = -1f;
            CharacterFacingDirection(direction);
        }
        else
        {
            direction = 1f;
            CharacterFacingDirection(direction);
        }
        AudioController.Instance.playFXSound(kappaJump);
        velocity = new Vector2(jumpDistance * direction, jumpHeight);
    }
}
