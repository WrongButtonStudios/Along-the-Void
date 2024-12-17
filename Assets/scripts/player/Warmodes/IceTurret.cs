using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTurret : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _gunBarrels = new List<Transform>();
    [SerializeField, Tooltip("Decides if the Turret shoots in every direction or just at a enemy nearby")]
    private FireMode _fireMode;
    [SerializeField, Tooltip("Cooldown duration in seconds.")]
    private float _coolDownDuration = 0.25f;
    [SerializeField]
    private GameObject _iceBullet; 
    private bool _coolingDown; 

    private enum FireMode
    {
        AnyDirection,
        ShootOnSight
    }

    // Update is called once per frame
    void Update()
    {
        switch (_fireMode)
        {
            case FireMode.AnyDirection:
                ShootInAnydirection();
                break;
            case FireMode.ShootOnSight:
                ShootAtEnemyNearby();
                break; 
        }
    }

    private void ShootInAnydirection()
    {
        
        if (!_coolingDown)
        {
            foreach (Transform t in _gunBarrels)
            {
                var iceBullet = IceBulletPool.Instance.GetPooledIceBullet();
                iceBullet.transform.position = t.position;
                iceBullet.transform.rotation = t.rotation;
                iceBullet.SetActive(true); 
            }
            StartCoroutine(CooldownYield()); 
        }
    }

    private void ShootAtEnemyNearby()
    {
            throw new System.NotImplementedException("bin faul, sry jungs"); 
    }

    private IEnumerator CooldownYield()
    {
        _coolingDown = true;
        yield return new WaitForSeconds(_coolDownDuration);
        _coolingDown = false; 
    }
}
