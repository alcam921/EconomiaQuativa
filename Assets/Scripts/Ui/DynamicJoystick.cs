using UnityEngine;

public class DynamicJoystick : MonoBehaviour
{
    public RectTransform joystickBase; // Base do joystick na UI
    public Canvas canvas; // Canvas onde o joystick está
    public float smoothSpeed = 15f; // Velocidade de transição

    private Vector3 targetPos;
    private bool moving = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 localPoint;
            RectTransform canvasRect = canvas.transform as RectTransform;

            // Converte para posição local no Canvas
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                Input.mousePosition,
                canvas.worldCamera,
                out localPoint
            );

            // Limita para metade esquerda (X <= 0) e metade inferior (Y <= 0)
            float minX = -canvasRect.rect.width / 2f;
            float maxX = -canvasRect.rect.width/4f;
            float minY = -canvasRect.rect.height / 2f;
            float maxY = -canvasRect.rect.width/10f;

            localPoint.x = Mathf.Clamp(localPoint.x, minX, maxX);
            localPoint.y = Mathf.Clamp(localPoint.y, minY, maxY);

            targetPos = localPoint;
            moving = true;
        }

        if (moving)
        {
            joystickBase.localPosition = Vector3.Lerp(
                joystickBase.localPosition,
                targetPos,
                Time.deltaTime * smoothSpeed
            );

            if (Vector3.Distance(joystickBase.localPosition, targetPos) < 0.1f)
                moving = false;
        }
    }
}
