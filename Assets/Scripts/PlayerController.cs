//Anthony Smiderle
//100695532
//2022/02/07
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [Range(0.1f, 1.0f)]
    [SerializeField] float movementFalloffPercent = 0.1f;
    [SerializeField] float movementFalloffThreshold = 1.0f;
    [SerializeField] float movementSpeed = 1.0f;
    [SerializeField] bool hasMomentum = false;
    [SerializeField] float momentumBuildupSpeed = 0.25f;
    [Tooltip("Momentum lerps between movementFalloffPercent and this value, then uses the resulting value to add momentum to the player.")]
    [SerializeField] float maxMomentum = 0.1f;
    [SerializeField] float jumpScalar = 1.0f;
    [Range(1.0f, 20.0f)]
    [SerializeField] float mouseSensitivity = 1.0f;
    [Tooltip("The upper and lower limits of the camera's rotation on the local x axis.")]
    [SerializeField] float upDownLimit = 90.0f;

    [Header("References")]
    [SerializeField] Transform cameraTransform = null;
    [SerializeField] new Rigidbody rigidbody = null;


    Vector2 _moveVec = new Vector2();
    [HideInInspector] public bool doMovementFalloff = true;
    float _momentumTValue = 0.0f;
    new Camera camera = null;
    bool _canJump = false;

    Vector2 _lookVec = new Vector2();
    float _verticalAngle = 0.0f;
    float _horizontalAngle = 0.0f;
    [HideInInspector] public bool canLook = true;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0.0f, -9.8f * 3.0f, 0.0f);

        camera = cameraTransform.gameObject.GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        var m = mouseSensitivity / 10.0f;
        _verticalAngle -= _lookVec.y * m;
        _verticalAngle = Mathf.Clamp(_verticalAngle, -upDownLimit, upDownLimit);
        _horizontalAngle += _lookVec.x * m;
        cameraTransform.localRotation = Quaternion.Euler(_verticalAngle, 0.0f, 0.0f);
        transform.localRotation = Quaternion.Euler(0.0f, _horizontalAngle, 0.0f);

        if (_moveVec.magnitude == 0.0f)
        {
            _momentumTValue -= Time.deltaTime * momentumBuildupSpeed * 2.0f;
            if (_momentumTValue <= 0.0f)
                _momentumTValue = 0.0f;
            return;
        }
        _momentumTValue += Time.deltaTime * momentumBuildupSpeed;
        if (_momentumTValue >= 1.0f)
            _momentumTValue = 1.0f;

    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(transform.forward * _moveVec.y * movementSpeed, ForceMode.Impulse);
        rigidbody.AddForce(transform.right * _moveVec.x * movementSpeed, ForceMode.Impulse);

        if (!doMovementFalloff)
            return;
        //ignore y component
        var vel = rigidbody.velocity;
        vel.y = 0.0f;
        float falloff = movementFalloffPercent;
        if (hasMomentum)
        {
            falloff = Mathf.Lerp(movementFalloffPercent, maxMomentum, _momentumTValue);
            camera.fieldOfView = Mathf.Lerp(60.0f, 90.0f, _momentumTValue);
        }

        if (vel.magnitude > movementFalloffThreshold)
            rigidbody.AddForce(-vel * falloff, ForceMode.Impulse);
        else if (vel.magnitude < movementFalloffThreshold)
            rigidbody.velocity = Vector3.zero;
    }

    public void OnMove(CallbackContext ctx)
    {
        _moveVec = ctx.ReadValue<Vector2>();
    }

    public void OnJump(CallbackContext ctx)
    {
        if (!_canJump)
            return;
        rigidbody.AddForce(Vector3.up * jumpScalar, ForceMode.Impulse);
    }

    public void OnLook(CallbackContext ctx)
    {
        if (!canLook)
            return;
        _lookVec = ctx.ReadValue<Vector2>();
    }
}
