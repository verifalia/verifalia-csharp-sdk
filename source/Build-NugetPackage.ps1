# Directory setup

del -Recurse -Force Nuget\lib
md -Force Nuget\lib

# Copy the net45 artifacts

cp .\Verifalia.Api\bin\Release\net45 .\Nuget\lib -Recurse -Container -Filter Verifalia.Api.* 

# Build the nuget package

nuget pack Nuget\Verifalia.nuspec -BasePath Nuget -OutputDirector Nuget