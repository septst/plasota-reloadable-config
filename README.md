# Logging and Dynamic Configuratiomn

## Goal

It is desirable to be able to dynamically change the log level
of the application as well as alternate any other configurable
pieces.

### Main goals

1. Compare available configuration injection options
2. Compare the downsides of each approach and decide on
   which one to pick and where
3. Understand how non-json sources can be leveraged
   for namemespace-level logging settings
4. Understand how Azure Key Vault configuration fits
   into this setup

### Side goals:

1. Understand if and how the options are cached

## Results

### Main goals

#### Understand how non-json sources can be leveraged for namemespace-level logging settings

Unfortunately they cannot. There is no conversion
for both Environment Variable configuration
and Azure Key Vault which means that while it's possible
to get the following settings overwritten

```json
{
    "TopLevelValue": "Something",
    "Section": {
        "NestedValue": "Something"
    }
}
```

as:

```dotenv
TopLevelValue=Overwritten
Section__NestedValue=OverwrittenAsWell
```

it's [not possible to handle keys containing a dot](https://github.com/dotnet/AspNetCore.Docs/issues/17378) like:

```json
{
    "LogLevel": {
        //   this dot is not possible
        //   outside of JSON provider
        //        ↓
        "Microsoft.AspnetCore": "Debug",
        //        ↑
        //       :-(
    }
}
```

and the same applies to Azure Key Vault provider. Keys can't contain dots in names and no conversion is in place.