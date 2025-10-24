using UnityEngine;

public class RespawnCenterBrick : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;



    public GameObject respawnBrickCent;
    public GameObject checkBrickHeight;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= checkBrickHeight.transform.position.y)
        {
            rb.linearVelocity = Vector3.zero;
            transform.position = respawnBrickCent.transform.position;
        }
    }
}
