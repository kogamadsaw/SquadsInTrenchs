using UnityEngine;

public class TrenchOptions : MonoBehaviour
{
    public float trenchMaxHealth = 100;
    private float _trenchHealth;

    [HideInInspector] public float trenchHealth
    {
        get => _trenchHealth;
        set => _trenchHealth = Mathf.Clamp(value, 0f, trenchMaxHealth);
    }


    [Header("Side color")]
    [SerializeField] private SpriteRenderer _sideSprite;
    [SerializeField] private Color32 _allyColor = new Color32(0, 145, 255, 80);
    [SerializeField] private Color32 _enemyColor = new Color32(255, 0, 0, 50);

    [Header("Side tag")]
    public string trenchAllyTag = "AlliesTrench";
    public string trenchEnemyTag = "EnemiesTrench";
    public enum sideType
    {
        Ally,
        Enemy
    }
    [Header("Trench side type")]
    public sideType side;

    private void Awake()
    {
        _sideSprite = GetComponentInChildren<SpriteRenderer>();

        trenchHealth = trenchMaxHealth;

        switch (side)
        {
            case sideType.Ally:
                gameObject.tag = trenchAllyTag;
                _sideSprite.color = _allyColor;
                break;

            case sideType.Enemy:
                _sideSprite.color = _enemyColor;
                gameObject.tag = trenchEnemyTag;
                break;
        }
    }

    private void Update()
    {
        if (trenchHealth > trenchMaxHealth) trenchHealth = trenchMaxHealth;
        else if (trenchHealth < 0) trenchHealth = 0;
    }
}
