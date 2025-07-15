using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SanityEvent : UnityEvent<int> { }

public class SanityManager : MonoBehaviour
{
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
                }
            }
            return _instance;
        }
    }

    [SerializeField] private int maxSanity = 100;
    [SerializeField] private int currentSanity = 80;

    public int MaxSanity => maxSanity;
    public int CurrentSanity => currentSanity;

    // 定义事件
    public SanityEvent onSanityIncreased;
    public SanityEvent onSanityDecreased;
    public UnityEvent onSanityChanged;
    public UnityEvent onSanityZero;

    private void Awake()
    {
        // 确保只有一个实例存在
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void IncreaseSanity(int amount)
    {
        if (amount <= 0) return;

        int oldValue = currentSanity;
        currentSanity = Mathf.Min(maxSanity, currentSanity + amount);

        if (currentSanity != oldValue)
        {
            onSanityIncreased?.Invoke(amount);
            onSanityChanged?.Invoke(); // 触发理智变化事件
        }
    }

    public void DecreaseSanity(int amount)
    {
        if (amount <= 0) return;

        int oldValue = currentSanity;
        currentSanity = Mathf.Max(0, currentSanity - amount);

        if (currentSanity != oldValue)
        {
            onSanityDecreased?.Invoke(amount);
            onSanityChanged?.Invoke(); // 触发理智变化事件
            if (currentSanity <= 0)
            {
                onSanityZero?.Invoke();
            }
        }
    }

    public void SetSanity(int newSanity)
    {
        int oldValue = currentSanity;
        currentSanity = Mathf.Clamp(newSanity, 0, maxSanity);

        if (currentSanity != oldValue)
        {
            onSanityChanged?.Invoke(); // 触发理智变化事件
            if (currentSanity <= 0)
            {
                onSanityZero?.Invoke();
            }
        }
    }
}
