using UnityEngine;

public class Trench : MonoBehaviour
{
    TrenchOptions _trenchOptions;

    [SerializeField] private string _allyTagString;
    [SerializeField] private string _enemyTagString;

    private string _allyTag;
    private string _enemyTag;
    private int _alliesInTrench;
    private int _enemiesInTrench;

    void Start()
    {
        _trenchOptions = GetComponent<TrenchOptions>();

        if (gameObject.tag == _trenchOptions.trenchAllyTag) { _enemyTag = _enemyTagString; _allyTag = _allyTagString; }
        else { _enemyTag = _allyTagString; _allyTag = _enemyTagString; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_enemyTag))
        {
            _enemiesInTrench += 1;
            Debug.Log(_trenchOptions.trenchHealth);
        }
        if (collision.CompareTag(_allyTag))
        {
            _alliesInTrench += 1;
        }
    }

    private void FixedUpdate()
    {
        if (_enemiesInTrench > _alliesInTrench && _trenchOptions.trenchHealth > 0)
        {
            _trenchOptions.trenchHealth -= _enemiesInTrench * Time.fixedDeltaTime;
        }
        else if (_enemiesInTrench < _alliesInTrench && _trenchOptions.trenchHealth <= _trenchOptions.trenchMaxHealth)
        {
            _trenchOptions.trenchHealth += _alliesInTrench * Time.fixedDeltaTime;
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
}
