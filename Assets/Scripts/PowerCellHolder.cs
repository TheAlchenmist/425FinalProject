using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCellHolder : MonoBehaviour
{
    public GameObject PowerCellDoor = default;
    public GameObject turnOnPowerCell;

    public float AngleClosed = 0f;
    public float AngleOpen = 120f;

    Quaternion RotationOpen;
    
    public float OpenTime = 2f;

    bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        RotationOpen = Quaternion.Euler(0, AngleOpen, 0);
    }

    public IEnumerator OpenDoor()
    {
        float iParam = 0;

        while (!isOpen)
        {
            iParam = iParam + Time.deltaTime / OpenTime;

            if (iParam >= 1)
            {
                iParam = 1;
                isOpen = true;
            }

            PowerCellDoor.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), RotationOpen, iParam);

            yield return null;
        }
        turnOnPowerCell.GetComponent<CapsuleCollider>().enabled = true;
        isOpen = true;
    }
}
