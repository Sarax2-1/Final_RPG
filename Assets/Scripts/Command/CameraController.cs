using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private float moveSpeed;

    [SerializeField] private Transform corner1;
    [SerializeField] private Transform corner2;

    public static CameraController instance;
    private InputAction moveAction;
    private Vector2 moveValue;

    [Header("Zoom")]
    [SerializeField] private float zoomModifier;
    [SerializeField] private float zoomSpeed;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minXAngle = 10f;
    [SerializeField] private float maxXAngle = 80f;

    private bool isRotating = false;
    private Vector2 lastMousePos;
    private float currentXAngle = 45f;

    private InputAction zoomAction;
    private Vector2 zoomValue;

    private void Awake()
    {
        instance = this;
        _camera = Camera.main;
    }

    void Start()
    {
        moveSpeed = 25f;
        zoomSpeed = 0.05f;
        moveAction = InputSystem.actions.FindAction("Move");
        zoomAction = InputSystem.actions.FindAction("Zoom");

        currentXAngle = transform.eulerAngles.x;
    }

    void Update()
    {
        MoveByKB();
        Zoom();
        MoveByMouse();
        RotateByMiddleMouse();
    }

    private void MoveByKB()
    {
        moveValue = moveAction.ReadValue<Vector2>();

        float xInput = moveValue.x;
        float zInput = moveValue.y;

        Vector3 dir = (transform.forward * zInput) + (transform.right * xInput);

        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.position = Clamp(corner1.position, corner2.position);
    }

    private void Zoom()
    {
        zoomValue = zoomAction.ReadValue<Vector2>();

        float scrollDelta = Mathf.Clamp(zoomValue.y, -1f, 1f);
        zoomModifier = scrollDelta * 5f;

        if (Keyboard.current.zKey.isPressed)
            zoomModifier = -1f;
        if (Keyboard.current.xKey.isPressed)
            zoomModifier = 1f;

        _camera.orthographicSize += zoomModifier * zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 4, 10);
    }

    private Vector3 Clamp(Vector3 lowerLeft, Vector3 topRight)
    {
        Vector3 pos = new Vector3(
            Mathf.Clamp(transform.position.x, lowerLeft.x, topRight.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, lowerLeft.z, topRight.z)
        );
        return pos;
    }

    private void MoveByMouse()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (mousePos.x >= Screen.width)
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        else if (mousePos.x <= 0)
            transform.position -= transform.right * moveSpeed * Time.deltaTime;

        if (mousePos.y >= Screen.height)
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        else if (mousePos.y <= 0)
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;

        transform.position = Clamp(corner1.position, corner2.position);
    }

    private void RotateByMiddleMouse()
    {
        if (Mouse.current.middleButton.wasPressedThisFrame)
        {
            isRotating = true;
            lastMousePos = Mouse.current.position.ReadValue();
        }

        if (Mouse.current.middleButton.wasReleasedThisFrame)
            isRotating = false;

        if (isRotating)
        {
            Vector2 currentMousePos = Mouse.current.position.ReadValue();
            Vector2 delta = currentMousePos - lastMousePos;

            transform.Rotate(Vector3.up, delta.x * rotationSpeed * Time.deltaTime, Space.World);

            currentXAngle -= delta.y * rotationSpeed * Time.deltaTime;
            currentXAngle = Mathf.Clamp(currentXAngle, minXAngle, maxXAngle);

            Vector3 euler = transform.eulerAngles;
            transform.eulerAngles = new Vector3(currentXAngle, euler.y, 0f);

            lastMousePos = currentMousePos;
        }
    }
}