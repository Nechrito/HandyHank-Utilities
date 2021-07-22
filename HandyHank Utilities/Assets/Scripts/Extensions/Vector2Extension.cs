using UnityEngine;

public static class Vector2Extension
{
    public static Vector2 GetDirectionTo(this Vector2 from, Vector2 to, float distance = 1)
    {
        return Vector2.MoveTowards(from, to, distance * Time.fixedDeltaTime);
    }

    public static Vector2 GetDirectionTo(this Vector3 from, Vector3 to, float distance = 1)
    {
        return Vector2.MoveTowards(from, to, distance * Time.fixedDeltaTime);
    }

    public static Vector2 GetRandomized(float radius = 1)
    {
        return new Vector2(Random.Range(-radius, radius), 
                           Random.Range(-radius, radius));
    }

    public static Vector2 GetRandomized(this Vector2 src, float radius = 1)
    {
        return src + GetRandomized();
    }

    public static Vector3 GetRandomized(this Vector3 src, float radius = 1)
    {
        return src + new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius), Random.Range(-radius, radius));
    }

#region Convert

    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector3 ToVector3(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y);
    }

    public static Vector2 ToVector2(this Vector2Int vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector3 ToVector3(this Vector3Int vector)
    {
        return new Vector3(vector.x, vector.y);
    }

#endregion


#region Offset

    public static Vector3 Offset(this Vector3 vector, Vector2 offset)
    {
        return new Vector3(vector.x + offset.x, vector.y + offset.y, vector.z);
    }


    public static Vector3 OffsetX(this Vector3 vector, float x)
    {
        return new Vector3(vector.x + x, vector.y, vector.z);
    }

    public static Vector2 OffsetX(this Vector2 vector, float x)
    {
        return new Vector2(vector.x + x, vector.y);
    }

    public static void OffsetX(this Transform transform, float x)
    {
        transform.position = transform.position.OffsetX(x);
    }


    public static Vector2 OffsetY(this Vector2 vector, float y)
    {
        return new Vector2(vector.x, vector.y + y);
    }

    public static Vector3 OffsetY(this Vector3 vector, float y)
    {
        return new Vector3(vector.x, vector.y + y, vector.z);
    }

    public static void OffsetY(this Transform transform, float y)
    {
        transform.position = transform.position.OffsetY(y);
    }


    public static Vector3 OffsetZ(this Vector3 vector, float z)
    {
        return new Vector3(vector.x, vector.y, vector.z + z);
    }

    public static void OffsetZ(this Transform transform, float z)
    {
        transform.position = transform.position.OffsetZ(z);
    }


    public static Vector3 OffsetXY(this Vector3 vector, float x, float y)
    {
        return new Vector3(vector.x + x, vector.y + y, vector.z);
    }

    public static void OffsetXY(this Transform transform, float x, float y)
    {
        transform.position = transform.position.OffsetXY(x, y);
    }

    public static Vector2 OffsetXY(this Vector2 vector, float x, float y)
    {
        return new Vector2(vector.x + x, vector.y + y);
    }

    public static Vector3 OffsetXZ(this Vector3 vector, float x, float z)
    {
        return new Vector3(vector.x + x, vector.y, vector.z + z);
    }

    public static void OffsetXZ(this Transform transform, float x, float z)
    {
        transform.position = transform.position.OffsetXZ(x, z);
    }


    public static Vector3 OffsetYZ(this Vector3 vector, float y, float z)
    {
        return new Vector3(vector.x, vector.y + y, vector.z + z);
    }

    public static void OffsetYZ(this Transform transform, float y, float z)
    {
        transform.position = transform.position.OffsetYZ(y, z);
    }
    #endregion

    
#region Snap

    public static Vector3 SnapValue(this Vector3 val, float snapValue)
    {
        return new Vector3(val.x.Snap(snapValue), val.y.Snap(snapValue), val.z.Snap(snapValue));
    }

    public static Vector2 SnapValue(this Vector2 val, float snapValue)
    {
        return new Vector2(val.x.Snap(snapValue), val.y.Snap(snapValue));
    }

    public static void SnapPosition(this Transform transform, float snapValue)
    {
        transform.position = transform.position.SnapValue(snapValue);
    }

    public static Vector2 SnapToOne(this Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    }

    public static Vector3 SnapToOne(this Vector3 vector)
    {
        return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
    }

#endregion
}
