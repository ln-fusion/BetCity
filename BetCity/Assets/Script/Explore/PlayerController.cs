using UnityEngine;
using System.Collections; // 协程所需

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

    // ========== 私有变量 ==========
    private Node currentNode;
    private int actionPoints;
    private bool isMoving;

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

        // 确保 SanityManager 已经初始化
        SanityManager.Instance.ToString(); // 访问一次 Instance 属性以触发其 Awake/单例创建

        // 监听理智归零事件，以便在游戏结束时停止玩家操作
        SanityManager.Instance.onSanityZero.AddListener(HandleSanityZero);
    }

    private void OnDestroy()
    {
        // 移除事件监听，防止内存泄漏
        if (SanityManager.Instance != null)
        {
            SanityManager.Instance.onSanityZero.RemoveListener(HandleSanityZero);
        }
    }

    // ========== 公共方法 ==========
    public void RollDiceButton()
    {
        if (diceManager == null)
        {
            Debug.LogError("DiceManager未赋值！");
            return;
        }

        // 检查理智值是否足够进行投骰子操作
        if (SanityManager.Instance.CurrentSanity <= 0)
        {
            Debug.LogWarning("理智值不足，无法投掷骰子。");
            return;
        }

        // 每日消耗理智
        SanityManager.Instance.DecreaseSanity(dailySanityCost);

        // 如果理智归零，则不再投骰子
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
    }

    // 处理理智归零的事件
    private void HandleSanityZero()
    {
        // 可以在这里添加游戏结束的视觉或音效反馈
        Debug.Log("玩家控制器收到理智归零通知，停止所有行动。");
        // 例如，禁用玩家输入，显示游戏结束画面等
        // this.enabled = false; // 禁用 PlayerController 脚本
    }
}
