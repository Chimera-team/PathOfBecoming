using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TransitionBetweenScenes : MonoBehaviour
{
    private bool firstCadr = true;
    bool done = false;
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private int loadLevel; // загружаемый уровень
    [SerializeField] private GameObject setAndOff;
    public bool CanULoadNextLvl;
    private float fadeLength; // длина анимации
    [SerializeField] CanvasGroup[] CutScenes;
    private void Awake()
    {
        if(setAndOff!=null)
        {
            setAndOff.SetActive(true);
            
        }      
    }
    private void Start()
    {
        if (setAndOff != null)
            setAndOff.SetActive(false);
    }
    private void Update()
    {
        if(!done)
        {
            if (firstCadr)
            {
                if (setAndOff != null)
                {
                    setAndOff.SetActive(true);
                    firstCadr = false;
                }
            }
            else if (setAndOff != null)
            {
                setAndOff.SetActive(false);
                done = true;
            }
        }
    }
    /// <summary>
    /// Затухание
    /// </summary>
    public void FadeComplete()
    {
        fadeAnimator.SetTrigger("FadeIn");
        StartCoroutine(LoadScene());
    }

    /// <summary>
    /// Загрузка сцены
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadScene()
    {
        GetAnimationTime();
        yield return new WaitForSeconds(fadeLength);
        SceneManager.LoadScene(loadLevel);
    }
    
    /// <summary>
    /// метод для показа катсцен
    /// </summary>
    public void ShowCutScenes(int num)
    {
        ChangeCG(CutScenes[num], true);
        if(num!=0)
        {
            ChangeCG(CutScenes[num - 1], false);
        }
    }
    private void ChangeCG(CanvasGroup canvasGroup, bool what)
    {
        if (what)
            canvasGroup.alpha = 1;
        else
            canvasGroup.alpha = 0;
        canvasGroup.interactable = what;
        canvasGroup.blocksRaycasts = what;
        
    }

    /// <summary>
    /// Получаем длину анимации, чтобы ничего не сломалось при ее изменении
    /// </summary>
    public void GetAnimationTime()
    {
        AnimationClip[] clip = fadeAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip animClip in clip)
        {
            if (animClip.name.Equals("FadeOut"))
            {
                fadeLength = animClip.length;
                Debug.Log(fadeLength);
            }
        }
    }
    private void OnMouseUp()
    {
        if (CanULoadNextLvl)
            FadeComplete();
    }
}
