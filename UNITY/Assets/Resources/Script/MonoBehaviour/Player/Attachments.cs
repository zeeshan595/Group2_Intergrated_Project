using UnityEngine;
using System.Collections;

public class Attachments : MonoBehaviour
{
    public enum AttachmentType
    {
        health = 0
    }

    public AttachmentType attachmentType = AttachmentType.health;
    public Vector3 offset;

    public void Activate(GameObject car)
    {
        switch (attachmentType)
        {
            case AttachmentType.health:
                car.GetComponent<Car>().increaseHealth(1);
                break;
        }
    }
}