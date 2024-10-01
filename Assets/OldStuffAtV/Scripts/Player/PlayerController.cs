using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float Geschwindigkeit = 20f;// Erstellt einen öffentlichen Float namens "Geschwindigkeit"
    public float GeschwindigkeitsAbfall = 40f;
    public float StandartGeschwindigkeit = 50f;
    public float maxSpeed = 150f; // Erstellt einen öffentlichen Float für die Maximalgeschwindigkeit
    public float flipSteigerung = 10f;
    //public float yellowRocket = 120f;

    public float Direction = 0f;// erstellt einen privaten Float namens "Direction" auf 0
    public float DirectionVertical = 0f; // erstellt einen privaten Float für namens "DirectionVertical" auf 0

    public float GroundCheckRadius = 0.2f;
    public float ShroomCheckRadius = 0.2f;
    public float RedGroundCheckRadius = 0.2f;


    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask shroomLayer;
    [SerializeField] LayerMask redgroundLayer;
    //[SerializeField] LayerMask blueWallLayer;

    public bool isRedGrounded = false;
    public bool isGrounded = false;
    [SerializeField] Transform GroundCheckCollider;

    //public bool isWallSliding;
    //public float wallSlidingSpeed;
    //[SerializeField] Transform blueWallCheck;


    public bool isJumping = false;
    [SerializeField] Transform ShroomCheckCollider;


    [SerializeField] Transform RedGroundCheckCollider;


    public KillPlayer killSpawn;
    public bool isDead;
    public float waitAtSpawn = 2f;


    [SerializeField] Transform spawnPoint;
    [SerializeField] private bool onSpawn = false;

    public bool isFacingRight = true;

    [SerializeField] public Rigidbody2D Player; // Bezug zu Rigidbody2D namens Player



    public Animator animator;

    [Header("Events")]
    [Space]

    public UnityEvent OnJumpEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnGravityEvent;
    private bool GravityControl = false;
    private bool GravityControlAnimation = false;
    public ColorManager colorManager;
    public DashScript dash;
    public VFXManager gravityVFX;
    public Surprise surprise;

    //[SerializeField] PauseMenue pauseMenue;



    public float SprungGeschwindigkeit = 50f; // Erstellt öffentlichen Float namens "SprungGeschwindigkeit"


    //public bool WandRennen;



    [SerializeField] private AudioSource walkingSound;
    [SerializeField] private AudioSource anlaufSound;

    [SerializeField] PauseMenue pauseMenue;

    private void Awake()
    {
        Player = GetComponent<Rigidbody2D>();

        if (OnJumpEvent == null)
            OnJumpEvent = new UnityEvent();

        if (OnGravityEvent == null)
            OnGravityEvent = new BoolEvent();
    }


    void Start()
    {
        pauseMenue = GameObject.FindObjectOfType<PauseMenue>();
        Player = GetComponent<Rigidbody2D>(); // Erstellt am Anfang des Games einen Bezug zum Rigidbody namens "Player"
    }



    void Update()
    {
       
        if (pauseMenue.GameIsPaused)
        {
            return;
        }


        //Movement
        //__________________________________________________________________________________________________________

        Direction = Input.GetAxis("Horizontal"); // schaltet den Unity Bezug der Tasteneingaben zu "Horizontal" Voreinstellung von Unity frei
        DirectionVertical = Input.GetAxis("Vertical");

        
        Movement();





        Flip(); // HIER IST DIE FUNKTION FÜR DIE STEIGERUNG FUER DIE GESCHWINDIGKEIT PRO SEKUNDE!!!!!!




        //WallSlide();




        //__________________________________________________________________________________________________________


        //RESPAWN
        //__________________________________________________________________________________________________________

        Spawn();
        if (onSpawn == true)
        {
            
            Geschwindigkeit = 0f;
            //dash.canDash = false;
        }






        // JUMP
        // ______________________________________________________________________________________________________
        //if (Input.GetAxis("Left Trigger") > 0f)
        //{
        //    Player.velocity = new Vector2(Player.velocity.x, SprungGeschwindigkeit);
        //}

        //if (Input.GetAxis("Left Trigger") > 0f && Player.velocity.y > 0f)
        //{
        //    Player.velocity = new Vector2(Player.velocity.x, Player.velocity.y * 0.5f);
        //}



        
    }

    public void FixedUpdate()
    {
        GroundCheck();
        ShroomCheck();
        RedGroundCheck();
    }

    public void GroundCheck()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheckCollider.position, GroundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
            //GravityControlAnimation = true;
        }

    }


    public void ShroomCheck()
    {
        isJumping = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(ShroomCheckCollider.position, ShroomCheckRadius, shroomLayer);
        if (colliders.Length > 0 && colorManager.greenIsActive)
            isJumping = true;

    }

    public void RedGroundCheck()
    {
        isRedGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(RedGroundCheckCollider.position, RedGroundCheckRadius, redgroundLayer);
        if (colliders.Length > 0)
            isRedGrounded = true;

    }



    public void Flip()
    {
        if (isFacingRight && Direction < 0f || !isFacingRight && Direction > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void Movement()
    {

        
        if (Direction == 0f)
        {
            Geschwindigkeit = Geschwindigkeit - GeschwindigkeitsAbfall * Time.deltaTime;

            if (Geschwindigkeit <= 20f)
            { Geschwindigkeit = StandartGeschwindigkeit; }

        }




        if (Direction > 0f) // wenn die Richtung der gedrückten Tasten ( a oder d ) auf der Y Achse über 0 sind
        {
            Player.velocity = new Vector2(Direction * Geschwindigkeit, Player.velocity.y);
            if (Geschwindigkeit < maxSpeed)
            {
                Geschwindigkeit += flipSteigerung * Time.deltaTime;


                if (Geschwindigkeit > 20f && Geschwindigkeit < 21f && dash.isDashing == false && dash.canDash == true)
                {
                    anlaufSound.Play();
                }
            }
        }

        else if (Direction < 0f) // wenn die Richtung der gedrückten Tasten(a oder d ) auf der Y Achse unter 0 sind
        {
            Player.velocity = new Vector2(Direction * Geschwindigkeit, Player.velocity.y);
            if (Geschwindigkeit < maxSpeed)
            {
                Geschwindigkeit += flipSteigerung * Time.deltaTime;



                if (Geschwindigkeit > 20f && Geschwindigkeit < 21f && dash.isDashing == false && dash.canDash == true)
                {
                    anlaufSound.Play();
                }
            }
        }
        else // "Andernfalls", also wenn gar keine Taste A oder D gedrückt wird, dann bedeutet das, dass auf der X Achse 0 Bewegung stattfindet!
        {
            Player.velocity = new Vector2(0, Player.velocity.y);
        }




        if (Input.GetKeyDown(KeyCode.S) && !isGrounded/* && !colorManager.blueIsActive && !colorManager.yellowIsActive && !colorManager.redIsActive */||
            Input.GetKey(KeyCode.Joystick1Button4) && !isGrounded/* && !colorManager.blueIsActive && !colorManager.yellowIsActive && !colorManager.redIsActive*/)
        {
            Player.gravityScale = 70f;
            GravityControl = true;
        }
        if (Input.GetKeyUp(KeyCode.Joystick1Button4) || Input.GetKeyUp("s"))
        {
            Player.gravityScale = 7f;
            GravityControl = false;
        }
        if (GravityControl == true)
        {
            animator.SetBool("IsGravityControl", true);
            gravityVFX.Gravity();

        }
        if (isGrounded == true)
        {
            animator.SetBool("IsGravityControl", false);
            gravityVFX.animator.SetBool("IsGravityControlVFX", false);
        }

        animator.SetFloat("Speed", (Geschwindigkeit));


        if (isGrounded)
        {
            animator.SetBool("IsGrounded", true);
            animator.SetBool("IsJumping", false);
        }
        else
        {
            animator.SetBool("IsGrounded", false);
        }


        if (isJumping)
        {
            animator.Play("jump");
        }

        //if (Input.GetKeyDown(KeyCode.S) && !isGrounded && colorManager.yellowIsActive && Direction > 0f ||
        //    Input.GetKeyDown(KeyCode.Joystick1Button4) && !isGrounded && colorManager.yellowIsActive && Direction > 0f)
        //{

        //    Player.gravityScale = 0f;
        //    Player.velocity = new Vector2(yellowRocket, 0f);
        //}
        //if (Input.GetKeyDown(KeyCode.S) && !isGrounded && colorManager.yellowIsActive && Direction < 0f ||
        //    Input.GetKey(KeyCode.Joystick1Button4) && !isGrounded && colorManager.yellowIsActive && Direction < 0f)
        //{
        //    Player.gravityScale = 0f;
        //    Player.velocity = new Vector2(-yellowRocket, 0f);
        //}
        //if ((Input.GetKeyUp(KeyCode.S) && isGrounded && colorManager.yellowIsActive ||
        //    Input.GetKeyUp(KeyCode.Joystick1Button4) && isGrounded && colorManager.yellowIsActive)
        //    || colorManager.blueIsActive || colorManager.redIsActive)
        //{
        //    Player.gravityScale = 7f;

        //}
        
    }


    public void Spawn()
    { //SPAWN mit eigener Tastenbelegung
        //______________________________________________________       
        if (Input.GetKeyUp(KeyCode.R) || Input.GetKeyDown(KeyCode.Joystick1Button6))
        {
            StartCoroutine(SpawnPause());
            Debug.Log("Test1");
            // return;
        }

        if (killSpawn.willSpawn)
        {
            StartCoroutine(SpawnPause());
        }

    }

    public IEnumerator SpawnPause()
    {
        Debug.Log("Start");
        onSpawn = true;
        Player.velocity = Vector2.zero;
        //SoundManager.sndMan.PlayRespawnSound();
        yield return new WaitForSeconds(waitAtSpawn);
        transform.CompareTag("Player");
        transform.position = spawnPoint.position;
        onSpawn = false;
        killSpawn.willSpawn = false;
        Debug.Log("Ende");

    }

    public void StartWallPause()
    {
        StartCoroutine(WallPause());
    }

    public IEnumerator WallPause()
    {
        Player.velocity = Vector2.zero;
        Geschwindigkeit = 0f;
        yield return new WaitForSeconds(3);
    }






    private void ChangeSize(Vector3 groesseAendern)
    {
        if (isFacingRight)
        {
            Player.gameObject.transform.localScale = groesseAendern;
        }

        else
        {
            Player.gameObject.transform.localScale = new Vector3(-groesseAendern.x, groesseAendern.y, groesseAendern.z);
        }
    }



    //    public bool IsBlueWalled()
    //{
    //    return Physics2D.OverlapCircle(blueWallCheck.position, 1f, blueWallLayer);
    //}

    //private void WallSlide()
    //{
    //    if (IsBlueWalled() && !isGrounded && Direction != 0f && colorManager.blueIsActive && Input.GetKey(KeyCode.Joystick1Button4) ||
    //        IsBlueWalled() && !isGrounded && Direction != 0f && colorManager.blueIsActive && Input.GetKey("s"))
    //    {
    //        isWallSliding = true;
    //        Player.velocity = new Vector2(Player.velocity.x, Mathf.Clamp(Player.velocity.y, -wallSlidingSpeed, float.MaxValue));
    //    }
    //    else
    //    {
    //        isWallSliding = false;
    //    }


    //}










}
//private void Movement()

//{

//    Direction = Input.GetAxisRaw("Horizontal"); // schaltet den Unity Bezug der Tasteneingaben zu "Horizontal" Voreinstellung von Unity frei
//    DirectionVertical = Input.GetAxisRaw("Vertical");

//    if (Direction == 0f)
//    {
//        Geschwindigkeit = Geschwindigkeit - GeschwindigkeitsAbfall * Time.deltaTime;

//        if (Geschwindigkeit <= 20f)
//        { Geschwindigkeit = StandartGeschwindigkeit; }

//    }



//    if (Direction > 0f) // wenn die Richtung der gedrückten Tasten ( a oder d ) auf der Y Achse über 0 sind
//    {
//        Player.velocity = new Vector2(Direction * Geschwindigkeit, Player.velocity.y);
//        if (Geschwindigkeit < maxSpeed)
//        {
//            Geschwindigkeit += flipSteigerung * Time.deltaTime;
//        }
//    }

//    else if (Direction < 0f) // wenn die Richtung der gedrückten Tasten(a oder d ) auf der Y Achse unter 0 sind
//    {
//        Player.velocity = new Vector2(Direction * Geschwindigkeit, Player.velocity.y);
//        if (Geschwindigkeit < maxSpeed)
//        {
//            Geschwindigkeit += flipSteigerung * Time.deltaTime;
//        }
//    }
//    else // "Andernfalls", also wenn gar keine Taste A oder D gedrückt wird, dann bedeutet das, dass auf der X Achse 0 Bewegung stattfindet!
//    {
//        Player.velocity = new Vector2(0, Player.velocity.y);
//    }
//}

//________________________________________________________________
// AUTOMATIC SPAWN



//private bool IsGrounded()
//{
//    return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
//}
