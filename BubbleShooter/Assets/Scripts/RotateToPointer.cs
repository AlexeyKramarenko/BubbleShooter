using UnityEngine;
using System.Collections;

public class RotateToPointer : MonoBehaviour
{
    private Vector2 direction;
    private Vector2 mousePosition;
    private float angle;

    void Update()
    {
        LimitArrowRotation();
    }

    void LimitArrowRotation()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePosition - (Vector2)transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        //Ограничение поворота стрелки при стрельбе
        if (angle < -85 && angle > -180)
        {
            angle = -85;
        }
        else if (angle <= -180 && angle >= -275)
        {
            angle = -275;
        }
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
