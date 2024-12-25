using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBulletPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _iceBulletPref;
    [SerializeField]
    private int _iceBulletCount = 30;

    private List<GameObject> _iceBullets = new List<GameObject>();

    public static IceBulletPool Instance; 
    // Start is called before the first frame update
    void Awake()
    {
        if (IceBulletPool.Instance == null)
        {
            IceBulletPool.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (IceBulletPool.Instance.gameObject != this.gameObject)
        {
            Destroy(this.gameObject);
            return; 
        }
        InitBullets(); 
    }


    private void InitBullets()
    {
        for (int i = 0; i < _iceBulletCount; ++i)
        {
            var bullet = Instantiate(_iceBulletPref, transform.position, Quaternion.identity);
            bullet.SetActive(false); 
            _iceBullets.Add(bullet);
        }
    }

    public GameObject GetPooledIceBullet()
    {
        foreach (GameObject g in _iceBullets)
        {
            if (g.activeInHierarchy == false)
                return g; 
        }

        Debug.LogError("currently is no Ice Bullet available or list is empty.");
        return null; 
    }
}
