# json-to-smart-csv

```text
Converts an input JSON file to CSV, using smart rules to select values for columns.

Options:
  -r <file>       --rules <file>      Rules CSV file
  -s <file>       --source <file>     Input JSON file
  -t <file>       --target <file>     Target CSV file
  -m <mode>       --mode <mode>       Mode (default: Create)

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
```

See `test-sample.sh` for an example.


