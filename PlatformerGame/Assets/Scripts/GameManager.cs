using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    private const int FIRST_GAMING_LEVEL = 1;
    private const int SECOND_GAMING_LEVEL = 2;
    private const int LAST_GAMING_LEVEL = 3;
    private const int MAX_LIVES = 3;

    private int actualLevel = 0;

    private int score = 0;
    private string playerName = "CapitaineCanard";
    private int lives = MAX_LIVES;
    private int bonus = 0;

    bool scenesAreInTransition = false;

    Text playerNameText;
    Text playerScoreText;
    Text playerLivesText;
    Text playerBonusText;

    private InputAction cancelAction;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        actualLevel = SceneManager.GetActiveScene().buildIndex;
        cancelAction = InputSystem.actions.FindAction("Cancel");
    }

    void Update()
    {
        if (cancelAction.WasPerformedThisFrame())
        {
            Quit();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // This method will be called every time a scene is loaded
        //Debug.Log("The persistent GameManager has arrived in a new scene: " + scene.name);
        //Debug.Log(mode);

        //La scène 0 n'a pas d'élément de UI
        if (scene.name == "Scene0") return;

        playerNameText = GameObject.FindWithTag("TextName").GetComponent<Text>();
        playerScoreText = GameObject.FindWithTag("TextScore").GetComponent<Text>();
        playerLivesText = GameObject.FindWithTag("TextLives").GetComponent<Text>();
        playerBonusText = GameObject.FindWithTag("TextBonus").GetComponent<Text>();

        playerNameText.text = playerName;
        playerScoreText.text = score.ToString();
        playerLivesText.text = lives.ToString();
        playerBonusText.text = bonus.ToString();

    }

    //Méthode alternative: l'objet après sa création se lie au GameManager
    public void LinkText(Text textToLink)
    {
        //switch (textToLink.gameObject.tag)
        //{
        //    case "TextName":
        //        playerNameText = textToLink;
        //        playerNameText.text = playerName;
        //        break;

        //    case "TextLives":
        //        playerLivesText = textToLink;
        //        playerLivesText.text = lives.ToString();
        //        break;

        //    case "TextScore":
        //        playerScoreText = textToLink;
        //        playerScoreText.text = score.ToString();
        //        break;

        //    case "TextBonus":
        //        playerBonusText = textToLink;
        //        playerBonusText.text = bonus.ToString();
        //        break;
        //}
    }

    public void RestartLevel(float delay)
    {
        if (scenesAreInTransition) return;  //Pour éviter plusieurs transitions lancées en bloc

        scenesAreInTransition = true;

        StartCoroutine(RestartLevelDelay(delay, actualLevel));
    }


    public void StartNextlevel(float delay)
    {
        if (scenesAreInTransition) return;  //Pour éviter plusieurs transitions lancées en bloc

        scenesAreInTransition = true;

        StartCoroutine(RestartLevelDelay(delay, GetNextLevel()));
    }

    private IEnumerator RestartLevelDelay(float delay, int level)
    {
        yield return new WaitForSeconds(delay);
        //textsNotLinked = true;

        if (lives == 0)
        {
            bonus = 0;
            SceneManager.LoadScene("SceneGameOver");
        }
        else if (level == FIRST_GAMING_LEVEL)
            SceneManager.LoadScene("Scene1");
        else if (level == SECOND_GAMING_LEVEL)
            SceneManager.LoadScene("Scene2");
        else
            SceneManager.LoadScene("Scene3");

        scenesAreInTransition = false;
    }

    public void ResetGame()
    {
        lives = MAX_LIVES;
        actualLevel = 0;
        score = 0;
        SceneManager.LoadScene("Scene0");
    }
    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    private int GetNextLevel()
    {
        if (++actualLevel == LAST_GAMING_LEVEL + 1)
            actualLevel = FIRST_GAMING_LEVEL;

        return actualLevel;
    }

    private void Quit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    //Pour éviter des leaks potentiels
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //---------------------------------------------------------------
    //Role "traditionnel" du Game Manager: petit donc on le garde ici
    //---------------------------------------------------------------
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        playerScoreText.text = score.ToString();
    }

    public void PlayerDie()
    {
        lives--;
        playerLivesText.text = lives.ToString();
    }

    public void SetBonus(int bonus)
    {
        this.bonus = bonus;

        if (playerBonusText == null) return;

        playerBonusText.text = this.bonus.ToString();
    }
}