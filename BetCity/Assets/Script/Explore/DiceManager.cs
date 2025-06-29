using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public int RollDice()
    {
        int result = Random.Range(1, 7); // 标准6面骰
        Debug.Log($"骰子结果: {result}");

        // 预留商人事件扩展点
        // if(result >= 6) EventManager.OnFullMoon?.Invoke();

        return result;
    }
}