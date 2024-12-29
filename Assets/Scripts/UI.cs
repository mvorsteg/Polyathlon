﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//[RequireComponent(typeof(Racer))]
public class UI : MonoBehaviour
{

    public TextMeshProUGUI positionText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI centerText;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI speedValueText;
    public TextMeshProUGUI speedUnitText;
    [SerializeField]
    private SpeedUnits speedUnit;
    public Image itemImage;

    public RectTransform scaleTransform;
    
    public Sprite defaultItemSprite;
    private Racer racer;

    private bool isPrimaryUI;
    private static UI primaryUI;
    private static bool isPrimaryTaken = false;

    private void Awake()
    {
        // if (!isPrimaryTaken)
        // {
        //     primaryUI = this;
        //     isPrimaryUI = true;
        //     isPrimaryTaken = true;

        // }
        isPrimaryUI = !isPrimaryTaken;
        isPrimaryUI = true; // solve this bug later its 4:04 AM
        isPrimaryTaken = true;

        SetSpeedUnit((SpeedUnits)PlayerPrefs.GetInt(PlayerPrefsKeys.SPEED_UNITS, 0));
    }

    // Start is called before the first frame update
    void Start()
    {
        racer = GetComponentInParent<Racer>();
        SetItemImage(null);
        StartCoroutine(StartRace());
        
        if (RaceManager.IsTrainingCourse)
        {
            timeText.enabled = false;
            positionText.enabled = false;
            itemText.text = "Respawn: Left Trigger (Gamepad)\nRight/Left Click (Mouse)";
            itemText.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!racer.isFinished && !RaceManager.IsTrainingCourse)
        {
            int pos = RaceManager.GetPosition(this.racer);
            switch (pos)
            {
                case 1:
                    positionText.text = pos + "st";
                    break;
                case 2:
                    positionText.text = pos + "nd";
                    break;
                case 3:
                    positionText.text = pos + "rd";
                    break;
                default:
                    positionText.text = pos + "th";
                    break;
            }
            timeText.text = FormatTime(RaceManager.Time);
        }

        SetSpeed(racer.Speed);
    }

    public void SetScale(int player, int maxPlayers)
    {
        if (maxPlayers == 1)
        {
            // do nothing
        }
        else if (maxPlayers < 3)
        {
            switch (player)
            {
                case 0:
                    scaleTransform.anchorMax = new Vector2(0, 0.5f);
                    scaleTransform.anchorMin = new Vector2(0, 0.5f);
                    scaleTransform.anchoredPosition = new Vector3(0, scaleTransform.sizeDelta.y / 2, 0);
                    break;
                case 1:
                    scaleTransform.anchorMax = new Vector2(1, 0.5f);
                    scaleTransform.anchorMin = new Vector2(1, 0.5f);
                    scaleTransform.anchoredPosition = new Vector3(-scaleTransform.sizeDelta.x / 2, scaleTransform.sizeDelta.y / 2, 0);
                    break;
            }
            scaleTransform.sizeDelta = new Vector2(scaleTransform.sizeDelta.x / 2 , scaleTransform.sizeDelta.y);

        }
        else
        {
            switch (player)
            {
                case 0:
                    scaleTransform.pivot = new Vector2(0, 1);
                    scaleTransform.anchorMax = new Vector2(0, 1);
                    scaleTransform.anchorMin = new Vector2(0, 1);
                    break;
                case 1:
                    scaleTransform.pivot = new Vector2(1, 1);
                    scaleTransform.anchorMax = new Vector2(1, 1);
                    scaleTransform.anchorMin = new Vector2(1, 1);
                    break;
                case 2:
                    scaleTransform.pivot = new Vector2(0, 0);
                    scaleTransform.anchorMax = new Vector2(0, 0);
                    scaleTransform.anchorMin = new Vector2(0, 0);
                    break;
                case 3:
                    scaleTransform.pivot = new Vector2(1, 0);
                    scaleTransform.anchorMax = new Vector2(1, 0);
                    scaleTransform.anchorMin = new Vector2(1, 0);
                    break;

            }
            scaleTransform.localScale = new Vector3(0.5f, 0.5f, 1);
            scaleTransform.anchoredPosition = new Vector3(0, 0, 0);
        }
    }

    /*  returns time in the form "minutes:seconds.milliseconds" */
    public static string FormatTime(float seconds)
    {
        int mm = ((int)(seconds / 60));
        int ss = ((int)(seconds % 60));
        int ms = ((int)((seconds - ss - 60 * mm) * 1000));
    
        return mm.ToString("D2")  + ":" + ss.ToString("D2") + "." + ms.ToString("D3");;
    }

    /*  sets the item image to the specified sprite */
    public void SetItemImage(Sprite icon)
    {
        if (icon == null)
        {
            // itemImage.sprite = defaultItemSprite;
            itemImage.gameObject.SetActive(false);
            itemText.gameObject.SetActive(false);
        }
        else
        {
            itemImage.gameObject.SetActive(true);
            itemText.gameObject.SetActive(true);
            itemImage.sprite = icon;
            //itemText.text = "Use Item: Left Trigger (Gamepad)\nRight/Left Click (Mouse)";
        }
    }

    public void ReviveText(bool val)
    {
        centerText.gameObject.SetActive(val);
        centerText.text = "Jump to Revive!";
    }

    public void SetSpeedUnit(SpeedUnits unit)
    {
        speedUnit = unit;
        switch (unit)
        {
            case SpeedUnits.MilesPerHour:
            {
                speedUnitText.text = "MPH";
            }
            break;
            case SpeedUnits.KilometersPerHour:
            {
                speedUnitText.text = "KPH";
            }
            break;
            case SpeedUnits.MetersPerSecond:
            {
                speedUnitText.text = "M/s";
            }
            break;
            case SpeedUnits.Knots:
            {
                speedUnitText.text = "Knots";
            }
            break;
            case SpeedUnits.FurlongsPerFortnite:
            {
                speedUnitText.text = "Fur/Ftn";
            }
            break;
            default:
            {
                speedUnitText.text = "";
                Debug.Log("Unknown unit " + unit);
            }
            break;
        }
    }

    public void SetSpeed(float velocity_mps)
    {
        float convertedSpeed;
        switch (speedUnit)
        {
            case SpeedUnits.MilesPerHour:
            {
                convertedSpeed = velocity_mps * 2.23694f;
            }
            break;
            case SpeedUnits.KilometersPerHour:
            {
                convertedSpeed = velocity_mps * 3.6f;
            }
            break;
            case SpeedUnits.MetersPerSecond:
            {
                convertedSpeed = velocity_mps;
            }
            break;
            case SpeedUnits.Knots:
            {
                convertedSpeed = velocity_mps * 1.94384f;
            }
            break;
            case SpeedUnits.FurlongsPerFortnite:
            {
                convertedSpeed = velocity_mps * 6012.87f;
            }
            break;
            default:
            {
                convertedSpeed = velocity_mps;
            }
            break;
        }
        speedValueText.text = convertedSpeed.ToString("F0");
    }

    private IEnumerator StartRace()
    {
        centerText.gameObject.SetActive(true);
        float elapsedTime;
        for (int i = 3; i > 0; i--)
        {   
            elapsedTime = 0f;
            centerText.text = i.ToString();
            centerText.rectTransform.localScale = Vector3.one;
            while (elapsedTime < 1f)
            {
                float scale = Mathf.Lerp(1f, 0.5f, elapsedTime);
                centerText.rectTransform.localScale = new Vector3(scale, scale, 1);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        if (isPrimaryUI)
        {
            RaceManager.StartRace();
        }
        
        centerText.text = "Go!";
        centerText.rectTransform.localScale = Vector3.one;
        elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            float scale = Mathf.Lerp(1f, 0.5f, elapsedTime);
            centerText.rectTransform.localScale = new Vector3(scale, scale, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        centerText.gameObject.SetActive(false);
    }

    public IEnumerator FinishRace()
    {
        positionText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        itemImage.gameObject.SetActive(false);
        centerText.gameObject.SetActive(true);
        centerText.text = "Finished!";
        centerText.rectTransform.localScale = Vector3.one;
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            float scale = Mathf.Lerp(1f, 0.5f, elapsedTime);
            centerText.rectTransform.localScale = new Vector3(scale, scale, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        centerText.gameObject.SetActive(false);
    }
}
