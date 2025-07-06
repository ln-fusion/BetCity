
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // <-- ������һ��

public class SanityUIController : MonoBehaviour
{
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Slider sanitySlider;
    [SerializeField] private TextMeshProUGUI sanityText; // <-- �޸���һ��
    [SerializeField] private int changeAmount = 10; // ÿ����������

    // ���ػ��ı����������Localizationϵͳʹ�ã�
    [SerializeField] private string sanityTextFormat = "Sanity: {0} / {1}"; // ���� "HP: {0} / {1}" ��


    // ȷ��UI�ڳ����л�ʱΨһ
    private static bool instanceExists;

    private void Awake()
    {
        // ����ģʽ��ֹ���UIʵ��
        if (!instanceExists)
        {
            instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // ��֤UI�������
        ValidateReferences();
    }

    private void Start()
    {
        // ȷ�� SanityManager �Ѿ���ʼ��
        SanityManager.Instance.ToString(); // ����һ�� Instance �����Դ����� Awake/��������

        UpdateSanityUI();
        SetupButtonEvents();
        SetupEventListeners();

        // �������������¼�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // �����¼���������ֹ�ڴ�й©
        RemoveEventListeners();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �������غ����UI
        UpdateSanityUI();
    }

    private void ValidateReferences()
    {
        // ���UI����Ƿ���ȷ��ֵ
        if (increaseButton == null)
            Debug.LogError("increaseButtonδ��ֵ��");

        if (decreaseButton == null)
            Debug.LogError("decreaseButtonδ��ֵ��");

        if (sanitySlider == null)
            Debug.LogError("sanitySliderδ��ֵ��");

        if (sanityText == null)
            Debug.LogError("sanityTextδ��ֵ��");
    }

    private void SetupButtonEvents()
    {
        // ��ť�¼���
        if (increaseButton != null)
            increaseButton.onClick.AddListener(() => SanityManager.Instance.IncreaseSanity(changeAmount));

        if (decreaseButton != null)
            decreaseButton.onClick.AddListener(() => SanityManager.Instance.DecreaseSanity(changeAmount));
    }

    private void SetupEventListeners()
    {
        // ���� SanityManager ������ֵ�仯�¼�
        if (SanityManager.Instance != null)
        {
            SanityManager.Instance.onSanityChanged.AddListener(UpdateSanityUI);
            // �����Ҫ��Ҳ���Լ��� onSanityIncreased �� onSanityDecreased
            // SanityManager.Instance.onSanityIncreased.AddListener((amount) => Debug.Log($"UI �յ���������֪ͨ: {amount}"));
            // SanityManager.Instance.onSanityDecreased.AddListener((amount) => Debug.Log($"UI �յ����Ǽ���֪ͨ: {amount}"));
        }
    }

    private void RemoveEventListeners()
    {
        if (SanityManager.Instance != null)
        {
            SanityManager.Instance.onSanityChanged.RemoveListener(UpdateSanityUI);
        }
    }

    // ����UI��ʾ
    public void UpdateSanityUI()
    {
        if (SanityManager.Instance == null) return;

        int current = SanityManager.Instance.CurrentSanity;
        int max = SanityManager.Instance.MaxSanity;

        if (sanitySlider != null)
        {
            sanitySlider.maxValue = max;
            sanitySlider.value = current;
        }

        if (sanityText != null)
        {
            sanityText.text = string.Format(sanityTextFormat, current, max);
        }
    }
}