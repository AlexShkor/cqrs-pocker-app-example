require 'albacore'

task :default => [:test]

desc "Build"
msbuild :build do |msb|
  msb.properties :configuration => :Release
  msb.targets :Clean, :Build
  msb.solution = "TrumpIt.sln"
end

desc "Test" 
nunit :test => :build do |nunit|
	nunit.command = "packages/NUnit.2.5.10.11092/tools/nunit-console.exe"
	nunit.options '/framework v4.0.30319'
	nunit.assemblies "AKQ.Tests/bin/Release/AKQ.Tests.dll"
end
