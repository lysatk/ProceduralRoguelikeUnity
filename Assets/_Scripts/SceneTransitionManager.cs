using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public Animator animator;
    private string sceneToLoad;
    void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void FadeToScene()
    {
        animator.SetTrigger("FadeOut");
        Time.timeScale = 0.0f;
    }

    public void OnFadeComplete()
    {
        if(GameManager.Instance!=null)
        GameManager.Instance.ResumeGame();
        this.gameObject.SetActive(false);
    }
 
}

