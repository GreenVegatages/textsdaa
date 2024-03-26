using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class StartPanel : MonoBehaviour
{
    public static StartPanel Instance;
    public Image image;
    public Button startBtn;

    public Text text;
    public Button overBtn;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        startBtn.onClick.AddListener(StartEvent);
        overBtn.onClick.AddListener(Restart);
        overBtn.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        startBtn.onClick.RemoveAllListeners();
    }

    private void StartEvent()
    {
        MusicManager.Instance.Play(1);
        GameManager.Instance.RandomPrefab();
        image.enabled = false;
        startBtn.gameObject.SetActive(false);
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        MusicManager.Instance.Play(1);
        image.enabled = true;
        overBtn.gameObject.SetActive(true);
        text.gameObject.SetActive(false);
        text.text = "你的分数: " + GameManager.Instance.maxHeight * Random.Range(1000, 1500);
    }
}