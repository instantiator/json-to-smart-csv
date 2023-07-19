# json-to-smart-csv

**A simple tool to convert JSON data into CSV records.** Uses a set of rules defining your CSV columns to construct CSV records from a source JSON file. Can append to an existing file or create a fresh file with headers.

## Prerequisites

* [.NET 7.0 SDK](https://dotnet.microsoft.com/en-us/download) - used to build and run the tool
* [bash](https://www.gnu.org/software/bash/) - shell scripting language

**Disclaimer:** This tool was developed on Mac OS, and I've done no testing on any other systems.

## Releases

Releases are available for `win-x64`, `osx-x64`, and `linux-x64`. See:

* [instantiator/json-to-smart-csv/releases](https://github.com/instantiator/json-to-smart-csv/releases)

## Usage

```text
JsonToSmartCsv -c <column-csv-file> -s <source-json-file> -t <target-csv-file> [-m <mode>] [-r <root>]
```

### Options

```text
  -c, --columns    Required. Column definitions CSV file.
  -s, --source     Required. Source data JSON file.
  -t, --target     Required. Target CSV file.
  -m, --mode       (Default: Create) Write mode (Append or Create)
  -r, --root       (Default: $) Root path
  --help           Display this help screen.
  --version        Display version information.
```

```
Root path:
  Provide a JSON path to the root note to process. The default is: $
  If this points to a single object, {}, the object will be processed to a single row.
  If this points to an array, [], each object in the array will be processed to a row.
```

```
Modes:
  Create = create a new target file, backup any existing file
  Append = append to the target file (if it exists)
```

### Column definitions

```
Provide the following columns in your column definitions CSV file:
  TargetColumn         = the name of the column in the target CSV file (string, eg. "id")
  SourcePath           = a relative path to a field in the current JSON object (string, eg. "$.id")
  SourceInterpretation = how to interpret the value of the field (see below)
  InterpretationArg1   = supplementary information about how to interpret the field (optional)
  InterpretationArg2   = supplementary information about how to interpret the field (optional)
```

```
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

## Example

See `test-sample-osx-x64.sh` for an example.
