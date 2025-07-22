using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("组件引用")]
    [SerializeField] private PlayerMovement playerMovement; // 玩家移动模块
    [SerializeField] private PlayerAction playerAction; // 玩家行动模块
    [SerializeField] private PlayerEventSystem playerEventSystem; // 玩家事件系统模块
    [SerializeField] private PlayerStateSaver playerStateSaver; // 玩家状态保存模块
    [SerializeField] private SanityManager sanityManager; // 理智管理器
    [SerializeField] private Node startNode; // 玩家起始节点

    // 初始化
    private void Start()
    {
        // 检查SanityManager引用
        if (sanityManager == null)
        {
            Debug.LogError("SanityManager引用未设置！");
            return;
        }

        if (startNode != null)
        {
            playerMovement.InitAtNode(startNode);
            Debug.Log($"玩家已初始化到节点: {startNode.name}");
        }
        else
        {
            Debug.LogError("未指定起始节点！");
        }

        // 添加理智归零事件监听
        sanityManager.onSanityZero.AddListener(HandleSanityZero);
    }

    // 处理理智归零的事件
    private void HandleSanityZero()
    {
        Debug.Log("玩家理智归零，游戏结束。");
        SceneManager.LoadScene("GameOverScene"); // 加载游戏结束场景
    }

    // 公共方法：投骰子按钮
    public void RollDiceButton()
    {
        if (playerEventSystem.IsInEvent)
        {
            Debug.LogWarning("当前正在事件中，无法投掷骰子。");
            return;
        }

        playerAction.RollDice();
    }

    // 公共方法：尝试移动到目标节点
    public void TryMoveToNode(Node targetNode)
    {
        if (playerEventSystem.IsInEvent)
        {
            Debug.LogWarning("当前正在事件中，无法移动。");
            return;
        }

        if (sanityManager.CurrentSanity <= 0)
        {
            Debug.LogWarning("理智值不足，无法移动。");
            return;
        }

        if (playerMovement.CanMoveTo(targetNode, playerAction.ActionPoints))
        {
            StartCoroutine(playerMovement.MoveToNode(targetNode));
            playerAction.DecreaseActionPoints();
            playerEventSystem.CheckForNodeEvent(playerMovement.CurrentNode);
        }
    }

    // 切换到事件场景 (由外部调用，例如节点事件触发)
    public void LoadEventScene(int sceneIndex)
    {
        playerStateSaver.SaveState(); // 保存当前状态
        playerEventSystem.LoadEventScene(sceneIndex);
    }

    // 离开事件，返回探索场景 (由外部调用，例如事件结束按钮)
    public void EndEvent()
    {
        playerEventSystem.EndEvent();
        playerStateSaver.RestoreState(); // 恢复玩家状态
    }
}

