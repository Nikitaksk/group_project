using UnityEngine;

public class BinDistributor : MonoBehaviour
{
    [Header("Spacing Settings")]
    [Tooltip("Space between the screen edges and the outer bins (world units).")]
    public float edgePadding = 0.5f;

    [Tooltip("Space between each bin (world units).")]
    public float spacingBetweenBins = 0.2f;

    [Tooltip("The vertical position of the bins in world units.")]
    public float yPosition = -4.2f;

    [Header("Constraints")]
    [Tooltip("Maximum scale multiplier allowed (relative to prefab's 1.0 scale).")]
    public float maxScaleMultiplier = 2.0f;

    [Header("References")]
    public GameObject[] bins;

    void Start()
    {
        DistributeAndResizeBins();
    }

    [ContextMenu("Distribute and Resize")]
    public void DistributeAndResizeBins()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // 2. Calculate Screen and Available Space
        float screenHeightWorld = 2f * cam.orthographicSize;
        float screenWidthWorld = screenHeightWorld * cam.aspect;

        int n = bins.Length;
        if (n == 0) return;

        // Total width available for the actual bin objects
        float totalAvailableWidth = screenWidthWorld - (2 * edgePadding) - ((n - 1) * spacingBetweenBins);
        float targetWorldWidthPerBin = totalAvailableWidth / n;

        // 3. Resize and Position Bins
        float currentX = -screenWidthWorld / 2f + edgePadding;

        for (int i = 0; i < n; i++)
        {
            if (bins[i] == null) continue;

            SpriteRenderer sr = bins[i].GetComponentInChildren<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                // To make them the "same size", we calculate the scale needed to reach targetWorldWidthPerBin
                // Scale = TargetWorldWidth / NativeSpriteWidth
                float nativeWidth = sr.sprite.bounds.size.x;
                float targetScale = targetWorldWidthPerBin / nativeWidth;

                // Clamp to prevent them from becoming too massive if there's only 1 bin
                targetScale = Mathf.Min(targetScale, maxScaleMultiplier);

                bins[i].transform.localScale = new Vector3(targetScale, targetScale, 1f);

                // Recalculate actual width after scaling (it might be clamped)
                float actualWorldWidth = nativeWidth * targetScale;

                // Position: CurrentX is the left edge of where the bin should go
                // We set transform.position (center) to CurrentX + HalfWidth
                float xPos = currentX + (actualWorldWidth / 2f);
                bins[i].transform.position = new Vector3(xPos, yPosition, bins[i].transform.position.z);

                // Move currentX forward for the next bin: Add this bin's width and the spacing
                currentX += actualWorldWidth + spacingBetweenBins;
            }
        }

        Debug.Log($"BinDistributor: Resized {n} bins to target width {targetWorldWidthPerBin:F2}. Screen: {screenWidthWorld:F2}");
    }
}
