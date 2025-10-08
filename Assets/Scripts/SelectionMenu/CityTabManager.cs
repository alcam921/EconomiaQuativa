using UnityEngine;

public class CityTabManager : MonoBehaviour
{
    private Animator _anim;
    private bool _tabOpen = true;
    [SerializeField] private GameObject tabObject;

    // Start is called before the first frame update
    void Start()
    {
        _anim = tabObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TabOpenClose(bool status){

        if(!status){
            _anim.SetBool("Open", true);
            _anim.SetBool("Close", false);
        }else{

            _anim.SetBool("Open", false);
            _anim.SetBool("Close", true);
        }
    }
}
