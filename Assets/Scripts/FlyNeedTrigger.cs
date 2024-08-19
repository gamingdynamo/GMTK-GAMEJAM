using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlyNeedTrigger : MonoBehaviour
{
    [SerializeField] private int NeededFliesCount;
    [SerializeField] private GameObject flyNeedHUD;
    [SerializeField]private TextMeshProUGUI flyNeedText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          flyNeedHUD.transform.LookAt(Camera.main.transform);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            flyNeedText.text = "You Need Flies " + (NeededFliesCount - GameManager.Instance.FlyCount);
            flyNeedHUD.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            flyNeedHUD.SetActive(false);

        }
    }
}
