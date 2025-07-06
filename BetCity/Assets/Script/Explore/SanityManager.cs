
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// ����ֵ�仯�¼�
[System.Serializable]
public class SanityEvent : UnityEvent<int> { } // ����Ϊ�仯������ֵ��

public class SanityManager : MonoBehaviour
{
    // �Ľ��ĵ���ʵ��
    private static SanityManager _instance;
    public static SanityManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SanityManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<SanityManager>();
                    singletonObject.name = typeof(SanityManager).ToString() + " (Singleton)";

                    // ȷ�������ڳ����л�ʱ��������
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    [SerializeField] private int maxSanity = 100;    // �������ֵ
    [SerializeField] private int currentSanity = 80; // ��ʼ����ֵ

    public int MaxSanity => maxSanity;
    public int CurrentSanity => currentSanity;

    public SanityEvent onSanityIncreased; // ����ֵ����ʱ����
    public SanityEvent onSanityDecreased; // ����ֵ����ʱ����
    public UnityEvent onSanityChanged;    // ����ֵ�仯ʱ���� (��������)
    public UnityEvent onSanityZero;       // ����ֵ����ʱ����

    private void Awake()
    {
        // ȷ��������ֻ��һ��ʵ��
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // ��ʼ���¼�
        if (onSanityIncreased == null)
            onSanityIncreased = new SanityEvent();

        if (onSanityDecreased == null)
            onSanityDecreased = new SanityEvent();

        if (onSanityChanged == null)
            onSanityChanged = new UnityEvent();

        if (onSanityZero == null)
            onSanityZero = new UnityEvent();
    }

    // ��Ӧ�ó����˳�ʱ�������˳�����ģʽ����������̬ʵ��
    private void OnApplicationQuit()
    {
        _instance = null; // ��վ�̬����
        // ��� SanityManager ��ͨ�����봴���� GameObject��
        // ����û�б� Destroy(gameObject) ���٣�
        // �����������ֶ�����������ͨ�� Unity ���Զ����� DontDestroyOnLoad �Ķ���
        // ����б�Ҫ���������� Debug.Log("SanityManager OnApplicationQuit called.");
    }

    // ��������ֵ
    public void IncreaseSanity(int amount)
    {
        if (amount <= 0) return;

        int oldValue = currentSanity;
        currentSanity = Mathf.Min(maxSanity, currentSanity + amount);

        if (currentSanity != oldValue)
        {
            onSanityIncreased?.Invoke(amount);
            onSanityChanged?.Invoke();
            Debug.Log($"����ֵ���� {amount}����ǰ����ֵ: {currentSanity}");
        }
    }

    // ��������ֵ
    public void DecreaseSanity(int amount)
    {
        if (amount <= 0) return;

        int oldValue = currentSanity;
        currentSanity = Mathf.Max(0, currentSanity - amount);

        if (currentSanity != oldValue)
        {
            onSanityDecreased?.Invoke(amount);
            onSanityChanged?.Invoke();
            Debug.Log($"����ֵ���� {amount}����ǰ����ֵ: {currentSanity}");

            if (currentSanity <= 0)
            {
                onSanityZero?.Invoke();
                Debug.Log("����ֵ���㣬��Ϸ������");
                // SceneManager.LoadScene("GameOverScene");
            }
        }
    }

    // ֱ����������ֵ
    public void SetSanity(int newSanity)
    {
        int oldValue = currentSanity;
        currentSanity = Mathf.Clamp(newSanity, 0, maxSanity);

        if (currentSanity != oldValue)
        {
            onSanityChanged?.Invoke();
            Debug.Log($"����ֵ����Ϊ {newSanity}����ǰ����ֵ: {currentSanity}");
            if (currentSanity <= 0)
            {
                onSanityZero?.Invoke();
                Debug.Log("����ֵ���㣬��Ϸ������");
            }
        }
    }
}