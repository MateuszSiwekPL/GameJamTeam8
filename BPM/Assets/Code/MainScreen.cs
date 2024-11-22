using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScreen : MonoBehaviour
{
    [SerializeField] private Button Scene1Button;
    [SerializeField] private Button Scene2Button;
    [SerializeField] private Button Scene3Button;
    void Start()
    {
        Scene1Button.onClick.AddListener(() => LoadScene(1));
        Scene2Button.onClick.AddListener(() => LoadScene(2));
        Scene3Button.onClick.AddListener(() => LoadScene(3));
    }
    
    private void LoadScene(int sceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
}
