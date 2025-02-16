﻿using System.Diagnostics;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

namespace SkbKontur.TypeScript.ContractGenerator.Tests
{
    public class CliTest
    {
        [Test]
        public void CliGenerated()
        {
            RunCmdCommand($"dotnet {pathToSlnDirectory}/TypeScript.ContractGenerator.Cli/bin/{configuration}/net6.0/SkbKontur.TypeScript.ContractGenerator.Cli.dll " +
                          $"-a {pathToSlnDirectory}/AspNetCoreExample.Generator/bin/{configuration}/net6.0/AspNetCoreExample.Generator.dll " +
                          $"-o {TestContext.CurrentContext.TestDirectory}/cliOutput " +
                          "--nullabilityMode Optimistic " +
                          "--lintMode TsLint " +
                          "--globalNullable true");

            var expectedDirectory = $"{pathToSlnDirectory}/AspNetCoreExample.Generator/output";
            var actualDirectory = $"{TestContext.CurrentContext.TestDirectory}/cliOutput";
            TestBase.CheckDirectoriesEquivalenceInner(expectedDirectory, actualDirectory, generatedOnly : true);
        }

        [Test]
        public void RoslynCliGenerated()
        {
            RunCmdCommand($"dotnet {pathToSlnDirectory}/TypeScript.ContractGenerator.Cli/bin/{configuration}/net6.0/SkbKontur.TypeScript.ContractGenerator.Cli.dll " +
                          $"-d {pathToSlnDirectory}/AspNetCoreExample.Api;{pathToSlnDirectory}/AspNetCoreExample.Generator " +
                          $"-a {typeof(ControllerBase).Assembly.Location} " +
                          $"-o {TestContext.CurrentContext.TestDirectory}/roslynCliOutput " +
                          "--nullabilityMode Optimistic " +
                          "--lintMode TsLint " +
                          "--globalNullable true");

            var expectedDirectory = $"{pathToSlnDirectory}/AspNetCoreExample.Generator/output";
            var actualDirectory = $"{TestContext.CurrentContext.TestDirectory}/roslynCliOutput";
            TestBase.CheckDirectoriesEquivalenceInner(expectedDirectory, actualDirectory, generatedOnly : true);
        }

        private static void RunCmdCommand(string command)
        {
            var process = new Process
                {
                    StartInfo =
                        {
                            FileName = "cmd.exe",
                            WindowStyle = ProcessWindowStyle.Hidden,
                            Arguments = "/C " + command,
                        }
                };
            process.Start();
            process.WaitForExit();
            process.ExitCode.Should().Be(0);
        }

        private static readonly string pathToSlnDirectory = $"{TestContext.CurrentContext.TestDirectory}/../../../../";

#if RELEASE
        const string configuration = "Release";
#elif DEBUG
        const string configuration = "Debug";
#endif
    }
}