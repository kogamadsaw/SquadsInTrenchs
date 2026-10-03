using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Selection : MonoBehaviour
{
    [Header("An object to display selection")]
    [SerializeField] private GameObject _selection;
    private SpriteRenderer _selectionDisplay;
    private Transform _selectionPos;

    [Header("An object to display selected cell")]
    [SerializeField] private GameObject _selectedCell;
    private SpriteRenderer _selectedCellDisplay;
    private Transform _selectedCellPos;

    [Header("An object to display blocked cell")]
    [SerializeField] private GameObject _blockedCellPrefab;
    [SerializeField] private Transform _blockedCellParent;
    [SerializeField] private float _updateInterval = 0.1f;

    private Grid _grid;
    private SquadTargetPosition _squadTargetPos;

    private List<GameObject> _activeForbiddenCells = new List<GameObject>();
    private float _updateTimer = 0f;

    void Awake()
    {
        _grid = FindAnyObjectByType<Grid>();

        if (_grid == null)
        {
            Debug.LogError("add Grid to scene");
        }

        _selectionDisplay = _selection.GetComponent<SpriteRenderer>();
        _selectionPos = _selection.GetComponent<Transform>();

        if (_selection == null)
        {
            Debug.LogError("Add selection object");
        }

        _selectedCellDisplay = _selectedCell.GetComponent<SpriteRenderer>();
        _selectedCellPos = _selectedCell.GetComponent<Transform>();

        if (_selectedCell == null)
        {
            Debug.LogError("Add Selected cell object");
        }

        _selectionDisplay.enabled = false;
        _selectedCellDisplay.enabled = false;
    }

    private void Start()
    {
        _squadTargetPos = GetComponent<SquadTargetPosition>();
    }

    public void SelectionMoving()
    {
        _selectionPos.position = _squadTargetPos.currentHoverCellWorldPos;

        RaycastHit2D hit = Physics2D.Raycast(_squadTargetPos.mouseWorldPos, Vector2.zero);

        if (hit.collider != null)
        {
            SquadMovement struckSquad = hit.collider.GetComponentInParent<SquadMovement>();
            if (struckSquad != null && !struckSquad.isMoving)
            {
                _selectionDisplay.color = new Color32(0, 255, 0, 100);
                return;
            }
        }

        _selectionDisplay.color = new Color32(0, 176, 255, 100);
    }

    public void SelectedCellPosition()
    {
        _selectedCellPos.position = _squadTargetPos.cellWorldPos;
    }

    public void DisplaySelection()
    {
        _selectionDisplay.enabled = _squadTargetPos.isSelected;
    }

    public void DisplaySelectedCell()
    {
        _selectedCellDisplay.enabled = _squadTargetPos.isCellSelected;
    }

    public bool IsCellOccupied(Vector3 targetPos)
    {
        SquadMovement[] allSquads = FindObjectsByType<SquadMovement>();

        foreach (SquadMovement squad in allSquads)
        {
            if (squad.transform.position == targetPos || squad.targetWorldPos == targetPos)
            {
                return true;
            }
        }

        return false;
    }

    public void ShowBlockedCellsDynamic()
    {
        _updateTimer += Time.deltaTime;

        if (_updateTimer < _updateInterval) return;

        _updateTimer = 0f;
        ClearBlockedCells();

        SquadMovement[] allSquads = FindObjectsByType<SquadMovement>();
        SquadMovement currentSquad = _squadTargetPos.squadScript;

        foreach (SquadMovement squad in allSquads)
        {
            if (squad == currentSquad) continue;

            if (squad.isMoving)
            {
                Vector3Int targetCell = _grid.WorldToCell(squad.targetWorldPos);
                Vector3 cellCenterPos = _grid.GetCellCenterWorld(targetCell);

                GameObject forbiddenVisual = Instantiate(_blockedCellPrefab, cellCenterPos, Quaternion.identity, _blockedCellParent);
                _activeForbiddenCells.Add(forbiddenVisual);
            }
        }
    }

    public void ClearBlockedCells()
    {
        foreach (GameObject cell in _activeForbiddenCells)
        {
            if (cell != null) Destroy(cell);
        }
        _activeForbiddenCells.Clear();
    }
}
