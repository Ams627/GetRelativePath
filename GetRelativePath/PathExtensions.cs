using System.IO;
using System.Linq;

namespace GetRelativePath
{
    public class PathExtensions
    {
        public static string GetRelativePath(string relativeTo, string path)
        {
            var relativeFull = Path.GetFullPath(relativeTo);
            var pathFull = Path.GetFullPath(path);

            var rootRelative = Path.GetPathRoot(relativeFull);
            var rootFull = Path.GetPathRoot(pathFull);

            // paths do not share a common root:
            if (rootRelative != rootFull)
            {
                return path;
            }

            var splitRelative = relativeFull.Split(Path.DirectorySeparatorChar).Where(x => x != string.Empty).ToArray();
            var splitPath = pathFull.Split(Path.DirectorySeparatorChar).Where(x => x != string.Empty).ToArray();

            int firstNonCommon = 0;
            for (int i = 0; i < splitPath.Length; i++)
            {
                bool segmentEqual = true;
                if (i < splitRelative.Length)
                {
                    segmentEqual = splitRelative[i] == splitPath[i];
                    if (segmentEqual)
                    {
                        continue;
                    }
                }
                if (!segmentEqual)
                {
                    return pathFull;
                }
                firstNonCommon = i;
                break;
            }

            var result = Path.Combine(splitPath.Skip(firstNonCommon).ToArray());
            return result;
        }


        public static string GetBashPath(string path)
        {
            var splitPath = path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });
            if (!splitPath.Any())
            {
                return path;
            }

            if (splitPath[0].Length == 2 && splitPath[0][1] == ':' && char.IsLetter(splitPath[0][0]))
            {
                return string.Join("/", new[] { $"/{char.ToLower(splitPath[0][0])}" }.Concat(splitPath.Skip(1)));
            }
            return string.Join("/", splitPath);
        }
    }
}
