using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public int RollDice()
    {
        int result = Random.Range(1, 7); // ��׼6����
        Debug.Log($"���ӽ��: {result}");

        // Ԥ�������¼���չ��
        // if(result >= 6) EventManager.OnFullMoon?.Invoke();

        return result;
    }
}