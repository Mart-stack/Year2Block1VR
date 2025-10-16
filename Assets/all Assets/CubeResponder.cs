using UnityEngine;

public class CubeResponder : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Vector3 targetOffset = new Vector3(0, 0, 5f);
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool active = false;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + targetOffset;
        
    }

    
    public void OnAlert()
    {
        active = true;

        var rend = GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = Color.red;
    }

    void Update()
    {
        if (!active) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );
    }
}