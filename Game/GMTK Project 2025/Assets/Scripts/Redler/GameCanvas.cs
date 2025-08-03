using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    public static GameCanvas Instance;

    private bool isPaused;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private AudioVariable pauseAudio;
    [SerializeField] private AudioVariable resumeAudio;
    [SerializeField] private AudioVariable mainMenuAudio;

    [SerializeField] private RectTransform inventoryTransform;
    private int itemsCount = 0;
    [SerializeField] private float itemUIWidth = 100f;

    [SerializeField] private Button warningButton;
    [SerializeField] private AudioVariable warningAudio;

    [SerializeField] private Button backButton;
    [SerializeField] private AudioVariable backAudio;

    [SerializeField] private GameObject warningPanel;
    private bool isWarningOpen;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        warningPanel.SetActive(false);
        pausePanel.SetActive(false);

        resumeButton.onClick.AddListener(Pause);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        warningButton.onClick.AddListener(Warning);
        backButton.onClick.AddListener(Warning);

        inventoryTransform.sizeDelta = new Vector2((itemUIWidth + 20) * itemsCount, itemUIWidth + 20);
    }

    private void GoToMainMenu()
    {
        AudioManager.Instance.PlayAudio(mainMenuAudio);

        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void Pause()
    {
        if (isWarningOpen)
        {
            isWarningOpen = false;
            warningPanel.SetActive(false);
            AudioManager.Instance.PlayAudio(backAudio);

            return;
        }

        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);

        if (isPaused)
        {
            AudioManager.Instance.PlayAudio(pauseAudio);
        }
        else
        {
            AudioManager.Instance.PlayAudio(resumeAudio);
        }
    }

    public void AddItemToInventory(int id)
    {
        inventoryTransform.GetChild(id).gameObject.SetActive(true);
        inventoryTransform.GetChild(id).GetChild(0).GetComponent<TextMeshProUGUI>().text = (itemsCount+1).ToString();
        itemsCount++;
        inventoryTransform.sizeDelta = new Vector2((itemUIWidth + 20) * itemsCount, itemUIWidth + 20);
    }

    public void BoldItem(int id)
    {
        for (int i = 0; i < inventoryTransform.childCount; i++)
        {
            Image childImage = inventoryTransform.GetChild(i).GetComponent<Image>();

            if (i == id)
            {
                childImage.color = Color.white;
                childImage.transform.localScale = Vector3.one;
            }
            else
            {
                childImage.color = Color.gray;
                childImage.transform.localScale = Vector3.one * 0.8f;
            }
        }
    }

    public void Warning()
    {
        isWarningOpen = !isWarningOpen;

        warningPanel.SetActive(isWarningOpen);

        AudioManager.Instance.PlayAudio(isWarningOpen ? warningAudio : backAudio);
    }

}
