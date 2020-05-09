using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class LevelManager : MonoBehaviour
{
    //creates a map from rooms

    //size of each room
    const int roomSize = 500;
    
    public void CreateRoomLevel(int length)
    {
        ////////////////////////////////////////////////////////////////
        
        GameObject map = GameObject.Find("Map");
        List<Vector3> vectorList = new List<Vector3>(); //positions for rooms
        List<GameObject> rooms = new List<GameObject>(); //room objects
        List<GameObject> specialRooms = new List<GameObject>(); //rooms with special placement

        ////////////////////////////////////////////////////////////////
        // ADD ROOM POSITIONS
        ////////////////////////////////////////////////////////////////

        for ( int i = 0; i < length; i++)
        {
            vectorList.Add(new Vector3(0, 0, i * roomSize));
        }

        ////////////////////////////////////////////////////////////////
        // GET AVAILABLE ROOMS
        ////////////////////////////////////////////////////////////////

        foreach ( Transform t in map.transform)
        {
            if (t.GetComponent<Room>().startRoom || t.GetComponent<Room>().endRoom)
            {
                specialRooms.Add(t.gameObject);
            }
            else
            {
                rooms.Add(t.gameObject);
            }
        }

        ////////////////////////////////////////////////////////////////
        // DESTROY AND RECREATE MAP
        ////////////////////////////////////////////////////////////////

        Destroy( map);
        map = new GameObject();
        map.name = "Map";
        //navmeshsurface to rebuild navmesh
        map.AddComponent<NavMeshSurface>();

        ////////////////////////////////////////////////////////////////
        // PLACE THE ROOMS
        ////////////////////////////////////////////////////////////////

        int counter = 1;
        foreach (Vector3 vec in vectorList)
        {
            bool special = false;
            foreach(GameObject go in specialRooms)
            {
                if (go.GetComponent<Room>().GetPosY() == vec.z / roomSize)
                {
                    GameObject room = Instantiate(go, vec, Quaternion.identity);
                    room.transform.parent = map.transform;
                    room.name += counter;
                    special = true;
                }
            }

            if(special == false)
            {
                GameObject room = Instantiate(rooms[Random.Range(0, rooms.Count)], vec, Quaternion.identity);
                room.transform.parent = map.transform;
                room.name += counter;
            }
            counter++;
        }
        
        map.GetComponent<NavMeshSurface>().BuildNavMesh();

        DebugManager.GetInstance().Print( "GameManager", "LevelManager", "Info: Built map" );
       
        ////////////////////////////////////////////////////////////////
        // INITIALIZE ENEMIES
        ////////////////////////////////////////////////////////////////

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach ( Enemy enemy in enemies )
            enemy.Initialize();

        AnimatedObstacle[] obstacles = FindObjectsOfType<AnimatedObstacle>();
        foreach ( AnimatedObstacle obstacle in obstacles )
            obstacle.Initialize();
        
        ////////////////////////////////////////////////////////////////

        DebugManager.GetInstance().Print( "GameManager", "LevelManager", "Info: Initialized Enemies" );
    }
    
    ////////////////////////////////////////////////////////////////

    public void CreatePointDefenseLevel(int length)
    {
        ////////////////////////////////////////////////////////////////
        
        GameObject map = GameObject.Find("Map");

        //navmeshsurface to rebuild navmesh
        map.AddComponent<NavMeshSurface>();

        //rebuild navmesh
        map.GetComponent<NavMeshSurface>().BuildNavMesh();

        Debug.Log("Info: Built map");
        
        ////////////////////////////////////////////////////////////////
        // INITIALIZE ENEMIES
        ////////////////////////////////////////////////////////////////

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach ( Enemy enemy in enemies )
            enemy.Initialize();

        AnimatedObstacle[] obstacles = FindObjectsOfType<AnimatedObstacle>();
        foreach ( AnimatedObstacle obstacle in obstacles )
            obstacle.Initialize();

        ////////////////////////////////////////////////////////////////

        Debug.Log("Info: Initialized Enemies");
    }
}
