using UnityEngine;
using UnityEngine.SceneManagement;
public class TurnOnOxygenMeter : MonoBehaviour
{
    [SerializeField]
    private GameObject oxygenMeter;

    void Update()
    {
        if (transform.position.x > 28.5)
        {
            oxygenMeter.SetActive(true);

        }

        
        if (transform.position.x > 56)
        {
            SceneManager.LoadScene(0);
            Debug.Log("arrived");

        }


    }
}
