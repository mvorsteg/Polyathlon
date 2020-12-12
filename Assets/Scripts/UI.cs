using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Racer))]
public class UI : MonoBehaviour
{
    public Text positionText;
    public Text timeText;
    public Text centerText;
    private Racer racer;

    private bool isPrimaryUI;
    private static bool isPrimaryTaken = false;

    private void Awake()
    {
        isPrimaryUI = !isPrimaryTaken;
        isPrimaryTaken = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        racer = GetComponentInParent<Racer>();

        StartCoroutine(StartRace());
    }

    // Update is called once per frame
    void Update()
    {
        positionText.text = RaceManager.GetPosition();
        timeText.text = FormatTime(RaceManager.Time);

    }

    /*  returns time in the form "minutes:seconds.milliseconds" */
    private static string FormatTime(float seconds)
    {
        int mm = (int)(seconds / 60);
        int ss = (int)(seconds % 60);
        int ms = (int)((seconds - ss) * 1000);

        return mm.ToString("D2")  + ":" + ss.ToString("D2") + "." + ms.ToString("D3");
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
