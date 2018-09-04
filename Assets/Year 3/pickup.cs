using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class pickup : MonoBehaviour
{
    public Transform PickupPoint;
    public Transform ZoomPoint;
    public PostProcessVolume ZoomPostProcess;
    public FirstPersonController Controller;
    [Range(100f, 2000f)]
    public float ThrowForce;

    private Rigidbody _carrying;
    private Vector2 _rotation;
    private Quaternion _pickupRotation;
    private Transform _itemPoint;
    private float _pickupRotationSpeed = 240f;
    private float _pickupPositionSpeed = 3f;
    private DepthOfField _depthOfField;
    private bool _mustResetFire1;
    private bool _handledFire2LetGo;

    private bool IsCarrying { get { return _carrying != null; } }

    public void Start()
    {
        _depthOfField = ZoomPostProcess.sharedProfile.GetSetting<DepthOfField>();
    }

    public void Update()
    {
        var fire1 = CrossPlatformInputManager.GetAxis("Fire1") < 0 || CrossPlatformInputManager.GetButtonDown("Fire1");
        if (!_mustResetFire1 && fire1)
        {
            if (!IsCarrying)
                TryPickup();
            else
                Throw();

            _mustResetFire1 = true;
        }

        if (_mustResetFire1 && !fire1)
            _mustResetFire1 = false;

        if (IsCarrying)
        {
            var distanceFromTarget = Vector3.Distance(_carrying.position, _itemPoint.position)*4;
            var newPos = Vector3.MoveTowards(_carrying.position, _itemPoint.position,
                _pickupPositionSpeed * Time.deltaTime * Mathf.Pow(distanceFromTarget, 2));
            _carrying.position = newPos;

            var targetRotation = _pickupRotation * transform.rotation;
            if (CrossPlatformInputManager.GetAxis("Fire2") > 0)
            {
                Controller.enabled = false;
                _itemPoint = ZoomPoint;

                var input = GetRotateInput();
                _rotation.x -= input.x;
                _rotation.y = Mathf.Clamp(_rotation.y - input.y, -180, 180);
                var upDownRotation = Quaternion.AngleAxis(-_rotation.y, transform.right);
                targetRotation = upDownRotation * transform.rotation * _pickupRotation * Quaternion.Euler(0, _rotation.x, 0);

                _depthOfField.enabled.Override(true);
                _handledFire2LetGo = false;
            }
            else if (!_handledFire2LetGo && Mathf.Approximately(CrossPlatformInputManager.GetAxis("Fire2"), 0))
            {
                _pickupRotation = GetOnlyYRotation(Quaternion.Inverse(transform.rotation) * _carrying.rotation);
                _rotation = Vector2.zero;
                Controller.enabled = true;
                _itemPoint = PickupPoint;
                _depthOfField.enabled.Override(false);
                _handledFire2LetGo = true;
            }

            _carrying.rotation = Quaternion.RotateTowards(_carrying.rotation, targetRotation,
                _pickupRotationSpeed * Time.deltaTime);
        }
    }

    private static Vector2 GetRotateInput()
    {
        var mouse = new Vector2(
            CrossPlatformInputManager.GetAxis("Mouse X"),
            CrossPlatformInputManager.GetAxis("Mouse Y"));
        var joystick = new Vector2(
            CrossPlatformInputManager.GetAxis("Joystick X"),
            CrossPlatformInputManager.GetAxis("Joystick Y"));
        return (mouse + joystick) * 3;
    }

    private void TryPickup()
    {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 5);
        if (Physics.Raycast(ray, out hit, 4, ~LayerMask.NameToLayer("Pickupable")))
        {
            hit.rigidbody.isKinematic = true;
            //hit.transform.SetParent(PickupPoint, true);

            _rotation = Vector2.zero;
            _carrying = hit.rigidbody;
            _pickupRotation = GetOnlyYRotation(hit.rigidbody.rotation);
            _itemPoint = PickupPoint;
        }
    }

    private static Quaternion GetOnlyYRotation(Quaternion rotation)
    {
        return Quaternion.Euler(0, rotation.eulerAngles.y, 0);
    }

    private void Throw()
    {
        _carrying.transform.SetParent(null);
        _carrying.isKinematic = false;
        _carrying.AddForce(transform.forward * ThrowForce, ForceMode.Force);
        _carrying = null;
    }
}
