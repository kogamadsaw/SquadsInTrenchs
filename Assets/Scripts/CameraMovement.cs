using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera _cam;

    [Header("Camera speed")]
    [Tooltip("Default camera speed")]
    [SerializeField] private float _camSpeed = 4;

    [Tooltip("How strong the acceleration will be")]
    [SerializeField] private float _camSpeedBoost = 25;

    [Header("Camera scaling")]
    [Tooltip("Power of zoom")]
    [SerializeField] private float _camZoomSpeed = 10;

    [Tooltip("How far the scaling will be")]
    [SerializeField] private float _maxZoom = 20;

    [Tooltip("How close the scaling will be")]
    [SerializeField] private float _minZoom = 5;

    [Header("Main map")]
    [Tooltip("Add main map")]
    [SerializeField] private GameObject _camMoveBorder;

    private float _minX = 100f;
    private float _maxX = 100f;
    private float _minY = 100f;
    private float _maxY = 100f;

    void Awake()
    {
        _cam = GetComponent<Camera>();

        if(_camMoveBorder == null)
        {
            Debug.Log("Add border for camera");
            return;
        }

        _minY = _camMoveBorder.transform.localScale.y / 2 * -1;
        _maxY = _camMoveBorder.transform.localScale.y / 2;

        _minX = _camMoveBorder.transform.localScale.x / 2 * -1;
        _maxX = _camMoveBorder.transform.localScale.x / 2;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        float camZoom = Input.GetAxis("Mouse ScrollWheel");

        if (moveX != 0 || moveY != 0)
        {
            Vector3 moveDirection = new Vector3(moveX, moveY, 0f).normalized;

            Vector3 nextPosition;

            nextPosition = transform.position + moveDirection * _camSpeed * Time.deltaTime;

            if (Input.GetKey(KeyCode.LeftShift)) {nextPosition = transform.position + moveDirection * _camSpeed * _camSpeedBoost * Time.deltaTime;}

            float camHalfHeight = _cam.orthographicSize;
            float camHalfWidth = camHalfHeight * _cam.aspect;

            float clampedX = Mathf.Clamp(nextPosition.x, _minX + camHalfWidth, _maxX - camHalfWidth);
            float clampedY = Mathf.Clamp(nextPosition.y, _minY + camHalfHeight, _maxY - camHalfHeight);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }

        if (camZoom != 0f)
        {
            float newSize = _cam.orthographicSize - camZoom * _camZoomSpeed;
            _cam.orthographicSize = Mathf.Clamp(newSize, _minZoom, _maxZoom);
        }
    }
}
