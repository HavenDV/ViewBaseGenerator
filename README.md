# [ViewBaseGenerator](https://github.com/HavenDV/ViewBaseGenerator/) 

[![Language](https://img.shields.io/badge/language-C%23-blue.svg?style=flat-square)](https://github.com/HavenDV/ViewBaseGenerator/search?l=C%23&o=desc&s=&type=Code) 
[![License](https://img.shields.io/github/license/HavenDV/ViewBaseGenerator.svg?label=License&maxAge=86400)](LICENSE.md) 
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Standard%202.0-blue.svg)](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md)
[![Build Status](https://github.com/HavenDV/ViewBaseGenerator/workflows/.NET/badge.svg?branch=master)](https://github.com/HavenDV/ViewBaseGenerator/actions?query=workflow%3A%22.NET%22)

## Nuget

[![NuGet](https://img.shields.io/nuget/dt/ViewBaseGenerator.svg?style=flat-square&label=ViewBaseGenerator)](https://www.nuget.org/packages/ViewBaseGenerator/)

```
Install-Package ViewBaseGenerator
```

### UWP Only

```xml
  <PropertyGroup>
    <ViewBaseGenerator_Namespace>YourNamespace.Views</ViewBaseGenerator_Namespace>
  </PropertyGroup>

  <ItemGroup Label="ViewBase">
    <AdditionalFiles Include="..\..\shared\YourNamespace.Shared\Views\**\*.xaml.cs" ViewBaseGenerator_BaseClass="ReactiveUI.Uno.ReactiveUserControl" ViewBaseGenerator_ViewModelNamespace="YourNamespace.ViewModels" Visible="False" />
  </ItemGroup>
```

### Uno
Uno uses Source Generators and there is currently no way to use the output of one generator in another. 
Therefore, the solution is somewhat more complicated:
1. Create `global.json` in repository root directory with this content:
```json
{
  "msbuild-sdks": {
    "MSBuild.Sdk.Extras": "3.0.22"
  }
}
```
2. Create new project like this:
```xml
<Project Sdk="MSBuild.Sdk.Extras">

  <PropertyGroup>
    <TargetFrameworks>uap10.0.19041;netstandard2.0;net6.0-maccatalyst;net6.0-android;net6.0-ios;net6.0-macos</TargetFrameworks>
  </PropertyGroup>

  <PropertyGroup>
    <ViewBaseGenerator_Namespace>YourNamespace.Views</ViewBaseGenerator_Namespace>
  </PropertyGroup>

  <ItemGroup Label="ViewBase">
    <AdditionalFiles Include="..\..\shared\YourNamespace.Shared\Views\**\*.xaml.cs" ViewBaseGenerator_BaseClass="ReactiveUI.Uno.ReactiveUserControl" ViewBaseGenerator_ViewModelNamespace="YourNamespace.ViewModels" Visible="False" />
    <AdditionalFiles Remove="..\..\shared\YourNamespace.Shared\Views\Navigation\MainView.xaml.cs" />
    <AdditionalFiles Include="..\..\shared\YourNamespace.Shared\Views\Navigation\MainView.xaml.cs" ViewBaseGenerator_BaseClass="ReactiveUI.Uno.ReactivePage" ViewBaseGenerator_ViewModelNamespace="YourNamespace.ViewModels" Visible="False" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="ViewBaseGenerator" Version="1.3.8">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>

  <ItemGroup>
    <!-- Your base class library -->
    <PackageReference Include="ReactiveUI.Uno" Version="16.2.6" />
  </ItemGroup>

  <ItemGroup>
    <!-- Your core project that contains ViewModels -->
    <ProjectReference Include="..\YourNamespace.Core\YourNamespace.Core.csproj" />
  </ItemGroup>
	
</Project>
```
3. Add this project reference to your apps.

## Contacts
* [mail](mailto:havendv@gmail.com)
