using Microsoft.NET.TestFramework;
using Microsoft.NET.TestFramework.ProjectConstruction;
using System;
using System.IO;

namespace Microsoft.NET.Build.Tests
{
    public class ConstantStringValues
    {
        public static string TestDirectoriesNamePrefix = "Nuget_reference_compat";
        public static string ReferencerDirectoryName = "Reference";
        public static string NuGetSharedDirectoryNamePostfix = "_NuGetDependencies";
        public static string NetstandardToken = "netstandard";
        public static string DependencyDirectoryNamePrefix = "D_";
        public static string NuGetPackageBaseDirectory = Path.Combine(
            RepoInfo.GetBaseDirectory(),
            TestDirectoriesNamePrefix + NuGetSharedDirectoryNamePostfix);

        public static string ConstructNuGetPackageReferencePath(TestProject dependencyProject)
        {
            return Path.Combine(NuGetPackageBaseDirectory, dependencyProject.Name, dependencyProject.Name, "bin", "Debug");
        }
    }

    public class DeleteNuGetArtifactsFixture : IDisposable
    {
        public DeleteNuGetArtifactsFixture()
        {
            DeleteNuGetArtifacts();
        }

        public void Dispose()
        {
            DeleteNuGetArtifacts();
        }

        private void DeleteNuGetArtifacts()
        {
            try
            {
                //  Delete the shared NuGet package directory before running all the tests.
                if (Directory.Exists(ConstantStringValues.NuGetPackageBaseDirectory))
                {
                    Directory.Delete(ConstantStringValues.NuGetPackageBaseDirectory, true);
                }
                //  Delete the generated NuGet packages in the cache.
                foreach (string dir in Directory.EnumerateDirectories(RepoInfo.NuGetCachePath, ConstantStringValues.DependencyDirectoryNamePrefix + "*"))
                {
                    Directory.Delete(dir, true);
                }
            }
            catch
            {
                // No-Op; as this is a precaution - do not throw an exception.
            }
        }
    }
}