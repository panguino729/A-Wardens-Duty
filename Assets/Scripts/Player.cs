﻿using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : Entity
{
    public float moveForce = 0.0f;
    public float maxSpeed = 0.0f;
    public float airSpeedMult = 0.0f;
    public float jumpForce = 0.0f;

    private bool grounded = true;

    void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
        //move the player
        Move();

        //if player is grounded check for jumping
        if (grounded)
        {
            Jump();
        }
        //else check to see when the player becomes grounded
        else
        {
            CheckGrounded();
        }




        Debug.Log(grounded);
    }

    private Vector2 GetDirection()
    {
        Vector2 direction = new Vector2(0, 0); 
        if (Input.GetKey(KeyCode.A))
        {
            direction.x--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x++;
        }
        return direction;
    }

    private void Jump()
    {
        //the player is on the ground and presses jump
        if(grounded && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)))
        {
            rigidbody.AddForce(new Vector2(0, jumpForce));
            
            grounded = false;
        }
    }

    private void CheckGrounded()
    {
        //cast a small box under the player to check if they are grounded
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .03f, Physics2D.AllLayers, 0, 0);
        grounded =  raycastHit2D.collider != null;
    }

    private void Move()
    {
        Vector2 direction = GetDirection();

        if (direction.x != 0)
        {
            //player is on the ground
            if (grounded)
            {
                //player is not moving
                if (rigidbody.velocity.x == 0)
                {
                    rigidbody.AddForce(direction * moveForce);
                }
                //the player is turning around
                else if (rigidbody.velocity.x * direction.x < 0)
                {
                    rigidbody.velocity = -rigidbody.velocity * 0.7f;
                }
                //cap the speed
                else if (math.abs(rigidbody.velocity.x) > maxSpeed)
                {
                    rigidbody.velocity = new Vector2(direction.x * maxSpeed, rigidbody.velocity.y);
                }
                //move as normal
                else
                {
                    rigidbody.AddForce(direction * moveForce);
                }
            }
            //player is in the air
            else
            {
                //player is not moving
                if (rigidbody.velocity.x == 0 && direction.x != 0)
                {
                    rigidbody.AddForce(direction * moveForce * airSpeedMult);
                }
                //cap the speed
                else if (math.abs(rigidbody.velocity.x) > maxSpeed)
                {
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x / math.abs(rigidbody.velocity.x) * maxSpeed, rigidbody.velocity.y);
                }
                //move as normal
                else
                {
                    rigidbody.AddForce(direction * moveForce * airSpeedMult);
                }
            }
        }
    }
}
