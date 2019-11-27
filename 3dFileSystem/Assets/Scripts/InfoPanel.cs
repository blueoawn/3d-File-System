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
            dn.GetFolderSize(dn.FileInfos(dn.Path));
            typeText.SetText("Folder");
            icon.sprite = folderIcon;
        }
        else if(dn.IsDir)
        {
            typeText.SetText("Drive");
            icon.sprite = driveIcon;
        }
        else
        {
            if(string.IsNullOrEmpty(dn.Extension))
            {
                typeText.SetText($"File");
            }
            else
            {
                typeText.SetText($"File ({dn.Extension})");
            }
            icon.sprite = fileIcon;
        }
        nameText.SetText(dn.Name);
        nameText.fontStyle = FontStyles.Bold;
        locationText.SetText(dn.Path);
        sizeText.SetText(string.Format("{0:#,##0}", dn.Size) + " bytes");
        dateCreatedText.SetText(dn.DateCreated);
        dateModifiedText.SetText(dn.DateModified);
    }
}
