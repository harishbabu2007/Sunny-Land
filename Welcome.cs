using UnityEngine.SceneManagement;
using UnityEngine;

public class Welcome : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame() {
        SceneManager.LoadScene("Scene1");
    }
}
