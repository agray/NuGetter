namespace NuGetter.Core {
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class NuGetter {
        public static Searcher Searcher = new Searcher();
        public static Consolidator Consolidator = new Consolidator();
        public static Downloader Downloader;
        public static DirectoryInfo MirrorDirectory;
        public static MyCredentialProvider MyCredentialsProvider;

        public NuGetter(string mirrorLocation, string username, string password) {
            Searcher = new Searcher();
            Consolidator = new Consolidator();
            MirrorDirectory = new DirectoryInfo(mirrorLocation);
            Downloader = new Downloader(MirrorDirectory, username, password);
        }

        public NuGetter(string mirrorLocation) {
            Searcher = new Searcher();
            Consolidator = new Consolidator();
            MirrorDirectory = new DirectoryInfo(mirrorLocation);
            Downloader = new Downloader(MirrorDirectory);
        }

        public IEnumerable<string> Search() {
            return Searcher.Search();
        }

        public void DetermineRequiredPackages(IEnumerable<string> packageConfigFilePaths) {
            Consolidator.DetermineRequiredPackages(packageConfigFilePaths);
        }

        public void DeterminePackagesToDownload() {
            Consolidator.DeterminePackagesToDownload(MirrorDirectory.GetDirectories());
        }

        public void Mirror() {
            Downloader.Download(Consolidator.PackagesToDownload);
            Console.WriteLine("All NuGet packages successfully mirrored.");
        }
    }
}