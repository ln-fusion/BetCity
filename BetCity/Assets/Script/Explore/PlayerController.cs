﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("组件引用")]
    [SerializeField] private DiceManager diceManager; // 投骰子管理器
    [SerializeField] private Node startNode; // 玩家起始节点
    [SerializeField] private MySceneLoader mySceneLoader; // 新增：引用自定义的场景加载器

    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 0.5f; // 移动速度
    [SerializeField] private float playerHeight = 0.5f; // 玩家高度

    [Header("理智消耗设置")]
    [SerializeField] private int dailySanityCost = 5; // 每次行动消耗的理智值

    [Header("事件场景索引")]
    [SerializeField] private int[] eventSceneIndices = { 2, 3, 4, 5 }; // 事件场景的索引

    private Node currentNode; // 当前所在节点
    private int actionPoints; // 剩余行动点数
    private bool isMoving; // 是否正在移动
    private bool isInEvent = false; // 标记是否在事件场景中

    private Vector3 savedCameraPosition; // 用于保存摄像机的初始位置

    // 初始化
    private void Start()
    {
        // 保存摄像机的位置
        if (Camera.main != null)
        {
            savedCameraPosition = Camera.main.transform.position;
        }
        else
        {
            Debug.LogError("未找到 MainCamera，请确保场景中有一个标记为 MainCamera 的摄像机！");
        }

        if (startNode != null)
        {
            InitAtNode(startNode);
            Debug.Log($"玩家已初始化到节点: {startNode.name}");
        }
        else
        {
            Debug.LogError("未指定起始节点！");
        }

        // 确保 SanityManager 实例化并添加监听器
        if (SanityManager.Instance != null)
        {
            SanityManager.Instance.onSanityZero.AddListener(HandleSanityZero); // 监听理智值归零事件
        }
        else
        {
            Debug.LogError("SanityManager 实例未初始化！");
        }

        // 确保 MySceneLoader 实例已赋值
        if (mySceneLoader == null)
        {
            // 尝试在场景中查找 MySceneLoader 实例
            mySceneLoader = FindObjectOfType<MySceneLoader>();
            if (mySceneLoader == null)
            {
                Debug.LogError("未找到 MySceneLoader 组件！请确保场景中有一个挂载了 MySceneLoader 脚本的 GameObject。");
            }
        }
    }






    // 保存玩家状态
    private void SaveState()
    {
        GameState.currentPlayerState = new PlayerState(
            SanityManager.Instance.CurrentSanity,
            transform.position,
            actionPoints
        );
        Debug.Log("玩家状态已保存");
    }

    // 切换到事件场景
    public void LoadEventScene(int sceneIndex)
    {
        SaveState(); // 保存当前状态

        // 获取场景名称（字符串）
        string sceneName = GetSceneNameByIndex(sceneIndex);

        // 加载事件场景
        if (mySceneLoader != null)
        {
            mySceneLoader.LoadScene(sceneName); // 使用自定义的场景加载器
        }
        else
        {
            Debug.LogError("MySceneLoader 未引用，无法加载场景！");
            // 备用方案：直接使用 Unity 内置的 SceneManager，但可能不是期望的行为
            // UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        isInEvent = true; // 设置为在事件场景中

        // 恢复摄像机的位置
        if (Camera.main != null)
        {
            Camera.main.transform.position = savedCameraPosition;
        }
        else
        {
            Debug.LogError("没有找到 MainCamera，无法恢复摄像机位置！");
        }
    }

    // 离开事件，返回探索场景
    public void EndEvent()
    {
        // 加载主探索场景
        if (mySceneLoader != null)
        {
            mySceneLoader.LoadScene("ExplorerMap"); // 使用自定义的场景加载器
        }
        else
        {
            Debug.LogError("MySceneLoader 未引用，无法加载场景！");
            // 备用方案：直接使用 Unity 内置的 SceneManager，但可能不是期望的行为
            // UnityEngine.SceneManagement.SceneManager.LoadScene("ExplorerMap");
        }
        isInEvent = false; // 设置为不在事件场景中
        RestoreState(); // 恢复玩家状态
    }

    // 恢复玩家状态
    private void RestoreState()
    {
        if (GameState.currentPlayerState != null)
        {
            transform.position = GameState.currentPlayerState.position;
            SanityManager.Instance.SetSanity(GameState.currentPlayerState.sanity); // 恢复理智值
            actionPoints = GameState.currentPlayerState.actionPoints; // 恢复行动点数
            Debug.Log("玩家状态已恢复");
        }
    }

    // 获取场景名称
    private string GetSceneNameByIndex(int index)
    {
        switch (index)
        {
            case 0:
                return "Scenes/ExplorerMap"; // 探索地图
            case 1:
                return "Scenes/MainCityEvent1"; // 事件场景 1
            case 2:
                return "Scenes/BattleScene"; // 战斗场景
            case 3:
                return "Scenes/Event1"; // 事件场景 1
            case 4:
                return "Scenes/Event2"; // 事件场景 2
            case 5:
                return "Scenes/Event3"; // 事件场景 3
            default:
                return "Scenes/ExplorerMap"; // 默认返回探索场景
        }
    }


    // 触发随机事件
    private void TriggerRandomEvent()
    {
        int randomIndex = Random.Range(0, eventSceneIndices.Length);
        int sceneToLoad = eventSceneIndices[randomIndex];
        LoadEventScene(sceneToLoad);
    }

    // 触发战斗
    private void TriggerBattle()
    {
        int sceneToLoad = eventSceneIndices[0]; // 假设战斗场景在事件场景索引中
        LoadEventScene(sceneToLoad);
    }

    // 处理理智归零的事件
    private void HandleSanityZero()
    {
        Debug.Log("玩家理智归零，游戏结束。");
        // 这里直接使用 Unity 内置的 SceneManager，确保在任何情况下都能加载游戏结束场景
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene"); // 加载游戏结束场景
    }

    // 公共方法：投骰子按钮
    public void RollDiceButton()
    {
        if (isInEvent)
        {
            Debug.LogWarning("当前正在事件中，无法投掷骰子。");
            return;
        }

        if (diceManager == null)
        {
            Debug.LogError("DiceManager未赋值！");
            return;
        }

        if (SanityManager.Instance.CurrentSanity <= 0)
        {
            Debug.LogWarning("理智值不足，无法投掷骰子。");
            return;
        }

        SanityManager.Instance.DecreaseSanity(dailySanityCost);

        if (SanityManager.Instance.CurrentSanity <= 0)
        {
            Debug.Log("理智值已归零，无法投掷骰子。");
            return;
        }

        actionPoints = diceManager.RollDice();
        Debug.Log($"获得行动点数: {actionPoints}");
    }

    // 公共方法：尝试移动到目标节点
    public void TryMoveToNode(Node targetNode)
    {
        if (isInEvent)
        {
            Debug.LogWarning("当前正在事件中，无法移动。");
            return;
        }

        if (SanityManager.Instance.CurrentSanity <= 0)
        {
            Debug.LogWarning("理智值不足，无法移动。");
            return;
        }

        if (CanMoveTo(targetNode))
        {
            StartCoroutine(MoveToNode(targetNode));
        }
    }

    // 私有方法：初始化节点位置
    private void InitAtNode(Node node)
    {
        currentNode = node;
        transform.position = node.transform.position + Vector3.up * playerHeight;
    }

    // 私有方法：检查是否可以移动到目标节点
    private bool CanMoveTo(Node targetNode)
    {
        if (isMoving)
        {
            Debug.LogWarning("正在移动中");
            return false;
        }

        if (actionPoints <= 0)
        {
            Debug.LogWarning("行动点数不足");
            return false;
        }

        if (!currentNode.connectedNodes.Contains(targetNode))
        {
            Debug.LogWarning("目标节点不相邻");
            return false;
        }

        return true;
    }

    // 私有方法：执行移动到目标节点
    private IEnumerator MoveToNode(Node targetNode)
    {
        isMoving = true;
        actionPoints--;

        Vector3 startPos = transform.position;
        Vector3 endPos = targetNode.transform.position + Vector3.up * playerHeight;
        float elapsed = 0f;

        while (elapsed < moveSpeed)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / moveSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        currentNode = targetNode;
        isMoving = false;
        Debug.Log($"移动完成，剩余点数: {actionPoints}");

        // 移动完成后，检查是否触发事件或战斗
        CheckForNodeEvent(currentNode);
    }

    // 启用或禁用玩家控制
    private void EnablePlayerControl(bool enable)
    {
        this.enabled = enable;
        Debug.Log($"玩家控制已 {(enable ? "启用" : "禁用")}");
    }

    // 检查当前节点是否触发事件
    private void CheckForNodeEvent(Node node)
    {
        switch (node.nodeType)
        {
            case NodeType.RandomEvent:
                TriggerRandomEvent();
                break;
            case NodeType.FixedEvent:
                if (node.fixedEventSceneIndex != -1)
                {
                    Debug.Log($"触发固定事件，加载场景索引: {node.fixedEventSceneIndex}");
                    LoadEventScene(node.fixedEventSceneIndex);
                }
                else
                {
                    Debug.LogWarning($"节点 {node.name} 的 NodeType 是 FixedEvent，但未指定 fixedEventSceneIndex。");
                }
                break;
            case NodeType.Battle:
                TriggerBattle();
                break;
            case NodeType.Normal:
                Debug.Log("当前节点是普通节点，无事件触发。");
                break;
            default:
                Debug.LogWarning($"未处理的节点类型: {node.nodeType}");
                break;
        }
    }
}
