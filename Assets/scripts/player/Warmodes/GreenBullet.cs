using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBullet : MonoBehaviour
{
    [SerializeField, Tooltip("The lifespan of the Object in sencods.")]
    private float _lifeDuration = 3f;
    [SerializeField, Tooltip("The force used to calculate the speed.")]
    private float _force = 25f;
    private float _timePassed;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.isActiveAndEnabled == false)
            return;
        _timePassed += Time.fixedDeltaTime;
        if (_timePassed >= _lifeDuration)
        {

            _timePassed = 0;
            GreenBulletPool.Instance.DeactivateGreenBullet(this.gameObject);
        }

        _rb.AddForce(this.transform.right * _force * Time.fixedDeltaTime);
        //kommentar um n neuen push mit dem namen "currently not merge ready" zu machen 
    }
}
