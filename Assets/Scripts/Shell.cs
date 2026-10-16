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
                    if (_armorPenetration > squadOptions.unitArmor && squadOptions.amount.Count != 0)
                    {
                        int a = Random.Range(0, squadOptions.amount.Count);

                        squadOptions.amount[a].unitHealth -= _armorPenetration - squadOptions.unitArmor;
                        if (squadOptions.amount[a].unitHealth <= 0)
                        {
                            squadOptions.amount.RemoveAt(a);
                            squadOptions.CheckUnits();
                        }
                    }

                    Destroy(gameObject);
                }
            }
        }
    }

    public void Init(string targetTag, float distance, float speed, float distanceTolerance, float armorPenetration)
    {
        _targetTag = targetTag;

        _distanceTolerance = Random.Range(1 - (distanceTolerance / 10), 1 + (distanceTolerance / 10));
        _distanceToFly = (distance / speed) * _distanceTolerance;

        _armorPenetration = armorPenetration;
    }
}
