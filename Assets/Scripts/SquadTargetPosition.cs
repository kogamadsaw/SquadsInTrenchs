using UnityEngine;

public class SquadTargetPosition : MonoBehaviour
{
    [Header("The world Map")]
    [SerializeField] private GameObject _moveBorder;

    [HideInInspector] public SquadMovement squadScript;
    private SquadOptions _squadUnit;

    private Selection _selection;
    private Grid _grid;

    [HideInInspector] public bool isSelected;
    [HideInInspector] public bool isCellSelected;
    private bool _canSelectCell;

    private Vector3 _targetWorldPos;

    [HideInInspector] public Vector3 mouseWorldPos;
    [HideInInspector] public Vector3 currentHoverCellWorldPos;
    [HideInInspector] public Vector3 cellWorldPos;

    // Границы карты
    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;

    void Awake()
    {
        // Расчет границ карты
        _minY = _moveBorder.transform.localScale.y / 2 * -1;
        _maxY = _moveBorder.transform.localScale.y / 2;
        _minX = _moveBorder.transform.localScale.x / 2 * -1;
        _maxX = _moveBorder.transform.localScale.x / 2;

        _grid = FindAnyObjectByType<Grid>();
        if (_grid == null)
        {
            Debug.LogError("add Grid to scene");
        }
    }

    private void Start()
    {
        _selection = gameObject.GetComponent<Selection>();
    }

    void Update()
    {
        MouseWorldPos();
        _selection.SelectionMoving();

        if (isSelected)
        {
            _selection.ShowBlockedCellsDynamic();
        }

        if (isCellSelected && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            squadScript.targetWorldPos = _targetWorldPos;
            squadScript.isMoving = true;

            isSelected = false;
            isCellSelected = false;
            _canSelectCell = false;

            _selection.DisplaySelection();
            _selection.DisplaySelectedCell();
            _selection.ClearBlockedCells();
        }

        if (Input.GetMouseButtonDown(0))
        {
            bool hadSelectionBeforeClick = isSelected;

            FindingSquad();
            _selection.DisplaySelection();

            if (!isSelected)
            {
                _selection.ClearBlockedCells();
                isCellSelected = false;
                _selection.DisplaySelectedCell();
            }
            else
            {
                if (!hadSelectionBeforeClick)
                {
                    _canSelectCell = false;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isSelected && _canSelectCell)
            {
                GetPosToMove();
                _selection.DisplaySelectedCell();
            }

            if (isSelected)
            {
                _canSelectCell = true;
            }
        }
    }

    void MouseWorldPos()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3Int hoverCell = _grid.WorldToCell(mouseWorldPos);
        currentHoverCellWorldPos = _grid.GetCellCenterWorld(hoverCell);
    }

    void FindingSquad()
    {
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.GetComponentInParent<SquadOptions>() != null)
        {
            if(hit.collider.GetComponentInParent<SquadOptions>().side == SquadOptions.sideType.Ally) 
            { 
                squadScript = hit.collider.GetComponentInParent<SquadMovement>();
                _squadUnit = hit.collider.GetComponentInParent<SquadOptions>();

                if (squadScript != null && (!squadScript.isMoving || (squadScript.isMoving && _squadUnit.canDoWhileWalking)))
                {
                    isSelected = true;

                    if (squadScript.isMoving)
                    {
                        _targetWorldPos = squadScript.targetWorldPos;

                        Vector3Int currentTargetCell = _grid.WorldToCell(_targetWorldPos);
                        cellWorldPos = _grid.GetCellCenterWorld(currentTargetCell);

                        _selection.SelectedCellPosition();

                        isCellSelected = true;
                        _selection.DisplaySelectedCell();
                        _canSelectCell = true;
                    }
                }
            }
        }
    }

    private void GetPosToMove()
    {
        if (mouseWorldPos.x >= _minX && mouseWorldPos.x <= _maxX && mouseWorldPos.y >= _minY && mouseWorldPos.y <= _maxY)
        {
            Vector3Int clickedCell = _grid.WorldToCell(mouseWorldPos);
            Vector3 temporaryTarget = _grid.GetCellCenterWorld(clickedCell);

            if (!_selection.IsCellOccupied(temporaryTarget) && _canSelectCell)
            {
                _targetWorldPos = temporaryTarget;
                cellWorldPos = temporaryTarget;

                isCellSelected = true;
                _selection.SelectedCellPosition();
            }
        }
    }
}
