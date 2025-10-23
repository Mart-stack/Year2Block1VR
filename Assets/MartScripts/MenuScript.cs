using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayDemo()
    {
        SceneManager.LoadScene(2);
    }

}
