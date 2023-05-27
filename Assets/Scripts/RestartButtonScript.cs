using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButtonScript : MonoBehaviour
{
    public Canvas canvas;
    public void OnClick()
    {
        Physics2D.IgnoreLayerCollision(7, 9, false);
        Physics2D.IgnoreLayerCollision(7, 10, false);
        SceneManager.LoadScene(0);
        canvas.gameObject.SetActive(false);
    }
}
