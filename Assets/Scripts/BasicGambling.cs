using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;   

public class BasicGambling : MonoBehaviour
{   
    public GameObject MenuCanvas;
    public GameObject BJCanvas;
    public TMP_Text moneyText;
    public TMP_InputField betField;
    public GameObject bettingField;
    public TMP_Text betText;
    public TMP_Text confirmText;
    public GameObject card1;
    public TMP_Text card1Text;
    public GameObject card2;
    public TMP_Text card2Text;
    public GameObject hand;
    public TMP_Text handText;
    public GameObject House;
    public TMP_Text houseText;
    public GameObject housecard1;
    public TMP_Text housecard1Text;
    public GameObject housecard2;
    public TMP_Text housecard2Text;
    public GameObject choice;
    public GameObject winText;
    public GameObject loseText;
    public GameObject tieText;
    public GameObject Restart;
    private int money;
    private int betMoney = 0;
    private int maxMoney;
    private int handValue = 0;
    private int houseValue = 0;
    private int extraHand = 0;
    private int extraY = -350;
    private int extraX = 72; 
    private int extraHx = 80;
    private int extraHy = 260;
    private int extraHouse = 0;
    private int card1value = 0;
    private int card2value = 0;
    private int housecard1value = 0;
    private int housecard2value = 0;
    private int housecardmultiply = 0;
    private int handcardmultiply = 0;
    private int handacecount = 0;
    private int houseacecount = 0;

    private void Start()
    {
        MenuCanvas.SetActive(true);
        BJCanvas.SetActive(false);

        if (!PlayerPrefs.HasKey("Money"))
        {
            money = 1000;
            PlayerPrefs.SetInt("Money", money);
            PlayerPrefs.Save();
        }
        else
        {
            money = PlayerPrefs.GetInt("Money");
        }

        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void handOver()
    {
        StartCoroutine(StayHand());
    }
    IEnumerator StayHand()
    {
        choice.SetActive(false);
        yield return new WaitForSeconds(1);
        housecard2value = Random.Range(1,11);
        housecard2Text.text = housecard2value.ToString();
        houseValue += housecard2value;
        if (housecard2value == 1 && houseValue+10 <= 21)
        {
            houseValue += 10;
            houseacecount = 1;
        }
        houseText.text = "Kasa Eli Toplam: " + houseValue.ToString();

        while (houseValue < 17)
        {
            yield return new WaitForSeconds(1);
            GameObject extracard1 = new GameObject("ExtraHouseCard" + extraHouse);
            Image image = extracard1.AddComponent<Image>();
            Shadow shadow = extracard1.AddComponent<Shadow>();
            shadow.effectDistance = new Vector2(10, 10);
            GameObject extraValue = new GameObject("ExtraHouseValue" + extraHouse);
            TextMeshProUGUI text = extraValue.AddComponent<TextMeshProUGUI>();
            int value = Random.Range(1, 11);
            text.text = value.ToString();
            text.color = Color.black;
            text.fontStyle = FontStyles.Bold;
            text.fontSize = 72;
            text.enableWordWrapping = false;
            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(50, 100);
            extraValue.transform.SetParent(GameObject.Find("ExtraHouseCard" + extraHouse).transform);
            extracard1.transform.SetParent(GameObject.Find("House").transform);
            extracard1.transform.localPosition = new Vector2(extraHx + (housecardmultiply * 236), extraHy);
            RectTransform extraRect = extracard1.GetComponent<RectTransform>();
            extraRect.sizeDelta = new Vector2(200, 300);
            extraRect.localScale = new Vector3(1, 1, 1);
            extraHouse += 1;
            housecardmultiply += 1;
            houseValue += value;

            if(value == 1 && houseValue+10 <= 21)
            {
                houseValue += 10;
                houseacecount = 1;
            }
            else if(value != 1 && (housecard1value == 1 || housecard2value == 1) && houseValue > 21 && houseacecount == 1)
            {
                houseValue -= 10;
                houseacecount = 0;
            }
            else if(value != 1 && housecard1value != 1 && housecard2value != 1 && houseValue > 21 && houseacecount == 1)
            {
                for (int i = 0; i < housecardmultiply; i++)
                {
                    GameObject card = GameObject.Find("ExtraHouseValue" + i);
                    if (card != null)
                    {
                        if (card.GetComponent<TextMeshProUGUI>().text == "1")
                        {
                            houseValue -= 10;
                            houseacecount = 0;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            houseText.text = "Kasa Eli Toplam: " + houseValue.ToString();
            if(housecardmultiply == 4)
            {
                extraHx -= 20;
                extraHy -= 10;
                housecardmultiply = 0;
            }
        }
        if(houseValue >= 17 && houseValue <= 21)
        {
            if(houseValue > handValue)
            {
                betMoney = 0;
                handValue = 0;
                houseValue = 0;
                extraHand = 0;
                extraHouse = 0;
                extraHx = 80;
                extraHy = 260;
                extraX = 72;
                extraY = -350;
                handcardmultiply = 0;
                housecardmultiply = 0;
                loseText.SetActive(true);
                choice.SetActive(false);
                Restart.SetActive(true);
            }
            else if(houseValue == handValue)
            {
                money += betMoney;
                PlayerPrefs.SetInt("Money", money);
                PlayerPrefs.Save();
                betMoney = 0;
                handValue = 0;
                houseValue = 0;
                extraHand = 0;
                extraHouse = 0;
                extraHx = 80;
                extraHy = 260;
                extraX = 72;
                extraY = -350;
                handcardmultiply = 0;
                housecardmultiply = 0;
                tieText.SetActive(true);
                choice.SetActive(false);
                Restart.SetActive(true);

            }
            else
            {
                if (houseValue == 21) 
                { 
                    money += (3 * betMoney) / 2 + betMoney;
                    PlayerPrefs.SetInt("Money", money);
                    PlayerPrefs.Save();
                }
                else
                {
                    money += betMoney * 2;
                    PlayerPrefs.SetInt("Money", money);
                    PlayerPrefs.Save();
                }
                betMoney = 0;
                handValue = 0;
                houseValue = 0;
                extraHand = 0;
                extraHouse = 0;
                extraHx = 80;
                extraHy = 260;
                extraX = 72;
                extraY = -350;
                handcardmultiply = 0;
                housecardmultiply = 0;
                winText.SetActive(true);
                choice.SetActive(false);
                Restart.SetActive(true);
            }
            moneyText.text = "Mevcut Para: " + money.ToString();

        }
        else
        {
            money += betMoney * 2;
            PlayerPrefs.SetInt("Money", money);
            PlayerPrefs.Save();
            betMoney = 0;
            handValue = 0;
            houseValue = 0;
            extraHand = 0;
            extraHouse = 0;
            extraHx = 80;
            extraHy = 260;
            extraX = 72;
            extraY = -350;
            handcardmultiply = 0;
            housecardmultiply = 0;
            winText.SetActive(true);
            choice.SetActive(false);
            Restart.SetActive(true);
            moneyText.text = "Mevcut Para: " + money.ToString();
        }

    }
    public void HitCard()
    {
        
        GameObject extracard1 = new GameObject("ExtraCard" + extraHand);
        Image image = extracard1.AddComponent<Image>();
        Shadow shadow = extracard1.AddComponent<Shadow>();
        shadow.effectDistance = new Vector2(10, 10);
        GameObject extraValue = new GameObject("ExtraValue" + extraHand);
        TextMeshProUGUI text = extraValue.AddComponent<TextMeshProUGUI>();
        int value = Random.Range(1, 11);
        text.text = value.ToString();
        text.color = Color.black;
        text.fontStyle = FontStyles.Bold;
        text.fontSize = 72;
        text.enableWordWrapping = false;
        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(50, 100);
        extraValue.transform.SetParent(GameObject.Find("ExtraCard" + extraHand).transform);
        extracard1.transform.SetParent(GameObject.Find("Hand").transform);
        RectTransform extraRect = extracard1.GetComponent<RectTransform>();
        extraRect.sizeDelta = new Vector2(200, 300);
        extraRect.localScale = new Vector3(1, 1, 1);
        extracard1.transform.localPosition = new Vector2(extraX + (handcardmultiply * 236), extraY);
        extraHand += 1;
        handcardmultiply += 1;
        if (handcardmultiply == 4)
        {
            extraY -= 20;
            extraX -= 10;
            handcardmultiply = 0;
        }
        handValue += value;

        if (value == 1 && handValue+10 <= 21)
        {
            handValue += 10;
            handacecount = 1;
        }
        else if(value == 1 && handValue+10 > 21 && handacecount == 1)   
        {
            handacecount = 0;
        }
        else if(value != 1 && (card1value == 1 || card2value == 1) && handValue > 21 && handacecount == 1)
        {
            handValue -= 10;
            handacecount = 0;
        }
        else if(value != 1 && card1value != 1 && card2value != 1 && handValue > 21 && handacecount == 1)
        {
            for (int i = 0; i < handcardmultiply; i++)
            {
                GameObject card = GameObject.Find("ExtraValue" + i);
                if (card != null)
                {
                    TextMeshProUGUI tmpText = card.GetComponent<TextMeshProUGUI>();
                    if (tmpText.text == "1")
                    {
                        handValue -= 10;
                        handacecount = 0;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        

  
        handText.text = "Elin Toplami: " + handValue.ToString();


        if (handValue > 21)
        {
            betMoney = 0;
            handValue = 0;
            houseValue = 0;
            extraHand = 0;
            extraHouse = 0;
            extraHx = 80;
            extraHy = 260;
            extraX = 72;
            extraY = -350;
            handcardmultiply = 0;
            housecardmultiply = 0;
            loseText.SetActive(true);
            choice.SetActive(false);
            Restart.SetActive(true);
        }
        
    }
    public void ResetCards()
    {
       
        for (int i = 0; i<10;  i++)
        {
            GameObject card = GameObject.Find("ExtraCard" + i);
            Destroy(card);
            GameObject housecard = GameObject.Find("ExtraHouseCard" + i);
            Destroy(housecard);
        }
        card1.SetActive(false);
        card2.SetActive(false);
        housecard1.SetActive(false);
        housecard2.SetActive(false);
        choice.SetActive(false);
        House.SetActive(false);
        hand.SetActive(false);
        bettingField.SetActive(true);
        betText.text = "Mevcut Bahis: 0";
        Restart.SetActive(false);
        loseText.SetActive(false);
        winText.SetActive(false);
        tieText.SetActive(false);
        handText.text = "Elin Toplami: ";
        houseText.text = "Kasa Eli Toplam: ";
        housecard2Text.text = " ";
        maxMoney = money;
    }
    IEnumerator StartBJ()
    {
        hand.SetActive(true);
        yield return new WaitForSeconds(1);

        card1.SetActive(true);
        card1value = Random.Range(1, 11);
        card1Text.text = card1value.ToString();
        handValue = card1value;
        if(card1value == 1)
        {
            handValue += 10;
            handacecount = 1;
        }
        handText.text = "Elin Toplami: " + handValue.ToString();
        yield return new WaitForSeconds(1);

        House.SetActive(true);
        housecard1.SetActive(true);
        housecard1value = Random.Range(1, 11);
        housecard1Text.text = housecard1value.ToString();
        houseValue = housecard1value;
        if(housecard1value == 1)
        {
            houseValue += 10;
            houseacecount = 1;
        }
        houseText.text = "Kasa Eli Toplam: " + houseValue.ToString();
        yield return new WaitForSeconds(1);

        card2.SetActive(true);
        card2value = Random.Range(1, 11);
        card2Text.text = card2value.ToString();
        handValue += card2value;
        if(card2value == 1 && handValue+10 <= 21)
        {
            handValue += 10;
            handacecount = 1;
        }
        handText.text = "Elin Toplami: " + handValue.ToString();
        yield return new WaitForSeconds(1);

        choice.SetActive(true);
        housecard2.SetActive(true);
    }
    public void ConfirmBet()
    {
        if (betMoney > 0)
        {
            bettingField.SetActive(false);
            StartCoroutine(StartBJ());
        }
        else { confirmText.text = "Bahis Koyun"; }
    }
    public void DecreaseBet()
    {
        if (betMoney + int.Parse(betField.text) <= maxMoney && betMoney >= int.Parse(betField.text))
        {
            money += int.Parse(betField.text);
            PlayerPrefs.SetInt("Money", money);
            PlayerPrefs.Save();
            betMoney -= int.Parse(betField.text);
            betText.text = "Mevcut Bahis: " + betMoney.ToString();
            moneyText.text = "Mevcut Para: " + money.ToString();
        }
        else if(betMoney + int.Parse(betField.text) > maxMoney && betMoney >= int.Parse(betField.text))
        {
            money += int.Parse(betField.text);
            PlayerPrefs.SetInt("Money", money);
            PlayerPrefs.Save();
            betMoney -= int.Parse(betField.text);
            betText.text = "Mevcut Bahis: " + betMoney.ToString();
            moneyText.text = "Mevcut Para: " + money.ToString();
        }
        else
        {
            money += betMoney;
            PlayerPrefs.SetInt("Money", money);
            PlayerPrefs.Save();
            betMoney = 0;
            betText.text = "Mevcut Bahis: " + betMoney.ToString();
            moneyText.text = "Mevcut Para: " + money.ToString();
        }
    }
    public void IncreaseBet()
    {
        if(int.Parse(betField.text) == 6464)
        {
            money += 10000;
            PlayerPrefs.SetInt("Money", money);
            PlayerPrefs.Save();
            maxMoney += 10000;
        }
        if (money - int.Parse(betField.text) > 0)
        {
            money -= int.Parse(betField.text);
            PlayerPrefs.SetInt("Money", money);
            PlayerPrefs.Save();
            betMoney += int.Parse(betField.text);
            betText.text = "Mevcut Bahis: " + betMoney.ToString();
            moneyText.text = "Mevcut Para: " + money.ToString();
        }
        else
        {
            betMoney += money;
            money = 0;
            PlayerPrefs.SetInt("Money", money);
            PlayerPrefs.Save();
            betText.text = "Mevcut Bahis: " + betMoney.ToString();
            moneyText.text = "Mevcut Para: " + money.ToString();
        }
        confirmText.text = "Bahsi Onayla";
    }
    public void negativeFail()
    {
        if(int.Parse(betField.text) < 0)
        {
            betField.text = "0";
        }
    }

    public void StartGame()
    {
        MenuCanvas.SetActive(false);
        BJCanvas.SetActive(true);
        moneyText.text = "Mevcut Para: " + money.ToString();
        maxMoney = money;
    }

    public void ExitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
