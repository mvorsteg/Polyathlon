using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CourseIcon : MonoBehaviour
{
    public Sprite[] sprites;
    public Image topImage;
    public Image bottomImage;

    private int idx = 0;

    private void Start()
    {
        StartCoroutine(ShuffleSprites());    
    }

    private IEnumerator ShuffleSprites()
    {
        float elapsedTime;
        while(true) // we want this to never stop
        {
            topImage.sprite = sprites[idx];
            idx = (idx + 1) % sprites.Length;
            yield return new WaitForSeconds(5f);
            bottomImage.sprite = sprites[idx];
            elapsedTime = 0;
            Color c = topImage.material.color;
            while (elapsedTime < 1f)
            {
                topImage.material.color = new Color(c.r, c.g, c.b, Mathf.Lerp(1, 0, elapsedTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            topImage.material.color = c;
            topImage.sprite = sprites[idx];
        }
        
    }    
}