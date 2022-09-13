using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeState
{
    FadeIn = 0,
    FadeOut,
    FadeInOut,
    FadeLoop
}
public class FadeEffect : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;     // ���� 10�̸� 1�� (���� Ŭ���� ����)
    public Image fadeImage;

    [Header("�ε��� ����ȭ��")]
    public Image background;
    [SerializeField]
    private Text gameTip;

    [Header("�ε�ȭ��")]
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField, TextArea]
    private string[] gameTips;

    [Header("���� �ε�ȭ��")]
    [SerializeField]
    private Sprite[] bossSprites;
    [SerializeField, TextArea]
    private string[] bossGameTips;

    private FadeState fadeState;

    private void Start()
    {
        // OnFade(FadeState.FadeInOut);
    }

    private void Update()
    {
        if(fadeImage.color.a == 1)
        {
            if (GameManager.Instance != null && GameManager.Instance.isPlayerEnterBossGround())
            {
                BossLoadingView();
                OnFade(FadeState.FadeIn);
            }
            else
            {
                RandomLoadingView();
                OnFade(FadeState.FadeInOut);
            }
        }
    }

    private void OnDisable()
    {
        if (fadeImage.color.a <= 1)
        {
            Color color = fadeImage.color;
            color.a = 1f;
            fadeImage.color = color;
        }
    }

    public void LoadingViewUpdate()
    {
        if (fadeImage.color.a == 1)
        {
            if (GameManager.Instance != null && GameManager.Instance.isPlayerEnterBossGround())
            {
                BossLoadingView();
                OnFade(FadeState.FadeIn);
            }
            else
            {
                RandomLoadingView();
                OnFade(FadeState.FadeInOut);
            }
        }
    }

    public void RandomLoadingView()
    {
        int index = Random.Range(0, sprites.Length);

        // �̹��� ���� ����
        Sprite spriteSelect = sprites[index];
        background.sprite = spriteSelect;

        // ������ ���� ����
        string gameTipSelect = gameTips[index].ToString();
        gameTip.text = $"����! {gameTipSelect}";
    }
    
    public void BossLoadingView()
    {
        // int index = Random.Range(0, bossSprites.Length);

        Sprite spriteSelect = bossSprites[0];
        background.sprite = spriteSelect;

        // ������ ���� ����
        string gameTipSelect = bossGameTips[0].ToString();
        gameTip.text = $"����! {gameTipSelect}";
    }

    public void OnFade(FadeState state)
    {
        fadeState = state;

        switch (fadeState)
        {
            case FadeState.FadeIn:  // Fade In
                StartCoroutine(Fade(1, 0));
                break;
            case FadeState.FadeOut: // Fade Out
                StartCoroutine(Fade(0, 1));
                break;
            case FadeState.FadeInOut:   // Fade ȿ���� In -> Out 1ȸ �ݺ�
            case FadeState.FadeLoop:    // Fade ȿ���� In -> Out ���� �ݺ�
                StartCoroutine(FadeInOut());
                break;
        }
    }

    private IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0));
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(Fade(0, 1));

            // 1ȸ�� ����ϴ� ������ ��
            if (fadeState == FadeState.FadeInOut)
            {
                break;
            }
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.unscaledDeltaTime;
            percent = currentTime / fadeTime;

            Color color = fadeImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            fadeImage.color = color;

            yield return null;
        }
    }
}
