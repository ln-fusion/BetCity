using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // <-- 添加这一行

public class SanityUIController : MonoBehaviour
{
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Slider sanitySlider;
    [SerializeField] private TextMeshProUGUI sanityText; // <-- 修改这一行
    [SerializeField] private int changeAmount = 10; // 每次增减的量

    // 本地化文本键（可配合Localization系统使用）
    [SerializeField] private string sanityTextFormat = "Sanity: {0} / {1}"; // 或者 "HP: {0} / {1}" 等


    // 确保UI在场景切换时唯一
    private static bool instanceExists;

    private void Awake()
    {
        // 单例模式防止多个UI实例
        if (!instanceExists)
        {
            instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 验证UI组件引用
        ValidateReferences();
    }

    private void Start()
    {
        // 确保 SanityManager 已经初始化
        SanityManager.Instance.ToString(); // 访问一次 Instance 属性以触发其 Awake/单例创建

        UpdateSanityUI();
        SetupButtonEvents();
        SetupEventListeners();

        // 监听场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 清理事件监听，防止内存泄漏
        RemoveEventListeners();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 场景加载后更新UI
        UpdateSanityUI();
    }

    private void ValidateReferences()
    {
        // 检查UI组件是否正确赋值
        if (increaseButton == null)
            Debug.LogError("increaseButton未赋值！");

        if (decreaseButton == null)
            Debug.LogError("decreaseButton未赋值！");

        if (sanitySlider == null)
            Debug.LogError("sanitySlider未赋值！");

        if (sanityText == null)
            Debug.LogError("sanityText未赋值！");
    }

    private void SetupButtonEvents()
    {
        // 按钮事件绑定
        if (increaseButton != null)
            increaseButton.onClick.AddListener(() => SanityManager.Instance.IncreaseSanity(changeAmount));

        if (decreaseButton != null)
            decreaseButton.onClick.AddListener(() => SanityManager.Instance.DecreaseSanity(changeAmount));
    }

    private void SetupEventListeners()
    {
        // 监听 SanityManager 的理智值变化事件
        if (SanityManager.Instance != null)
        {
            SanityManager.Instance.onSanityChanged.AddListener(UpdateSanityUI);
            // 如果需要，也可以监听 onSanityIncreased 和 onSanityDecreased
            // SanityManager.Instance.onSanityIncreased.AddListener((amount) => Debug.Log($"UI 收到理智增加通知: {amount}"));
            // SanityManager.Instance.onSanityDecreased.AddListener((amount) => Debug.Log($"UI 收到理智减少通知: {amount}"));
        }
    }

    private void RemoveEventListeners()
    {
        if (SanityManager.Instance != null)
        {
            SanityManager.Instance.onSanityChanged.RemoveListener(UpdateSanityUI);
        }
    }

    // 更新UI显示
    public void UpdateSanityUI()
    {
        if (SanityManager.Instance == null) return;

        int current = SanityManager.Instance.CurrentSanity;
        int max = SanityManager.Instance.MaxSanity;

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
