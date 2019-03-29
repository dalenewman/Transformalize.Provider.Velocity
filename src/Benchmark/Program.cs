using Autofac;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using Transformalize.Configuration;
using Transformalize.Containers.Autofac;
using Transformalize.Contracts;
using Transformalize.Providers.Bogus.Autofac;
using Transformalize.Transforms.Velocity.Autofac;
using NullLogger = Transformalize.Logging.NullLogger;

namespace Benchmark {


   [LegacyJitX64Job]
   public class Benchmarks {

      [Benchmark(Baseline = true, Description = "10000 test rows")]
      public void TestRows() {
         var logger = new NullLogger();
         using (var outer = new ConfigurationContainer().CreateScope(@"files\bogus.xml?Size=10000", logger)) {
            var process = outer.Resolve<Process>();
            using (var inner = new TestContainer(new BogusModule()).CreateScope(process, logger)) {
               var controller = inner.Resolve<IProcessController>();
               controller.Execute();
            }
         }
      }

      [Benchmark(Baseline = false, Description = "10000 rows with 1 Velocity")]
      public void CSharpRows() {
         var logger = new NullLogger();
         using (var outer = new ConfigurationContainer(new VelocityTransformModule()).CreateScope(@"files\bogus-with-transform.xml?Size=10000", logger)) {
            var process = outer.Resolve<Process>();
            using (var inner = new TestContainer(new VelocityTransformModule(), new BogusModule()).CreateScope(process, logger)) {
               var controller = inner.Resolve<IProcessController>();
               controller.Execute();
            }
         }
      }

   }

   public class Program {
      private static void Main(string[] args) {
         var summary = BenchmarkRunner.Run<Benchmarks>();
         Console.WriteLine(summary);
      }
   }
}
