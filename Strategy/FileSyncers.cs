/*
   Exercise: implement the "FileSyncer"
   It should behave different when using different deletestrategies
 - NoDelete (ta inte bort något)
 - DeleteIfNotInSource (ta bort de filer som inte finns i source)
 - KeepLogFiles (ta bort de filer som inte finns i source förutom ".log"-filer)
*/
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace DesignPatterns.Strategy
{
    [TestClass]
    public class FileSyncers
    {
        const string SourceDirectory = @"c:\TMP\Sync\A";
        const string DestinationDirectory = @"c:\TMP\Sync\B";

        public void InitDirectories()
        {
            // Setup two directories and fill them with some test files

            Directory.CreateDirectory(SourceDirectory);
            Directory.CreateDirectory(DestinationDirectory);

            ClearDirectory(SourceDirectory);
            ClearDirectory(DestinationDirectory);

            CreateText("1.txt", "1111", SourceDirectory);
            CreateText("2.txt", "2222", SourceDirectory);
            CreateText("3.txt", "3333", SourceDirectory);
            CreateText("6.txt", "6666", DestinationDirectory);
            CreateText("my.log", "Loggy", DestinationDirectory);

        }

        private void CreateText(string filename, string content, string directory)
        {
            File.AppendAllText(Path.Combine(directory, filename), content);
        }

        private void ClearDirectory(string directory)
        {
            foreach (var file in new DirectoryInfo(directory).GetFiles())
                File.Delete(file.FullName);
        }

        [TestMethod]
        public void dont_delete_extra_files_in_destination_folder()
        {
            InitDirectories();

            AssertFiles(new[] { "1.txt", "2.txt", "3.txt" }, SourceDirectory);
            AssertFiles(new[] { "6.txt", "my.log" }, DestinationDirectory);

            var filesyncer = new FileSyncer();
            filesyncer.Sync(
                new DirectoryInfo(SourceDirectory),
                new DirectoryInfo(DestinationDirectory),
                new NoDelete());  // don't delete files in destination directory like 6.txt and my.log

            AssertFiles(new[] { "1.txt", "2.txt", "3.txt" }, SourceDirectory);
            AssertFiles(new[] { "1.txt", "2.txt", "3.txt", "6.txt", "my.log" }, DestinationDirectory);

        }

        [TestMethod]
        public void delete_if_not_in_source_strategy()
        {
            InitDirectories();

            AssertFiles(new[] { "1.txt", "2.txt", "3.txt" }, SourceDirectory);
            AssertFiles(new[] { "6.txt", "my.log" }, DestinationDirectory);

            var filesyncer = new FileSyncer();
            filesyncer.Sync(
                new DirectoryInfo(SourceDirectory),
                new DirectoryInfo(DestinationDirectory),
                new DeleteIfNotInSource()); // which stategy to choose (here 6.txt and my.log will be deleted)

            AssertFiles(new[] { "1.txt", "2.txt", "3.txt" }, SourceDirectory);
            AssertFiles(new[] { "1.txt", "2.txt", "3.txt" }, DestinationDirectory);

        }



        [TestMethod]
        public void keep_log_files_in_destination()
        {
            InitDirectories();

            AssertFiles(new[] { "1.txt", "2.txt", "3.txt" }, SourceDirectory);
            AssertFiles(new[] { "6.txt", "my.log" }, DestinationDirectory);

            var s = new FileSyncer();
            s.Sync(
                new DirectoryInfo(SourceDirectory),
                new DirectoryInfo(DestinationDirectory),
                new KeepLogFiles()); // same as "DeleteIfNotInSource" but keep .log-files

            AssertFiles(new[] { "1.txt", "2.txt", "3.txt" }, SourceDirectory);
            AssertFiles(new[] { "1.txt", "2.txt", "3.txt", "my.log" }, DestinationDirectory);

        }

        private void AssertFiles(string[] filenames, string directory)
        {
            var filenamesInDirectory = new DirectoryInfo(directory).GetFiles().Select(x => x.Name).ToArray();
            CollectionAssert.AreEqual(filenames, filenamesInDirectory);
        }

        #region solution

        public interface IDeleteStrategy
        {
            void DeleteExtraFiles(DirectoryInfo source, DirectoryInfo destination);
        }

        public class NoDelete : IDeleteStrategy
        {
            public void DeleteExtraFiles(DirectoryInfo source, DirectoryInfo destination)
            {
                //Do nothing!
            }
        }

        public class KeepLogFiles : DeleteIfNotInSource
        {
            protected override bool ShouldDelete(FileInfo file, DirectoryInfo source)
            {
                // Keep ".log"-files
                if (file.Extension == ".log")
                {
                    return false;
                }

                // ... otherwise let baseclass decide if the file should be kept or not
                return base.ShouldDelete(file, source);
            }

        }

        public class DeleteIfNotInSource : IDeleteStrategy
        {
            public void DeleteExtraFiles(DirectoryInfo source, DirectoryInfo destination)
            {

                foreach (var file in destination.GetFiles())
                {
                    if (ShouldDelete(file, source))
                    {
                        file.Delete();
                    }
                }
            }
            protected virtual bool ShouldDelete(FileInfo file, DirectoryInfo source)
            {
                var sourceFiles = source.GetFiles();
                return !sourceFiles.Select(x => x.Name).Contains(file.Name);
            }
        }


        class FileSyncer
        {
            public void Sync(DirectoryInfo source, DirectoryInfo destination, IDeleteStrategy deleteStrategy)
            {
                CopyFiles(source, destination);

                deleteStrategy.DeleteExtraFiles(source, destination);
            }

            private void CopyFiles(DirectoryInfo source, DirectoryInfo destination)
            {
                foreach (var file in source.GetFiles())
                {
                    file.CopyTo(Path.Combine(destination.FullName, file.Name), true);
                }
            }

        }

        #endregion
    }
}
