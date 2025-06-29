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
    }

    // ========== 公共方法 ==========
    public void RollDiceButton()
    {
        if (diceManager == null)
        {
            Debug.LogError("DiceManager未赋值！");
            return;
        }

        actionPoints = diceManager.RollDice();
        Debug.Log($"获得行动点数: {actionPoints}");
    }

    public void TryMoveToNode(Node targetNode)
    {
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
}