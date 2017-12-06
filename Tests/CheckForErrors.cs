﻿using System.Collections.Generic;
using System.IO;
using Mono.Cecil;
using NUnit.Framework;

[TestFixture]
public class CheckForErrors
{
    [Test]
    public void VerifyRefError()
    {
        var errors = new List<string>();
        var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "AssemblyToProcessWithErrors.dll");
        var assemblyPath = Path.GetFullPath(path);

        var parameters = new ReaderParameters
        {
            //SymbolStream = symbolStream,
            ReadSymbols = true
        };
        using (var moduleDefinition = ModuleDefinition.ReadModule(assemblyPath, parameters))
        {
            var weavingTask = new ModuleWeaver
            {
                ModuleDefinition = moduleDefinition,
                LogError = s => errors.Add(s)
            };
            weavingTask.Execute();
            Assert.Contains("Method 'ClassUsingOutParam.Method' uses member 'ClassWithField.Member' as a 'ref' or 'out' parameter. This is not supported by Fielder. Please convert this field to a property manually.", errors);
            Assert.Contains("Method 'ClassUsingRefParam.Method' uses member 'ClassWithField.Member' as a 'ref' or 'out' parameter. This is not supported by Fielder. Please convert this field to a property manually.", errors);
            Assert.AreEqual(2, errors.Count);
        }
    }
}