using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float orbitSpeed = 10.0f;
    public Vector3 orbitAxis = Vector3.up;

    private float currentAngleX = 0f;
    private float currentAngleY = 20f;

    void Update()
    {
        if (target == null) return;

        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.middleButton.isPressed)
        {
            Vector2 delta = mouse.delta.ReadValue();
            currentAngleX += delta.x * orbitSpeed * Time.deltaTime;
            currentAngleY -= delta.y * orbitSpeed * Time.deltaTime;
            currentAngleY = Mathf.Clamp(currentAngleY, -80f, 80f);
        }

        Quaternion rotation = Quaternion.Euler(currentAngleY, currentAngleX, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}

