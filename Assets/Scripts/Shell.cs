using UnityEngine;

public class Shell : MonoBehaviour
{
    private float _distanceToFly;
    private float _distanceTolerance;
    private string _targetTag;

    private float _armorPenetration;

    void FixedUpdate()
    {
        if (_distanceToFly <= 0)
        {
            Destroy(gameObject);

        }
        else _distanceToFly -= Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_targetTag != null)
        {
            if (collision.CompareTag(_targetTag))
            {
                SquadOptions squadOptions = collision.GetComponentInParent<SquadOptions>();

                if (squadOptions != null)
                {
                    if (_armorPenetration > squadOptions.unitArmor) squadOptions.squadHealth -= _armorPenetration - squadOptions.unitArmor;

                    Destroy(gameObject);
                }
            }
        }
    }

    public void Init(string targetTag, float distance, float speed, float distanceToleranceMin, float distanceToleranceMax, float armorPenetration)
    {
        _targetTag = targetTag;

        _distanceTolerance = Random.Range(distanceToleranceMin, distanceToleranceMax);
        _distanceToFly = (distance / speed) * _distanceTolerance;

        _armorPenetration = armorPenetration;
    }
}
