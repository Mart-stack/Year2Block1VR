using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class breathMeterScript : MonoBehaviour
{
    private readonly float maxOxygen = 60f;
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

    IEnumerator StartBreathHold()
    {
        while (stopTimer == false)
        {
            currentOxygen -= Time.deltaTime;
            
            if (currentOxygen <= 0 ) 
            {
                stopTimer = true;
                if ( stopTimer)
                    Debug.Log("Timer has ended, you're dead");//SceneManager.LoadScene(4);
            }
            
            if (stopTimer == false) 
                UpdateWatch();
            
            yield return new WaitForSeconds(0.001f);
        }
    }
}