using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class QuestArrowPointer : MonoBehaviour
{
    [SerializeField] private Camera _uiCamera;//Pega as dimenções da camera
    [SerializeField] private Sprite[] image;//altera entre a imagem da seta e quando chega ao destino
    public Transform _target;//Aqui colocamos as tasks
    [SerializeField] private Transform _player;
    [SerializeField] private TextMeshProUGUI goText;
    [SerializeField] private Color borderColor;

    private RectTransform pointerRectTransform;
    private Image pointerImage;
    private Canvas canvas;
    private Vector3 targetHolder;
    private bool sameTarget;
    private bool pointerOn = false;

    //Aqui são para animação
    private float floatSpeed = 2f;
    private float floatHeight = 0.8f;
    private float borderSize = 100f;

    public Vector3 offSet = new Vector3(0, 0, 0);

    private Image imageHolder;
    [SerializeField] private List<Image> imageList = new List<Image>();



    void Awake()
    {
        // _target = new Vector3(-17.318f, -6.48f);

    }
    void OnEnable()
    {
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        pointerImage = transform.Find("Pointer").GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();

    }
    private void Start()
    {

    }

    void Update()
    {
        if (pointerOn)
        {

            Vector3 targetScreenPoint = Camera.main.WorldToScreenPoint(_target.position);
            bool isOffScreen = targetScreenPoint.x <= borderSize || targetScreenPoint.x >= Screen.width - borderSize || targetScreenPoint.y <= borderSize || targetScreenPoint.y >= Screen.height - borderSize;
            //Debug.Log(isOffScreen + "  " + targetScreenPoint);

            if (isOffScreen)//Aqui verificamos se a task está fora da tela
            {
                // pointerImage.GetComponent<Animator>().SetBool("inLocal", false);
                RotatePointerTargetPosision();
                pointerImage.sprite = image[0];
                // Vector3 cappedScreenPos = targetScreenPoint;

                // cappedScreenPos.x = Mathf.Clamp(cappedScreenPos.x, borderSize, Screen.width - borderSize); //distanciamento do ponteiro entre a borda
                // cappedScreenPos.y = Mathf.Clamp(cappedScreenPos.y, borderSize, Screen.height - borderSize);
                // cappedScreenPos.z = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? 0f : canvas.planeDistance;

                // Vector3 pointerWorldPos = _uiCamera.ScreenToWorldPoint(cappedScreenPos);
                // pointerRectTransform.position = pointerWorldPos;

                Vector3 dirToTarget = (_target.position - _player.position).normalized;
                Vector3 pointerPosition = _player.position + dirToTarget * 2f;
                Vector3 screenPos = Camera.main.WorldToScreenPoint(pointerPosition);
                screenPos.z = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? 0f : canvas.planeDistance;
                Vector3 pointerWorldPos = _uiCamera.ScreenToWorldPoint(screenPos);
                pointerRectTransform.position = pointerWorldPos;

            }
            else //Aqui Indica quando a task estiver na tela
            {
                // pointerImage.GetComponent<Animator>().SetBool("inLocal", true);
                pointerImage.sprite = image[1];
                // Se o alvo estiver visível, você pode esconder a seta ou posicioná-la por cima dele
                // pointerRectTransform.gameObject.SetActive(false);
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(_target.position);
                Vector3 uiWorldPosition = _uiCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, _uiCamera.nearClipPlane));

                float newY = Mathf.PingPong(Time.time * floatSpeed, floatHeight);
                Vector3 floatingPos = uiWorldPosition + new Vector3(0, newY, 0);
                pointerRectTransform.position = floatingPos;
                pointerRectTransform.localEulerAngles = Vector3.zero;

            }
        }

        // if (player == null && _target == null)
        // {
        //     return;
        // }
        // transform.position = player.position + offSet;

        // Vector3 directionToTarget = _target.position - player.position;
        // directionToTarget.z = 0f;
        // RotatePointerTargetPosision();

    }

    //Aqui fazemos a rotação do poiteiro para a posição da task
    private void RotatePointerTargetPosision()
    {
        Vector3 toPosition = _target.position;
        // Vector3 fromPosition = Camera.main.transform.position;
        // fromPosition.z = 0f;
        toPosition.z = 0f;

        Vector3 dir = (toPosition - _player.position).normalized;

        float a = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; //faz o calculo do angulo da seta
        //float a = Mathf.Atan2(dir.y, dir.x) * (-180 / Mathf.PI);
        // angle = arccos(dot(v1, v2) / (magnitude(v1) * magnitude(v2)))
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, a);

    }

    public void SetPoint(Transform transform)
    {
        targetHolder = transform.position;
        if (targetHolder == _target.position)
        {
            sameTarget = true;
        }
        else
        {
            sameTarget = false;
        }
    }
    public void PointerOn()
    {
        _target.position = targetHolder;
        
        if (sameTarget)
        {
            imageHolder.color = Color.white;
            gameObject.SetActive(false);
            pointerOn = false;
            _target.position = Vector3.zero;
        }
        else
        {
            for(int i = 0; i < imageList.Count; i++)
            {
                imageList[i].color = Color.white;
            }
            imageHolder.color = borderColor;
            gameObject.SetActive(true);
            pointerOn = true;
        }

    }
    public void ChangeText(Image image)
    {
        imageHolder = image;

        if (pointerOn && sameTarget)
        {
            
            goText.text = "Parar de mostrar";
        } else
        {
            
            goText.text = "Mostrar local";
        }
    }

}
