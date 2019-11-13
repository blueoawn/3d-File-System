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
    public TextMeshProUGUI dateCreatedText;
    public TextMeshProUGUI dateModifiedText;

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
        if(dn.IsFolder)
        {
            dn.Size = dn.GetFolderSize(dn.FullName);
            typeText.SetText("Type: Folder");
        }
        else if(dn.IsDrive)
        {
            typeText.SetText("Type: Drive");
        }
        else
        {
            typeText.SetText($"Type: File ({dn.Extension})");
        }
        nameText.SetText(dn.Name);
        locationText.SetText("Location: " + dn.FullName);
        sizeText.SetText("Size: " + string.Format("{0:#,##0}", dn.Size) + " bytes");
        dateCreatedText.SetText($"Created: {dn.DateCreated}");
        dateModifiedText.SetText($"Modified: {dn.DateModified}");
    }
}
