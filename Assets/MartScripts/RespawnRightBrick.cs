using UnityEngine;

public class RespawnRightBrick : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject respawnBrickRight;
    public GameObject checkBrickHeight;

    [SerializeField]
    private Rigidbody rightRb;

    private void Start()
    {
        rightRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y <= checkBrickHeight.transform.position.y)
        {
            rightRb.linearVelocity = Vector3.zero;
            transform.position = respawnBrickRight.transform.position;
        }
    }
}
