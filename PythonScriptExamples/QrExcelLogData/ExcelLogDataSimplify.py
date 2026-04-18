import pandas as pd
import os

#信号白名单
signal_whitelist = ['(DCDC_1)DCDC_FaultCode','(DCDC_1)DCDC_FaultLevelSts', '(DCDC_1)DCDC_WorkingMode', '(DCDC_1)DCDC_LvSideVolt','(DCDC_2)DCDC_HvVolt',
                    '(OBC_4)OBC_FaultCode','(OBC_3_G)OBC_WorkingMode_PHEV','(OBC_3_G)OBC_CC_ConnectSts','(OBC_3_G)OBC_CP_DutyCycleValue',
                    '(NMm_CDU)NETWORK_WAKEUP_CDU','(NMm_CDU)ECUSPEC_WAKEUP_CDU','(NMm_CDU)NETWORK_AWAKE_CDU','(NMm_CDU)IGNITION_AWAKE_CDU','(NMm_CDU)DIAGNOSTIC_AWAKE_CDU','(NMm_CDU)ECUSPEC_AWAKE_CDU',
                    '(NMm_BMS)NETWORK_WAKEUP_BMS','(NMm_BMS)ECUSPEC_WAKEUP_BMS','(NMm_BMS)NETWORK_AWAKE_BMS','(NMm_BMS)IGNITION_AWAKE_BMS','(NMm_BMS)DIAGNOSTIC_AWAKE_BMS','(NMm_BMS)ECUSPEC_AWAKE_BMS',
                    '(NMm_HCU)NETWORK_WAKEUP_HCU','(NMm_HCU)ECUSPEC_WAKEUP_HCU','(NMm_HCU)NETWORK_AWAKE_HCU','(NMm_HCU)IGNITION_AWAKE_HCU','(NMm_HCU)DIAGNOSTIC_AWAKE_HCU','(NMm_HCU)ECUSPEC_AWAKE_HCU',
                    '(NMM_CEM)NETWORK_WAKEUP_CEM','(NMM_CEM)ECUSPEC_WAKEUP_CEM','(NMM_CEM)NETWORK_AWAKE_CEM','(NMM_CEM)IGNITION_AWAKE_CEM','(NMM_CEM)DIAGNOSTIC_AWAKE_CEM','(NMM_CEM)ECUSPEC_AWAKE_CEM',
                    '(NWM_CGW)NWM_CGW_NETWORK_WAKEUP','(NWM_CGW)NWM_CGW_ECUSPEC_WAKEUP','(NWM_CGW)NWM_CGW_NETWORK_AWAKE','(NWM_CGW)NWM_CGW_IGNITION_AWAKE','(NWM_CGW)NWM_CGW_DIAGNOSTIC_AWAKE','(NWM_CGW)NWM_CGW_ECUSPEC_AWAKE',
                    '(HCU_7)HCU_DCDC_StModeReq','(HCU_7)HCU_DCDCEnableReq',
                    '(BMS_7)BMSH_OBCChargeReq','(BMS_7)BMSH_OBCHeatRequest',
                    '平台接收时间']

excelLogFolderPath = os.getcwd() + "/excelLogData" #excel日志数据文件夹路径
files= os.listdir(excelLogFolderPath) #得到文件夹下的所有文件名称
if(files.__len__() != 1):
    print("请确保excelLogData文件夹下只有一个Excel日志数据文件！")
    input("按任意键退出...")
    exit()

excelFileName = files[0]
whiteData = pd.read_excel(os.path.join(excelLogFolderPath, excelFileName), sheet_name=0, usecols=signal_whitelist)

df = pd.DataFrame(whiteData)
df.to_excel('简化后数据.xlsx', index=False)
print("数据简化完成，已保存为简化后数据.xlsx")

input("按任意键退出...")