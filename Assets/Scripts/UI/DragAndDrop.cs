using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] AudioSource audioSource;

    public void OnBeginDrag(PointerEventData data)
    {
        audioSource.Play();
    }
    public void OnEndDrag(PointerEventData data)
    {
        audioSource.Play();
    }
}
