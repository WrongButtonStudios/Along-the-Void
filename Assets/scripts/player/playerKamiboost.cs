using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerKamiboost : MonoBehaviour, IplayerFeature
{

    private characterController characterController;
    private CharacterMovement characterMovement;
    private ContactFilter2D contactFilter = new ContactFilter2D();
    private CollisionHandler _collisionHandler;
    private float damagePerSecond = 0.1f;
    private fairyController _fairyController;  
    private string defaultLayerName = "yellowDustArea";
    public int kamiBoostSpeed = 200;
    private bool doKamiboost = false;
    private Vector2 _dir; 
    public LayerMask layerMask;
    private GameObject kamiBoostParticelEffect;
    private ParticleSystem particleEffect;





    public void Awake()
    {
        layerMask = LayerMask.GetMask(defaultLayerName);
        _collisionHandler = this.GetComponent<CollisionHandler>();
        _fairyController = this.GetComponent<Warmodes>().FairyController; 
        contactFilter.useTriggers = true;
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask;
        kamiBoostParticelEffect = transform.Find("KamiBoost").gameObject;
        kamiBoostParticelEffect.SetActive(false);
        // Existierende Logik...
        particleEffect = kamiBoostParticelEffect.transform.Find("Flare").GetComponent<ParticleSystem>();

        var main = particleEffect.main;
        

    }

    public void FixedUpdate()
    {
        if (doKamiboost)
        {
            
            characterController.StatusData.isAllowedToMove = false;

            if (_collisionHandler.InYellowFog == false)
            {
                float yellowColorAmount = PlayerDamageHandler.GetDamage(damagePerSecond * Time.fixedDeltaTime, PlayerUttillitys.GetPlayerColor(characterController), _fairyController);
                if (yellowColorAmount <= 0f)
                {
                    endFeauture();
                    return;
                }
            }

            if (!characterController.Input.TriggerPlayerFeatureInput)
            {
                endFeauture();
                return;
            }

            characterMovement.disableMovement();
            characterController.rb.gravityScale = 0;
            characterController.rb.velocity = new Vector2(characterController.rb.velocity.x, 0);

            // Neue Geschwindigkeitslogik
            float currentSpeed = Mathf.Abs(characterController.rb.velocity.x);
            float newSpeed = Mathf.Min(currentSpeed + (kamiBoostSpeed * Time.fixedDeltaTime), kamiBoostSpeed);
            float direction = characterMovement.GetCharacterLookingDirection() ? 1 : -1;

            characterMovement.setMaxSpeed(kamiBoostSpeed);
            characterController.rb.velocity = new Vector2(newSpeed * direction, 0);
        }
    }


    public void initFeauture(characterController characterController)
    {
        this.characterController = characterController;
        this.characterMovement = characterController.GetComponent<CharacterMovement>();
    }



    public void triggerFeauture(bool useInput = false, bool input = false)
    {
        float yellowColorAmount = PlayerDamageHandler.GetHealth(PlayerUttillitys.GetPlayerColor(characterController), _fairyController);
        if (!characterController.getPlayerStatus().isGrounded && !doKamiboost && yellowColorAmount > 0)
        {

            List<Collider2D> colliders = new List<Collider2D>();
            characterController.rb.OverlapCollider(contactFilter, colliders);
            CameraShake.Instance.ShakeCamera(2.5f); 
            Debug.Log("Activated Kami boost...");
            doKamiboost = input;
            Debug.Log("doKamiboost = " + doKamiboost);

            // Flip Sprite basierend auf Blickrichtung
            bool isLookingRight = characterMovement.GetCharacterLookingDirection();

            CameraShake.Instance.ShakeCamera(2.5f); 
            kamiBoostParticelEffect.SetActive(true);
            if (input == false)
            {
                endFeauture();
            }
        }
    }

    public void endFeauture()
    {
        characterController.rb.gravityScale = 1;
        characterMovement.enableMovement();
        doKamiboost = false;
        kamiBoostParticelEffect.SetActive(false);
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (LayerMask.LayerToName(collider.gameObject.layer) == defaultLayerName)
        {
            endFeauture();
        }
    }

    public bool getKamiboostStatus()
    {
        return doKamiboost;
    }
}
