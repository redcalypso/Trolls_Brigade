using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance = null;

    private float _timeScale = 0;

    public float WaitLodeTime = 3;

    private const string _deadLevel = "DeadScene";
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        _timeScale = Time.timeScale;
    }

    public void LoadLevel(string mapName)
    {
        SceneManager.LoadScene(mapName);
    }

    public void LoadLodingLevel()
    { 
        StartCoroutine(LoadingScene());
    }
    public void LoadLodingAgainLevel()
    {
        StartCoroutine(LoadingAgainScene());
    }
    IEnumerator LoadingScene()
    {
        GameObject.Find("Player").transform.GetComponent<Collider2D>().enabled = false;
        GameObject.Find("Player").transform.GetComponent<Animator>().SetTrigger("Death");
        GameObject.Find("Fade").transform.GetComponent<Image>().raycastTarget = true;
        yield return new WaitForSeconds(1f);
        GameObject.Find("Fade").transform.GetComponent<Image>().DOFade(1, 1);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("LoadingScene");
        yield return new WaitForSeconds(WaitLodeTime);
        SceneManager.LoadScene("DeadScene");
    }
    IEnumerator LoadingAgainScene()
    {
        SceneManager.LoadScene("LoadingScene");
        yield return new WaitForSeconds(WaitLodeTime);
        SceneManager.LoadScene("Dungeon");
    }

    public void RePlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        Debug.Log("Pause");
        Time.timeScale = 0;
    }
    public void Play()
    {
        Debug.Log("Play");
        Time.timeScale = _timeScale;
    }
}
