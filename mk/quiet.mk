Q=$(if $(V),,@)
# echo -e "\\t" does not work on some systems, so use 5 spaces
Q_GEN=  $(if $(V),,@echo "GEN      $(@F)";)
QF_GEN= $(if $(V),,@echo "GEN      $@";)
Q_LN=   $(if $(V),,@echo "LN       $(@F)";)
QF_LN=  $(if $(V),,@echo "LN       $@";)
Q_MCS=  $(if $(V),,@echo "MCS      $(@F)";)
Q_CSC=  $(if $(V),,@echo "CSC      $(@F)";)
Q_GMCS= $(if $(V),,@echo "GMCS     $(@F)";)
Q_DMCS= $(if $(V),,@echo "DMCS     $(@F)";)
Q_SMCS= $(if $(V),,@echo "SMCS     $(@F)";)
Q_PMCS= $(if $(V),,@echo "PMCS     $(@F)";)
Q_AS=   $(if $(V),,@echo "AS       $(@F)";)
Q_CC=   $(if $(V),,@echo "CC       $(@F)";)
QT_CC=  $(if $(V),,@echo "CC       $$(@F)";)
Q_CXX=  $(if $(V),,@echo "CXX      $(@F)";)
Q_CCLD= $(if $(V),,@echo "CCLD     $(@F)";)
Q_OBJC= $(if $(V),,@echo "OBJC     $(@F)";)
QT_OBJC=$(if $(V),,@echo "OBJC     $$(@F)";)
Q_STRIP=$(if $(V),,@echo "STRIP    $(@F)";)
Q_CP=   $(if $(V),,@echo "CP       $(@F)";)
Q_AR=   $(if $(V),,@echo "AR       $(@F)";)
QT_AR=   $(if $(V),,@echo "AR      $$(@F)";)
Q_LIPO= $(if $(V),,@echo "LIPO     $(@F)";)
QT_LIPO= $(if $(V),,@echo "LIPO    $$(@F)";)
Q_MDB=  $(if $(V),,@echo "MDB      $(@F)";)
Q_NUNIT= $(if $(V),,@echo "NUNIT     $(@F)";)
Q_PACK     =$(if $(V),,@echo "PACK      $(@F)";)
Q_NUGET_ADD=$(if $(V),,@echo "NUGET ADD $(@F)";)
Q_NUGET_DEL=$(if $(V),,@echo "NUGET DEL $(@F)";)

Q_SN=   $(if $(V),,@echo "SN       $(@F)";)
Q_XBUILD=$(if $(V),,@echo "XBUILD  $(@F)";)
Q_TT=   $(if $(V),,@echo "TT       $(@F)";)
Q_BUILD=$(if $(V),,@echo "BUILD    $(@F)";)
Q_CURL=$(if $(V),,@echo "CURL     $(@F)";)
Q_ZIP=$(if $(V),,@echo "ZIP      $(@F)";)

Q_DOTNET_BUILD=$(if $(V),,@echo "CSC      [dotnet] $(@F)";)
Q_DOTNET_GEN  =$(if $(V),,@echo "GEN      [dotnet] $(@F)";)

Q_PROF_MCS =  $(if $(V),,@echo "MCS      [$(1)] $(@F)";)
Q_PROF_CSC =  $(if $(V),,@echo "CSC      [$(1)] $(@F)";)
Q_PROF_GEN  = $(if $(V),,@echo "GEN      [$(1)] $(@F)";)
Q_PROF_SN   = $(if $(V),,@echo "SN       [$(1)] $(@F)";)
Q_1 = $(if $(V),,@echo "$(1) $(@F)";)
Q_2 = $(if $(V),,@echo "$(1) $(2) $(@F)";)

ifeq ($(V),)
ifeq ($(BUILD_REVISION)$(JENKINS_HOME),)
# non-verbose local build
XBUILD_VERBOSITY=/nologo /verbosity:quiet
XBUILD_VERBOSITY_QUIET=/nologo /verbosity:quiet
MMP_VERBOSITY=-q
MTOUCH_VERBOSITY=-q
DOTNET_PACK_VERBOSITY=--verbosity:quiet --nologo
DOTNET_BUILD_VERBOSITY=--verbosity quiet --nologo -consoleLoggerParameters:NoSummary
DOTNET_WORKLOAD_VERBOSITY=--verbosity quiet
NUGET_VERBOSITY=-verbosity quiet
INSTALLER_VERBOSITY=
ZIP_VERBOSITY=--quiet
else
# CI build
XBUILD_VERBOSITY=/nologo /verbosity:normal
XBUILD_VERBOSITY_QUIET=/nologo /verbosity:quiet
MMP_VERBOSITY=-vvvv
MTOUCH_VERBOSITY=-vvvv
DOTNET_PACK_VERBOSITY=
DOTNET_BUILD_VERBOSITY=
DOTNET_WORKLOAD_VERBOSITY=--verbosity diagnostic
NUGET_VERBOSITY=
INSTALLER_VERBOSITY=
ZIP_VERBOSITY=
endif
else
# verbose build
XBUILD_VERBOSITY=/verbosity:diagnostic
XBUILD_VERBOSITY_QUIET=/verbosity:diagnostic
MMP_VERBOSITY=-vvvv
MTOUCH_VERBOSITY=-vvvv
DOTNET_PACK_VERBOSITY=--verbosity:detailed
DOTNET_BUILD_VERBOSITY=--verbosity detailed
DOTNET_WORKLOAD_VERBOSITY=--verbosity diagnostic
NUGET_VERBOSITY=-verbosity detailed
INSTALLER_VERBOSITY=-verbose -dumplog
ZIP_VERBOSITY=--verbose
endif
MSBUILD_VERBOSITY=$(XBUILD_VERBOSITY)
MSBUILD_VERBOSITY_QUIET=$(XBUILD_VERBOSITY_QUIET)
