using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform _follower;
    [SerializeField] Transform _realCam;

    [SerializeField] float minClampAngle;
    [SerializeField] float maxClampAngle;
    [SerializeField] float sensitivity;

    [SerializeField] float followSpeed;
    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;

    [SerializeField] float smmothness;

    float rotX;
    float rotY;
    float finalDis;
    bool _isMouseVisible = false;

    Vector3 dirNormal;
    Vector3 finalDir;

    void Start()
    {
        rotX = transform.localEulerAngles.x;
        rotY = transform.localEulerAngles.y;

        dirNormal = _realCam.localPosition.normalized;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _isMouseVisible = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _isMouseVisible = true;
        }
        if (_isMouseVisible)
            return;

        rotX += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * -1;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -minClampAngle, maxClampAngle);

        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _follower.position, Time.deltaTime * followSpeed);
        finalDir = transform.TransformPoint(dirNormal * maxDistance);

        //Debug.DrawLine(transform.position, finalDir, Color.green, 5);
        RaycastHit hit;
        if (Physics.Linecast(transform.position, finalDir, out hit))
        {
            finalDis = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDis = maxDistance;
        }

        _realCam.localPosition = Vector3.Lerp(_realCam.localPosition, dirNormal * finalDis, Time.deltaTime * smmothness);
    }
}
