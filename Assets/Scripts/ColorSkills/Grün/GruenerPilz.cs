using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GruenerPilz : MonoBehaviour
{
    public float bounce = 20f;
    private PolygonCollider2D GreenColliderSkill;
    private SpriteRenderer GreenSkill;
    public Rigidbody2D Player;
    public Material[] material;
    public bool isGreenShining;
    public PilzDetector shine;
    public Animator animator;

    [SerializeField] PauseMenue pauseMenue;


  

    private void OnTriggerStay2D(Collider2D collision)
    {
        

        //if (collision.gameObject.tag == "ColorDetector" && !Green.greenIsActive && Green.redIsActive||
        //    collision.gameObject.tag == "ColorDetector" && Green.greenIsActive && !Green.redIsActive)
        //{

        //    isGreenShining = true;

        //}
        if (collision.gameObject.CompareTag("Player") /*&& Green.greenIsActive*/)
        {
            Debug.Log("BOING");
            Player.AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
            animator.SetBool("PlayerIsJumping", true);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "ColorDetector" && !Green.greenIsActive ||
        //  collision.gameObject.tag == "ColorDetector" && Green.greenIsActive)

        //{
        //    isGreenShining = false;
        //}
        if (collision.gameObject.CompareTag("Player") /*&& Green.greenIsActive*/)
        {
            SoundManager.sndMan.PlaySprungSound();
            animator.SetBool("PlayerIsJumping", false);
            SoundManager.sndMan.PlayShroomSound();
        }
    }



    void Start()
    {
        pauseMenue = GameObject.FindObjectOfType<PauseMenue>();
        GreenSkill = GetComponent<SpriteRenderer>();
        GreenColliderSkill = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        if (pauseMenue.GameIsPaused)
        {
            return;
        }
        if (shine.isGreenShining == true)
        {
            isGreenShining = true;
        }
        else
        {
            isGreenShining = false;
        }
        ColorManager.Instance.Green();
        ColorManager.Instance.Red();
        //Green.Blue();
        //Green.Yellow();



        if (ColorManager.Instance.redIsActive)
        {

            this.gameObject.GetComponent<SpriteRenderer>().material = material[1];
            if (GreenSkill != null)

            if (GreenColliderSkill != null)
            {
                GreenColliderSkill.enabled = false;
            }
            animator.SetBool("GreenIsActivated", false);

        }
        if (ColorManager.Instance.greenIsActive && isGreenShining)
        {
            animator.SetBool("GreenIsActivated", true);
            this.gameObject.GetComponent<SpriteRenderer>().material = material[1];
           

            if (GreenColliderSkill != null)
            {
                GreenColliderSkill.enabled = true;
            }
        }

        if (ColorManager.Instance.greenIsActive && !isGreenShining)
        {
            animator.SetBool("GreenIsActivated", false) ;
            this.gameObject.GetComponent<SpriteRenderer>().material = material[1];
  
            if (GreenColliderSkill != null)
            {
                GreenColliderSkill.enabled = false;
            }

        }



        if (!ColorManager.Instance.greenIsActive && isGreenShining)
        {
            this.gameObject.GetComponent<SpriteRenderer>().material = material[0];

            animator.SetBool("GreenIsActivated", false);

            if (GreenColliderSkill != null)
            {
                GreenColliderSkill.enabled = false;
            }

        }
    }
}
