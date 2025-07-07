// DiceCounter.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCounter : MonoBehaviour
{
    [Header("骰子图片 (索引0可为空或为滚动中图片，索引1-6对应1-6点)")]
    public Sprite[] AllNumbers; // AllNumbers[0] = null/rolling, AllNumbers[1]=1点, ..., AllNumbers[6]=6点
    public SpriteRenderer Render;

    [Header("动画设置")]
    public float PerTime = 0.05f; // 每帧图片切换时间
    public float ProgressTime = 2f; // 滚动动画总时长

    private Coroutine currentRollCoroutine; // 用于管理协程，避免重复启动

    // 可以在这里添加一个事件，通知外部骰子滚动结束，并传递结果
    public System.Action<int> OnDiceRollFinished;

    private void Awake() // 使用 Awake 确保 Render 引用在 Start 之前可用
    {
        if (Render == null)
        {
            Render = GetComponent<SpriteRenderer>();
            if (Render == null)
            {
                Debug.LogError("DiceCounter: SpriteRenderer 未赋值且未找到！请确保挂载在带有 SpriteRenderer 的 GameObject 上。");
            }
        }
        // 确保 AllNumbers 数组至少有 7 个元素 (索引 0-6)
        if (AllNumbers == null || AllNumbers.Length < 7)
        {
            Debug.LogError("DiceCounter: AllNumbers 数组未赋值或长度不足7！请在 Inspector 中设置。");
        }
    }

    // 外部调用此方法来设置骰子结果并开始动画
    public void SetResultIndexAndAnimate(int finalResult)
    {
        if (finalResult < 1 || finalResult > 6)
        {
            Debug.LogError($"DiceCounter: 无效的骰子结果 {finalResult}。结果必须在 1 到 6 之间。");
            return;
        }

        if (currentRollCoroutine != null)
        {
            StopCoroutine(currentRollCoroutine);
        }
        currentRollCoroutine = StartCoroutine(ShowProgress(finalResult));
    }

    IEnumerator ShowProgress(int finalResult)
    {
        // 可选：在动画开始时显示一个“滚动中”的图片
        if (AllNumbers.Length > 0 && AllNumbers[0] != null)
        {
            Render.sprite = AllNumbers[0]; // 假设 AllNumbers[0] 是滚动中的图片
        }

        float timer = 0;
        while (timer < ProgressTime)
        {
            // 随机选择 1 到 6 的图片进行显示
            int index = Random.Range(1, 7);
            if (Render != null && AllNumbers.Length > index && AllNumbers[index] != null)
            {
                Render.sprite = AllNumbers[index];
            }
            timer += PerTime;
            yield return new WaitForSeconds(PerTime);
        }

        // 动画结束，显示最终结果
        if (Render != null && AllNumbers.Length > finalResult && AllNumbers[finalResult] != null)
        {
            Render.sprite = AllNumbers[finalResult];
        }
        else
        {
            Debug.LogError($"DiceCounter: 无法显示最终结果 {finalResult}，AllNumbers 数组中没有对应的 Sprite。");
        }

        // 通知外部骰子滚动结束
        OnDiceRollFinished?.Invoke(finalResult);

        currentRollCoroutine = null; // 协程结束，清空引用
    }

    // 可以在这里添加一个方法，让外部查询骰子是否正在滚动
    public bool IsRolling()
    {
        return currentRollCoroutine != null;
    }
}
