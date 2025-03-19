@echo off
REM このバッチファイルはminicondaを使用する想定で記述されています

REM minicondaの実行ファイルまでのパス
REM 現在ログインしているユーザーフォルダからの相対パスを記述する
set CONDA_PATH=\Miniconda3\Scripts\activate.bat

REM Remdisを動作させる環境名
set CONDA_ENV=remdis

REM Remdis内のmodulesまでのパスを記述
set MODULE_PATH=..\Remdis\modules

REM Condaの起動とConda環境のアクティベート、Remdisのプログラムファイルがあるフォルダへの移動を行うコマンド
set COMMON_CMD="%USERPROFILE%%CONDA_PATH% && conda activate %CONDA_ENV% && cd %MODULE_PATH%"

start cmd /k %COMMON_CMD% "&& python input.py"
REM start cmd /k %COMMON_CMD% "&& python audio_vap.py"
start cmd /k %COMMON_CMD% "&& python asr.py"
start cmd /k %COMMON_CMD% "&& python dialogue.py"
start cmd /k %COMMON_CMD% "&& python tts.py"
