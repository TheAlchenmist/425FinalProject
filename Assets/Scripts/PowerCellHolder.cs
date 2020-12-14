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
    Quaternion RotationClosed;
    
    public float OpenTime = 2f;

    bool isClosed = true;
    bool opening = false;

    float changeSign;

    // Start is called before the first frame update
    void Start()
    {
        RotationOpen = Quaternion.Euler(0, AngleOpen, 0);
        RotationClosed = Quaternion.Euler(0, AngleClosed, 0);
    }

    public IEnumerator OpenDoor()
    {
        if (opening)
        {
            changeSign *= -1;
            isClosed = !isClosed;
            yield break;
        }

        float iParam = 0;

        opening = true;

        if (isClosed)
        {
            iParam = 0;
            changeSign = 1;
        }
        else
        {
            iParam = 1;
            changeSign = -1;
        }

        while (opening)
        {
            iParam = iParam + changeSign * Time.deltaTime / OpenTime;

            if (iParam >= 1 || iParam <= 0)
            {
                iParam = Mathf.Clamp(iParam, 0, 1);
                opening = false;
            }

            PowerCellDoor.transform.localRotation = Quaternion.Lerp(RotationClosed, RotationOpen, iParam);

            yield return null;
        }
        turnOnPowerCell.GetComponent<CapsuleCollider>().enabled = true;
        isClosed = !isClosed;
    }
}
