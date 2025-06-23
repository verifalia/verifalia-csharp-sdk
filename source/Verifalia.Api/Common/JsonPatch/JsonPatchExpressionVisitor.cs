/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2024 Cobisi Research
*
* Cobisi Research
* Via Della Costituzione, 31
* 35010 Vigonza
* Italy - European Union
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;

namespace Verifalia.Api.Common.JsonPatch
{
    internal sealed class JsonPatchExpressionVisitor : ExpressionVisitor
    {
        private readonly List<Operation> _operations = [];

        public Operation[] BuildJsonPatchDocument<T>(Expression<Func<T, T>> lambda)
        {
            if (lambda.Body is MemberInitExpression init)
            {
                VisitMemberInit(init, String.Empty);
            }
            else
            {
                throw new NotSupportedException("Only simple MemberInitExpressions are supported");
            }

            return _operations.ToArray();
        }

        private void VisitMemberInit(MemberInitExpression init, string pathPrefix)
        {
            foreach (var binding in init.Bindings.OfType<MemberAssignment>())
            {
                // derive the JSON name (honoring [JsonProperty])
                var jsonName = GetJsonPropertyName(binding.Member);
                var currentPath = $"{pathPrefix}/{jsonName}";

                switch (binding.Expression)
                {
                    case ConstantExpression c:
                        // simple literal
                        _operations.Add(new Operation(OperationType.Replace, currentPath, c.Value));
                        break;

                    case MemberInitExpression childInit:
                        // nested object → recurse
                        VisitMemberInit(childInit, currentPath);
                        break;

                    default:
                        // any other expression → compile & evaluate
                        var lambda = Expression.Lambda(binding.Expression);
                        var value = lambda.Compile().DynamicInvoke();
                        _operations.Add(new Operation(OperationType.Replace, currentPath, value));
                        break;
                }
            }
        }

        private string GetJsonPropertyName(MemberInfo mi)
        {
            var attr = mi.GetCustomAttributes(typeof(JsonPropertyAttribute), true)
                .Cast<JsonPropertyAttribute>()
                .FirstOrDefault();
            
            return attr?.PropertyName ?? mi.Name;
        }
    }
}