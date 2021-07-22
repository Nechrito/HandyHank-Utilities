using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GenericExtensions
{
    public static void Swap<T>(this T[] array, int a, int b)
    {
        var x = array[a];
        array[a] = array[b];
        array[b] = x;
    }

    public static void Swap<T>(ref T a, ref T b)
    {
        var x = a;
        a = b;
        b = x;
    }

    public static float Snap(this float val, float round)
    {
        return round * Mathf.Round(val / round);
    }

    public static bool IsWorldPointInViewport(this Camera camera, Vector3 point)
    {
        var position = camera.WorldToViewportPoint(point);

        return position.x > 0 && position.y > 0;
    }

    public static bool HasMethod(this object target, string methodName)
    {
        return target.GetType().GetMethod(methodName) != null;
    }

    public static bool HasField(this object target, string fieldName)
    {
        return target.GetType().GetField(fieldName) != null;
    }

    public static bool HasProperty(this object target, string propertyName)
    {
        return target.GetType().GetProperty(propertyName) != null;
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var toGet = gameObject.GetComponent<T>();
        return toGet != null ? toGet : gameObject.AddComponent<T>();
    }

    public static T GetOrAddComponent<T>(this Component component) where T : Component
    {
        var toGet = component.gameObject.GetComponent<T>();
        return toGet != null ? toGet : component.gameObject.AddComponent<T>();
    }

    public static bool HasComponent<T>(this GameObject gameObject)
    {
        return gameObject.GetComponent<T>() != null;
    }

    public static List<Transform> GetObjectsOfLayerInChilds(this GameObject gameObject, int layer)
    {
        var list = new List<Transform>();
        CheckChildsOfLayer(gameObject.transform, layer, list);

        return list;
    }

    public static List<Transform> GetObjectsOfLayerInChilds(this GameObject gameObject, string layer)
    {
        return gameObject.GetObjectsOfLayerInChilds(LayerMask.NameToLayer(layer));
    }

    public static List<Transform> GetObjectsOfLayerInChilds(this Component component, string layer)
    {
        return component.GetObjectsOfLayerInChilds(LayerMask.NameToLayer(layer));
    }

    public static List<Transform> GetObjectsOfLayerInChilds(this Component component, int layer)
    {
        return component.gameObject.GetObjectsOfLayerInChilds(layer);
    }

    private static void CheckChildsOfLayer(Transform transform, int layer, List<Transform> childsCache)
    {
        foreach (Transform t in transform)
        {
            CheckChildsOfLayer(t, layer, childsCache);

            if (t.gameObject.layer != layer)
                continue;
            childsCache.Add(t);
        }
    }

    public static void SetBodyState(this Rigidbody body, bool state)
    {
        body.isKinematic      = !state;
        body.detectCollisions = state;
    }
    
    public static T[] FindObjectsOfInterface<T>() where T : class
    {
        var monoBehaviours = Object.FindObjectsOfType<Transform>();
        return monoBehaviours.Select(behaviour => behaviour.GetComponent(typeof(T))).OfType<T>().ToArray();
    }

    public static ComponentOfInterface<T>[] FindObjectsOfInterfaceAsComponents<T>() where T : class
    {
        return Object.FindObjectsOfType<Component>()
                     .Where(c => c is T)
                     .Select(c => new ComponentOfInterface<T>(c, c as T)).ToArray();
    }

    public readonly struct ComponentOfInterface<T>
    {
        public readonly Component Component;
        public readonly T Interface;

        public ComponentOfInterface(Component component, T @interface)
        {
            Component = component;
            Interface = @interface;
        }
    }

    public static T[] OnePerInstance<T>(this T[] components) where T : Component
    {
        if (components == null || components.Length == 0)
            return null;

        return components.GroupBy(h => h.transform.GetInstanceID()).Select(g => g.First()).ToArray();
    }

    public static RaycastHit2D[] OneHitPerInstance(this RaycastHit2D[] hits)
    {
        if (hits == null || hits.Length == 0)
            return null;

        return hits.GroupBy(h => h.transform.GetInstanceID()).Select(g => g.First()).ToArray();
    }


    public static Collider2D[] OneHitPerInstance(this Collider2D[] hits)
    {
        if (hits == null || hits.Length == 0)
            return null;

        return hits.GroupBy(h => h.transform.GetInstanceID()).Select(g => g.First()).ToArray();
    }

    public static List<Collider2D> OneHitPerInstanceList(this Collider2D[] hits)
    {
        if (hits == null || hits.Length == 0)
            return null;

        return hits.GroupBy(h => h.transform.GetInstanceID()).Select(g => g.First()).ToList();
    }

}