using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    private bool isLookingRight;
    public LayerMask layerMask;
    private GameObject kamiBoostParticelEffect;
    private Dictionary<ParticleSystem, bool> particleSystems = new Dictionary<ParticleSystem, bool>();

    void Awake()
    {
        layerMask = LayerMask.GetMask(defaultLayerName);
        _collisionHandler = this.GetComponent<CollisionHandler>();
        _fairyController = this.GetComponent<Warmodes>().FairyController;
        contactFilter.useTriggers = true;
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = layerMask;

        kamiBoostParticelEffect = transform.Find("KamiBoost").gameObject;
        kamiBoostParticelEffect.SetActive(false);

        var rotationConfigs = new Dictionary<string, bool>()
        {
            {"Flare", true},
            {"Glow", false},
            {"Goo Particles", false},
            {"Goo Particles Burst", false},
            {"Ring1", false},
            {"Ring1 (1)", false},
            {"Ring2leftturn", false},
            {"Ring2Rightturn", false}
        };

        foreach (var config in rotationConfigs)
        {
            try
            {
                var ps = kamiBoostParticelEffect.transform.Find(config.Key).GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    particleSystems.Add(ps, config.Value);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Konnte ParticleSystem {config.Key} nicht finden: {e.Message}");
            }
        }
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
                    endFeature();
                    return;
                }
            }

            if (!characterController.Input.TriggerPlayerFeatureInput)
            {
                endFeature();
                return;
            }

            characterMovement.disableMovement();
            characterController.rb.gravityScale = 0;
            characterController.rb.velocity = new Vector2(characterController.rb.velocity.x, 0);

            float currentSpeed = Mathf.Abs(characterController.rb.velocity.x);
            float newSpeed = Mathf.Min(currentSpeed + (kamiBoostSpeed * Time.fixedDeltaTime), kamiBoostSpeed);
            float direction = characterMovement.GetCharacterLookingDirection() ? 1 : -1;

            characterMovement.setMaxSpeed(kamiBoostSpeed);
            characterController.rb.velocity = new Vector2(newSpeed * direction, 0);
        }
    }

    public void initFeature(characterController characterController)
    {
        this.characterController = characterController;
        this.characterMovement = characterController.GetComponent<CharacterMovement>();
    }

    public void triggerFeature(bool useInput = false, bool input = false)
    {
        float yellowColorAmount = PlayerDamageHandler.GetHealth(PlayerUttillitys.GetPlayerColor(characterController), _fairyController);
        if (!characterController.getPlayerStatus().isGrounded && !doKamiboost && yellowColorAmount > 0)
        {
            List<Collider2D> colliders = new List<Collider2D>();
            characterController.rb.OverlapCollider(contactFilter, colliders);
            CameraShake.Instance.ShakeCamera(5f); 
            Debug.Log("Activated Kami boost...");
            doKamiboost = input;
            Debug.Log("doKamiboost = " + doKamiboost);

            isLookingRight = characterMovement.GetCharacterLookingDirection();
            UpdateRotation();

            kamiBoostParticelEffect.SetActive(true);
            if (input == false)
            {
                endFeature();
            }
        }
    }

    void UpdateRotation()
    {
        foreach (var ps in particleSystems)
        {
            var main = ps.Key.main;
            float rotation = (isLookingRight == ps.Value) ? 180f * Mathf.Deg2Rad : 0f;
            main.startRotation = new ParticleSystem.MinMaxCurve(rotation);
        }
    }

    public void endFeature()
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
            endFeature();
        }
    }

    public bool getKamiboostStatus()
    {
        return doKamiboost;
    }
}