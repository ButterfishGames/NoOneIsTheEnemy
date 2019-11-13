using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController singleton;

    public int mapScene;
    public int dailyEnergy;
    public float fadeTime;

    public string playerName;
    public int money;
    public int energy;
    public int day;

    public Character[] characters;

    private Image fadeImg;

    public bool loading;

    // Start is called before the first frame update
    void Start()
    {
        if (singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }

        fadeImg = GetComponentInChildren<Image>();
        fadeImg.enabled = false;

        NewDay();
    }

    private void NewDay()
    {
        day++;
        energy = dailyEnergy;
        MapController[] mapControllers = FindObjectsOfType<MapController>();
        if (mapControllers.Length > 0)
        {
            mapControllers[0].UpdateUI();
        }
    }

    public void LoadScene(int index)
    {
        StartCoroutine(FadeAndLoad(index));
    }

    private IEnumerator Fade(bool fadeOut)
    {
        Color color = fadeImg.color;
        color.a = fadeOut ? 0 : 1;
        fadeImg.color = color;
        float targetAlpha = fadeOut ? 1 : 0;
        int dirMod = fadeOut ? 1 : -1;

        while (fadeImg.color.a != targetAlpha)
        {
            color.a = Mathf.Clamp(color.a + dirMod * Time.deltaTime / fadeTime, 0, 1);
            fadeImg.color = color;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeAndLoad(int index)
    {
        loading = true;
        fadeImg.enabled = true;
        yield return Fade(true);

        SceneManager.LoadScene(index);

        if (index == mapScene && energy == 0)
        {
            day++;
            energy = dailyEnergy;
        }
        yield return new WaitForSeconds(1.0f);

        yield return Fade(false);
        fadeImg.enabled = false;
        loading = false;
    }
}
