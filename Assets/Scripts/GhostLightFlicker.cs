using System.Collections;
using UnityEngine;

public class GhostLightFlicker : MonoBehaviour
{
    public GameObject flickerPanel;
    public float minDelay = 0.04f;
    public float maxDelay = 0.18f;

    private Coroutine flickerRoutine;

    public void StartFlicker()
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        flickerRoutine = StartCoroutine(FlickerLoop());
    }

    public void StopFlicker()
    {
        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
            flickerRoutine = null;
        }

        if (flickerPanel != null)
            flickerPanel.SetActive(false);
    }

    private IEnumerator FlickerLoop()
    {
        while (true)
        {
            flickerPanel.SetActive(true);
            yield return new WaitForSeconds(
                Random.Range(minDelay, maxDelay)
            );

            flickerPanel.SetActive(false);
            yield return new WaitForSeconds(
                Random.Range(minDelay, maxDelay)
            );

            // Occasionally leave the screen dark for a little longer
            if (Random.value > 0.7f)
                yield return new WaitForSeconds(0.25f);
        }
    }
}
