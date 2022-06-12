# ViewBaseGenerator
The main purpose of the generator is to avoid boilerplate code in the code-behind views files, like this:
```cs
public abstract partial class PreviewDropViewBase
: ReactiveUI.Uno.ReactiveUserControl<Ratbuddyssey.ViewModels.PreviewDropViewModel>
{
}

public partial class MainView
{
    public MainView()
    {
        InitializeComponent();

        _ = this.WhenActivated(disposables =>
        {
            DataContext = ViewModel;
        });
    }
}
```

At the same time, the generator supports the ability to add your code anywhere through the definition of partial methods for special cases:
```cs
public partial class MainView
{
    partial void BeforeInitializeComponent();
    partial void AfterInitializeComponent();

    partial void AfterWhenActivated(CompositeDisposable disposables);

    public MainView()
    {
        BeforeInitializeComponent();

        InitializeComponent();

        AfterInitializeComponent();

        _ = this.WhenActivated(disposables =>
        {
            DataContext = ViewModel;

            if (ViewModel == null)
            {
                return;
            }

            AfterWhenActivated(disposables);
        });
    }
}
```

I also recommend not deleting .xaml.cs files, but leaving them empty like this:
```cs
namespace YourNamespace.Views;

public partial class MainView
{
}
```

## Nuget

[![NuGet](https://img.shields.io/nuget/dt/ViewBaseGenerator.svg?style=flat-square&label=ViewBaseGenerator)](https://www.nuget.org/packages/ViewBaseGenerator/)

```
Install-Package ViewBaseGenerator
```

### WPF/UWP/WinUI
```xml
  <PropertyGroup>
    <ViewBaseGenerator_Namespace>YourNamespace.Views</ViewBaseGenerator_Namespace>
  </PropertyGroup

  <ItemGroup Label="View Constructors">
    <AdditionalFiles Include="Views\**\*.xaml" ViewBaseGenerator_GenerateConstructor="True" ViewBaseGenerator_SetReactiveUIDataContext="True" />
  </ItemGroup>

  <ItemGroup Label="ViewBase">
    <AdditionalFiles Include="Views\**\*.xaml.cs" ViewBaseGenerator_BaseClass="ReactiveUI.Uno.ReactiveUserControl" ViewBaseGenerator_ViewModelNamespace="YourNamespace.ViewModels" />
  </ItemGroup>
```

Although ReactiveUI is supported, you can also use the generator without it, 
just to get rid of the `InitializeComponent()` constructors. In this case, you need code like:
```xml
  <PropertyGroup>
    <ViewBaseGenerator_Namespace>YourNamespace.Views</ViewBaseGenerator_Namespace>
  </PropertyGroup

  <ItemGroup Label="View Constructors">
    <AdditionalFiles Include="Views\**\*.xaml" ViewBaseGenerator_GenerateConstructor="True" />
  </ItemGroup>
```

### Uno (projects besides UWP/WinUI)
Uno uses Source Generators and there is currently no way to use the output of one generator in another. 
Therefore, the solution is somewhat more complicated:
1. Create new project like this:
```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFrameworks>netstandard2.0;net6.0-android;net6.0-ios;net6.0-macos;net6.0-maccatalyst</TargetFrameworks>
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
2. Add this project reference to your apps.

## Contacts
* [mail](mailto:havendv@gmail.com)
