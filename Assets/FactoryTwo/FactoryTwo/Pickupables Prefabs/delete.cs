using UnityEngine;

public class delete : MonoBehaviour
{
    private Vector3 beginningPosition;
    
    void Start()
    {
        beginningPosition = transform.position;
    }

    void Update()
    {
        if (transform.position != beginningPosition)
            Pickup();
    }

    void Pickup()
    {
        Destroy(gameObject);
    }
}