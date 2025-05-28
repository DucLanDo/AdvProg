using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    public int weaponLevel;
    public List<WeaponStats> stats;
}

[System.Serializable]
public class WeaponStats {

    public float cooldown;
    public float duration;
    public float damage;
    public float range;
    public float speed;
}
