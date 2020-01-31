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

namespace Be.Stateless.Xml.Xsl
{
	public class XsltArgument
	{
		public XsltArgument(string name, object value) : this(name, string.Empty, value) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1054:Uri parameters should not be strings")]
		public XsltArgument(string name, string namespaceUri, object value)
		{
			Name = name;
			NamespaceUri = namespaceUri;
			Value = value;
		}

		public string Name { get; private set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:Uri properties should not be strings")]
		public string NamespaceUri { get; private set; }

		public object Value { get; private set; }
	}
}
