using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController singleton;

    public int mapScene;
    public int dailyEnergy;
    public float fadeTime;

    public int fileNum;
    public string playerName;
    public int money;
    public int energy;
    public int day;

    public Character[] characters;

    private Image fadeImg;

    public bool loading;

    private SaveData save;

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
        Save();
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

        yield return new WaitForSeconds(0.5f);

        if (index == mapScene)
        {
            FindObjectOfType<MapController>().UpdateUI();
            if (energy == 0)
            {
                NewDay();
            }
        }

        yield return new WaitForSeconds(0.5f);

        yield return Fade(false);
        fadeImg.enabled = false;
        loading = false;
    }

    public void Save()
    {
        save = new SaveData(fileNum, playerName, money, day, characters);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + save.fileName);
        bf.Serialize(file, save);
        file.Close();
    }

    public void Load(int num)
    {
        if (!File.Exists(Application.persistentDataPath + "/vds_save_" + num + ".dat"))
        {
            Debug.Log("ERROR: File vds_save_" + num + ".dat not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/vds_save_" + num + ".dat", FileMode.Open);
        save = (SaveData)bf.Deserialize(file);
        file.Close();

        StartCoroutine(FadeAndLoad(mapScene));
        playerName = save.playerName;
        money = save.money;
        day = save.day;

        for(int i = 0; i < characters.Length; i++)
        {
            characters[i].relationship = save.relationships[i];
        }

        LoadScene(mapScene);
    }

    public void NewGame(int num)
    {
        fileNum = num;
        day = 0;
        LoadScene(mapScene);
        NewDay();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
