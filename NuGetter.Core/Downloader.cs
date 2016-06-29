using System.Linq;

namespace NuGetter.Core {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NuGet;

    public class Downloader {
        private const string NuGetApiUrl = @"http://packages.nuget.org/api/v2";
        private readonly DirectoryInfo _mirror;
        private readonly MyCredentialProvider _credentials;

        public Downloader(DirectoryInfo mirror, string username, string password) {
            _mirror = mirror;
            _credentials = new MyCredentialProvider(username, password);
        }

        public Downloader(DirectoryInfo mirror)
        {
            _mirror = mirror;
        }

        public void Download(List<PackageReference> packagesToDownload) {
            foreach (var package in packagesToDownload) {
                GoGetPackage(package);
            }
            if (packagesToDownload.Any()) {
                Console.WriteLine($"Successfully mirrored {packagesToDownload.Count} packages.");
            }
        }

        private void GoGetPackage(PackageReference package) {
            var repo = PackageRepositoryFactory.Default.CreateRepository(NuGetApiUrl);
            HttpClient.DefaultCredentialProvider = _credentials;
            var packageManager = new PackageManager(repo, _mirror.FullName);
            packageManager.InstallPackage(package.Id, package.Version);
            Console.WriteLine($"Successfully mirrored package {package.Id}.{package.Version}.");
        }
    }
}