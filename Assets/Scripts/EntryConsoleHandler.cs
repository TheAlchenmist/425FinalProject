using UnityEngine;

public class EntryConsoleHandler : MonoBehaviour
{
    public GameObject player;
    public Material onColor;
    public Material offColor;
    public float proximity;
    
    private bool isItOn;
    private MeshRenderer mesh;
    private Vector3 consolePosition;
    public bool tutOpen = false;
    public GameController gc;

    // Start is called before the first frame update
    void Start()
    {
         isItOn = false;
         consolePosition = transform.GetChild(1).position;
         mesh = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
         mesh.material = offColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(consolePosition, player.transform.position) <= proximity && Input.GetKeyDown("e") && (GameObject.ReferenceEquals(player,gc.CheckActive())))
        {
            tutOpen = true;
            isItOn = !isItOn;
            mesh.material = isItOn ? onColor : offColor;
        }
    }

    public bool IsOn()
    {
        return isItOn;
    }
}
