using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioControlling;

// Author: Nicole Mynarek
public class KappaNotFollowing : Kappa
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

    }
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

        velocity = new Vector2(jumpDistance * direction, jumpHeight);
        AudioController.Instance.playFXSound(kappaJump);
    }
}
