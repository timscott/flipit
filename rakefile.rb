require 'rake'
require 'albacore'
require 'fileutils'

PRODUCT_NAME = "FlipIt"
BASE_PATH = File.expand_path ""
SOURCE_PATH = "#{BASE_PATH}/src/"
OUTPUT_PATH = "#{BASE_PATH}/output/"
TESTS_PATH = "#{SOURCE_PATH}/#{PRODUCT_NAME}.Tests/bin/Release/"
TESTS_OUTPUT_PATH = "#{OUTPUT_PATH}/tests"
PACKAGE_OUTPUT_PATH = "#{OUTPUT_PATH}/package/"
PACKAGE_OUTPUT_LIB_PATH = "#{PACKAGE_OUTPUT_PATH}/lib/"
TEST_REPORT_PATH =  "#{OUTPUT_PATH}/report"
COMMON_ASSEMBY_INFO_FILE = "#{SOURCE_PATH}/CommonAssemblyInfo.cs"
MSPEC_VERSION = "0.5.3.0"
VERSION = "0.1.0.0"
PACKAGE_PATH = "#{BASE_PATH}/package"
NUSPEC_FILENAME = "#{PRODUCT_NAME}.nuspec"
NUSPEC_PATH = "#{PACKAGE_PATH}/#{NUSPEC_FILENAME}"

task :default => [:standard]
task :standard => [:assemblyInfo,:build,:output,:test,:define_package]
task :package => [:standard,:create_package]

msbuild :build do |msb|
	msb.properties :configuration => :Release
	msb.targets :Clean, :Build
	msb.verbosity = "minimal"
	msb.solution = "#{SOURCE_PATH}/#{PRODUCT_NAME}.sln"
end

task :output do
	FileUtils.rmtree OUTPUT_PATH
	FileUtils.mkdir OUTPUT_PATH
	FileUtils.mkdir TEST_REPORT_PATH
	FileUtils.mkdir TESTS_OUTPUT_PATH
	FileUtils.mkdir PACKAGE_OUTPUT_PATH
	FileUtils.mkdir PACKAGE_OUTPUT_LIB_PATH
	FileUtils.cp_r "#{TESTS_PATH}.", TESTS_OUTPUT_PATH
	copy_files TESTS_PATH, PACKAGE_OUTPUT_LIB_PATH, PRODUCT_NAME, ["dll", "pdb", "xml"]
end

mspec :test do |mspec|
	mspec.command = "lib/Machine.Specifications.#{MSPEC_VERSION}/tools/mspec.exe"
	mspec.assemblies "#{TESTS_OUTPUT_PATH}/#{PRODUCT_NAME}.Tests.dll"
	mspec.html_output = TEST_REPORT_PATH
end

desc "Assembly Version Info Generator"
assemblyinfo :assemblyInfo do |asm|
	FileUtils.rm COMMON_ASSEMBY_INFO_FILE, :force => true 
	asm.output_file = COMMON_ASSEMBY_INFO_FILE
	asm.title = PRODUCT_NAME
	asm.company_name = "Lunaverse Software"
	asm.product_name = PRODUCT_NAME
	asm.version = VERSION
	asm.file_version = VERSION
	asm.copyright = "Copyright (c) 2011 Lunaverse Software"
end

desc "Create the FlipIt nuspec file"
nuspec :define_package do |nuspec|
	version = "#{ENV['version']}"
	nuspec.id = "FlipIt"
	nuspec.version = version.length == 7 ? version : VERSION
	nuspec.authors = "Tim Scott"
	nuspec.owners = "Tim Scott"
	nuspec.description = "A feature flipper. FlipIt provides a simple and flexible way to flip features in a .NET application."
	nuspec.title = "FlipIt"
	nuspec.language = "en-US"
	nuspec.licenseUrl = "https://github.com/timscott/flipit/blob/master/LICENSE.TXT"
	nuspec.projectUrl = "https://github.com/timscott/flipit/wiki"
	nuspec.tags = "FeatureFlipper Deployment Settings Configuration"
	nuspec.working_directory = PACKAGE_PATH
	nuspec.output_file = NUSPEC_FILENAME
end

desc "Create the nuget package"
task :create_package do
    cmd = Exec.new
    cmd.command = 'tools/nuget.exe'
    cmd.parameters = "pack #{NUSPEC_PATH} -basepath #{PACKAGE_OUTPUT_PATH} -outputdirectory #{PACKAGE_PATH}"
    cmd.execute
end

def copy_files(from, to, filename, extensions)
  extensions.each do |ext|
	from_name = "#{from}#{filename}.#{ext}"
	to_name = "#{to}#{filename}.#{ext}"
	if File.exists? from_name
		puts "Copying '#{from_name}' to '#{to_name}'"
		FileUtils.cp from_name, to_name
	end
  end
end