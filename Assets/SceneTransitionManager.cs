using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public Animator animator;
    private string sceneToLoad;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void FadeToScene()
    {
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {

    }
 
}

