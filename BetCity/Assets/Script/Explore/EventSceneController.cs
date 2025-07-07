using UnityEngine;
using UnityEngine.SceneManagement;

public class EventSceneController : MonoBehaviour
{
    // 假设事件场景结束时，按下某个键返回主场景
    [SerializeField] private KeyCode returnKey = KeyCode.Escape;

    void Update()
    {
        if (Input.GetKeyDown(returnKey))
        {
            EndEvent();
        }
    }

    // 当事件结束时调用此方法
    public void EndEvent()
    {
        Debug.Log("事件结束，卸载当前场景。");
        // 卸载当前事件场景
        SceneManager.UnloadSceneAsync(gameObject.scene);
        // 注意：UnloadSceneAsync 是异步操作，PlayerController 会在 OnSceneUnloaded 中重新启用控制
    }

    // 如果是战斗场景，可能在战斗胜利或失败时调用 EndEvent()
    // public void OnBattleEnd() { EndEvent(); }
}
