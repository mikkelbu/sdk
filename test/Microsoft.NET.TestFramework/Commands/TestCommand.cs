// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.Cli.Utils;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Microsoft.NET.TestFramework.Commands
{
    public abstract class TestCommand
    {
        private Dictionary<string, string> _environment = new Dictionary<string, string>();

        public ITestOutputHelper Log { get; }

        protected TestCommand(ITestOutputHelper log)
        {
            Log = log;
        }

        protected abstract ICommand CreateCommand(string[] args);

        public TestCommand WithEnvironmentVariable(string name, string value)
        {
            _environment[name] = value;
            return this;
        }

        public CommandResult Execute(params string[] args)
        {
            var command = CreateCommand(args)
                .CaptureStdOut()
                .CaptureStdErr();

            foreach (var variable in _environment)
            {
                command.EnvironmentVariable(variable.Key, variable.Value);
            }

            var result = command.Execute();

            Log.WriteLine($"> {result.StartInfo.FileName} {result.StartInfo.Arguments}");
            Log.WriteLine(result.StdOut);

            if (!string.IsNullOrEmpty(result.StdErr))
            {
                Log.WriteLine("");
                Log.WriteLine("StdErr:");
                Log.WriteLine(result.StdErr);
            }

            if (result.ExitCode != 0)
            {
                Log.WriteLine($"Exit Code: {result.ExitCode}");
            }

            return result;
        }
    }
}