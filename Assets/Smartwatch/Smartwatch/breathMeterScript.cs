using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using JetBrains.Annotations;
//using UnityEngine.SceneManagement;

public class breathMeterScript : MonoBehaviour
{

    

    [SerializeField]
    private GameObject playerLocation;

    [SerializeField]
    private GameObject watchSec;

    public GameObject respawnPos;

    //public int playerDeath = 0;

    public float maxOxygen = 60f;
    private float currentOxygen;
    public bool stopTimer = false;

    public TextMeshProUGUI o2PercentageText;
    public Image coloredCircle;

    void Start()
    {
      
        currentOxygen = maxOxygen;
        UpdateWatch();
        StartTimer();
    }




    private void StartTimer()
    {
        StartCoroutine(StartBreathHold());
    }

   

    private void UpdateWatch()
    {
        float percentage = currentOxygen / maxOxygen * 100f;

        o2PercentageText.text = Mathf.RoundToInt(percentage) + "%";

        float t = Mathf.Pow(percentage / 100f, 1.4f);
        float hue = Mathf.Lerp(0f / 360f, 130f / 360f, t);
        Color color = Color.HSVToRGB(hue, 1f, 1f);
        Color colorText = Color.HSVToRGB(hue, .5f, 1f);

        coloredCircle.color = color;

        var gradient = o2PercentageText.colorGradient;
        gradient.bottomLeft = colorText;
        gradient.bottomRight = colorText;

        o2PercentageText.colorGradient = gradient;

       
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            currentOxygen = currentOxygen - 12f;
            Debug.Log("Collided");
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentOxygen = currentOxygen - 12f;
            Debug.Log("collided");
        }
    }



    IEnumerator StartBreathHold()
    {
        while (stopTimer == false)
        {
            currentOxygen -= Time.deltaTime;

            UpdateWatch();


            if (currentOxygen <= 0 ) 
            {
                // playerDeath = 1;
                stopTimer = true;

                playerLocation.transform.position = respawnPos.transform.position;
                
                watchSec.SetActive(true);

                Destroy(gameObject);

                if ( stopTimer)
                    Debug.Log("Timer has ended, you're dead");//SceneManager.LoadScene(4);
            }



            yield return new WaitForSeconds(0.001f);
        }
    }
}