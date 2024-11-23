using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreen : MonoBehaviour
{
    [SerializeField] private Button _MainScene;

    // Start is called before the first frame update
    void Start()
    {
        _MainScene.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
