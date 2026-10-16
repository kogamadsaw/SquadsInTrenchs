using UnityEngine;

public class TrenchOptions : MonoBehaviour
{
    public enum trenchType
    {
        HQ,
        Trench
    }
    [Header("Trench type")]
    public trenchType trench;

    public int ammo;
    public int soldiers;

    public string allyTagString;
    public string enemyTagString;

    public float trenchMaxHealth;
    private float _trenchHealth;

    [HideInInspector]
    public float trenchHealth
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
        Grid grid = FindAnyObjectByType<Grid>();

        if (grid != null)
        {
            Vector3Int startCell = grid.WorldToCell(transform.position);
            transform.position = grid.GetCellCenterWorld(startCell);
        }
        else
        {
            Debug.LogError("add Grid to scene");
        }

        TrenchSide();

        switch (trench)
        {
            case trenchType.HQ:
                trenchMaxHealth = 100;
                break;

            case trenchType.Trench:
                trenchMaxHealth = 10;
                break;
        }

        trenchHealth = trenchMaxHealth;
    }

    private void Update()
    {
        if (trenchHealth > trenchMaxHealth) trenchHealth = trenchMaxHealth;
        else if (trenchHealth < 0) trenchHealth = 0;
    }

    public void TrenchSide()
    {
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
}
