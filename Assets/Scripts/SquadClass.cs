using System;
using UnityEngine;

public enum SquadType
{
    Soldier,
    Flamethrower,
    MachineGunner
}

[CreateAssetMenu(fileName = "NewSquadData", menuName = "Strategy/Squad Data")]
public class SquadClass : ScriptableObject
{
    public UnitType unit;
    public int maxUnitAmount;
    public int maxAmmo;
}