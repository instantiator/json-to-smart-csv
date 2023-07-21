#!/bin/bash

./release/osx-x64/JsonToSmartCsv   \
  -c sample-data/sample-rules.json \
  -s sample-data/sample-list.json  \
  -t sample-data/sample-out.csv
