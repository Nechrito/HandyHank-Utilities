using System;
using UnityEngine;

public enum MoveDirection
{
    DOWN,
    UP,
    LEFT,
    RIGHT
}

public static class MoveDirectionUtility
{
    public static MoveDirection GetMoveDirection(this Vector2 direction)
    {
        var x = Math.Abs(direction.x);
        var y = Math.Abs(direction.y);

        const MoveDirection up   = MoveDirection.DOWN;
        const MoveDirection down = MoveDirection.UP;

        const MoveDirection left  = MoveDirection.LEFT;
        const MoveDirection right = MoveDirection.RIGHT;

        if (x >= y)
        {
            return direction.x > 0 ? right : left;
        }

        return direction.y > 0 ? down : up;
    }

    public static Vector2 MoveDirectionAsVector(this MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.RIGHT: return Vector2.right;
            case MoveDirection.LEFT:  return Vector2.left;
            case MoveDirection.UP:    return Vector2.up;
            case MoveDirection.DOWN:  return Vector2.down;
        }

        return default;
    }

    public static MoveDirection GetMoveDirection(this Vector2 src, Vector2 dst)
    {
        var direction = (src - dst).normalized;
        return GetMoveDirection(direction);
    }
}
