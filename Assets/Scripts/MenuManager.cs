using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject train;
    [SerializeField] private Color color;
    [SerializeField] private Button btn1, btn2;
    [SerializeField] private Transform meio, spawn;
    [SerializeField] private GameObject activePanel;
    private Transform alvo;
    private Vector3 refe = Vector3.zero;
    public float speed = 0.25f;
    
    // Start is called before the first frame update
    void Start()
    {
        alvo = spawn;
    }

    // Update is called once per frame
    void Update()
    {
        activePanel.transform.position = Vector3.SmoothDamp(activePanel.transform.position, alvo.position, ref refe, speed);     

    }

    public void ConfigDie(){

        alvo = spawn;
        StartCoroutine(Wait());      
    }
    public void OpenConfig()
    {     
        
        activePanel.transform.position = spawn.transform.position;
        btn1.interactable = false;
        btn2.interactable = false;
        alvo = meio;
        activePanel.SetActive(true);

        
    }
    public void ChangeToOtherScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        activePanel.SetActive(false);
        btn1.interactable = true;
        btn2.interactable = true;
    }
}
