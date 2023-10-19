@echo off

set Configuration=Release
set DoSign=0
set Version=2.2.26

dotnet build SDK.build.proj /p:Configuration=%Configuration% /p:DoSign=%DoSign% /p:Version=%Version% /fileLogger

pause