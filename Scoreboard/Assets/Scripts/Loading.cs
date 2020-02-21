using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        SceneManager.LoadScene(2);
    }
}
