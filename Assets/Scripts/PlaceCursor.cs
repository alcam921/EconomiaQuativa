using UnityEngine;

public class PlaceCursor : MonoBehaviour
{
    public Transform target;


    void Update()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp (transform.rotation, rotation, 2f * Time.deltaTime);
    }
}
