using UnityEngine;

public class DefaultClass : MonoBehaviour
{
    protected bool NearlyEqual(float a, float b, float delta)
    {
        return Mathf.Abs(a - b) <= delta;
    }

    protected bool NearlyEqual(Vector2 a, Vector2 b, float delta)
    {
        return Vector2.Distance(a, b) <= delta;
    }

    protected bool NearlyEqual(Vector3 a, Vector3 b, float delta)
    {
        return Vector3.Distance(a, b) <= delta;
    }
}
