using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.IO; // 添加文件操作支持

[System.Serializable]
public class SanityEvent : UnityEvent<int> { }

public class SanityManager : MonoBehaviour
{
    [SerializeField] private int maxSanity = 100;    // 最大理智值
    [SerializeField] private int currentSanity = 80; // 当前理智值
    private string csvFilePath; // CSV文件路径

    public int MaxSanity => maxSanity;
    public int CurrentSanity => currentSanity;

    public SanityEvent onSanityIncreased; // 理智增加事件
    public SanityEvent onSanityDecreased; // 理智减少事件
    public UnityEvent onSanityChanged;    // 理智变化事件
    public UnityEvent onSanityZero;       // 理智归零事件

    private void Awake()
    {
        // 设置CSV文件路径
        // 获取项目Assets文件夹的路径
        string assetsPath = Application.dataPath;
        // 组合成Assets/Data/Assets/Data/playerdata.csv路径
        csvFilePath = Path.Combine(assetsPath, "Data", "playerdata.csv");

        // 初始化事件
        if (onSanityIncreased == null)
            onSanityIncreased = new SanityEvent();

        if (onSanityDecreased == null)
            onSanityDecreased = new SanityEvent();

        if (onSanityChanged == null)
            onSanityChanged = new UnityEvent();

        if (onSanityZero == null)
            onSanityZero = new UnityEvent();

        // 从CSV加载数据
        LoadFromCSV();
    }

    // 从CSV文件加载数据
    private void LoadFromCSV()
    {
        if (File.Exists(csvFilePath))
        {
            string[] lines = File.ReadAllLines(csvFilePath);
            if (lines.Length > 1)
            {
                string[] values = lines[1].Split(',');
                if (values.Length >= 2)
                {
                    int.TryParse(values[0], out maxSanity);
                    int.TryParse(values[1], out currentSanity);
                }
            }
        }
        else
        {
            // 文件不存在则创建并保存默认值
            SaveToCSV();
        }
    }

    // 保存数据到CSV
    private void SaveToCSV()
    {
        string[] lines = {
            "MaxSanity,CurrentSanity",
            $"{maxSanity},{currentSanity}"
        };
        File.WriteAllLines(csvFilePath, lines);
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
            Debug.Log($"理智值增加了 {amount}点, 当前理智: {currentSanity}");
            SaveToCSV(); // 保存到CSV
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
            Debug.Log($"理智值减少了 {amount}点, 当前理智: {currentSanity}");
            SaveToCSV(); // 保存到CSV

            if (currentSanity <= 0)
            {
                onSanityZero?.Invoke();
                Debug.Log("理智值已归零，游戏结束!");
            }
        }
    }

    // 设置理智值
    public void SetSanity(int newSanity)
    {
        int oldValue = currentSanity;
        currentSanity = Mathf.Clamp(newSanity, 0, maxSanity);

        if (currentSanity != oldValue)
        {
            onSanityChanged?.Invoke();
            Debug.Log($"理智值设置为 {newSanity}, 当前理智: {currentSanity}");
            SaveToCSV(); // 保存到CSV
            if (currentSanity <= 0)
            {
                onSanityZero?.Invoke();
                Debug.Log("理智值已归零，游戏结束!");
            }
        }
    }

}