
using UnityEngine;

public class PlayerStateSaver : MonoBehaviour
{
    [Header("组件引用")]
    [SerializeField] private SanityManager sanityManager; // 理智管理器
    [SerializeField] private PlayerMovement playerMovement; // 玩家移动模块
    [SerializeField] private PlayerAction playerAction; // 玩家行动模块

    // 保存玩家状态
    public void SaveState()
    {
        GameState.currentPlayerState = new PlayerState(
            sanityManager.CurrentSanity,
            playerMovement.transform.position,
            playerAction.ActionPoints
        );
        Debug.Log("玩家状态已保存");
    }

    // 恢复玩家状态
    public void RestoreState()
    {
        if (GameState.currentPlayerState != null)
        {
            playerMovement.transform.position = GameState.currentPlayerState.position;
            sanityManager.SetSanity(GameState.currentPlayerState.sanity);
            // 注意：这里直接设置了PlayerAction的行动点数，如果PlayerAction有更复杂的内部逻辑，可能需要提供一个公共方法来设置
            // playerAction.ActionPoints = GameState.currentPlayerState.actionPoints; // 直接设置私有set属性会报错
            // 假设PlayerAction有一个公共方法来设置行动点数
            playerAction.SetActionPoints(GameState.currentPlayerState.actionPoints);
            Debug.Log("玩家状态已恢复");
        }
    }
}

