import can
import os

blfLogFolderPath = os.getcwd() + "/blfLog" #asc报文文件夹目录
ascLogFolderPath = os.getcwd() + "/ascLog" #blf报文文件夹目录

_0x5F4_CDU_CDU_TotalFaultNum = 0
_0x467_DCDC_FaultCode = 0
_0x49E_OBC_FaultCode = 0
fileIndex = 1

#检测BLF报文文件夹下的BLF文件故障信息
files= os.listdir(blfLogFolderPath) #得到文件夹下的所有文件名称
for file in files: #遍历文件夹
    absoluteFilePath = os.path.join(blfLogFolderPath, file)
    if not os.path.isdir(absoluteFilePath): #判断是否是文件夹，不是文件夹才打开
        blf_data = can.BLFReader(absoluteFilePath); #读取Blf文件
        #检测BLF报文数据故障信号
        _0x5F4_CDU_CDU_TotalFaultNum = 0
        _0x467_DCDC_FaultCode = 0
        _0x49E_OBC_FaultCode = 0
        print("正在检测BLF文件-" + str(fileIndex) + " : " + file)
        for msg in blf_data:
            #检测故障总数信号0x5F4-CDU_CDU_TotalFaultNum
            if msg.arbitration_id == 0x5F4 and msg.data[0] != 0:
                if msg.data[0] != _0x5F4_CDU_CDU_TotalFaultNum:
                    print("检测到故障：" + "0x5F4-CDU_CDU_TotalFaultNum : " + str(msg.data[0]))
                    _0x5F4_CDU_CDU_TotalFaultNum = msg.data[0]

            #检测DCDC故障码0x467-DCDC_FaultCode
            if msg.arbitration_id == 0x467 and msg.data[6] != 0:
                if msg.data[6] != _0x467_DCDC_FaultCode:
                    print("检测到故障：" + "0x467-DCDC_FaultCode : " + str(msg.data[6]))
                    _0x467_DCDC_FaultCode = msg.data[6]

            #检测OBC故障码0x49E-OBC_FaultCode
            if msg.arbitration_id == 0x49E and msg.data[7] != 0:
                if msg.data[7] != _0x49E_OBC_FaultCode:
                    print("检测到故障：" + "0x49E-OBC_FaultCode : " + str(msg.data[7]))
                    _0x49E_OBC_FaultCode = msg.data[7]
        print("完成检测BLF文件-"+ str(fileIndex) + " : " + file)
        if _0x5F4_CDU_CDU_TotalFaultNum != 0 or _0x467_DCDC_FaultCode != 0 or _0x49E_OBC_FaultCode != 0:
            print("ERROR！！！检测到故障信息！")
        fileIndex += 1
        print("================================================================")

print("BLF文件故障信息检测完成！")
print("################################################################")
_0x5F4_CDU_CDU_TotalFaultNum = 0
_0x467_DCDC_FaultCode = 0
_0x49E_OBC_FaultCode = 0
fileIndex = 1

# 检测ASC报文文件夹下的ASC文件故障信息
files= os.listdir(ascLogFolderPath) #得到文件夹下的所有文件名称
for file in files: #遍历文件夹
    absoluteFilePath = os.path.join(ascLogFolderPath, file)
    if not os.path.isdir(absoluteFilePath): #判断是否是文件夹，不是文件夹才打开
        asc_data = can.ASCReader(absoluteFilePath); #读取Asc文件
        #检测ASC报文数据故障信号
        _0x5F4_CDU_CDU_TotalFaultNum = 0
        _0x467_DCDC_FaultCode = 0
        _0x49E_OBC_FaultCode = 0
        print("正在检测ASC文件-" + str(fileIndex) + " : " + file)
        for msg in asc_data:
            #检测故障总数信号0x5F4-CDU_CDU_TotalFaultNum
            if msg.arbitration_id == 0x5F4 and msg.data[0] != 0:
                if msg.data[0] != _0x5F4_CDU_CDU_TotalFaultNum:
                    print("检测到故障：" + "0x5F4-CDU_CDU_TotalFaultNum : " + str(msg.data[0]))
                    _0x5F4_CDU_CDU_TotalFaultNum = msg.data[0]

            #检测DCDC故障码0x467-DCDC_FaultCode
            if msg.arbitration_id == 0x467 and msg.data[6] != 0:
                if msg.data[6] != _0x467_DCDC_FaultCode:
                    print("检测到故障：" + "0x467-DCDC_FaultCode : " + str(msg.data[6]))
                    _0x467_DCDC_FaultCode = msg.data[6]

            #检测OBC故障码0x49E-OBC_FaultCode
            if msg.arbitration_id == 0x49E and msg.data[7] != 0:
                if msg.data[7] != _0x49E_OBC_FaultCode:
                    print("检测到故障：" + "0x49E-OBC_FaultCode : " + str(msg.data[7]))
                    _0x49E_OBC_FaultCode = msg.data[7]
        print("完成检测ASC文件-" + str(fileIndex) + " : " + file)
        if _0x5F4_CDU_CDU_TotalFaultNum != 0 or _0x467_DCDC_FaultCode != 0 or _0x49E_OBC_FaultCode != 0:
            print("ERROR！！！检测到故障信息！")
        fileIndex += 1
        print("================================================================")

print("ASC文件故障信息检测完成！")

input("按任意键退出...")