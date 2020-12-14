using System.Linq;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public EntryConsoleHandler[] entryConsoles;
    public bool doorOpened = false;

    // Update is called once per frame
    void Update()
    {
        if (entryConsoles.All(entryConsole => entryConsole.IsOn()))
        {
            if (!doorOpened)
            {
                GetComponent<Animation>().Play();
                doorOpened = true;
            }
        }
    }
}
