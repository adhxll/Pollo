using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// A class to take care all things related to our in-game currency
// this includes storing, changing, showing, animating, etc
// refer to this class whenever you want to do anything with the coins
// this class is initialized in the GameController class
public class CoinMechanism : MonoBehaviour
{
    private int totalCoin;
    private GameObject[] coinAmount;
    private GameObject[] coinChangeIndicator;

    public int GetTotalCoin() { return this.totalCoin; }

    // Start is called before the first frame update
    void Start()
    {
        this.totalCoin = PlayerPrefs.GetInt(PlayerDataKey.CoinAmount.ToString(), 0);
        ShowCoinAmount();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowCoinAmount()
    {
        // setting the value into the game object 
        coinAmount = GameObject.FindGameObjectsWithTag("CoinAmount");

        foreach (GameObject c in coinAmount)
        {
            c.GetComponent<TMP_Text>().text = totalCoin.ToString();
        }
    }

    // animate the decrease or increase in coinAmount
    // parameter:
    // - string sign => a "+" or "-"
    // - int amount => an integer that represents the amount increased or decreased
    private void AnimateCoinChange(string sign, int amount)
    {
        ShowCoinAmount();
        coinChangeIndicator = GameObject.FindGameObjectsWithTag("CoinChangeIndicator");
        foreach (GameObject c in coinChangeIndicator)
        {
            c.GetComponent<TMP_Text>().text = sign + "" + amount.ToString();
            AnimationUtilities.AnimateAddMoney(c); // calls on AnimationUtilities class
            break;
        }
    }

    // public function where you can add any amount of coin
    public void CoinAdd(int coinAmount)
    {
        AnimateCoinChange("+", coinAmount);
        this.totalCoin += coinAmount;
        PlayerPrefs.SetInt(PlayerDataKey.CoinAmount.ToString(), totalCoin); // Automatically saves the new value to PlayerPrefs
        AudioController.Instance.PlaySound(SoundNames.coinadd);
    }

    // public function where you can substract any amount of coin
    public void CoinSubstract(int coinAmount)
    {
        AnimateCoinChange("-", coinAmount);
        this.totalCoin -= coinAmount;
        PlayerPrefs.SetInt(PlayerDataKey.CoinAmount.ToString(), totalCoin); // Automatically saves the new value to PlayerPrefs
    }


}
