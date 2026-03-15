using System.Collections;
using UnityEngine;

public class BoundEffect : MonoBehaviour
{
    [SerializeField]
    private float bounceHeight = 0.3f;

    [SerializeField]
    private float bounceDuration = 0.4f;

    [SerializeField]
    private int bounceCount = 2;

    public void StartBounce()
    {
        StartCoroutine(BounceHandler(transform));
    }

    private IEnumerator BounceHandler(Transform objectTf)
    {
        Vector3 startPos = objectTf.position;

        float localHeight = bounceHeight;
        float localDuration = bounceDuration;

        for (int i = 0; i < bounceCount; i++)
        {
            yield return Bounce(objectTf, startPos, localHeight, localDuration / 2);
            localHeight *= 0.5f;
            localDuration *= 0.8f;
        }

        objectTf.position = startPos;
    }

    private IEnumerator Bounce(
        Transform objectTransform,
        Vector3 start,
        float height,
        float duration
    )
    {
        Vector3 peak = start + Vector3.up * height;

        float elapsed = 0f;

        // Move upwards
        while (elapsed < duration)
        {
            objectTransform.position = Vector3.Lerp(start, peak, elapsed / duration);

            elapsed += Time.deltaTime;

            yield return null;
        }

        elapsed = 0f;

        // Move downwards
        while (elapsed < duration)
        {
            objectTransform.position = Vector3.Lerp(peak, start, elapsed / duration);

            elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
