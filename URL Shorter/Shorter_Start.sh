#!/bin/bash
cd URL_Shorter/URL_Shorter
dotnet build
cd bin/Debug/netcoreapp2.1
dotnet URL_Shorter.dll