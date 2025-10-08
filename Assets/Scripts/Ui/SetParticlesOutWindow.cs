using UnityEngine;

public enum PositionY{
    Top,
    Down
}
public enum PositionX{
    Left,
    Right
}
public class SetParticlesOutWindow : MonoBehaviour
{
    [SerializeField] private PositionX posX;
    [SerializeField] private PositionY posY;
    // Start is called before the first frame update
    void Start()
    {
        if(posX == PositionX.Left && posY == PositionY.Top){
            gameObject.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1, (Camera.main.ScreenToWorldPoint(Vector3.zero).y * -1) + 1);
            gameObject.transform.eulerAngles = new Vector3(0f, -180f, gameObject.transform.eulerAngles.z);
        }
        else if(posX == PositionX.Left && posY == PositionY.Down){
            gameObject.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1, (Camera.main.ScreenToWorldPoint(Vector3.zero).y) + 1);
            gameObject.transform.eulerAngles = new Vector3(-180f, -180f, gameObject.transform.eulerAngles.z);
        }
        else if(posX == PositionX.Right && posY == PositionY.Top){
            gameObject.transform.position = new Vector2((Camera.main.ScreenToWorldPoint(Vector3.zero).x -1) *-1 , (Camera.main.ScreenToWorldPoint(Vector3.zero).y * -1) + 1);
            gameObject.transform.eulerAngles = new Vector3(-180f, -180, gameObject.transform.eulerAngles.z);
        }
        else if(posX == PositionX.Right && posY == PositionY.Down){
            gameObject.transform.position = new Vector2((Camera.main.ScreenToWorldPoint(Vector3.zero).x -1) *-1, (Camera.main.ScreenToWorldPoint(Vector3.zero).y) + 1);
            gameObject.transform.eulerAngles = new Vector3(0f, -180, gameObject.transform.eulerAngles.z);
        }
    }

}
