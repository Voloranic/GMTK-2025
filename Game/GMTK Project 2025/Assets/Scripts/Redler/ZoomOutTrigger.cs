using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class ZoomOutTrigger : MonoBehaviour
{
    private CinemachineCamera cineCam;

    [SerializeField] private string triggeredTag = "Player";

    [SerializeField] private float zoomedOutSize = 15f;
    [SerializeField] private float zoomOutSpeed = 5f;
    [SerializeField] private float zoomInSpeed = 7.5f;
    [SerializeField] private float zoomedOutDuration = 1f;
    [SerializeField] private bool triggerOnlyOnce = true;

    private float originalSize;
    private Coroutine zoomCoroutine;
    private bool triggeredOnce = false;

    [System.Obsolete]
    private void Awake()
    {
        cineCam = FindObjectOfType<CinemachineCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggeredOnce && collision.CompareTag(triggeredTag))
        {
            triggeredOnce = true;
            if (zoomCoroutine != null)
                StopCoroutine(zoomCoroutine);

            zoomCoroutine = StartCoroutine(ZoomRoutine());

            // Disable this collider to prevent retriggering
            Collider2D col = GetComponent<Collider2D>();
            if (col != null && triggerOnlyOnce)
                col.enabled = false;
        }
    }

    private IEnumerator ZoomRoutine()
    {
        if (cineCam == null) yield break;

        var lens = cineCam.Lens;
        originalSize = lens.OrthographicSize;

        // Zoom out
        yield return StartCoroutine(SmoothZoom(zoomedOutSize, zoomOutSpeed));

        // Wait
        yield return new WaitForSeconds(zoomedOutDuration);

        // Zoom back in
        yield return StartCoroutine(SmoothZoom(originalSize, zoomInSpeed));
    }

    private IEnumerator SmoothZoom(float to, float speed)
    {
        float t = 0f;
        float currentSize = cineCam.Lens.OrthographicSize;

        while (Mathf.Abs(currentSize - to) > 0.01f)
        {
            t += Time.deltaTime * speed;
            float newSize = Mathf.Lerp(currentSize, to, t);

            var lens = cineCam.Lens;
            lens.OrthographicSize = newSize;
            cineCam.Lens = lens;

            currentSize = newSize; // update to continue loop
            yield return null;
        }

        // Final snap
        var finalLens = cineCam.Lens;
        finalLens.OrthographicSize = to;
        cineCam.Lens = finalLens;
    }

}
