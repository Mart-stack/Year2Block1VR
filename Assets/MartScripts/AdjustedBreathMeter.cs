using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class AdjustedBreathMeter : MonoBehaviour
{


    [SerializeField]
    private GameObject playerLocation;

    [SerializeField]
    private GameObject watchSec;

    public GameObject respawnPos;
    public GameObject checkPoint;

    //public int playerDeath = 0;

    public float maxOxygen = 60f;
    private float currentOxygen;
    public bool stopTimer = false;

    private int colCount = 0;
    private bool startNextTimer = false;
    private bool startTimerThree = false;
    public TextMeshProUGUI o2PercentageText;
    public Image coloredCircle;
    private AudioSource audioSource;
    public AudioClip lowOxygen;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentOxygen = maxOxygen;
        UpdateWatch();
        StartTimer();

    }

    private void Update()
    {
        if (playerLocation.transform.position.x >= checkPoint.transform.position.x)
        {
            Debug.Log("worked");
            SceneManager.LoadScene(0);
        }
        else if (respawnPos.transform.position.x >= checkPoint.transform.position.x)
        {
            Debug.Log("alternative");
            SceneManager.LoadScene(0);

        }
        EnemyCollision();

        if (startNextTimer)
        {
            StartCoroutine(EnemySecondColl());
        }
        if (startTimerThree)
        {
            StartCoroutine(EnemyThirdColl());
        }

        if (currentOxygen <= 18)
        {
            audioSource.PlayOneShot(lowOxygen, 1.0f);
        }

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

    private void EnemyCollision()
    {
        if (colCount >= 1)
        {
            startNextTimer = true;
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                currentOxygen = currentOxygen - 20f;
                Debug.Log("collided");
                colCount += 1;
            }
        }

    }

    IEnumerator EnemySecondColl()
    {

        yield return new WaitForSeconds(3.0f);
        EnemyCollisionSecond();

    }

    IEnumerator EnemyThirdColl()
    {
        yield return new WaitForSeconds(3.0f);
        EnemyCollisionThird();

    }

    private void EnemyCollisionSecond()
    {
        if (colCount >= 2)
        {
            startTimerThree = true;
            return;
        }


        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                currentOxygen = currentOxygen - 20f;
                Debug.Log("collided");
                colCount += 1;
            }
        }


    }

    private void EnemyCollisionThird()
    {
        if (colCount >= 3)
        {
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                currentOxygen = currentOxygen - 20f;
                Debug.Log("collided");
                colCount += 1;
            }
        }


    }

    private void OnDrawGizmos()
    {
        // Draw wire sphere outline.
        Gizmos.color = Color.darkBlue;
        Gizmos.DrawWireSphere(transform.position, 1f);


    }

    IEnumerator StartBreathHold()
    {
        while (stopTimer == false)
        {
            currentOxygen -= Time.deltaTime;

            UpdateWatch();


            if (currentOxygen <= 0)
            {
                // playerDeath = 1;
                stopTimer = true;

                SceneManager.LoadScene(0);

                watchSec.SetActive(true);

                Destroy(gameObject);

                if (stopTimer)
                    Debug.Log("Timer has ended, you're dead");
            }



            yield return new WaitForSeconds(0.001f);
        }
    }
}