# Config Comparer

This tool can compare combined JSON configuration files (for example: `appsettings.json` and `appsettings.Production.json`).

## Install

```
dotnet tool install config-comparer -g
```

## Usage

```
USAGE:
Compare two folders:
  dotnet tool run config-comparer c:\project-a c:\project-b appsettings.json appsettings.Production.json
Compare two folders. Show only differences:
  dotnet tool run config-comparer --skip-same c:\project-a c:\project-b appsettings.json appsettings.Production.json
Compare two folders. Show only differences and hide real values:
  dotnet tool run config-comparer --no-values --skip-same c:\project-a c:\project-b appsettings.json appsettings.Production.json

  -s, --skip-same    If set, only non-equal configuration is printed.

  -n, --no-values    If set, values are not shown.

  --help             Display this help screen.

  --version          Display version information.

  value pos. 0       Required. Path to the left compare directory with JSON config files.

  value pos. 1       Required. Path to the right compare directory with JSON config files.

  value pos. 2       List of config files to read (for example: appsettings.json appsettings.Production.json)

```