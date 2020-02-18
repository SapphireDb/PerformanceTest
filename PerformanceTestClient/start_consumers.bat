@echo off

set count=%1

:loop
start cmd /c "node consumer.js"
set /a count=count-1

if %count%==0 goto exitloop
goto loop

:exitloop
