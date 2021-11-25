using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private CinemachineTransposer camTransposer;
    
    public void Init(Transform target, Vector3 cameraOffset)
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        cam.Follow = target;
        cam.LookAt = target;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = cameraOffset;
    }

    //[SerializeField]
    //private Transform target;
    //[SerializeField]
    //private float minDistance = 2.0f;
    //[SerializeField]
    //private float maxDistance = 20.0f;
    //
    //private float xSpeed = 250.0f;
    //private float ySpeed = 120.0f;
    //private float yMinLimit = 5.0f;
    //private float yMaxLimit = 80.0f;
    //private float wheelSpeed = 500.0f;
    //private float x, y;
    //private float distance;
    //private Vector3 offset;
    //
    //private void Awake()
    //{
    //    //distance = Mathf.Lerp(minDistance, maxDistance, 0.5f);
    //    offset = transform.position;
    //
    //    Vector3 angles = transform.eulerAngles;
    //    x = angles.y;
    //    y = angles.x;
    //}
    //
    //private void Update()
    //{
    //    if (!target)
    //    {
    //        return;
    //    }
    //
    //    //if (Input.GetMouseButton(1))
    //    //{
    //    //    UpdateRotate();
    //    //}
    //
    //    UpdatePosition();
    //}
    //
    //private void UpdateRotate()
    //{
    //    x += Input.GetAxisRaw("Mouse X") * xSpeed * Time.deltaTime;
    //    y -= Input.GetAxisRaw("Mouse Y") * ySpeed * Time.deltaTime;
    //
    //    y = ClampAngle(y, yMinLimit, yMaxLimit);
    //
    //    transform.rotation = Quaternion.Euler(y, x, 0);
    //}
    //
    //private float ClampAngle(float angle, float min, float max)
    //{
    //    if (angle < -360) angle += 360;
    //    if (angle > 360) angle -= 360;
    //
    //    return Mathf.Clamp(angle, min, max);
    //}
    //
    //private void UpdatePosition()
    //{
    //    //distance -= Input.GetAxisRaw("Mouse ScrollWheel") * wheelSpeed * Time.deltaTime;
    //    //distance = Mathf.Clamp(distance, minDistance, maxDistance);
    //
    //    //transform.position = target.position + transform.rotation * Vector3.back * distance;
    //    transform.position = target.position + offset;
    //}
}
