using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class Shop : MonoBehaviour
{
    public static Shop shop;

    private void Awake()
    {
        shop = this;
    }

    public GameData gData;

    [Space]
    public SpriteRenderer table_sprite_sp;
    public GameObject[] tables;    
    public Sprite[] sp_table;
    public Sprite[] sprite_buttons_table;
    public int[] table_prices;
    public int[] bonus_prices;

    [Space]
    public Text[] bonuses_text;

    [Space]    
    public Text coin_text;

    void Start()
    {
        table_sprite_sp.sprite = sp_table[gData.table_id];
        Checktables();        
    }
    
    void Update()
    {        
        coin_text.text = gData.coins.ToString();
    }

    void Checktables()
    {
        for (int i = 0; i < gData.table_count.Length; i++)
        {
            if(gData.table_count[i] > 0)
            {
                tables[i].transform.Find("PickButtons").Find("PickTable").GetComponent<Image>().sprite = sprite_buttons_table[0];
                tables[i].transform.Find("PickButtons").gameObject.SetActive(true);
                tables[i].transform.Find("BuyButtons").gameObject.SetActive(false);

                if(i == gData.table_id)
                    tables[i].transform.Find("PickButtons").Find("PickTable").GetComponent<Image>().sprite = sprite_buttons_table[1];
            }
            else
            {                
                tables[i].transform.Find("PickButtons").gameObject.SetActive(false);
                tables[i].transform.Find("BuyButtons").gameObject.SetActive(true);

                tables[i].transform.Find("BuyButtons").Find("BuyItemCoin").Find("Price").GetComponent<Text>().text = table_prices[i].ToString();
            }
        }

        bonuses_text[0].text = gData.coffee.ToString();
        bonuses_text[1].text = gData.healthpak.ToString();
        bonuses_text[2].text = gData.finger.ToString();
    }

    public void BuyTable(int i)
    {
        if(gData.coins >= table_prices[i])
        {
            gData.coins -= table_prices[i];
            gData.table_count[i]++;
            Checktables();
            AudioSFX.instance.PlaySingle(AudioSFX.instance.buy_sound, 0.5f);
            StartCoroutine(MoneyAnims());
            SaveLoad.control.SaveGame();
            AnalyticsEvent.Custom("Table" + i.ToString() + "Bought");
        }
        else
        {
            AudioSFX.instance.PlaySingle(AudioSFX.instance.error_sound, 1f);
            StartCoroutine(MoneyAnims());
        }
    }

    public void Picktable(int i)
    {
        table_sprite_sp.sprite = sp_table[i];
        gData.table_id = i;
        Checktables();

        AnalyticsEvent.Custom("Table" + i.ToString() + "Picked");
    }

    public IEnumerator MoneyAnims()
    {
        coin_text.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        coin_text.color = Color.white;
    }

    public void BuyCoffee()
    {
        if (gData.coins >= bonus_prices[0])
        {
            gData.coins -= bonus_prices[0];
            gData.coffee++;
            Checktables();
            AudioSFX.instance.PlaySingle(AudioSFX.instance.buy_sound, 0.5f);
            StartCoroutine(MoneyAnims());
            SaveLoad.control.SaveGame();
            AnalyticsEvent.Custom("Coffee Bought");
        } 
        else
        {
            AudioSFX.instance.PlaySingle(AudioSFX.instance.error_sound, 1f);
            StartCoroutine(MoneyAnims());
        }
    }

    public void BuyHealth()
    {
        if (gData.coins >= bonus_prices[1])
        {
            gData.coins -= bonus_prices[1];
            gData.healthpak++;
            Checktables();
            AudioSFX.instance.PlaySingle(AudioSFX.instance.buy_sound, 0.5f);
            StartCoroutine(MoneyAnims());
            SaveLoad.control.SaveGame();
            AnalyticsEvent.Custom("Health Bought");
        }
        else
        {
            AudioSFX.instance.PlaySingle(AudioSFX.instance.error_sound, 1f);
            StartCoroutine(MoneyAnims());
        }
    }

    public void BuyFinger()
    {
        if (gData.coins >= bonus_prices[2])
        {
            gData.coins -= bonus_prices[2];
            gData.finger++;
            Checktables();
            AudioSFX.instance.PlaySingle(AudioSFX.instance.buy_sound, 0.5f);
            StartCoroutine(MoneyAnims());
            SaveLoad.control.SaveGame();
            AnalyticsEvent.Custom("Finger Bought");
        }
        else
        {
            AudioSFX.instance.PlaySingle(AudioSFX.instance.error_sound, 1f);
            StartCoroutine(MoneyAnims());
        }
    }

    public void OpenShop()
    {
        AnalyticsEvent.Custom("StoreOpened");
    }
}
