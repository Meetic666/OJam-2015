using UnityEngine;
using System.Collections;

public class FallApart : MonoBehaviour
{
    public static void FallApartHelper(GameObject obj, GameObject effectPrefab, Vector3 pos, float radius, float force)
    {
        obj.AddComponent<TimedLife>();

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            Transform child = obj.transform.GetChild(i);
            child.gameObject.AddComponent<Rigidbody>();
            child.gameObject.AddComponent<SphereCollider>();
            child.gameObject.AddComponent<TimedLife>();
            ExplodingElement element = child.gameObject.AddComponent<ExplodingElement>();
            if (effectPrefab != null)
            {
                element.m_CollisionEffectsPrefab = effectPrefab;
            }
            element.Explode(pos, radius, force);
            FallApartHelper(child.gameObject, effectPrefab, pos, radius, force);
        }
        obj.transform.DetachChildren();
    }
}