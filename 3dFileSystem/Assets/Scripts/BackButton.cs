using System;
using System.IO;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class BackButton : MonoBehaviour
{
    public void SetCurrentSelectedNode()
    {
        DataNode node = GridSystem.Instance.currentSelectedDataNode;
        if(node.parentDataNode)
        {
            node = node.parentDataNode;
            GridSystem.Instance.currentSelectedDataNode = node;
        }
        
        node.CollapseNode();
        MainCamera.Instance.BackButtonPressed = true;
    }
}
