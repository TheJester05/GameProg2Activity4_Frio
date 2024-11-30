using System.Collections;
using UnityEngine;

public class KhaZixInvisibility : MonoBehaviour
{
    public float invisibilityDuration = 3f; // How long the character stays invisible
    public float cooldownDuration = 10f;   // Time before the ability can be reused
    private bool isOnCooldown = false;
    private Renderer characterRenderer;
    private Color originalColor;

    private void Start()
    {
        characterRenderer = GetComponent<Renderer>();

        if (characterRenderer == null)
        {
            Debug.LogError("Renderer component not found! Ensure the character has a Renderer.");
            return;
        }

        originalColor = characterRenderer.material.color; // Store the original color
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isOnCooldown)
        {
            StartCoroutine(ActivateInvisibility());
        }
    }

    private IEnumerator ActivateInvisibility()
    {
        isOnCooldown = true;

        // Gradually fade out
        yield return StartCoroutine(FadeToAlpha(0f));

        // Wait for the invisibility duration
        yield return new WaitForSeconds(invisibilityDuration);

        // Gradually fade back in
        yield return StartCoroutine(FadeToAlpha(1f));

        // Start cooldown
        yield return new WaitForSeconds(cooldownDuration);

        isOnCooldown = false;
    }

    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        Color color = characterRenderer.material.color;
        float startAlpha = color.a;
        float duration = 0.5f; // Time it takes to fade in or out
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            color.a = newAlpha;
            characterRenderer.material.color = color;
            yield return null;
        }

        // Ensure final alpha is set accurately
        color.a = targetAlpha;
        characterRenderer.material.color = color;
    }
}
