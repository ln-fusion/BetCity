using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SanityUIController : MonoBehaviour
{
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Slider sanitySlider;
    [SerializeField] private TextMeshProUGUI sanityText;
    [SerializeField] private int changeAmount = 10; // 理智变化量
    [SerializeField] private string sanityTextFormat = "Sanity: {0} / {1}";

    private SanityManager sanityManager;

    private void Awake()
    {
        // 查找场景中的SanityManager实例
        sanityManager = FindObjectOfType<SanityManager>();
        if (sanityManager == null)
        {
            Debug.LogError("场景中未找到SanityManager实例!");
        }

        // 验证UI引用
        ValidateReferences();
    }

    private void Start()
    {
        UpdateSanityUI();
        SetupButtonEvents();
        SetupEventListeners();

        // 场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 清理事件监听
        RemoveEventListeners();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 重新查找SanityManager实例
        sanityManager = FindObjectOfType<SanityManager>();
        if (sanityManager != null)
        {
            // 重新设置事件监听
            RemoveEventListeners();
            SetupEventListeners();
            UpdateSanityUI();
        }
    }

    private void ValidateReferences()
    {
        if (increaseButton == null)
            Debug.LogError("increaseButton未分配!");

        if (decreaseButton == null)
            Debug.LogError("decreaseButton未分配!");

        if (sanitySlider == null)
            Debug.LogError("sanitySlider未分配!");

        if (sanityText == null)
            Debug.LogError("sanityText未分配!");
    }

    private void SetupButtonEvents()
    {
        if (increaseButton != null)
            increaseButton.onClick.AddListener(() => sanityManager?.IncreaseSanity(changeAmount));

        if (decreaseButton != null)
            decreaseButton.onClick.AddListener(() => sanityManager?.DecreaseSanity(changeAmount));
    }

    private void SetupEventListeners()
    {
        if (sanityManager != null)
        {
            sanityManager.onSanityChanged.AddListener(UpdateSanityUI);
        }
    }

    private void RemoveEventListeners()
    {
        if (sanityManager != null)
        {
            sanityManager.onSanityChanged.RemoveListener(UpdateSanityUI);
        }
    }

    // 更新UI显示
    public void UpdateSanityUI()
    {
        if (sanityManager == null) return;

        int current = sanityManager.CurrentSanity;
        int max = sanityManager.MaxSanity;

        if (sanitySlider != null)
        {
            sanitySlider.maxValue = max;
            sanitySlider.value = current;
        }

        if (sanityText != null)
        {
            sanityText.text = string.Format(sanityTextFormat, current, max);
        }
    }

}