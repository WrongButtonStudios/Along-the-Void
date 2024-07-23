using System.Collections;
using UnityEngine;

public class DashScript : MonoBehaviour
{



    public float Direction = 0f;// erstellt einen privaten Float namens "Direction" auf 0
    public float DirectionVertical = 0f; // erstellt einen privaten Float f r namens "DirectionVertical" auf 0
    private PlayerController DashControll;


    public bool canDash = true;
    public bool isDashing;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    public bool notGroundDash = true;

    public Animator animator;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] public TrailRenderer tr;




    private void Start()
    {
        DashControll = GetComponent<PlayerController>();
    }


    private void FixedUpdate()
    {
        if (isDashing)
        {

            DashControll.Geschwindigkeit = DashControll.maxSpeed;

        }
    }



    private IEnumerator DashDaniel(string animationName, Vector2 dashDirection)
    {
        animator.Play(animationName);
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        SoundManager.sndMan.PlayDashSound();
        SoundManager.sndMan.PlayDashDashSound();
        rb.AddForce(dashDirection, ForceMode2D.Impulse);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        notGroundDash = false;
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (DashControll.isGrounded)
        {
            notGroundDash = true;
        }

        Direction = Input.GetAxis("Horizontal");
        DirectionVertical = Input.GetAxis("Vertical");



        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && Direction != 0f && DirectionVertical == 0f && notGroundDash ||
            Input.GetKeyDown(KeyCode.Joystick1Button5) && canDash && Direction != 0f && DirectionVertical == 0f && notGroundDash)
        {
            DashControll.Geschwindigkeit = DashControll.StandartGeschwindigkeit;
            StartCoroutine(DashDaniel("DashFbF", new Vector2(transform.localScale.x * dashingPower, 0f)));
        }



        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && DirectionVertical > 0f && Direction == 0f && notGroundDash ||
            Input.GetKey(KeyCode.Joystick1Button5) && canDash && DirectionVertical > 0f && Direction == 0f && notGroundDash)
        {
            StartCoroutine(DashDaniel("DashFbFVertical", Vector2.up * dashingPower * 0.5f));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && DirectionVertical > 0f && Direction > 0f && notGroundDash ||
            Input.GetKey(KeyCode.Joystick1Button5) && canDash && DirectionVertical > 0f && Direction > 0f && notGroundDash)
        {
            StartCoroutine(DashDaniel("DashFbFDiagonal", new Vector2(transform.localScale.x * dashingPower, transform.localScale.y * dashingPower)));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && DirectionVertical < 0f && Direction > 0f && !notGroundDash ||
            Input.GetKey(KeyCode.Joystick1Button5) && canDash && DirectionVertical < 0f && Direction > 0f && !notGroundDash)
        {
            StartCoroutine(DashDaniel("DashFbFDiagonalLeft", new Vector2(transform.localScale.x * dashingPower, transform.localScale.y * -dashingPower)));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && DirectionVertical > 0f && Direction < 0f && notGroundDash ||
            Input.GetKey(KeyCode.Joystick1Button5) && canDash && DirectionVertical > 0f && Direction < 0f && notGroundDash)
        {
            StartCoroutine(DashDaniel("DashFbFDiagonalLeft", new Vector2(transform.localScale.x * -dashingPower, transform.localScale.y * dashingPower)));
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && DirectionVertical < 0f && Direction < 0f && !notGroundDash ||
            Input.GetKey(KeyCode.Joystick1Button5) && canDash && DirectionVertical < 0f && Direction < 0f && !notGroundDash)
        {
            StartCoroutine(DashDaniel("DashFbFDiagonal", new Vector2(-transform.localScale.x * -dashingPower, transform.localScale.y * -dashingPower)));
        }
    }
}