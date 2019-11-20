using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI locationText;
    public TextMeshProUGUI sizeText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI dateCreatedText;
    public TextMeshProUGUI dateModifiedText;

    public Image icon;
    public Sprite fileIcon;
    public Sprite folderIcon;
    public Sprite driveIcon;

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
        if(dn.IsDir)
        {
            dn.Size = dn.GetFolderSize(dn.Path);
            typeText.SetText("Type: Folder");
            icon.sprite = folderIcon;
        }
        else if(dn.IsDir)
        {
            typeText.SetText("Type: Drive");
            icon.sprite = driveIcon;
        }
        else
        {
            typeText.SetText($"Type: File ({dn.Extension})");
            icon.sprite = fileIcon;
        }
        nameText.SetText(dn.Name);
        locationText.SetText("Location: " + dn.Path);
        sizeText.SetText("Size: " + string.Format("{0:#,##0}", dn.Size) + " bytes");
        dateCreatedText.SetText($"Created: {dn.DateCreated}");
        dateModifiedText.SetText($"Modified: {dn.DateModified}");
    }
}
