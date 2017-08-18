try {
	$basePath = Split-Path $script:MyInvocation.MyCommand.Path
	pushd $basePath

	# Cleanup

	msbuild /t:clean /verbosity:minimal
    if ($lastExitCode -ne 0) { exit $lastExitCode }

	del -Recurse -Force Nuget\lib
	md -Force Nuget\lib

    msbuild /t:restore /p:Configuration=Release /verbosity:minimal
    if ($lastExitCode -ne 0) { exit $lastExitCode }
	
	# Build

    msbuild /p:Configuration=Release /verbosity:minimal
    if ($lastExitCode -ne 0) { exit $lastExitCode }

	# Copy the package artifacts

	cp .\Verifalia.Api\bin\Release\net45 .\Nuget\lib -Recurse -Container -Filter Verifalia.Api.* 
	cp .\Verifalia.Api\bin\Release\netstandard1.3 .\Nuget\lib -Recurse -Container -Filter Verifalia.Api.* 

	# Build the nuget package

	msbuild /t:pack /verbosity:minimal /p:NuspecFile=$(Resolve-Path Nuget\Verifalia.nuspec) /p:NuspecBasePath=$(Resolve-Path Nuget)
    if ($lastExitCode -ne 0) { exit $lastExitCode }
}
catch {
    Write-Error $_.Exception
	exit -1
}
finally {
	popd
}