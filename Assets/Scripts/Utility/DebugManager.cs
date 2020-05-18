using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    //////////////////////////////////////////////////
    //        PUBLIC VARIABLES
    //////////////////////////////////////////////////
    

    public GameObject Log;


    //////////////////////////////////////////////////
    //        PRIVATE VARIABLES
    //////////////////////////////////////////////////

    Transform LogContent;
    int textCounter;
    int textAmount;
    bool consoleOpen = false;

    static DebugManager singleton;

    //////////////////////////////////////////////////
    //          FUNCTIONS
    //////////////////////////////////////////////////

    public void Initialize()
    {
        if ( singleton == null )
        {
            singleton = this;
        }
        else if ( singleton != this )
        {
            Destroy( gameObject );
        }

        //////////////////////////////////////////////////
        //          SET VALUES
        //////////////////////////////////////////////////

        LogContent = Log.transform.GetChild( 0 ).GetChild( 0 ).GetChild( 0 );
        textAmount = LogContent.transform.childCount;
        textCounter = textAmount - 1;

        Log.SetActive( false );
    }

    public static DebugManager GetInstance()
    {
        return singleton;
    }

    public void ChangeConsoleState( )
    {
        consoleOpen = !consoleOpen;
        Log.SetActive( consoleOpen );
    }

    public void Print( string scriptName, string text )
    {

        //////////////////////////////////////////////////
        // PUSH UP PREVIOUS LOGS
        //////////////////////////////////////////////////

        PushUpLogs();

        //////////////////////////////////////////////////
        // CREATE LOG MESSAGE
        //////////////////////////////////////////////////

        int hour = System.DateTime.Now.Hour;
        int minutes = System.DateTime.Now.Minute;
        int seconds = System.DateTime.Now.Second;

        string message = scriptName + ", " + hour.ToString() + ":"+ minutes.ToString() + ":"+ seconds.ToString() + " : " +  text;

        LogContent.transform.GetChild( textAmount - 1 ).GetComponent<Text>().text = message;

        //////////////////////////////////////////////////
        // RESET COUNTER
        //////////////////////////////////////////////////

        if ( textCounter < 0 )
            textCounter = textAmount - 1;
        textCounter--;
    }

    void PushUpLogs()
    {
        //////////////////////////////////////////////////
        // GO THROUGH LOGS AND PUSH THEM UP ONE STEP
        //////////////////////////////////////////////////

        string next = " ";

        for (int i = textAmount - 1; i > 0; i-- )
        {
            if ( i == textAmount - 1 )
            {
                string current = LogContent.transform.GetChild( i ).GetComponent<Text>().text;
                next = LogContent.transform.GetChild( i - 1 ).GetComponent<Text>().text;

                LogContent.transform.GetChild( i - 1 ).GetComponent<Text>().text = current;
            }
            else if ( i != 0 )
            {
                string current = next;
                next = LogContent.transform.GetChild( i - 1 ).GetComponent<Text>().text;

                LogContent.transform.GetChild( i - 1 ).GetComponent<Text>().text = current;
            }

        }
    }
}
