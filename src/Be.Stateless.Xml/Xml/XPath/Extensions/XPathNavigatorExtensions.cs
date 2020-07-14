﻿#region Copyright & License

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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using Be.Stateless.IO;

namespace Be.Stateless.Xml.XPath.Extensions
{
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
	public static class XPathNavigatorExtensions
	{
		public static Stream AsStream(this XPathNavigator navigator)
		{
			if (navigator == null) throw new ArgumentNullException(nameof(navigator));
			return new StringStream(navigator.OuterXml);
		}

		public static XmlNamespaceManager GetNamespaceManager(this XPathNavigator navigator)
		{
			if (navigator == null) throw new ArgumentNullException(nameof(navigator));
			var namespaceManager = new XmlNamespaceManager(navigator.NameTable);
			namespaceManager.AddNamespace("xs", XmlSchema.Namespace);
			namespaceManager.AddNamespace("xsi", XmlSchema.InstanceNamespace);
			return namespaceManager;
		}
	}
}
