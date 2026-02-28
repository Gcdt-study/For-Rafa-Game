using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    public float minX = -48f;
    public float maxX = 48f;
    public float minY = -48f;
    public float maxY = 48f;

    void Update()
    {
        Vector3 pos = transform.position;

        if (pos.x > maxX) pos.x = minX;
        else if (pos.x < minX) pos.x = maxX;

        if (pos.y > maxY) pos.y = minY;
        else if (pos.y < minY) pos.y = maxY;

        transform.position = pos;
    }
}
