namespace NuGetter.Core {
    using System.Collections.Generic;
    using System.IO;

    public class Searcher {
        private readonly List<string> _exclusions;
        private readonly List<string> _packageConfigFilePaths;

        public Searcher() {
            _exclusions = new List<string> {
                ".nuget",
                ".git",
                ".vs",
                "$tf",
                "packages",
                "NuGetter",
                "JenkinsScripts",
                "TFS-SDK-11.0.0"
            };

            _packageConfigFilePaths = new List<string>();
        }

        public IEnumerable<string> Search() {
            var rootDir = GetSolutionRoot(Directory.GetCurrentDirectory());
            ProcessDirectory(rootDir);
            return _packageConfigFilePaths;
        }

        private static string GetSolutionRoot(string startDirectory) {
            if (startDirectory == null) {
                return "";
            }

            var info = new DirectoryInfo(startDirectory);

            while (info.GetFiles("*.sln").Length == 0) {
                info = info.Parent;
            }

            return info.FullName;
        }

        private void ProcessDirectory(string targetDirectory) {
            // Process the list of files found in the directory.
            var fileEntries = Directory.GetFiles(targetDirectory, "packages.config");
            foreach (var fileName in fileEntries) {
                ProcessFile(fileName);
            }

            // Recurse into subdirectories of this directory.
            var subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            var cleanedSubdirectoryEntries = RemoveExcludedDirectories(subdirectoryEntries);

            foreach (var subdirectory in cleanedSubdirectoryEntries) {
                ProcessDirectory(subdirectory);
            }
        }

        // Insert logic for processing found files here.
        private void ProcessFile(string path) {
            _packageConfigFilePaths.Add(path);
        }

        private IEnumerable<string> RemoveExcludedDirectories(IEnumerable<string> subdirectoryEntries) {
            var cleaned = new List<string>();

            foreach (var subdirectory in subdirectoryEntries) {
                var found = false;
                foreach (var exclusion in _exclusions) {
                    if (subdirectory.Contains(exclusion)) {
                        found = true;
                    }
                }

                if (!found && !cleaned.Contains(subdirectory)) {
                    cleaned.Add(subdirectory);
                }
            }

            return cleaned;
        }
    }
}