using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI locationText;
    public TextMeshProUGUI sizeText;
    public TextMeshProUGUI typeText;

    //public Image icon;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fillPanel(DataNode dn)
    {
        nameText.SetText(dn.Name);
        locationText.SetText("Location: " + dn.FullName);
        sizeText.SetText("Size: " + string.Format("{0:#,##0}", dn.Size) + " bytes");
        
        if(dn.IsFolder)
        {
            typeText.SetText("Type: Folder");
        }
        else if(dn.IsDrive)
        {
            typeText.SetText("Type: Drive");
        }
        else
        {
            typeText.SetText("Type: File");
        }
    }
}
