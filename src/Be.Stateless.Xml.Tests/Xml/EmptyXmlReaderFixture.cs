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
using System.Xml;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.Xml
{
	public class EmptyXmlReaderFixture
	{
		[Fact]
		public void MoveToContent()
		{
			var expected = new XmlDocument().CreateNavigator().ReadSubtree();
			var sut = EmptyXmlReader.Create();

			sut.ReadState.Should().Be(expected.ReadState);
			sut.MoveToContent().Should().Be((expected.MoveToContent()));
			sut.ReadState.Should().Be((expected.ReadState));
		}

		[Fact]
		public void Read()
		{
			var expected = new XmlDocument().CreateNavigator().ReadSubtree();
			var sut = EmptyXmlReader.Create();

			sut.ReadState.Should().Be((expected.ReadState));
			sut.Read().Should().Be((expected.Read()));
			sut.ReadState.Should().Be((expected.ReadState));
		}

		[Fact]
		public void ReadStateEndOfFileBehavior()
		{
			var expected = new XmlDocument().CreateNavigator().ReadSubtree();
			var sut = EmptyXmlReader.Create();

			sut.Read().Should().Be((expected.Read()));

			ValidateExpectedBehavior(sut, expected);
		}

		[Fact]
		public void ReadStateInitialBehavior()
		{
			var expected = new XmlDocument().CreateNavigator().ReadSubtree();
			var sut = EmptyXmlReader.Create();

			ValidateExpectedBehavior(sut, expected);
		}

		[Fact]
		public void ReadToEnd()
		{
			var expected = new XmlDocument().CreateNavigator().ReadSubtree();
			var sut = EmptyXmlReader.Create();
			using (expected)
			using (sut)
			{
				while (expected.Read()) { }

				while (sut.Read()) { }

				ValidateExpectedBehavior(sut, expected);
			}

			ValidateExpectedBehavior(sut, expected);
		}

		private void ValidateExpectedBehavior(XmlReader sut, XmlReader expected)
		{
			sut.ReadState.Should().Be((expected.ReadState));
			sut.AttributeCount.Should().Be((expected.AttributeCount));
			sut.BaseURI.Should().Be((expected.BaseURI));
			sut.Depth.Should().Be((expected.Depth));
			sut.EOF.Should().Be((expected.EOF));
			sut.HasValue.Should().Be((expected.HasValue));
			sut.IsEmptyElement.Should().Be((expected.IsEmptyElement));
			sut.LocalName.Should().Be((expected.LocalName));
			sut.NameTable.Should().NotBeNull();
			sut.NamespaceURI.Should().Be((expected.NamespaceURI));
			sut.NodeType.Should().Be((expected.NodeType));
			sut.Prefix.Should().Be((expected.Prefix));
			sut.Value.Should().Be((expected.Value));

			Action act = expected.Close;
			act.Should().NotThrow();

			act = sut.Close;
			act.Should().NotThrow();

			act = () => expected.GetAttribute(0);
			act.Should().Throw<ArgumentOutOfRangeException>();

			act = () => sut.GetAttribute(0);
			act.Should().Throw<ArgumentOutOfRangeException>();

			sut.GetAttribute("name").Should().Be((expected.GetAttribute("name")));
			sut.GetAttribute("name", "ns").Should().Be((expected.GetAttribute("name", "ns")));
			sut.LookupNamespace("ns").Should().Be((expected.LookupNamespace("ns")));

			act = () => expected.MoveToAttribute(1);
			act.Should().Throw<ArgumentOutOfRangeException>();

			act = () => sut.MoveToAttribute(1);
			act.Should().Throw<ArgumentOutOfRangeException>();

			sut.MoveToAttribute("name").Should().Be((expected.MoveToAttribute("name")));
			sut.MoveToAttribute("name", "ns").Should().Be((expected.MoveToAttribute("name", "ns")));
			sut.MoveToElement().Should().Be((expected.MoveToElement()));
			sut.MoveToFirstAttribute().Should().Be((expected.MoveToFirstAttribute()));
			sut.MoveToNextAttribute().Should().Be((expected.MoveToNextAttribute()));
			sut.ReadAttributeValue().Should().Be((expected.ReadAttributeValue()));

			act = expected.ResolveEntity;
			act.Should().Throw<InvalidOperationException>();

			act = sut.ResolveEntity;
			act.Should().Throw<InvalidOperationException>();
		}
	}
}
