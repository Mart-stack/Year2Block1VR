using UnityEngine;
public class BreakGlass : MonoBehaviour
{
    [SerializeField]
    private GameObject brokenGlass;
    [SerializeField]
    private GameObject unbrokenGlass;
    [SerializeField]
    private GameObject enemyObj;

    public AudioClip breakGlass;
    public AudioSource breakGlassSource;

    private void Start()
    {
        breakGlassSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Window"))
        {
            breakGlassSource.PlayOneShot(breakGlass, 1.0f);

            Debug.Log("entered");
            unbrokenGlass.SetActive(false);
            brokenGlass.SetActive(true);
        }
        else if (other.CompareTag("Enemy"))
        {
            Destroy(enemyObj);
        }
    }


}
