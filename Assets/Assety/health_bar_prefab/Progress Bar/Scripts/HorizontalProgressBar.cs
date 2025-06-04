using UnityEngine;

public class HorizontalProgressBar : MonoBehaviour
{
    [Header("Overlay Bar")]
    public RectTransform overlayBar;
    public float sizeMin = 0.02f;
    public float sizeMax = 0.98f;

    [Header("Options")]
    public bool invertProgress = false;
    public float transitionTime = 0f;

    private float currentProgress = 1f;

    public void SetProgress(float value)
    {
        value = Mathf.Clamp01(value);
        currentProgress = invertProgress ? 1f - value : value;

        float newSize = Mathf.Lerp(sizeMin, sizeMax, currentProgress);

        if (overlayBar != null)
        {
            Vector3 scale = overlayBar.localScale;
            overlayBar.localScale = new Vector3(newSize, scale.y, scale.z);
        }
    }
}
