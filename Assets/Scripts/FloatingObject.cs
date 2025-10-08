using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    #region Fields
    [Header("Floating Settings")]
    [SerializeField] private float _floatSpeed = 2f;     // Velocidade da subida/descida
    [SerializeField] private float _floatHeight = 0.5f;  // Altura máxima de movimento
    [SerializeField] private float _startOffset = 0f;    // Offset para variar entre fantasmas

    private Vector3 _startPosition; // Posição inicial
    private float _timeCounter;     // Contador interno
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _startPosition = transform.position;
        _timeCounter = _startOffset; // Permite fantasmas começarem em momentos diferentes
    }

    private void Update()
    {
        AnimateFloating();
    }
    #endregion

    #region Private Methods
    private void AnimateFloating()
    {
        // Incrementa o tempo
        _timeCounter += Time.deltaTime * _floatSpeed;

        // Calcula posição Y com base no seno (movimento suave)
        float newY = _startPosition.y + Mathf.Sin(_timeCounter) * _floatHeight;

        // Aplica nova posição
        transform.position = new Vector3(_startPosition.x, newY, _startPosition.z);
    }
    #endregion
}
