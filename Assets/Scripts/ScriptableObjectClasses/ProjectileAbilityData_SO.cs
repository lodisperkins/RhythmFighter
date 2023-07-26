
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityData/ProjectileAbilityData")]
class ProjectileAbilityData_SO : AbilityData_SO
{
    public ProjectileAbilityData_SO()
    {
        _customStats = new Stat[] { new Stat("Speed", 1), new Stat("MaxInstances", -1) };
    }
}
