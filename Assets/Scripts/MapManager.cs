using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public GameObject[] outwallTile;
    public GameObject[] floorTile;
    public GameObject[] wallTile;
    public GameObject[] foodTile;
    public GameObject[] enemyTile;
    public GameObject exitPrefab;

    public int rows = 10;
    public int cols = 10;

    public int minWallnum = 2;
    public int maxWallnum = 8;

    private Transform mapHandle;
    private List<Vector2> positionList = new List<Vector2>();

    private ActionManager actionManager;


    // initialize map
    private void InitMap()
    {
        mapHandle = new GameObject("Map").transform;
        // outwall and floor
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                if (x == 0 || y == 0 || x == cols - 1 || y == rows - 1)
                {
                    int index = Random.Range(0, outwallTile.Length);
                    GameObject go = Instantiate(outwallTile[index], new Vector3(x, y, 0), Quaternion.identity);
                    go.transform.SetParent(mapHandle);
                }
                else
                {
                    int index = Random.Range(0, floorTile.Length);
                    GameObject go = Instantiate(floorTile[index], new Vector3(x, y, 0), Quaternion.identity);
                    go.transform.SetParent(mapHandle);
                }
            }
        }
        
        positionList.Clear();
        for(int x = 2; x < rows-2; x++)
        {
            for (int y = 2; y < cols - 2; y++)
                positionList.Add(new Vector2(x, y));
        }
        // wall
        int wallNum = Random.Range(minWallnum, maxWallnum + 1);
        InstantitateItems(wallNum, wallTile);
        // food [ 2 -- level*2 ]
        int foodCount = Random.Range(2, actionManager.level*2 - 1);
        InstantitateItems(foodCount, foodTile);
        // enemy level/2
        int enemyCount = actionManager.level / 2;
        InstantitateItems(enemyCount, enemyTile);
        // exit
        GameObject exit = Instantiate(exitPrefab, new Vector2(rows - 2, cols - 2), Quaternion.identity);
        exit.transform.SetParent(mapHandle);

    }

    private void InstantitateItems(int count, GameObject[] prefabsTile)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = RandomPosition();
            GameObject Prefabs = RandomPrefabs(prefabsTile);
            GameObject item = Instantiate(Prefabs, pos, Quaternion.identity);
            item.transform.SetParent(mapHandle);
        }
    }

    private Vector2 RandomPosition()
    {
        int positionIndex = Random.Range(0, positionList.Count);
        Vector2 pos = positionList[positionIndex];
        positionList.RemoveAt(positionIndex);
        return pos;
    }

    private GameObject RandomPrefabs(GameObject[] prefabs)
    {
        int index = Random.Range(0, prefabs.Length);
        return prefabs[index];
    }

    // Start is called before the first frame update
    void Awake()
    {
        actionManager = this.GetComponent<ActionManager>();
        InitMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
