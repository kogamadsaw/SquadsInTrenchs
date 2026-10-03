using UnityEngine;

public class SquadMovement : MonoBehaviour
{
    private SquadOptions _squadOptions;

    //Where the squad needs to go
    [HideInInspector] public Vector3 targetWorldPos;

    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool stopMoving = true;

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
    }

    private void Start()
    {
        _squadOptions = GetComponent<SquadOptions>();
    }

    void Update()
    {
        if (isMoving)
        {
            MoveSquad();
        }
    }

    private void MoveSquad()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, _squadOptions.speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.001f)
        {
            transform.position = targetWorldPos;
            isMoving = false;
            stopMoving = true;
        }
    }
}
