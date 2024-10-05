using UnityEngine;

public static class VectorExtensionMethods
{
	public static bool IsZero(this Vector2 value, float sqrEpsilon = Vector2.kEpsilon)
	{
		return value.sqrMagnitude <= sqrEpsilon;
	}

    public static bool IsZero(this Vector3 value, float sqrEpsilon = Vector3.kEpsilon)
    {
        return value.sqrMagnitude <= sqrEpsilon;
    }
}
