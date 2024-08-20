using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpHUD : MonoBehaviour
{
    [SerializeField] private string Message;
    [SerializeField] private GameObject PopUpParent;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private bool useData = false;
    private Transform CamTrasform;
    void Start()
    {
        CamTrasform = Camera.main.transform;

    }

    // Update is called once per frame
    void Update()
    {
        PopUpParent.transform.LookAt(CamTrasform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
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

    }
}
