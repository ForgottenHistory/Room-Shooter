using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BulletStockpile : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////
    //
    //                      BULLET STOCKPILE
    //
    // stockpile for all enemy bullets
    // enemy loans bullet to shoot
    // shared stockpile creates less gameobjects than individual stockpile
    //
    ////////////////////////////////////////////////////////////////

    Dictionary<string, GameObject> prefabBullets = new Dictionary<string, GameObject>(); //prefab bullets
    Dictionary<string, int> toMake = new Dictionary<string, int>(); //string name, int amount to make
    Dictionary<string, Dictionary<string, int>> users = new Dictionary<string, Dictionary<string, int>>(); //room, bullet, users
    Dictionary<string, int> created = new Dictionary<string, int>(); //string name, int amount to make
    Dictionary<string, int> inventory = new Dictionary<string, int>(); //string name, int currentbullet
    List<string> rooms = new List<string>();

    ////////////////////////////////////////////////////////////////
    // INITIALIZE
    ////////////////////////////////////////////////////////////////

    public void Initialize()
    {
        ////////////////////////////////////////////////////////////////
        
        foreach ( Transform trans in GameObject.Find("PlaceholderBullets").transform)
        {
            prefabBullets.Add(trans.name, trans.gameObject);
        }
        
        ////////////////////////////////////////////////////////////////

        int[] mostUsed = new int[toMake.Keys.Count];
        List<string> bulletTypes = new List<string>(prefabBullets.Keys);
        foreach(string room in rooms)
        {
            for(int i = 0; i < prefabBullets.Keys.Count; i++)
            {
                if (users[room].ContainsKey(bulletTypes[i]))
                {
                    if (users[room][bulletTypes[i]] > mostUsed[i])
                    {
                        mostUsed[i] = users[room][bulletTypes[i]];
                    }
                }
            }
        }
        
        ////////////////////////////////////////////////////////////////

        List<string> keys = new List<string>(toMake.Keys);
        for(int i = 0; i < keys.Count; i++)
        {
            toMake[keys[i]] = Mathf.RoundToInt(toMake[keys[i]] * mostUsed[i]);
            toMake[keys[i]] += 200;
            if ( toMake[ keys[ i ] ] > 2000 ) // hard limit of 2000 bullets of this type can exist
                toMake[ keys[ i ] ] = 2000;

            DebugManager.GetInstance().Print( this.ToString(), "To make: " + keys[i] + " " + toMake[ keys[ i ] ] );
            inventory.Add(keys[i], 0);
            created.Add(keys[i], 0);

            ////////////////////////////////////////////////////////////////

            //get correct bullet in scene
            GameObject bullet = new GameObject();
            foreach(GameObject b in prefabBullets.Values)
            {
                if(b.name == keys[i])
                {
                    bullet = b;
                    break;
                }
            }
            
            //make bullets
            if (transform.Find(keys[i] + "list") == false)
            {
                GameObject list = new GameObject();
                list.name = keys[i] + "list";
                list.transform.parent = transform;
                Instantiate(bullet, list.transform);
            }
        }

        ////////////////////////////////////////////////////////////////
        
        DebugManager.GetInstance().Print( this.ToString(), "Info: Set bullet limits" );
        
        ////////////////////////////////////////////////////////////////
    }

    ////////////////////////////////////////////////////////////////
    // GET BULLET
    ////////////////////////////////////////////////////////////////

    // Find a bullet and give it to enemy or obstacle
    public GameObject GetBullet(string name)
    {
        ////////////////////////////////////////////////////////////////
        
        GameObject bullet;
        inventory[name] += 1;
        
        // Inventory loop
        if (inventory[name] >= toMake[name] - 1)
        {
            inventory[name] = 0;
        }
        
        ////////////////////////////////////////////////////////////////
        // Check if bullets are created to the limit
        // If yes get one, else create one

        if ( created[name] >= toMake[name])
        {
            bullet = transform.Find(name + "list").transform.GetChild(inventory[name]).gameObject;
        }
        else
        {
            bullet = Instantiate(prefabBullets[name]);
            bullet.transform.parent = transform.Find(name + "list").transform;
            inventory[name]++;
            created[name]++;
        }

        return bullet;
        
        ////////////////////////////////////////////////////////////////
    }

    ////////////////////////////////////////////////////////////////
    // ADD BULLETS TO MAKE
    ////////////////////////////////////////////////////////////////

    public void AddBullet(string name, int amount, float firerate, float bulletSpeed, string room)
    {
        ////////////////////////////////////////////////////////////////

        if ( toMake.ContainsKey( name ) )
        {
            if( toMake[ name ] < (500 / bulletSpeed) * firerate)
            {
                toMake[ name ] = Mathf.RoundToInt( 500.0f / bulletSpeed / firerate );
            }
        }
        else
        {
            toMake.Add( name, Mathf.RoundToInt( 500.0f / bulletSpeed / firerate ) );
        }
        
        ////////////////////////////////////////////////////////////////

        if ( users.ContainsKey(room))
        {
            if (users[room].ContainsKey(name))
            {
                users[room][name]++;
            }
            else
            {
                Dictionary<string, int> usersPerBullet = new Dictionary<string, int>();
                usersPerBullet.Add(name, 1);
                users[room] = usersPerBullet;
            }
        }
        else
        {
            Dictionary<string, int> usersPerBullet = new Dictionary<string, int>();
            usersPerBullet.Add(name, 1);
            users.Add(room, usersPerBullet);
        }
        
        ////////////////////////////////////////////////////////////////

        if ( rooms.Contains(room) == false)
        {
            rooms.Add(room);
        }
        
        ////////////////////////////////////////////////////////////////
    }

    ////////////////////////////////////////////////////////////////
}