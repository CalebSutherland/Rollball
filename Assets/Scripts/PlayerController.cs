using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;

    private float movementX;
    private float movementY;

    public float speed = 0;

    public TextMeshProUGUI countText;

    public GameObject winTextObject;

    public GameObject player;
    private Collider playerCollider;
    public Vector3 jump;
    public float jumpForce = 3.0f;
    public float distToGround;
    public bool canDJump;
    

    // Start is called before the first frame update
    void Start() 
    {
        rb = GetComponent <Rigidbody>();
        playerCollider = GetComponent <Collider>();
        count = 0;
        jump = new Vector3(0.0f, 3.0f, 0.0f);
        distToGround = playerCollider.bounds.extents.y;
        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove (InputValue movementValue) 
    {
        Vector2 movementVector = movementValue.Get<Vector2>(); 
        
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

    void SetCountText() 
    {
        countText.text =  "Count: " + count.ToString();
        if (count >= 12)
        {
           winTextObject.SetActive(true);
        }
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    private void FixedUpdate() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                canDJump = true;
            } 
            else
            {
                if (canDJump)
                {
                    canDJump = false;
                    rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                }
            }
           
        }

        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
        rb.AddForce(movement * speed); 
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
    }
}
