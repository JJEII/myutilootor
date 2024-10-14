@echo off
setlocal enabledelayedexpansion

REM PURPOSE: This file augments myutilootor's default functionality by enabling a
REM user to set output directories for .utl and .mut files (see below), then drag
REM as many .utl and .mut files as desired (in any combination) onto this batch
REM file, and it will appropriately convert each file and output the result to the
REM relevant directory.

REM !!! WARNING !!!
REM !!! WARNING: This OVERWRITES any files currently there that have the same name!
REM !!! WARNING !!!

REM ==== CHANGE THESE DIRECTORIES TO WHATEVER YOU WANT THEM TO BE. =================
set utlOutDir=C:\Games\VirindiPlugins\VirindiTank
set mutOutDir=C:\Games\VirindiPlugins\VirindiTank\mut\tmp
REM ================================================================================


set argc=0

for %%x in (%*) do (
   set /A argc+=1
   set "argv[!argc!]=%%~x"
   set "names[!argc!]=%%~nx"
   set "exts[!argc!]=%%~xx"
)

for /L %%i in (1,1,%argc%) do (
   echo PROCESSING: "!argv[%%i]!"
   if /i "!exts[%%i]!"==".mut" (
	  "%~dp0!myutilootor.exe" "!argv[%%i]!" "%utlOutDir%/!names[%%i]!.utl"
   ) else (
	  "%~dp0!myutilootor.exe" "!argv[%%i]!" "%mutOutDir%/!names[%%i]!.mut"
   )
)

echo.
echo FILES PROCESSED: %argc%
