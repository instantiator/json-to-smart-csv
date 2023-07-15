#!/bin/bash

set -e
set -o pipefail

# defaults
MODE=Create

usage() {
  cat << EOF
Converts an input JSON file to CSV, using smart rules to select values for columns.

Options:
  -r <file>       --rules <file>      Rules CSV file
  -s <file>       --source <file>     Input JSON file
  -t <file>       --target <file>     Target CSV file
  -m <mode>       --mode <mode>       Mode (default: $MODE)

Modes:
  Create = create a new target file, backup any existing file
  Append = append to the target file (if it exists)

Provide the following columns in your rules CSV file:
  TargetColumn         = the name of the column in the target CSV file (string, eg. "id")
  SourcePath           = a path to the field you want to use from your JSON object (string, eg. "$.id")
  SourceInterpretation = how to interpret the value of the field (see below)
  InterpretationRule   = supplementary information about how to interpret the field (optional)

Source interpretations:
  AsString             = as a standard string
  AsDecimal            = as a decimal number
  AsInteger            = as an integer
  AsBoolean            = as a boolean (true/false)
  AsObjectAsJson       = as a JSON string, representing the object (dictionary)
  AsListAsJson         = as a JSON string, representing the list (array)
  AsListCount          = as the number of items in the list (array)
EOF
}

# parameters
while [ -n "$1" ]; do
  case $1 in
  -r | --rules)
    shift
    RULES_FILE_CSV=$1
    ;;
  -s | --source)
    shift
    SOURCE_FILE_JSON=$1
    ;;
  -t | --target)
    shift
    TARGET_FILE_CSV=$1
    ;;
  -m | --mode)
    shift
    MODE=$1
    ;;
  --help)
    usage
    exit 0
    ;;
  *)
    echo -e "Unknown option $1...\n"
    usage
    exit 1
    ;;
  esac
  shift
done

if [[ -z "$RULES_FILE_CSV" ]]; then
  echo "Please provide a rules file (csv)."
  usage
  exit 1
fi

if [[ -z "$SOURCE_FILE_JSON" ]]; then
  echo "Please provide a source file (json)."
  usage
  exit 1
fi

if [[ -z "$TARGET_FILE_CSV" ]]; then
  echo "Please indicate the target filename (csv)."
  usage
  exit 1
fi

dotnet run --project JsonToSmartCsv/JsonToSmartCsv.csproj -- $RULES_FILE_CSV $SOURCE_FILE_JSON $TARGET_FILE_CSV $MODE
