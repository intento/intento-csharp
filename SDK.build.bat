@echo off

set Configuration=Release
set DoSign=1
set Version=2.2.7

dotnet build SDK.build.proj /p:Configuration=%Configuration% /p:DoSign=%DoSign% /p:Version=%Version% /fileLogger

pause