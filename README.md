# json-to-smart-csv

```text
Usage: json-to-smart-csv.sh [options]

Converts an input JSON file to CSV, using some (fairly) smart rules to select values for columns.

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
```

See `test-sample.sh` for an example.
