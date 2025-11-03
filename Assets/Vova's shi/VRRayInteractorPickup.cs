using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class VRRayInteractorPickup : MonoBehaviour
{
    [Header("XR Settings")]
    [SerializeField] private XRNode handNode = XRNode.RightHand;   // какая рука
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;        // можно задать вручную в инспекторе

    private InputDevice hand;
    private bool prevPressed;

    void Awake()
    {
        // если не подключили вручную — пытаемся найти на этом объекте
        if (!rayInteractor)
            rayInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();

        if (!rayInteractor)
            Debug.LogError("❌ VRRayInteractorPickup: XRRayInteractor не найден! Повесь скрипт туда, где он есть (обычно Near-Far Interactor).");
        else
            Debug.Log("✅ VRRayInteractorPickup подключен к " + rayInteractor.name);
    }

    void OnEnable() => GrabDevice();

    void GrabDevice()
    {
        var list = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(handNode, list);
        if (list.Count > 0)
        {
            hand = list[0];
            Debug.Log("🎮 Контроллер найден: " + hand.name);
        }
    }

    void Update()
    {
        if (!rayInteractor) return;

        if (!hand.isValid) GrabDevice();

        bool pressed = false;
        if (hand.isValid)
            hand.TryGetFeatureValue(CommonUsages.triggerButton, out pressed);
        else
            pressed = Input.GetKeyDown(KeyCode.E); // тест на клавиатуре

        if (pressed && !prevPressed)
        {
            Debug.Log("🔸 Нажатие триггера зарегистрировано");

            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                Debug.Log("🎯 Луч попал в: " + hit.collider.name);
                var item = hit.collider.GetComponent<VRPickupItem>();
                if (item)
                {
                    Debug.Log("✅ Подобран предмет: " + item.itemType);
                    VRInventory.Instance.AddItem(item.itemType);
                    Destroy(item.gameObject);
                }
                else
                {
                    Debug.Log("❌ Попадание есть, но VRPickupItem не найден на " + hit.collider.name);
                }
            }
            else
            {
                Debug.Log("❌ TryGetCurrent3DRaycastHit ничего не вернул");
            }
        }

        prevPressed = pressed;
    }
}