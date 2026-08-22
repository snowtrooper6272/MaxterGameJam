using UnityEngine;
using UnityEngine.InputSystem; // Добавлено пространство имен новой системы ввода

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2.0f;
    public float maxYAngle = 80.0f;

    [Header("Mouse Inertia")]
    public float inertia = 0.05f;
    public float inertiaReturn = 12f;

    [Header("Head Bob")]
    public float stepTime = 0.6f;
    public float runStepTime = 0.4f;
    public float bobAmount = 0.12f;
    public float returnSpeed = 12f;

    private float rotationX = 0.0f;
    private Vector3 startPosition;
    private float bobTimer;

    private float mouseVelocityX;
    private float mouseVelocityY;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        // 1. Получаем движение мыши в новой системе ввода
        float mouseX = 0f;
        float mouseY = 0f;

        if (Mouse.current != null)
        {
            // Умножаем на 0.05, чтобы компенсировать разницу в масштабе значений Delta мыши
            mouseX = Mouse.current.delta.x.ReadValue() * 0.05f;
            mouseY = Mouse.current.delta.y.ReadValue() * 0.05f;
        }

        mouseVelocityX = Mathf.Lerp(
            mouseVelocityX,
            mouseX * inertia,
            Time.deltaTime * inertiaReturn
        );

        mouseVelocityY = Mathf.Lerp(
            mouseVelocityY,
            mouseY * inertia,
            Time.deltaTime * inertiaReturn
        );

        transform.parent.Rotate(
            Vector3.up * (mouseX * sensitivity + mouseVelocityX)
        );

        rotationX -= mouseY * sensitivity + mouseVelocityY;
        rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);

        // 2. Получаем нажатия клавиш клавиатуры в новой системе ввода
        bool moving = false;
        bool running = false;

        if (Keyboard.current != null)
        {
            // Проверяем, нажата ли любая из клавиш движения (W, A, S, D)
            moving = Keyboard.current.wKey.isPressed ||
                     Keyboard.current.aKey.isPressed ||
                     Keyboard.current.sKey.isPressed ||
                     Keyboard.current.dKey.isPressed;

            // Проверяем бег (W + LeftShift)
            running = Keyboard.current.wKey.isPressed && Keyboard.current.leftShiftKey.isPressed;
        }

        if (moving)
        {
            bobTimer += Time.deltaTime;

            float currentStepTime = running ? runStepTime : stepTime;

            float bobY = Mathf.Sin(
                (bobTimer / currentStepTime) * Mathf.PI * 2f
            ) * bobAmount;

            transform.localPosition =
                startPosition + new Vector3(0, bobY, 0);
        }
        else
        {
            bobTimer = 0;

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                startPosition,
                Time.deltaTime * returnSpeed
            );
        }

        transform.localRotation =
            Quaternion.Euler(rotationX, 0.0f, 0.0f);
    }
}
