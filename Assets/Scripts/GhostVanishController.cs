using UnityEngine;

public class GhostVanishController : MonoBehaviour
{
    #region Fields
    [Header("References")]
    [SerializeField] private SpriteRenderer _ghostSprite;
    [SerializeField] private GameObject _dustEffectPrefab;
    [SerializeField] private Transform _effectSpawnPoint;

    private GameObject _currentEffect;
    #endregion

    #region Unity Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HideGhost();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShowGhost();
        }
    }
    #endregion

    #region Private Methods
    private void HideGhost()
    {
        // Desativa o fantasma
        _ghostSprite.enabled = false;

        _currentEffect = Instantiate(
                 _dustEffectPrefab,
                 _effectSpawnPoint != null ? _effectSpawnPoint.position : transform.position,
                 Quaternion.identity,
                 transform

             );
        Destroy(_currentEffect, 0.8f);
    }

    private void ShowGhost()
    {
        _ghostSprite.enabled = true;

    }
    #endregion
}
