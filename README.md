### Overview

This adds a `velocity` provider and transform to Transformalize using [NVelocity](https://www.nuget.org/packages/NVelocity).  

Build the Autofac project and put it's output into Transformalize's *plugins* folder.

### Write Usage

```xml
<add name='TestProcess' mode='init'>
  <connections>
    <add name='input' provider='bogus' seed='1' />
    <add name='output' provider='Velocity' template='template.vtl' file='output.html' />
  </connections>
  <entities>
    <add name='Contact' size='1000'>
      <fields>
        <add name='Identity' type='int' primary-key='true' />
        <add name='FirstName' />
        <add name='LastName' />
        <add name='Stars' type='byte' min='1' max='5' />
        <add name='Reviewers' type='int' min='0' max='500' />
      </fields>
    </add>
  </entities>
</add>
```

This writes 1000 rows of bogus data to *output.html*.

The template *template.vtl* is passed a [VelocityModel](https://github.com/dalenewman/Transformalize.Provider.Velocity/blob/master/src/Transformalize.Provider.Velocity/VelocityModel.cs).  The template in this example looks like this:

```html
<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title>Velocity Output</title>
</head>
<body>
    <table>
        <thead>
            <tr>
                #foreach($field in $Model.Entity.Fields)
                    #if(!$field.System)<th>$field.Label</th> #end
                #end
            </tr>
        </thead>
        <tbody>
            #foreach($row in $Model.Rows)
                <tr>
                    #foreach($field in $Model.Entity.Fields)
                        #if(!$field.System)<td>$row.get_Item($field)</td> #end
                    #end
                </tr>
                #set( $Model.Entity.Inserts = $Model.Entity.Inserts + 1 )
            #end
        </tbody>
    </table>
</body>
</html>
```

The table in *output.html* looks like this (clipped for brevity):

<table>
        <thead>
            <tr>
                    <th>Identity</th>
                    <th>FirstName</th>
                    <th>LastName</th>
                    <th>Stars</th>
                    <th>Reviewers</th>
            </tr>
        </thead>
        <tbody>
                <tr>
                        <td>1</td>
                        <td>Justin</td>
                        <td>Konopelski</td>
                        <td>3</td>
                        <td>153</td>
                </tr>
                <tr>
                        <td>2</td>
                        <td>Eula</td>
                        <td>Schinner</td>
                        <td>2</td>
                        <td>41</td>
                </tr>
                <tr>
                        <td>3</td>
                        <td>Tanya</td>
                        <td>Shanahan</td>
                        <td>4</td>
                        <td>412</td>
                </tr>
                <tr>
                        <td>4</td>
                        <td>Emilio</td>
                        <td>Hand</td>
                        <td>4</td>
                        <td>469</td>
                </tr>
                <tr>
                        <td>5</td>
                        <td>Rachel</td>
                        <td>Abshire</td>
                        <td>3</td>
                        <td>341</td>
                </tr>
    </tbody>
</table>

### Benchmark

*Note: Numbers get better with more records.*

``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.17134.407 (1803/April2018Update/Redstone4)
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=2742192 Hz, Resolution=364.6718 ns, Timer=TSC
  [Host]       : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 32bit LegacyJIT-v4.7.3221.0
  LegacyJitX64 : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit LegacyJIT/clrjit-v4.7.3221.0;compatjit-v4.7.3221.0

Job=LegacyJitX64  Jit=LegacyJit  Platform=X64  
Runtime=Clr  

```
|                       Method |       Mean |    Error |   StdDev |     Median | Ratio | RatioSD |
|----------------------------- |-----------:|---------:|---------:|-----------:|------:|--------:|
|            &#39;10000 test rows&#39; |   987.5 ms | 22.37 ms | 44.68 ms |   968.8 ms |  1.00 |    0.00 |
| &#39;10000 rows with 1 Velocity&#39; | 1,726.7 ms | 27.51 ms | 25.74 ms | 1,722.5 ms |  1.75 |    0.07 |
