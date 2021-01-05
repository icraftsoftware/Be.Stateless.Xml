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

using System.Text;
using System.Xml;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.Text.Extensions
{
	public class StringBuilderXmlExtensionsFixture
	{
		[Fact]
		public void GetReaderAtContent()
		{
			using (var xmlReader = new StringBuilder("<part-one><child-one>one</child-one></part-one>").GetReaderAtContent())
			{
				xmlReader.ReadState.Should().Be(ReadState.Interactive);
				xmlReader.NodeType.Should().Be(XmlNodeType.Element);
				xmlReader.LocalName.Should().Be("part-one");
			}
		}

		[Fact]
		public void GetReaderAtEmptyContent()
		{
			using (var xmlReader = new StringBuilder().GetReaderAtContent())
			{
				xmlReader.ReadState.Should().Be(ReadState.EndOfFile);
				xmlReader.NodeType.Should().Be(XmlNodeType.None);
				xmlReader.LocalName.Should().BeEmpty();
			}
		}

		[Fact]
		public void GetReaderAtPseudoEmptyContent()
		{
			using (var xmlReader = new StringBuilder("   \r\n    \r\n  ").GetReaderAtContent())
			{
				xmlReader.ReadState.Should().Be(ReadState.EndOfFile);
				xmlReader.NodeType.Should().Be(XmlNodeType.None);
				xmlReader.LocalName.Should().BeEmpty();
			}
		}
	}
}
