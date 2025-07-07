// DiceManager.cs
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private DiceCounter diceCounter; // 引用 DiceCounter

    private void Awake()
    {
        if (diceCounter == null)
        {
            Debug.LogError("DiceCounter 未赋值给 DiceManager！请在 Inspector 中拖拽赋值。");
        }
    }

    public int RollDice()
    {
        if (diceCounter == null)
        {
            Debug.LogError("DiceCounter 未赋值！无法投掷可视化骰子。");
            return 0;
        }

        int result = Random.Range(1, 7); // 标准6面骰
        Debug.Log($"骰子结果: {result}");

        // 调用 DiceCounter 来显示可视化骰子动画
        diceCounter.SetResultIndexAndAnimate(result);

        // 预留商人事件扩展点
        // if(result >= 6) EventManager.OnFullMoon?.Invoke();

        return result;
    }
}
