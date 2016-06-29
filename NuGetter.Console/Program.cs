namespace NuGetter.Console {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;

    public class Program {
        private static NuGetter _nuGetter;

        static void Main(string[] args) {
            if (!args.Any()) {
                Console.WriteLine("Please provide location of NuGetMirror at a minimum");
                Environment.Exit(0);
            }

            CreateNuGetter(args);

            var packageConfigFilePaths = _nuGetter.Search();

#if DEBUG
            Console.WriteLine("=======================");
            Console.WriteLine("====PACKAGE CONFIGS====");
            Console.WriteLine("=======================");
            foreach (var package in packageConfigFilePaths) {
                Console.WriteLine(package);
            }

            Console.WriteLine("=======================");
#endif
            _nuGetter.DetermineRequiredPackages(packageConfigFilePaths);
            _nuGetter.DeterminePackagesToDownload();
            _nuGetter.Mirror();
        }

        private static void CreateNuGetter(IReadOnlyList<string> args) {
            switch (args.Count) {
                case 1:
                    _nuGetter = new NuGetter(args[0]);
                    break;
                case 3:
                    _nuGetter = new NuGetter(args[0], args[1], args[2]);
                    break;
            }
        }
    }
}