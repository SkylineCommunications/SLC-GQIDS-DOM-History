# SLC-GQIDS-DOM-History

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkylineCommunications_SLC-GQIDS-DOM-History&metric=alert_status&token=179e73f49a728e6224e74f27b56e58895795b2b2)](https://sonarcloud.io/summary/new_code?id=SkylineCommunications_SLC-GQIDS-DOM-History)

Contains a GQI Data Source that fetches the DOM History of a DOM Instance.

> [!WARNING]
> Minimum required DataMiner version 10.3.4.
> To fully use everything of the source it's recommended to have 10.4.0, or higher.

## Extra feature

The DOM History we fetch are also DOM Objects, therefor to have all the dashboard and app features we need to add Metadata to the row indicating that this row is linked to a DOM Instance. This is available from DataMiner 10.4.0 onwards.
If your DataMiner system is 10.4.0 or higher you can uncomment the define preprocessor tag in the main .cs file.

Default:
```csharp
// You can uncomment these directives to support newer features if your DataMiner system supports it.
// See the Github readme for more info on what these features do.
// https://github.com/SkylineCommunications/SLC-GQIDS-DOM-History

////#define DATAMINER_10_4_0
```

For 10.4.0 onwards, uncomment the #define DATAMINER_10_4_0:
```csharp
// You can uncomment these directives to support newer features if your DataMiner system supports it.
// See the Github readme for more info on what these features do.
// https://github.com/SkylineCommunications/SLC-GQIDS-DOM-History

#define DATAMINER_10_4_0
```


## How to use

1. Install the Data Source.
1. Create a new query in you dashboard or app.
1. Select "Get ad hoc data"
1. Select "Get DOM History"
1. Fill in the DOM Module, or link a feed
1. Fill in the DOM Instance ID, or link a feed
1. Drag the query on your dashboard or app.

![How To Use Query](/Documentation/How_To_Use_Query.png)

## Example

![Table example](/Documentation/Result_Table.png)

## About DataMiner

DataMiner is a transformational platform that provides vendor-independent control and monitoring of devices and services. 
Out of the box and by design, it addresses key challenges such as security, complexity, multi-cloud, and much more. 
It has a pronounced open architecture and powerful capabilities enabling users to evolve easily and continuously.

The foundation of DataMiner is its powerful and versatile data acquisition and control layer. 
With DataMiner, there are no restrictions to what data users can access. 
Data sources may reside on premises, in the cloud, or in a hybrid setup.

A unique catalog of 7000+ connectors already exist. 
In addition, you can leverage DataMiner Development Packages to build you own connectors (also known as "protocols" or "drivers").

> **Note**
> See also: [About DataMiner](https://aka.dataminer.services/about-dataminer).

## About Skyline Communications

At Skyline Communications, we deal in world-class solutions that are deployed by leading companies around the globe. 
Check out [our proven track record](https://aka.dataminer.services/about-skyline) and see how we make our customers' lives easier by empowering them to take their operations to the next level.