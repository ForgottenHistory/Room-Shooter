using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToolsManager : MonoBehaviour {

    //tools for mapeditor

    //variables, gameobjects etc
    public GameObject gridLines;
    public Dropdown creatableObjectsDropDown;
    bool gridlinesEnabled = true;
    ObjectTools transformTools;

    private void Start()
    {
        transformTools = GameObject.Find("UIScripts").GetComponent<ObjectTools>();
        List<string> creatableobjects = new List<string>();
        foreach(Transform trans in GameObject.Find("CreatableObjects").transform)
        {
            creatableobjects.Add(trans.gameObject.name);
        }
        creatableObjectsDropDown.ClearOptions();
        creatableObjectsDropDown.AddOptions(creatableobjects);
        creatableObjectsDropDown.value = 0;
    }

    public void CreateNewObject()
    {
        //if currently editchunk exists
        //if (GameObject.Find("SetCompiler").GetComponent<SetCompiler>().ReturnActiveChunk())
        //{
            //instantiate from what is selected in dropdown
            GameObject newObject = Instantiate(GameObject.Find("CreatableObjects").transform.GetChild(creatableObjectsDropDown.value).gameObject);
        if (transformTools.xPos.text != "" && transformTools.yPos.text != "" && transformTools.zPos.text != "")
            newObject.transform.position = new Vector3(float.Parse(transformTools.xPos.text), float.Parse(transformTools.yPos.text) + transformTools.selectedObject.transform.lossyScale.y, float.Parse(transformTools.zPos.text));
        else 
            newObject.transform.position = Vector3.zero;
        //newObject.transform.parent = GameObject.Find("SetCompiler").GetComponent<SetCompiler>().ReturnActiveChunk();
        if (newObject.GetComponent<Rigidbody>())
            {
                newObject.GetComponent<Rigidbody>().isKinematic = true;
            }

        //if (newObject.GetComponent<Collider>() == false)
        //{
        //    newObject.AddComponent<BoxCollider>();
        //    newObject.GetComponent<BoxCollider>().isTrigger = true;
        //}
        newObject.AddComponent<MoveObject>();
            newObject.GetComponent<MoveObject>().InitializeScript();
        transformTools.ActiveObject(newObject);
        GameObject.Find("PositionTool").GetComponent<PositionTool>().ChangeSelectedGameObject(newObject);
        //}
    }
    //change gridlines
    public void ChangeGridlineState()
    {
        gridlinesEnabled = !gridlinesEnabled;
        gridLines.SetActive(gridlinesEnabled);
    }
}
