using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookCfgWin
{
    //是否初始化
    bool isInit;

    //上位机面板预设体
    static readonly string panelPath = "Prefabs/CanDbcCfgWin";

    //上位机面板对象
    GameObject panelObj;

    //信号显示单元格预设物路径
    static readonly string sigCellPath = "Prefabs/SigCellCanCfg";

    //报文显示单元格预设物路径
    static readonly string msgCellPath = "Prefabs/MsgCellCanCfg";

    //信号接收内容显示对象
    GameObject viewContent;

    //配置单元格列表
    List<GameObject> cfgCells = new List<GameObject>();

    // Start is called before the first frame update
    public LookCfgWin()
    {
        isInit = false;

        panelObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(panelPath));
        //设置窗口显示位置
        panelObj.transform.SetParent(UITool.GetChildObjectByName(UITool.FindCanvas(), "DataDisplayWin").transform, false);

        GameObject obj = UITool.GetChildObjectByName(panelObj, "CanDbcCfg");
        obj = UITool.GetChildObjectByName(obj, "CanDbcCfgView");
        obj = UITool.GetChildObjectByName(obj, "Viewport");
        viewContent = UITool.GetChildObjectByName(obj, "Content");

    }

    /// <summary>
    /// 重置初始化参数
    /// </summary>
    public void ResetCfg()
    { 
        isInit = false;
    }

    /// <summary>
    /// 设置面板是否使能使用
    /// </summary>
    /// <param name="active">是否使能</param>
    public void SetPanelActive(bool active)
    {
        panelObj.SetActive(active);
    }

    // Update is called once per frame
    public void UpdateCfg()
    {
        if (true == CanDbcDataManager.GetInstance().isLoadCfg
            && false == isInit)
        {
            ClearWin();//重置窗口
            isInit = true;

            foreach (var item in CanDbcDataManager.GetInstance().canMsgSet)
            {
                //首先现在报文信息
                GameObject msgCell;
                msgCell = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(msgCellPath));

                //显示报文名
                UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(msgCell, "MsgName"), "Text").text = item.Value.msgName;
                //显示报文ID
                UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(msgCell, "MsgId"), "Text").text = "0x" + item.Value.msgId.ToString("x3").ToUpper();
                //显示报文周期
                UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(msgCell, "MsgCycle"), "Text").text = item.Value.msgCycle.ToString();

                msgCell.transform.SetParent(viewContent.transform);
                cfgCells.Add(msgCell);

                //显示该报文包含的信号
                foreach (var sig in item.Value.signals)
                {
                    GameObject sigCell;
                    sigCell = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(sigCellPath));
                    //显示信号名
                    UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(sigCell, "SigName"), "Text").text = sig.sigName;
                    //显示信号描述
                    UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(sigCell, "SigDesc"), "Text").text = sig.sigDesc;
                    //显示信号排列方式
                    UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(sigCell, "SigOrder"), "Text").text = sig.sigOrderType.ToString();
                    //显示信号开始位置
                    UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(sigCell, "SigStartBit"), "Text").text = sig.sigStartBit.ToString();
                    //显示信号长度
                    UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(sigCell, "SigLen"), "Text").text = sig.sigLen.ToString();
                    //显示信号精度
                    UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(sigCell, "SigFactor"), "Text").text = sig.sigFactor.ToString();
                    //显示信号偏移
                    UITool.GetOrAddComponentInChildByName<Text>(UITool.GetChildObjectByName(sigCell, "SigOffset"), "Text").text = sig.sigOffset.ToString();

                    sigCell.transform.SetParent(viewContent.transform);
                    cfgCells.Add(sigCell);
                }

            }


        }
    }
    /// <summary>
    /// 重置该窗口
    /// </summary>
    public void ClearWin()
    {
        isInit = false;

        if (cfgCells.Count > 0)
        {
            foreach (var item in cfgCells)
            {
                GameObject.Destroy(item.gameObject);
            }
        }

        cfgCells.Clear();
    }
}
