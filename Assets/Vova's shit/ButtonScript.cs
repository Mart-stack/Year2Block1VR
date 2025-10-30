using UnityEngine;

public class SimpleReplaceOnClick : MonoBehaviour
{
    [SerializeField] private GameObject modelNormal;   // оригинальная модель
    [SerializeField] private GameObject modelPressed;  // замена

    private bool isPressed = false;

    void Start()
    {
        modelNormal.SetActive(true);
        modelPressed.SetActive(false);
    }

    // срабатывает, когда на объект кликают (или попадает луч с "Interact" / "Press")
    private void OnTriggerEnter(Collider other)
{
    // если луч или контроллер попал
    modelNormal.SetActive(false);
    modelPressed.SetActive(true);
}
}