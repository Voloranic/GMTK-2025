using System.Runtime.InteropServices;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class GroundDirection : MonoBehaviour
{
    [SerializeField] LayerMask GroundLayer;
    RaycastHit2D[] search = new RaycastHit2D[4];
    float[] rayDistance = { 0, 0, 0, 0 };
    RaycastHit2D hit;
    [SerializeField] Rigidbody2D rb;

    Vector2 gravityDirection;
    Vector2 playerDown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //every object will have its own rotation so this one is useless
        Physics2D.gravity = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
        ApplyGravity();
    }
    private void ApplyGravity()
    {

        // oppesite of up
        playerDown = -transform.parent.up;

        //cast ray to the ground
        hit = Physics2D.Raycast(transform.position, playerDown, 3, GroundLayer);
        Debug.DrawRay(transform.position, playerDown * 3, Color.red);

        if (hit.collider != null)
        {
          
            // Set gravity to pull the player towards the hit normal.
            gravityDirection = -hit.normal;

            // rotate the player to match the ground normal.
            transform.parent.up = hit.normal;
        }
        else
        {
            //if you fly and have no ground
            //shoot 4 rays to find the closest ground and get the distance of each from the ground

            search[0] = Physics2D.Raycast(transform.position, transform.up, 50, GroundLayer);
            Debug.DrawRay(transform.position, transform.up * 50, Color.cyan);
            rayDistance[0] = search[0].distance;
            search[1] = Physics2D.Raycast(transform.position, -transform.up, 50, GroundLayer);
            Debug.DrawRay(transform.position, -transform.up * 50, Color.cyan);
            rayDistance[1] = search[1].distance;
            search[2] = Physics2D.Raycast(transform.position, transform.right, 50, GroundLayer);
            Debug.DrawRay(transform.position, transform.right * 50, Color.cyan);
            rayDistance[2] = search[2].distance;
            search[3] = Physics2D.Raycast(transform.position, -transform.right, 50, GroundLayer);
            Debug.DrawRay(transform.position, -transform.right * 50, Color.cyan);
            rayDistance[3] = search[3].distance;

            //find the closest one and assign it as the new ground
            int smallest = 0;
            for(int i = 0; i < search.Length; i++)
            {
                if (rayDistance[smallest] > rayDistance[i])
                {
                    smallest = i;
                }
            }

            //apply new gravity and rotation to gravityDirection
            gravityDirection = -search[smallest].normal;
            transform.parent.up = search[smallest].normal;
           



        }
        // apply the final gravity direction.
        rb.AddForce(gravityDirection * 9.8f * rb.gravityScale * rb.mass, ForceMode2D.Force);
    }
}
