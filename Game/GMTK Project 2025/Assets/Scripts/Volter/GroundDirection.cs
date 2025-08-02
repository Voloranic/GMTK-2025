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

    private Vector2 targetUp;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private float searchRayDistance = 25f;
    [SerializeField] private float invertedGravityDivider = 6f;

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

        if (hit.collider != null && hit.collider.gameObject.layer != LayerMask.NameToLayer("NoGravity"))
        {
            grounded = true;
            // Set gravity to pull the player towards the hit normal.
            gravityDirection = -hit.normal;

            // rotate the player to match the ground normal.
            //transform.parent.up = hit.normal;
            targetUp = hit.normal;
        }
        else
        {
            grounded = false;
            //if you fly and have no ground
            //shoot 4 rays to find the closest ground and get the distance of each from the ground
            
            RaycastHit2D[] search = new RaycastHit2D[8];
            float[] rayDistance = new float[8];

            search[0] = Physics2D.Raycast(transform.position, transform.up, searchRayDistance, groundLayer);
            Debug.DrawRay(transform.position, transform.up * searchRayDistance, Color.cyan);

            search[1] = Physics2D.Raycast(transform.position, -transform.up, searchRayDistance, groundLayer);
            Debug.DrawRay(transform.position, -transform.up * searchRayDistance, Color.cyan);

            search[2] = Physics2D.Raycast(transform.position, transform.right, searchRayDistance, groundLayer);
            Debug.DrawRay(transform.position, transform.right * searchRayDistance, Color.cyan);

            search[3] = Physics2D.Raycast(transform.position, -transform.right, searchRayDistance, groundLayer);
            Debug.DrawRay(transform.position, -transform.right * searchRayDistance, Color.cyan);

            //Diagonal rays
            search[4] = Physics2D.Raycast(transform.position, (transform.up + transform.right).normalized, searchRayDistance, groundLayer);
            Debug.DrawRay(transform.position, (transform.up + transform.right).normalized * searchRayDistance, Color.cyan);

            search[5] = Physics2D.Raycast(transform.position, (-transform.up + transform.right).normalized, searchRayDistance, groundLayer);
            Debug.DrawRay(transform.position, (-transform.up + transform.right).normalized * searchRayDistance, Color.cyan);

            search[6] = Physics2D.Raycast(transform.position, (transform.up - transform.right).normalized, searchRayDistance, groundLayer);
            Debug.DrawRay(transform.position, (transform.up - transform.right).normalized * searchRayDistance, Color.cyan);

            search[7] = Physics2D.Raycast(transform.position, (-transform.up - transform.right).normalized, searchRayDistance, groundLayer);
            Debug.DrawRay(transform.position, (-transform.up - transform.right).normalized * searchRayDistance, Color.cyan);


            for (int i = 0; i < search.Length; i++)
            {
                if (search[i])
                {
                    if (search[i].collider.gameObject.layer == LayerMask.NameToLayer("NoGravity"))
                    {
                        rayDistance[i] = -search[i].distance;
                    }
                    else
                    {
                        rayDistance[i] = search[i].distance;
                    }
                }
                else
                {
                    rayDistance[i] = float.MaxValue;
                }
            }

            //find the closest one and assign it as the new ground
            int smallestDistanceIndex = 0;
            for(int i = 0; i < search.Length; i++)
            {
                float smallestDistance = rayDistance[smallestDistanceIndex];
                float currentDistance = rayDistance[i];

                if (currentDistance < smallestDistance && currentDistance > 0)
                {
                    smallestDistanceIndex = i;
                }
                else if (currentDistance < 0)
                {
                    if (Mathf.Abs(currentDistance) < smallestDistance)
                    {
                        smallestDistanceIndex = i;
                    }
                }
            }

            float _smallestDistance = rayDistance[smallestDistanceIndex];

            //Didnt find any ground
            if (_smallestDistance == float.MaxValue)
            {
                gravityDirection = Vector2.down;
                //transform.parent.up = search[smallest].normal;
                targetUp = Vector2.up;
            }
            //Invert Gravity
            else if (_smallestDistance < 0)
            {
                //apply new gravity and rotation to gravityDirection
                gravityDirection = search[smallestDistanceIndex].normal / invertedGravityDivider;
                //transform.parent.up = search[smallest].normal;
                targetUp = search[smallestDistanceIndex].normal;
            }
            else
            {
                //apply new gravity and rotation to gravityDirection
                gravityDirection = -search[smallestDistanceIndex].normal;
                //transform.parent.up = search[smallest].normal;
                targetUp = search[smallestDistanceIndex].normal;
            }

            print(rayDistance[0] +"," + rayDistance[1] + "," + rayDistance[2] + "," + rayDistance[3]);
            print(smallestDistanceIndex);
        }

        Quaternion currentRotation = transform.parent.rotation;
        Quaternion targetRotation = Quaternion.FromToRotation(transform.parent.up, targetUp) * currentRotation;
        transform.parent.rotation = Quaternion.Lerp(currentRotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // apply the final gravity direction.
        rb.AddForce(gravityScale * mass * gravityDirection, ForceMode2D.Force);

        transform.root.eulerAngles = new(0f, 0f, transform.eulerAngles.z);
    }

    public bool IsGrounded()
    {
        return grounded;
    }
}
