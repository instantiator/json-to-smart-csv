# `JsonToSmartCSV`

**A simple tool to convert JSON data into CSV records.** This tool uses a set of rules to construct CSV records from a source JSON file. It can append to an existing CSV file or create a fresh file with headers.

**Disclaimer:** JsonToSmartCsv was developed on Mac OS, and I've done no testing on any other systems. Binaries for Windows and Linux ought to work. Please let me know if you encounter issues.

## Version history

| Version | Notes |
|-|-|
| `0.1` | Initial release. Simple rules defined in a CSV file direct the tool to craft CSV from JSON input. |
| `0.2` | Switched to defining the column rules as JSON, allowing for more complex nested rules to match more complex input data. |
| `0.3` | Refactor release - builds an intermediary tree before building the final table. Support for aggregation. |
| `0.4` | Support for aggregation, and more complete documentation. |

## Releases

Release binaries are available for `win-x64`, `osx-x64`, and `linux-x64` systems. See:

* Latest release: [instantiator/json-to-smart-csv/releases/latest](https://github.com/instantiator/json-to-smart-csv/releases/latest)
* All releases: [instantiator/json-to-smart-csv/releases](https://github.com/instantiator/json-to-smart-csv/releases)

Download the zip file from the release, and use the binary for your system.

## Usage

```text
JsonToSmartCsv -c <column-csv-file> -s <source-json-file> -t <target-csv-file> [-m <mode>]
```

### Options

```text
  -c, --columns    Required. Column definitions CSV file.
  -s, --source     Required. Source data JSON file.
  -t, --target     Required. Target CSV file.
  -m, --mode       (Default: Create) Write mode (Append or Create)
  --help           Display this help screen.
  --version        Display version information.
```

### Modes

1. `Create` = create a new target file, backup any existing file
2. `Append` = append to the target file (if it exists)

### Column configuration

Provide column configuration as a JSON file:

```jsonc
{
    "root": "<string>", // topmost object to process, default: "$"
    "rules": // array of rules defining columns
    [
        {
            "path": "<string>", // relative path to the field in the current object
            "target": "<string>", // name of the column in the target CSV file
            "interpretation": "<string>", // how to interpret the value of the field (see below)
            "children": [] // optional array of rules to apply to nested objects and lists
        }
    ]
}
```

#### Interpretations

* `AsString` - interpret this value as a string
* `AsNumber` - interpret this value as an integer or decimal number
* `AsJson` - convert this object or list to a JSON string
* `IterateListItems` - apply child rules to the items in this list
* `IteratePropertiesAsList` - apply child rules to the object properties, as if a list
* `AsIndex` - item's index (IterateListItems), or property (IteratePropertiesAsList)
* `AsAggregateSum` - aggregate and sum all numeric values from child rules
* `AsAggregateMax` - aggregate and find the max of numeric values from child rules
* `AsAggregateMin` - aggregate and find the min of numeric values from child rules
* `AsAggregateAvg` - aggregate and find the mean of numeric values from child rules
* `AsAggregateCount` - count all (non-null) values from by child rules

## Data types

The root item of a JSON document is either an object or a list.

1. If an object, you can start to apply rules with `$.property` paths
2. If it's a list indexed by properties, you might use `IteratePropertiesAsList`
3. If it's a regular list, you'll want to apply an `IterateListItems` rule

## Examples

- [JSON list](example-json-list.md)
- [JSON object](example-json-object.md)
- [JSON properties](example-json-props.md)
- [Aggregation](example-aggregation.md)

### A fully worked example

See `test-sample-osx-x64.sh` for a working sample. This script invokes `JsonToSmartCsv` with the following files:

* `sample-data/sample-list.json` - some sample data
* `sample-data/sample-rules.json` - rules to interpret them
* `sample-data/sample-out.csv` - generated CSV output

Take a look at the sample data, and sample rules to see how they interact to generate the output CSV.

_Check which binary the script uses. You may need to download a copy of the release binary, or create your own with: `publish.sh`_

## Developers

See: [Developer notes](dev-notes.md)