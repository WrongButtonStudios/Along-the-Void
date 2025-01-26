using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBulletPool : MonoBehaviour
{

    public static GreenBulletPool Instance;

    [SerializeField] private GameObject GreenBulletPref;
    private List<GameObject> _inactiveGreenBullet = new();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    public GameObject GetPooledGreenBullet()
    {
        GameObject greenBullet = null;
        if (_inactiveGreenBullet.Count == 0)
        {
            greenBullet = Instantiate(GreenBulletPref, transform.position, Quaternion.identity);
            greenBullet.SetActive(false);
            return greenBullet;
        }
        greenBullet = _inactiveGreenBullet[0];
        _inactiveGreenBullet.Remove(greenBullet);
        return greenBullet;
    }

    public void DeactivateGreenBullet(GameObject greenBullet)
    {
        _inactiveGreenBullet.Add(greenBullet.gameObject);
        greenBullet.gameObject.SetActive(false);
    }
}