BIN=Program

nothing:

run:
	dotnet run --project $(BIN)/$(BIN).csproj &

run-release:
	dotnet run --project $(BIN)/$(BIN).csproj --configuration Release &


##########################
# PROFILING
##########################

PROFILING_DIR=.profiling
DUMP_FILE=dump.dmp
GCDUMP_FILE=gcdump.gcdump
TRACE_FILE=trace.nettrace

setup-profiling:
	mkdir -p $(PROFILING_DIR)
	dotnet tool install --global dotnet-dump
	dotnet tool install --global dotnet-gcdump
	dotnet tool install --global dotnet-trace

dump:
	dotnet-dump collect \
		--output $(PROFILING_DIR)/$(DUMP_FILE) \
		--name $(BIN) \
		--type Heap
	make dump-analyze

# See analyze commands at:
# https://learn.microsoft.com/en-us/dotnet/framework/tools/sos-dll-sos-debugging-extension
dump-analyze:
	dotnet-dump analyze $(PROFILING_DIR)/$(DUMP_FILE) --command 'dumpheap -stat'

gcdump:
	dotnet-gcdump collect \
		--output $(PROFILING_DIR)/$(GCDUMP_FILE) \
		--name $(BIN) \
		--verbose

trace:
	dotnet build
	dotnet-trace collect \
		--output $(PROFILING_DIR)/$(TRACE_FILE) \
		--profile gc-verbose \
		--format Speedscope \
		-- \
		$(BIN)/bin/Debug/net10.0/$(BIN)
