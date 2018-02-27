using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ToMain : MonoBehaviour {

    void NextLevel()
    {
        SceneManager.LoadScene("Start");
    }
}
