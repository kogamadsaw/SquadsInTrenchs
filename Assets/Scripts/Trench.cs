using UnityEngine;

public class Trench : MonoBehaviour
{
    TrenchOptions _trenchOptions;

    private string _allyTag;
    private string _enemyTag;
    private int _alliesInTrench;
    private int _enemiesInTrench;

    void Start()
    {
        _trenchOptions = GetComponentInParent<TrenchOptions>();

        _alliesInTrench += _trenchOptions.soldiers;

        ChangeTags();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
        {
            _enemiesInTrench += 1;
        }
        if (collision.CompareTag(_allyTag))
        {
            _alliesInTrench += 1;
            _alliesInTrench += _trenchOptions.soldiers;
        }
    }

    private void FixedUpdate()
    {
        if (_trenchOptions.trench == TrenchOptions.trenchType.HQ)
        {
            if (_enemiesInTrench > _alliesInTrench && _trenchOptions.trenchHealth > 0)
            {
                _trenchOptions.trenchHealth -= _enemiesInTrench * Time.fixedDeltaTime;
            }
            else if (_enemiesInTrench < _alliesInTrench && _trenchOptions.trenchHealth <= _trenchOptions.trenchMaxHealth)
            {
                _trenchOptions.trenchHealth += _alliesInTrench * Time.fixedDeltaTime;
            }

            if (_trenchOptions.trenchHealth <= 0) Destroy(gameObject);
        }
        else
        {
            if (_enemiesInTrench > _alliesInTrench && _trenchOptions.trenchHealth > 0)
            {
                _trenchOptions.trenchHealth -= _enemiesInTrench * Time.fixedDeltaTime;
            }
            else if (_enemiesInTrench < _alliesInTrench && _trenchOptions.trenchHealth <= _trenchOptions.trenchMaxHealth)
            {
                _trenchOptions.trenchHealth += _alliesInTrench * Time.fixedDeltaTime;
            }

            if (_trenchOptions.trenchHealth <= 0)
            {
                _trenchOptions.side = (_trenchOptions.side == TrenchOptions.sideType.Ally) ? TrenchOptions.sideType.Enemy : TrenchOptions.sideType.Ally;
                _trenchOptions.TrenchSide();

                ChangeTags();

                _trenchOptions.trenchHealth = 1;

                int a = _alliesInTrench;
                _alliesInTrench = _enemiesInTrench;
                _enemiesInTrench = a;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
        {
            _enemiesInTrench -= 1;
        }
        if (collision.CompareTag(_allyTag))
        {
            _alliesInTrench -= 1;
        }
    }

    private void ChangeTags()
    {
        _enemyTag = (gameObject.tag == _trenchOptions.trenchAllyTag) ? _trenchOptions.enemyTagString : _trenchOptions.allyTagString;
        _allyTag = (gameObject.tag == _trenchOptions.trenchAllyTag) ? _trenchOptions.allyTagString : _trenchOptions.enemyTagString;
    }
}
