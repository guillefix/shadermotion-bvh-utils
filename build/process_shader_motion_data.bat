REM set arg1=%1
set arg1=D:\code\metagen\shadermotion_data_test
FOR /R %arg1% %%G IN (*.mp4) DO (shadermotion2.exe --video "%%G" -logFile .\log.txt)
REM FOR /R %arg1% %%G IN (*.mp4) DO (@echo %%G)
timeout 5