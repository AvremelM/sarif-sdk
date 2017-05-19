﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.CodeAnalysis.Sarif.Driver.Sdk
{
    public static class EntryPointUtilities
    {
        internal static string[] GenerateArguments(
            string[] args,
            IFileSystem fileSystem,
            IEnvironmentVariables environmentVariables)
        {
            List<string> expandedArguments = new List<string>();

            foreach (string argument in args)
            {
                if (argument[0] != '@' &&
                    (argument.Length == 1 || argument[1] != '@'))
                {
                    expandedArguments.Add(argument);
                    continue;
                }

                string responseFile = argument.Trim('"').Substring(1);
                try
                {
                    responseFile = environmentVariables.ExpandEnvironmentVariables(responseFile);
                    responseFile = fileSystem.GetFullPath(responseFile);

                    if (!fileSystem.FileExists(responseFile))
                        responseFile = null;
                }
                catch (IOException)
                {
                    responseFile = null;
                }

                if (responseFile == null)
                {
                    Console.WriteLine("!! Could not locate response file specified in argument: " + argument);
                }
                else
                {
                    string[] responseFileLines = fileSystem.ReadAllLines(responseFile);

                    ExpandResponseFile(responseFileLines, expandedArguments);
                }
            }

            return expandedArguments.ToArray();
        }

        private static void ExpandResponseFile(string[] responseFileLines, List<string> expandedArguments)
        {
            // BUG : This function does not allow response files to escape quotes like they can be escaped
            // on the command line. Suggest following CommandLineToArgVW rules here:
            // http://msdn.microsoft.com/en-us/library/windows/desktop/bb776391.aspx
            // 
            // * 2n backslashes followed by a quotation mark produce n backslashes followed by a quotation mark.
            // * (2n) + 1 backslashes followed by a quotation mark again produce n backslashes followed by a quotation mark.
            // * n backslashes not followed by a quotation mark simply produce n backslashes.
            //
            foreach (string responseFileLine in responseFileLines)
            {
                string line = responseFileLine.Trim();
                int argumentStart = 0;
                bool insideQuote = false;
                for (int i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case ' ':

                        if (!insideQuote)
                        {
                            expandedArguments.Add(line.Substring(argumentStart, i - argumentStart).Trim('"'));
                            argumentStart = i + 1;
                        }
                        break;

                        case '"':
                        if (insideQuote)
                        {
                            insideQuote = false;
                        }
                        else
                        {
                            insideQuote = true;
                        }
                        break;

                        default:
                        break;
                    }

                    if (i == line.Length - 1)
                    {
                        string argument = line.Substring(argumentStart, i - argumentStart + 1);
                        if (argument.StartsWith("\""))
                            argument = argument.Trim('\"');
                        expandedArguments.Add(argument);
                    }
                }
            }
        }
    }
}
