using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject playerPrefab;



    public Text scoreText;
    public Text ballsText;
    public Text levelText;
    public Text highscoreText; 

    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLevelCompleted;
    public GameObject panelGameOver;

    public GameObject[] levels;
    //gameManagere ulaşmak için 
    public static GameManager Instance { get; private set; }

    public enum State { MENU , INIT, PLAY , LEVELCOMPLETED , LOADLEVEL , GAMEOVER }
    State _state;

    GameObject _currentBall;
    GameObject _currentLevel;
    bool _isSwitchingState;

    //propfull ve 2 kere tab 
    private int _score;

    public int Score
    {
        get { return _score; }
        set { _score = value;
            scoreText.text = "SCORE: " + _score;
        }

    }

    private int _level;

    public int Level
    {
        get { return _level; }
        set { _level = value;
            levelText.text = "LEVEL: " + _level;
        }
    }

    private int _balls;

    public int Balls
    {
        get { return _balls; }
        set { _balls = value;
            ballsText.text = "BALLS: " + _balls;
        }
    }



    public void PlayClicked()
    {
        SwitchState(State.INIT);
    }

    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);
    }

    public void SwitchState(State newState,float delay = 0)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }
    
    IEnumerator SwitchDelay(State newState , float delay)
    {
        _isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        //panellerin ayarı için
        EndState();
        _state = newState;
        BeginState(newState);
        _isSwitchingState = false;
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                //play butonuna tıklamak için mouse'u burda visible yapmadık.
                Cursor.visible = true;
                highscoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("highscore");
                panelMenu.SetActive(true);
                break;
            case State.INIT:
                //mouse ile sağ sol yaparken yukarı-aşağı oluşan hareketliliği kaldırdık. 
                Cursor.visible = false;
                panelPlay.SetActive(true);
                //başlangıçta score level top değerlerini atayıp.
                //ınstantiate ile de player ve top objelerimizi çıkarıcaz
                Score = 0;
                Level = 0;
                Balls = 3;
                //GameOver olduktan yeni oyuna başlayınca leveli resetlemek için
                if(_currentLevel != null)
                {
                    Destroy(_currentLevel);
                }
                Instantiate(playerPrefab);
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                //level complete olduğun complete yazısından önce oyunun durmasını istiyoruz ve topun yok olmasını
                Destroy(_currentBall);
                Destroy(_currentLevel);
                Level++;
                panelLevelCompleted.SetActive(true);
                SwitchState(State.LOADLEVEL,2f);
                break;
            case State.LOADLEVEL:
                //eğer son levele geldiysek game over panelini çağırıyoruz
                if(Level >= levels.Length)
                {
                    SwitchState(State.GAMEOVER);
                }
                else
                {
                    //değilse şimdiki leveli yeni level seviyesi ile değişiyoruz
                    _currentLevel = Instantiate(levels[Level]);
                    //yeni leveli getirdikden sonra tekrar play paneline gidiyoruz
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                //game over olduğunda unity bize yüksek skoru saklamamız için playerprefs ile yardımcı oluyor
                if(Score > PlayerPrefs.GetInt("highscore"))
                {
                    PlayerPrefs.SetInt("highscore", Score);
                }
                panelGameOver.SetActive(true);
                break;
        } 
    }



    void Update()
    {
        switch (_state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if(_currentBall == null)
                {
                    //eğer top yoksa durumunda
                    if(Balls > 0)
                    {
                        //eğer şimdiki senaryomuzda top yoksa ve top hakkımızda 0 dan büyükse sahneye topu çıkarıyoruz
                        _currentBall = Instantiate(ballPrefab);
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER);
                    }
                }

                //kırılacak brick kalmadığında bir sonraki level seviyesine geçmesi için
                if(_currentLevel != null && _currentLevel.transform.childCount == 0 && !_isSwitchingState)
                {
                    SwitchState(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                //mesela levellerimiz bitti ve sonsuz game over döngüsünü kırmak için 
                if (Input.anyKeyDown)
                {
                    SwitchState(State.MENU);
                }
                break;
        }
    }

    void EndState()
    {
        switch (_state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                panelLevelCompleted.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelPlay.SetActive(false);
                panelGameOver.SetActive(false);
                break;
        }
    }








}
