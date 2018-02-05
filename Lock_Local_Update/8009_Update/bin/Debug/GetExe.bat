set addr=%~dp0
"C:\Program Files (x86)\Microsoft\ILMerge"\ilmerge.exe /ndebug /target:winexe /out:%addr%\Lock_Update_Tools.exe %addr%\Lock_Concentrator_Update.exe %addr%\UsbLibrary.dll
pause 