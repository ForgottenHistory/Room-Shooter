using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIUpgradesManager : MonoBehaviour
{
    [SerializeField]
    Transform content;
    [SerializeField]
    Text coinAmountText;
    public void LoadUpgrades()
    {
        foreach (Transform upgrade in content)
        {
                upgrade.Find("LevelText").GetComponent<Text>().text = "Level: " + DataManager.singleton.GetValue(upgrade.name);
        }
        coinAmountText.text = "Coins: " + DataManager.singleton.misc_Values.coinsAmount.ToString();
    }
    public void BuyUpgrade(string upgrade)
    {
        if(DataManager.singleton.misc_Values.coinsAmount >= 2)
        {
            DataManager.singleton.SetValue(upgrade, 1);

            DataManager.singleton.misc_Values.coinsAmount -= 2;

            coinAmountText.text = "Coins: " + DataManager.singleton.misc_Values.coinsAmount.ToString();
            EventSystem.current.currentSelectedGameObject.transform.parent.Find("LevelText").GetComponent<Text>().text = "Level: " + DataManager.singleton.GetValue(upgrade);
        }
    }
    public void SellUpgrade(string upgrade)
    {
        DataManager.singleton.SetValue(upgrade, -1);

        DataManager.singleton.misc_Values.coinsAmount += 1;
        coinAmountText.text = "Coins: " + DataManager.singleton.misc_Values.coinsAmount;
        EventSystem.current.currentSelectedGameObject.transform.parent.Find("LevelText").GetComponent<Text>().text = "Level: " + DataManager.singleton.GetValue(upgrade);
        PlayerPrefs.Save();
    }

    public void ResetUpgrades()
    {
        DataManager.singleton.SetDefaultValues();
        foreach (Transform upgrade in content)
        {
            upgrade.Find("LevelText").GetComponent<Text>().text = "Level: " + DataManager.singleton.GetValue(upgrade.name);
        }

        coinAmountText.text = "Coins: " + DataManager.singleton.misc_Values.coinsAmount;
    }
}
