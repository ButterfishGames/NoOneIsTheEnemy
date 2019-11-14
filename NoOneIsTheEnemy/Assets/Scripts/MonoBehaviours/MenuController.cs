using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public int numSaves;

    public GameObject fileButtonPrefab;

    public GameObject loadPanel;
    public GameObject storyPanel;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(GameObject.Find("NewGameButton"));
    }

    public void NewGame()
    {
        if (loadPanel.activeInHierarchy)
        {
            return;
        }
        loadPanel.SetActive(true);

        for (int i = 0; i < numSaves; i++)
        {
            GameObject button = Instantiate(fileButtonPrefab, loadPanel.transform);

            if (File.Exists(Application.persistentDataPath + "/vds_save_" + i + ".dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/vds_save_" + i + ".dat", FileMode.Open);
                SaveData temp = (SaveData)bf.Deserialize(file);
                file.Close();

                if (temp.day % 2 == 1)
                {
                    button.GetComponentInChildren<Text>().text = "File " + (i + 1) + " (Day " + Mathf.CeilToInt(temp.day / 2.0f) + ")";
                }
                else
                {
                    button.GetComponentInChildren<Text>().text = "File " + (i + 1) + " (Night " + (temp.day / 2) + ")";
                }
            }
            else
            {
                button.GetComponentInChildren<Text>().text = "File" + (i + 1) + " (Empty)";
            }

            int tempInt = i;
            button.GetComponent<Button>().onClick.AddListener(() => GameController.singleton.NewGame(tempInt));

            if (i == 0)
            {
                EventSystem.current.SetSelectedGameObject(button);
            }
        }
    }

    public void Continue()
    {
        if (loadPanel.activeInHierarchy)
        {
            return;
        }
        loadPanel.SetActive(true);

        int startSelect = 0;

        for (int i = 0; i < numSaves; i++)
        {
            GameObject button = Instantiate(fileButtonPrefab, loadPanel.transform);

            if (File.Exists(Application.persistentDataPath + "/vds_save_" + i + ".dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/vds_save_" + i + ".dat", FileMode.Open);
                SaveData temp = (SaveData)bf.Deserialize(file);
                file.Close();

                if (temp.day % 2 == 1)
                {
                    button.GetComponentInChildren<Text>().text = "File " + (i + 1) + " (Day " + Mathf.CeilToInt(temp.day / 2.0f) + ")";
                }
                else
                {
                    button.GetComponentInChildren<Text>().text = "File " + (i + 1) + " (Night " + (temp.day / 2) + ")";
                }

                int tempInt = i;
                button.GetComponent<Button>().onClick.AddListener(() => GameController.singleton.Load(tempInt));
            }
            else
            {
                button.GetComponentInChildren<Text>().text = "File" + (i + 1) + " (Empty)";
                button.GetComponent<Button>().interactable = false;
            }

            if (i == startSelect)
            {
                if (button.GetComponent<Button>().IsInteractable())
                {
                    EventSystem.current.SetSelectedGameObject(button);
                }
                else
                {
                    startSelect++;
                }
            }
        }
    }

    public void CloseLoadPanel()
    {
        Button[] buttons = loadPanel.GetComponentsInChildren<Button>();
        foreach(Button button in buttons)
        {
            if (!button.name.Equals("CloseButton"))
            {
                Destroy(button.gameObject);
            }
        }

        loadPanel.SetActive(false);
    }

    public void OpenStoryPanel()
    {
        storyPanel.SetActive(true);
    }

    public void CloseStoryPanel()
    {
        storyPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
