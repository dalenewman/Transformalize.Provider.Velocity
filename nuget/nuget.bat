nuget pack Transformalize.Provider.Velocity.nuspec -OutputDirectory "c:\temp\modules"
nuget pack Transformalize.Provider.Velocity.Autofac.nuspec -OutputDirectory "c:\temp\modules"
nuget pack Transformalize.Transform.Velocity.nuspec -OutputDirectory "c:\temp\modules"
nuget pack Transformalize.Transform.Velocity.Autofac.nuspec -OutputDirectory "c:\temp\modules"

REM nuget push "c:\temp\modules\Transformalize.Provider.Velocity.0.6.20-beta.nupkg" -source https://api.nuget.org/v3/index.json
REM nuget push "c:\temp\modules\Transformalize.Provider.Velocity.Autofac.0.6.20-beta.nupkg" -source https://api.nuget.org/v3/index.json
REM nuget push "c:\temp\modules\Transformalize.Transform.Velocity.0.6.20-beta.nupkg" -source https://api.nuget.org/v3/index.json
REM nuget push "c:\temp\modules\Transformalize.Transform.Velocity.Autofac.0.6.20-beta.nupkg" -source https://api.nuget.org/v3/index.json