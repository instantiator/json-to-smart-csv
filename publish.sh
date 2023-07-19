#!/bin/bash

for OS in win linux osx
do
    mkdir -p "release/${OS}-x64"
    dotnet publish JsonToSmartCsv/JsonToSmartCsv.csproj \
        -c Release \
        --os $OS \
        /p:PublishSingleFile=true \
        /p:CopyOutputSymbolsToPublishDirectory=false \
        --self-contained false \
        --output "release/${OS}-x64"
    chmod +x release/${OS}-x64/*
done
