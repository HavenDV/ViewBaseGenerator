# [ViewBaseGenerator](https://github.com/HavenDV/ViewBaseGenerator/) 

[![Language](https://img.shields.io/badge/language-C%23-blue.svg?style=flat-square)](https://github.com/HavenDV/ViewBaseGenerator/search?l=C%23&o=desc&s=&type=Code) 
[![License](https://img.shields.io/github/license/HavenDV/ViewBaseGenerator.svg?label=License&maxAge=86400)](LICENSE.md) 
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Standard%202.0-blue.svg)](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md)
[![Build Status](https://github.com/HavenDV/ViewBaseGenerator/workflows/.NET/badge.svg?branch=master)](https://github.com/HavenDV/ViewBaseGenerator/actions?query=workflow%3A%22.NET%22)

### Nuget

[![NuGet](https://img.shields.io/nuget/dt/ViewBaseGenerator.svg?style=flat-square&label=ViewBaseGenerator)](https://www.nuget.org/packages/ViewBaseGenerator/)

```
Install-Package ViewBaseGenerator
```

### Usage

```xml
  <ViewBaseGenerator_Namespace>YourNamespace.Views</ViewBaseGenerator_Namespace>

  <ItemGroup Label="ViewBase">
    <AdditionalFiles Include="..\..\shared\YourNamespace.Shared\Views\**\*.xaml.cs" ViewBaseGenerator_BaseClass="ReactiveUI.Uno.ReactiveUserControl" ViewBaseGenerator_ViewModelNamespace="YourNamespace.ViewModels" Visible="False" />
  </ItemGroup>
```

### Contacts
* [mail](mailto:havendv@gmail.com)
