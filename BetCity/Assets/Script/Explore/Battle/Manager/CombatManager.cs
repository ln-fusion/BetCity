using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance; // 单例模式方便访问

    public Tilemap combatGrid; // 引用Tilemap
    public List<Card> publicDeck; // 公共牌库
    public List<Card> DiscardPile; // A方弃牌堆

    private List<Card> playerAHand; // A方手牌
    private List<Card> playerBHand; // B方手牌

    private CardOwner currentPlayer; // 当前回合玩家

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        // 初始化牌库、手牌等
        // 根据策划文档，将双方25张卡牌混合洗牌后形成公共牌库
        // 双方各抽5张手牌
        // 确定先手玩家
    }

    public void StartTurn()
    {
        // 回合开始逻辑
        // 判断是否为后手第一回合特权
        // 进入抽卡阶段
    }

    private void DrawPhase()
    {
        // 抽卡逻辑：投掷d4，抽取X张卡
        // 牌库重置逻辑
        // 抛掷硬币从对手手牌抽卡逻辑
        // 对手手牌不足惩罚逻辑
    }

    private void PlayPhase()
    {
        // 出牌逻辑：打出从对手手牌抽取的卡牌
        // 卡牌效果结算逻辑
    }

    private void CombinationPhase()
    {
        // 特殊组合结算逻辑
    }

    private void EndPhase()
    {
        // 结束阶段逻辑：手牌上限、胜负判断
    }

    // 其他辅助方法，例如：
    // public bool IsGridOccupied(Vector3Int gridPosition)
    // public void PlaceCardOnGrid(Card card, Vector3Int gridPosition)
    // public void RemoveCardFromGrid(Vector3Int gridPosition)
}
