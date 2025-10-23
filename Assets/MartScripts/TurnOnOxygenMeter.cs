using UnityEngine;
using UnityEngine.SceneManagement;
public class TurnOnOxygenMeter : MonoBehaviour
{
    [SerializeField]
    private GameObject oxygenMeter;

    private breathMeterScript playerDeathScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // playerDeathScript = GameObject.Find("watch").GetComponent<breathMeterScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > 38 )
        {
            oxygenMeter.SetActive(true);

        }

        
        if (transform.position.z > 72)
        {
            SceneManager.LoadScene(0);
            Debug.Log("arrived");

        }


    }
}
