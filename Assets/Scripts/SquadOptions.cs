using System;
using System.Collections.Generic;
using UnityEngine;

public class SquadOptions : MonoBehaviour
{
    [Header("Squad classes")]
    [SerializeField] private SquadClass _soldierSquadConfig;
    [SerializeField] private SquadClass _machineGunnerSquadConfig;

    [Header("Unit classes")]
    [SerializeField] private UnitClass _soldierUnitConfig;
    [SerializeField] private UnitClass _machineGunnerUnitConfig;

    [Header("Squad object with collider")]
    [SerializeField] private GameObject _squadCollider;

    [Serializable]
    public class Unit
    {
        public UnitType type;
        public float unitHealth;
        public float maxUnitHealth;
    }



    //survivability
    [HideInInspector] public float unitArmor => (squad == squadType.MachineGunner) ? _machineGunnerUnitConfig.survivability.unitArmor : _soldierUnitConfig.survivability.unitArmor;
    [HideInInspector] public int maxUnitAmount => (squad == squadType.MachineGunner) ? _machineGunnerSquadConfig.maxUnitAmount : _soldierSquadConfig.maxUnitAmount;

    //movement
    [HideInInspector] public float speed => (squad == squadType.MachineGunner) ? _machineGunnerUnitConfig.movement.speed : _soldierUnitConfig.movement.speed;
    [HideInInspector] public float rotationSpeed => (squad == squadType.MachineGunner) ? _machineGunnerUnitConfig.movement.rotationSpeed : _soldierUnitConfig.movement.rotationSpeed;

    //combat
    [HideInInspector] public float rotationToleranceMin => (squad == squadType.MachineGunner) ? _machineGunnerUnitConfig.combat.rotationTolerance.min : _soldierUnitConfig.combat.rotationTolerance.min;
    [HideInInspector] public float rotationToleranceMax => (squad == squadType.MachineGunner) ? _machineGunnerUnitConfig.combat.rotationTolerance.max : _soldierUnitConfig.combat.rotationTolerance.max;
    [HideInInspector] public float distanceTolerance => (squad == squadType.MachineGunner) ? _machineGunnerUnitConfig.combat.distanceTolerance : _soldierUnitConfig.combat.distanceTolerance;
    [HideInInspector] public float detectionRadius => (squad == squadType.MachineGunner) ? _machineGunnerUnitConfig.combat.detectionRadius : _soldierUnitConfig.combat.detectionRadius;
    [HideInInspector] public int maxAmmo => (squad == squadType.MachineGunner) ? _machineGunnerSquadConfig.maxAmmo : _soldierSquadConfig.maxAmmo;
    [HideInInspector] public bool canChangeWhileWalking => (squad == squadType.MachineGunner) ? _machineGunnerUnitConfig.combat.canChangeWhileWalking : _soldierUnitConfig.combat.canChangeWhileWalking;
    [HideInInspector] public bool canShootWhileWalking => (squad == squadType.MachineGunner) ? _machineGunnerUnitConfig.combat.canShootWhileWalking : _soldierUnitConfig.combat.canShootWhileWalking;
    [HideInInspector] public float shellSpeed => (squad == squadType.MachineGunner) ? _machineGunnerUnitConfig.combat.shellSpeed : _soldierUnitConfig.combat.shellSpeed;
    [HideInInspector] public float armorPenetration => (squad == squadType.MachineGunner) ? _machineGunnerUnitConfig.combat.armorPenetration : _soldierUnitConfig.combat.armorPenetration;
    [HideInInspector] public GameObject shell => (squad == squadType.MachineGunner) ? _machineGunnerUnitConfig.combat.unitShellPrefab : _soldierUnitConfig.combat.unitShellPrefab;



    [HideInInspector] public float reloadingTime;
    [HideInInspector] private float _saveReloadingTime;
    [HideInInspector] public int ammo;

    // Amount variables
    private float _floatMaxUnitAmount;
    [HideInInspector] public List<Unit> amount;
    [HideInInspector] public int deadUnits;

    public enum squadType
    {
        Soldier,
        MachineGunner
    }
    [Header("Squad type")]
    public squadType squad;



    private SpriteRenderer _squadSprite;

    [Header("Squad sprite")]
    [SerializeField] private Sprite _soldierSprite;
    [SerializeField] private Sprite _machineGunnerSprite;

    public string neutalsTag;
    public string alliesTag;
    public string enemiesTag;

    public enum sideType { Neutral, Ally, Enemy }
    [Header("Side type")]
    public sideType side;

    [Header("Side color")]
    [SerializeField] private Color32 _neutralColor = new Color32(255, 255, 255, 255);
    [SerializeField] private Color32 _allyColor = new Color32(90, 90, 255, 255);
    [SerializeField] private Color32 _enemyColor = new Color32(255, 90, 90, 255);

    void Awake()
    {
        _squadSprite = GetComponentInChildren<SpriteRenderer>();

        SquadSettings();
        ammo = maxAmmo;
    }

    void Start()
    {
        ChangeTag();
        CheckUnits();
    }

    public void ChangeTag()
    {
        switch (side)
        {
            case sideType.Neutral:
                _squadCollider.tag = neutalsTag;
                _squadSprite.color = _neutralColor;
                break;
            case sideType.Ally:
                _squadCollider.tag = alliesTag;
                _squadSprite.color = _allyColor;
                break;
            case sideType.Enemy:
                _squadCollider.tag = enemiesTag;
                _squadSprite.color = _enemyColor;
                break;
        }
    }

    public void SquadSettings()
    {
        switch (squad)
        {
            case squadType.MachineGunner:

                reloadingTime = _machineGunnerUnitConfig.combat.reloadingTime;
                _saveReloadingTime = reloadingTime;

                _floatMaxUnitAmount = maxUnitAmount;

                amount = new List<Unit>();

                for (int i = 0; i < maxUnitAmount; i++)
                {
                    Unit newUnit = new Unit();
                    newUnit.type = UnitType.MachineGunner;
                    newUnit.maxUnitHealth = _machineGunnerUnitConfig.survivability.maxUnitHealth;
                    newUnit.unitHealth = newUnit.maxUnitHealth;

                    amount.Add(newUnit);
                }

                _squadSprite.sprite = _machineGunnerSprite;
                break;

            case squadType.Soldier:

                reloadingTime = _soldierUnitConfig.combat.reloadingTime;
                _saveReloadingTime = reloadingTime;

                _floatMaxUnitAmount = maxUnitAmount;

                amount = new List<Unit>();

                for (int i = 0; i < maxUnitAmount; i++)
                {
                    Unit newUnit = new Unit();
                    newUnit.type = UnitType.Soldier;
                    newUnit.maxUnitHealth = _soldierUnitConfig.survivability.maxUnitHealth;
                    newUnit.unitHealth = newUnit.maxUnitHealth;

                    amount.Add(newUnit);
                }

                _squadSprite.sprite = _soldierSprite;
                break;
        }

        switch (side)
        {
            case sideType.Neutral:
                GetComponentInChildren<SquadAttack>().transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(180f, -180f));
                break;
            case sideType.Ally:
                GetComponentInChildren<SquadAttack>().transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case sideType.Enemy:
                GetComponentInChildren<SquadAttack>().transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                break;
        }
    }

    public void CheckUnits()
    {
        if (amount.Count == 0) Destroy(gameObject);
        else reloadingTime = _saveReloadingTime * (1 + (1 - amount.Count / _floatMaxUnitAmount));

        Debug.Log(amount.Count);
    }
}