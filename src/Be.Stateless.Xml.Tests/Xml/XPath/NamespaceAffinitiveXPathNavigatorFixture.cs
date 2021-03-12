#region Copyright & License

// Copyright © 2012 - 2021 François Chabot
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

using System.Xml;
using Be.Stateless.Xml.Extensions;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.Xml.XPath
{
	public class NamespaceAffinitiveXPathNavigatorFixture
	{
		[Fact]
		public void XPathNavigatorPropagatesNamespaceManagerThroughXPathNavigator()
		{
			var document = new XmlDocument();
			document.LoadXml(CONTENT);
			var navigator = document.CreateNamespaceAffinitiveXPathNavigator();
			navigator.NamespaceManager.AddNamespace("s", "urn");

			var node = navigator.SelectSingleNode("s:root");
			node.Should().NotBeNull();
			node = node!.SelectSingleNode("s:one/s:two/text()");
			node.Should().NotBeNull();
			node!.Value.Should().Be("2");
		}

		[Fact]
		public void XPathNavigatorPropagatesNamespaceManagerThroughXPathNodeIterator()
		{
			var document = new XmlDocument();
			document.LoadXml(CONTENT);
			var navigator = document.CreateNamespaceAffinitiveXPathNavigator();
			navigator.NamespaceManager.AddNamespace("s", "urn");

			var iterator = navigator.Select("s:root");
			iterator.Should().NotBeNull();
			iterator = iterator.Current.Select("s:one/s:two/text()");
			iterator.Should().NotBeNull();
			iterator.Current.Value.Should().Be("2");
		}

		[Fact]
		public void XPathNavigatorResultingOfSelectIsNullWhenNodeIsNotFound()
		{
			var document = new XmlDocument();
			document.LoadXml(CONTENT);
			var navigator = document.CreateNamespaceAffinitiveXPathNavigator();
			navigator.NamespaceManager.AddNamespace("s", "urn");

			var iterator = navigator.SelectSingleNode("s:root");
			iterator!.SelectSingleNode("s:six").Should().BeNull();
		}

		[Fact]
		public void XPathNodeIteratorResultingOfSelectIsEmptyWhenNodeIsNotFound()
		{
			var document = new XmlDocument();
			document.LoadXml(CONTENT);
			var navigator = document.CreateNamespaceAffinitiveXPathNavigator();
			navigator.NamespaceManager.AddNamespace("s", "urn");

			var iterator = navigator.SelectSingleNode("s:root");
			iterator!.Select("s:six").Should().BeEmpty();
		}

		private const string CONTENT = "<root xmlns='urn'><one><two>2</two></one></root>";
	}
}
