#!/bin/bash

set -e
set -o pipefail

# defaults
MODE=Create
ROOT="$"

usage() {
  cat << EOF
Usage: json-to-smart-csv.sh [options]

Converts an input JSON file to CSV, using smart rules to select values for columns.

Options:
  -c <file>       --columns <file>    Rules CSV file     (required)
  -s <file>       --source <file>     Input JSON file    (required)
  -t <file>       --target <file>     Target CSV file    (required)
  -m <mode>       --mode <mode>       Mode               (default: $MODE)
  -r <path>       --root <path>       Root path          (default: $ROOT)

Path:
  Provide a JSON path to the root note to process. The default is: $ROOT
  If this points to a single object, {}, the object will be processed to a single row.
  If this points to an array, [], each object in the array will be processed to a row.

Modes:
  Create = create a new target file, backup any existing file
  Append = append to the target file (if it exists)

Provide the following columns in your rules CSV file:
  TargetColumn         = the name of the column in the target CSV file (string, eg. "id")
  SourcePath           = a relative path to a field in the current JSON object (string, eg. "$.id")
  SourceInterpretation = how to interpret the value of the field (see below)
  InterpretationArg1   = supplementary information about how to interpret the field (optional)
  InterpretationArg2   = supplementary information about how to interpret the field (optional)

Source interpretations:
  AsString             = as a standard string
  AsDecimal            = as a decimal number
  AsInteger            = as an integer
  AsBoolean            = as a boolean (true/false)
  AsJson               = as a JSON string, representing the object or list (array)
  AsCount              = as the number of items in the list (array) or 1 (object) or 0 (not present)
  AsConcatenation      = as a concatenation of strings found inside the element (array) or a single string (object)
    InterpretationArg1 = a a relative path inside each element to retrieve values
    InterpretationArg2 = the separator to use between values
  AsAggregation        = as an aggregate of sub-elements inside this object or array
    InterpretationArg1 = a a relative path inside each element to retrieve values
    InterpretationArg2 = the aggregation (Sum, Avg, Min, Max, Count)

EOF
}

# parameters
while [ -n "$1" ]; do
  case $1 in
  -r | --root)
    shift
    ROOT=$1
    ;;
  -c | --columns)
    shift
    COLS_FILE_CSV=$1
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

if [[ -z "$COLS_FILE_CSV" ]]; then
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

echo "Columns file (csv): $COLS_FILE_CSV"
echo "Source data (json): $SOURCE_FILE_JSON"
echo "Source root path:   $ROOT"
echo "Target file (csv):  $TARGET_FILE_CSV"
echo "Mode:               $MODE"

dotnet run --project JsonToSmartCsv/JsonToSmartCsv.csproj -- $COLS_FILE_CSV $SOURCE_FILE_JSON $ROOT $TARGET_FILE_CSV $MODE
