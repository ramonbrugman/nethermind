//  Copyright (c) 2018 Demerzel Solutions Limited
//  This file is part of the Nethermind library.
// 
//  The Nethermind library is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  The Nethermind library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Nethermind.Logging
{
    public static class PathUtils
    {
        public static string AssemblyDirectory { get; }
        
        public static string MainModuleDirectory { get; }
        
        public static string ExecutingDirectory { get; }

        static PathUtils()
        {
            var process = Process.GetCurrentProcess();
            AssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            MainModuleDirectory = Path.GetDirectoryName(process.MainModule.FileName);
            ExecutingDirectory = process.ProcessName.StartsWith("dotnet", StringComparison.InvariantCultureIgnoreCase) ? AssemblyDirectory : MainModuleDirectory;
        }

        public static string GetApplicationResourcePath(this string resourcePath, string overridePrefixPath = null, PathType type = PathType.Default)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                resourcePath = string.Empty;
            }
            
            if (Path.IsPathRooted(resourcePath))
            {
                return resourcePath;
            }

            var executingDirectory = GetExecutingDirectory(type);
            
            if (string.IsNullOrEmpty(overridePrefixPath))
            {
                return Path.Combine(executingDirectory, resourcePath);
            }

            return Path.IsPathRooted(overridePrefixPath)
                ? Path.Combine(overridePrefixPath, resourcePath)
                : Path.Combine(executingDirectory, overridePrefixPath, resourcePath);
        }

        private static string GetExecutingDirectory(PathType type)
        {
            switch (type)
            {
                case PathType.Assembly:
                    return AssemblyDirectory;
                case PathType.MainModule:
                    return MainModuleDirectory;
                default:
                    return ExecutingDirectory;
            }
        }

        public enum PathType
        {
            Default,
            Assembly,
            MainModule
        }
    }
}
