using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // 确保引入 SceneManagement

public class PlayerController : MonoBehaviour
{
    // ========== 公开字段 ==========
    [Header("组件引用")]
    [SerializeField] private DiceManager diceManager;
    [SerializeField] private Node startNode;

    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float playerHeight = 0.5f;

    [Header("理智消耗设置")]
    [SerializeField] private int dailySanityCost = 5; // 每天消耗的理智值

    [Header("场景索引")]
    [SerializeField] private int battleSceneIndex = 2; // 战斗场景索引，根据截图应为 2
    [SerializeField] private int gameOverSceneIndex = 5; // 游戏结束场景索引 (假设您会有一个游戏结束场景，这里暂时用 Event3 的索引作为示例，您需要替换为实际的游戏结束场景索引)
    // 假设您的事件场景索引是 1, 3, 4, 5 (MainCityEvent1, Event1, Event2, Event3)
    [SerializeField] private int[] eventSceneIndices = {2, 3, 4, 5 }; // 对应 Battle, Event1, Event2, Event3



    // ========== 私有变量 ==========
    private Node currentNode;
    private int actionPoints;
    private bool isMoving;
    private bool isInEvent = false; // 新增：标记是否在事件场景中

    // ========== 初始化 ==========
    private void Start()
    {
        if (startNode != null)
        {
            InitAtNode(startNode);
            Debug.Log($"玩家已初始化到节点: {startNode.name}");
        }
        else
        {
            Debug.LogError("未指定起始节点！");
        }

        SanityManager.Instance.ToString();
        SanityManager.Instance.onSanityZero.AddListener(HandleSanityZero);

        // 监听场景卸载事件，以便在事件场景卸载后重新启用玩家控制
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDestroy()
    {
        if (SanityManager.Instance != null)
        {
            SanityManager.Instance.onSanityZero.RemoveListener(HandleSanityZero);
        }
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // 当场景卸载时调用
    private void OnSceneUnloaded(Scene scene)
    {
        // 检查卸载的场景是否是我们的事件场景之一
        foreach (int index in eventSceneIndices)
        {
            if (scene.buildIndex == index || scene.buildIndex == battleSceneIndex)
            {
                Debug.Log($"事件/战斗场景 {scene.name} 已卸载，重新启用玩家控制。");
                isInEvent = false;
                EnablePlayerControl(true); // 重新启用玩家控制
                break;
            }
        }
    }

    // ========== 公共方法 ==========
    public void RollDiceButton()
    {
        if (isInEvent) // 如果在事件中，不允许投骰子
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

    public void TryMoveToNode(Node targetNode)
    {
        if (isInEvent) // 如果在事件中，不允许移动
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

    // ========== 私有方法 ==========
    private void InitAtNode(Node node)
    {
        currentNode = node;
        transform.position = node.transform.position + Vector3.up * playerHeight;
    }

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
        // 禁用或启用 PlayerController 脚本本身
        this.enabled = enable;
        // 如果有其他输入管理脚本，也需要在这里禁用/启用
        // 例如：GetComponent<PlayerInput>().enabled = enable;
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
                    LoadAdditiveScene(node.fixedEventSceneIndex);
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
    // 触发随机事件
    private void TriggerRandomEvent()
    {
        if (eventSceneIndices.Length == 0)
        {
            Debug.LogWarning("未配置事件场景索引！");
            return;
        }

        int randomIndex = Random.Range(0, eventSceneIndices.Length);
        int sceneToLoad = eventSceneIndices[randomIndex];

        Debug.Log($"触发随机事件，加载场景索引: {sceneToLoad}");
        LoadAdditiveScene(sceneToLoad);
    }

    // 触发战斗
    private void TriggerBattle()
    {
        Debug.Log($"触发战斗，加载场景索引: {battleSceneIndex}");
        LoadAdditiveScene(battleSceneIndex);
    }

    // 加载叠加场景
    private void LoadAdditiveScene(int sceneIndex)
    {
        isInEvent = true;
        EnablePlayerControl(false); // 禁用玩家控制
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
    }

    // 处理理智归零的事件
    private void HandleSanityZero()
    {
        Debug.Log("玩家控制器收到理智归零通知，游戏结束。");
        EnablePlayerControl(false); // 禁用玩家控制
        // 加载游戏结束场景，这里使用 LoadScene 而不是 Additive，因为游戏已经结束
        SceneManager.LoadScene(gameOverSceneIndex);
    }
}
