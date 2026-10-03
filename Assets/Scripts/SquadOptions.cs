using UnityEngine;

public class SquadOptions : MonoBehaviour
{
    [Header("Squad object with collider")]
    [SerializeField] private GameObject _squadCollider;

    //Amount variables
    [HideInInspector] public int maxAmount;
    [HideInInspector] public int amount;

    //Squad variables
    [HideInInspector] public float speed;
    [HideInInspector] public float unitHealth;
    [HideInInspector] public float squadHealth;
    [HideInInspector] public float unitArmor;
    [HideInInspector] public float armorPenetration;
    [HideInInspector] public float reloadingTime;
    [HideInInspector] public float[] rotationTolerance = new float[2];
    [HideInInspector] public float distanceToleranceMin; [HideInInspector] public float distanceToleranceMax;
    [HideInInspector] public bool canDoWhileWalking;

    //Squad visual
    public enum squadType
    {
        Soldiers,
        Tanks,
        Artillery
    }
    [Header("Squad type")]
    public squadType squad;
    private SpriteRenderer _squadSprite;

    [Header("Squad sprite")]
    [SerializeField] private Sprite _soldierSprite;
    [SerializeField] private Sprite _artillerySprite;
    [SerializeField] private Sprite _tankSprite;

    //Finding variables
    [HideInInspector] public float rotationSpeed;
    [HideInInspector] public float detectionRadius;
    
    
    //Shells variables
    [HideInInspector] public float shellSpeed;
    [HideInInspector] public GameObject shell;

    [Header("Shell prefab")]
    [SerializeField] private GameObject _soldierShellPrefab;
    [SerializeField] private GameObject _tankShellPrefab;
    [SerializeField] private GameObject _artilleryShellPrefab;

    //Side variables
    [Header("Side Tag")]
    public string neutalsTag;
    public string alliesTag;
    public string enemiesTag;

    //Side visual
    public enum sideType
    {
        Neutral,
        Ally,
        Enemy
    }
    [Header("Side type")]
    public sideType side;

    [Header("Side color")]
    [SerializeField] private Color32 _neutralColor = new Color32(255, 255, 255, 255);
    [SerializeField] private Color32 _allyColor = new Color32(90, 90, 255, 255);
    [SerializeField] private Color32 _enemyColor = new Color32(255, 90, 90, 255);

    void Awake()
    {
        _squadSprite = GetComponentInChildren<SpriteRenderer>();

        switch (squad)
        {
            case squadType.Soldiers:
                maxAmount = 10;
                unitHealth = 10f;
                squadHealth = unitHealth * maxAmount;
                unitArmor = 0.1f;

                speed = 3f;

                rotationSpeed = 100f;
                detectionRadius = 15f;

                rotationTolerance[0] = -10f; rotationTolerance[1] = 10f;
                distanceToleranceMin = 0.7f; distanceToleranceMax = 2f - distanceToleranceMin;

                armorPenetration = 7.87f;
                reloadingTime = 1;

                shell = _soldierShellPrefab;
                shellSpeed = 15;

                canDoWhileWalking = false;

                _squadSprite.sprite = _soldierSprite;
                break;

            case squadType.Tanks:
                maxAmount = 1;
                unitHealth = 100f;
                squadHealth = unitHealth * maxAmount;
                unitArmor = 25f;

                speed = 2f;

                rotationSpeed = 40f;
                detectionRadius = 40f;
                rotationTolerance[0] = -20f; rotationTolerance[1] = 20f;
                distanceToleranceMin = 0.5f; distanceToleranceMax = 2f - distanceToleranceMin;

                armorPenetration = 50.18f;
                reloadingTime = 3;

                shell = _tankShellPrefab;
                shellSpeed = 10;

                canDoWhileWalking = true;

                _squadSprite.sprite = _tankSprite;
                break;

            case squadType.Artillery:
                maxAmount = 2;
                unitHealth = 25f;
                squadHealth = unitHealth * maxAmount;
                unitArmor = 5f;

                speed = 1f;

                rotationSpeed = 50f;
                detectionRadius = 17f;
                rotationTolerance[0] = -30f; rotationTolerance[1] = 30f;
                distanceToleranceMin = 0.2f; distanceToleranceMax = 2f - distanceToleranceMin;

                armorPenetration = 30.13f;
                reloadingTime = 5;

                shell = _artilleryShellPrefab;
                shellSpeed = 7;

                canDoWhileWalking = false;

                _squadSprite.sprite = _artillerySprite;
                break;
        }

        switch (side)
        {
            case sideType.Neutral:
                _squadSprite.color = _neutralColor;
                break;

            case sideType.Ally:
                _squadSprite.color = _allyColor;
                break;

            case sideType.Enemy:
                _squadSprite.color = _enemyColor;
                break;
        }
    }

    private void Start()
    {
        if (side == sideType.Neutral)
        {
            GetComponentInChildren<SquadAttack>().transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(180f, -180f));
            _squadCollider.tag = neutalsTag;
        }
        else if (side == sideType.Ally)
        {
            GetComponentInChildren<SquadAttack>().transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            _squadCollider.tag = alliesTag;
        }
        else
        {
            GetComponentInChildren<SquadAttack>().transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            _squadCollider.tag = enemiesTag;
        }
    }

    private void Update()
    {
        if (squadHealth <= 0) Destroy(gameObject);
    }
}
