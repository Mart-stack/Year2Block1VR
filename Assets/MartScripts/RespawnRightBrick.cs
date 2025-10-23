using UnityEngine;

public class RespawnRightBrick : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject respawnBrickRight;
    public GameObject checkBrickHeight;



  
    void Update()
    {
        if (transform.position.y <= checkBrickHeight.transform.position.y)
        {
            transform.position = respawnBrickRight.transform.position;
        }
    }
}
