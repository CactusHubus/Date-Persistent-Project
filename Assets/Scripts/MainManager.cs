using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text highScoreText;
    public Text highNameText;

    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;

    public static int high_score;
    public static string high_name;

    private bool m_GameOver = false;

    public string NameText;

    public static MainManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        LoadScore();

        MainManager.Instance.highScoreText.text = high_score.ToString();
        MainManager.Instance.highNameText.text = high_name;

        NameText = GameManager.inputNameText;
        AddPoint(0);

        m_Started = false;

        m_GameOver = false;


        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{NameText} Score : {m_Points}";

        if (high_score == 0)
        {
            high_name = NameText;
            highNameText.text = $"{high_name} - ";
            highScoreText.text = $"High Score : {high_score}";
        }

        if (m_Points == 0)
        {
            highNameText.text = $"{high_name} - ";
            highScoreText.text = $"High Score : {high_score}";
        }

        if (m_Points > high_score)
        {
            high_score = m_Points;
            high_name = NameText;

            highNameText.text = $"{high_name} - ";
            highScoreText.text = $"High Score : {high_score}";
        }
        
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadSceneAsync(0);

        SaveScore();


    }

    [System.Serializable]
    class SaveData
    {
        public int high_score;
        public string high_name;
    }
    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            high_score = data.high_score;
            high_name = data.high_name;
        }
    }
    public void SaveScore()
    {
        SaveData data = new SaveData();

        data.high_score = high_score;
        data.high_name = high_name;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }


}
