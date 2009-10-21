# Rules to more easily specify a C# build for automake.
#
# Inspired and adapted from Banshee's build system

include $(top_srcdir)/build.rules.common.mk

COMPONENT_REFERENCES = $(foreach ref, $(PROJECT_REFERENCES),-r:$(BUILD_DIR)/$(ref).dll)
COMPONENT_DEPS = $(foreach ref,$(PROJECT_REFERENCES),$(BUILD_DIR)/$(ref).dll)

moduledir = $(pkglibdir)
# Install libraries as data; there's no need for them to be excutable
module_DATA = $(foreach file,$(filter %.dll,$(OUTPUT_FILES)),$(file) $(file).mdb) $(foreach file,$(filter %.exe,$(OUTPUT_FILES)),$(file).mdb)
# Install executables as scripts
module_SCRIPTS = $(filter %.exe,$(OUTPUT_FILES))

all: $(ASSEMBLY_FILE)

$(ASSEMBLY_FILE).mdb: $(ASSEMBLY_FILE)

build-debug :
	@echo $(COMPONENT_REFERENCES)

#
# pkg-config handling
#
pkgconfigdir = $(libdir)/pkgconfig
pkgconfig_DATA = $(PKG_CONFIG_FILES)

%.pc : %.pc.in
	@colors=no; \
	case $$TERM in \
                "xterm" | "rxvt" | "rxvt-unicode") \
                        test "x$$COLORTERM" != "x" && colors=yes ;; \
                "xterm-color") colors=yes ;; \
	esac; \
	if [ "x$$colors" = "xyes" ]; then \
                tty -s && true || { colors=no; true; } \
	fi; \
	test "x$$colors" = "xyes" && \
	        echo -e "\033[1mGenerating $(notdir $@)...\033[0m" || \
	        echo "Generating $(notdir $@)...";
	@sed "s,\@ABI_VERSION\@,$(ABI_VERSION),g; s,\@expanded_libdir\@,$(expanded_libdir),g; s,\@PACKAGE\@,$(PACKAGE),g" < $< > $@
