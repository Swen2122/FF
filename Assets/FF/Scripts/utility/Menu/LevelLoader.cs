using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Ease easeType = Ease.Linear;
    public static string TargetSceneName;

    public static void LoadLevel(string sceneName)
    {
        TargetSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private void Start()
    {
        RotateImage();
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        yield return new WaitForSeconds(1f);
        AsyncOperation op = SceneManager.LoadSceneAsync(TargetSceneName);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            if (op.progress >= 0.9f)
            {
                image?.DOKill();
                op.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    private void RotateImage()
    {
        if (image != null)
        {
            image.rectTransform
                .DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360)
                .SetEase(easeType)
                .SetLoops(-1)
                .SetLink(image.gameObject); 
        }
    }
}
