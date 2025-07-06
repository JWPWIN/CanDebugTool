using I18N.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AppRoot : MonoBehaviour
{
    static private AppRoot instance;

    //主界面按钮管理
    MainWinBtnMng mainWinMng;

    //CAN DBC配置文件管理
    CanDbcDataManager canDbcDataMng;

    //数据显示窗口管理
    WinPanelMng winPanelMng;

    //CAN设备管理
    CanDeviceMng canDeviceMng;

    //日志显示管理
    LogMng logMng;

    static public AppRoot GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("AppRoot instance dont existed !");
            return null;
        }
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;

        mainWinMng = new MainWinBtnMng();

        canDbcDataMng = new CanDbcDataManager();

        winPanelMng = new WinPanelMng();

        canDeviceMng= new CanDeviceMng();

        logMng = new LogMng();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        //更新窗口
        WinPanelMng.GetInstance().UpdateWinPanel();
    }

}
