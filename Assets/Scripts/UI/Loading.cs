using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private Image panel;

    private void Awake()
    {
        Sprite sprite = Loader.LoadSpriteFromPath(Application.dataPath + "/Videos/Season/" +
                                                  MatchConfig.GetInstance().Match + "_loading.png");
        if (sprite != null)
            panel.sprite = sprite;
    }

    private void Update()
    {
        SceneManager.LoadScene("Match");
    }
}