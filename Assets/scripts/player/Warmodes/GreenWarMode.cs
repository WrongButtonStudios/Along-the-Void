using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenWarMode : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _gunBarrels = new List<Transform>();
    [SerializeField, Tooltip("Decides if the Turret shoots in every direction or just at a enemy nearby")]
    private FireMode _fireMode;
    [SerializeField, Tooltip("Cooldown duration in seconds.")]
    private float _coolDownDuration = 0.25f;
    [SerializeField]
    private GameObject _greenBullet;
    private bool _coolingDown;
    private bool _isActive = false;

    private enum FireMode
    {
        AnyDirection,
        ShootOnSight
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive == false)
        {
            return;
        }
        ShootInAnydirection();
    }


    public void SetIsActive(bool var)
    {
        _isActive = var;
    }

    private void ShootInAnydirection()
    {

        if (!_coolingDown)
        {
            foreach (Transform t in _gunBarrels)
            {
                var greenBullet = GreenBulletPool.Instance.GetPooledGreenBullet();
                greenBullet.transform.position = t.position;
                greenBullet.transform.rotation = t.rotation;
                greenBullet.SetActive(true);
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
