using UnityEngine;

/// Адаптер под Meta OVR: подбор предметов по пинчу + реальному касанию подушек пальцев.
/// Ничего не трогает в ваших блоках и не использует луч (он остаётся для перемещения).
public class OVRPinchPickupAdapter : MonoBehaviour
{
    [Header("OVR")]
    [Tooltip("OVRHand с этого же объекта (OVR*HandDataSource)")]
    public OVRHand ovrHand;

    [Tooltip("Необязательно. Если знаешь, где лежит OVRSkeleton (например, объект OVRHands) — перетащи сюда.")]
    public OVRSkeleton skeletonOverride;

    [Header("Pinch / Contact")]
    [Tooltip("Порог силы пинча по OVR (0..1)")]
    [Range(0f, 1f)] public float pinchStrengthThreshold = 0.9f;

    [Tooltip("Максимальная дистанция между IndexTip и ThumbTip, считаем как 'касание' (метры)")]
    [Range(0.0f, 0.03f)] public float touchDistance = 0.012f;

    [Header("Pickup Search")]
    [Tooltip("Радиус зоны подбора вокруг кончика указательного (метры)")]
    [Range(0.01f, 0.07f)] public float pickupRadius = 0.035f;

    [Tooltip("Слои, на которых лежат подбираемые предметы")]
    public LayerMask pickupMask = ~0;

    [Tooltip("Подстраховочный SphereCast вперёд от пальца (метры)")]
    public float pickupReach = 0.15f;

    [Header("Debug")]
    public bool logDebug = false;
    public bool drawGizmos = false;

    // runtime
    Transform indexTip;
    Transform thumbTip;
    bool wasPinching;
    OVRSkeleton cachedSkeleton;

    void Awake()
    {
        if (!ovrHand) ovrHand = GetComponent<OVRHand>();
    }

    void Update()
    {
        EnsureJoints();

        if (!ovrHand) return;

        // 1) Пинч из OVR (Index)
        bool isOVRPinching =
            ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Index) &&
            ovrHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) >= pinchStrengthThreshold;

        // 2) Реальное касание подушек
        bool tipsTouching = false;
        Vector3 indexPos = transform.position;

        if (indexTip && thumbTip)
        {
            indexPos = indexTip.position;
            float d = Vector3.Distance(indexTip.position, thumbTip.position);
            tipsTouching = d <= touchDistance;
        }

        bool pinchingNow = isOVRPinching && tipsTouching;

        // Триггер на фронте пинча
        if (pinchingNow && !wasPinching)
            TryPickupAt(indexPos);

        wasPinching = pinchingNow;
    }

    void TryPickupAt(Vector3 origin)
    {
        // 1) Сначала OverlapSphere — самый надёжный вариант «соприкосновения»
        Collider[] hits = Physics.OverlapSphere(origin, pickupRadius, pickupMask, QueryTriggerInteraction.Collide);

        VRPickupItem picked = null;
        float bestDist = float.MaxValue;

        foreach (var col in hits)
        {
            if (!col) continue;
            var item = col.GetComponent<VRPickupItem>() ?? col.GetComponentInParent<VRPickupItem>();
            if (!item) continue;

            float d = Vector3.Distance(origin, col.ClosestPoint(origin));
            if (d < bestDist)
            {
                bestDist = d;
                picked = item;
            }
        }

        // 2) Если рядом не нашли — короткий сферо-луч вперёд (подстраховка)
        if (!picked && indexTip)
        {
            Ray ray = new Ray(indexTip.position, indexTip.forward);
            if (Physics.SphereCast(ray, pickupRadius * 0.7f, out RaycastHit hit, pickupReach, pickupMask, QueryTriggerInteraction.Collide))
            {
                picked = hit.collider.GetComponent<VRPickupItem>() ?? hit.collider.GetComponentInParent<VRPickupItem>();
            }
        }

        if (picked)
        {
            VRInventory.Instance.AddItem(picked.itemType);
            if (logDebug) Debug.Log($"✅ Подобран: {picked.itemType} ({picked.name})", picked);
            Object.Destroy(picked.gameObject);
        }
        else
        {
            if (logDebug) Debug.Log("❌ Рядом с кончиком пальца предметов не найдено");
        }
    }

    void EnsureJoints()
    {
        if (indexTip && thumbTip) return;

        var skel = GetSkeleton();
        if (!skel || skel.Bones == null || skel.Bones.Count == 0) return;

        foreach (var b in skel.Bones)
        {
            if (b == null || b.Transform == null) continue;

            switch (b.Id)
            {
                case OVRSkeleton.BoneId.Hand_IndexTip:
                    indexTip = b.Transform;
                    break;
                case OVRSkeleton.BoneId.Hand_ThumbTip:
                    thumbTip = b.Transform;
                    break;
            }
        }
    }

    OVRSkeleton GetSkeleton()
    {
        if (cachedSkeleton) return cachedSkeleton;

        // 1) Указали руками — используем
        if (skeletonOverride) { cachedSkeleton = skeletonOverride; return cachedSkeleton; }

        // 2) Ищем в детях текущего объекта
        cachedSkeleton = GetComponentInChildren<OVRSkeleton>(true);
        if (cachedSkeleton) return cachedSkeleton;

        // 3) Ищем по сцене ближайший OVRSkeleton
#if UNITY_2023_1_OR_NEWER
        var all = Object.FindObjectsByType<OVRSkeleton>(FindObjectsSortMode.None);
#else
        var all = Object.FindObjectsOfType<OVRSkeleton>(true);
#endif
        if (all != null && all.Length > 0)
        {
            float best = float.MaxValue;
            Vector3 p = transform.position;
            OVRSkeleton bestSkel = null;

            foreach (var s in all)
            {
                if (!s) continue;
                float d = (s.transform.position - p).sqrMagnitude;
                if (d < best)
                {
                    best = d;
                    bestSkel = s;
                }
            }
            cachedSkeleton = bestSkel;
        }
        return cachedSkeleton;
    }

    void OnDrawGizmos()
    {
        if (!drawGizmos) return;

        if (indexTip)
        {
            Gizmos.DrawWireSphere(indexTip.position, pickupRadius);
            Gizmos.DrawLine(indexTip.position, indexTip.position + indexTip.forward * pickupReach);
        }

        if (indexTip && thumbTip)
            Gizmos.DrawLine(indexTip.position, thumbTip.position);
    }
}

