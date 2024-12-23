using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageSelector : MonoBehaviour
{
    private Color color;
    [SerializeField]
    private TextMeshProUGUI label;
    [SerializeField]
    private float moveDuration_sec = 0.05f;
    private bool isMoving = false;

    public StageGridEntry selectedStage;
    private Transform target;

    public bool Locked { get; private set; }

    public void Initialize()
    {
        
    }

    public void MoveToTarget(Transform newTarget)
    {
        if (!isMoving)
        {
            isMoving = true;
            target = newTarget;
            StartCoroutine(MoveToTargetCoroutine());
        }
    }

    public void Lock()
    {
        Locked = true;
    }

    public void Unlock()
    {
        Locked = false;
    }

    private IEnumerator MoveToTargetCoroutine()
    {
        yield return null;
        float elapsedTime = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = target.position;
        while (elapsedTime < moveDuration_sec)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration_sec);
            //Debug.Log(string.Format("moved to {0},{1},{2}", transform.position.x, transform.position.y, transform.position.z));
            yield return null;
        }
        isMoving = false;
    }
}