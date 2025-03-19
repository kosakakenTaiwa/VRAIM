using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TextGrabedObject : MonoBehaviour
{
    [SerializeField] private string TextGameObject;
    private XRGrabInteractable grabInteractable;
    
    void Awake()
    {
        //このオブジェクトにXRGrabInteractableがついているか確認
        grabInteractable = GetComponent<XRGrabInteractable>();

        //イベントリスナーを登録
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
        else
        {
            Debug.LogError("XRGrabIntertactable component is missing on this object.");
        }
    }
    
    //オブジェクトが掴まれたときに呼ばれる関数
    private void OnGrab(SelectEnterEventArgs args)
    {
        //掴んだオブジェクトの名前をコンソールに表示
        Debug.Log("Object Grabbed :" + TextGameObject);
    }

    //オブジェクトが放されたときに呼ばれる関数
    private void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("Object Released:" + TextGameObject);
    }

    void OnDestroy()
    {
        //イベントリスナーの解除
        if(grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }
}
