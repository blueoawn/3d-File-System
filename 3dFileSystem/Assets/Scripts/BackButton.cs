using System;
using System.IO;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class BackButton : MonoBehaviour
{
    public Text txtSelectedNode;
    public Text txtHoveredOverNode;

    public void SetCurrentSelectedNode(){
       DataNode node = GridSystem.Instance.currentSelectedDataNode;
       node.CollapseNode();
        //if current Selected has a parent or it has been collapsed
        if(node.IsDir && node.HasChild && node.parentNode){
            GridSystem.Instance.currentSelectedDataNode = node.parentNode.GetComponent<DataNode>();
        }
    }

}
