using UnityEngine;

public class RespawnLeftBrick : MonoBehaviour
{
    public GameObject respawnBrickLeft;
    public GameObject checkBrickHeight;

    [SerializeField]
    private Rigidbody leftRb;


    private void Start()
    {
        leftRb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= checkBrickHeight.transform.position.y)
        {
            leftRb.linearVelocity = Vector3.zero;
            transform.position = respawnBrickLeft.transform.position;
        }
    }
}
