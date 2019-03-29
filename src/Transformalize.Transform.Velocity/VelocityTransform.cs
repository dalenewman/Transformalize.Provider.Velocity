#region license
// Transformalize
// Configurable Extract, Transform, and Load
// Copyright 2013-2019 Dale Newman
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//   
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cfg.Net.Contracts;
using Cfg.Net.Loggers;
using NVelocity;
using Transformalize.Configuration;
using Transformalize.Contracts;
using Transformalize.Extensions;
using Transformalize.Providers.Velocity;

namespace Transformalize.Transforms.Velocity {

    public class VelocityTransform : BaseTransform {

        private readonly Field[] _input;
        private readonly string _templateName;

        public VelocityTransform(IContext context = null, IReader reader = null) : base(context, "object") {

            if (IsMissingContext()) {
                return;
            }

            Returns = Context.Field.Type;

            if (IsMissing(Context.Operation.Template)) {
                return;
            }

            VelocityInitializer.Init();

            var fileBasedTemplate = Context.Process.Templates.FirstOrDefault(t => t.Name == Context.Operation.Template);

            if (fileBasedTemplate != null) {
                var memoryLogger = new MemoryLogger();
                Context.Operation.Template = reader.Read(fileBasedTemplate.File, null, memoryLogger);
                if (memoryLogger.Errors().Any()) {
                    foreach (var error in memoryLogger.Errors()) {
                        Context.Error(error);
                    }
                }
            }

            _input = MultipleInput();
            _templateName = Context.Field.Alias + " Template";
        }

        public override IRow Operate(IRow row) {

            var context = new VelocityContext();
            foreach (var field in _input) {
                context.Put(field.Alias, row[field]);
            }

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb)) {
                NVelocity.App.Velocity.Evaluate(context, sw, _templateName, Context.Operation.Template);
                sw.Flush();
            }

            sb.Trim(' ', '\n', '\r');
            row[Context.Field] = Context.Field.Convert(sb.ToString());

            
            return row;
        }

        public override IEnumerable<OperationSignature> GetSignatures() {
            yield return new OperationSignature("velocity") {
                Parameters = new List<OperationParameter>(2) {
                    new OperationParameter("template"),
                    new OperationParameter("content-type","raw")
                }
            };
        }

    }
}