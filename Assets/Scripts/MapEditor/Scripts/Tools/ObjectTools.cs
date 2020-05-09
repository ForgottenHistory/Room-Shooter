using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ObjectTools : MonoBehaviour {

    bool isSomethingSelected = false;
    //currently selected object, from
    [HideInInspector]
    public GameObject selectedObject;

    //Input from the inputfields on transform tools
    public InputField xPos;
    public InputField yPos;
    public InputField zPos;

    public InputField xRot;
    public InputField yRot;
    public InputField zRot;

    public InputField xScale;
    public InputField yScale;
    public InputField zScale;

    public InputField red;
    public InputField green;
    public InputField blue;
    public InputField alpha;

    bool isInputFieldSelected = false;

    public bool IsSomethingSelected
    {
        get
        {
            return isSomethingSelected;
        }

        set
        {
            isSomethingSelected = value;
        }
    }

    private void Update()
    {
        CheckIfFocused();
        //if no inputfield is selected, will update transform on selected object
        if (IsSomethingSelected && isInputFieldSelected == false)
        {
            try
            {
                xPos.text = selectedObject.transform.position.x.ToString();
                yPos.text = selectedObject.transform.position.y.ToString();
                zPos.text = selectedObject.transform.position.z.ToString();

                xRot.text = selectedObject.transform.eulerAngles.x.ToString();
                yRot.text = selectedObject.transform.eulerAngles.y.ToString();
                zRot.text = selectedObject.transform.eulerAngles.z.ToString();

                xScale.text = selectedObject.transform.lossyScale.x.ToString();
                yScale.text = selectedObject.transform.lossyScale.y.ToString();
                zScale.text = selectedObject.transform.lossyScale.z.ToString();
                if (selectedObject.GetComponent<Renderer>())
                {
                    red.text = Mathf.Round(selectedObject.GetComponent<Renderer>().material.color.r * 255).ToString();
                    green.text = Mathf.Round(selectedObject.GetComponent<Renderer>().material.color.g * 255).ToString();
                    blue.text = Mathf.Round(selectedObject.GetComponent<Renderer>().material.color.b * 255).ToString();
                    alpha.text = Mathf.Round(selectedObject.GetComponent<Renderer>().material.color.a * 255).ToString();
                }
            }
            catch
            {
                Debug.Log("Error: Can not update inputfields");
            }
        }
        //if inputfield is selected, able to change transform
        else if(IsSomethingSelected && isInputFieldSelected && EventSystem.current.currentSelectedGameObject.GetComponent<InputField>().text != "")
        {
            try
            {
                //limit for inputs, dont want game to crash
                if (float.Parse(xPos.text) <= 5000 
                    && float.Parse(xPos.text) >= -5000 && float.Parse(yPos.text) <= 5000 
                    && float.Parse(yPos.text) >= -5000 && float.Parse(zPos.text) <= 5000 
                    && float.Parse(zPos.text) >= -5000)
                {
                    selectedObject.transform.position = new Vector3(float.Parse(xPos.text), float.Parse(yPos.text), float.Parse(zPos.text));
                }

                if (float.Parse(xRot.text) <= 360 && float.Parse(yRot.text) <= 360 && float.Parse(zRot.text) <= 360)
                {
                    selectedObject.transform.eulerAngles = new Vector3(float.Parse(xRot.text), float.Parse(yRot.text), float.Parse(zRot.text));
                }

                if (float.Parse(xScale.text) <= 5000 
                    && float.Parse(xScale.text) >= 1 
                    && float.Parse(yScale.text) <= 5000 
                    && float.Parse(yScale.text) >= 1 
                    && float.Parse(zScale.text) <= 5000 
                    && float.Parse(zScale.text) >= 1
                    && selectedObject.transform.childCount == 0)
                {
                    selectedObject.transform.localScale = new Vector3(float.Parse(xScale.text), float.Parse(yScale.text), float.Parse(zScale.text));
                }
                if (selectedObject.GetComponent<Renderer>())
                {
                    if (float.Parse(red.text) <= 255 && float.Parse(red.text) >= 0
                        && float.Parse(green.text) <= 255 && float.Parse(green.text) >= 0
                        && float.Parse(blue.text) <= 255 && float.Parse(blue.text) >= 0
                        && float.Parse(alpha.text) <= 255 && float.Parse(alpha.text) >= 0)
                    {
                        selectedObject.GetComponent<Renderer>().material.color = new Color(float.Parse(red.text) / 255, float.Parse(green.text) / 255, float.Parse(blue.text) / 255, float.Parse(alpha.text) / 255);
                    }
                }
            }
            //if user inputs invalid number, example: 0, it cant be converted!
            catch
            {
                Debug.Log("Error: Can not convert");
            }
        }
    }

    public void CheckIfFocused()
    {
        //check every inputfield if they are in focus
        if (xPos.isFocused || yPos.isFocused || zPos.isFocused 
            || xRot.isFocused || yRot.isFocused || zRot.isFocused 
            || xScale.isFocused || yScale.isFocused || zScale.isFocused 
            || red.isFocused || green.isFocused || blue.isFocused || alpha.isFocused)
        {
            isInputFieldSelected = true;
        }
        else
        {
            isInputFieldSelected = false;
        }
    }
    public void MakeFalse()
    {
        IsSomethingSelected = false;
    }
    //used in MoveObject script
    public void ActiveObject(GameObject selectedObject)
    {
        this.selectedObject = selectedObject;
    }
}
