using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VFX : MonoBehaviour
{
    public GameObject speedLinesObj;
    public Image damageImage;
    public float damageFadeTime = 1f;

    private void Start()
    {
        SetSpeedLines(false);   
    }

    public void SetSpeedLines(bool enable)
    {
        speedLinesObj.SetActive(enable);
    }

    public void ShowDamage()
    {
        StartCoroutine(ShowDamageCoroutine());
    } 

    private IEnumerator ShowDamageCoroutine()
    {
        Color opaque = new Color(damageImage.color.r, damageImage.color.g, damageImage.color.b, 1f);
        Color transparent = new Color(damageImage.color.r, damageImage.color.g, damageImage.color.b, 0f);

        float elapsedTime = 0f;
        while (elapsedTime < damageFadeTime)
        {
            elapsedTime += Time.deltaTime;
            damageImage.color = Color.Lerp(opaque, transparent, elapsedTime / damageFadeTime);
            yield return null;
        }
    }
}