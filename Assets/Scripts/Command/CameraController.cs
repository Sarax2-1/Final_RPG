using Unity.VectorGraphics;
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

    [SerializeField] private float xInput;
    [SerializeField] private float zInput;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = 25f;
        zoomSpeed = 0.05f;
        moveAction = InputSystem.actions.FindAction("Move");
        zoomAction = InputSystem.actions.FindAction("Zoom");
    }

    // Update is called once per frame
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
        xInput = moveValue.x;
        zInput = moveValue.y;

        Vector3 dir = (transform.forward * zInput) + (transform.right * xInput);

        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.position = Clamp(corner1.position, corner2.position);
    }

    private void Zoom()
    {
        zoomValue = zoomAction.ReadValue<Vector2>();
        zoomModifier = zoomValue.y * 5f;

        if (Keyboard.current.zKey.isPressed)
            zoomModifier = -1f;
        if (Keyboard.current.xKey.isPressed)
            zoomModifier = 1f;

        _camera.orthographicSize += zoomModifier * zoomSpeed;
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, 4, 10);
    }

    private Vector3 Clamp(Vector3 lowerLeft, Vector3 topRight)
    {
        Vector3 pos = new Vector3(Mathf.Clamp(transform.position.x, lowerLeft.x, topRight.x),
            transform.position.y, Mathf.Clamp(transform.position.z, lowerLeft.z, topRight.z));

        return pos;

    }

    private void MoveByMouse()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (mousePos.x >= Screen.width)
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
        else if (mousePos.x <= 0)
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);

        if (mousePos.y >= Screen.height)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
        else if (mousePos.y <= 0)
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
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

            // หมุนซ้าย-ขวา (ไม่มี limit)
            transform.Rotate(Vector3.up, delta.x * rotationSpeed * Time.deltaTime, Space.World);

            // หมุนขึ้น-ลง (มี clamp)
            currentXAngle -= delta.y * rotationSpeed * Time.deltaTime;
            currentXAngle = Mathf.Clamp(currentXAngle, minXAngle, maxXAngle);

            // เอา Y angle เดิมไว้ แล้วใส่ X angle ที่ clamp แล้ว
            Vector3 euler = transform.eulerAngles;
            transform.eulerAngles = new Vector3(currentXAngle, euler.y, 0f);

            lastMousePos = currentMousePos;
        }
    }
}
