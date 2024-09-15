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

Additional features:

- [x] Refactor to build an intermediary tree before converting to a data table
- [x] Add aggregate interpretations (count, min, max, avg)
- [x] Add nested aggregate interpretations (sum across lists, etc.)
- [x] A full set of unit tests for building the intermediary tree, and preparing data tables
- [x] More complete documentation with examples
- [ ] Add support for `WithPropertiesAsColumns` - a handy shortcut for simple objects
