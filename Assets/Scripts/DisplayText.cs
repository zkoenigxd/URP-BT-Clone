using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayText : MonoBehaviour
{
    [SerializeField] string message;
    [SerializeField] string subMessage;
    [SerializeField] Color textColor = Color.white;
    [SerializeField] bool isWarning;
    [SerializeField] bool reverse;

    TMP_Text textBox;
    TMP_Text subtextBox;
    GameObject warningBox;

    private void Awake()
    {
        GameObject parent = GameObject.FindGameObjectWithTag("Message Board");
        warningBox = parent.transform.GetChild(0).gameObject;
        textBox = parent.transform.GetChild(1).GetComponent<TMP_Text>();
        subtextBox = parent.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();

        textBox.gameObject.SetActive(false);
        warningBox.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            if (!reverse)
                ActivateMessageBox();
            else
                DeactivateMessageBox();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            if (!reverse)
                DeactivateMessageBox();
            else
                ActivateMessageBox();
        }
    }

    void ActivateMessageBox()
    {
        textBox.gameObject.SetActive(true);
        textBox.text = message;
        subtextBox.text = subMessage;
        textBox.color = textColor;
        if (isWarning)
        {
            warningBox.SetActive(true);
            //Set warningbox size equal to text box size?
        }
    }

    void DeactivateMessageBox()
    {
        textBox.gameObject.SetActive(false);
        warningBox.SetActive(false);
    }
}
