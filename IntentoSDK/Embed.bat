echo off
set arg1=%1
set arg2=%2
%arg1%..\ilmerge\ilmerge /targetplatform:v4 /out:%arg1%IntentoSDK.dll %arg1%%arg2%IntentoSDK.dll %arg1%..\Newtonsoft\Newtonsoft.Json.dll

