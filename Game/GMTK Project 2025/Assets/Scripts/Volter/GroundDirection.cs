using System;
using System.Collections.Generic;
using UnityEngine;

public class GroundDirection : MonoBehaviour
{
    Rigidbody2D rb;
    
    [SerializeField] float mass;
    [SerializeField] float gravitationalConstant = 9.81f;

    private float gravityScale;

    [SerializeField] LayerMask groundLayer;

    private bool grounded;


    void Start()
    {
        //every object will have its own rotation so this one is useless
        Physics2D.gravity = new Vector2(0, 0);

        rb = transform.root.GetComponent<Rigidbody2D>();

        SetDefaultGravityScale();
    }

    public void SetDefaultGravityScale()
    {
        gravityScale = gravitationalConstant;
    }

    public void ChangeGravityScale(float _gravityScale)
    {
        gravityScale = _gravityScale;
    }

    private void FixedUpdate()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        
        Vector2 playerDown = -transform.root.up;

        Vector2 gravityDirection;

        //cast ray to the ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDown, 0.8f, groundLayer);
        Debug.DrawRay(transform.position, playerDown * 0.8f, Color.red);

        if (hit.collider != null)
        {
            grounded = true;
            // Set gravity to pull the player towards the hit normal.
            gravityDirection = -hit.normal;

            // rotate the player to match the ground normal.
            transform.parent.up = hit.normal;
        }
        else
        {
            grounded = false;
            //if you fly and have no ground
            //shoot 4 rays to find the closest ground and get the distance of each from the ground
            
            RaycastHit2D[] search = new RaycastHit2D[4];
            float[] rayDistance = new float[4];

            search[0] = Physics2D.Raycast(transform.position, transform.up, 50, groundLayer);
            Debug.DrawRay(transform.position, transform.up * 50, Color.cyan);
            rayDistance[0] = search[0].distance;

            search[1] = Physics2D.Raycast(transform.position, -transform.up, 50, groundLayer);
            Debug.DrawRay(transform.position, -transform.up * 50, Color.cyan);
            rayDistance[1] = search[1].distance;
            
            search[2] = Physics2D.Raycast(transform.position, transform.right, 50, groundLayer);
            Debug.DrawRay(transform.position, transform.right * 50, Color.cyan);
            rayDistance[2] = search[2].distance;
            
            search[3] = Physics2D.Raycast(transform.position, -transform.right, 50, groundLayer);
            Debug.DrawRay(transform.position, -transform.right * 50, Color.cyan);
            rayDistance[3] = search[3].distance;

            //find the closest one and assign it as the new ground
            int smallest = 0;
            for(int i = 0; i < search.Length; i++)
            {
                if (rayDistance[smallest] > rayDistance[i] && rayDistance[i] != 0)
                {
                    smallest = i;
                }
            }
            print(rayDistance[0] +"," + rayDistance[1] + "," + rayDistance[2] + "," + rayDistance[3]);
            print(smallest);

            //apply new gravity and rotation to gravityDirection
            gravityDirection = -search[smallest].normal;
            transform.parent.up = search[smallest].normal;
            
        }

        // apply the final gravity direction.
        rb.AddForce(gravityScale * mass * gravityDirection, ForceMode2D.Force);
    }

    public bool IsGrounded()
    {
        return grounded;
    }
}
