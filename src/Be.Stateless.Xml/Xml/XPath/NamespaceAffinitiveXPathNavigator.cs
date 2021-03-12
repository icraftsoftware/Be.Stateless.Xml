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

using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.XPath;

namespace Be.Stateless.Xml.XPath
{
	/// <summary>
	/// <see cref="XPathNavigatorDecorator"/> that propagates the <see cref="NamespaceManager"/> further down the resulting <see cref="XPathNavigator"/>
	/// and <see cref="XPathNodeIterator"/> and consequently allows subsequent XPath queries to be implicitly scoped by the XML
	/// namespaces that are propagated by the <see cref="NamespaceManager"/>.
	/// </summary>
	public class NamespaceAffinitiveXPathNavigator : XPathNavigatorDecorator
	{
		public NamespaceAffinitiveXPathNavigator(XPathNavigator decoratedNavigator, XmlNamespaceManager namespaceManager) : base(decoratedNavigator)
		{
			NamespaceManager = namespaceManager ?? throw new ArgumentNullException(nameof(namespaceManager));
		}

		#region Base Class Member Overrides

		protected override XPathNavigator CreateXPathNavigatorDecorator(XPathNavigator decoratedNavigator)
		{
			return decoratedNavigator == null ? null : new NamespaceAffinitiveXPathNavigator(decoratedNavigator, NamespaceManager);
		}

		public override object Evaluate(string xpath)
		{
			return Evaluate(xpath, NamespaceManager);
		}

		public override XPathNodeIterator Select(string xpath)
		{
			return Select(xpath, NamespaceManager);
		}

		public override XPathNavigator SelectSingleNode(string xpath)
		{
			return SelectSingleNode(xpath, NamespaceManager);
		}

		#endregion

		#region Base Class Member Overrides

		public override bool Matches(string xpath)
		{
			return Matches(XPathExpression.Compile(xpath, NamespaceManager));
		}

		#endregion

		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
		public XmlNamespaceManager NamespaceManager { get; }
	}
}
