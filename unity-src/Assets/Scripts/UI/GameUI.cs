using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // 인스턴스 및 일시정지 상태 변수 선언
    public static GameUI instance;
    public bool isPausing;

    // 텍스트 , 게이지 변수 선언
    public Text comboText;
    public Text timerText;
    public Text speedText;
    public Text countDownTimer;
    public Text timeIncreaseText;
    public Text hpIncreaseText;
    public UIBarScript barHp;

    // 타이머  변수 선언
    public float timer;
    public float playTime;
    public float comboDisplayTimer;
    public float damageEffectTimer;

    void Awake() { instance = this; }

    void Start()
    {
        isPausing = false;
        InitialGameTimer();
        Show();
    }

    // 게임 타이머 초기화
    void InitialGameTimer() 
    {
        comboDisplayTimer = 0;
        damageEffectTimer = 0;
        timer = ConstInfo.gameTime;
        playTime = 0;
    }



    // UI 보여주기
    public void Show()
    {
        GameManager.instance.SetGameState(GameState.Game);
        transform.gameObject.SetActive(true);
    }



    // 정확한 시간 측정을 위해 FixedUpdate 사용
    void FixedUpdate()
    {
        if (GameManager.instance.GetGameState() == GameState.Game)
        {
            HandleTime();
            HandleGameUI();
        }
    }

    // 타이머 변수 설정
    void HandleTime()
    {
        playTime += Time.fixedDeltaTime;
        if (Setting.GetCurrentTimeState() == TimeState.Normal) 
        {
            timer = Mathf.Round((timer - Time.fixedDeltaTime) * 100) / 100;
            if (timer < 0)
                timer = 0;
        }
    }

    // 게임 UI 업데이트
    void HandleGameUI()
    {
        HandleGameText();
        HandleEffect();
        barHp.UpdateValue(Player.instance.hp, ConstInfo.maxHp);

    }


    // 상태 텍스트 설정 (시간, 콤보, 속도)
    void HandleGameText()
    {
        HandleTimeText();
        HandleComboText();
        HandleSpeedText();

    }

    // 상태 이펙트 설정 (시간 증가, 체력 증가, 피격)
    void HandleEffect() {
        HandleIncreaseText(timeIncreaseText, timerText);
        HandleIncreaseText(hpIncreaseText, null);
        ShowDamageEffect();
    }



    // 속도 텍스트 업데이트
    void HandleSpeedText() { speedText.text = ActualSpeedToDisplaySpeed(Tile.actualSpeed).ToString("#0.00") + " km/s"; }

    // 타일 실제 속도를 UI상의 속도로 변환 (30 ~ 90 -> 5km/s ~ 20km/s)
    float ActualSpeedToDisplaySpeed(float actualSpeed) { return Mathf.Round((((actualSpeed - ConstInfo.initialActualSpeed) / 4) + 5) * 100) / 100; }



    // 콤보 표시 타이머에 따른 콤보 텍스트 업데이트 (ConstInfo.comboDisplayTime 에 따른)
    void HandleComboText()
    {
        if (comboDisplayTimer > 0)
            comboDisplayTimer -= Time.fixedDeltaTime;
        else
            comboText.text = "";
    }



    // 시간 설정에 따른 시간 텍스트 업데이트
    void HandleTimeText() {
        if (Setting.GetCurrentTimeState() == TimeState.Normal)
            timerText.text = timer.ToString("Time : 00.00");
        else if (Setting.GetCurrentTimeState() == TimeState.Infinite)
            timerText.text = "Infinite";

        if (timer < 5)
            HandleCountDownText();
        else
            countDownTimer.text = "";

    }

    // 5초 이하로 남았을 경우 카운트다운 텍스트 업데이트
    void HandleCountDownText() {
        if (timer > 0)
            countDownTimer.text = Mathf.Floor(timer + 1).ToString();
        else
            countDownTimer.text = "";
    }


    public void DamageEffectTrigger() { GameUI.instance.damageEffectTimer = ConstInfo.damageEffectTime; }

    // 피격된 경우 (이펙트 생성)
    public void ShowDamageEffect()
    {
        if (damageEffectTimer > 0)
        {
            GetComponent<Image>().color = new Color(1, 0, 0, 0.3f);
            damageEffectTimer -= Time.fixedDeltaTime;
        }
        else
            GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }

    // 콤보 변경된 경우 (증가) ConstInfo.comboDisplayTime 동안 텍스트 표시
    public void ChangeCombo(int newCombo)
    {
        if (Player.instance.maxCombo <= newCombo)
            Player.instance.maxCombo = newCombo;
        comboText.text = newCombo.ToString() + " COMBO";
        comboDisplayTimer = ConstInfo.comboDisplayTime;
    }

    // 증가 이펙트용 텍스트 보여주기 (흰색)
    public void ShowTimeIncreaseText() { timeIncreaseText.color = Color.white; }
    public void ShowHpIncreaseText() { hpIncreaseText.color = Color.white; }


    // 효과 텍스트의 알파값이 0.3까지 떨어진 뒤 삭제, test2는 초록색 효과
    void HandleIncreaseText(Text text, Text text2) 
    {
        if (text.color.a > 0.3)
            text.color = new Color(1, 1, 1, text.color.a - ConstInfo.increaseTextAlphaDecrease);
        else
            text.color = new Color(1, 1, 1, 0);
        text.GetComponent<Outline>().effectColor = new Color(0, 0, 0, text.color.a / 2);
        if (text2)
            text2.color = new Color(1 - (text.color.a), 1, 1 - (text.color.a));

    }



    // 일시정지 설정, 해제 (점프 시 점프상태 조기화)
    public void Pause()
    {
        if (isPausing)
            GameManager.instance.SetGameState(GameState.Game);
        else
            GameManager.instance.SetGameState(GameState.Pause);
        isPausing = !isPausing;
        Player.instance.InitialJumpState();
    }

    // 새 게임 버튼을 누른 경우
    public void HandleNewGame() { SceneManager.LoadScene("Game"); }

    // 메뉴로 버튼을 누른 경우
    public void HandleToMenu() { SceneManager.LoadScene("Menu"); }

    // 게임이 종료된 경우
    public void HandleGameEnd(int maxCombo, int point)
    {
        ResultUI.instance.Show(maxCombo, point, playTime, InsertRank(point));
        transform.gameObject.SetActive(false);
    }

    // 랭킹 등록 알고리즘
    public string InsertRank(int score)
    {
        int myRank = 0;
        List<int> scores = new List<int>();
        scores.Add(-1 * score);
        for (int i = 0; i < 5; i++)
            scores.Add(-1 * PlayerPrefs.GetInt(i.ToString()));
        scores.Sort();
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt(i.ToString(), scores[i] * -1);

            if (scores[i] == -1 * score)
                myRank = i + 1;
        }
        if (myRank == 0)
            return "순위권에 들지 못했습니다.";
        else
            return "내 순위 : " + myRank.ToString();
    }
}


