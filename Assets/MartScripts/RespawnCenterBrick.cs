using UnityEngine;

public class RespawnCenterBrick : MonoBehaviour
{
    public GameObject respawnBrickCent;
    public GameObject checkBrickHeight;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
 

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= checkBrickHeight.transform.position.y)
        {
            transform.position = respawnBrickCent.transform.position;
        }
    }
}
