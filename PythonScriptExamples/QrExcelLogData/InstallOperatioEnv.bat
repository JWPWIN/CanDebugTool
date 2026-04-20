python --version

if %errorlevel% neq 0 goto InstallingPY
if %errorlevel% equ 0 goto InstallingDepPkg

:InstallingPY
echo Have not installed Python yet, Start to install Python3.8...
echo Starting the setup program of Python3.8，please finished the steps then continue...
python-3.8.0.exe
goto InstallingDepPkg

:InstallingDepPkg
echo Finished the installation of Python3.8, start to install the dependent packages...
pip install pandas
pip install openpyxl
pip list
echo Finished the installation of dependent packages...
goto End

:End
echo Finished the environment of operation!
pause