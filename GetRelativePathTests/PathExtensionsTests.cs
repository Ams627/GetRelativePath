using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetRelativePath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.IO;

namespace GetRelativePath.Tests
{
    [TestClass()]
    public class PathExtensionsTests
    {
        [TestMethod()]
        public void GetRelativePathTestNoCommonRoot()
        {
            var path1 = @"d:\temp";
            var path2 = @"c:\temp\dog\cat\hamster";

            var relativePath = PathExtensions.GetRelativePath(path1, path2);
            relativePath.Should().Be(path2);
        }


        [TestMethod()]
        public void GetRelativePathTestStandardTests()
        {
            var path3 = @"c:\temp\goat\elephant";
            var path4 = @"c:\temp\goat\elephant\sheep\monkey";
            var relativePath = PathExtensions.GetRelativePath(path3, path4);
            var expected = MakeExpectedPath("sheep", "monkey");
            relativePath.Should().Be(expected);


            var path5 = @"c:\temp\goat/elephant\";
            var path6 = @"c:\temp\goat\elephant\sheep\monkey";
            relativePath = PathExtensions.GetRelativePath(path5, path6);
            relativePath.Should().Be(expected);
        }

        [TestMethod()]
        public void GetRelativePathTestAltPathSeparator()
        {
            var path3 = @"c:\temp/goat\elephant/bear";
            var path4 = @"c:/temp\goat/elephant\bear\sheep\monkey";
            var path3a = @"c:/temp\goat/";
            var relativePath = PathExtensions.GetRelativePath(path3, path4);

            var expected = MakeExpectedPath("sheep", "monkey");
            relativePath.Should().Be(expected);

            relativePath = PathExtensions.GetRelativePath(path3a, path4);
            expected = MakeExpectedPath("elephant", "bear", "sheep", "monkey");
            relativePath.Should().Be(expected);
        }

        /// <summary>
        /// test paths with no drive at the front:
        /// </summary>
        [TestMethod()]
        public void GetRelativePathNoDrive()
        {
            var path1 = @"/a/b/c/d/e/f/g/";
            var path2 = @"/a/b/c/d/e/f/g/h/i/j/k/l/m/n/o/p/";
            var relativePath = PathExtensions.GetRelativePath(path1, path2);

            var expectedPath = string.Join($"{Path.DirectorySeparatorChar}", "hijklmnop".Select(x => x));
            relativePath.Should().Be(expectedPath);
        }


        [TestMethod()]
        public void GetRelativePathNotRelative()
        {
            var path3 = @"c:\temp\goat\elephant\zebra";
            var path4 = @"c:\temp\goat\elephant\sheep\monkey";
            var relativePath = PathExtensions.GetRelativePath(path3, path4);
            relativePath.Should().Be(path4);
        }


        private string MakeExpectedPath(params string[] v)
        {
            return string.Join($"{Path.DirectorySeparatorChar}", v);
        }

        [TestMethod()]
        public void GetBashPath()
        {
            var path1 = @"d:\temp";
            var path2 = @"c:\temp\dog\cat\hamster";

            var bash1 = PathExtensions.GetBashPath(path1);
            bash1.Should().Be("/d/temp");
            var bash2 = PathExtensions.GetBashPath(path2);
            bash2.Should().Be("/c/temp/dog/cat/hamster");
        }
    }
}