using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatonVfxHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> _vfx = new(); 

    public void PlayActivationVfx(PlayerColor color) 
    {
        sbyte effect = (sbyte)color;
        _vfx[effect].SetActive(true);
        StartCoroutine(VfxDuration(effect)); 
    }

    private IEnumerator VfxDuration(sbyte i) 
    {
        yield return new WaitForSeconds(1);
        _vfx[i].SetActive(false);
    }
}
