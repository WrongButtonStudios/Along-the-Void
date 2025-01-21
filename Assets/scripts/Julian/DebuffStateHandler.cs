using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffStateHandler : MonoBehaviour
{
    public void HandleDebuffs(Health health, Debuffs debuffs)
    {
        if((debuffs & Debuffs.Burning) == Debuffs.Burning)
        {
            health.GetDamage((3.33f * Time.deltaTime) * PhysicUttillitys.TimeScale);
        }
    }
}