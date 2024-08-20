using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpHUD : MonoBehaviour
{
    [SerializeField] private string Message;
    [SerializeField] private GameObject PopUpParent;
    [SerializeField] private TMP_Text messageText;
    [SerializeField]
    private float hideDelay = 3.0f;
    private Transform camTransform;
    public Transform CamTransform 
    { 
        get {
            if (camTransform == null) { camTransform = Camera.main.transform; }
            return camTransform; 
        } 
    }

    void Update()
    {
        PopUpParent.transform.LookAt(CamTransform.position);
    }


    public void ShowPopUpHud(Vector3 position)
    {
        CancelInvoke();
        PopUpParent.SetActive(true);
        transform.position = position;
    }

    public void UpdatePopupMessage(string message)
    {
        messageText.text = message;
    }

    public void HidePopUpHudDummy()
    {
        CancelInvoke();
        Invoke(nameof(HidePopUpHud), hideDelay);
    }

    public void HidePopUpHud()
    {
        PopUpParent?.SetActive(false);
    }

/*    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {c
            PopUpParent.SetActive(true);
            messageText.text = Message + " " + (useData ?  GameManager.Instance.FlyRequired : "");
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PopUpParent.SetActive(false);

        }

    }*/
}
