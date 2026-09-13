# Mile.DotNet.Helpers

[![NuGet Package](https://img.shields.io/nuget/vpre/Mile.DotNet.Helpers)](https://www.nuget.org/packages/Mile.DotNet.Helpers)

The essential helper functions for the .NET platform.

## Available MSBuild project options

If you want to disable all features from Mile.DotNet.Helpers, please set the
following option.

```
<MileDotNetHelpersDisableAllFeatures>true</MileDotNetHelpersDisableAllFeatures>
```

If you want to include Git helpers, please set the following option.

```
<MileDotNetHelpersEnableGitHelpers>true</MileDotNetHelpersEnableGitHelpers>
```

If you want to include ImageAssets helpers, please set the following option.

```
<MileDotNetHelpersEnableImageAssetsHelpers>true</MileDotNetHelpersEnableImageAssetsHelpers>
```

The ImageAssets helpers also require Magick.NET. Using the latest stable version
is recommended to receive the latest security fixes. Add the following package
reference to your project.

```
<ItemGroup Condition="'$(MileDotNetHelpersEnableImageAssetsHelpers)' == 'true'">
  <PackageReference Include="Magick.NET-Q8-AnyCPU" Version="*" />
</ItemGroup>
```

If you want to include Text helpers, please set the following option.

```
<MileDotNetHelpersEnableTextHelpers>true</MileDotNetHelpersEnableTextHelpers>
```

## Documents

- [License](License.md)
- [Release Notes](ReleaseNotes.md)
- [Versioning](Versioning.md)
