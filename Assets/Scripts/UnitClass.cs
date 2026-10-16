using System;
using UnityEngine;

public enum UnitType
{
    Soldier,
    Flamethrower,
    MachineGunner
}

[Serializable]
public struct MinMaxRange
{
    public float min;
    public float max;

    public MinMaxRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}

[CreateAssetMenu(fileName = "NewUnitData", menuName = "Strategy/Unit Data")]
public class UnitClass : ScriptableObject
{
    public UnitType unit;

    public UnitMovementStats movement;
    public UnitCombatStats combat;
    public UnitSurvivabilityStats survivability;
}

[Serializable]
public class UnitSurvivabilityStats
{
    public float maxUnitHealth;
    public float unitArmor;
}

[Serializable]
public class UnitMovementStats
{
    public float speed;
    public float rotationSpeed;
}

[Serializable]
public class UnitCombatStats
{
    public float armorPenetration;
    public float reloadingTime;

    public MinMaxRange rotationTolerance;
    public MinMaxRange rotationToleranceWhileMoving;

    public float distanceTolerance;
    public float distanceToleranceWhileMoving;

    public bool canChangeWhileWalking;
    public bool canShootWhileWalking;

    public GameObject unitShellPrefab;
    public float shellSpeed;

    public bool isAreaAttack;

    public float detectionRadius;
}