using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionManager : MonoBehaviour
{
    public int level = 1;
    public int food = 10;
    [HideInInspector]  public bool Clearance = false;
    public List<Enemy> enemyList = new List<Enemy>();

    private Player player;
    private MapManager map;    
    private bool sleepStep = true;
    private Text foodText;
    private Text lossText;

    private static ActionManager _instance;

    public static ActionManager Instance
    {
        get{
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitGame()
    {
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        lossText = GameObject.Find("LossText").GetComponent<Text>();
        lossText.enabled = false;
        UpdateFoodText(0);
        player = GameObject.FindGameObjectWithTag("Protag").GetComponent<Player>();
        map = GetComponent<MapManager>();
    }

    public void UpdateFoodText(int foodChange)
    {
        if (foodChange == 0)
        {
            foodText.text = "Food:" + food;
        }
        else
        {
            string statusText = "";
            if (foodChange < 0)
            {
                statusText = foodChange.ToString();
            }
            else
            {
                statusText = "+" + foodChange.ToString();
            }
            foodText.text = "Food:" + food + " " + statusText;
        }      
    }

    public void Addfood(int count)
    {
        food += count;
        UpdateFoodText(count);
    }

    public void Reducefood(int count)
    {
        food -= count;
        UpdateFoodText(-count);
        if (food <= 0)
        {
            lossText.enabled = true;
        }
    }


    public void OnPlayerMove()
    {
        if (sleepStep)
        {
            sleepStep = false;
        }
        else
        {
            foreach(var enemy in enemyList)
            {
                enemy.Move();
            }
            sleepStep = true;
        }
        // Clearance 
        if (player.tagPosition.x == map.rows - 2 && player.tagPosition.y == map.cols - 2)
        {
            Clearance = true;
            // Next Level
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
