//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class playerKamiboost : MonoBehaviour, IplayerFeature
//{

//    private characterController characterController;
//    private ContactFilter2D contactFilter = new ContactFilter2D();
//    private string defaultLayerName = "yellowDustArea";



//    public void Awake()
//    {
//        layerMask = LayerMask.GetMask(defaultLayerName);

//        contactFilter.useTriggers = true;
//        contactFilter.useLayerMask = true;
//        contactFilter.layerMask = layerMask;
//    }
//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }


//    public void initFeauture(characterController characterController)
//    {
//        this.characterController = characterController;
//    }

//    public void triggerFeauture()
//    {
//        if (characterController.getPlayerStatus().!isGrounded)
//        {
//            List<Collider2D> colliders = new List<Collider2D>();

//            characterController.rb.OverlapCollider(contactFilter, colliders);

//            if (colliders.Count > 0)
//            {
//                characterController.rb.gravityScale = characterController.rb.gravityScale;
//            }
//        }
//    }

//}
