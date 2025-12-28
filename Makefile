BIN=Program

nothing:

setup_profiling:
	mkdir -p .profiling
	dotnet tool install --global dotnet-dump
	dotnet tool install --global dotnet-gcdump
	dotnet tool install --global dotnet-trace


# See analyze commands at:
# https://learn.microsoft.com/en-us/dotnet/framework/tools/sos-dll-sos-debugging-extension
dump:
	dotnet-dump collect \
		--output .profiling/dump.dmp \
		--name $(BIN) \
		--type Heap
	dotnet-dump analyze .profiling/dump.dmp --command 'dumpheap -stat'

gcdump:
	dotnet-gcdump collect \
		--output .profiling/gcdump.gcdump \
		--name $(BIN) \
		--verbose

trace:
	dotnet build
	dotnet-trace collect \
		--output .profiling/trace.nettrace \
		--profile gc-verbose \
		--format speedscope \
		-- \
		$(BIN)/bin/Debug/net10.0/$(BIN)
