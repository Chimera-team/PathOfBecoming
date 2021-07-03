﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    public float MoveInput;
    private Rigidbody2D rb;

    public bool faceRight = true;
    public bool isGround;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;


    private int extraJump;
    public int ExtraJumpValue;

    [SerializeField] GameObject firstDiaTrigger;
    [SerializeField] float waitTimeTillStart;
    [SerializeField] Transform pixy;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJump = ExtraJumpValue;
        if(waitTimeTillStart != 0)
        {
            StartCoroutine(Wait());
        }
       
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
       
        rb.velocity = new Vector2(Joystick.axisX * speed, rb.velocity.y);

        if (faceRight == false && Joystick.axisX > 0)
        {
            Flip();           
        }
        else if (faceRight == true && Joystick.axisX < 0)
        {
            Flip();           
        }

        if (isGround == true)
        {
            extraJump = ExtraJumpValue;
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJump > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJump--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJump == 0 && isGround == true)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    public void Flip()
    {
        if(!faceRight)
            pixy.transform.localRotation = Quaternion.Euler(0, 180, 0);
        else if(faceRight)
            pixy.transform.localRotation = Quaternion.Euler(0, 0, 0);
        faceRight = !faceRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(waitTimeTillStart);
        firstDiaTrigger.SetActive(true);
    }
    public void OnRightButtonDown()
    {
        MoveInput = 1;
        
    }
    public void OnLeftButtonDown()
    {
        MoveInput = -1;
        
    }
    public void OnButtonUp()
    {
        MoveInput = 0;
    }
    public void OnJumpButton()
    {
        if (isGround)
        {
            rb.velocity = Vector2.up * jumpForce;

            //StartCoroutine(showJumpButton());
        }
            
    }
    private IEnumerator showJumpButton()
    {
        yield return new WaitForSeconds(0.2f);

    }
}
