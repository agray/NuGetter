namespace NuGetter.Core {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NuGet;

    public class Consolidator {
        public List<PackageReference> ConsolidatedPackages { get; set; }

        public List<PackageReference> PackagesToDownload { get; set; }

        public Consolidator() {
            ConsolidatedPackages = new List<PackageReference>();
            PackagesToDownload = new List<PackageReference>();
        }

        public void DetermineRequiredPackages(IEnumerable<string> packagesConfigFilePaths) {
            foreach (var filename in packagesConfigFilePaths) {
                var file = new PackageReferenceFile(filename);
                var packages = file.GetPackageReferences().ToList();

                foreach (var package in packages.Where(package => !ConsolidatedPackages.Contains(package))) {
                    ConsolidatedPackages.Add(package);
                }
            }
        }

        public void DeterminePackagesToDownload(IEnumerable<DirectoryInfo> localPackages) {
            var localPackageNames = (from package in localPackages
                where package.FullName.Contains(@"\")
                select package.FullName.Substring(package.FullName.LastIndexOf(@"\", StringComparison.Ordinal) + 1))
                .ToList();

            foreach (var package in ConsolidatedPackages) {
                var packageNameWeMightAlreadyHave = $"{package.Id}.{package.Version}";
                var found = localPackageNames.Any(localName => localName.Equals(packageNameWeMightAlreadyHave));

                if (!found) {
                    PackagesToDownload.Add(package);
                }
            }
        }
    }
}