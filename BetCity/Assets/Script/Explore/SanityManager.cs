using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// 理智值变化事件
[System.Serializable]
public class SanityEvent : UnityEvent<int> { } // 参数为变化的理智值量

public class SanityManager : MonoBehaviour
{
    // 改进的单例实现
    private static SanityManager _instance;
    public static SanityManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SanityManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<SanityManager>();
                    singletonObject.name = typeof(SanityManager).ToString() + " (Singleton)";

                    // 确保单例在场景切换时不被销毁
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    [SerializeField] private int maxSanity = 100;    // 最大理智值
    [SerializeField] private int currentSanity = 80; // 初始理智值

    public int MaxSanity => maxSanity;
    public int CurrentSanity => currentSanity;

    public SanityEvent onSanityIncreased; // 理智值增加时触发
    public SanityEvent onSanityDecreased; // 理智值减少时触发
    public UnityEvent onSanityChanged;    // 理智值变化时触发 (不带参数)
    public UnityEvent onSanityZero;       // 理智值归零时触发

    private void Awake()
    {
        // 确保场景中只有一个实例
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // 初始化事件
        if (onSanityIncreased == null)
            onSanityIncreased = new SanityEvent();

        if (onSanityDecreased == null)
            onSanityDecreased = new SanityEvent();

        if (onSanityChanged == null)
            onSanityChanged = new UnityEvent();

        if (onSanityZero == null)
            onSanityZero = new UnityEvent();
    }

    // 在应用程序退出时（包括退出播放模式），清理静态实例
    private void OnApplicationQuit()
    {
        _instance = null; // 清空静态引用
        // 如果 SanityManager 是通过代码创建的 GameObject，
        // 并且没有被 Destroy(gameObject) 销毁，
        // 可以在这里手动销毁它，但通常 Unity 会自动处理 DontDestroyOnLoad 的对象。
        // 如果有必要，可以添加 Debug.Log("SanityManager OnApplicationQuit called.");
    }

    // 增加理智值
    public void IncreaseSanity(int amount)
    {
        if (amount <= 0) return;

        int oldValue = currentSanity;
        currentSanity = Mathf.Min(maxSanity, currentSanity + amount);

        if (currentSanity != oldValue)
        {
            onSanityIncreased?.Invoke(amount);
            onSanityChanged?.Invoke();
            Debug.Log($"理智值增加 {amount}，当前理智值: {currentSanity}");
        }
    }

    // 减少理智值
    public void DecreaseSanity(int amount)
    {
        if (amount <= 0) return;

        int oldValue = currentSanity;
        currentSanity = Mathf.Max(0, currentSanity - amount);

        if (currentSanity != oldValue)
        {
            onSanityDecreased?.Invoke(amount);
            onSanityChanged?.Invoke();
            Debug.Log($"理智值减少 {amount}，当前理智值: {currentSanity}");

            if (currentSanity <= 0)
            {
                onSanityZero?.Invoke();
                Debug.Log("理智值归零，游戏结束！");
                // SceneManager.LoadScene("GameOverScene");
            }
        }
    }

    // 直接设置理智值
    public void SetSanity(int newSanity)
    {
        int oldValue = currentSanity;
        currentSanity = Mathf.Clamp(newSanity, 0, maxSanity);

        if (currentSanity != oldValue)
        {
            onSanityChanged?.Invoke();
            Debug.Log($"理智值设置为 {newSanity}，当前理智值: {currentSanity}");
            if (currentSanity <= 0)
            {
                onSanityZero?.Invoke();
                Debug.Log("理智值归零，游戏结束！");
            }
        }
    }
}
