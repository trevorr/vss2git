; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "Vss2Git"
!define PRODUCT_VERSION "1.0.11"
!define PRODUCT_PUBLISHER "Trevor Robinson"
!define PRODUCT_WEB_SITE "https://github.com/trevorr/vss2git"
!define PRODUCT_REGISTRY_KEY "Software\Vss2Git"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\Vss2Git.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

; UAC-friendly MUI
!define MULTIUSER_EXECUTIONLEVEL Highest
!define MULTIUSER_MUI
!define MULTIUSER_INSTALLMODE_COMMANDLINE
!define MULTIUSER_INSTALLMODE_DEFAULT_CURRENTUSER
!define MULTIUSER_INSTALLMODE_DEFAULT_REGISTRY_KEY "${PRODUCT_REGISTRY_KEY}"
!define MULTIUSER_INSTALLMODE_DEFAULT_REGISTRY_VALUENAME "InstallMode"
!define MULTIUSER_INSTALLMODE_INSTDIR_REGISTRY_KEY "${PRODUCT_REGISTRY_KEY}"
!define MULTIUSER_INSTALLMODE_INSTDIR_REGISTRY_VALUENAME "InstallPath"
!include MultiUser.nsh
!include MUI2.nsh

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Welcome page
!insertmacro MUI_PAGE_WELCOME
; License page
!insertmacro MUI_PAGE_LICENSE "LICENSE.txt"
; System-wide or per-user install
!insertmacro MULTIUSER_PAGE_INSTALLMODE
; Directory page
!insertmacro MUI_PAGE_DIRECTORY
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!define MUI_FINISHPAGE_RUN "$INSTDIR\Vss2Git.exe"
!define MUI_FINISHPAGE_SHOWREADME "$INSTDIR\Vss2Git.html"
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "English"

; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Vss2GitSetup-${PRODUCT_VERSION}.exe"
InstallDir "$PROGRAMFILES\Vss2Git"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show

Function .onInit
  !insertmacro MULTIUSER_INIT
  ClearErrors
  ReadRegStr $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5" "InstallPath"
  IfErrors 0 continue
  ReadRegStr $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "InstallPath"
  IfErrors 0 continue
  MessageBox MB_YESNO|MB_ICONQUESTION \
    "Microsoft .NET Framework 3.5 or later is required but not detected. Continue installation?" IDYES continue
  Abort
continue:
FunctionEnd

Section "Program" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  CreateDirectory "$SMPROGRAMS\Vss2Git"
  CreateShortCut "$SMPROGRAMS\Vss2Git\Vss2Git.lnk" "$INSTDIR\Vss2Git.exe"
  CreateShortCut "$DESKTOP\Vss2Git.lnk" "$INSTDIR\Vss2Git.exe"
  File "Vss2Git\bin\Release\Vss2Git.exe"
  File "Vss2Git\bin\Release\Vss2Git.exe.config"
  File "Vss2Git\bin\Release\Hpdi.VssLogicalLib.dll"
  File "Vss2Git\bin\Release\Hpdi.VssPhysicalLib.dll"
  File "Vss2Git\bin\Release\Hpdi.HashLib.dll"
SectionEnd

Section "Docs" SEC02
  File "LICENSE.html"
  File "LICENSE.txt"
  File "Vss2Git.html"
  File "Vss2Git.png"
SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\Vss2Git\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\Vss2Git\Uninstall.lnk" "$INSTDIR\uninst.exe" /$MultiUser.InstallMode
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\Vss2Git.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" \
    "$INSTDIR\uninst.exe /$MultiUser.InstallMode"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\Vss2Git.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd


Function un.onInit
  !insertmacro MULTIUSER_UNINIT
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 \
    "Are you sure you want to completely remove $(^Name) and all of its components?" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) was successfully removed from your computer."
FunctionEnd

Section Uninstall
  Delete "$INSTDIR\${PRODUCT_NAME}.url"
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\Vss2Git.png"
  Delete "$INSTDIR\Vss2Git.html"
  Delete "$INSTDIR\LICENSE.txt"
  Delete "$INSTDIR\LICENSE.html"
  Delete "$INSTDIR\Hpdi.HashLib.dll"
  Delete "$INSTDIR\Hpdi.VssPhysicalLib.dll"
  Delete "$INSTDIR\Hpdi.VssLogicalLib.dll"
  Delete "$INSTDIR\Vss2Git.exe.config"
  Delete "$INSTDIR\Vss2Git.exe"

  Delete "$SMPROGRAMS\Vss2Git\Uninstall.lnk"
  Delete "$SMPROGRAMS\Vss2Git\Website.lnk"
  Delete "$DESKTOP\Vss2Git.lnk"
  Delete "$SMPROGRAMS\Vss2Git\Vss2Git.lnk"

  RMDir "$SMPROGRAMS\Vss2Git"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd
