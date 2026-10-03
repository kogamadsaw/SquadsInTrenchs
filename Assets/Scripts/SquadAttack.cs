using Unity.VisualScripting;
using UnityEngine;

public class SquadAttack : MonoBehaviour
{
    private SquadOptions _squadOptions;
    private SquadMovement _squad;

    [Header("Attack & Detection Settings")]
    [SerializeField] private float _detectionRadius = 5f;

    [SerializeField] private LayerMask _squadLayer;

    private Transform _currentTarget;

    private Vector3 _distance;

    private bool _rotationEnded;
    private float _randAngle;

    [Header("Weapon & Shooting Settings")]
    [SerializeField] private string _shellParentTag;
    private GameObject _shellParentObj;
    private Transform _shellParent;
    [SerializeField] private Transform _firePoint;

    private float _reloadTimer;

    void Awake()
    {
        _shellParentObj = GameObject.FindWithTag(_shellParentTag);
        _shellParent = _shellParentObj.GetComponent<Transform>();
    }

    void Start()
    {
        _squad = GetComponentInParent<SquadMovement>();
        _squadOptions = GetComponentInParent<SquadOptions>();

        _randAngle = Random.Range(_squadOptions.rotationTolerance[0], _squadOptions.rotationTolerance[1]);

        _reloadTimer = _squadOptions.reloadingTime;
    }

    void Update()
    {
        _reloadTimer += Time.deltaTime;

        if (_squad != null && _squad.isMoving && !_squadOptions.canDoWhileWalking) return;

        FindClosestEnemy();

        if (_currentTarget != null || (_currentTarget != null && _squad.stopMoving))
        {
            RotateTowardsTarget();
        }

        if (_rotationEnded)
        {
            Attack();
        }
    }

    private void FindClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _squadOptions.detectionRadius, _squadLayer);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        SquadOptions.sideType enemySide = (_squadOptions.side == SquadOptions.sideType.Ally)
            ? SquadOptions.sideType.Enemy
            : SquadOptions.sideType.Ally;

        if (_squadOptions.side == SquadOptions.sideType.Neutral)
        {
            _currentTarget = null;
            return;
        }

        foreach (Collider2D col in colliders)
        {
            if (col.transform.IsChildOf(transform) || col.transform == transform) continue;

            SquadOptions targetOptions = col.GetComponentInParent<SquadOptions>();

            if (targetOptions != null && targetOptions.side == enemySide)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, col.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = targetOptions.transform;
                }
            }
        }

        _currentTarget = closestEnemy;
    }

    private void RotateTowardsTarget()
    {
        _distance = _currentTarget.position - transform.position;
        _distance.z = 0f;

        if (_distance != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(_distance.y, _distance.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);

            targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _squadOptions.rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.001f)
            { 
                _rotationEnded = true;
            }
            else 
            {
                _rotationEnded = false;
            }
        }

        if (_squad.stopMoving) _squad.stopMoving = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }

    private void Attack()
    {
        if (_reloadTimer < _squadOptions.reloadingTime || _currentTarget == null) return;

        _reloadTimer = 0f;

        Quaternion rotationTolerance = transform.rotation * Quaternion.Euler(0f, 0f, _randAngle);

        Vector3 spawnPosition = (_firePoint != null) ? _firePoint.position : transform.position;

        GameObject shell;

        if (_distance.magnitude > 10f) shell = Instantiate(_squadOptions.shell, spawnPosition, rotationTolerance, _shellParent);
        else shell = Instantiate(_squadOptions.shell, spawnPosition, rotationTolerance, _shellParent);

        Shell shellScript = shell.GetComponent<Shell>();

        string targetTag = "";

        if (_squadOptions.side == SquadOptions.sideType.Ally) targetTag = _squadOptions.enemiesTag;
        else if (_squadOptions.side == SquadOptions.sideType.Enemy) targetTag = _squadOptions.alliesTag;

        shellScript.Init(targetTag, _distance.magnitude, _squadOptions.shellSpeed, _squadOptions.distanceToleranceMin, _squadOptions.distanceToleranceMax, _squadOptions.armorPenetration);

        Rigidbody2D rb = shell.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = shell.transform.right * _squadOptions.shellSpeed;
        }
        else Debug.LogError("Add Rigidbody2D to shell");

        _randAngle = Random.Range(_squadOptions.rotationTolerance[0], _squadOptions.rotationTolerance[1]);

        _rotationEnded = false;
    }
}
