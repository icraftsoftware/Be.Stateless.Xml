#region Copyright & License

// Copyright © 2012 - 2020 François Chabot
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml;
using Be.Stateless.Linq.Extensions;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.Xml.Xsl
{
	public class XsltArgumentListFixture
	{
		[Fact]
		public void Clone()
		{
			var arguments = new XsltArgumentList();
			_extensions.ForEach(e => arguments.AddExtensionObject(e.Key, e.Value));
			_params.ForEach(p => arguments.AddParam(p.Key.Name, p.Key.Namespace, p.Value));

			var copy = arguments.Clone();
			_extensions.ForEach(e => copy.GetExtensionObject(e.Key).Should().BeSameAs(e.Value));
			_params.ForEach(p => copy.GetParam(p.Key.Name, p.Key.Namespace).Should().BeSameAs(p.Value));
		}

		[Fact]
		public void CopyConstructor()
		{
			var arguments = new System.Xml.Xsl.XsltArgumentList();
			_extensions.ForEach(e => arguments.AddExtensionObject(e.Key, e.Value));
			_params.ForEach(p => arguments.AddParam(p.Key.Name, p.Key.Namespace, p.Value));

			var copy = new XsltArgumentList(arguments);
			_extensions.ForEach(e => copy.GetExtensionObject(e.Key).Should().BeSameAs(e.Value));
			_params.ForEach(p => copy.GetParam(p.Key.Name, p.Key.Namespace).Should().BeSameAs(p.Value));
		}

		[Fact]
		public void Initializer()
		{
			var arguments = new XsltArgumentList { new XsltArgument("param", "value") };
			arguments.GetParam("param", string.Empty).Should().NotBeNull();
			arguments.GetParam("param", string.Empty).Should().Be("value");
		}

		[Fact]
		public void Union()
		{
			var arguments = new XsltArgumentList();
			_extensions.ForEach(e => arguments.AddExtensionObject(e.Key, e.Value));
			_params.ForEach(p => arguments.AddParam(p.Key.Name, p.Key.Namespace, p.Value));

			_extensions.Add("urn:extensions:six", new object());
			_params.Add(new XmlQualifiedName("p4", "urn:parameters"), new object());

			var newArguments = new System.Xml.Xsl.XsltArgumentList();
			newArguments.AddExtensionObject(_extensions.Last().Key, _extensions.Last().Value);
			newArguments.AddParam(_params.Last().Key.Name, _params.Last().Key.Namespace, _params.Last().Value);

			var union = arguments.Union(newArguments);

			_extensions.ForEach(e => union.GetExtensionObject(e.Key).Should().BeSameAs(e.Value));
			_params.ForEach(p => union.GetParam(p.Key.Name, p.Key.Namespace).Should().BeSameAs(p.Value));
		}

		[Fact]
		[SuppressMessage("ReSharper", "ImplicitlyCapturedClosure")]
		public void UnionThrowsWhenDuplicate()
		{
			var arguments = new XsltArgumentList();
			_extensions.ForEach(e => arguments.AddExtensionObject(e.Key, e.Value));
			_params.ForEach(p => arguments.AddParam(p.Key.Name, p.Key.Namespace, p.Value));

			var union1 = new System.Xml.Xsl.XsltArgumentList();
			union1.AddExtensionObject(_extensions.First().Key, _extensions.First().Value);

			Action act = () => arguments.Union(union1);
			act.Should().Throw<ArgumentException>();

			var union2 = new System.Xml.Xsl.XsltArgumentList();
			union2.AddParam(_params.First().Key.Name, _params.First().Key.Namespace, _params.First().Value);

			act = () => arguments.Union(union2);
			act.Should().Throw<ArgumentException>();
		}

		[Fact]
		public void UnionWithNull()
		{
			var lhs = new XsltArgumentList();
			lhs.AddExtensionObject("urn:extensions:one", new object());
			lhs.AddParam("p1", "urn:parameters", new object());

			var union = lhs.Union((System.Xml.Xsl.XsltArgumentList) null);

			union.Should().NotBeSameAs(lhs);
			union.GetExtensionObject("urn:extensions:one").Should().BeSameAs(lhs.GetExtensionObject("urn:extensions:one"));
			union.GetParam("p1", "urn:parameters").Should().BeSameAs(lhs.GetParam("p1", "urn:parameters"));
		}

		[Fact]
		public void UnionYieldsNewInstance()
		{
			var lhs = new XsltArgumentList();
			lhs.AddExtensionObject("urn:extensions:one", new object());
			lhs.AddParam("p1", "urn:parameters", new object());

			var rhs = new XsltArgumentList();
			rhs.AddExtensionObject("urn:extensions:two", new object());
			rhs.AddParam("p2", "urn:parameters", new object());

			var union = lhs.Union(rhs);

			union.Should().NotBeSameAs(lhs);
			union.Should().NotBeSameAs(rhs);
		}

		public XsltArgumentListFixture()
		{
			_extensions = new Dictionary<string, object> {
				{ "urn:extensions:one", new object() },
				{ "urn:extensions:two", new object() },
				{ "urn:extensions:ten", new object() }
			};
			_params = new Dictionary<XmlQualifiedName, object> {
				{ new XmlQualifiedName("p1", "urn:parameters"), new object() },
				{ new XmlQualifiedName("p2", "urn:parameters"), new object() },
				{ new XmlQualifiedName("p3", "urn:parameters"), new object() }
			};
		}

		private readonly Dictionary<string, object> _extensions;
		private readonly Dictionary<XmlQualifiedName, object> _params;
	}
}
