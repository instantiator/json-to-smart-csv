# json-to-smart-csv

**A simple tool to convert JSON data into CSV records.** This tool uses a set of rules to construct CSV records from a source JSON file. It can append to an existing CSV file or create a fresh file with headers.

**Disclaimer:** JsonToSmartCsv was developed on Mac OS, and I've done no testing on any other systems. Binaries for Windows and Linux ought to work. Please let me know if you encounter issues.

## Version history

| Version | Notes |
|-|-|
| `0.1` | Initial release. Simple rules defined in a CSV file direct the tool to craft CSV from JSON input. |
| `0.2` | Switched to defining the column rules as JSON, allowing for more complex nested rules to match more complex input data. |

## Releases

Release binaries are available for `win-x64`, `osx-x64`, and `linux-x64` systems. See:

* Latest release: [instantiator/json-to-smart-csv/releases/latest](https://github.com/instantiator/json-to-smart-csv/releases/latest)
* All releases: [instantiator/json-to-smart-csv/releases](https://github.com/instantiator/json-to-smart-csv/releases)

Download the zip file from the release, and use the binary for your system.

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
* `WithPropertiesAsColumns` - Not yet implemented, a shortcut to transform an object to columns

## Data types

The root item of a JSON document is either an object or a list.

1. If an object, you can start to apply rules with `$.property` paths.
2. If a list, you'll want to apply an `IterateListItems` rule

For example, you have a list, that looks like this:

```json
[
    { "name": "John" },
    { "name": "Jane" }
]
```

A simple rule set to extract the names from this list:

```jsonc
{
    "root": "$",
    "rules": [
        {
            "path": "$",
            "target": "list", // target not actually used
            "interpretation": "IterateListItems",
            "children":
            [
                {
                    "path": "$.name",
                    "target": "name",
                    "interpretation": "AsString"
                }
            ]
        }
    ]
}
```

## Example

See `test-sample-osx-x64.sh` for an example. This script invokes a published copy of the tool (found at `release/osx-x64/JsonToSmartCsv`).

If you need to create this, run the `publish.sh` script, or download a copy for your system from the [latest release](https://github.com/instantiator/json-to-smart-csv/releases/latest).

It invokes `JsonToSmartCsv` with the following files:

* `sample-data/sample-list.json` - some sample data
* `sample-data/sample-rules.json` - rules to interpret them
* `sample-data/sample-out.csv` - generated CSV output

Take a look at the sample data, and sample rules to see how they interact to generate the output CSV.

## Developer notes

### Prerequisites

* [.NET 7.0 SDK](https://dotnet.microsoft.com/en-us/download) - used to build and run the tool
* [bash](https://www.gnu.org/software/bash/) - shell scripting language

### Build

Either build the solution through Visual Studio, or use the `build-all.sh` script.

### Test

Launch the unit tests with the `run-unit-tests.sh` script, or through Visual Studio.

### Publish

Publish with the `publish.sh` script - this creates binaries at:

* `release/win-x64`
* `release/osx-x64`
* `release/linux-x64`

## TODOs

Additional features I think would be fun and handy:

- [ ] Add support for `WithPropertiesAsColumns` - a handy shortcut for simple objects
- [ ] Add aggregate interpretations (count, min, max, avg)
- [ ] Add nested aggregate interpretations (sum across lists, etc.)
