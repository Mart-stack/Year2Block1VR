using UnityEditor.PackageManager.UI;
using UnityEngine;
public class BreakGlass : MonoBehaviour
{
    [SerializeField]
    private GameObject brokenGlass;
    [SerializeField]
    private GameObject unbrokenGlass;
    [SerializeField]
    private GameObject enemyObj;
    

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.name == "Window")
    //    {
    //        unbrokenGlass.SetActive(false);
    //        brokenGlass.SetActive(true);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Window"))
        {
            unbrokenGlass.SetActive(false);
            brokenGlass.SetActive(true);
        }
        else if (other.CompareTag("Enemy"))
        {
            Destroy(enemyObj);
        }
    }


}
